using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Model;
using ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Servicios;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.GRIDS.WPF_CambiarNombreGrid
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_CambiarNombreGrid : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        public ViewGeom viewGeomSelec { get; set; }

        private readonly Document _doc;

        private UIApplication _uiapp;




        public ObservableCollection<EnvoltorioGrid_view> _listaElev;
        public ObservableCollection<EnvoltorioGrid_view> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaElev");
            }
        }


        //************************ horizontal
        public ObservableCollection<EnvoltorioGrid_view> _listaGridHorizontal;
        public ObservableCollection<EnvoltorioGrid_view> ListaGridHorizontal
        {
            get { return _listaGridHorizontal; }
            set
            {
                _listaGridHorizontal = value;
                RaisePropertyChanged("ListaGridHorizontal");
            }
        }

        //************************ vertical
        public ObservableCollection<EnvoltorioGrid_view> _listaGridVerticales;
        public ObservableCollection<EnvoltorioGrid_view> ListaGridVerticales
        {
            get { return _listaGridVerticales; }
            set
            {
                _listaGridVerticales = value;
                RaisePropertyChanged("ListaGridVerticales");
            }
        }

        //************************ otros
        public ObservableCollection<EnvoltorioGrid_view> _listaGridOtros;
        public ObservableCollection<EnvoltorioGrid_view> ListaGridOtros
        {
            get { return _listaGridOtros; }
            set
            {
                _listaGridOtros = value;
                RaisePropertyChanged("ListaGridOtros");
            }
        }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public ServicioBuscarViewContendioEnGrid _ServicioBuscarViewContendioEnGrid { get; set; }

        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public UI_CambiarNombreGrid(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg, ServicioBuscarViewContendioEnGrid _ServicioBuscarViewContendioEnGrid)
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
            this._ServicioBuscarViewContendioEnGrid = _ServicioBuscarViewContendioEnGrid;
            SeleccionTodos_elevHorizontal.IsChecked = true;
            SeleccionTodos_elevVertical.IsChecked = true;
            SeleccionTodos_elevOtro.IsChecked = true;

            LoadData();

            SeleccionTodos_elevHorizontal.Checked += SeleccionTodos_elev_Checked;
            SeleccionTodos_elevHorizontal.Unchecked += SeleccionTodos_elev_Unchecked;

            SeleccionTodos_elevVertical.Checked += SeleccionTodos_elev_Checked;
            SeleccionTodos_elevVertical.Unchecked += SeleccionTodos_elev_Unchecked;

            SeleccionTodos_elevOtro.Checked += SeleccionTodos_elev_Checked;
            SeleccionTodos_elevOtro.Unchecked += SeleccionTodos_elev_Unchecked;
        }

        private void LoadData()
        {
            ListaElev = new ObservableCollection<EnvoltorioGrid_view>(_ServicioBuscarViewContendioEnGrid.Lista_EnvoltorioGrid_view);
            ListaGridHorizontal = new ObservableCollection<EnvoltorioGrid_view>(_ServicioBuscarViewContendioEnGrid.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Horizonal).OrderBy(c => c.CoordeParaOrden));
            double valorMin = 0;
            if (ListaGridHorizontal.Count > 0)
            {
                valorMin = ListaGridHorizontal.Min(c => c.CoordeParaOrden);
                ListaGridHorizontal.ForEach(c => { c.CoordeParaOrden = (c.CoordeParaOrden - valorMin) * 30.48; });
            }

            ListaGridVerticales = new ObservableCollection<EnvoltorioGrid_view>(_ServicioBuscarViewContendioEnGrid.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Vertical).OrderBy(c => c.CoordeParaOrden));
            if (ListaGridVerticales.Count > 0)
            {
                valorMin = ListaGridVerticales.Min(c => c.CoordeParaOrden);
                ListaGridVerticales.ForEach(c => { c.CoordeParaOrden = (c.CoordeParaOrden - valorMin) * 30.48; });
            }

            ListaGridOtros = new ObservableCollection<EnvoltorioGrid_view>(_ServicioBuscarViewContendioEnGrid.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Otros).OrderBy(c => c.CoordeParaOrden));

            if (ListaGridOtros.Count > 0)
            {
                valorMin = ListaGridOtros.Min(c => c.CoordeParaOrden);
                ListaGridOtros.ForEach(c => { c.CoordeParaOrden = (c.CoordeParaOrden - valorMin) * 30.48; });
            }
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
            if (BotonOprimido == "bton_Horizontal" || BotonOprimido == "bton_cambiarVertical" || BotonOprimido == "bton_cambiarOtro")
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

        private void SeleccionTodos_elev_Checked(object sender, RoutedEventArgs e)
        {
            var selecc = sender as CheckBox;
            if (selecc == null) return;

            if (selecc.Name == "SeleccionTodos_elevHorizontal")
            {
                CambiarValor(ListaGridHorizontal, true);
                ListaGridHorizontal = new ObservableCollection<EnvoltorioGrid_view>(ListaGridHorizontal);
            }
            else if (selecc.Name == "SeleccionTodos_elevVertical")
            {
                CambiarValor(ListaGridVerticales, true);
                ListaGridVerticales = new ObservableCollection<EnvoltorioGrid_view>(ListaGridVerticales);
            }
            else if (selecc.Name == "SeleccionTodos_elevOtro")
            {
                CambiarValor(ListaGridOtros, true);
                ListaGridOtros = new ObservableCollection<EnvoltorioGrid_view>(ListaGridOtros);
            }

        }
        private void SeleccionTodos_elev_Unchecked(object sender, RoutedEventArgs e)
        {
            var selecc = sender as CheckBox;
            if (selecc == null) return;

            if (selecc.Name == "SeleccionTodos_elevHorizontal")
            {
                CambiarValor(ListaGridHorizontal, false);
                ListaGridHorizontal = new ObservableCollection<EnvoltorioGrid_view>(ListaGridHorizontal);
            }
            else if (selecc.Name == "SeleccionTodos_elevVertical")
            {
                CambiarValor(ListaGridVerticales, false);
                ListaGridVerticales = new ObservableCollection<EnvoltorioGrid_view>(ListaGridVerticales);
            }
            else if (selecc.Name == "SeleccionTodos_elevOtro")
            {
                CambiarValor(ListaGridOtros, false);
                ListaGridOtros = new ObservableCollection<EnvoltorioGrid_view>(ListaGridOtros);
            }
        }

        private void CambiarValor(ObservableCollection<EnvoltorioGrid_view> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsSelected = valor;
            }



        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid1Horizontal_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            string dataGridName = dataGrid.Name;

            var direc = e.Column.SortDirection;
            var columnName = e.Column.Header.ToString();


            //if (e.Column.DataGridOwner.Name== "dataGrid1Horizontal")

            if (columnName == "esto no corre pero sirve de ejemplo")
            {
                if (direc == null || direc == ListSortDirection.Descending)
                {
                    if ("dataGrid1Horizontal" == dataGridName)
                        ListaGridHorizontal = new ObservableCollection<EnvoltorioGrid_view>(ListaGridHorizontal.OrderBy(c => c.CoordeParaOrden));
                    else if ("dataGrid1Horizontal" == dataGridName)
                        ListaGridVerticales = new ObservableCollection<EnvoltorioGrid_view>(ListaGridVerticales.OrderBy(c => c.CoordeParaOrden));
                    else
                        ListaGridOtros = new ObservableCollection<EnvoltorioGrid_view>(ListaGridOtros.OrderBy(c => c.CoordeParaOrden));
                    //asce
                }
                else if (direc == ListSortDirection.Ascending)
                {
                    //realmente es desce
                    if ("dataGrid1Horizontal" == dataGridName)
                        ListaGridHorizontal = new ObservableCollection<EnvoltorioGrid_view>(ListaGridHorizontal.OrderByDescending(c => c.CoordeParaOrden));
                    else if ("dataGrid1Horizontal" == dataGridName)
                        ListaGridVerticales = new ObservableCollection<EnvoltorioGrid_view>(ListaGridVerticales.OrderByDescending(c => c.CoordeParaOrden));
                    else
                        ListaGridOtros = new ObservableCollection<EnvoltorioGrid_view>(ListaGridOtros.OrderByDescending(c => c.CoordeParaOrden));
                }

            }


        }

        private void dataGrid1Horizontal_ColumnReordering(object sender, DataGridColumnReorderingEventArgs e)
        {
            var direc = e.Column.SortDirection;
            var columnName = e.Column.Header.ToString();
            if (columnName == "")
            { }
        }

        private void OnPrenderView(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as RadioButton;
            if (dataGrid == null) return;
            var resul = dataGrid.DataContext as EnvoltorioViewAsosciadoGrid;
            if(resul==null) return;
            viewGeomSelec = resul.viewGeom;
            BotonOprimido = "VisulaizarVIew";


            _mExternalMethodWpfArg.Raise(this);
        }
        private void OnOcutarView(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as RadioButton;
            if (dataGrid == null) return;
            var resul = dataGrid.DataContext as EnvoltorioViewAsosciadoGrid;
            if (resul == null) return;
            viewGeomSelec = resul.viewGeom;
            BotonOprimido = "OCultarVIew";


            _mExternalMethodWpfArg.Raise(this);
        }
        
    }
}