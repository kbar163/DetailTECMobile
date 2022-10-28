using DetailTECMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DetailTECMobile.ViewModels
{
    public class MyAppointmentsViewModel : BaseViewModel
    {
        
        private bool isRefreshing;
        public ObservableCollection<NewAppRequest> PendingAppointments
        {
            get;
            set;
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.isRefreshing = value; }
        }

        public ICommand SyncCommand{ get; }


        private async Task Sync()
        {
            var appointments = await App.Database.GetAllAppRequest();
            if(appointments.Count != 0)
            {
                this.PendingAppointments.Clear();
                foreach (NewAppRequest element in appointments)
                {
                    var date = element.hora.Substring(0, 10);
                    var time = element.hora.Substring(11, 5);
                    var dateTime = date + " a las " + time;
                    element.hora = dateTime;
                    element.placa_vehiculo = element.placa_vehiculo.ToUpper();
                    PendingAppointments.Add(element);
                }
            }
            
        }

       
        public MyAppointmentsViewModel()
        {
            SyncCommand = new MvvmHelpers.Commands.AsyncCommand(Sync);
            PendingAppointments = new ObservableCollection<NewAppRequest>();
            
        }

    }
}
