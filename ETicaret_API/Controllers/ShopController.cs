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

    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;

        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        // GET: api/<ShopController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<Shop>>>> Get()
        {
            var response = await _shopRepository.GetShops();
            return Ok(response);
        }

        // GET api/<ShopController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> Get(int id)
        {
            var response = await _shopRepository.GetByIdShop(id);
            return Ok(response);
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        // POST api/<ShopController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostRequest postRequest)
        {
            Shop shop = new Shop
            {
                Name = postRequest.Name,
                Description = postRequest.Description
            };
            await _shopRepository.AddAsync(shop);
            return Ok("İşlem Başarılı");
        }
        [Authorize(Roles = UserRoleEnums.Admin)]
        // POST api/<ShopController>
        [HttpPost("shopUser")]
        public async Task<ActionResult> AddShopUser([FromBody] ShopUserRequest shopUserRequest)
        {
            ShopUser shopUser = new ShopUser
            {
                UserId = shopUserRequest.userId,
                ShopId = shopUserRequest.shopId
            };
            var response = await _shopRepository.AddShopUser(shopUser);
            if (response) return Ok("Kayıt Başarılı");
            else return BadRequest("Bu kullanıcı, mağaza kullanıcısı değil");
        }

        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        // PUT api/<ShopController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PostRequest postRequest)
        {
            Shop shop = new Shop
            {
                Name = postRequest.Name,
                Description = postRequest.Description
            };
            await _shopRepository.UpdateAsync(id, shop);
            return Ok();
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        // DELETE api/<ShopController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _shopRepository.DeleteAsync(id);
            return Ok("İşlem Başarılı");
        }

        [Authorize(Roles = $"{UserRoleEnums.Admin},{UserRoleEnums.ShopUser}")]
        [HttpGet("GetShopUserById/{userId}")]
        public async Task<ActionResult<ShopUser>> GetShopUser(int userId)
        {
            var response = await _shopRepository.GetShopUser(userId);
            ShopUser shopUser = new ShopUser
            {
                Id = response.Id,
                UserId = response.UserId,
                ShopId = response.ShopId
            };
            return shopUser;
        }
    }
}

public record PostRequest(string Name, string Description);
public record ShopUserRequest(int userId, int shopId);

