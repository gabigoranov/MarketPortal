using Market.Data.Models;

namespace Market.Models
{
    public class CartOrderViewModel
    {
        public CartOrderViewModel(Order order, string uRL, int id)
        {
            Order = order;
            URL = uRL;
            Id = id;
        }

        public Order Order { get; set; }
        public string URL { get; set; }

        public int Id { get; set; }
    }
}
