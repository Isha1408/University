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
      
      
        public string UserId { get; set; }
        public  virtual Role Roles { get; set; }
        public virtual User Users{ get; set; }
    }
} 