using System;
using System.Collections.Generic;
using System.Text;

namespace DetailTECMobile.Models
{
    //Esta clase es usada como modelo de datos para respuesta de GET request
    //en el que se devuelve una lista de valores del modelo WashType.
    public class MultivalueWash
    {
        public bool exito { get; set; }
        public List<WashType> lavados { get; set; }

    }
}
