using DeezNuts.BLL.Interfaces;
using Common.DTOs;
using Common.Models;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace DeezNuts.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(ApplicationDbContext context, ITokenService tokenManager, IMapper mapper)
        {
            _tokenService = tokenManager;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if (await UserExists(model.Username))
            {
                return BadRequest("Username already exists");
            }

            var user  = _mapper.Map<User>(model);

            using var hmach = new HMACSHA512();

            user.UserName = model.Username.ToLower();
            user.PasswordHash = hmach.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            user.PasswordSalt = hmach.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO()
            {
                Username = model.Username.ToLower(),
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
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
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(u => u.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}
