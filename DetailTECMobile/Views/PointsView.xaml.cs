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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointsView : ContentPage
    {
        public PointsView()
        {
            InitializeComponent();
            BindingContext = new PointsViewModel();
        }
    }
}