using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table("Country")]
    public partial class Country
    {
        public Country()
        {
            this.Addresses = new HashSet<Address>();
            this.States = new HashSet<State>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection< State> States { get; set; }
        public bool IsActive { get; set; }
    }
}