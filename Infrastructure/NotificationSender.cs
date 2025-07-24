using NotificationService.Core.Models;
using OperationWorker.Core.Abstractions.Notificators;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;
using System.Net;
using System.Text;

namespace OperationWorker.Infrastructure
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NotificationSender(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private static readonly string notificationMicroservice = "https://127.0.0.1:8081";

        public async Task<ResponseDTO> SendByEmail(Email mail)
        {
            var response = new ResponseDTO();
            var emailServiceHttpClient = _httpClientFactory.CreateClient();
            StringContent content = new(Newtonsoft.Json.JsonConvert.SerializeObject(mail), Encoding.UTF8, "application/json");
            var httpResponse = await emailServiceHttpClient.PostAsync($"{notificationMicroservice}/api/MailSender/SendInfo", content);

            string responseText = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.StatusCode != HttpStatusCode.Accepted)
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Unexpected Response Code. Notification email has not been sent. Reason: " + httpResponse.StatusCode;
            }
            else
            {
                response.IsCompleted = true;
            }
            return response;
        }

        public async Task<ResponseDTO> SendByWhatsapp(WhatsappSMS sms)
        {
            var response = new ResponseDTO();
            var whatsappHttpClient = _httpClientFactory.CreateClient();
            StringContent content = new(Newtonsoft.Json.JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var httpResponse = await whatsappHttpClient.PostAsync($"{notificationMicroservice}/api/WhatsappSender/SendWhatsappSms", content);

            string responseText = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.StatusCode != HttpStatusCode.Accepted)
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Unexpected Response Code. Notification email has not been sent. Reason: " + httpResponse.StatusCode;
            }
            else
            {
                response.IsCompleted = true;
            }
            return response;
        }
    }
}
