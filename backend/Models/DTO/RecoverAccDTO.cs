using System.Text.Json.Serialization;

public class RecoverAccResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }


    public static RecoverAccResponse Success()
    {
        return new RecoverAccResponse
        {
            Status = "success",
            Message = "Código de recuperação enviado com sucesso."
        };
    }

    public static RecoverAccResponse Error()
    {
        return new RecoverAccResponse
        {
            Status = "error",
            Message = "Ocorreu um erro durante o envio do código de recuperação."
        };
    }
}

