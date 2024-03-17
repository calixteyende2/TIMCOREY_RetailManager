using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMWPFUserInterface.EventModels;
using TRMWPFUserInterface.Helpers;

namespace TRMWPFUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName;
        private string _password;
		private IAPIHelper _apiHelper;
		private IEventAggregator _eventAggregator;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator eventAggregator)
        {
			_apiHelper = apiHelper;
			_eventAggregator = eventAggregator;
        }

        public string UserName
		{
			get { return _userName; }
			set 
			{ 
				_userName = value; 
				NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
		}
		public string Password
		{
			get { return _password; }
			set 
			{ 
				_password = value; 
				NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
		}

		private bool _isErrorVisible;

		public bool IsErrorVisible
        {
			get 
			{
				bool output = false;
				if (!string.IsNullOrEmpty(ErrorMessage)) 
				{
					output = true;
				}
				return output;
			}
		}

		private string _errorMessage;

		public string ErrorMessage
        {
			get { return _errorMessage; }
			set 
			{ 
				_errorMessage = value; 
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}



		public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
            }
        }

        public async Task Login()
		{
			try
			{
				ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);

				//Capture more information about the user
				await _apiHelper.GetLoggedInUserInfo(result.access_token);

                await _eventAggregator.PublishOnUIThreadAsync(new LogOnEvent());

			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
			
		}


	}
}
