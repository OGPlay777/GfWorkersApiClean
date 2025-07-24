namespace GF_Workers_API.Contracts
{
    public record OperationResponse(
        int Id,
        int DarbaId,
        int Darbinieks,
        string DarbaVeids,
        int DarbaLaiks,
        decimal DarbaMaksa,
        string OperationType);
}
