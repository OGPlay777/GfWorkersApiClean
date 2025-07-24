using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Models;

namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OperationsController : ControllerBase
    {
        private readonly IOperationsService _operationsService;

        public OperationsController(IOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Create([FromBody] OperationRequest operationRequest, CancellationToken ct)
        {
            int id = 0;
            var operation = Operation.Create(id, operationRequest.DarbaId, operationRequest.Darbinieks, operationRequest.DarbaVeids, operationRequest.DarbaLaiks, operationRequest.DarbaMaksa, operationRequest.OperationType);
            var operationResponse = await _operationsService.CreateOperation(operation, ct);
            if (operationResponse.IsCompleted == false)
            {
                return BadRequest(operationResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var operationResponse = await _operationsService.GetAllOperations();
            var response = operationResponse.operationsList!.Select(b => new OperationResponse(b.Id, b.DarbaID, b.Darbinieks, b.DarbaVeids, b.DarbaLaiks, b.DarbaMaksa, b.OperationType)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var operationResponse = await _operationsService.GetOperation(id);
            if (operationResponse.IsCompleted == false)
            {
                return BadRequest(operationResponse.ErrorMessage);
            }
            var response = new OperationResponse(operationResponse.Operation!.Id, operationResponse.Operation.DarbaID, operationResponse.Operation.Darbinieks,
                operationResponse.Operation.DarbaVeids, operationResponse.Operation.DarbaLaiks, operationResponse.Operation.DarbaMaksa,
                operationResponse.Operation.OperationType);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Policy = "WorkerPolicy")]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetAllByGfWorker(int id)
        {
            var operationResponse = await _operationsService.GetAllOperationsByGfWorker(id);
            if (operationResponse.IsCompleted == false)
            {
                return BadRequest(operationResponse.ErrorMessage);
            }
            var response = operationResponse.operationsList!.Select(b => new OperationResponse(b.Id, b.DarbaID, b.Darbinieks, b.DarbaVeids, b.DarbaLaiks, b.DarbaMaksa, b.OperationType)).ToList();
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Policy = "SupervisorPolicy")]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OperationRequest operationRequest, CancellationToken ct)
        {
            var operationResponse = await _operationsService.UpdateOperation(id, operationRequest.DarbaId, operationRequest.Darbinieks, operationRequest.DarbaVeids, operationRequest.DarbaLaiks, operationRequest.DarbaMaksa, operationRequest.OperationType, ct);
            if (operationResponse.IsCompleted == false)
            {
                return BadRequest(operationResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var operationResponse = await _operationsService.DeleteOperation(id, ct);
            if (operationResponse.IsCompleted == false)
            {
                return BadRequest(operationResponse.ErrorMessage);
            }
            return Ok();
        }

        //Get Operation by user for a time period


    }
}
