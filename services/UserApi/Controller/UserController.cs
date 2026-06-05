using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;

    public UserController(UserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto? createUserDto)
    {
        if (createUserDto == null)
            return BadRequest("Informações não recebidas.");


        await _userService.CreateUserAsync(createUserDto);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto? loginDto)
    {
        if (loginDto == null)
            return BadRequest("Informações não recebidas.");

        var user = await _userService.GetByEmailAsync(loginDto.Email);
        if (!await _userService.ValidateUserCredentialsAsync(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized();
        }
        var token = await _authService.AuthenticateAsync(user);
        return Ok(new { Token = token });
    }
}