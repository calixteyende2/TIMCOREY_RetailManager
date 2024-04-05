using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMWPFUserInterface.Helpers;
using TRMWPFUserInterface.Library.Models;

namespace TRMWPFUserInterface.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private IAPIHelper _apiHelper;
        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        //Etape 2
        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage responseMessage = await _apiHelper.APIClient.PostAsJsonAsync("api/Sale/Post", sale))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    //TODO if successful

                }
                else
                {
                    throw new Exception($"{responseMessage.ReasonPhrase}");
                }

            }
        }
    }
}
