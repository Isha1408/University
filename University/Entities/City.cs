using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table("City")]
    public partial class City
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("City")]
        public string Name { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public bool IsActive { get; set; }

    }
}