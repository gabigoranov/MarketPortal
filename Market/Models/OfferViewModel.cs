using Market.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Market.Models
{
    public class OfferViewModel
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

        [Required]
        public double PricePerKG { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }


        [Required]
        [Range(minimum: 5, maximum: 20)]
        public int Discount { get; set; }
    }
}
