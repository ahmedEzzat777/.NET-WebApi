using AutoMapper;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Dtos.Fight;
using RpgWebApi.Dtos.Skill;
using RpgWebApi.Dtos.Weapon;
using RpgWebApi.Models;
using System.Linq;

namespace RpgWebApi
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>()
                .ForMember(dto => dto.Skills, c=> c.MapFrom(c => c.CharacterSkills.Select(cs => cs.Skill)));

            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighscoreDto>();
        }
    }
}
