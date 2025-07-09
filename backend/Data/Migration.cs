namespace Data.MigrationDb;

public class Migration
{
    public string migration_query()
    {
        string sql = @"CREATE TABLE messages (
                                        msg_id NUMBER
                                        msg_time TIMESTAMP, 
                                        msg_from VARCHAR2, 
                                        msg_to VARCHAR2, 
                                        msg_text TEXT
                                    )";
        return sql;
    }
}