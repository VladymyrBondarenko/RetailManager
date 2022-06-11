using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api.Endpoints;
using RetailManagerDesktopUI.Library.Models;
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
        private int _itemQuantity = 1;
        private BindingList<ProductModel> _products;
        private ProductModel _selectedProduct;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private readonly IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadProduct();
        }

        public async Task LoadProduct()
        {
            var res = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(res);
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductModel SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
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
                    subTotal += item.Product.RetailPrice * item.QuantityInCart;
                }

                return subTotal.ToString("C");
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
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart 
        {
            get
            {
                var output = false;

                if(SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if(existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                // TODO: There should be a better way of refreshing the cart display
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                var cartItem = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(cartItem);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Cart);
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
            NotifyOfPropertyChange(() => SubTotal);
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
