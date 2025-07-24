using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Application.Services
{
    public class PaintPowderService : IPaintPowderService
    {
        private readonly IPaintPowderRepository _paintPowderRepository;
        private readonly ILogger<PaintPowderService> _logger;

        public PaintPowderService(IPaintPowderRepository paintPowderRepository, ILogger<PaintPowderService> logger)
        {
            _paintPowderRepository = paintPowderRepository;
            _logger = logger;
        }

        public async Task<PaintPowderResponseDTO> CreatePaintPowder(PaintPowder paintPowder, CancellationToken ct)
        {
            return await _paintPowderRepository.Create(paintPowder, ct);
        }

        public async Task<PaintPowderResponseDTO> GetPaintPowder(int id)
        {
            return await _paintPowderRepository.Get(id);
        }

        public async Task<PaintPowderResponseDTO> GetAllPaintPowders()
        {
            return await _paintPowderRepository.GetAll();
        }

        public async Task<PaintPowderResponseDTO> UpdatePaintPowder(int id, string paintCode, decimal paintPriceKG, CancellationToken ct)
        {
            return await _paintPowderRepository.Update(id, paintCode, paintPriceKG, ct);
        }

        public async Task<PaintPowderResponseDTO> DeletePaintPowder(int id, CancellationToken ct)
        {
            return await _paintPowderRepository.Delete(id, ct);
        }
    }
}
