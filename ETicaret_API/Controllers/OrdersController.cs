using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ETicaret_Application.UseCases;
using ETicaret_Application.DTOs;

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
            return Ok(id);
            //return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrdersById(int id)
        {
            var test = await _getOrder.ExecuteByIdAsync(id);
            return Ok(test);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<GetOrderDto>>>> GetOrders()
        {
            var test = await _getOrder.ExecuteListAsync();
            return Ok(test);
        }


    }
}
