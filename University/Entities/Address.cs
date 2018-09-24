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
        public int Id { get; set; }
 
        public int UserId { get; set; }
       // [ForeignKey("User Id")]
        public virtual User Users { get; set; }
     
        public int CountryId { get; set; }
      //  [ForeignKey("Country Id")]
        public virtual Country Country { get; set; }
       
        public int StateId { get; set; }
       // [ForeignKey("State Id")]
        public virtual State State { get; set; }
        
        public int CityId { get; set; }
        //[ForeignKey("City Id")]
        public virtual City City { get; set; }
        public int ZipCode { get; set; }
  
       
     
       

    }
}