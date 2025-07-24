using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IAppUsersRepository
    {
        Task<AppUserResponseDTO> Create(AppUser appUser, CancellationToken ct);
        Task<AppUserResponseDTO> Delete(int id, CancellationToken ct);
        Task<AppUserResponseDTO> GetByLogin(string login);
        Task<AppUserResponseDTO> Get(int id);
        Task<AppUserResponseDTO> GetAll();
        Task<AppUserResponseDTO> Update(int id, string login, string password, string telephone, string accessLevel, CancellationToken ct);
    }
}