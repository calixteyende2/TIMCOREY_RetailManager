using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManagerClassLibrary.Models;

namespace TRMDataManagerClassLibrary.DataAccess
{
    public class SaleDataAccess
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            //Start filling in the saleInfo detail models we will save to the database
            List<SaleDetailDBModel> saleDetails = new List<SaleDetailDBModel>();
            ProductDataAccess productDataAccess = new ProductDataAccess();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel 
                { 
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get infos about this product
                var productInfos = productDataAccess.GetProductById(detail.ProductId);

                if (productInfos == null)
                {
                    throw new Exception($"The product of Id {detail.ProductId} could not be found in database.");
                }

                detail.PurchasePrice = (productInfos.RetailPrice * detail.Quantity);

                if(productInfos.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                saleDetails.Add(detail);
            }

            //Fill in the available information
            //Create the Sale model
            SaleDBModel sale = new SaleDBModel()
            { 
                SubTotal = saleDetails.Sum(x => x.PurchasePrice),
                Tax = saleDetails.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            using(SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    //Start transaction
                    sql.StartTransaction("TRMDataBase");

                    //Save the Sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    //Get the ID from the saleInfo model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();


                    //Finish filling in the saleInfo detail models
                    foreach (var item in saleDetails)
                    {
                        item.SaleId = sale.Id;
                        //Save the saleInfo detail models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch(Exception ex) 
                {

                    sql.RollbackTransaction();
                    throw;
                }
            }

        }

    }

}
