using DetailTECMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DetailTECMobile.ViewModels
{

    public class CustomerViewModel
    {
        #region Atributes
        private Customer user = App.loggedUser;
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
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand { get; }
        #endregion

        #region Methods

        public async Task ChangePassword(string password)
        {

        }
        #endregion

        #region Constructor
        public CustomerViewModel()
        {
            //ChangePasswordCommand = new MvvmHelpers.Commands.AsyncCommand(ChangePassword);
        }
        #endregion
    }
}
