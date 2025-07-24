namespace GF_Workers_API.Contracts
{
    public record PaintPowderRequest(
        int Id,
        string PaintCode,
        decimal PaintPriceKG);
}
