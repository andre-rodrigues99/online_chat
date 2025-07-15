using Microsoft.AspNetCore.Identity;
using Data.DataBaseOperations;

namespace Services.AuthServices;

public static class Authentication
{
    public static string new_hash(int usr_id, string password)
    {
        var pass_h = new PasswordHasher<object>();

        return pass_h.HashPassword(usr_id, password);
    }

    public static bool validate_password(int usr_id, string password)
    {
        var hasher = new PasswordHasher<object>();
        var db_hash = DbOps.GetUserHash(usr_id);

        return hasher.VerifyHashedPassword(
            usr_id,
            db_hash,
            password
            ) != PasswordVerificationResult.Failed;
    }

    public static bool send_recover_code(string email)
    {
        Console.WriteLine(email);
        return true;
    }

    public static bool validate_recovery_code(string email, int recover_code)
    {
        return true;
    }

    public static string gen_token(int user_id)
    {
        return "token";
    }

    public static bool change_passw(int user_id, string old_passw, string new_passw)
    {
        return true;
    }
}