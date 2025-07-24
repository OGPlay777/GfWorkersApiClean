using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;
using OperationWorker.DataAccess.Cache;

namespace OperationWorker.Application.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsRepository _operationRepository;
        private readonly ILogger<OperationsService> _logger;
        private readonly IDistributedCache _distribudedCache;

        public OperationsService(IOperationsRepository operationsRepository, ILogger<OperationsService> logger, IDistributedCache distributedCache)
        {
            _operationRepository = operationsRepository;
            _distribudedCache = distributedCache;
            _logger = logger;
        }

        public async Task<OperationResponseDTO> CreateOperation(Operation operation, CancellationToken ct)
        {
            var cacheKey = "operations";
            var response = await _operationRepository.Create(operation, ct);
            if(response.IsCompleted == true)
            {
                _distribudedCache.Remove(cacheKey);
                _logger.LogInformation("Old cache for key: {CacheKey} is removed.", cacheKey);
            }

            return response;
        }

        public async Task<OperationResponseDTO> GetOperation(int id)
        {
            var cacheKey = $"operation:{id}";
            _logger.LogInformation("fetching data with key: {CacheKey} from cache.", cacheKey);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var operation = await _distribudedCache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database.", cacheKey);
                return await _operationRepository.Get(id);
            },
            cacheOptions)!;

            return operation!;
        }

        public async Task<OperationResponseDTO> GetAllOperations()
        {
            var cacheKey = "Orders";
            _logger.LogInformation("fetching data with key: {CacheKey} from cache.", cacheKey);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var operations = await _distribudedCache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database.", cacheKey);
                return await _operationRepository.GetAll();
            },
            cacheOptions)!;

            return operations!;
        }

        public async Task<OperationResponseDTO> UpdateOperation(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType, CancellationToken ct)
        {
            var cacheKey = "operations";
            var cacheKey2 = $"operation:{id}";
            var response = await _operationRepository.Update(id, darbaId, darbinieks, darbaVeids, darbaLaiks, darbaMaksa, operationType, ct);
            if (response.IsCompleted == true)
            {
                _logger.LogInformation("Old cache for key: {CacheKey} is removed.", cacheKey);
                _distribudedCache.Remove(cacheKey);
                _distribudedCache.Remove(cacheKey2);
            }

            return response;
        }

        public async Task<OperationResponseDTO> DeleteOperation(int id, CancellationToken ct)
        {
            var cacheKey = "operations";
            var cacheKey2 = $"operation:{id}";
            var response = await _operationRepository.Delete(id, ct);
            if(response.IsCompleted == true)
            {
                _distribudedCache.Remove(cacheKey);
                _distribudedCache.Remove(cacheKey2);
                _logger.LogInformation("Old cache for key: {CacheKey} is removed.", cacheKey);
            }

            return response;
        }

        public async Task<OperationResponseDTO> GetAllOperationsByGfWorker(int id)
        {
            return await _operationRepository.GetAllByGfWorker(id);
        }

        public async Task<(OperationResponseDTO, decimal)> GetAllEquipmentOperationsForOrder(string operationType)
        {
            var response = await _operationRepository.GetAllByOperationType(operationType);
            var equipmentOperations = response.operationsList;
            decimal operationsTotalSum = 0;

            foreach (var operation in equipmentOperations)
            {
                operationsTotalSum += operation.DarbaMaksa;
            }

            return (response, operationsTotalSum);
        }
    }
}