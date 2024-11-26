using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Data.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(28)]
        public string Title { get; set; }

        [Required]
        [StringLength(16)]
        public string Town { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public double AvgRating => !Reviews.IsNullOrEmpty() ? Math.Round(Reviews.Select(x => x.Rating).Average(), 2) : 0;
        

        [Required]
        public double PricePerKG { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid OwnerId { get; set; }
        [NotMapped]
        public User Owner { get; set; }

        [Required]
        [ForeignKey(nameof(Stock))]
        public int StockId { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }


        public Stock Stock { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

    }
}
