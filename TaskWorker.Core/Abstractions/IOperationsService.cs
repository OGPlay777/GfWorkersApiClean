using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions
{
    public interface IOperationsService
    {
        Task<OperationResponseDTO> CreateOperation(Operation operation, CancellationToken ct);
        Task<OperationResponseDTO> DeleteOperation(int id, CancellationToken ct);
        Task<OperationResponseDTO> GetAllOperations();
        Task<OperationResponseDTO> GetOperation(int id);
        Task<OperationResponseDTO> UpdateOperation(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType, CancellationToken ct);
        Task<OperationResponseDTO> GetAllOperationsByGfWorker(int id);
        Task<(OperationResponseDTO, decimal)> GetAllEquipmentOperationsForOrder(string operationType);
    }
}