using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class IdentityRole
    {
        
        public int IdentityRoleId { get; set; } //primary key
        public string IdentityRoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
