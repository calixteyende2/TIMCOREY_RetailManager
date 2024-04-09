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
        private HttpClient _apiClient;
        private ILogInUserModel _logInUserModel;

        public APIHelper(ILogInUserModel logInUserModel)
        {
            InitializeClient();
            _logInUserModel = logInUserModel;
        }

        public HttpClient APIClient
        { 
            get 
            { 
                return _apiClient; 
            } 
        }

      
        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (HttpResponseMessage responseMessage = await _apiClient.PostAsync("/token", data))
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

        public void LogOffUser()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task  GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage responseMessage = await _apiClient.GetAsync("api/User"))
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
