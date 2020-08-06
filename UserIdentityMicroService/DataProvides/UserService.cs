using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentityMicroService.DataContext;
using UserIdentityMicroService.DBEntities;
using UserIdentityMicroService.DTO;
using UserIdentityMicroService.Utilities;

namespace UserIdentityMicroService.DataProvides
{
    public class UserService : IUserService
    {
        private readonly UserContext userContext;

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public UserService(UserContext _userCtx)
        {
            this.userContext = _userCtx;
        }
        public async Task<UserDTO> Authenticate(string username, string password)
        {
            var user = await this.GetUser(username, password);

            if (user == null) 
                return null;

            var userDTO = new UserDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId,
                Username = user.Username
            };

            return userDTO;
        }

        public async Task<UserDTO> Create(UserDTO user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new CustomException("Password is required");

            if (userContext.Users.Any(x => x.Username == user.Username))
                throw new CustomException("Username \"" + user.Username + "\" is already exist");

            User userData = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                RoleId = user.RoleId,
                Username = user.Username
            };

            await userContext.Users.AddAsync(userData);
            await userContext.SaveChangesAsync();

            return user;
        }

        public void Delete(int id)
        {
            var user = userContext.Users.Find(id);
            if (user != null)
            {
                userContext.Users.Remove(user);
                userContext.SaveChanges();
            }
        }

        public User GetById(int id)
        {
            return userContext.Users.Find(id);
        }

        public async Task<User> GetUser(string username,string password)
        {
            return await userContext.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
