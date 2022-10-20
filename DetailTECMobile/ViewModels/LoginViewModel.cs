using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel.Internals;
using Xamarin.Forms;

namespace DetailTECMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        #region Attributes

        private string userName;
        private string userPassword;

        #endregion

        #region Properties
        public string UserName 
        {
            get { return this.userName; }
            set { SetValue(ref this.userName, value); }
        }

        public string UserPassword
        {
            get { return this.userPassword; }
            set { SetValue(ref this.userPassword, value); }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }

        #endregion

        #region Methods
        private void Login()
        {
            if(UserName.ToString() == "email@gmail.com"
               && UserPassword.ToString() == "password")
            {
                Application.Current.MainPage.DisplayAlert("Status", "Logged in", "Ok");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Status", "Try again", "Ok");
            }
        }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            LoginCommand = new MvvmHelpers.Commands.Command(Login);
        }

        #endregion



    }
}
