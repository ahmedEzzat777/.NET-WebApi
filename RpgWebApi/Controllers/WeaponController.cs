using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgWebApi.Dtos.Weapon;
using RpgWebApi.Services.WeaponService;
using System.Threading.Tasks;

namespace RpgWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _service;

        public WeaponController(IWeaponService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = await _service.AddWeapon(newWeapon);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
