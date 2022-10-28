using DetailTECMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DetailTECMobile.Models
{
    public class NewAppRequest
    {
        public string cedula_cliente { get; set; }
        public string placa_vehiculo { get; set; }
        public string nombre_sucursal { get; set; }
        public string nombre_lavado { get; set; }
        public string hora { get; set; }
        public bool facturada { get; set; }
        
    }
}
