public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGnerator _jwtTokenGnerator;

    public AuthService(IUserRepository userRepository, IJwtTokenGnerator jwtTokenGnerator)
    {
        _userRepository = userRepository;
        _jwtTokenGnerator = jwtTokenGnerator;
    }

    public async Task<string> AuthenticateAsync(User user)
    {
        return _jwtTokenGnerator.GenerateToken(user);
    }
}