using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class PaintPowderRepository : IPaintPowderRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<GfWorkersRepository> _logger;

        public PaintPowderRepository(OperationWorkerDbContext context, ILogger<GfWorkersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaintPowderResponseDTO> Create(PaintPowder paint, CancellationToken ct)
        {
            var response = new PaintPowderResponseDTO();
            try
            {
                var paintEntity = EntityMapper.MapToPaintPowderEntity(paint);

                await _context.PaintPowder.AddAsync(paintEntity, ct);
                await _context.SaveChangesAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'{@paint}' failed to create, because of DB error - '{status}'", paint, response.ErrorMessage);
            }

            return response;
        }

        public async Task<PaintPowderResponseDTO> Get(int id)
        {
            var response = new PaintPowderResponseDTO();
            try
            {
                var paintEntity = await _context.PaintPowder.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (paintEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No such Paint Powder with id:{id} in database";
                }
                else
                {
                    response.PaintPowder = EntityMapper.MapToPaintPowder(paintEntity);
                    response.IsCompleted = true;
                }
            }
            catch (Exception ex) 
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("Failed to find Paind Powder with id '{id}', because of DB error - '{ex}'",id ,ex);
            }
            
            return response;
        }

        public async Task<PaintPowderResponseDTO> GetAll()
        {
            var response = new PaintPowderResponseDTO();
            try
            {
                var painttEntity = await _context.PaintPowder
                    .AsNoTracking()
                    .ToListAsync();
                if( painttEntity == null )
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No Paint Powders in database";
                }
                else
                {
                    response.paintPowderList = painttEntity
                    .Select(b => PaintPowder.Create(b.Id, b.PaintCode, b.PaintPriceKG))
                    .ToList();
                    response.IsCompleted = true;
                } 
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all paintpowders, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<PaintPowderResponseDTO> Update(int id, string paintCode, decimal paintPriceKG, CancellationToken ct)
        {
            var response = new PaintPowderResponseDTO();
            try
            {
                await _context.PaintPowder
                    .Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.PaintCode, b => paintCode)
                    .SetProperty(b => b.PaintPriceKG, b => paintPriceKG), ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to update paintpowder with ID '{id}' and '{paintCode}', because of DB error - '{ex}'", paintCode, id, ex);
            }

            return response;
        }

        public async Task<PaintPowderResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new PaintPowderResponseDTO();
            try
            {
                await _context.PaintPowder
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to delete paintpowder with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }
    }
}
