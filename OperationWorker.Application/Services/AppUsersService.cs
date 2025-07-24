using NotificationService.Core.Models;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Auth;
using OperationWorker.Core.Abstractions.Notificators;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.Application.Services
{
    public class AppUsersService : IAppUsersService
    {
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly INotificationSender _notificationSender;
        private readonly IIdentityProvider _identityProvider;

        public AppUsersService(IAppUsersRepository appUsersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, INotificationSender notificationSender, IIdentityProvider identityProvider)
        {
            _appUsersRepository = appUsersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _notificationSender = notificationSender;
            _identityProvider = identityProvider;
        }

        public async Task<AppUserResponseDTO> CreateAppUser(int id, string login, string password, string telephone, string accessLevel, CancellationToken ct)
        {
            var hashedPassword = _passwordHasher.Generate(password);
            var loginToLower = login.ToLower().Trim();
            var appUser = AppUser.Create(id, loginToLower, hashedPassword, telephone, accessLevel);
            var response = await _appUsersRepository.Create(appUser, ct);

            if (response.IsCompleted == false)
            {
                return response;
            } 

            //testing notification microservice
            var testMessage = $"user {appUser.Login} is successfully created";

            var emailResponse = await _notificationSender.SendByEmail(Email.CreateRegistrationInfo(appUser));
            if (emailResponse.IsCompleted == false)
            {
                response.ErrorMessage = $"Email notification error: {emailResponse.ErrorMessage}";
            }
            var whatsappResponse = await _notificationSender.SendByWhatsapp(WhatsappSMS.Create(appUser.Telephone, testMessage));
            if(whatsappResponse.IsCompleted == false)
            {
                response.ErrorMessage += $"Whatsapp notification error: {whatsappResponse.ErrorMessage}";
            }
            // end of testing
            return response;
        }

        public async Task<IdentityResponseDTO> LoginAppUser(string login, string password, CancellationToken ct)
        {
            var identityResponse = await _identityProvider.RequestTokenWithHeaders(login, password, ct);
            return identityResponse;
        }   

        public async Task<AppUserResponseDTO> GetAppUser(int id)
        {
            return await _appUsersRepository.Get(id);
        }

        public async Task<AppUserResponseDTO> GetAllAppUsers()
        {
            return await _appUsersRepository.GetAll();
        }

        public async Task<AppUserResponseDTO> UpdateAppUser(int id, string login, string password, string telephone, string accessLevel, CancellationToken ct)
        {
            var loginToLower = login.ToLower();
            return await _appUsersRepository.Update(id, loginToLower, password, telephone, accessLevel, ct);
        }

        public async Task<AppUserResponseDTO> DeleteAppUser(int id, CancellationToken ct)
        {
            return await _appUsersRepository.Delete(id, ct);
        }
    }
}
