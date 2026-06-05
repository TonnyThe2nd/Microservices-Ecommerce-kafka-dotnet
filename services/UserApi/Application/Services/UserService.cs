public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task CreateUserAsync(CreateUserDto request)
    {
        var userExists = await _userRepository.GetByEmailAsync(request.Email);

        if (userExists != null)
        {
            throw new Exception("Usuário com este email já existe.");
        }
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            BirthDate = request.BirthDate,
            Phone = request.Phone
        };
        await _userRepository.CreateAsync(user);
    }

    public async Task UpdateUserAsync(Guid id, User request)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        user.Name = request.Name;
        user.Email = request.Email;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        user.BirthDate = request.BirthDate;
        user.Phone = request.Phone;

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        await _userRepository.DeleteAsync(id);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        return user;
    }

    private async Task<bool> VerifyPasswordAsync(string password, string userPasswordHash )
    {

        return BCrypt.Net.BCrypt.Verify(password, userPasswordHash);
    }

    public  async Task<bool> ValidateUserCredentialsAsync(string password, string userPasswordHash)
    {
        return  await VerifyPasswordAsync( password, userPasswordHash);
    }

}