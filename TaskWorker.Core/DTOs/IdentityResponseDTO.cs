using System.Text.Json.Serialization;

namespace OperationWorker.Core.DTOs
{
    public class IdentityResponseDTO : ResponseDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
