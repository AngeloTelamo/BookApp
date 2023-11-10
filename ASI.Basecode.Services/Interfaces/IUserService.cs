using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string userid, string password, ref User user);
        UserEditViewModel GetUser(string userId);
        UserListViewModel GetUserList(UserListViewModel model);
       // IQueryable<User> GetUsersWithRole();
        void AddUser(UserViewModel model);
        void UpdateUser(UserEditViewModel model);
        void DeleteUser(string userId);
    }
}
