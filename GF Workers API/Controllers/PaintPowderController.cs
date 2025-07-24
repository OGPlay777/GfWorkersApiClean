using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Models;

namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PaintPowderController : ControllerBase
    {
        private readonly IPaintPowderService _paintPowdertService;

        public PaintPowderController(IPaintPowderService paintPowdertService)
        {
            _paintPowdertService = paintPowdertService;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] PaintPowderRequest paintPowderRequest, CancellationToken ct)
        {
            int id = 0;
            var paintPowder = PaintPowder.Create(id, paintPowderRequest.PaintCode, paintPowderRequest.PaintPriceKG);
            var paintPowderResponse = await _paintPowdertService.CreatePaintPowder(paintPowder, ct);
            if (paintPowderResponse.IsCompleted == false)
            {
                return BadRequest(paintPowderResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "OfficePolicy")]
        public async Task<IActionResult> GetAll()
        {
            var paintPowderResponse = await _paintPowdertService.GetAllPaintPowders();
            if (paintPowderResponse.IsCompleted == false)
            {
                return BadRequest(paintPowderResponse.ErrorMessage);
            }
            var response = paintPowderResponse.paintPowderList!.Select(b => new PaintPowderResponse(b.Id, b.PaintCode, b.PaintPriceKG)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var paintPowderResponse = await _paintPowdertService.GetPaintPowder(id);
            if (paintPowderResponse.IsCompleted == false)
            {
                return BadRequest(paintPowderResponse.ErrorMessage);
            }
            var response = new PaintPowderResponse(paintPowderResponse.PaintPowder!.Id, paintPowderResponse.PaintPowder.PaintCode, paintPowderResponse.PaintPowder.PaintPriceKG);
            return Ok(response);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] PaintPowderRequest paintPowderRequest, CancellationToken ct)
        {
            var paintPowderResponse = await _paintPowdertService.UpdatePaintPowder(id, paintPowderRequest.PaintCode, paintPowderRequest.PaintPriceKG, ct);
            if (paintPowderResponse.IsCompleted == false)
            {
                return BadRequest(paintPowderResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var paintPowderResponse = await _paintPowdertService.DeletePaintPowder(id, ct);
            if (paintPowderResponse.IsCompleted == false)
            {
                return BadRequest(paintPowderResponse.ErrorMessage);
            }
            return Accepted();
        }
    }
}
