using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ETicaret_Application.UseCases;
using ETicaret_Application.DTOs.OrderDTOs;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderUseCase _createOrder;
        private readonly GetOrderUseCase _getOrder;

        public OrdersController(CreateOrderUseCase createOrder, GetOrderUseCase getOrder)
        {
            _createOrder = createOrder;
            _getOrder = getOrder;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var id = await _createOrder.ExecuteAsync(dto);
            return Ok();
            //return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrdersById(int id)
        {
            var response = await _getOrder.ExecuteByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<GetOrderDto>>>> GetOrders()
        {
            var response = await _getOrder.ExecuteListAsync();
            return Ok(response);
        }

        [HttpGet("withItems/{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderWithItemsDto>>> GetOrderWithItems(int id)
        {
            var response = await _getOrder.ExecuteWithItemsAsync(id);
            return Ok(response);
        }


    }
}
