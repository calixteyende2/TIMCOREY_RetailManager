using System.Threading.Tasks;
using TRMWPFUserInterface.Library.Models;

namespace TRMWPFUserInterface.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}