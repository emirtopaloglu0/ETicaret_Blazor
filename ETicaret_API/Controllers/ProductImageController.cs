using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageController(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }


        // GET: api/<ProductController>
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            var response = await _productImageRepository.GetByProductId(productId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] List<ProductImageDTO> productImageDTO)
        {
            var response = await _productImageRepository.AddAsync(productImageDTO);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(int productId, [FromBody] List<ProductImageDTO> productImages)
        {
            var response = await _productImageRepository.UpdateAsync(productId, productImages);
            if (!response)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var response = await _productImageRepository.DeleteAsync(imageId);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet("IsImagesExists/{productId}")]
        public async Task<IActionResult> IsImagesAlreadyExist(int productId)
        {
            var response = await _productImageRepository.IsImageAlreadyExist(productId);
            if (!response)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
