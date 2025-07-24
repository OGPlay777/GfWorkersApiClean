using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(AppUser appuser);
    }
}