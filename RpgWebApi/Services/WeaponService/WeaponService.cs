using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RpgWebApi.Data;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Dtos.Weapon;
using RpgWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RpgWebApi.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private DataContext _context;
        private IHttpContextAccessor _accessor;
        private IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor accessor, IMapper mapper)
        {
            _context = context;
            _accessor = accessor;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == GetUserId());

                if (character != null)
                {
                    var weapon = _mapper.Map<Weapon>(newWeapon);

                    weapon.Character = character;
                    await _context.Weapons.AddAsync(weapon);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    serviceResponse.Message = "no such character";
                    serviceResponse.Success = false;
                }

            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
