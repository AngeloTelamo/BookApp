using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserListViewModel
    {
        public List<UserViewModel> UserList { get; set; }
        public UserListFilterModel Filters { get; set; }

        public class UserListFilterModel
        {
            public string UserId { get; set; }

            [DisplayName("Name")]
            public string Name { get; set; }
        }
    }
}
