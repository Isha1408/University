using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table("City")]
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public int StateId { get; set; }
        //[ForeignKey("State Id")]
        public virtual State State { get; set; }

        public ICollection<Address> Addresses { get; set; }

    }
}