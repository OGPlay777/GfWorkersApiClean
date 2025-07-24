using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class OperationsRepository : IOperationsRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<GfWorkersRepository> _logger;

        public OperationsRepository(OperationWorkerDbContext context, ILogger<GfWorkersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResponseDTO> Create(Operation operation, CancellationToken ct)
        {
            var response = new OperationResponseDTO();
            try
            {
                var operationEntity = EntityMapper.MapToOperationEntity(operation);

                await _context.Operations.AddAsync(operationEntity, ct);
                await _context.SaveChangesAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'{@operation}' failed to create, because of DB error - '{status}'", operation, ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> Get(int id)
        {
            var response = new OperationResponseDTO();
            try
            {
                var operationEntity = await _context.Operations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (operationEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No such Operation in database";
                }
                else
                {
                    response.IsCompleted = true;
                    response.Operation = EntityMapper.MapToOperation(operationEntity);
                }
            }
            catch(Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get operation, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> GetAll()
        {
            var response = new OperationResponseDTO();
            try
            {
                var operationsEntities = await _context.Operations
                    .AsNoTracking()
                    .ToListAsync();
                if (operationsEntities == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No Operations in database";
                }
                else
                {
                    response.IsCompleted = true;
                    response.operationsList = operationsEntities
                    .Select(b => Operation.Create(b.Id, b.DarbaID, b.Darbinieks, b.DarbaVeids, b.DarbaLaiks, b.DarbaMaksa, b.OperationType))
                    .ToList();
                }
             }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all operations, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> Update(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType, CancellationToken ct)
        {
            var response = new OperationResponseDTO();
            try
            {
                await _context.Operations
                    .Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.DarbaID, b => darbaId)
                    .SetProperty(b => b.Darbinieks, b => darbinieks)
                    .SetProperty(b => b.DarbaVeids, b => darbaVeids)
                    .SetProperty(b => b.DarbaLaiks, b => darbaLaiks)
                    .SetProperty(b => b.DarbaMaksa, b => darbaMaksa)
                    .SetProperty(b => b.OperationType, b => operationType), ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to update operation with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new OperationResponseDTO();
            try
            {
                await _context.Operations
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to delete operation with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> GetAllByGfWorker(int id)
        {
            var response = new OperationResponseDTO();
            try 
            {
                var operationsEntities = await _context.Operations.Where(b => b.Darbinieks == id).AsNoTracking().ToListAsync();
                if(operationsEntities == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No Operations in database for Worker with id {id}";
                }
                else
                {
                    response.operationsList = operationsEntities
                    .Select(b => Operation.Create(b.Id, b.DarbaID, b.Darbinieks, b.DarbaVeids, b.DarbaLaiks, b.DarbaMaksa, b.OperationType))
                    .ToList();

                    response.IsCompleted = true;
                }
            }
            catch(Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all operations for Worker with id '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<OperationResponseDTO> GetAllByOperationType(string operationType)
        {
            var response = new OperationResponseDTO();
            try
            {
                var operationsByType = await _context.Operations.Where(b => b.OperationType == operationType).AsNoTracking().ToListAsync();
                if (operationsByType == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No Operations founded in database for operation type - {operationType}";
                }
                else
                {
                    response.operationsList = operationsByType
                    .Select(b => Operation.Create(b.Id, b.DarbaID, b.Darbinieks, b.DarbaVeids, b.DarbaLaiks, b.DarbaMaksa, b.OperationType))
                    .ToList();

                    response.IsCompleted = true;
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all operations for operation type - '{operationType}', because of DB error - '{ex}'", operationType, ex);
            }

            return response;
        }
    }
}
