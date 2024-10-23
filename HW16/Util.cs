using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace HW16;

public static class Util
{
    public static IEnumerable<T> Parse<T>(string input, bool setEmptyAsNull=true) where T : new()
    {
        List<T> collection = [];
        foreach (var line in input.Split('\n'))
        {
            if(line is "\n" or "" or "\r") continue;
            var item = new T();
            var props = item.GetType().GetProperties();
            var rawValues = line.Split('\t');
            List<string> values = [];
            values.AddRange(rawValues.Where(value => value != "\r"));
            if(values.Count != props.Length) throw new Exception($"Error: {input} is not {typeof(T).Name}. Expected {props.Length}, got {values.Count}.");
            for (var i = 0; i < props.Length; i++)
            {
                props[i].SetValue(item, values[i] == "          " && setEmptyAsNull? null : values[i].TrimEnd());
            }
            collection.Add(item);
        }
        return collection;
    }
    
    public static (DbConnection, Type) GetDbTypes(ICanBeInsertedToDatabase obj)
    {
        return obj.Table switch
        {
            Database.Tables.Clients => (Database.LocalConnection, typeof(SqlCommand)),
            Database.Tables.ProductSales => (Database.OleDbConnection, typeof(OleDbCommand)),
            _ => throw new InvalidOperationException("Unknown table type")
        };
    }
}

public class Client : ICanBeInsertedToDatabase
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Database.Tables Table { get; set; } = Database.Tables.Clients;
}

public class ProductSaleEntry : ICanBeInsertedToDatabase
{
    public int Id { get; set; }
    public string Email { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public Database.Tables Table { get; set; } = Database.Tables.ProductSales;
}

public interface ICanBeInsertedToDatabase
{
    public Database.Tables Table { get; set; }
}
