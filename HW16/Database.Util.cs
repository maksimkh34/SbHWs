using System.Data.Common;
using System.Reflection;

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