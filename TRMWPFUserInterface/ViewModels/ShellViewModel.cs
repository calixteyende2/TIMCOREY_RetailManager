using Caliburn.Micro;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMWPFUserInterface.EventModels;
using TRMWPFUserInterface.Helpers;
using TRMWPFUserInterface.Library;

namespace TRMWPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {        
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        private ILogInUserModel _logInUserModel;
        private IAPIHelper _apiHelper;

        [Obsolete]
        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesViewModel, ILogInUserModel logInUserModel, IAPIHelper apiHelper)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _salesViewModel = salesViewModel;
            _logInUserModel = logInUserModel;
            _apiHelper = apiHelper;
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }


        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_logInUserModel.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }
        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public async Task LogOut()
        {
            _apiHelper.LogOffUser();
            _logInUserModel.ResetUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken = default)
        {
            ActivateItemAsync(_salesViewModel, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
            return Task.CompletedTask;
        }

    }
}
