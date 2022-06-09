using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IRestServiceCaller _restServiceCaller;

        public ProductEndpoint(IRestServiceCaller restServiceCaller)
        {
            _restServiceCaller = restServiceCaller;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            using (var httpResponse = await _restServiceCaller.HttpClient.GetAsync("api/Product"))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var res = await httpResponse.Content.ReadAsAsync<List<ProductModel>>();
                    return res;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }
    }
}
