using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace ArmaduraLosaRevit.Model.BarraV.Automatico.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_VariasElevAuto : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;



        public ObservableCollection<ViewSelected> _listaTodasView_general { get; set; }
        public ObservableCollection<ViewSelected> ListaTodasView_general
        {
            get { return _listaTodasView_general; }
            set
            {
                if (value == null) return; _listaTodasView_general = value;
               // RaisePropertyChanged("ListaTodasView");

            }

        }
        //1)
        public ObservableCollection<LevelDTO> _listaLevel;
        public ObservableCollection<LevelDTO> ListaLevel
        {
            get { return _listaLevel; }
            set
            {
                _listaLevel = value;
                RaisePropertyChanged("ListaLevel");
            }
        }

        //2) losas
        public ObservableCollection<LosaCubDto> _listaLosas;
        public ObservableCollection<LosaCubDto> ListaLosa
        {
            get { return _listaLosas; }
            set
            {
                _listaLosas = value;
                RaisePropertyChanged("ListaLosa");
            }
        }

        //3) Elev
        public ObservableCollection<ElevacionVAriosDto> _listaElev;
        public ObservableCollection<ElevacionVAriosDto> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaElev");
            }
        }

        //4) Elev
        private int _cantidadView;
        public int CantidadView
        {
            get { return _cantidadView; }
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
            set { if (value == null) return;
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
                _cantidadView = value;
                RaisePropertyChanged("CantidadView"); }
        }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        private readonly List<ElevacionVAriosDto> listaElevacionVAriosDto_;


        // private readonly List<ElevacionVAriosDto> listaArchivos;

        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public List<ViewSelected> Listainicial { get; private set; }
        public ObservableCollection<string> ListaView3d { get; private set; }

        public ObservableCollection<string> ListaNombreTipoElev { get; set; }
    
        public string SelectView3d { get; set; }
        public string ActualView3d { get; set; }
        public Ui_AutoElev Ui_AutoElev { get; }
        public string rutaGuardarArchivos { get; internal set; }

        bool IStodosElev;
        public UI_VariasElevAuto(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg, List<ElevacionVAriosDto> ListaElevacionVAriosDto_,
             Ui_AutoElev ui_AutoElev , List<string> listaTipoView,string mpathDirecotrio)
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
            listaElevacionVAriosDto_ = ListaElevacionVAriosDto_;
            Ui_AutoElev = ui_AutoElev;
            this.rutaGuardarArchivos = mpathDirecotrio;

            //  this.listaArchivos = ListaElevacionVAriosDto_;
            ListaElev = new ObservableCollection<ElevacionVAriosDto>(ListaElevacionVAriosDto_);
            IStodosElev = true;
            listaTipoView.Add("Todos");
            listaTipoView.Reverse();
            ListaNombreTipoElev = new ObservableCollection<string>(listaTipoView) ;

            CantidadView = ListaElevacionVAriosDto_.Count;
            // LoadData();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }
        #region External Project Methods


        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;
            BotonOprimido = bton.Name;
       
            // Raise external event with this UI instance (WPF) as an argument
            if (_mExternalMethodWpfArg != null)
                _mExternalMethodWpfArg.Raise(this);
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



        private void CambiarValor(ObservableCollection<ViewDTO> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsSelected = valor;
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var cbx=(System.Windows.Controls.ComboBox)sender;
            var nombre=cbx.Text;
        }

        private void ComboBox_DropDownClosed_1(object sender, EventArgs e)
        {
            var cbx = (System.Windows.Controls.ComboBox)sender;
            
            var _SelectedItem = cbx.SelectedItem;
            var _DisplayMemberPath = cbx.DisplayMemberPath;
            var _SelectedValue = cbx.SelectedValue;
            var _SelectedValuePath = cbx.SelectedValuePath;




        }

        private void Cambiar(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Name == "SelecAllVista")
            {
                IStodosElev = !IStodosElev;
                cambiarEstado(ListaElev, IStodosElev);
            }
        }

        private void cambiarEstado(ObservableCollection<ElevacionVAriosDto> lista, bool estado)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].IsSelected = estado;
            }
        }

        private void CambiarTipo(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CambiarTipo_down(object sender, EventArgs e)
        {
            var cbx = (System.Windows.Controls.ComboBox)sender;

            if (cbx.Text == "Todos")
            {

                CantidadView = listaElevacionVAriosDto_.Count;
                ListaElev = new ObservableCollection<ElevacionVAriosDto>(listaElevacionVAriosDto_);
            }
            else
            {
                var tipoView = listaElevacionVAriosDto_.Where(c => c.TipoView == cbx.Text).ToList();
                CantidadView = tipoView.Count;
                ListaElev = new ObservableCollection<ElevacionVAriosDto>(tipoView);
            }

        }
    }


}