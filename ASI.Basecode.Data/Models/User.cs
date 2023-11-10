using System;
using System.Collections.Generic;
using System.Data;

namespace ASI.Basecode.Data.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int RoleId { get; set; } // Foreign key to IdentityRoles
      //  public ICollection<IdentityRole> IdentityRoles { get; set; }


    }
}
