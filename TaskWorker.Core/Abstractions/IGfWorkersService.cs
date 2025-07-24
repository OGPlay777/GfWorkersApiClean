using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions
{
    public interface IGfWorkersService
    {
        Task<GfWorkerResponseDTO> CreateGfWorker(GfWorker gfWorker, CancellationToken ct);
        Task<GfWorkerResponseDTO> DeleteGfWorker(int id, CancellationToken ct);
        Task<GfWorkerResponseDTO> GetAllGfWorkers();
        Task<GfWorkerResponseDTO> GetGfWorker(int id);
        Task<GfWorkerResponseDTO> UpdateGfWorker(int id, string name, string surname, decimal stundasmaksa, string telefons, string epasts, string komentars, string prasmes, CancellationToken ct);
    }
}