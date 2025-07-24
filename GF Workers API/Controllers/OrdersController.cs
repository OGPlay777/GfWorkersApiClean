using GF_Workers_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Models;
using OperationWorker.Core.UserRoles;

namespace GF_Workers_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "OfficePolicy")]
        public async Task<IActionResult> Create([FromBody] OrderRequest orderRequest, CancellationToken ct)
        {
            int id = 0;
            var order = Order.Create(id, orderRequest.Pasutitajs, orderRequest.PienemsanasDatums, orderRequest.NodosanasDatums,
                orderRequest.PasTelefons, orderRequest.PasEpasts, orderRequest.Pasutijums, orderRequest.Pienemejs, orderRequest.RekinsKlientam,
                orderRequest.Pasizmaksa, orderRequest.Pelna, orderRequest.Komentars, orderRequest.Operacijas, orderRequest.Image, orderRequest.Status);
            var orderResponse = await _ordersService.CreateOrder(order, ct);
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [HasRole("Worker")]
        public async Task<IActionResult> GetAll()
        {
            var orderResponse = await _ordersService.GetAllOrders();
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            var response = orderResponse.ordersList.Select(b => new OrderResponse(b.Id, b.Pasutitajs, b.PienemsanasDatums, b.NodosanasDatums,
                b.PasTelefons, b.PasEpasts, b.Pasutijums, b.Pienemejs, b.RekinsKlientam, b.Pasizmaksa, b.Pelna, b.Komentars, b.Operacijas, b.Image, b.Status)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var orderResponse = await _ordersService.GetOrder(id);
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            var response = new OrderResponse(orderResponse.Order.Id, orderResponse.Order.Pasutitajs, orderResponse.Order.PienemsanasDatums,
                orderResponse.Order.NodosanasDatums, orderResponse.Order.PasTelefons, orderResponse.Order.PasEpasts, orderResponse.Order.Pasutijums,
                orderResponse.Order.Pienemejs, orderResponse.Order.RekinsKlientam, orderResponse.Order.Pasizmaksa, orderResponse.Order.Pelna,
                orderResponse.Order.Komentars, orderResponse.Order.Operacijas, orderResponse.Order.Image, orderResponse.Order.Status);
            return Ok(response);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderRequest orderRequest, CancellationToken ct)
        {
            var orderResponse = await _ordersService.UpdateOrder(id, orderRequest.Pasutitajs, orderRequest.PienemsanasDatums,
                orderRequest.NodosanasDatums, orderRequest.PasTelefons, orderRequest.PasEpasts, orderRequest.Pasutijums, orderRequest.Pienemejs,
                orderRequest.RekinsKlientam, orderRequest.Pasizmaksa, orderRequest.Pelna, orderRequest.Komentars, orderRequest.Operacijas,
                orderRequest.Image, orderRequest.Status, ct);
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var orderResponse = await _ordersService.DeleteOrder(id, ct);
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            return Accepted();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> GetByWorkerSkills(int id)
        {
            var orderResponse = await _ordersService.GetOrdersByWorkerSkills(id);
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            var response = orderResponse.ordersList.Select(b => new OrderResponse(b.Id, b.Pasutitajs, b.PienemsanasDatums, b.NodosanasDatums,
                b.PasTelefons, b.PasEpasts, b.Pasutijums, b.Pienemejs, b.RekinsKlientam, b.Pasizmaksa, b.Pelna, b.Komentars, b.Operacijas, b.Image, b.Status)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<IActionResult> GetAllInProcess()
        {
            var orderResponse = await _ordersService.GetOrdersInProcess();
            if (orderResponse.IsCompleted == false)
            {
                return BadRequest(orderResponse.ErrorMessage);
            }
            var response = orderResponse.ordersList.Select(b => new OrderResponse(b.Id, b.Pasutitajs, b.PienemsanasDatums, b.NodosanasDatums,
                b.PasTelefons, b.PasEpasts, b.Pasutijums, b.Pienemejs, b.RekinsKlientam, b.Pasizmaksa, b.Pelna, b.Komentars, b.Operacijas, b.Image, b.Status)).ToList();
            return Ok(response);
        }
    }
}
