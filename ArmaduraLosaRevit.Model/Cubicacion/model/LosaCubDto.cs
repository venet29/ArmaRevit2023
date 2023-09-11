using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.model
{
    public class LosaCubDto : INotifyPropertyChanged
    {

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set {  isSelected = value; NotifyPropertyChanged("IsSelected"); }
        }


        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { if (value == null) return;  nombre = value; NotifyPropertyChanged("Nombre"); }
        }

        private string nivel;
        public string Nivel
        {
            get { return nivel; }
            set { if (value == null) return; nivel = value; NotifyPropertyChanged("Nivel"); }
        }


        private View _viewLosa;
        public View ViewLosa
        {
            get { return _viewLosa; }
            set { if (value == null) return; _viewLosa = value; NotifyPropertyChanged("ViewLosa"); }
        }


        private List<ViewSelected> _listaTodasView;
        public List<ViewSelected> ListaTodasView
        {
            get { return _listaTodasView; }
            set { if (value == null) return; _listaTodasView = value; NotifyPropertyChanged("ListaTodasView"); }
        }

        private ViewSelected _dequienSeCopia;
        public ViewSelected DequienSeCopia
        {
            get { return _dequienSeCopia; }
            set {
                if (value == null) return;

                _dequienSeCopia = value;
                NotifyPropertyChanged("DequienSeCopia"); }
        }


        public LosaCubDto(string name, View _viewLosa, List<ViewSelected> listaTodasLosaList)
        {
            this.Nombre = name;
            this.ViewLosa = _viewLosa;
            this.DequienSeCopia = new ViewSelected("ninguno");
            this.ListaTodasView = listaTodasLosaList;
            this.IsSelected = true;
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



    public class ViewSelected
    {
        private static int cont = 0;
        public ViewSelected(string name)
        {
            Name = name;
            Value = ++cont;
        }
        public ViewSelected()
        {
            Name = "ninguno";
            Value = ++cont;
        }
       
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
