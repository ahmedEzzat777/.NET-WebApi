using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgWebApi.Dtos.CharacterSkill;
using RpgWebApi.Services;
using System.Threading.Tasks;

namespace RpgWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterSkillController : ControllerBase
    {
        private readonly ICharacterSkillService _service;

        public CharacterSkillController(ICharacterSkillService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillDto characterSkillDto)
        {
            var response = await _service.AddCharacterSkill(characterSkillDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
