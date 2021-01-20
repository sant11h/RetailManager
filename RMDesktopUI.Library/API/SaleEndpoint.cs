using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public class SaleEndpoint : ISaleEndpoint
    {
        IAPIHelper _APIHelper;
        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _APIHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _APIHelper.ApiClient.PostAsJsonAsync("/api/sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Log succesful call?
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<SaleModel>> GetAll()
        {
            using (HttpResponseMessage response = await _APIHelper.ApiClient.GetAsync("/api/product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<SaleModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
