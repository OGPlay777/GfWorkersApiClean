using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions
{
    public interface IEquipmentService
    {
        Task<EquipmentResponseDTO> CreateEquipment(Equipment equipment, CancellationToken ct);
        Task<EquipmentResponseDTO> DeleteEquipment(int id, CancellationToken ct);
        Task<EquipmentResponseDTO> GetAllEquipment();
        Task<EquipmentResponseDTO> GetEquipment(int id);
        Task<EquipmentResponseDTO> UpdateEquipment(int id, string equipmentName, decimal equipmentSelfcost, CancellationToken ct);
    }
}