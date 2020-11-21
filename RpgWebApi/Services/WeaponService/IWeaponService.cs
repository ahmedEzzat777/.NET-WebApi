using RpgWebApi.Dtos.Character;
using RpgWebApi.Dtos.Weapon;
using RpgWebApi.Models;
using System.Threading.Tasks;

namespace RpgWebApi.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
