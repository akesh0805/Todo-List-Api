namespace TodoList.Api.Dtos;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
}

public class RegisterUser
{
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
}

public class LoginUser
{
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
}
