using Market.Data.Models;
using Market.Models;

namespace Market.Services.Inventory
{
    public interface IInventoryService
    {
        public Task AddStockAsync(StockViewModel model);
        public Task UpStockAsync(ChangeStockViewModel model);
        public Task DownStockAsync(int id, double quantity);
        public Task DeleteStockAsync(int id);
        public Task<List<Stock>> GetSellerStocksAsync();
    }
}
