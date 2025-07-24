namespace OperationWorker.Core.Models
{
    public class Order
    {
        public Order(int id, string pasutitajs, string pienemsanasDatums, string? nodosanasDatums, string? pasTelefons,
            string? pasEpasts, string pasutijums, string pienemejs, decimal? rekinsKlientam, decimal? pasizmaksa,
            decimal? pelna, string? komentars, string? operacijas, byte[]? image, string? status)
        {
            Id = id;
            Pasutitajs = pasutitajs;
            PienemsanasDatums = pienemsanasDatums;
            NodosanasDatums = nodosanasDatums;
            PasTelefons = pasTelefons;
            PasEpasts = pasEpasts;
            Pasutijums = pasutijums;
            Pienemejs = pienemejs;
            RekinsKlientam = rekinsKlientam;
            Pasizmaksa = pasizmaksa;
            Pelna = pelna;
            Komentars = komentars;
            Operacijas = operacijas;
            Image = image;
            Status = status;
        }

        public int Id { get; }
        public string? Pasutitajs { get; } = string.Empty;
        public string? PienemsanasDatums { get; } = string.Empty;
        public string? NodosanasDatums { get; } = string.Empty;
        public string? PasTelefons { get; } = string.Empty;
        public string? PasEpasts { get; } = string.Empty;
        public string? Pasutijums { get; } = string.Empty;
        public string? Pienemejs { get; } = string.Empty;
        public decimal? RekinsKlientam { get; } = 0;
        public decimal? Pasizmaksa { get; } = 0;
        public decimal? Pelna { get; } = 0;
        public string? Komentars { get; } = string.Empty;
        public string? Operacijas { get; } = string.Empty;
        public byte[]? Image { get; }
        public string? Status { get; } = string.Empty;

        public static Order Create(int id, string pasutitajs, string pienemsanasDatums, string? nodosanasDatums,
            string? pasTelefons, string? pasEpasts, string pasutijums, string pienemejs, decimal? rekinsKlientam,
            decimal? pasizmaksa, decimal? pelna, string? komentars, string? operacijas, byte[] image, string status) =>
            new(id, pasutitajs, pienemsanasDatums, nodosanasDatums, pasTelefons, pasEpasts, pasutijums, pienemejs,
                rekinsKlientam, pasizmaksa, pelna, komentars, operacijas, image, status);
    }
}