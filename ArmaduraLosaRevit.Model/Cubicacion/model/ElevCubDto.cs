using Autodesk.Revit.DB;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Cubicacion.model
{
    public class ElevCubDto : INotifyPropertyChanged
    {
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                Debug.WriteLine($"IsSelected:{IsSelected}");
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }


        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (value == null)
                {
                    Debug.WriteLine($"Nombre:{null}");
                    return;
                }
                else
                    Debug.WriteLine($"Nombre:{value}");

                nombre = value; NotifyPropertyChanged("Nombre");
            }
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
            set
            {
                if (value == null) return;
                _viewLosa = value;
                NotifyPropertyChanged("ViewLosa");
            }
        }


        private ObservableCollection<ViewSelected> _listaTodasView;
        public ObservableCollection<ViewSelected> ListaTodasView
        {
            get { return _listaTodasView; }
            set
            {
                if (value == null) return;
                _listaTodasView = value;
                NotifyPropertyChanged("ListaTodasView");
            }
        }

        private ViewSelected _dequienSeCopia;
        public ViewSelected DequienSeCopia
        {
            get { return _dequienSeCopia; }
            set
            {
                if (value == null) return;
                _dequienSeCopia = value;
                NotifyPropertyChanged("DequienSeCopia");
            }
        }

        private string _dequienSeCopiaStr;
        public string DequienSeCopiaStr
        {
            get { return _dequienSeCopiaStr; }
            set
            {
                if (value == null) return;
                _dequienSeCopiaStr = value;
                NotifyPropertyChanged("DequienSeCopiaStr");
            }
        }


        public ElevCubDto(string name, View _viewLosa, ObservableCollection<ViewSelected> listaTodasLosaList)
        {
            this.Nombre = name;
            this.ViewLosa = _viewLosa;
          
            if (_viewLosa.Name.Contains("="))
                this.DequienSeCopiaStr = "=";
            else
                this.DequienSeCopiaStr = "";
            //this.DequienSeCopia = new ViewSelected("ninguno");
            this.ListaTodasView = new ObservableCollection<ViewSelected>(listaTodasLosaList);
            IsSelected = true;
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




}
