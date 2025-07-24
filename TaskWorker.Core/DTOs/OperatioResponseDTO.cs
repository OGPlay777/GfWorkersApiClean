using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class OperationResponseDTO : ResponseDTO
    {
        public Operation? Operation { get; set; }
        public List<Operation>? operationsList = [];
    }
}
