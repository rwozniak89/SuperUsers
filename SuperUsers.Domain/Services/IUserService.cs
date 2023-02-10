using SuperUsers.Domain.Entities;

namespace SuperUsers.Domain.Services
{
    public interface IUserService
    {
        string GetName();
        Task<User> RegisterUser(User user);
        User GetUser(string email);
        public void UpdateUser(User user);
        void UpdateRefreshToken(User user);
    }
}