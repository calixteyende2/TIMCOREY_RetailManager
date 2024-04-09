using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using TRMWPFUserInterface.Library.Api;
using TRMWPFUserInterface.Library.Helpers;
using TRMWPFUserInterface.Library.Models;
using TRMWPFUserInterface.Models;

namespace TRMWPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        ISaleEndpoint _saleEndpoint;
        IProductEnpoint _productEnpoint;
        IConfigHelper _configHelper;
        IMapper _mapper;

        public SalesViewModel(IProductEnpoint productEnpoint, 
                              IConfigHelper configHelper, 
                              ISaleEndpoint saleEndpoint,
                              IMapper mapper)
        {
            _productEnpoint = productEnpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts(); 
        }

        private async Task LoadProducts()
        {                                          //Etape 1
            var productList = await _productEnpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;
        public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private ProductDisplayModel _selectedProduct;
        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private async Task ResetSaleViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            //TODO - Add clearing the selectedCartItem if it does not do it itself
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private CartItemDisplayModel _selectedCartItem;
        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        public BindingList<CartItemDisplayModel> Cart
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
                CartItemDisplayModel existingItem = Cart.FirstOrDefault(X => X.Product == SelectedProduct);
                if (existingItem != null)
                {
                    existingItem.QuantityInCart += ItemQuantity;
                    //HACK There should be a better way of refreshing the cart display
                    //Cart.Remove(existingItem); //Apres ajout de automap
                    //Cart.Add(existingItem);    //Apres ajout de automap
                }
                else
                {
                    CartItemDisplayModel item = new CartItemDisplayModel()
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
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    canRemoveFromCart = true;
                }

                return canRemoveFromCart;
            }
        }

        public void RemoveFromCart()
        {
            try
            {
                SelectedCartItem.Product.QuantityInStock += 1;

                if (SelectedCartItem.QuantityInCart > 1)
                {
                    SelectedCartItem.QuantityInCart -= 1;                    
                }
                else
                {
                    Cart.Remove(SelectedCartItem);
                }

                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => Total);
                NotifyOfPropertyChange(() => CanCheckOut);
                NotifyOfPropertyChange(() => CanAddToCart);
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

                await ResetSaleViewModel();
            }
            catch (Exception ex)
            {
                
            }

        }

    }
}
