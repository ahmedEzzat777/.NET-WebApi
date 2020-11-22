using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Services.CharacterService;

namespace RpgWebApi.Controllers
{
    [Authorize(Roles = "Player,Admin")]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        //private static CharacterService _service = new CharacterService();

        ICharacterService _service;

        public CharacterController(ICharacterService service)
        {
            _service = service;
        }

        //[AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {

            var serviceResponse = await _service.GetCharacter(id);

            if (!serviceResponse.Success)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _service.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = await _service.UpdateCharacter(character);

            if (!serviceResponse.Success)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var serviceResponse = await _service.DeleteCharacter(id);

            if (!serviceResponse.Success)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }
    }
}
