using Microsoft.AspNetCore.Mvc;
using RpgWebApi.Dtos.Fight;
using RpgWebApi.Services.FightService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _service;

        public FightController(IFightService service)
        {
            _service = service;
        }

        [HttpPost("Weapon")]
        public async Task<IActionResult> WeaponAttack(WeaponAttackDto weaponAttackDto)
        {
            var response = await _service.WeaponAttack(weaponAttackDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("Skill")]
        public async Task<IActionResult> SkillAttack(SkillAttackDto skillAttackDto)
        {
            var response = await _service.SkillAttack(skillAttackDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Fight(FightRequestDto fightRequestDto)
        {
            var response = await _service.Fight(fightRequestDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        public async Task<IActionResult> GetHighscores()
        {
            var response = await _service.GetHighscores();

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

    }
}
