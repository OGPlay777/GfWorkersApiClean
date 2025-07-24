using OperationWorker.Core.Models;

namespace NotificationService.Core.Models
{
    public class Email
    {
        private Email(string recepient, string subject, string body)
        {
            Recepient = recepient;
            Subject = subject;
            Body = body;

        }

        public string Recepient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public static Email Create(string recepient,string subject,string body)
        {
            var email = new Email(recepient, subject, body);

            return email;
        }

        public static Email CreateRegistrationInfo(AppUser user)
        {
            var subject = "User registration confirmation";
            var date = DateTime.Now;
            var body = $"User {user.Login} successfully registrated in GF Worker 2000. Account status is set to - Guest, you have to wait for administration approval. Registration date - {date}";

            var email = new Email(user.Login, subject, body);

            return email;
        }
    }
}
 