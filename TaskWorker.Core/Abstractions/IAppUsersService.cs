using OperationWorker.Core.DTOs;

namespace OperationWorker.Core.Abstractions
{
    public interface IAppUsersService
    {
        Task<AppUserResponseDTO> CreateAppUser(int id, string login, string password, string telephone, string accessLevel, CancellationToken ct);
        Task<IdentityResponseDTO> LoginAppUser(string login, string password, CancellationToken ct);
        Task<AppUserResponseDTO> DeleteAppUser(int id, CancellationToken ct);
        Task<AppUserResponseDTO> GetAllAppUsers();
        Task<AppUserResponseDTO> GetAppUser(int id);
        Task<AppUserResponseDTO> UpdateAppUser(int id, string login, string password, string telephone, string accessLevel, CancellationToken ct);
    }
}