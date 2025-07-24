namespace OperationWorker.Core.Models
{
    public class GfWorker
    {
        private GfWorker(int id, string name, string surname, decimal stundasMaksa, string? telefons, string? epasts, string? komentars, string prasmes)
        {
            Id = id;
            Name = name;
            Surname = surname;
            StundasMaksa = stundasMaksa;
            Telefons = telefons;
            Epasts = epasts;
            Komentars = komentars;
            Prasmes = prasmes;
        }

        public int Id { get; }
        public string Name { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public decimal StundasMaksa { get; } = 0;
        public string? Telefons { get; } = string.Empty;
        public string? Epasts { get; } = string.Empty;
        public string? Komentars { get; } = string.Empty;
        public string Prasmes { get; } = string.Empty;

        public static GfWorker Create(int id, string name, string surname, decimal stundasMaksa, string? telefons, string? epasts, string? komentars, string prasmes) =>
            new(id, name, surname, stundasMaksa, telefons, epasts, komentars, prasmes);
    }

}
