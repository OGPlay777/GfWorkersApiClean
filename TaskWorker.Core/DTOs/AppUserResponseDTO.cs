using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class AppUserResponseDTO : ResponseDTO
    {
        public AppUser? AppUser { get; set; }
        public List<AppUser>? appUsersList = [];
    }
}   