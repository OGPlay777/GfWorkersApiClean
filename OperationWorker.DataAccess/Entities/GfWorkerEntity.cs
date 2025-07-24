
namespace OperationWorker.DataAccess.Entities
{
    public class GfWorkerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public decimal StundasMaksa { get; set; } = 0;
        public string Telefons { get; set; } = string.Empty;
        public string Epasts { get; set; } = string.Empty;
        public string Komentars { get; set; } = string.Empty;
        public string Prasmes { get; set; } = string.Empty;
    }
}
