using Caliburn.Micro;
using RetailManagerDesktopUI.EventModels;
using RetailManagerDesktopUI.Helpers;
using RetailManagerDesktopUI.Library.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "vladymyr.bondarenko1@gmail.com";
        private string _password = "1777897Vova.";
        private string _errorMessage;
        private IRestServiceCaller _restServiceCaller;
        private readonly IEventAggregator _events;

        public LoginViewModel(IRestServiceCaller restServiceCaller, IEventAggregator events)
        {
            _restServiceCaller = restServiceCaller;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value; 
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value; 
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool IsErrorVisible
        {
            get 
            {
                var output = false;
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    output = true;
                }
                return output; 
            }
        }

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


        public bool CanLogIn
        {
            get
            {
                var output = false;

                // TODO: do more thorough check on password and username later

                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty;
                var res = await _restServiceCaller.Authenticate(UserName, Password);
                if (!string.IsNullOrWhiteSpace(res.Access_Token))
                {
                    await _restServiceCaller.GetLoggedInUser(res.Access_Token);
                    await _events.PublishOnUIThreadAsync(new LogOnEvent());
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
