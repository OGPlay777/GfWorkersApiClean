using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IOperationsRepository
    {
        Task<OperationResponseDTO> Create(Operation operation, CancellationToken ct);
        Task<OperationResponseDTO> Delete(int id, CancellationToken ct);
        Task<OperationResponseDTO> Get(int id);
        Task<OperationResponseDTO> GetAll();
        Task<OperationResponseDTO> Update(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType, CancellationToken ct);
        Task<OperationResponseDTO> GetAllByGfWorker(int id);
        Task<OperationResponseDTO> GetAllByOperationType(string operationType);
    }
}