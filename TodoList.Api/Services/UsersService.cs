using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoList.Api.Data;
using TodoList.Api.Dtos;

namespace TodoList.Api.Services;

public class UsersService(TodoListDbContext context, IConfiguration configuration)
{
    public async Task<LoginResponse> LoginUserAsync(TodoList.Api.Dtos.LoginUser user)
    {
        var getUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (getUser is null)
            return new LoginResponse(false, "Foydalanuvchi topilmadi");

        bool checkPassword = BCrypt.Net.BCrypt.Verify(user.PasswordHash, getUser.PasswordHash);
        if (checkPassword)
            return new LoginResponse(true, "Login successfully", JwtTokenGenerate(getUser));
        else
            return new LoginResponse(false, "Noto'gri malumotlar");
    }

    public async Task<RegistrationResponse> RegisterUserAsync(TodoList.Api.Dtos.RegisterUser user)
    {
        var getUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (getUser is not null)
            return new RegistrationResponse(false, "Bu foydalanuvchi allaqachon mavjud");

        var newUser = new TodoList.Api.Entites.User
        {
            Username = user.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash)
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new RegistrationResponse(true, "Foydalanuvchi muvaffaqiyatli ro'yxatdan o'tdi");
    }

    private string JwtTokenGenerate(TodoList.Api.Entites.User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username!),
        };

        var token = new JwtSecurityToken(
            audience: configuration["JwtConfig:Audience"],
            issuer: configuration["JwtConfig:Issuer"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}