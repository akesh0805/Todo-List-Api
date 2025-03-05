using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Dtos;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class UsersController(UsersService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUser user)
    {
        var result = await service.RegisterUserAsync(user);
        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginUser user)
    {
        var result = await service.LoginUserAsync(user);
        return Ok(result);
    }
}