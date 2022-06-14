using AutoMapper;
using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api.Endpoints;
using RetailManagerDesktopUI.Library.Helpers;
using RetailManagerDesktopUI.Library.Models;
using RetailManagerDesktopUI.Models;
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
        private BindingList<ProductDisplayModel> _products;
        private ProductDisplayModel _selectedProduct;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private readonly IProductEndpoint _productEndpoint;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly IMapper _mapper;

        public SalesViewModel(
            IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, 
            IConfigHelper configHelper, IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadProduct();
        }

        public async Task LoadProduct()
        {
            var res = await _productEndpoint.GetAll();

            var products = _mapper.Map<List<ProductDisplayModel>>(res);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductDisplayModel SelectedProduct
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

        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set 
            { 
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        public string SubTotal
        {
            get 
            {
                return calcSubTotal().ToString("C");
            }
        }

        private decimal calcSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += item.Product.RetailPrice * item.QuantityInCart;
            }

            return subTotal;
        }

        public string Total
        {
            get
            {
                var total = calcSubTotal() + calcTaxAmount();
                return total.ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return calcTaxAmount().ToString("C");
            }
        }

        private decimal calcTaxAmount()
        {
            decimal taxRate = _configHelper.GetTaxRate()/100;

            return Cart
                .Where(i => i.Product.IsTaxable)
                .Sum(i => i.Product.RetailPrice * i.QuantityInCart * taxRate);
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
            }
            else
            {
                var cartItem = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(cartItem);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                var output = false;

                if (SelectedCartItem?.QuantityInCart >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += ItemQuantity;

            if (SelectedCartItem.QuantityInCart > ItemQuantity)
            {
                SelectedCartItem.QuantityInCart -= ItemQuantity;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanAddToCart);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                var output = false;

                if (Cart.Any())
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task CheckOut()
        {
            var saleModel = new SaleModel();

            foreach (var item in Cart)
            {
                saleModel.SaleDetailModels.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(saleModel);
        }
    }
}
