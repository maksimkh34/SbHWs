using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace HW16;

public static class Database
{
    public static SqlConnection LocalConnection { get; set; } = null!;
    public static OleDbConnection OleDbConnection { get; set; } = null!;

    public static async Task Initialize()
    {
        var localConTask = Task.Factory.StartNew(() =>
        {
            var localResult = ConnectLocal();
            LocalConnection = localResult.Connection;
        });
        
        var oleConTask = Task.Factory.StartNew(() =>
        {
            var accessResult = ConnectAccess();
            OleDbConnection = accessResult.Connection;
        });
        
        await Task.WhenAll(localConTask, oleConTask);

        // var strClients = Database.Select<SqlConnection, SqlCommand>(LocalConnection!, "Clients");
        // var clients = Util.Parse<Client>(strClients);
        // var strSales = Database.Select<OleDbConnection, OleDbCommand>(OleDbConnection!, "ProductSales");
        // var sales = Util.Parse<ProductSaleEntry>(strSales);
    }
    
    
    private static ConnectionInfo<SqlConnection> ConnectLocal()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "(localdb)\\SkillBoxDBLecture",
            InitialCatalog = "SkillBoxDBLecture",
            IntegratedSecurity = true,
            Pooling = false,
            Encrypt = false,
            UserID = "admin",
            Password = "pwd"
        };
        var localConnection = new SqlConnection(builder.ConnectionString);
        localConnection.Open();
        
        return new ConnectionInfo<SqlConnection>(localConnection);
    }

    private class ConnectionInfo<T>(T connection)
        where T : DbConnection
    {
        public T Connection { get; } = connection;
    }
    

    private static ConnectionInfo<OleDbConnection> ConnectAccess()
    {
        var dbConnectionStringBuilder = new OleDbConnectionStringBuilder
        {
            Provider = "Microsoft.ACE.OLEDB.12.0",
            DataSource = "D:\\TestDb.accdb"
        };
        var oleConnection = new OleDbConnection(dbConnectionStringBuilder.ConnectionString);
        oleConnection.Open();
        
        return new ConnectionInfo<OleDbConnection>(oleConnection);
    }
    
    public static List<TObject> Select<TObject>() where TObject : ICanBeInsertedToDatabase, new()
    {
        var objectToSelect = new TObject();
        var (conn, commandType) = Util.GetDbTypes(objectToSelect);
        var command = (DbCommand)Activator.CreateInstance(commandType)!;

        // Получаем список колонок, исключая "Table"
        var columns = objectToSelect.GetType().GetProperties()
            .Where(p => p.Name != "Table")
            .Aggregate("", (current, propertyInfo) => current + propertyInfo.Name + ", ")
            .TrimEnd(',', ' ');

        var sql = $"SELECT {columns} FROM {objectToSelect.Table};";
        command.CommandText = sql;
        command.Connection = conn;

        using var reader = command.ExecuteReader();

        var results = new List<TObject>();
        while (reader.Read())
        {
            var obj = new TObject();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.Name == "Table") continue;

                var value = reader[propertyInfo.Name];
                if (value == DBNull.Value) continue;
                var propertyType = propertyInfo.PropertyType;
                var convertedValue = Convert.ChangeType(value, propertyType);
                if(convertedValue is string stringValue) convertedValue = stringValue.TrimEnd();
                propertyInfo.SetValue(obj, convertedValue);
            }
            results.Add(obj);
        }
        return results;
    }



    public static void Insert(ICanBeInsertedToDatabase objectToInsert)
    {
        var (conn, commandType) = Util.GetDbTypes(objectToInsert);
        var command = (DbCommand)Activator.CreateInstance(commandType)!;

        var columns = objectToInsert.GetType().GetProperties().Aggregate("", (current, propertyInfo) => current + propertyInfo.Name + ", ").TrimEnd(',', ' ');
        if (columns.StartsWith("Id, ")) columns = columns[4..];
        columns = columns.Replace(", Table", "");
        var values = objectToInsert.GetType().GetProperties().Where(p => p.Name != "Id" && p.Name != "Table")
            .Select((_, index) => $"@param{index}").Aggregate("", (current, param) => current + param + ", ").TrimEnd(',', ' ');

        var sql = $"INSERT INTO {objectToInsert.Table} ({columns}) VALUES ({values});";

        command.CommandText = sql;
        command.Connection = conn;

        var parameterIndex = 0;
        foreach (var propertyInfo in objectToInsert.GetType().GetProperties())
        {
            if (propertyInfo.Name is "Id" or "Table") continue;
            var value = propertyInfo.GetValue(objectToInsert);
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@param{parameterIndex++}";
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        if (command == null)
            throw new InvalidOperationException("Command creation failed.");

        command.ExecuteNonQuery();
    }




    public enum Tables
    {
        ProductSales,
        Clients
    }
}