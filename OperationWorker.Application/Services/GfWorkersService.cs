using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Application.Services
{
    public class GfWorkersService : IGfWorkersService
    {
        private readonly IGfWorkersRepository _gfWorkersRepository;
        private readonly ILogger<GfWorkersService> _logger;

        public GfWorkersService(IGfWorkersRepository gfWorkersRepository, ILogger<GfWorkersService> logger)
        {
            _gfWorkersRepository = gfWorkersRepository;
            _logger = logger;
        }

        public async Task<GfWorkerResponseDTO> CreateGfWorker(GfWorker gfWorker, CancellationToken ct)
        {
            return await _gfWorkersRepository.Create(gfWorker, ct);
        }

        public async Task<GfWorkerResponseDTO> GetGfWorker(int id)
        {
            return await _gfWorkersRepository.Get(id);
        }

        public async Task<GfWorkerResponseDTO> GetAllGfWorkers()
        {
            return await _gfWorkersRepository.GetAll();
        }

        public async Task<GfWorkerResponseDTO> UpdateGfWorker(int id, string name, string surname, decimal stundasmaksa, string telefons, string epasts, string komentars, string prasmes, CancellationToken ct)
        {
            return await _gfWorkersRepository.Update(id, name, surname, stundasmaksa, telefons, epasts, komentars, prasmes, ct);
        }

        public async Task<GfWorkerResponseDTO> DeleteGfWorker(int id, CancellationToken ct)
        {
            return await _gfWorkersRepository.Delete(id, ct);
        }
    }
}
