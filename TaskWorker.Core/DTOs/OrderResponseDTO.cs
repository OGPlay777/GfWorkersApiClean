using OperationWorker.Core.Models;

namespace OperationWorker.Core.DTOs
{
    public class OrderResponseDTO : ResponseDTO
    {
        public Order? Order { get; set; }
        public List<Order>? ordersList = [];
    }
}
