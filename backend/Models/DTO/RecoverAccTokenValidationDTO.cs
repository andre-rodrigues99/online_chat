using System.Text.Json.Serialization;

public class RecoverAccTokenValidationRequest
{
    public required string UserEmail { get; set; }
    public required int RecoverCode { get; set; }
}



public class DataRecoverAccTokenValidationResponse
{
    public string Token { get; set; } = string.Empty;
}

public class RecoverAccTokenValidationResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("data")]
    public required DataRecoverAccTokenValidationResponse Data { get; set; }



    public static RecoverAccTokenValidationResponse Success(string token)
    {
        return new RecoverAccTokenValidationResponse
        {
            Status = "success",
            Message = "Código validado com sucesso.",
            Data = new DataRecoverAccTokenValidationResponse { Token = token }
        };
    }


    public static RecoverAccTokenValidationResponse Error()
    {
        return new RecoverAccTokenValidationResponse
        {
            Status = "error",
            Message = "Erro ao validar o código de recuperação.",
            Data = new DataRecoverAccTokenValidationResponse()
        };
    }
}
