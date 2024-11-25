using System.ComponentModel.DataAnnotations;

namespace Market.Data.Models
{
    public class OfferType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(22)]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
