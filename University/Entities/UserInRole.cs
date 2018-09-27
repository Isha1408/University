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

      
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

     
      public int UserId { get; set; }
        [ForeignKey("UserId")]

        public virtual User User{ get; set; }
    }
} 