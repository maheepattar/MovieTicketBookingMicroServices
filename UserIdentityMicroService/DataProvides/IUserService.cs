using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentityMicroService.DBEntities;
using UserIdentityMicroService.DTO;

namespace UserIdentityMicroService.DataProvides
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(string username, string password);

        Task<User> GetUser(string username, string password);
    }
}