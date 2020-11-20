using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dtos.Character;
using WebApplication1.Models;

namespace WebApplication1.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        //private static List<Character> characters = new List<Character>{
        //    new Character(),
        //    new Character(){ Id = 1, Name = "Sam"}
        //};

        private IMapper _mapper;
        private DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(await _context.Characters.ToListAsync());
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var deletedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

                _context.Characters.Remove(deletedCharacter);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(await _context.Characters.ToListAsync());
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

            var dbCharacters = await _context.Characters.Where(c => c.User.Id == userId).ToListAsync();

            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var newCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);

                foreach (var prop in newCharacter.GetType().GetProperties())
                    prop.SetValue(newCharacter, character.GetType().GetProperty(prop.Name)?.GetValue(character));

                await _context.SaveChangesAsync();

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
