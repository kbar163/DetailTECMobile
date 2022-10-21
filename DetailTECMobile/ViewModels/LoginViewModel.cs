using DetailTECMobile.Data;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel.Internals;
using Xamarin.Forms;
using SQLite;

namespace DetailTECMobile.ViewModels
{
    //Maneja la logica de navegacion y enventos de la vista de login, hereda la clase BaseViewModel, la cual implementa la interfaz de manejo de notificaciones de
    //cambio de contenido dentro de Xamarin.
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
            
            var customer = App.Database.GetCustomer(UserName);
            if(customer != null)
            {
                if (customer.usuario != null && customer.usuario == UserName.ToString())
                {
                    if (UserPassword.ToString() == customer.password_cliente)
                    {

                        App.loggedUser = customer;
                        App.Current.MainPage = new AppShell();
                        Console.WriteLine(customer.telefonos[0]);

                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Atencion", "La contraseña ingresada es incorrecta", "Ok");
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Atencion", "El nombre de usuario es incorrecto", "Ok");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("ERROR", "Error al obtener usuario en la base de datos", "Ok");
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
