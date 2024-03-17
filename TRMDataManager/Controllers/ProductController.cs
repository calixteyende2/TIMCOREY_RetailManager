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
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        // GET api/Product/GetAll
        [Route("GetAll")]
        public IList<ProductModel> Get()
        {
            ProductDataAccess dataAccess = new ProductDataAccess();

            return dataAccess.GetProducts();
            
        }
    }
}
