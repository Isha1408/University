using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University.Entities;

namespace University.Models
{
    public class UserModel
    {
        public User User { get; set; }
         public Address Address { get; set; }

    }
}