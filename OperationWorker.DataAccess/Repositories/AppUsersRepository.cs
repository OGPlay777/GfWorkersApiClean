using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class AppUsersRepository : IAppUsersRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<AppUsersRepository> _logger;

        public AppUsersRepository(OperationWorkerDbContext context, ILogger<AppUsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AppUserResponseDTO> Create(AppUser appUser, CancellationToken ct)
        {
            var response = new AppUserResponseDTO();

            try
            {
                var checkUser = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Login == appUser.Login);
                if (checkUser == null)
                {
                    try
                    {
                        var appUserEntity = EntityMapper.MapToUserEntity(appUser);
                        await _context.AppUsers.AddAsync(appUserEntity, ct);
                        await _context.SaveChangesAsync(ct);
                        response.IsCompleted = true;
                    }
                    catch (Exception ex)
                    {
                        response.IsCompleted = false;
                        response.ErrorMessage = ex.Message;
                        _logger.LogError("User'{Login}' failed to create, because of DB error - '{status}'", appUser.Login, response.ErrorMessage);
                    }

                    return response;
                }
                else
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"Registration failed. User {appUser.Login} already exists ";
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("Failed to find '{Login}' in DB, because of DB error - '{status}'", appUser.Login, response.ErrorMessage);
            }

            return response;
        }

        public async Task<AppUserResponseDTO> Get(int id)
        {
            var response = new AppUserResponseDTO();
            try
            {
                var appUserEntity = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (appUserEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No such AppUser in database";
                }
                else
                {
                    response.IsCompleted = true;
                    response.AppUser = EntityMapper.MapToUser(appUserEntity);
                }
            }
            catch(Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("Failed to get user with id - '{id}' from DB, because of DB error - '{status}'", id, response.ErrorMessage);
            }

            return response;
        }

        public async Task<AppUserResponseDTO> GetByLogin(string login)
        {
            var response = new AppUserResponseDTO();
            try
            {
                var appUserEntity = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login);
                if (appUserEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No such AppUser in database";
                }
                else
                {
                    response.IsCompleted = true;
                    response.AppUser = EntityMapper.MapToUser(appUserEntity);
                    
                }
            }
            catch(Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("Failed to get user with login - '{id}' from DB, because of DB error - '{status}'", login, response.ErrorMessage);
            }

            return response;
        }

        public async Task<AppUserResponseDTO> GetAll()
        {
            var response = new AppUserResponseDTO();
            try
            {
                var appUsersEntity = await _context.AppUsers
                    .AsNoTracking()
                    .ToListAsync();

                response.appUsersList = appUsersEntity
                    .Select(b => AppUser.Create(b.Id, b.Login, b.PasswordHash, b.Telephone, b.AccessLevel))
                    .ToList();
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError(" Failed to load list of AppUsers, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<AppUserResponseDTO> Update(int id, string login, string passwordHash, string telephone, string accessLevel, CancellationToken ct)
        {
            var response = new AppUserResponseDTO();
            try
            {
                await _context.AppUsers
                    .Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Login, b => login)
                    .SetProperty(b => b.PasswordHash, b => passwordHash)
                    .SetProperty(b => b.Telephone, b => telephone)
                    .SetProperty(b => b.AccessLevel, b => accessLevel), ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError(" Failed to update AppUser '{Login}', because of DB error - '{status}'", login, response.ErrorMessage);
            }
            return response;
        }

        public async Task<AppUserResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new AppUserResponseDTO();
            try
            {
                await _context.AppUsers
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError(" Failed to update AppUser with ID '{id}', because of DB error - '{status}'", id, response.ErrorMessage);
            }

            return response;
        }
    }
}
