using ETicaret_Application.DTOs;
using ETicaret_Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryUseCase _categoryUseCase;
        public ProductCategoryController(ProductCategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
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
    }
}
