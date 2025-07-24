using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class PaintPowderResponseDTO : ResponseDTO
    {
        public PaintPowder? PaintPowder { get; set; }
        public List<PaintPowder>? paintPowderList = [];
    }
}
