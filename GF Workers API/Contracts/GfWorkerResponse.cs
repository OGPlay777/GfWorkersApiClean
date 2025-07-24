namespace GF_Workers_API.Contracts
{
    public record GfWorkerResponse(
        int Id, 
        string Name, 
        string Surname, 
        decimal StundasMaksa, 
        string Telefons,
        string Epasts,
        string Komentars, 
        string Prasmes);
    
}
