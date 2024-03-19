using Caliburn.Micro;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMWPFUserInterface.EventModels;

namespace TRMWPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {        
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;

        [Obsolete]
        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesViewModel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _salesViewModel = salesViewModel;
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken = default)
        {
            ActivateItemAsync(_salesViewModel, cancellationToken);
            return Task.CompletedTask;
        }

    }
}
