using ETicaret_Application.DTOs.CartDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;


        public CartItemController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        // GET: api/<CarItemController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }

        // GET api/<CarItemController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<CartItemDTO>>> Get(int id)
        {
            var response = await _cartItemRepository.GetByCartItemsByUserId(id);
            if (response == null) return BadRequest(response);
            return Ok(response);
        }

        // POST api/<CarItemController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CartItemDTO cartItemDTO)
        {
            var response = await _cartItemRepository.AddAsync(cartItemDTO);
            if (!response) return BadRequest(response);
            return Ok(response);
        }

        // PUT api/<CarItemController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] int quantity)
        {
            var response = await _cartItemRepository.UpdateCategory(id, quantity);
            if (!response) return BadRequest(response);
            return Ok(response);
        }

        // DELETE api/<CarItemController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _cartItemRepository.DeleteCategory(id);
            if (!response) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("IsUserHaveCartItems/{userId}")]
        public async Task<ActionResult> IsUserHaveCartItems(int userId)
        {
            var response = await _cartItemRepository.IsUserHaveCartItems(userId);
            if (!response) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<ActionResult> ClearCartItems(int userId)
        {
            var response = await _cartItemRepository.ClearCart(userId);
            if (!response) return BadRequest(response);
            return Ok(response);
        }
    }
}
