using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class EquipmentResponseDTO : ResponseDTO
    {
        public Equipment? Equipment { get; set; }
        public List<Equipment>? equipmentList = [];
    }
}