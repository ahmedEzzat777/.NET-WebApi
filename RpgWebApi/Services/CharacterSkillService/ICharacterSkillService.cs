using RpgWebApi.Dtos.Character;
using RpgWebApi.Dtos.CharacterSkill;
using RpgWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgWebApi.Services
{
    public interface ICharacterSkillService
    {
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto characterSkill);
    }
}
