using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ILogger<EquipmentService> _logger;

        public EquipmentService(IEquipmentRepository equipmentRepository, ILogger<EquipmentService> logger)
        {
            _equipmentRepository = equipmentRepository;
            _logger = logger;
        }

        public async Task<EquipmentResponseDTO> CreateEquipment(Equipment equipment, CancellationToken ct)
        {
            return await _equipmentRepository.Create(equipment, ct);
        }

        public async Task<EquipmentResponseDTO> GetEquipment(int id)
        {
            return await _equipmentRepository.Get(id);
        }

        public async Task<EquipmentResponseDTO> GetAllEquipment()
        {
            return await _equipmentRepository.GetAll();
        }

        public async Task<EquipmentResponseDTO> UpdateEquipment(int id, string equipmentName, decimal equipmentSelfcost, CancellationToken ct)
        {
            return await _equipmentRepository.Update(id, equipmentName, equipmentSelfcost, ct);
        }

        public async Task<EquipmentResponseDTO> DeleteEquipment(int id, CancellationToken ct)
        {
            return await _equipmentRepository.Delete(id, ct);
        }
    }
}
