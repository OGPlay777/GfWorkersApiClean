namespace GF_Workers_API.Contracts
{
    public record AppUserResponse(
        int Id,
        string Login,
        string Telephone,
        string AccessLevel);
}
