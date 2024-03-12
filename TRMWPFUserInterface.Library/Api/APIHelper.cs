using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TRMWPFUserInterface.Library;
using TRMWPFUserInterface.Models;

namespace TRMWPFUserInterface.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient apiClient { get; set; }
        private ILogInUserModel _logInUserModel;

        public APIHelper(ILogInUserModel logInUserModel)
        {
            InitializeClient();
            _logInUserModel = logInUserModel;
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (HttpResponseMessage responseMessage = await apiClient.PostAsync("/token", data))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(responseMessage.ReasonPhrase);
                }
            }            
        }

        public async Task  GetLoggedInUserInfo(string token)
        {
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage responseMessage = await apiClient.GetAsync("api/User"))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsAsync<LogInUserModel>();
                    _logInUserModel.CreatedDate = result.CreatedDate;
                    _logInUserModel.EmailAddress = result.EmailAddress;
                    _logInUserModel.FirstName = result.FirstName;
                    _logInUserModel.LastName = result.LastName;
                    _logInUserModel.Id = result.Id;
                    _logInUserModel.Token = token;
                }
                else
                {
                    throw new Exception($"{responseMessage.ReasonPhrase}");
                }

            }          

        }
    }
}
