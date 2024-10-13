using Market.Data.Models;

namespace Market.Services.Orders
{
    public interface IOrdersService
    {
        public Task ApproveOrderAsync(int id);
        public Task DeclineOrderAsync(int id);
        public Task DeliverOrderAsync(int id);
        public Task<List<Order>> GetUserOrders(Guid id);
    }
}
