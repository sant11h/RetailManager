using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using RMDesktopUI.Models;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.API
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _APIClient { get; set; }
        private ILoggedInUserModel _loggedInUser;

        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        public HttpClient ApiClient
        {
            get { return _APIClient; }
        }

        private void InitializeClient()
        {
            //Reads the api value in App.config
            string api = ConfigurationManager.AppSettings["api"];

            _APIClient = new HttpClient();
            _APIClient.BaseAddress = new Uri(api);
            _APIClient.DefaultRequestHeaders.Accept.Clear();
            _APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
            });

            using (HttpResponseMessage response = await _APIClient.PostAsync("/token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _APIClient.DefaultRequestHeaders.Clear();
            _APIClient.DefaultRequestHeaders.Accept.Clear();
            _APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _APIClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

            using (HttpResponseMessage response = await _APIClient.GetAsync("/api/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
