namespace Data.OracleDbHandler;

using Oracle.ManagedDataAccess.Client;
using System;

public class OracleDbHandle
{
    private string? url = Environment.GetEnvironmentVariable("ORACLE_DB_URL");
    private int? port = int.Parse(Environment.GetEnvironmentVariable("ORACLE_DB_URL") ?? "0");
    private string? user = Environment.GetEnvironmentVariable("ORACLE_DB_URL");
    private string? passw = Environment.GetEnvironmentVariable("ORACLE_DB_URL");
    private string? service = Environment.GetEnvironmentVariable("ORACLE_DB_URL");

    public OracleConnection StartConnection()
    {
        if (url is null || port is null || user is null || passw is null || service is null)
        {
            Console.WriteLine("ERRO: Variaveis de ambiente não encontradas!");
        }

        string conn_query = $"User Id={this.user};Password={this.passw}Data Source=//{this.url}:{this.port}/{this.service}";

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

        string sql = @"CREATE TABLE messages (
                                                msg_id NUMBER
                                                msg_time TIMESTAMP, 
                                                msg_from VARCHAR2, 
                                                msg_to VARCHAR2, 
                                                msg_text TEXT
                                            )";

        OracleCommand query = new OracleCommand(sql, conn);

        query.ExecuteNonQuery();
        conn.Commit();

        CloseConnection(conn);
        Console.WriteLine("Migração do Banco de Dados Finalizada.");
    }
}