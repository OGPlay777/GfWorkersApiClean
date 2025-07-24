using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using System.Security.Claims;


namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AppUsersController : ControllerBase
    {
        private readonly IAppUsersService _appUsersService;
        private readonly ILogger<AppUsersController> _logger;
        private readonly IConfiguration _configuration;

        public AppUsersController(IAppUsersService appUsersService, ILogger<AppUsersController> logger, IConfiguration configuration)
        {
            _appUsersService = appUsersService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                User.Identity.Name,
                Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value),
                AllClaims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] AppUserRequest appUserRequest, CancellationToken ct)
        {
            int id = 0;
            var appUserResponse = await _appUsersService.CreateAppUser(id, appUserRequest.Login, appUserRequest.Password, appUserRequest.Telephone, appUserRequest.AccessLevel, ct);
            if (appUserResponse.IsCompleted == false)
            {
                return BadRequest(appUserResponse.ErrorMessage);
            }
            return Accepted(appUserResponse.ErrorMessage);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] AppUserRequest appUserRequest, CancellationToken ct)
        {
            int id = 0;
            var accessLevel = "Guest";
            var appUserResponse = await _appUsersService.CreateAppUser(id, appUserRequest.Login, appUserRequest.Password, appUserRequest.Telephone, accessLevel, ct);
            if (appUserResponse.IsCompleted == false)
            {
                _logger.LogError("New user - '{appUserRequest.Login}' is not created, because of error: {appUserResponse.errorMessage}", appUserRequest.Login, appUserResponse.ErrorMessage);
                return BadRequest(appUserResponse.ErrorMessage);
            }
            _logger.LogInformation("New user - '{appUserRequest.Login}' is successfully created", appUserRequest.Login);
            return Accepted(appUserResponse.ErrorMessage);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] AppUserRequest appUserRequest, CancellationToken ct)
        {
            var appUserResponse = await _appUsersService.LoginAppUser(appUserRequest.Login, appUserRequest.Password, ct);
            if (appUserResponse.IsCompleted == false)
            {
                _logger.LogInformation("'{appUserRequest.Login}' had a bad luck at logging in", appUserRequest.Login);
                return BadRequest(appUserResponse.ErrorMessage);
            }
            else
            {
                Response.Cookies.Append(_configuration["CookieOptions:SecretName"]!, appUserResponse.AccessToken!);
                _logger.LogInformation("'{appUserRequest.Login}' successfully logged in", appUserRequest.Login);
                return Ok();
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var appUserResponse = await _appUsersService.GetAllAppUsers();
            if (appUserResponse.IsCompleted == false)
            {
                return BadRequest(appUserResponse.ErrorMessage);
            }
            var response = appUserResponse.appUsersList!.Select(b => new AppUserResponse(b.Id, b.Login,b.Telephone, b.AccessLevel)).ToList();
            return Ok(appUserResponse.appUsersList);
        }

        [HttpDelete]
        [Route("[action]")]
        public Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("ThickSquirrel");
            //and redirect to login page
            return Logout();
        }
    }
}
