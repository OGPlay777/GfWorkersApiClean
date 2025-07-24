using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions
{
    public interface IOrdersService
    {
        Task<OrderResponseDTO> CreateOrder(Order order, CancellationToken ct);
        Task<OrderResponseDTO> DeleteOrder(int id, CancellationToken ct);
        Task<OrderResponseDTO> GetAllOrders();
        Task<OrderResponseDTO> GetOrder(int id);
        Task<OrderResponseDTO> UpdateOrder(int id, string? pasutitajs, string? pienemsanasDatums, string? nodosanasDatums, string? pasTelefons, string? pasEpasts, string? pasutijums, string? pienemejs, decimal? rekinsKlientam, decimal? pasizmaksa, decimal? pelna, string? komentars, string? operacijas, byte[]? image, string? status, CancellationToken ct);
        Task<OrderResponseDTO> GetOrdersInProcess();
        Task<OrderResponseDTO> GetOrdersByWorkerSkills(int id);
        Task<OrderResponseDTO> TakeOrderOperation(int orderId, int appWorkerId, string darbs, CancellationToken ct);
        Task<OrderResponseDTO> FinishOrderOperation(int orderId, int appWorkerId, CancellationToken ct);

    }
}