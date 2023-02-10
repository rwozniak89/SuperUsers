using SuperUsers.Domain.Entities;

namespace SuperUsers.Domain.Repositories
{
    public interface IUserRepository
    {
        
        Task<User> Create(User user);
        User Get(string email);

        void Update(User entity);

        void UpdateRefreshToken(User user);
    }
}