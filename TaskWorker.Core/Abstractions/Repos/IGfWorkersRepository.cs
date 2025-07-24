using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IGfWorkersRepository
    {
        Task<GfWorkerResponseDTO> Create(GfWorker gfWorker, CancellationToken ct);
        Task<GfWorkerResponseDTO> Delete(int id, CancellationToken ct);
        Task<GfWorkerResponseDTO> Get(int id);
        Task<GfWorkerResponseDTO> GetAll();
        Task<GfWorkerResponseDTO> Update(int id, string name, string surname, decimal stundasmaksa, string telefons, string epasts, string komentars, string prasmes, CancellationToken ct);
    }
}