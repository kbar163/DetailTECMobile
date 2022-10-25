using DetailTECMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DetailTECMobile.ViewModels
{
    public class PointsViewModel
    {
        #region Atributes
        private Customer user = App.loggedUser;
        #endregion

        #region Properties
        public string AccumulatedPoints
        {
            get
            {
                return this.user.puntos_acum.ToString();
            }
        }

        public string ObtainedPoints
        {
            get
            {
                return this.user.puntos_obt.ToString();
            }
        }

        public string RedeemedPoints
        {
            get
            {
                return this.user.puntos_redim.ToString();
            }
        }

        #endregion

        #region Constructor
        public PointsViewModel()
        {

        }
        #endregion



    }
}
