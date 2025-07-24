using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IPaintPowderRepository
    {
        Task<PaintPowderResponseDTO> Create(PaintPowder paint, CancellationToken ct);
        Task<PaintPowderResponseDTO> Delete(int id, CancellationToken ct);
        Task<PaintPowderResponseDTO> Get(int id);
        Task<PaintPowderResponseDTO> GetAll();
        Task<PaintPowderResponseDTO> Update(int id, string paintCode, decimal paintPriceKG, CancellationToken ct);
    }
}