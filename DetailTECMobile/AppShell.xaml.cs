using DetailTECMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DetailTECMobile
{   
    //Esta vista stock de Xamarin forms se utiliza para facilitar navegacion y transferencia de datos entre distintas vistas,
    //puede considerarse el hub de la aplicacion.
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

    }
}
