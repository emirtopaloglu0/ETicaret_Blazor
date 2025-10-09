using ETicaret_Application.DTOs.OrderDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using ETicaret_UI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryCompaniesController : ControllerBase
    {
        private readonly IDeliveryCompanyRepository _companyRepository;

        public DeliveryCompaniesController(IDeliveryCompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // GET: api/<DeliveryCompaniesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<DeliveryCompany>>>> Get()
        {
            var response = await _companyRepository.GetDeliveryCompanies();
            return Ok(response);
        }

        // GET api/<DeliveryCompaniesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryCompany>> Get(int id)
        {
            var response = await _companyRepository.GetDeliveryCompanyById(id);
            return Ok(response);
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        // POST api/<DeliveryCompaniesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string Name)
        {
            DeliveryCompany deliveryCompany = new DeliveryCompany
            {
                Name = Name
            };
            await _companyRepository.AddAsync(deliveryCompany);
            return Ok();
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        // PUT api/<DeliveryCompaniesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string Name)
        {
            var response = await _companyRepository.UpdateCompany(id, Name);
            return Ok(response);
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        // DELETE api/<DeliveryCompaniesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _companyRepository.DeleteCompany(id);
            return Ok();
        }

        [HttpGet("GetCompanyByUser/{userId}")]
        public async Task<ActionResult<Deliverer>> GetCompanyByUserId(int userId)
        {
            var response = await _companyRepository.GetDeliveryCompanyByUserId(userId);
            if (response is null) return BadRequest();

            return Ok(response);
        }
    }
}
