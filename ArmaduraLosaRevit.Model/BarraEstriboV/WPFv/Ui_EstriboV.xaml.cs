using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraEstriboV.WPFv
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_EstriboV : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;


        public ObservableCollection<float> Listaespacimiamiento { get; set; }
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }
        public ObservableCollection<string> ListaCasoDibujarEstribo { get; set; }
        public ObservableCollection<string> ListaTraba { get; set; }




        private bool _extenderIzq;
        public bool ExtenderIzq
        {
            get { return _extenderIzq; }
            set
            {
                if (_extenderIzq != value)
                {
                    _extenderIzq = value;
                    RaisePropertyChanged("ExtenderIzq");
                }
            }
        }

        private bool _extenderDere;
        public bool ExtenderDere
        {
            get { return _extenderDere; }
            set
            {
                if (_extenderDere != value)
                {
                    _extenderDere = value;
                    RaisePropertyChanged("ExtenderDere");
                }
            }
        }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_EstriboV(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;


            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            Listaespacimiamiento = new ObservableCollection<float>() { 5, 6, 7.5f, 8, 10, 12, 14, 15, 16, 18, 20, 22, 24, 25 };
            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            ListaEstribo = new ObservableCollection<string>() { "E.", "E.D.", "E.T.", "E.C." };

            ListaTraba = new ObservableCollection<string>() { "A", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            ExtenderIzq = false;
            ExtenderDere = false;
            this.Topmost = true;


            cbx_tipodeseñoEstriboVIga2.DropDownClosed += Cbx_tipodeseñoEstriboVIga2_DropDownClosed;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }



        internal DatosConfinamientoAutoDTO ObtenerPArametrosDIseño()
        {
            try
            {

                DatosConfinamientoAutoDTO configuracionInicialEstriboDTO =
               new DatosConfinamientoAutoDTO()
               {
                   DiamtroEstriboMM = Util.ConvertirStringInInteger(diam_estribo.Text),
                   espaciamientoEstriboCM = Util.ConvertirStringInDouble(espa_estribo.Text),
                   cantidadEstribo = tipo_estr.Text,
                   tipoConfiguracionEstribo = TipoConfiguracionEstribo.EstriboViga,
                   tipoEstriboGenera = TipoEstriboGenera.Eviga,

                   IsEstribo = chbox_estribo.IsChecked,

                   IsLateral = (bool)chbox_lat.IsChecked,
                   DiamtroLateralEstriboMM = Util.ConvertirStringInInteger(diam_lat.Text),
                   cantidadLaterales = Util.ConvertirStringInInteger(tipo_lat.Text),
                   IsExtenderLatInicio = (bool)chbox_extenIni.IsChecked,
                   IsExtenderLatFin = (bool)chbox_extenFin.IsChecked,

                   IsTraba = chbox_traba.IsChecked,
                   cantidadTraba = Util.ConvertirStringInInteger(cantidad_traba.Text),
                   DiamtroTrabaEstriboMM = Util.ConvertirStringInInteger(diam_traba.Text),
                   espaciamientoTrabaCM = Util.ConvertirStringInDouble(espa_estribo.Text),
                   TipoDiseñoEstriboViga= EnumeracionBuscador.ObtenerEnumGenerico(TipoDisenoEstriboVIga.NONE, cbx_tipodeseñoEstriboVIga2.Text.Replace(" ","").Replace(".", ""))

               };

                return  configuracionInicialEstriboDTO;

            }
            catch (Exception)
            {
                return null;

            }
        }



        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        #region External Project Methods

        private void BExtString_Click(object sender, RoutedEventArgs e)
        {
            // Raise external event with a string argument. The string MAY
            // be pulled from a Revit API context because this is an external event
            _mExternalMethodStringArg.Raise($"Title: {_doc.Title}");
        }



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

        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;

            if (bton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;
            }

            BotonOprimido = bton.Name;

            if (BotonOprimido != "btnCerrar_Viga")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else 
            {
                Close();
            }

        }


        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

   
        private void Cbx_tipodeseñoEstriboVIga2_DropDownClosed(object sender, EventArgs e)
        {
            if (cbx_tipodeseñoEstriboVIga2.Text == "Seleccionar Viga")
            {
                chbox_barraHSup.IsEnabled = true;
                chbox_barraHInf.IsEnabled = true;
            }
            else
            {
                chbox_barraHSup.IsChecked = false;
                chbox_barraHSup.IsEnabled = false;

                chbox_barraHInf.IsChecked = false;
                chbox_barraHInf.IsEnabled = false;
            }
        }

        private void Rdbton_Editar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_estr.IsEnabled = false;
            tipo_lat.IsEnabled = false;
            cantidad_traba.IsEnabled = false;
        }

        private void Rdbton_dibujar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_estr.IsEnabled = true;
            tipo_lat.IsEnabled = true;
            cantidad_traba.IsEnabled = true;
        }
    }


}