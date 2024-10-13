using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public double Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [ForeignKey(nameof(Offer))]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        public Guid? BuyerId { get; set; }
        public User Buyer { get; set; }

        public Guid? SellerId { get; set; }
        public User Seller { get; set; }

        public bool IsDelivered { get; set; } = false;
        public DateTime? DeliveredDate { get; set; } = null;

        [Required]
        public DateTime DateOrdered { get; set; }
    }
}
