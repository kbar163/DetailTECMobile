using DetailTECMobile.Models;
using DetailTECMobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DetailTECMobile.ViewModels
{

    public class CustomerViewModel : BaseViewModel
    {
        #region Atributes
        private Customer user = App.loggedUser;
        public string newPassword;
        #endregion

        #region Properties
        public Customer User
        {
            get { return this.user; }
        }

        public string CustomerName
        {
            get
            {
                return this.user.nombre + " " + this.user.primer_apellido + " " + this.user.segundo_apellido;
            }
        }

        public string CustomerId
        {
            get
            {
                return this.user.cedula_cliente;
            }
        }

        public string CustomerEmail
        {
            get
            {
                return this.user.correo_cliente;
            }
        }

        public string CustomerUser
        {
            get
            {
                return this.user.usuario;
            }
        }

        public string CustomerPassword
        {
            get
            {
                return this.user.password_cliente;
            }
            set
            {
                this.user.password_cliente = value;
            }
        }

        public string PrimaryAddress
        {
            get
            {
                return this.user.direcciones[0].provincia + ", " + this.user.direcciones[0].canton + ", " + this.user.direcciones[0].distrito;
            }
        }

        public string PrimaryPhone
        {
            get
            {
                return this.user.telefonos[0];
            }
        }

        public string NewPassword
        {
            get { return this.newPassword; }
            set { SetValue(ref this.newPassword, value); }
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand { get; }
        #endregion

        #region Methods
        private async Task ChangePassword()
        {
            if(this.newPassword != null)
            {
                this.CustomerPassword = this.newPassword;
                bool isUpdated = await PassUpdateService.UpdateRemoteCustomer(this.User);
                if (isUpdated)
                {
                    await App.Database.UpdateCustomer(this.User);
                    await Application.Current.MainPage.DisplayAlert("Exito", "Password actualizado exitosamente!", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atención", "Esta acción solo puede ser realizada con una conexión activa", "Ok");
                }
            }
            

        }

        #endregion

        #region Constructor
        public CustomerViewModel()
        {
            ChangePasswordCommand = new MvvmHelpers.Commands.AsyncCommand(ChangePassword);
        }

        
        #endregion
    }
}
