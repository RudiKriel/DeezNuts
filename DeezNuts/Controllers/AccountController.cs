using BLL.Interfaces;
using Common.DTOs;
using Common.Models;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DeezNuts.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenManager _tokenManager;

        public AccountController(ApplicationDbContext context, ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if (await UserExists(model.Username))
            {
                return BadRequest("Username already exists");
            }

            using var hmach = new HMACSHA512();

            var user = new User()
            {
                UserName = model.Username.ToLower(),
                PasswordHash = hmach.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                PasswordSalt = hmach.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO()
            {
                Username = model.Username.ToLower(),
                Token = _tokenManager.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmach = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmach.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }

            return new UserDTO()
            {
                Username = user.UserName,
                Token = _tokenManager.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}
