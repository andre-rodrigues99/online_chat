using System.Text.Json.Serialization;


public class ChangePasswRequest
{
    public required string UserEmail { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
public class ChangePasswResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    public static ChangePasswResponse Success()
    {
        return new ChangePasswResponse
        {
            Status = "success",
            Message = "Senha alterada com sucesso."
        };
    }

    public static ChangePasswResponse Error()
    {
        return new ChangePasswResponse
        {
            Status = "error",
            Message = "Erro ao alterar a senha."
        };
    }
}

