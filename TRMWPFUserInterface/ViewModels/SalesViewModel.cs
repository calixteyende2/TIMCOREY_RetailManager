using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TRMWPFUserInterface.Library.Api;
using TRMWPFUserInterface.Library.Helpers;
using TRMWPFUserInterface.Library.Models;

namespace TRMWPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        ISaleEndpoint _saleEndpoint;
        IProductEnpoint _productEnpoint;
        IConfigHelper _configHelper;

        public SalesViewModel(IProductEnpoint productEnpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
        {
            _productEnpoint = productEnpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts(); 
        }

        private async Task LoadProducts()
        {                                          //Etape 1
            var productList = await _productEnpoint.GetAll(); 
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;
        public BindingList<ProductModel> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private ProductModel _selectedProduct;
        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity = 1;
		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value; 
				NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
        
        public string SubTotal
        {
            get 
            {
                return CalculateSubTotal().ToString("c"); //"c" for currency
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            subTotal = Cart.Sum(x=>x.Product.RetailPrice*x.QuantityInCart);

            //foreach (var item in Cart)
            //{
            //    subTotal += item.Product.RetailPrice * item.QuantityInCart;
            //}

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //    }
            //}

            return taxAmount;
        }
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("c");
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateTax() + CalculateSubTotal();
                return total.ToString("c");
            }
        }


        public bool CanAddToCart
        {
            get
            {
                bool canAddToCart = false;
                
                //Make sure something is selected
                if(ItemQuantity >0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    canAddToCart = true;
                }

                return canAddToCart;
            }
        }

        public void AddToCart()
        {
            try
            {
                CartItemModel existingItem = Cart.FirstOrDefault(X => X.Product == SelectedProduct);
                if (existingItem != null)
                {
                    existingItem.QuantityInCart += ItemQuantity;
                    Cart.Remove(existingItem);
                    Cart.Add(existingItem);
                }
                else
                {
                    CartItemModel item = new CartItemModel()
                    {
                        Product = SelectedProduct,
                        QuantityInCart = ItemQuantity
                    };
                    Cart.Add(item);
                }
                SelectedProduct.QuantityInStock -= ItemQuantity;
                ItemQuantity = 1;                
                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => Total);
                NotifyOfPropertyChange(() => CanCheckOut);
            }
            catch (Exception ex)
            {

            }

        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool canRemoveFromCart = false;

                //Make sure something is selected

                return canRemoveFromCart;
            }
        }

        public void RemoveFromCart()
        {
            try
            {

                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => Total);
                NotifyOfPropertyChange(() => CanCheckOut);
            }
            catch (Exception ex)
            {

            }

        }

        public bool CanCheckOut
        {
            get
            {
                bool canCheckOut = false;

                //Make sure is something in the cart
                if (Cart.Count>0)
                {
                    canCheckOut = true; 
                }
                return canCheckOut;
            }
        }

        public async Task CheckOut()
        {
            try
            {
                //Create a SaleModel and post to the API
                SaleModel sale = new SaleModel();

                foreach (var item in Cart)
                {
                    sale.SaleDetails.Add(new SaleDetailModel
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.QuantityInCart
                    });                    
                }
                await _saleEndpoint.PostSale(sale);
            }
            catch (Exception ex)
            {
                
            }

        }

    }
}
