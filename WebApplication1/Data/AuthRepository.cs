using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
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
                serviceResponse.Data = $"{user.Id} token";
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
    }
}
