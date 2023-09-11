using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO
{
    public class ViewDTO
    {


        public string Nombre { get; set; }
        public bool IsSelected { get; set; }
        public View View_ { get; set; }
        public ExtensionViewDTO ValorZ { get; private set; }


        //****para dibujar sheet
        public string NombreSheetInicial { get; private set; } // no se cambia 
        public string NumeroSheetInicial { get; private set; } // no se cambia 

        public List<ViewSheetNH> ListaSheet { get; }
        //******************


        private List<string> _listaSheetNombre;
        public List<string> ListaSheetNombre
        {
            get { return _listaSheetNombre; }
            set
            {
                if (_listaSheetNombre != value)
                {
                    _listaSheetNombre = value;
                    RaisePropertyChanged("ListaSheetNombre");
                }
            }
        }
        //***
        public List<string> _listaSheetNumero;
        public List<string> ListaSheetNumero
        {
            get { return _listaSheetNumero; }
            set
            {
                if (_listaSheetNumero != value)
                {
                    _listaSheetNumero = value;
                    RaisePropertyChanged("ListaSheetNumero");
                }
            }
        }
        //**
        private string _numeroSheet;
        public string NumeroSheet
        {
            get { return _numeroSheet; }
            set
            {
                if (_numeroSheet != value)
                {
                    _numeroSheet = value;
                    RaisePropertyChanged("NumeroSheet");
                }
            }
        }
        //******************
        private string _nombreSheetEditado;
        public string NombreSheetEditado
        {
            get { return _nombreSheetEditado; }
            set
            {
                if (_nombreSheetEditado != value)
                {
                    _nombreSheetEditado = value;
                    RaisePropertyChanged("NombreSheetEditado");
                }
            }
        }



        public ViewDTO(View c)
        {
            this.View_ = c;
            this.Nombre = c.Name;
        }
        public ViewDTO(View c, List<ViewSheetNH> listaSheet, List<string> _ListaSheetNombre, List<string> listaSheetNumero)
        {
            this.View_ = c;
            this.ListaSheet = listaSheet;
            this.ListaSheetNombre = _ListaSheetNombre;
            this.ListaSheetNumero = listaSheetNumero;
            this.Nombre = c.Name;
            this.ValorZ = c.Obtener_Z_SoloPLantas(false);
        }


        public bool BuscarSiVIewInSheet()
        {
            try
            {
                // ListaSheetNombre = ListaSheet.Select(c => c.NombreVista).ToList();
                if (!ListaSheetNombre.Contains(View_.Name))
                    ListaSheetNombre.Add(View_.Name);
                var sheetEncontrado = ListaSheet.Where(c => c.listaPortInSheet.Exists(vp => vp.Name == Nombre)).FirstOrDefault();
                if (sheetEncontrado != null)
                {
                    NombreSheetInicial = sheetEncontrado.NombreVista;
                    NombreSheetEditado = sheetEncontrado.NombreVista.Replace(sheetEncontrado.SheetNumber + " -", "").Replace(sheetEncontrado.SheetNumber + "-", "");

                    NumeroSheetInicial = sheetEncontrado.SheetNumber;
                    NumeroSheet = sheetEncontrado.SheetNumber;
                }
                else
                {
                    NombreSheetInicial = View_.Name;
                    NombreSheetEditado = View_.Name;

                    NumeroSheetInicial = "";
                    NumeroSheet = "";
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BuscarSiVIewInSheet'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public ViewSheetNH ObtenerViewSheetNH()
        {
            ViewSheetNH _ViewSheetNHActual = default;
            return _ViewSheetNHActual;
        }

        //**********************************************************************************************************
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }




        #endregion
    }
}
