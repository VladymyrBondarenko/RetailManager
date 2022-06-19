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

        private Dictionary<string, string> _roles;

        public Dictionary<string, string> Roles
        {
            get { return _roles; }
            set { _roles = value; }
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

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set 
            { 
                _selectedUser = value;
                SelectedUserName = value.EmailAddress;
                UserRoles = new BindingList<string>(
                    value.UserRoles.Select(x => x.Value).ToList());
                LoadAvailableRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set 
            { 
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set 
            { 
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set 
            { 
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _userRoles = new BindingList<string>();

        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set 
            { 
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
                await LoadRoles();
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

        public void LoadAvailableRoles()
        {
            foreach (var role in
                Roles.Where(x => !UserRoles.Contains(x.Value)))
            {
                AvailableRoles.Add(role.Value);
            }
        }

        public async Task LoadRoles()
        {
            Roles = await _userEndpoint.GetAllRoles();
        }

        public async void AddSelectedRole()
        {
            await _userEndpoint.AddToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);

            // TODO: Update roles in the selected user roles

            NotifyOfPropertyChange(() => SelectedUser);
        }

        public async void RemoveSelectedRole()
        {
            await _userEndpoint.RemoveFromRole(SelectedUser.Id, SelectedUserRole);

            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);

            // TODO: Update roles in the selected user roles

            NotifyOfPropertyChange(() => SelectedUser);
        }
    }
}