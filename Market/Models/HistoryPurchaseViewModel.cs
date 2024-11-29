using Market.Data.Models;

namespace Market.Models
{
    public class HistoryPurchaseViewModel
    {
        public HistoryPurchaseViewModel(Purchase purchase, Dictionary<int, string> urls)
        {
            Purchase = purchase;
            Urls = urls;
        }

        public Purchase Purchase { get; set; }
        public Dictionary<int, string> Urls { get; set; }
    }
}
