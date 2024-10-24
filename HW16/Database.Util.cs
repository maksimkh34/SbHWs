using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HW16;

public static partial class Database
{
    public abstract class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static bool operator true(OperationResult result)
        {
            return result.Success;
        }

        public static bool operator false(OperationResult result)
        {
            return !result.Success;
        }
    }

    private class ConnectionInfo<T>(T connection)
        where T : DbConnection
    {
        public T Connection { get; } = connection;
    }

    public class UpdateOperationResult : OperationResult
    {
        public UpdateErrorCode? ErrorCode { get; set; }
        public bool MultipleRowsAffected { get; set; }
    }

    public class DeleteOperationResult : OperationResult
    {
        public DeleteErrorCode? ErrorCode { get; set; }
        public bool MultipleRowsAffected { get; set; }
    }

    private static bool IsValidValue(object? value, PropertyInfo propertyInfo)
    {
        return true;
    }
    
    private static string BuildSqlCondition<TObject>(Expression<Func<TObject, bool>> filter, out List<object> parameters)
    {
        var visitor = new SqlExpressionVisitor();
        visitor.Visit(filter);
        parameters = visitor.Parameters;
        return visitor.Condition;
    }
    
    private class SqlExpressionVisitor : ExpressionVisitor
    {
        private readonly StringBuilder _condition = new();
        public List<object> Parameters { get; } = [];

        public string Condition => _condition.ToString();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _condition.Append('(');
            Visit(node.Left);
            _condition.Append($" {GetSqlOperator(node.NodeType)} ");
            Visit(node.Right);
            _condition.Append(')');
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _condition.Append(node.Member.Name);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _condition.Append($"@param{Parameters.Count}");
            if (node.Value != null) Parameters.Add(node.Value);
            return node;
        }

        private static string GetSqlOperator(ExpressionType type) => type switch
        {
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            _ => throw new NotSupportedException($"Unsupported operator: {type}")
        };
    }



    public class SelectOperationResult<T> : OperationResult
    {
        public List<T> Data { get; set; } = [];
        public SelectErrorCode? ErrorCode { get; set; }
    }

    public enum DeleteErrorCode
    {
        RecordNotFound,
        ConcurrencyViolation,
        ValidationError,
        DatabaseError
    }

    public enum UpdateErrorCode
    {
        RecordNotFound,
        ConcurrencyViolation,
        ValidationError,
        DatabaseError
    }

    public enum SelectErrorCode
    {
        RecordNotFound,
        ValidationError,
        DatabaseError,
        TimeoutError
    }

    public class InsertOperationResult : OperationResult;

    public enum Tables
    {
        ProductSales,
        Clients
    }
}