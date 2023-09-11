using ArmaduraLosaRevit.Model.ViewportnNH.model;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Model
{
    public class EnvoltorioViewAsosciadoGrid : INotifyPropertyChanged
    {     
        public ViewGeom viewGeom { get; set; }
        public string Nombre_Actual { get; set; }
        //public string Nombre_Nuevo { get; set; }


        //***

        private string _nombre_Nuevo;
        public string Nombre_Nuevo
        {
            get { return _nombre_Nuevo; }
            set
            {
                if (_nombre_Nuevo != value)
                {
                    _nombre_Nuevo = value;

                    if (_nombre_Nuevo != Nombre_Actual)
                        IsCambiarColor = true;
                    else
                        IsCambiarColor = false;
                    OnPropertyChanged();
                }
            }
        }

        //**
        private bool _isCambiarColor;
        public bool IsCambiarColor
        {
            get { return _isCambiarColor; }
            set
            {
                if (_isCambiarColor != value)
                {

                    _isCambiarColor = value;
                    OnPropertyChanged();
                }
            }
        }

        //***********
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {

                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }



        public bool IsAnalizado { get; set; }// se utiliza al momento de cmabiar los nombre para ver si ya se analizo
        public bool IsOK { get; set; }
        public EnvoltorioViewAsosciadoGrid(ViewGeom viewGeom)
        {
            this.viewGeom = viewGeom;
            this.Nombre_Actual = viewGeom.viewDTO_.Nombre;
            this.Nombre_Nuevo = viewGeom.viewDTO_.Nombre;
            this.IsOK  = true;
        }

        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
