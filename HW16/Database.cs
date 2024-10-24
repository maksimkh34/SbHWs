using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace HW16;

public static partial class Database
{
    public static SqlConnection LocalConnection { get; private set; } = new();
    public static OleDbConnection OleDbConnection { get; private set; } = new();

    private static bool Loaded => LocalConnection.State == ConnectionState.Open && 
                                  OleDbConnection.State == ConnectionState.Open;

    public static void CheckInitializeSync()
    {
        if (Loaded) return;
        var localResult = ConnectLocal();
        LocalConnection = localResult.Connection;
        var accessResult = ConnectAccess();
        OleDbConnection = accessResult.Connection;
    }

    public static async Task Initialize()
    {
        if (Loaded) return;
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

    public static async Task<UpdateOperationResult> UpdateAsync(ICanBeInsertedToDatabase objectToUpdate)
    {
        var conn = objectToUpdate.GetConnection();
        var commandType = objectToUpdate.GetCommandType();
        var command = (DbCommand)Activator.CreateInstance(commandType)!;

        var columns = objectToUpdate.GetType().GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "Table")
            .Select((propertyInfo, index) => $"{propertyInfo.Name} = @param{index}")
            .Aggregate("", (current, param) => current + param + ", ")
            .TrimEnd(',', ' ');

        var sql = $"UPDATE {objectToUpdate.Table} SET {columns} WHERE Id = @Id;";
        command.CommandText = sql;
        command.Connection = conn;

        var parameterIndex = 0;
        foreach (var propertyInfo in objectToUpdate.GetType().GetProperties())
        {
            if (propertyInfo.Name == "Table") continue;
            var value = propertyInfo.GetValue(objectToUpdate);
            if (propertyInfo.Name != "Id" && !IsValidValue(value, propertyInfo))
            {
                return new UpdateOperationResult
                {
                    Success = false,
                    Message = $"Invalid value for {propertyInfo.Name}",
                    ErrorCode = UpdateErrorCode.ValidationError
                };
            }
            var parameter = command.CreateParameter();
            parameter.ParameterName = propertyInfo.Name == "Id" ? "@Id" : $"@param{parameterIndex++}";
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        try
        {
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected switch
            {
                0 => new UpdateOperationResult
                {
                    Success = false,
                    Message = "Record not found.",
                    ErrorCode = UpdateErrorCode.RecordNotFound
                },
                1 => new UpdateOperationResult { Success = true, Message = null, ErrorCode = null },
                _ => new UpdateOperationResult
                {
                    Success = true,
                    Message = null,
                    ErrorCode = null,
                    MultipleRowsAffected = true
                }
            };
        }
        catch (DBConcurrencyException)
        {
            return new UpdateOperationResult
            {
                Success = false,
                Message = "Concurrency violation.",
                ErrorCode = UpdateErrorCode.ConcurrencyViolation
            };
        }
        catch (Exception ex)
        {
            return new UpdateOperationResult
            {
                Success = false,
                Message = $"Update failed: {ex.Message}",
                ErrorCode = UpdateErrorCode.DatabaseError
            };
        }
    }
    
    public static async Task<DeleteOperationResult> DeleteAsync(ICanBeInsertedToDatabase objectToDelete)
    {
        var conn = objectToDelete.GetConnection();
        var commandType = objectToDelete.GetCommandType();
        var command = (DbCommand)Activator.CreateInstance(commandType)!;

        var conditions = objectToDelete.GetType().GetProperties()
            .Where(p => p.Name != "Table")
            .Select((propertyInfo, index) => $"{propertyInfo.Name} = @param{index}")
            .Aggregate("", (current, param) => current + param + " AND ")
            .TrimEnd(" AND ".ToCharArray());

        var sql = $"DELETE FROM {objectToDelete.Table} WHERE {conditions};";
        command.CommandText = sql;
        command.Connection = conn;

        var parameterIndex = 0;
        foreach (var propertyInfo in objectToDelete.GetType().GetProperties())
        {
            if (propertyInfo.Name == "Table") continue;
            var value = propertyInfo.GetValue(objectToDelete);
            if (!IsValidValue(value, propertyInfo))
            {
                return new DeleteOperationResult
                {
                    Success = false,
                    Message = $"Invalid value for {propertyInfo.Name}",
                    ErrorCode = DeleteErrorCode.ValidationError
                };
            }
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@param{parameterIndex++}";
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        try
        {
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected switch
            {
                0 => new DeleteOperationResult
                {
                    Success = false,
                    Message = "Record not found.",
                    ErrorCode = DeleteErrorCode.RecordNotFound
                },
                1 => new DeleteOperationResult { Success = true, Message = null, ErrorCode = null },
                _ => new DeleteOperationResult { Success = true, Message = null, ErrorCode = null, MultipleRowsAffected = true}
            };
        }
        catch (DBConcurrencyException)
        {
            return new DeleteOperationResult
            {
                Success = false,
                Message = "Concurrency violation.",
                ErrorCode = DeleteErrorCode.ConcurrencyViolation
            };
        }
        catch (Exception ex)
        {
            return new DeleteOperationResult
            {
                Success = false,
                Message = $"Delete failed: {ex.Message}",
                ErrorCode = DeleteErrorCode.DatabaseError
            };
        }
    }
    
    public static async Task<SelectOperationResult<TObject>> SelectAsync<TObject>(Expression<Func<TObject, bool>>? filter = null) where TObject : ICanBeInsertedToDatabase, new()
{
    var objectToSelect = new TObject();
    var conn = objectToSelect.GetConnection();
    var commandType = objectToSelect.GetCommandType();
    var command = (DbCommand)Activator.CreateInstance(commandType)!;

    var columns = objectToSelect.GetType().GetProperties()
        .Where(p => p.Name != "Table")
        .Aggregate("", (current, propertyInfo) => current + propertyInfo.Name + ", ")
        .TrimEnd(',', ' ');

    var sql = $"SELECT {columns} FROM {objectToSelect.Table}";

    if (filter != null)
    {
        var condition = BuildSqlCondition(filter, out var parameters);
        if (!string.IsNullOrEmpty(condition))
        {
            sql += $" WHERE {condition}";
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = $"@param{i}";
                parameter.Value = parameters[i];
                command.Parameters.Add(parameter);
            }
        }
    }

    command.CommandText = sql;
    command.Connection = conn;

    var results = new SelectOperationResult<TObject>();

    try
    {
        await using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            results.Success = false;
            results.Message = "No records found.";
            results.ErrorCode = SelectErrorCode.RecordNotFound;
            return results;
        }

        while (await reader.ReadAsync())
        {
            var obj = new TObject();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.Name == "Table") continue;
                var value = reader[propertyInfo.Name];
                if (value == DBNull.Value) continue;

                if (!IsValidValue(value, propertyInfo))
                {
                    results.Success = false;
                    results.Message = $"Invalid value for {propertyInfo.Name}";
                    results.ErrorCode = SelectErrorCode.ValidationError;
                    return results;
                }

                var propertyType = propertyInfo.PropertyType;
                var convertedValue = Convert.ChangeType(value, propertyType);
                if (convertedValue is string stringValue) convertedValue = stringValue.TrimEnd();
                propertyInfo.SetValue(obj, convertedValue);
            }

            results.Data.Add(obj);
        }

