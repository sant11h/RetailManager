using RMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public interface ISaleEndpoint
    {
        Task<List<SaleModel>> GetAll();
        Task PostSale(SaleModel sale);
    }
}