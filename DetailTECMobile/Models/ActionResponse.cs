﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DetailTECMobile.Models
{
    //Modelo de datos para respuesta a front-end despues de hacer cualquier
    //tipo de accion CRUD.
    public class ActionResponse
    {
        public bool actualizado { get; set; }
        public string mensaje { get; set; }
    }
}
