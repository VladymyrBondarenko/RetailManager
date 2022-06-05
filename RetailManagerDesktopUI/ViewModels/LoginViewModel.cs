using Caliburn.Micro;
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

        public void LogIn(string userName, string password)
        {
            Console.WriteLine();
        }
    }
}
