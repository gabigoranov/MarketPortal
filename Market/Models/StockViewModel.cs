using Market.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Market.Models
{
    public class StockViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [ForeignKey(nameof(OfferType))]
        public int OfferTypeId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid SellerId { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}
