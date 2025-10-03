using ETicaret_Application.DTOs.OrderDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using ETicaret_UI.Enums;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderUseCase _createOrder;
        private readonly GetOrderUseCase _getOrder;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(CreateOrderUseCase createOrder, GetOrderUseCase getOrder,
            IOrderRepository orderRepository)
        {
            _createOrder = createOrder;
            _getOrder = getOrder;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var id = await _createOrder.ExecuteAsync(dto, userId);
            return Ok(id);
            //return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrdersById(int id)
        {
            var response = await _getOrder.ExecuteByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("byUser")]
        public async Task<ActionResult<IEnumerable<List<GetOrderDto>>>> GetOrders(int id)
        {
            int userId = id;
            if (User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "customer"))
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }
            var response = await _getOrder.ExecuteListAsync(userId);
            return Ok(response);
        }

        [HttpGet("withItems/{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderWithItemsDto>>> GetOrderWithItems(int id)
        {
            var response = await _getOrder.ExecuteWithItemsAsync(id);
            return Ok(response);
        }

        [Authorize(Roles = $"{UserRoleEnums.Deliverer},{UserRoleEnums.Admin},{UserRoleEnums.Customer}")]
        [HttpPut("updateCargo/{id}")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            //Kargocular comboboxtan seçerek yapar burayı.
            await _orderRepository.UpdateCargoStatus(id, status);
            return Ok();
        }

        [HttpGet("GetByCompanyId/{id}")]
        public async Task<ActionResult<IEnumerable<List<GetOrderDto>>>> GetOrdersByCompanyId(int id)
        {
            var response = await _orderRepository.GetByCompanyId(id);
            return Ok(response);
        }
    }
}
