using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Repos
{
    public interface IEquipmentRepository
    {
        Task<EquipmentResponseDTO> Create(Equipment equipment, CancellationToken ct);
        Task<EquipmentResponseDTO> Delete(int id, CancellationToken ct);
        Task<EquipmentResponseDTO> Get(int id);
        Task<EquipmentResponseDTO> GetAll();
        Task<EquipmentResponseDTO> Update(int id, string equipmentName, decimal equipmentSelfcost, CancellationToken ct);
    }
}