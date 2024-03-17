using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMWPFUserInterface.Library.Api;
using TRMWPFUserInterface.Library.Models;

namespace TRMWPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEnpoint _productEnpoint;

        public SalesViewModel(IProductEnpoint productEnpoint)
        {
            _productEnpoint = productEnpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
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
                decimal subTotal = 0;

                foreach (var item in Cart)
                {
                    subTotal += item.Product.RetailPrice*item.QuantityInCart;
                }
                return subTotal.ToString("c"); 
            }
        }

        public string Tax
        {
            get
            {
                //TODO - Replace with calculation
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                //TODO - Replace with calculation
                return "$0.00";
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
                return canCheckOut;
            }
        }

        public async Task CheckOut()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

        }

    }
}
