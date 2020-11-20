using System.Collections.Generic;
using System.Threading.Tasks;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Models;

namespace RpgWebApi.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId);
        Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id);
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character);
        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    }
}
