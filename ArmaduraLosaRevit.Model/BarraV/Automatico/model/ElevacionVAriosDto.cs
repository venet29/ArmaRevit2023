using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model
{
    public class ElevacionVAriosDto : INotifyPropertyChanged
    {

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; NotifyPropertyChanged("IsSelected"); }
        }


        private string nombreJson;
        public string NombreJson
        {
            get { return nombreJson; }
            set
            {
                if (value == null) return;
                nombreJson = value;
                var infoNuewJson = ListaRutasArchivos.Where(c => c.Name == nombreJson).FirstOrDefault();
                if (infoNuewJson != null)
                    ArchJsonSeleccion = infoNuewJson;

                NotifyPropertyChanged("NombreJson");
            }
        }

        private string nombreElev;
        public string NombreElev
        {
            get { return nombreElev; }
            set { if (value == null) return; nombreElev = value; NotifyPropertyChanged("NombreElev"); }
        }



        private View _viewElev;
        public View ViewElev
        {
            get { return _viewElev; }
            set { if (value == null) return; _viewElev = value; NotifyPropertyChanged("ViewLosa"); }
        }


        private List<FileInfo> _listaRutasArchivos;
        public List<FileInfo> ListaRutasArchivos
        {
            get { return _listaRutasArchivos; }
            set { if (value == null) return; _listaRutasArchivos = value; NotifyPropertyChanged("ListaRutasArchivos"); }
        }

        public FileInfo ArchJsonSeleccion { get; private set; }

        public string TipoView { get; set; }
        private ElevacionVarioSelected _dequienSeCopia;
        public ElevacionVarioSelected DequienSeCopia
        {
            get { return _dequienSeCopia; }
            set
            {
                if (value == null) return;

                _dequienSeCopia = value;
                NotifyPropertyChanged("DequienSeCopia");
            }
        }


        public ElevacionVAriosDto(string name, View _viewLosa, List<FileInfo> ListaRutasArchivos, FileInfo ArchJsonSelec)
        {
            this.NombreElev = name;
            this.ViewElev = _viewLosa;
            this.DequienSeCopia = new ElevacionVarioSelected("ninguno");
            this.ListaRutasArchivos = ListaRutasArchivos;
            this.ArchJsonSeleccion = ArchJsonSelec;
            this.NombreJson = ArchJsonSelec.Name;
            this.IsSelected = true;
        }


        public bool ObtenerTipoDeView()
        {
            try
            {
                TipoView= ViewElev.ObtenerNombreTipoView("TIPO DE ESTRUCTURA (VISTA)");
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }



    public class ElevacionVarioSelected
    {
        private static int cont = 0;
        public ElevacionVarioSelected(string name)
        {
            Name = name;
            Value = ++cont;
        }


        public string Name { get; set; }
        public int Value { get; set; }
        public FileInfo infoArchivo { get; set; }
    }
}
