using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" UserInRole")]
    public class UserInRole
    {

        [Required]
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public string UserId { get; set; }
    }
}