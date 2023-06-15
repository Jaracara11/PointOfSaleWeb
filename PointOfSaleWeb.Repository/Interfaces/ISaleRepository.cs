using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<IEnumerable<decimal>> GetDiscountsByUsername(string username);
    }
}
