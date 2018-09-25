using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table("State")]
    public  class State
    {
        public State()
        {
            this.Addresses = new HashSet<Address>();
            this.Cities = new HashSet<City>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public int CountryId { get; set; }
        //[ForeignKey("Country Id")]
        public virtual Country Country{ get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<City> Cities { get; set; }
        public bool IsActive { get; set; }
    }
}