using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.WPFm
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_Malla : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;


        public ObservableCollection<float> Listaespacimiamiento { get; set; }
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }

        public ObservableCollection<string> ListaMalla { get; set; }
        public ObservableCollection<int> ListaTraba { get; set; }


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;


        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_Malla(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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
            ListaDiam = new ObservableCollection<float>() { 6,8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            ListaEstribo = new ObservableCollection<string>() { "E.", "E.D.", "E.T.", "E.C." };

            ListaTraba = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            ListaMalla = new ObservableCollection<string>() { "Ambos", "Vertical", "Horizontal" };

            tipo_mallaV.Visibility = System.Windows.Visibility.Hidden;

            this.Topmost = true;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }

        public ConfiguracionIniciaWPFlBarraVerticalDTO ObtenerConfiguracionInicialMallaMuroVDTO()
        {
          return  new ConfiguracionIniciaWPFlBarraVerticalDTO()
            {
                Inicial_Cantidadbarra = ObtenerNUmeroMallas(tipo_mallaH.Text),//tipo_mallaV.Text   pq hay dos mallas si fueran 3 mallas serian  '2+2+2', 
                incial_ComoIniciarTraslapo_LineaPAr = 1,
                incial_ComoIniciarTraslapo_LineaImpar = 1,//barra incio
                inicial_ComoTraslapo = 1,
                inicial_diametroMM = Util.ConvertirStringInInteger(diam_mallaV.Text),
                Document_ = _doc,
                inicial_tipoBarraV = Enumeraciones.TipoPataBarra.BarraVSinPatas,
                inicial_IsDirectriz = false,
                inicial_ISIntercalar = false,
                CasoAnalisasBarrasElevacion_ = CasoAnalisasBarrasElevacion.Manual,
                Inicial_espacienmietoCm_EntreLineasBarras = espa_mallaV.Text,
                TipoSeleccionMousePtoInferior = (rbt_ini_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                TipoSeleccionMousePtoSuperior = (rbt_sup_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                IsDibujarTag = false,
                TipoBarraRebar_ = TipoBarraVertical.MallaV,
                TipoBarraRebarHorizontal_ = TipoBarraVertical.MallaH,
                TipoSelecion = (cbx_text_TipoSeleccionHorizontal.Text == "Elemento" ? TipoSeleccion.ConElemento : TipoSeleccion.ConMouse) //se ocupa esta seleccio pq es la que siempre se utilizo

            };
        }

        public string ObtenerNUmeroMallas(string  txt)
        {
            switch (txt)
            {
                case "E.":
                    return "2";
                case "E.D.":
                    return "2+2";
                case "E.T.":
                    return "2+2+2";
                case "E.C.":
                    return "2+2+2+2";
                default:
                    return "2";
            }
        }

        public DatosMallasAutoDTO ObtenerDatosMallasDTO()
        {
            DatosMallasAutoDTO datosMallasDTO = new DatosMallasAutoDTO()
            {
                diametroH = Util.ConvertirStringInInteger(diam_mallaH.Text),
                diametroV = Util.ConvertirStringInInteger(diam_mallaV.Text),
                paraCantidadLineasV = ObtenerNUmeroMallas(tipo_mallaH.Text),//tipo_mallaV.Text
                paraCantidadLineasH = ObtenerNUmeroMallas(tipo_mallaH.Text),
                espaciemientoH = Util.ConvertirStringInInteger(espa_mallaH.Text.Replace(",",".")),
                espaciemientoV = Util.ConvertirStringInInteger(espa_mallaV.Text.Replace(",", ".")),
                tipoMallaH = ObtenerTipo(tipo_mallaH.Text),
                tipoMallaV = ObtenerTipo(tipo_mallaH.Text),//tipo_mallaV.Text
                tipoSeleccionInf = (rbt_ini_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                tipoSeleccionSup = (rbt_sup_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                CualMallaDibujar = ObtenerCualMAllaDibujar(tipo_MAllaDibujar.Text),
                Tipo_PataH= EnumeracionBuscador.ObtenerEnumGenerico( TipoPataMAlla.Auto, cbx_pataH.Text),
                Tipo_PataV = EnumeracionBuscador.ObtenerEnumGenerico(TipoPataMAlla.Auto, cbx_pataV.Text),
                IsTipoViga = (bool)Estipoviga.IsChecked
            };

            return datosMallasDTO;
        }


        public static TipoMAllaMuro ObtenerTipo(string tipo)
        {
            switch (tipo)
            {
                case "E.":
                    return TipoMAllaMuro.SM;
                case "E.D.":
                    return TipoMAllaMuro.DM;

                case "E.T.":
                    return TipoMAllaMuro.TM;

                case "E.C.":
                    return TipoMAllaMuro.CM;
                default:
                    return TipoMAllaMuro.SM;
            }
        }

        public static CualMAllaDibujar ObtenerCualMAllaDibujar(string tipo)
        {
            switch (tipo)
            {
                case "Ambos":
                    return CualMAllaDibujar.Ambos;
                case "Horizontal":
                    return CualMAllaDibujar.Horizontal;

                case "Vertical":
                    return CualMAllaDibujar.Vertical;

                default:
                    return CualMAllaDibujar.Ambos;
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


            if (diam_mallaH.Text == "6" || diam_mallaV.Text == "6")
            {
                diam_mallaH.Text = "6";
                diam_mallaV.Text = "6";
                espa_mallaH.Text = "15";
                espa_mallaV.Text = "15";
                //tipo_mallaH.Text = "E.";
            }

            BotonOprimido = bton.Name;
            if (BotonOprimido != "btnCerrar_e" || BotonOprimido != "btnCerrar_var")
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

        private void Rdbton_Editar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_mallaH.IsEnabled = false;
            tipo_mallaV.IsEnabled = false;
        }

        private void Rdbton_dibujar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_mallaH.IsEnabled = true;
            tipo_mallaV.IsEnabled = true;
        }
    }


}