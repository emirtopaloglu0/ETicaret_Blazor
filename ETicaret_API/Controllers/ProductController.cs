using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.UseCases;
using ETicaret_Core.Entities;
using ETicaret_UI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly ProductDescriptionUseCase _descUseCase;

        public ProductController(IProductRepository productRepository, ProductDescriptionUseCase descUseCase)
        {
            _productRepository = productRepository;
            _descUseCase = descUseCase;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _productRepository.GetAllProduct();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _productRepository.GetByIdAsync(id);
            if (response == null) { return NotFound(); }
            return Ok(response);
        }

        [HttpGet("byCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var response = await _productRepository.GetByCategoryIdAsync(categoryId);
            if (response == null) { return NotFound(); }
            return Ok(response);
        }

        [HttpGet("byShop/{shopId}")]
        public async Task<IActionResult> GetByShop(int shopId)
        {
            var response = await _productRepository.GetByShopIdAsync(shopId);
            if (response == null) { return NotFound(); }
            return Ok(response);
        }

        // POST api/<ProductController>
        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] _product _product)
        {
            //shopUser eklerken, UI tarafında, kayıtlı olduğu shop'un ID'sini alacağız.
            Product product = new Product
            {
                CategoryId = _product.CategoryId,
                SubCategoryId = _product.SubCategoryId,
                ShopId = _product.ShopId,
                Name = _product.Name,
                Description = _product.Description,
                Stock = _product.Stock,
                Price = _product.Price,
                ImageUrl = _product.ImageUrl,
                IsDelete = false
            };

            var response = await _productRepository.AddAsync(product);
            //if (response <= 0)
            //{
            //    return BadRequest("Kaydetme İşlemi Başarısız!");
            //}
            return Ok(response);
        }

        // PUT api/<ProductController>/5
        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] _product _product)
        {
            int shopUserId = 0;
            if (User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "shopUser"))
            {
                shopUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }
            Product product = new Product
            {
                CategoryId = _product.CategoryId,
                SubCategoryId = _product.SubCategoryId,
                ShopId = _product.ShopId,
                Name = _product.Name,
                Description = _product.Description,
                Stock = _product.Stock,
                Price = _product.Price,
                ImageUrl = _product.ImageUrl,
                IsDelete = _product.isDelete
            };
            var response = await _productRepository.UpdateAsync(id, product, shopUserId);
            if (!response) { return BadRequest(); }
            return Ok("Güncelleme Başarılı");
        }

        // DELETE api/<ProductController>/5
        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productRepository.DeleteAsync(id);
            if (!response) { return BadRequest(); }
            return Ok("Silme başarılı");
        }

        [HttpGet("generate-description/{id}")]
        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        public async Task<IActionResult> GenerateDescription(int id)
        {
            try
            {
                var desc = await _descUseCase.ExecuteGenerateDescriptionAsync(id);
                return Ok(desc);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Bulunamadı");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public record _product(int CategoryId, int SubCategoryId, int ShopId, string Name, string Description,
            int Stock, decimal Price, string ImageUrl, bool isDelete = false);
    }
}
