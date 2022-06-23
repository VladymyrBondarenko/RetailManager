using Microsoft.Extensions.Logging;
using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IRestServiceCaller _restServiceCaller;

        public SaleEndpoint(IRestServiceCaller restServiceCaller)
        {
            _restServiceCaller = restServiceCaller;
        }

        public async Task PostSale(SaleModel saleModel)
        {
            using (var httpResponse = await _restServiceCaller.HttpClient.PostAsJsonAsync("api/Sale", saleModel))
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }
    }
}
