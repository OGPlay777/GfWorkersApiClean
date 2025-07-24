using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Models;

namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class GfWorkersController : ControllerBase
    {
        private readonly IGfWorkersService _gfWorkersService;

        public GfWorkersController(IGfWorkersService gfWorkersService)
        {
            _gfWorkersService = gfWorkersService;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] GfWorkerRequest gfWorkerRequest, CancellationToken ct)
        {
            int id = 0;
            var gfWorker = GfWorker.Create(id, gfWorkerRequest.Name, gfWorkerRequest.Surname, gfWorkerRequest.StundasMaksa, gfWorkerRequest.Telefons, gfWorkerRequest.Epasts, gfWorkerRequest.Komentars, gfWorkerRequest.Prasmes);
            var gfWorkerResponse = await _gfWorkersService.CreateGfWorker(gfWorker, ct);
            if (gfWorkerResponse.IsCompleted == false)
            {
                return BadRequest(gfWorkerResponse.ErrorMessage);
            }
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "OfficePolicy")]
        public async Task<IActionResult> GetAll()
        {
            var gfWorkerResponse = await _gfWorkersService.GetAllGfWorkers();
            if (gfWorkerResponse.IsCompleted == false)
            {
                return BadRequest(gfWorkerResponse.ErrorMessage);
            }
            var response = gfWorkerResponse.gfWorkersList!.Select(b => new GfWorkerResponse(b.Id, b.Name, b.Surname, b.StundasMaksa, b.Telefons, b.Epasts, b.Komentars, b.Prasmes)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var gfWorkerResponse = await _gfWorkersService.GetGfWorker(id);
            if (gfWorkerResponse.IsCompleted == false)
            {
                return BadRequest(gfWorkerResponse.ErrorMessage);
            }
            var response = new GfWorkerResponse(gfWorkerResponse.GfWorker.Id, gfWorkerResponse.GfWorker.Name, gfWorkerResponse.GfWorker.Surname,
                gfWorkerResponse.GfWorker.StundasMaksa, gfWorkerResponse.GfWorker.Telefons, gfWorkerResponse.GfWorker.Epasts,
                gfWorkerResponse.GfWorker.Komentars, gfWorkerResponse.GfWorker.Prasmes);
            return Ok(response);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] GfWorkerRequest gfWorkerRequest, CancellationToken ct)
        {
            var response = await _gfWorkersService.UpdateGfWorker(id, gfWorkerRequest.Name, gfWorkerRequest.Surname, gfWorkerRequest.StundasMaksa, gfWorkerRequest.Telefons, gfWorkerRequest.Epasts, gfWorkerRequest.Komentars, gfWorkerRequest.Prasmes, ct);
            if (response.IsCompleted == false)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Accepted();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _gfWorkersService.DeleteGfWorker(id, ct);
            if (response.IsCompleted == false)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok();
        }
    }
}
