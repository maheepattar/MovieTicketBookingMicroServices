using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentityMicroService.DataContext;
using UserIdentityMicroService.DBEntities;
using UserIdentityMicroService.DTO;

namespace UserIdentityMicroService.DataProvides
{
    public class UserService : IUserService
    {
        private readonly UserContext userContext;

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

        public async Task<User> GetUser(string username,string password)
        {
            return await userContext.Users.SingleOrDefaultAsync(x => x.Username == username);
        }       
    }
}
