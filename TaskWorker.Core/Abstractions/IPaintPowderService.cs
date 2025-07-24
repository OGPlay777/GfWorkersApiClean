using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions
{
    public interface IPaintPowderService
    {
        Task<PaintPowderResponseDTO> CreatePaintPowder(PaintPowder paintPowder, CancellationToken ct);
        Task<PaintPowderResponseDTO> DeletePaintPowder(int id, CancellationToken ct);
        Task<PaintPowderResponseDTO> GetAllPaintPowders();
        Task<PaintPowderResponseDTO> GetPaintPowder(int id);
        Task<PaintPowderResponseDTO> UpdatePaintPowder(int id, string paintCode, decimal paintPriceKG, CancellationToken ct);
    }
}