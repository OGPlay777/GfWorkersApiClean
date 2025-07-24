namespace OperationWorker.Core.Models
{
    public class WhatsappSMS
    {
        private WhatsappSMS(string chatId, string message)
        {
            this.chatId = chatId;
            this.message = message;
        }

        public string chatId { get; set; }
        public string message { get; set; }

        public static WhatsappSMS Create(string chatId, string message) =>
            new(chatId, message);
    }
}
