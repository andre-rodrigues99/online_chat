namespace Data.OracleDbHandle;

using Oracle.ManagedDataAccess.Client;
using Data.MigrationDb;

public class OracleDb
{
    private string? url = Environment.GetEnvironmentVariable("ORACLE_DB_URL");
    private string? port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
    private string? user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
    private string? passw = Environment.GetEnvironmentVariable("ORACLE_DB_PASSW");
    private string? service = Environment.GetEnvironmentVariable("ORACLE_DB_SERV_SID");


    public OracleConnection StartConnection()
    {
        Console.WriteLine(url);
        Console.WriteLine(port);
        Console.WriteLine(user);
        Console.WriteLine(passw);
        Console.WriteLine(service);

        if (url is null || port is null || user is null || passw is null || service is null)
        {
            Console.WriteLine("ERRO: Variaveis de ambiente não encontradas!");
        }

        string conn_query = $"User Id={this.user};Password={this.passw};Data Source={this.url}:{this.port}/{this.service};";
        Console.WriteLine(conn_query);
        OracleConnection conn = new OracleConnection(conn_query);

        conn.Open();

        return conn;
    }

    public void CloseConnection(OracleConnection conn)
    {
        conn.Close();
    }

    public void RunMigration()
    {
        Console.WriteLine("Inicializando Migração do Banco de Dados.");
        var conn = StartConnection();

        OracleCommand query = new OracleCommand(new Migration().migration_query(), conn);

        query.ExecuteNonQuery();
        conn.Commit();

        CloseConnection(conn);
        Console.WriteLine("Migração do Banco de Dados Finalizada.");
    }


}