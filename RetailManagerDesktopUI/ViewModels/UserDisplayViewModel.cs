using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api.Endpoints;
using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailManagerDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _statusInfoView;
        private readonly IWindowManager _windowManager;
        private readonly IUserEndpoint _userEndpoint;

        public UserDisplayViewModel(
            StatusInfoViewModel statusInfoView, IWindowManager windowManager, 
            IUserEndpoint userEndpoint)
        {
            _statusInfoView = statusInfoView;
            _windowManager = windowManager;
            _userEndpoint = userEndpoint;
        }

        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _statusInfoView.UpdateMessage(
                        "Unauthorized Acceess", "You do not have access to interact with Sale Form.");
                }
                else
                {
                    _statusInfoView.UpdateMessage(
                        "Fatal Exception", ex.Message);
                }
                await _windowManager.ShowWindowAsync(_statusInfoView, settings: settings);

                await TryCloseAsync();
            }
        }

        public async Task LoadUsers()
        {
            var res = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(res.ToList());
        }
    }
}