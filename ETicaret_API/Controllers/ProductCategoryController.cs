using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryUseCase _categoryUseCase;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public ProductCategoryController(ProductCategoryUseCase categoryUseCase, IProductCategoryRepository productCategoryRepository)
        {
            _categoryUseCase = categoryUseCase;
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryDto dto)
        {
            var test = await _categoryUseCase.ExecuteCreate(dto);
            if (test == 0 || test is nuint)
            {
                return BadRequest();
            }
            return Ok(test);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<GetProductCategoryDto>>>> GetCategories()
        {
            var response = await _categoryUseCase.ExecuteListAsync();
            if (response is null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetProductCategoryDto>>> GetCategoryById(int id)
        {
            var response = await _categoryUseCase.ExecuteGetById(id);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            var response = await _categoryUseCase.ExecuteUpdateCategory(id, request.Name, request.Description);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _productCategoryRepository.DeleteCategory(id);
            return Ok();
        }

        public record UpdateCategoryRequest(string Name, string Description);

    }
}
