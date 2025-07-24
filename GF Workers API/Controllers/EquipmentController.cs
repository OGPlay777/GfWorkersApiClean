using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Models;

namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        private readonly ILogger<EquipmentController> _logger;

        public EquipmentController(IEquipmentService equipmentService, ILogger<EquipmentController> logger)
        {
            _equipmentService = equipmentService;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] EquipmentRequest equipmentRequest, CancellationToken ct)
        {
            int id = 0;
            var equipment = Equipment.Create(id, equipmentRequest.EquipmentName, equipmentRequest.EquipmentSelfcost);
            var responseResponse = await _equipmentService.CreateEquipment(equipment, ct);
            if (responseResponse.IsCompleted == false)
            {
                return BadRequest(responseResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "OfficePolicy")]
        public async Task<IActionResult> GetAll()
        {
            var equipmentResponse = await _equipmentService.GetAllEquipment();
            if (equipmentResponse.IsCompleted == false)
            {
                return BadRequest(equipmentResponse.ErrorMessage);
            }
            var response = equipmentResponse.equipmentList!.Select(b => new EquipmentResponse(b.Id, b.EquipmentName, b.EquipmentSelfcost)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var equipmentResponse = await _equipmentService.GetEquipment(id);
            if (equipmentResponse.IsCompleted == false)
            {
                return BadRequest(equipmentResponse.ErrorMessage);
            }
            var response = new EquipmentResponse(equipmentResponse.Equipment!.Id, equipmentResponse.Equipment.EquipmentName, equipmentResponse.Equipment.EquipmentSelfcost);
            return Ok(response);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] EquipmentRequest equipmentRequest, CancellationToken ct)
        {
            var equipmentResponse = await _equipmentService.UpdateEquipment(id, equipmentRequest.EquipmentName, equipmentRequest.EquipmentSelfcost, ct);
            if (equipmentResponse.IsCompleted == false)
            {
                return BadRequest(equipmentResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var equipmentResponse = await _equipmentService.DeleteEquipment(id, ct);
            if (equipmentResponse.IsCompleted == false)
            {
                return BadRequest(equipmentResponse.ErrorMessage);
            }
            return Accepted();
        }

    }
}
