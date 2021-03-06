﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RpgWebApi.Data;
using RpgWebApi.Dtos.User;

namespace RpgWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            var response = await _authRepository.Register(new Models.User() { UserName = request.UserName }, request.Password);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.UserName, request.Password);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
