using AutoMapper;
using BusinessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Service
{
    public class AuthService : IAuthService
    {
        private readonly AddressBookContext _context;
        private readonly IMapper _mapper;
        private readonly string _jwtSecret;


        public AuthService(AddressBookContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _jwtSecret = configuration["JwtSettings:Secret"];
        }

        public async Task<string> Register(UserDTO userDTO)
        {
            var existingUser = await _context.AddressBook.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (existingUser != null)
                return "User already exists!";

            var user = _mapper.Map<UserEntity>(userDTO);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user.Address = userDTO.Address ?? "Not Provided";
            user.PhoneNumber = userDTO.PhoneNumber ?? "Not Provided";

            _context.AddressBook.Add(user);
            await _context.SaveChangesAsync();
            return "User registered successfully!";
        }

        public async Task<string> Login(UserDTO userDTO)
        {
            var user = await _context.AddressBook.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
                return "Invalid credentials!";

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret); ;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
