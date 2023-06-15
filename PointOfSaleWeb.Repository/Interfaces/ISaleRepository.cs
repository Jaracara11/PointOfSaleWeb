using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Discount>> GetDiscountsAvailable();
    }
}
