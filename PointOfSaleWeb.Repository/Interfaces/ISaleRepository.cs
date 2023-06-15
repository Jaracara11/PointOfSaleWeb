using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<IEnumerable<Discount>> GetDiscountsByUsername(string username);
    }
}
