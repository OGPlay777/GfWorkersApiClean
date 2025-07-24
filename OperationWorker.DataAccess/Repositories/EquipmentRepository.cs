using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.Core.DTOs;
using OperationWorker.Core.Models;

namespace OperationWorker.DataAccess.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly OperationWorkerDbContext _context;
        private readonly ILogger<EquipmentRepository> _logger;

        public EquipmentRepository(OperationWorkerDbContext context, ILogger<EquipmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<EquipmentResponseDTO> Create(Equipment equipment, CancellationToken ct)
        {
            var response = new EquipmentResponseDTO();
            try
            {
                var equipmentEntity = EntityMapper.MapToEquipmentEntity(equipment);
                await _context.Equipment.AddAsync(equipmentEntity, ct);
                await _context.SaveChangesAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'{@Equipment}' failed to create, because of DB error - '{ex}'", equipment, ex);
            }

            return response;
        }

        public async Task<EquipmentResponseDTO> Get(int id)
        {
            var response = new EquipmentResponseDTO();
            try
            {
                var equipmentEntity = await _context.Equipment.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (equipmentEntity == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No such Equipment in database";
                }
                else
                {
                    response.Equipment = EntityMapper.MapToEquipment(equipmentEntity);
                    response.IsCompleted = true;
                }
            }
            catch (Exception ex) 
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("Failed to get equipment with id - {id}, because of DB error - '{ex}'", id, ex);
            }

            return response;
        }

        public async Task<EquipmentResponseDTO> GetAll()
        {
            var response = new EquipmentResponseDTO();
            try
            {
                var equipmentEntityList = await _context.Equipment
                    .AsNoTracking()
                    .ToListAsync();
                if (equipmentEntityList == null)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "No Equipment in database";
                }
                else
                {
                    response.equipmentList = equipmentEntityList
                    .Select(b => Equipment.Create(b.Id, b.EquipmentName, b.EquipmentSelfcost))
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to get all quipment, because of DB error - '{ex}'", ex);
            }

            return response;
        }

        public async Task<EquipmentResponseDTO> Update(int id, string equipmentName, decimal equipmentSelfcost, CancellationToken ct)
        {
            var response = new EquipmentResponseDTO();
            try
            {
                await _context.Equipment
                    .Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.EquipmentName, b => equipmentName)
                    .SetProperty(b => b.EquipmentSelfcost, b => equipmentSelfcost), ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to update Equimpment '{equipmentName}', because of DB error - '{ex}'", equipmentName, ex);
            }

            return response;
        }

        public async Task<EquipmentResponseDTO> Delete(int id, CancellationToken ct)
        {
            var response = new EquipmentResponseDTO();
            try
            {
                await _context.Equipment
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync(ct);

                response.IsCompleted = true;
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = ex.Message;
                _logger.LogError("'Failed to delete Equimpment with ID '{id}', because of DB error - '{ex}'", id, ex);
            }

            return response;
        }
    }
}
