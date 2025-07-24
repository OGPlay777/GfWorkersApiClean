using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class GfWorkerResponseDTO : ResponseDTO
    {
        public GfWorker? GfWorker { get; set; }
        public List<GfWorker>? gfWorkersList = [];
    }
}
