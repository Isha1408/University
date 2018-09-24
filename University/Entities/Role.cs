using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" Role")]
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        [Required]
        [MaxLength(255)]
        public string RoleName { get; set; }

        public virtual  ICollection<UserInRole> UserInRole { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}