        results.Success = true;
        results.Message = null;
        results.ErrorCode = null;
    }
    catch (SqlException ex) when (ex.Number == -2)
    {
        results.Success = false;
        results.Message = $"Select failed: {ex.Message}";
        results.ErrorCode = SelectErrorCode.TimeoutError;
    }
    catch (Exception ex)
    {
        results.Success = false;
        results.Message = $"Select failed: {ex.Message}";
        results.ErrorCode = SelectErrorCode.DatabaseError;
    }

    return results;
}
    
    public static async Task<InsertOperationResult> InsertAsync(ICanBeInsertedToDatabase objectToInsert)
    {
        var conn = objectToInsert.GetConnection();
        var commandType = objectToInsert.GetCommandType();
        var command = (DbCommand)Activator.CreateInstance(commandType)!;

        var columns = objectToInsert.GetType().GetProperties()
            .Aggregate("", (current, propertyInfo) => current + propertyInfo.Name + ", ")
            .TrimEnd(',', ' ');

        if (columns.StartsWith("Id, ")) columns = columns[4..];
        columns = columns.Replace(", Table", "");

        var values = objectToInsert.GetType().GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "Table")
            .Select((_, index) => $"@param{index}")
            .Aggregate("", (current, param) => current + param + ", ")
            .TrimEnd(',', ' ');

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

        try
        {
            await command.ExecuteNonQueryAsync();
            return new InsertOperationResult { Success = true, Message = null };
        }
        catch (Exception ex)
        {
            return new InsertOperationResult { Success = false, Message = $"Insert failed: {ex.Message}" };
        }
    }

    public static SelectOperationResult<TObject> Select<TObject>(Expression<Func<TObject, bool>>? filter = null) where TObject : ICanBeInsertedToDatabase, new()
    {
        return Task.Run(() => SelectAsync(filter)).GetAwaiter().GetResult();
    }

    public static UpdateOperationResult Update(ICanBeInsertedToDatabase objectToUpdate)
    {
        return Task.Run(() => UpdateAsync(objectToUpdate)).GetAwaiter().GetResult();
    }
    
    public static DeleteOperationResult Delete(ICanBeInsertedToDatabase objectToDelete)
    {
        return Task.Run(() => DeleteAsync(objectToDelete)).GetAwaiter().GetResult();
    }
    
    public static InsertOperationResult Insert(ICanBeInsertedToDatabase objectToInsert)
    {
        return Task.Run(() => InsertAsync(objectToInsert)).GetAwaiter().GetResult();
    }
}