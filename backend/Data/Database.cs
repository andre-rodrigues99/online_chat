namespace Data.OracleDbHandle;

using Oracle.ManagedDataAccess.Client;
using Data.MigrationDb;
using Npgsql;
using System.Threading.Tasks;

public class Database
{
    private string? db_type = Environment.GetEnvironmentVariable("DB_TYPE");
    private string? url = Environment.GetEnvironmentVariable("DB_URL");
    private string? port = Environment.GetEnvironmentVariable("DB_PORT");
    private string? user = Environment.GetEnvironmentVariable("DB_USER");
    private string? passw = Environment.GetEnvironmentVariable("DB_PASSW");
    private string? service = Environment.GetEnvironmentVariable("DB_SERV_SID_DBNAME");

    public OracleConnection OracleStartConnection()
    {
        string conn_query = $"User Id={this.user};Password={this.passw};Data Source={this.url}:{this.port}/{this.service};";
        Console.WriteLine(conn_query);
        OracleConnection conn = new OracleConnection(conn_query);

        conn.Open();

        return conn;
    }

    public void OracleCloseConnection(OracleConnection conn)
    {
        conn.Close();
    }

    public NpgsqlDataSource PostgresStartConnection()
    {
        string conn_query = $"Host={this.url};Username={this.user};Password={this.passw};Database={this.service};";

        NpgsqlDataSource dataSource = NpgsqlDataSource.Create(conn_query);
        dataSource.OpenConnection();

        return dataSource;
    }
    public void PostgresCloseConnection(NpgsqlDataSource conn)
    {
        conn.Dispose();
    }

    public void RunMigration()
    {
        Console.WriteLine("Inicializando Migração do Banco de Dados.");

        if (this.db_type == "ORACLE")
        {
            var conn = OracleStartConnection();
            OracleCommand query1 = new OracleCommand(new Migration().migration_messages_table(), conn);
            OracleCommand query2 = new OracleCommand(new Migration().migration_users_table(), conn);

            query1.ExecuteNonQuery();
            query2.ExecuteNonQuery();
            conn.Commit();
            OracleCloseConnection(conn);
        }
        else if (this.db_type == "POSTGRES")
        {
            var conn = PostgresStartConnection();

            var query1 = conn.CreateCommand(new Migration().migration_messages_table());
            var query2 = conn.CreateCommand(new Migration().migration_users_table());

            query1.ExecuteNonQueryAsync();
            query2.ExecuteNonQueryAsync();

            PostgresCloseConnection(conn);
        }
        Console.WriteLine("finalizando Migração do Banco de Dados.");
    }
    
    
}