using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }


        public string AddressLine1 { get; set; }


        public string AddressLine2 { get; set; }

       

         public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }


        public int StateId { get; set; }
       [ForeignKey("StateId")]
        public virtual State State { get; set; }



        public int CityId { get; set; }
       [ForeignKey("CityId")]
       public virtual City City { get; set; }


        public int ZipCode { get; set; }


        public virtual ICollection<User> Users { get; set; }




    }
}