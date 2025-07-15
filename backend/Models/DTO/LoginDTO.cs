using System.Text.Json.Serialization;

namespace Models.DTO.Login;

public class LoginRequest
{
    [JsonPropertyName("user_email")]
    public required string UserEmail { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}





public class LoginResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("data")]
    public required DataLoginResponse Data { get; set; }


    public static LoginResponse Success(string usr_id, string token)
    {
        return new LoginResponse
        {
            Status = "success",
            Message = "Login efetuado com sucesso",
            Data = new DataLoginResponse
            {
                user_id = usr_id,
                token = token
            }
        };
    }

    public static LoginResponse Error()
    {
        return new LoginResponse
        {
            Status = "error",
            Message = "Não foi possivel efetuar o Login",
            Data = new DataLoginResponse()
        };
    }

}





public class DataLoginResponse
{
    [JsonPropertyName("user_id")]
    public string user_id { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string token { get; set; } = string.Empty;

}
