using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSubCategoryController : ControllerBase
    {

        private readonly IProductSubCategoryRepository _productSubCategoryRepository;

        public ProductSubCategoryController(IProductSubCategoryRepository productSubCategoryRepository)
        {
            _productSubCategoryRepository = productSubCategoryRepository;
        }

        // GET: api/<ProductSubCategoryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<ProductSubCategory>>>> Get()
        {
            var response = await _productSubCategoryRepository.GetSubCategoriesAsync();
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        // GET api/<ProductSubCategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSubCategory>> Get(int id)
        {
            var response = _productSubCategoryRepository.GetById(id);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<ActionResult<List<ProductSubCategory>>> GetByCategory(int categoryId)
        {
            var response = await _productSubCategoryRepository.GetByCategoryId(categoryId);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        // POST api/<ProductSubCategoryController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductSubCategory productSubCategory)
        {
            var response = await _productSubCategoryRepository.AddAsync(productSubCategory);
            if (!response)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        // PUT api/<ProductSubCategoryController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductSubCategory productSubCategory)
        {
            var response = await _productSubCategoryRepository.UpdateCategory(id,
                productSubCategory.CategoryId, productSubCategory.Name, productSubCategory.Description);
            if (!response)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        // DELETE api/<ProductSubCategoryController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _productSubCategoryRepository.DeleteCategory(id);
            if (!response)
            {
                return BadRequest();
            }
            return Ok(response);
        }
    }
}
