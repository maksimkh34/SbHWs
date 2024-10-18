using System.Data.Common;
using System.Data.SqlClient;
using System.Windows;
using System.Data.OleDb;
using System.Windows.Controls;

namespace HW16;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private SqlConnection? LocalConnection { get; set; }
    private OleDbConnection? OleDbConnection { get; set; }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        var localConTask = Task.Factory.StartNew(() =>
        {
            var localResult = ConnectLocal();
            Dispatcher.InvokeAsync(() => UpdateData(LocalDbConStr, LocalDbStatus, localResult));
            LocalConnection = localResult.Connection;
        });
        
        var oleConTask = Task.Factory.StartNew(() =>
        {
            var accessResult = ConnectAccess();
            Dispatcher.InvokeAsync(() => UpdateData(AccessConStr, AccessStatus, accessResult));
            OleDbConnection = accessResult.Connection;
        });
        
        await Task.WhenAll(localConTask, oleConTask);

        var strClients = Select<SqlConnection, SqlCommand>(LocalConnection!, "Clients");
        var clients = Util.Parse<Client>(strClients);
        var strSales = Select<OleDbConnection, OleDbCommand>(OleDbConnection!, "ProductSales");
        var sales = Util.Parse<ProductSaleEntry>(strSales);
    }

    private static string Select<TConnection, TCommand>(TConnection conn, string selectionTarget)
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        var sql = "SELECT * FROM " + selectionTarget;
        var entries = string.Empty;

        using var command = (TCommand)Activator.CreateInstance(typeof(TCommand), sql, conn)!;
        if (command == null)
            throw new InvalidOperationException("Command creation failed.");

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                entries += reader[i] + "\t";
            }
            entries += "\r\n";
        }
        
        return entries;
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
        
        //Thread.Sleep(3000);
        return new ConnectionInfo<SqlConnection>(builder.ConnectionString, localConnection.State.ToString(),
            localConnection);
    }

    private static void UpdateData(TextBlock conTextBlock, TextBlock statusTextBlock, IConnectionInfo connectionInfo)
    {
        conTextBlock.Dispatcher.Invoke(() =>
        {
            conTextBlock.Text += connectionInfo.ConnectionString;
            statusTextBlock.Text += connectionInfo.Status;
        });

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
        
        //Thread.Sleep(5000);
        return new ConnectionInfo<OleDbConnection>(dbConnectionStringBuilder.ConnectionString, oleConnection.State.ToString(), oleConnection);
    }

    public interface IConnectionInfo
    {
        string ConnectionString { get; }
        string Status { get; }
    }

    public class ConnectionInfo<T>(string connectionString, string status, T connection) : IConnectionInfo
        where T : DbConnection
    {
        public T Connection { get; set; } = connection;
        public string ConnectionString { get; } = connectionString;
        public string Status { get; } = status;
    }
}
