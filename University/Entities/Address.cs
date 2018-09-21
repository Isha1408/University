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
        [ForeignKey("User Id")]
        public int UserId { get; set; }
        [ForeignKey("Country Id")]
        public int CountryId { get; set; }
        [ForeignKey("State Id")]
        public int StateId { get; set; }
        [ForeignKey("City Id")]
        public int CityId { get; set; }   
        public int ZipCode { get; set; }
        public virtual User Users { get; set; }
        public ICollection<Country> Country { get; set; }
        public ICollection<State> States{ get; set; }
        public ICollection<City>Cities { get; set; }

    }
}