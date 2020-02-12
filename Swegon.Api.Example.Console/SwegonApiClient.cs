using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Swegon.Api.Example
{
    public class SwegonApiClient
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;

        public SwegonApiClient(HttpClient client, IConfiguration config)
        {
            this.client = client;
            this.config = config;
        }

        public async Task<string> GetManufacturingOrderAsync(string accessToken, string manufacturingOrderNumber, string facility, string itemNumber)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{config["ApiBaseUrl"]}/manufacturing-order/{manufacturingOrderNumber}/{facility}/{itemNumber}"),
                Headers =
                {
                    { "Authorization", "Bearer " + accessToken }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}