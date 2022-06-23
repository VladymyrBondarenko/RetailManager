using Caliburn.Micro;
using RetailManagerDesktopUI.EventModels;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUserModel;
        private readonly IRestServiceCaller _restServiceCaller;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel loggedInUserModel, 
            IRestServiceCaller restServiceCaller)
        {
            _events = events;
            _loggedInUserModel = loggedInUserModel;
            _restServiceCaller = restServiceCaller;
            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>());

            NotifyOfPropertyChange(() => IsUserLoggedIn);
        }

        public async Task ExitApp()
        {
            await TryCloseAsync();
        }

        public async Task LogOutAsync()
        {
            _loggedInUserModel.ResertUserModel();
            _restServiceCaller.LogOffUser();

            await ActivateItemAsync(IoC.Get<LoginViewModel>());

            NotifyOfPropertyChange(() => IsUserLoggedIn);
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }

        public bool IsUserLoggedIn
        {
            get
            {
                var output = false;

                if (!string.IsNullOrWhiteSpace(_loggedInUserModel.Token))
                {
                    output = true;
                }

                return output;
            }
        }
    }
}
