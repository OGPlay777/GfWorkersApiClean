using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IOrdersRepository
    {
        Task<OrderResponseDTO> Create(Order order, CancellationToken ct);
        Task<OrderResponseDTO> Delete(int id, CancellationToken ct);
        Task<OrderResponseDTO> Get(int id);
        Task<OrderResponseDTO> GetAll();
        Task<OrderResponseDTO> Update(int id, string? pasutitajs, string? pienemsanasDatums, string? nodosanasDatums, string? pasTelefons, string? pasEpasts, string? pasutijums, string? pienemejs, decimal? rekinsKlientam, decimal? pasizmaksa, decimal? pelna, string? komentars, string? operacijas, byte[]? image, string? status, CancellationToken ct);
        Task<OrderResponseDTO> GetAllInProcess();
    }
}