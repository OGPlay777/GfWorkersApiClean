using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;
using OperationWorker.DataAccess.Cache;
using System.Globalization;

namespace OperationWorker.Application.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IGfWorkersRepository _gfWorkersRepository;
        private readonly IOperationsRepository _operationsRepository;
        private readonly ILogger<OrdersService> _logger;
        private readonly IDistributedCache _distributedCache;

        public OrdersService(IOrdersRepository ordersRepository, IGfWorkersRepository gfWorkersRepository, IOperationsRepository operationsRepository, ILogger<OrdersService> logger, IDistributedCache distributedCache)
        {
            _ordersRepository = ordersRepository;
            _gfWorkersRepository = gfWorkersRepository;
            _operationsRepository = operationsRepository;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<OrderResponseDTO> CreateOrder(Order order, CancellationToken ct)
        {
            var cacheKey = "orders";
            var response = await _ordersRepository.Create(order, ct);
            if (response.IsCompleted == true)
            {
                _distributedCache.Remove(cacheKey);
                _logger.LogInformation("Old cache for key: {cacheKey} is removed.", cacheKey);
            }
            return response;
        }

        public async Task<OrderResponseDTO> GetOrder(int id)
        {
            var cacheKey = $"order:{id}";
            _logger.LogInformation("fetching data with key: {cacheKey} from cache.", cacheKey);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var response = await _distributedCache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {cacheKey} from database.", cacheKey);
                return await _ordersRepository.Get(id);
            },
            cacheOptions)!;

            return response!;
        }

        public async Task<OrderResponseDTO> GetAllOrders()
        {
            var cacheKey = "Orders";
            _logger.LogInformation("fetching data with key: {cacheKey} from cache.", cacheKey);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var response = await _distributedCache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {cacheKey} from database.", cacheKey);
                return await _ordersRepository.GetAll();
            },
            cacheOptions)!;

            return response!;
        }

        public async Task<OrderResponseDTO> UpdateOrder(int id, string? pasutitajs, string? pienemsanasDatums, string? nodosanasDatums, string? pasTelefons, string? pasEpasts, string? pasutijums, string? pienemejs, decimal? rekinsKlientam, decimal? pasizmaksa, decimal? pelna, string? komentars, string? operacijas, byte[]? image, string? status, CancellationToken ct)
        {
            var cacheKey = "orders";
            var cacheKey2 = $"order:{id}";
            var response = await _ordersRepository.Update
                (id, pasutitajs, pienemsanasDatums, nodosanasDatums,
                pasTelefons, pasEpasts, pasutijums, pienemejs, rekinsKlientam,
                pasizmaksa, pelna, komentars, operacijas, image, status, ct);
            if (response.IsCompleted == true)
            {
                _distributedCache.Remove(cacheKey);
                _distributedCache.Remove(cacheKey2);
                _logger.LogInformation("Old cache for key: {cacheKey} is removed.", cacheKey);
            }
            return response;
        }

        public async Task<OrderResponseDTO> DeleteOrder(int id, CancellationToken ct)
        {
            var cacheKey = "orders";
            var cacheKey2 = $"order:{id}";
            var response = await _ordersRepository.Delete(id, ct);
            if (response.IsCompleted == true)
            {
                _distributedCache.Remove(cacheKey);
                _distributedCache.Remove(cacheKey2);
                _logger.LogInformation("Old cache for key: {cacheKey}, {cachekey2} is removed.", cacheKey, cacheKey2);
            }
            return response;
        }

        public async Task<OrderResponseDTO> GetOrdersInProcess()
        {
            var cacheKey = "ordersInProcess";
            _logger.LogInformation("fetching data with key: {cacheKey} from cache.", cacheKey);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var response = await _distributedCache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {cacheKey} from database.", cacheKey);
                return await _ordersRepository.GetAllInProcess();
            },
            cacheOptions)!;

            return response!;
        }

        public async Task<OrderResponseDTO> GetOrdersByWorkerSkills(int id)
        {
            var workersResponse = await _gfWorkersRepository.Get(id);
            if (workersResponse.IsCompleted == false)
            {
                var ordersResponse = new OrderResponseDTO
                {
                    IsCompleted = false,
                    ErrorMessage = workersResponse.ErrorMessage
                };
                return ordersResponse;
            }
            var skills = workersResponse.GfWorker!.Prasmes.Split(',').ToArray();
            var allOrdersResponse = await _ordersRepository.GetAllInProcess();
            if (allOrdersResponse.IsCompleted == false)
            {
                return allOrdersResponse;
            }
            foreach (Order order in allOrdersResponse.ordersList!)
            {
                if (order.Operacijas!.Split(',').ToArray().Any(value => skills.Contains(value)))
                {
                    allOrdersResponse.ordersList.Add(order);
                }
            }
            return allOrdersResponse;
        }

        public async Task<OrderResponseDTO> TakeOrderOperation(int orderId, int appWorkerId, string darbs, CancellationToken ct)
        {
            var cacheKey = "orders";
            var orderResponse = await _ordersRepository.Get(orderId);
            if(orderResponse.IsCompleted == false)
            {
                return orderResponse;
            }
            var workerResponse = await _gfWorkersRepository.Get(appWorkerId);
            if (workerResponse.IsCompleted == false)
            {
                var ordersResponse = new OrderResponseDTO
                {
                    IsCompleted = false,
                    ErrorMessage = workerResponse.ErrorMessage
                };
                return ordersResponse;
            }
            DateTime time = DateTime.Now;
            var status = $"{orderId}|{workerResponse.GfWorker.Id}|{darbs}|{time}|inProgress";
            var orderUpdateResponse = await _ordersRepository.Update(
                orderId, orderResponse.Order.Pasutitajs, orderResponse.Order.PienemsanasDatums, orderResponse.Order.NodosanasDatums,
                orderResponse.Order.PasTelefons, orderResponse.Order.PasEpasts, orderResponse.Order.Pasutijums, orderResponse.Order.Pienemejs, orderResponse.Order.RekinsKlientam,
                orderResponse.Order.Pasizmaksa, orderResponse.Order.Pelna, orderResponse.Order.Komentars, orderResponse.Order.Operacijas, orderResponse.Order.Image, status, ct);
            if (orderUpdateResponse.IsCompleted == false)
            {
                var ordersResponse = new OrderResponseDTO
                {
                    IsCompleted = false,
                    ErrorMessage = orderUpdateResponse.ErrorMessage
                };
                return ordersResponse;
            }
            _distributedCache.Remove(cacheKey);
            _logger.LogInformation("Old cache for key: {cacheKey} is removed.", cacheKey);
            return orderUpdateResponse;
        }

        public async Task<OrderResponseDTO> FinishOrderOperation(int orderId, int appWorkerId, CancellationToken ct)
        {
            var cacheKey = "orders";
            var cacheKey2 = "operations";
            var orderResponse = await _ordersRepository.Get(orderId);
            if (orderResponse.IsCompleted == false)
            {
                return orderResponse;
            }
            var workerResponse = await _gfWorkersRepository.Get(appWorkerId);
            if (workerResponse.IsCompleted == false)
            {
                var ordersResponse = new OrderResponseDTO
                {
                    IsCompleted = false,
                    ErrorMessage = workerResponse.ErrorMessage
                };
                return ordersResponse;
            }
            var cultureInfo = new CultureInfo("lv-LV");
            DateTime startTime = Convert.ToDateTime(orderResponse.Order!.Status!.Split('|')[3], cultureInfo);
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            var taskMinutes = Convert.ToInt32(span.TotalMinutes);
            var darbaMaksaKalkulacija = workerResponse.GfWorker!.StundasMaksa / 60 * taskMinutes;
            var darbaVeids = orderResponse.Order.Status.Split('|')[2];
            var operationType = "worker";
            int id = 0;
            var operation = Operation.Create(id, orderId, appWorkerId, darbaVeids, taskMinutes, darbaMaksaKalkulacija, operationType);
            var operationResponse = await _operationsRepository.Create(operation, ct);
            if (operationResponse.IsCompleted == false)
            {
                var ordersResponse = new OrderResponseDTO
                {
                    IsCompleted = false,
                    ErrorMessage = operationResponse.ErrorMessage
                };
                return ordersResponse;
            }
            var status = string.Empty;
            var response = await _ordersRepository.Update(
                orderId, orderResponse.Order.Pasutitajs, orderResponse.Order.PienemsanasDatums, orderResponse.Order.NodosanasDatums,
                orderResponse.Order.PasTelefons, orderResponse.Order.PasEpasts, orderResponse.Order.Pasutijums, orderResponse.Order.Pienemejs, orderResponse.Order.RekinsKlientam,
                orderResponse.Order.Pasizmaksa, orderResponse.Order.Pelna, orderResponse.Order.Komentars, orderResponse.Order.Operacijas, orderResponse.Order.Image, status, ct);
            if (response.IsCompleted == false)
            {
                return response;
            }
            _distributedCache.Remove(cacheKey);
            _distributedCache.Remove(cacheKey2);
            return response;
        }
    }
}
