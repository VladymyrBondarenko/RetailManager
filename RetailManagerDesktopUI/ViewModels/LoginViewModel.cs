using Caliburn.Micro;
using RetailManagerDesktopUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.ViewModels
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
                var res = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
