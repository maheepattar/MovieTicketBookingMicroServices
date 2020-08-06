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
        User GetById(int id);
        Task<UserDTO> Create(UserDTO user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }
}