namespace GF_Workers_API.Contracts
{
    public record AppUserRequest(
        int Id, 
        string Login, 
        string Password,
        string Telephone,
        string AccessLevel);
}
