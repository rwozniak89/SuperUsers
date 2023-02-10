using SuperUsers.DataAccess.Context;
using SuperUsers.Domain.Entities;
using SuperUsers.Domain.Repositories;

namespace SuperUsers.Implementation.Repositories    
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Task<User> Create(User userRegister)
        {
            User userModel = new User();
            userModel.Email = userRegister.Email;
            userModel.PasswordHash = userRegister.PasswordHash;
            userModel.RefreshToken = userRegister.RefreshToken;

            try
            {
                _context.Users.Add(userModel);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Task.FromResult(userModel);

        }

        public User Get(string email)
        {
            User usuario = new User();
            usuario = _context.Users.FirstOrDefault(a => a.Email == email);
            if (usuario is not null)
            {
                return usuario;
            }
            else
            {
                return usuario;
            }


        }

        public void Update(User user)
        {
            var usuario = _context.Users.FirstOrDefault(a => a.Email == user.Email);
            if (usuario is not null)
            {
                usuario = user;
                _context.Users.Update(usuario);
                _context.SaveChanges();
            }
        }

        public void UpdateRefreshToken(User user)
        {
            var usuario = _context.Users.FirstOrDefault(a => a.Email == user.Email);
            if (usuario is not null)
            {
                usuario = user;
                _context.Users.Update(usuario);
                _context.SaveChanges();
            }
        }
    }
}