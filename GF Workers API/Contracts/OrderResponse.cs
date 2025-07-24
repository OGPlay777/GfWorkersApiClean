namespace GF_Workers_API.Contracts
{
    public record OrderResponse(
        int? Id,
        string? Pasutitajs,
        string? PienemsanasDatums,
        string? NodosanasDatums,
        string? PasTelefons,
        string? PasEpasts,
        string? Pasutijums,
        string? Pienemejs,
        decimal? RekinsKlientam,
        decimal? Pasizmaksa,
        decimal? Pelna,
        string? Komentars,
        string? Operacijas,
        byte[] Image,
        string? Status);
}
