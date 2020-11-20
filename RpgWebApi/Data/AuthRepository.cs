using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RpgWebApi.Models;

namespace RpgWebApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;
        private IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            if (user == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "invalid user or password";
            }
            else if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            { 
                serviceResponse.Data = CreateToken(user);
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "invalid user or password";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();

            if (await UserExists(user.UserName))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "user already there";
                return serviceResponse;
            }


            CreatePassword(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            serviceResponse.Data = user.Id;

            return serviceResponse;
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            bool result = false;
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                if (Enumerable.SequenceEqual(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), passwordHash))
                    result = true;
            }

            return result;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            { 
                 Subject = new ClaimsIdentity(claims),
                 Expires = DateTime.Now.AddDays(1),
                 SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
