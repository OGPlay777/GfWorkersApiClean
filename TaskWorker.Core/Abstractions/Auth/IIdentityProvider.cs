using OperationWorker.Core.DTOs;

namespace OperationWorker.Core.Abstractions.Auth
{
    public interface IIdentityProvider
    {
        Task<IdentityResponseDTO> RequestToken(string login, string password);
        Task<IdentityResponseDTO> RequestTokenWithHeaders(string login, string password, CancellationToken ct);
    }
}