using System.Text.RegularExpressions;

namespace OperationWorker.Core.Models
{
    public class AppUser
    {
        private const int maxLenght = 50;
        public const string motif = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";

        private AppUser(int id, string login, string passwordHash, string telephone, string accessLevel)
        {
            Id = id;
            Login = login;
            PasswordHash = passwordHash;
            Telephone = telephone;
            AccessLevel = accessLevel;
        }

        public int Id { get; set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public string Telephone { get; private set; }
        public string AccessLevel { get; private set; } = "Guest";

        public static AppUser Create(int id, string login, string passwordHash, string telephone, string accessLevel) =>
            new(id, login, passwordHash, telephone, accessLevel);

        public static (AppUser? user, bool isValid, List<string> error) CreateAndValidate(int id, string login, string passwordHash, string telephone, string accessLevel)
        {
            var errors = new List<string>();
            var isValid = false;

            if(IsValidEmail(login) == false || login.Length < maxLenght)
            {
                errors.Add($"Login is not valid. Should be email adress - xxx@xxx.xx, and should be shorter than {maxLenght}");
            }
            if(IsValidNumber(telephone) == false)
            {
                errors.Add("Telephone number is not valid");
            }
            if(errors.Count < 1)
            {
                isValid = true;
            }
            
            return (new AppUser(id, login, passwordHash, telephone, accessLevel), isValid, errors);

        }

        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidNumber(string number)
        {
            if (number != null) return Regex.IsMatch(number, motif);
            else return false;
        }
    }
}