using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Models;

namespace RpgWebApi.Services.CharacterService
{
    public class MockCharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
            new Character(){ Id = 1, Name = "Sam"}
        };
        private IMapper _mapper;

        public MockCharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var deletedCharacterIndex = characters.FindIndex(c => c.Id == id);

                characters.RemoveAt(deletedCharacterIndex);

                serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters.Where(c => c.User.Id == userId));

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters[id]);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var newCharacter = characters.FirstOrDefault(c => c.Id == character.Id);

                foreach (var prop in newCharacter.GetType().GetProperties())
                    prop.SetValue(newCharacter, character.GetType().GetProperty(prop.Name)?.GetValue(character));

                var returnValue = _mapper.Map<GetCharacterDto>(newCharacter);

                serviceResponse.Data = returnValue;
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
