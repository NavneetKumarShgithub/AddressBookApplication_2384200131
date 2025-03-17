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
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using ModelLayer.Config;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLayer.Service
{
    public class AuthService : IAuthService
    {
        private readonly AddressBookContext _context;
        private readonly IMapper _mapper;
        private readonly string _jwtSecret;
        private readonly IConfiguration _configuration;


        public AuthService(AddressBookContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 🚀 Ensure context is not null
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context;
            _mapper = mapper;
            _jwtSecret = configuration["JwtSettings:Secret"];

            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new ArgumentNullException(nameof(_jwtSecret), "JWT Secret Key is missing in configuration.");
            }
        }


        public async Task<string> Register(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO), "UserDTO is null before mapping.");
            }
            var existingUser = await _context.AddressBook.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (existingUser != null)
                return "User already exists!";

            var user = _mapper.Map<UserEntity>(userDTO);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user.Address = userDTO.Address ?? "Not Provided";
            user.PhoneNumber = userDTO.PhoneNumber ?? "Not Provided";
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

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
            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new ArgumentNullException(nameof(_jwtSecret), "JWT Secret Key is not initialized.");
            }
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
        public async Task<string> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _context.AddressBook.FirstOrDefaultAsync(u => u.Email == forgotPasswordDTO.Email);
            if (user == null)
                return "User not found.";

            // Generate Reset Token
            user.ResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            // Send Email
            SendResetEmail(user.Email, user.ResetToken);

            return "Password reset link has been sent to your email.";
        }

        private void SendResetEmail(string email, string token)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            string host = smtpSettings.Host;
            if (smtpSettings == null)
                throw new Exception("SMTP settings are missing in appsettings.json");

            var resetLink = $"https://yourfrontend.com/reset-password?token={token}";

            MailMessage message = new MailMessage
            {
                From = new MailAddress(smtpSettings.Username),
                Subject = "Password Reset Request",
                Body = $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>",
                IsBodyHtml = true
            };

            message.To.Add(email);

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = smtpSettings.Host;
                smtp.Port = int.Parse(smtpSettings.Port);
                smtp.EnableSsl = bool.Parse(smtpSettings.EnableSSL);
                smtp.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                smtp.Send(message);
            }
        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _context.AddressBook.FirstOrDefaultAsync(u =>
                u.Email == resetPasswordDTO.Email &&
                u.ResetToken == resetPasswordDTO.Token &&
                u.ResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return "Invalid or expired token.";

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();
            return "Password has been reset successfully.";
        }
    }
}
