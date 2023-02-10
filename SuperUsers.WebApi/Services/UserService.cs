using SuperUsers.Domain.Entities;
using SuperUsers.Domain.Repositories;
using SuperUsers.Domain.Services;
using System.Security.Claims;

namespace SuperUsers.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result!;
        }

        public async Task<User> RegisterUser(User user)
        {
            var save = await _userRepository.Create(user);

            return save;
        }

        public User GetUser(string email)
        {
            var usuario = _userRepository.Get(email);
            return usuario;
        }


        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void UpdateRefreshToken(User user)
        {
            _userRepository.UpdateRefreshToken(user);
        }
    }
}
