using AutoMapper;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Dtos.Weapon;
using RpgWebApi.Models;

namespace RpgWebApi
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<Weapon, GetWeaponDto>();
        }
    }
}
