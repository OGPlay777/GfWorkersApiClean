using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<GfWorkersRepository> _logger;

        public OrdersRepository(OperationWorkerDbContext context, ILogger<GfWorkersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderResponseDTO> Create(Order order, CancellationToken ct)
        {
            var response = new OrderResponseDTO();
            try
            {
                var orderEntity = EntityMapper.MapToOrderEntity(order);

                await _context.Orders.AddAsync(orderEntity, ct);
                await _context.SaveChangesAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'{@order}' failed to create, because of DB error - '{status}'", order, response.ErrorMessage);
            }

            return response;
        }

        public async Task<OrderResponseDTO> Get(int id)
        {
            var response = new OrderResponseDTO();
            try
            {
                var orderEntity = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
                if (orderEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No such Order '{id}' in database";
                }
                else
                {
                    response.Order = EntityMapper.MapToOrder(orderEntity);
                    response.IsCompleted = true;
                }
            }
            catch (Exception ex) 
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get order '{id}', because of DB error - '{ex}'",id, ex);
            }

            return response;
        }

        public async Task<OrderResponseDTO> GetAll()
        {
            var response = new OrderResponseDTO();
            try
            {
                var ordersEntities = await _context.Orders
                    .AsNoTracking()
                    .ToListAsync();
                if (ordersEntities == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No Orders found in database";
                }
                else
                {
                    response.ordersList = ordersEntities
                    .Select(b => Order.Create(b.Id, b.Pasutitajs, b.PienemsanasDatums, b.NodosanasDatums, b.PasTelefons, b.PasEpasts, b.Pasutijums, b.Pienemejs, b.RekinsKlientam, b.Pasizmaksa, b.Pelna, b.Komentars, b.Operacijas, b.Image, b.Status))
                    .ToList();
                    response.IsCompleted = true;
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all orders, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<OrderResponseDTO> Update(int id, string? pasutitajs, string? pienemsanasDatums, string? nodosanasDatums, string? pasTelefons, string? pasEpasts, string? pasutijums, string? pienemejs, decimal? rekinsKlientam, decimal? pasizmaksa, decimal? pelna, string? komentars, string? operacijas, byte[]? image, string? status, CancellationToken ct)
        {
            var response = new OrderResponseDTO();
            try
            {
                await _context.Orders
                    .Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Pasutitajs, b => pasutitajs)
                    .SetProperty(b => b.PienemsanasDatums, b => pienemsanasDatums)
                    .SetProperty(b => b.NodosanasDatums, b => nodosanasDatums)
                    .SetProperty(b => b.PasTelefons, b => pasTelefons)
                    .SetProperty(b => b.PasEpasts, b => pasEpasts)
                    .SetProperty(b => b.Pasutijums, b => pasutijums)
                    .SetProperty(b => b.Pienemejs, b => pienemejs)
                    .SetProperty(b => b.RekinsKlientam, b => rekinsKlientam)
                    .SetProperty(b => b.Pasizmaksa, b => pasizmaksa)
                    .SetProperty(b => b.Pelna, b => pelna)
                    .SetProperty(b => b.Komentars, b => komentars)
                    .SetProperty(b => b.Operacijas, b => operacijas)
                    .SetProperty(b => b.Image, b => image)
                    .SetProperty(b => b.Status, b => status), ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to update order with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<OrderResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new OrderResponseDTO();
            try
            {
                await _context.Orders
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to delete order with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<OrderResponseDTO> GetAllInProcess()
        {
            var response = new OrderResponseDTO();
            try
            {
                var allOrdersInProcessEntities = await _context.Orders.Where(b => b.NodosanasDatums == "nav pabeigts").ToListAsync();
                if(allOrdersInProcessEntities == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"No unfinished Orders are founded in database";
                }
                else
                {
                    response.ordersList = allOrdersInProcessEntities
                    .Select(b => Order.Create(b.Id, b.Pasutitajs, b.PienemsanasDatums, b.NodosanasDatums, b.PasTelefons, b.PasEpasts, b.Pasutijums, b.Pienemejs, b.RekinsKlientam, b.Pasizmaksa, b.Pelna, b.Komentars, b.Operacijas, b.Image, b.Status))
                    .ToList();

                    response.IsCompleted = true;
                }    
            }
            catch(Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get not completed orders , because of DB error - '{ex}'", ex);
            }
            
            return response;
        }
    }
}
