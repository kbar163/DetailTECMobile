using DetailTECMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DetailTECMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyAppointmentsView : ContentPage
    {
        public MyAppointmentsView()
        {
            InitializeComponent();
            BindingContext = new MyAppointmentsViewModel();
        }

        
    }
}