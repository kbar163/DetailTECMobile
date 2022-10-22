using DetailTECMobile.Models;
using DetailTECMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DetailTECMobile.Views
{
    //EN CONSTRUCCION
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerView : ContentPage
    {
       
        
        public CustomerView()
        {
            InitializeComponent();
            BindingContext = new CustomerViewModel();
        }
    }
}