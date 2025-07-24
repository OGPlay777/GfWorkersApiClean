
namespace OperationWorker.DataAccess.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public string? Pasutitajs { get; set; } = string.Empty;
        public string? PienemsanasDatums { get; set; } = string.Empty;
        public string? NodosanasDatums { get; set; } = string.Empty;
        public string? PasTelefons { get; set; } = string.Empty;
        public string? PasEpasts { get; set; } = string.Empty;
        public string? Pasutijums { get; set; } = string.Empty;
        public string? Pienemejs { get; set; } = string.Empty;
        public decimal? RekinsKlientam { get; set; }
        public decimal? Pasizmaksa { get; set; }
        public decimal? Pelna { get; set; }
        public string? Komentars { get; set; } = string.Empty;
        public string? Operacijas { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string? Status { get; set; } = string.Empty;
    }
}
