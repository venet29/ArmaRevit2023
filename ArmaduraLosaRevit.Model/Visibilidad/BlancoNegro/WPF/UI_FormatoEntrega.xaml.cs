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
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_FormatoEntrega : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;

        public ObservableCollection<ViewDTO> _listaEstructura;
        public ObservableCollection<ViewDTO> ListaEstructura
        {
            get { return _listaEstructura; }
            set
            {
                _listaEstructura = value;
                RaisePropertyChanged("ListaEstructura");
            }
        }


        public ObservableCollection<ViewDTO> _listalosa;
        public ObservableCollection<ViewDTO> ListaLosa
        {
            get { return _listalosa; }
            set
            {
                _listalosa = value;
                RaisePropertyChanged("ListaLosa");
            }
        }

        public ObservableCollection<ViewDTO> _listaElev;
        public ObservableCollection<ViewDTO> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaElev");
            }
        }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public UI_FormatoEntrega(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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
            LoadData();
        }

        private void LoadData()
        {

          

            var aux_ListaEstructura = SeleccionarView.ObtenerView_losa_elev_fund_estructura(_uiapp.ActiveUIDocument);

            // estructuras
            string PArametroBusqueda_estruc = _nombreParametro_estruc.Text;
            ListaEstructura = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => c.ViewType == ViewType.CeilingPlan &&
                                                                                              c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_estruc))
                                                                                    .Select(c => new ViewDTO(c)).OrderBy(c=>c.Nombre));
            //losa
            string PArametroBusqueda_armadura = _nombreParametro_armadu.Text;
            ListaLosa = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => c.ViewType == ViewType.FloorPlan &&
                                                                                         (c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_armadura)))
                                                                                    .Select(c => new ViewDTO(c)).OrderBy(c => c.Nombre));

            //elevaciones
            string PArametroBusqueda_Elev = _nombreParametro_elev.Text;
            ListaElev = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => c.ViewType == ViewType.Section &&
                                                                                         c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_Elev))
                                                                                    .Select(c => new ViewDTO(c)).OrderBy(c => c.Nombre));
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
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
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

        private void SeleccionTodos_elev_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, true);
            ListaElev = new ObservableCollection<ViewDTO>(ListaElev);
        }
        private void SeleccionTodos_elev_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, false);
            ListaElev = new ObservableCollection<ViewDTO>(ListaElev);
        }

        private void CambiarValor(ObservableCollection<ViewDTO> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsSelected = valor;
            }



        }

        private void SeleccionTodos_planta_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, true);
            ListaLosa = new ObservableCollection<ViewDTO>(ListaLosa);
        }
        private void SeleccionTodos_planta_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, false);
            ListaLosa = new ObservableCollection<ViewDTO>(ListaLosa);
        }
        private void SeleccionTodos_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, true);
            ListaEstructura = new ObservableCollection<ViewDTO>(ListaEstructura);
        }

        private void SeleccionTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, false);
            ListaEstructura = new ObservableCollection<ViewDTO>(ListaEstructura);
        }


    }


}