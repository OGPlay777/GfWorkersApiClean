
namespace OperationWorker.DataAccess.Entities
{
    public class OperationEntity
    {
        public int Id { get; set; }
        public int DarbaID { get; set; }
        public int Darbinieks { get; set; }
        public string DarbaVeids { get; set; } = string.Empty;
        public int DarbaLaiks { get; set; }
        public decimal DarbaMaksa { get; set; } = 0;
        public string OperationType { get; set; } = string.Empty;
    }
}
