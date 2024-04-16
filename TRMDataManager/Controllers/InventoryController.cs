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
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiController
    {
        //Get api/Inventory/GetInventories
        [Route("GetInventories")]
        public List<InventoryModel> GetInventories()
        {
            InventoryDataAccess inventoryData = new InventoryDataAccess();
            return inventoryData.GetInventories();
        }

        [Route("Post")]
        public void Post(InventoryModel item)
        {
            InventoryDataAccess inventoryData = new InventoryDataAccess();
            inventoryData.SaveInventoryRecord(item);
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}