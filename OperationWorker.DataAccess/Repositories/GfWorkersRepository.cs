using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class GfWorkersRepository : IGfWorkersRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<GfWorkersRepository> _logger;

        public GfWorkersRepository(OperationWorkerDbContext context, ILogger<GfWorkersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GfWorkerResponseDTO> Create(GfWorker gfWorker, CancellationToken ct)
        {
            var response = new GfWorkerResponseDTO();
            try
            {
                var gfWorkerEntity = EntityMapper.MapToGfWorkerEntity(gfWorker);
                await _context.GfWorkers.AddAsync(gfWorkerEntity, ct);
                await _context.SaveChangesAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'{@gfWorker}' failed to create, because of DB error - '{status}'", gfWorker, response.ErrorMessage);
            }

            return response;
        }

        public async Task<GfWorkerResponseDTO> Get(int id)
        {
            var response = new GfWorkerResponseDTO();
            try
            {
                var gfWorkerEntity = await _context.GfWorkers.FirstOrDefaultAsync(x => x.Id == id);
                if (gfWorkerEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No such GfWorker in database";
                }
                else
                {
                    response.GfWorker = EntityMapper.MapToGfWorker(gfWorkerEntity);
                    response.IsCompleted = true;
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to find GfWorker with id - {id}, because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<GfWorkerResponseDTO> GetAll()
        {
            var response = new GfWorkerResponseDTO();
            try
            {
                var gfWorkerEntities = await _context.GfWorkers
                .AsNoTracking()
                .ToListAsync();
                if (gfWorkerEntities == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No GfWorkers in database";
                }
                else
                {
                    response.gfWorkersList = gfWorkerEntities
                   .Select(b => GfWorker.Create(b.Id, b.Name, b.Surname, b.StundasMaksa, b.Telefons, b.Epasts, b.Komentars, b.Prasmes))
                   .ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all gfworkers, because of DB error - '{ex}'", ex);
            }

            return response;
        }


        public async Task<GfWorkerResponseDTO> Update(int id, string name, string surname, decimal stundasmaksa, string telefons, string epasts, string komentars, string prasmes, CancellationToken ct)
        {
            var response = new GfWorkerResponseDTO();
            try
            {
                await _context.GfWorkers
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Name, b => name)
                .SetProperty(b => b.Surname, b => surname)
                .SetProperty(b => b.StundasMaksa, b => stundasmaksa)
                .SetProperty(b => b.Telefons, b => telefons)
                .SetProperty(b => b.Epasts, b => epasts)
                .SetProperty(b => b.Komentars, b => komentars)
                .SetProperty(b => b.Prasmes, b => prasmes), ct); ;

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to update GfWorkers with ID '{id}' and '{name}', because of DB error - '{ex}'", name, id, ex);
            }

            return response;
        }

        public async Task<GfWorkerResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new GfWorkerResponseDTO();
            try
            {
                await _context.GfWorkers
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to delete GfWorkers with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }
    }
}
