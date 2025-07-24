
namespace OperationWorker.DataAccess.Entities
{
    public class AppUserEntity
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string AccessLevel { get; set; } = "Guest";
    }
}
