using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgWebApi.Data;
using RpgWebApi.Dtos.Character;
using RpgWebApi.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RpgWebApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private IMapper _mapper;
        private DataContext _context;
        private IHttpContextAccessor _accessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _context = context;
            _accessor = accessor;
        }

        private int GetUserId() => int.Parse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character character = _mapper.Map<Character>(newCharacter);

                character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

                await _context.Characters.AddAsync(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(
                        await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync()
                        );
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try 
            {
                var deletedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

                if (deletedCharacter != null)
                {
                    _context.Characters.Remove(deletedCharacter);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(
                        await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync()
                        );
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

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();

                serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            { 
                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

                if (dbCharacter != null)
                {
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
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

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            { 
                var newCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id && c.User.Id == GetUserId());

                if (newCharacter != null)
                {
                    foreach (var prop in newCharacter.GetType().GetProperties())
                        prop.SetValue(newCharacter, character.GetType().GetProperty(prop.Name)?.GetValue(character));

                    await _context.SaveChangesAsync();

                    var returnValue = _mapper.Map<GetCharacterDto>(newCharacter);
                    serviceResponse.Data = returnValue;
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
