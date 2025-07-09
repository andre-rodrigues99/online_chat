namespace ModelsUsers;

using System.Net.WebSockets;
using System.Collections.Concurrent;
using Microsoft.OpenApi.Services;

public class User
{
    public int user_id { get; set; }
    public string user_name { get; set; }
    public WebSocket user_conn { get; set; }
    public List<int> user_groups { get; set; } = [0];

    public User(string name, WebSocket conn)
    {
        this.user_id = gen_id();
        this.user_name = name;
        this.user_conn = conn;
    }

    private int gen_id()
    {
        int new_id = new Random().Next();

        if (Users.IdExists(new_id))
        {
            gen_id();
        }

        return new_id;
    }
}


public static class Users
{
    private static ConcurrentDictionary<int, User> UsersPool = new ConcurrentDictionary<int, User>();

    public static ConcurrentDictionary<int, User> GetUsers()
    {
        return Users.UsersPool;
    }

    public static bool IdExists(int id)
    {
        return Users.UsersPool.ContainsKey(id);
    }

    public static void RegisterNew(User new_user)
    {
        Users.UsersPool.TryAdd(new_user.user_id, new_user);
    }

    public static ConcurrentDictionary<int, User> GetGroup(int group_id)
    {
        var group = new ConcurrentDictionary<int, User>(
            UsersPool.Where(x => x.Value.user_groups.Contains(group_id))
        );

        return group;
    }

    public static int GetId(WebSocket webSocket)
    {
        var user = UsersPool.FirstOrDefault(x => x.Value.user_conn.GetHashCode() == webSocket.GetHashCode());
        return user.Equals(default(KeyValuePair<int, User>)) ? -1 : user.Key;
    }
}


