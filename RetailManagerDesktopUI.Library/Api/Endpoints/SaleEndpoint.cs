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
        private readonly ILogger<SaleEndpoint> _logger;

        public SaleEndpoint(IRestServiceCaller restServiceCaller, ILogger<SaleEndpoint> logger)
        {
            _restServiceCaller = restServiceCaller;
            _logger = logger;
        }

        public async Task PostSale(SaleModel saleModel)
        {
            using (var httpResponse = await _restServiceCaller.HttpClient.PostAsJsonAsync("api/Sale", saleModel))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "A sale posted successfully at {0}", DateTime.Now);
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }
        }
    }
}
