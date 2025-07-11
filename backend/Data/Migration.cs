namespace Data.MigrationDb;

public class Migration
{
    public string migration_messages_table()
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
    public string migration_users_table()
    {
        string sql = @"CREATE TABLE users (
                                user_id NUMBER,
                                user_name VARCHAR2(30),
                                email VARCHAR2,
                                salt_hash_bytes BYTEA,
        )";

        return sql;
    }
}