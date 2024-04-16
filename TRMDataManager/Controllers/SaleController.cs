using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManagerClassLibrary.DataAccess;
using TRMDataManagerClassLibrary.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiController
    {
        //POST api/Sale/Post
        [Route("Post")]
        public void Post(SaleModel saleModel)
        {
            SaleDataAccess dataAccess = new SaleDataAccess();
            string userId = RequestContext.Principal.Identity.GetUserId();
            
            dataAccess.SaveSale(saleModel, userId);
        }

        //Get api/Sale/GetSalesReport
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleDataAccess saleData = new SaleDataAccess();
            return saleData.GetSaleReports();
        }

    }
}
