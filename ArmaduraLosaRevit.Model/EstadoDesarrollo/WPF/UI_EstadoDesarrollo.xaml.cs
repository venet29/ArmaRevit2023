using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_EstadoDesarrollo : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;

        public ObservableCollection<viewNH> _listaEstructura;
        public ObservableCollection<viewNH> ListaEstructura
        {
            get { return _listaEstructura; }
            set
            {
                _listaEstructura = value;
                RaisePropertyChanged("ListaEstructura");
            }
        }


        public ObservableCollection<viewNH> _listalosa;
        public ObservableCollection<viewNH> ListaLosa
        {
            get { return _listalosa; }
            set
            {
                _listalosa = value;
                RaisePropertyChanged("ListaLosa");
            }
        }

        public ObservableCollection<viewNH> _listaElev;
        public ObservableCollection<viewNH> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaElev");
            }
        }

        public ObservableCollection<viewNH> _listaSheet;
        public ObservableCollection<viewNH> ListaSheet
        {
            get { return _listaSheet; }
            set
            {
                _listaSheet = value;
                RaisePropertyChanged("ListaSheet");
            }
        }

        public string _stringEstructuraView;
        public string StringEstructuraView
        {
            get { return _stringEstructuraView; }
            set
            {
                _stringEstructuraView = value;
                RaisePropertyChanged("StringEstructuraView");
            }
        }
        public string _stringLosaView;
        public string StringLosaView
        {
            get { return _stringLosaView; }
            set
            {
                _stringLosaView = value;
                RaisePropertyChanged("StringLosaView");
            }
        }

        //**
        public string _stringElevView;
        public string StringElevView
        {
            get { return _stringElevView; }
            set
            {
                _stringElevView = value;
                RaisePropertyChanged("StringElevView");
            }
        }
        //**
        public string _stringSheetView;
        public string StringSheetView
        {
            get { return _stringSheetView; }
            set
            {
                _stringSheetView = value;
                RaisePropertyChanged("StringSheetView");
            }
        }

        public string _stringTotalView;
        public string StringTotalView
        {
            get { return _stringTotalView; }
            set
            {
                _stringTotalView = value;
                RaisePropertyChanged("StringTotalView");
            }
        }

        public string EstadoAvance { get; set; }

        public EstadoTipoView EstadoVIewEstructura { get; set; }
        public EstadoTipoView EstadoVIewLosa { get; set; }
        public EstadoTipoView EstadoVIewElevacion { get; set; }

        public EstadoTipoView EstadoVIewSheet { get; set; }

        public EstadoTipoView EstadoVIewTotal { get; set; }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public ManejadorEstadoDesarrollo ManejadorEstadoDesarrollo_ { get; internal set; }

        public UI_EstadoDesarrollo(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiapp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            EstadoVIewElevacion = new EstadoTipoView();
            EstadoVIewLosa = new EstadoTipoView();
            EstadoVIewEstructura = new EstadoTipoView();
            EstadoVIewSheet = new EstadoTipoView();
            EstadoVIewTotal = new EstadoTipoView();
            StringTotalView= $"Cantidad Total View:{EstadoVIewTotal.POrTerminar} de {EstadoVIewTotal.Total}";
            StringEstructuraView = $"Estructura";
            StringLosaView = $"Losa";
            StringElevView = $"Elevacion";
            StringSheetView = $"Sheet";
            LoadData();
        }

        private bool LoadData()
        {
            try
            {
                ManejadorEstadoDesarrollo_ = new ManejadorEstadoDesarrollo(_uiapp);
                ManejadorEstadoDesarrollo_.Cargar();
                ManejadorEstadoDesarrollo_.Ejecutar();

                var lsiatTerminado=ManejadorEstadoDesarrollo_.ActualProyecto.ListasView.Where(c => c.IsTerminado).ToList();
                // estructuras
                string PArametroBusqueda_estruc = _nombreParametro_estruc.Text;
                ListaEstructura = new ObservableCollection<viewNH>(ManejadorEstadoDesarrollo_.ActualProyecto.ListasView.Where(c => c.TipoView == ViewType.CeilingPlan &&
                                                                                                                              c.TipoEstructura.Contains(PArametroBusqueda_estruc))
                                                                                                                             .OrderBy(c => c.NombreVista));
                //losa

                string PArametroBusqueda_armadura = _nombreParametro_armadu.Text;
                ListaLosa = new ObservableCollection<viewNH>(ManejadorEstadoDesarrollo_.ActualProyecto.ListasView.Where(c => c.TipoView == ViewType.FloorPlan &&
                                                                                                                           c.TipoEstructura.Contains(PArametroBusqueda_armadura))
                                                                                                                                      .OrderBy(c => c.NombreVista));
                //elevaciones

                string PArametroBusqueda_Elev = _nombreParametro_elev.Text;
                ListaElev = new ObservableCollection<viewNH>(ManejadorEstadoDesarrollo_.ActualProyecto.ListasView.Where(c => c.TipoView == ViewType.Section &&
                                                                                            c.TipoEstructura.Contains(PArametroBusqueda_Elev))
                                                                                            .OrderBy(c => c.NombreVista));
                //sheet

                ListaSheet =  new ObservableCollection<viewNH>(ManejadorEstadoDesarrollo_.ActualProyecto.ListasView.Where(c => c.TipoView == ViewType.DrawingSheet)
                                                                                            .OrderBy(c => c.NombreVista));
                if(ListaSheet.Count>0)
                    EstadoAvance ="Avance : "+ ParameterUtil.FindValueParaByName( _doc.GetElement(new ElementId( ListaSheet.First()._viewid)), "ESTADO DE AVANCE", _doc); 

                CargarTotales();

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        public void CargarTotales()
        {
            EstadoVIewEstructura.Total = ListaEstructura.Count();
            EstadoVIewEstructura.Termindas = ListaEstructura.Count(c => c.IsTerminado);
            EstadoVIewEstructura.POrTerminar = ListaEstructura.Count(c => !c.IsTerminado);
            StringEstructuraView= $"Estructura ({EstadoVIewEstructura.Termindas}/{EstadoVIewEstructura.Total})";


            EstadoVIewLosa.Total = ListaLosa.Count();
            EstadoVIewLosa.Termindas = ListaLosa.Count(c => c.IsTerminado);
            EstadoVIewLosa.POrTerminar = ListaLosa.Count(c => !c.IsTerminado);
            StringLosaView = $"Losa ({EstadoVIewLosa.Termindas}/{EstadoVIewLosa.Total})";


            EstadoVIewElevacion.Total = ListaElev.Count();
            EstadoVIewElevacion.Termindas = ListaElev.Count(c => c.IsTerminado);
            EstadoVIewElevacion.POrTerminar = ListaElev.Count(c => !c.IsTerminado);
            StringElevView = $"Elevacion ({EstadoVIewElevacion.Termindas}/{EstadoVIewElevacion.Total})";


            EstadoVIewSheet.Total = ListaSheet.Count();
            EstadoVIewSheet.Termindas = ListaSheet.Count(c => c.IsTerminado);
            EstadoVIewSheet.POrTerminar = ListaSheet.Count(c => !c.IsTerminado);
            StringSheetView = $"Sheet ({EstadoVIewSheet.Termindas}/{EstadoVIewSheet.Total})";


            EstadoVIewTotal.Total = EstadoVIewEstructura.Total + EstadoVIewLosa.Total + EstadoVIewElevacion.Total+ EstadoVIewSheet.Total;
            EstadoVIewTotal.Termindas = EstadoVIewEstructura.Termindas + EstadoVIewLosa.Termindas + EstadoVIewElevacion.Termindas + EstadoVIewSheet.Termindas;
            EstadoVIewTotal.POrTerminar = EstadoVIewEstructura.POrTerminar + EstadoVIewLosa.POrTerminar + EstadoVIewElevacion.POrTerminar + EstadoVIewSheet.POrTerminar;

            StringTotalView = $"Cantidad : {EstadoVIewTotal.Termindas} de {EstadoVIewTotal.Total}";

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void RecargarView(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        #region External Project Methods


        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {


            Button bton = (Button)sender;

            BotonOprimido = bton.Name;
            if (BotonOprimido.Contains("GuardarInfo"))
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);

            }
            else
                Close();
        }



        #endregion

        #region Non-External Project Methods







        #endregion


        //**********************************************************************************************************
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }




        #endregion

        private void Cerrar_estruc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Check todos 
        private void SeleccionTodos_elev_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, true);
            ListaElev = new ObservableCollection<viewNH>(ListaElev);
        }
        private void SeleccionTodos_elev_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, false);
            ListaElev = new ObservableCollection<viewNH>(ListaElev);
        }

        private void SeleccionTodos_planta_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, true);
            ListaLosa = new ObservableCollection<viewNH>(ListaLosa);
        }
        private void SeleccionTodos_planta_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, false);
            ListaLosa = new ObservableCollection<viewNH>(ListaLosa);
        }

        private void SeleccionTodos_Sheet_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaSheet, true);
            ListaSheet = new ObservableCollection<viewNH>(ListaSheet);
        }
        private void SeleccionTodos_sheet_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaSheet, false);
            ListaSheet = new ObservableCollection<viewNH>(ListaSheet);
        }



        private void SeleccionTodos_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, true);
            ListaEstructura = new ObservableCollection<viewNH>(ListaEstructura);
        }

        private void SeleccionTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, false);
            ListaEstructura = new ObservableCollection<viewNH>(ListaEstructura);
        }

        private void CambiarValor(ObservableCollection<viewNH> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsTerminado = valor;
            }

            CargarTotales();
        }
        #endregion
    }


}