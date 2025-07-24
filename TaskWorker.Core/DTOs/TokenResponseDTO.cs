using System.Text.Json.Serialization;

namespace OperationWorker.Core.DTOs
{
    public class TokenResponseDTO
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
