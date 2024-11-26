using Market.Data.Models;

namespace Market.Models
{
    public class OverviewViewModel
    {
        public OverviewViewModel(List<Order> orders, List<Review> reviews, List<Stock> stocks)
        {
            Orders = orders;
            Reviews = reviews;
            Stocks = stocks;
        }

        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }

        public List<Stock> Stocks { get; set; }
    }
}
