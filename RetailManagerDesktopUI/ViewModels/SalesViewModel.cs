using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private int _itemQuantity;
        private BindingList<string> _products;
        private BindingList<string> _cart;

        public BindingList<string> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<string> Card
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Card);
            }
        }

        public string SubTotal
        {
            get 
            { 
                // TODO: Replace with calculation
                return "$0.00"; 
            }
        }

        public string Total
        {
            get
            {
                // TODO: Replace with calculation
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO: Replace with calculation
                return "$0.00";
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public bool CanAddToCart 
        {
            get
            {
                var output = false;

                // Make sure sm is selected
                // Make sure there is an item quantity

                return output;
            }
        }

        public void AddToCard()
        {

        }

        public bool CanRemoveFromCart
        {
            get
            {
                var output = false;

                // Make sure sm is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {

        }

        public bool CanCheckOut
        {
            get
            {
                var output = false;

                // Make sure there is sm in the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }
    }
}
