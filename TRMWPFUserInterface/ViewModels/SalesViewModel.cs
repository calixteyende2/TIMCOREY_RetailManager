using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMWPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<string>	 _products;

		public BindingList<string> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private string _itemQuantity;

		public string ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value; 
				NotifyOfPropertyChange(() => Products);
			}
		}
        
        public string SubTotal
        {
            get 
            { 
                //TODO - Replace with calculation
                return "$0.00"; 
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

                return canAddToCart;
            }
        }

        public async Task AddToCart()
        {
            try
            {
               
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

        public async Task RemoveFromCart()
        {
            try
            {

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
