using Autofac;
using RMDataManager.Library.Models;
using RMDataManager.Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static RMDataManager.Startup;

namespace RMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        private readonly IProductData _productData;

        public ProductController()
        {
            _productData = ServiceTuner.Resolve<IProductData>();
        }

        [HttpGet()]
        public async Task<List<ProductModel>> Get()
        {
            var res = await _productData.GetProducts();
            return res;
        }
    }
}
