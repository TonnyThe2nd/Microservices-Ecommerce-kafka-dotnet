public interface IUserRepository
{
    public Task<User> GetByIdAsync(Guid id);
    public Task<User> GetByEmailAsync(string email);
    public Task CreateAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(Guid id);

}