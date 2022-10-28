using DetailTECMobile.Models;
using DetailTECMobile.Services;
using DetailTECMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DetailTECMobile.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        #region Attributes
        private Customer user = App.loggedUser;
        private DateTime end = new DateTime(2022, 12, 31);
        private DateTime start = DateTime.Today;
        public List<string> availableTimes = new List<string>();
        private string selectedDate;
        private string selectedTime;
        private WashType selectedWash;
        private MultivalueAppointment currentAppointments;
        private MultivalueWash washTypes = App.Database.GetAllWashTypes().Result;
        private string placa;
        private NewAppRequest newAppointment = new NewAppRequest();
        #endregion

        #region Properties

        public string Placa
        {
            get { return this.placa; }
            set { SetValue(ref this.placa, value); }
        }
        public List<string> AllTimes
        {
            get
            {
                var times = new List<string>();
                times.Add("08:00");
                times.Add("08:30");
                times.Add("09:00");
                times.Add("09:30");
                times.Add("10:00");
                times.Add("10:30");
                times.Add("11:00");
                times.Add("11:30");
                times.Add("12:00");
                times.Add("12:30");
                times.Add("13:00");
                times.Add("13:30");
                times.Add("14:00");
                times.Add("14:30");
                times.Add("15:00");
                times.Add("15:30");
                times.Add("16:00");
                times.Add("16:30");
                times.Add("17:00");
                return times;
            }
        }

        public List<WashType> WashTypes
        {
            get 
            { 
                return washTypes.lavados;
            }
        }
        public List<string> AvailableTimes
        {
            get { return this.availableTimes; }
            set { this.availableTimes = value; }
        }

        public List<string> Dates
        {
            get
            {
                var dateTimes = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                    .Select(offset => start.AddDays(offset))
                    .ToList();
                var dateStrings = new List<string>();

                foreach(DateTime date in dateTimes)
                {
                    dateStrings.Add(date.Date.ToShortDateString());
                }
                return dateStrings;
            }

        }

        public string SelectedDate
        {
            get
            {
                return this.selectedDate;
            }
            set
            {
                this.selectedDate = value;
            }
        }

        public string SelectedTime
        {
            get
            {
                return this.selectedTime;
            }
            set
            {
                this.selectedTime = value;
            }
        }

        public WashType SelectedWash
        {
            get
            {
                return this.selectedWash;
            }
            set
            {
                this.selectedWash = value;
            }
        }
        #endregion

        #region Commands
        public ICommand CheckDateCommand { get; }
        #endregion

        #region Methods
        private async Task CheckDate()
        {
            await syncApps();
            AvailableTimes = new List<string>(AllTimes);
            foreach(Appointment appointment in currentAppointments.citas)
            {
                if(DateCompare(Convert.ToDateTime(selectedDate),appointment.hora))
                {
                    var firstHString = appointment.hora.Hour.ToString();
                    var firstMString = appointment.hora.Minute.ToString();
                    if (firstMString == "0") firstMString = "00";
                    var firstTString = firstHString + ":" + firstMString;
                    if(appointment.duracion == 30)
                    {
                        this.availableTimes.RemoveAll(times => times == firstTString);
                    }
                    if(appointment.duracion == 60)
                    {
                       
                        var secondTime = appointment.hora.AddMinutes(30);
                        var secondHString = secondTime.Hour.ToString();
                        var secondMString = secondTime.Minute.ToString();
                        if (secondMString == "0") secondMString = "00";
                        var secondTString = secondHString + ":" + secondMString;
                        this.availableTimes.RemoveAll(times => times == firstTString || times == secondTString);
                    }
                }
            }
            if (!availableTimes.Contains(this.selectedTime))
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "La hora indicada no se encuentra disponible, por favor elija otra hora", "Ok");
            }
            else
            {
                
                this.newAppointment.cedula_cliente = App.loggedUser.cedula_cliente;
                this.newAppointment.nombre_lavado = SelectedWash.nombre_lavado;
                this.newAppointment.nombre_sucursal = "San Pedro";
                this.newAppointment.placa_vehiculo = Placa;
                this.newAppointment.hora = this.selectedDate + "T" + this.selectedTime + ":00.0000000";
                this.newAppointment.facturada = false;
                var insertLocalApp = await App.Database.AddPendingApp(this.newAppointment);
                if (insertLocalApp)
                {
                    await Application.Current.MainPage.DisplayAlert("Exito", "Su cita fue agendada exitosamente", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al crear cita pendiente", "Ok");
                }
            }
        }

         
        private bool DateCompare(DateTime date1 , DateTime date2)
        {
            return date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year;
        }

        private async Task syncApps()
        {
            var appointments = await AppointmentService.GetAllAppointments();
            if(appointments.exito)
            {
                var updateAppointments = await App.Database.AddAppointments(appointments);
                if (updateAppointments)
                {
                    this.currentAppointments = appointments;
                }
                else
                {
                    this.currentAppointments = await App.Database.GetAllAppointments();
                    await Application.Current.MainPage.DisplayAlert("ERROR", "La aplicacion Encontro un error al insertar una cita en la base de datos local, se utilizaran los datos obtenidos en la ultima sesion en linea.", "Ok");
                }
            }
            else
            {
                this.currentAppointments = await App.Database.GetAllAppointments();
                await Application.Current.MainPage.DisplayAlert("Atencion", "La aplicacion se encuentra en modo offline, se utilizaran los datos de disponibilidad de citas actualizados en la sesion online previa.", "Ok");
            }
        }

        #endregion

        #region Constructor
        public AppointmentViewModel()
        {
            CheckDateCommand = new MvvmHelpers.Commands.AsyncCommand(CheckDate);
        }
        #endregion
    }
}
