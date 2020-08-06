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
        UserDTO Authenticate(string username, string password);

        User GetUser(string username, string password);
    }
}
