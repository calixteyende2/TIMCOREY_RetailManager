using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMWPFUserInterface.Helpers;

namespace TRMWPFUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName;
        private string _password;
		private IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper apiHelper)
        {
				_apiHelper = apiHelper;
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
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}


	}
}
