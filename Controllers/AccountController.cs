using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Data;

namespace API.Controllers
{
    public class AccountController (DataContext context, ITokenService tokenService): BaseApiController
    {
        [HttpPost("register")] //account register
        public async Task<ActionResult<UserDto>> Register (RegisterDto registerDto)
        {
            if (await UserExist(registerDto.UserName)) return BadRequest("Username is already exist..!!");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            context.AppUsers.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
        {
            
            var user = await context.AppUsers.FirstOrDefaultAsync(x => x.UserName 
                    == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length; i++ )
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");

            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };

        }



        private async Task<bool> UserExist (string username)
        {
            return await context.AppUsers.AnyAsync(a => a.UserName.ToLower() == username.ToLower());
        }
    }
}
