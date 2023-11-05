using Kviz.Data;
using Kviz.Migrations.Tables;
using Kviz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kviz.Services
{
    public class AccountService: IAccountService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(Login input)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName!.ToLower().Equals(input.UserName!.ToLower()));
            if (user == null)
            {
                return null!;
            }

            if(!BCrypt.Net.BCrypt.Verify(input.Password, user.Password))
            {
                return null!;
            }

            return GenerateToken(user.Id);
        }

        private string GenerateToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddSeconds(10),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool flag, string message)> RegisterAsync(User user)
        {
            //Check if the username is already in use
            var query = await _context.Users.FirstOrDefaultAsync(u => u.UserName!.ToLower().Equals(user.UserName!.ToLower()));
            if(query is not null)
            {
                return (false, "This username is already registered");
            }

            //Create new user in db
            var entity = _context.Users.Add(
                new UserTable()
                {
                    UserName = user.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                }).Entity;
            await _context.SaveChangesAsync();
            return (true, "Successfully registered!");
        }
    }
}
