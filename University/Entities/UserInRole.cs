using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace University.Entities
{
    [Table(" UserInRole")]
    public class UserInRole
    {
      
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Role Id")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

       // [ForeignKey("User Id")]
        public string UserId { get; set; }
      
        public virtual User User{ get; set; }
    }
} 