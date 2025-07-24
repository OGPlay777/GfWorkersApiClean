using NotificationService.Core.Models;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Core.Abstractions.Notificators
{
    public interface INotificationSender
    {
        Task<ResponseDTO> SendByEmail(Email mail);
        Task<ResponseDTO> SendByWhatsapp(WhatsappSMS sms);
    }
}