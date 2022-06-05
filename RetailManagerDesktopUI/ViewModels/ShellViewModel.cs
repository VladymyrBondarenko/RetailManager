using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private readonly LoginViewModel _loginViewModel;

        public ShellViewModel(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            ActivateItemAsync(_loginViewModel);
        }
    }
}
