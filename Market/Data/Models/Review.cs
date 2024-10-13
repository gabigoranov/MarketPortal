using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [ForeignKey(nameof(Offer))]
        public int OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }
    }
}
