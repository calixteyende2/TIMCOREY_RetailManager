using System.Collections.Generic;
using System.Threading.Tasks;
using TRMWPFUserInterface.Library.Models;

namespace TRMWPFUserInterface.Library.Api
{
    public interface IProductEnpoint
    {
        Task<IList<ProductModel>> GetAll();
    }
}