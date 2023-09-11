using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.WPF_pathSymbol.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.WPF_pathSymbol
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_pathSymbol : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;
        private UIApplication _uiApp;

        public ObservableCollection<string> TipoCaso { get; set; }

        private System.Windows.Visibility isvisibleLargo;
        public System.Windows.Visibility IsvisibleLargo
        {
            get { return isvisibleLargo; }
            set
            {
                if (isvisibleLargo != value)
                {
                    isvisibleLargo = value;
                    RaisePropertyChanged("IsvisibleLargo");
                }
            }
        }

        private System.Windows.Visibility isvisibleMouse;
        public System.Windows.Visibility IsvisibleMouse
        {
            get { return isvisibleMouse; }
            set
            {
                if (isvisibleMouse != value)
                {
                    isvisibleMouse = value;
                    RaisePropertyChanged("IsvisibleMouse");
                }
            }
        }
        private UbicacionLosa ubicacion;
        public FormaDibujarPAth FormaDibujar_ { get; private set; }



        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public DireccionEdicionPathRein direccion { get; internal set; }
        public double valorCm { get; internal set; }
        public bool IscambiarLargos { get; private set; }

        public Ui_pathSymbol(UIApplication uiApp,
                            EventHandlerWithStringArg evExternalMethodStringArg,
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

            this.Topmost = true;
            TipoCaso = new ObservableCollection<string>() { "Largo PathSymbol Izquierda-Inferior", "Largo PathSymbol Derecha-Superior", "Equidistante", "Normal Mouse" };

            IsvisibleLargo = System.Windows.Visibility.Hidden;
            IsvisibleMouse = System.Windows.Visibility.Hidden;

            ubicacion = UbicacionLosa.Izquierda;
    
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }

        internal CargarBarraRoomDTO ObtenerParametrosENtrada(string NombrebotonOprimido)
        {
            string tipoBarras = AyudaObtenerParametros.ObtenerTipoBArras(NombrebotonOprimido, ubicacion);

            IscambiarLargos = AyudaObtenerParametros.ObtenerIscambiarLargos(NombrebotonOprimido, this);

            CargarBarraRoomDTO _CargarBarraRoomDTO = new CargarBarraRoomDTO(tipoBarras, ObtenerDireccion());

            //  a) pararametros de largo general de path
            Ui_pathSymbolDTO Ui_pathSymbolDTO_ = null;
            try
            {
                Ui_pathSymbolDTO_ = new Ui_pathSymbolDTO()
                {
                    FormaDibujar_ = FormaDibujar_,
                    Largo_Foot = ObtenerLargoL_foot(),
                    LargoIzq_foot = ObtenerLargoIZq_foot(),
                    LargoDere_foot = ObtenerLargoDere_foot(),
                    IsOk = true

                };
            }
            catch (Exception)
            {
                Ui_pathSymbolDTO_ = new Ui_pathSymbolDTO();
            }

            // b) parametros de  cambio de largo internos de path
            PathSymbol_REbarshape_FxxDTO _PathSymbol_REbarshape_FxxDTO = null;
            if (IscambiarLargos == true)
            {
                _PathSymbol_REbarshape_FxxDTO = new PathSymbol_REbarshape_FxxDTO()
                {
                    IsOK = true,
                    CopiarFamiliasDiferentesPatas=false,
                    DesDereInf_foot = AyudaObtenerParametros.ObtenerDesDereInf_foot(tipoBarras, this),
                    DesDereSup_foot = AyudaObtenerParametros.ObtenerDesDereSup_foot(tipoBarras, this),

                    DesIzqInf_foot = AyudaObtenerParametros.ObtenerDesIzqInf_foot(tipoBarras, this),
                    DesIzqSup_foot = AyudaObtenerParametros.ObtenerDesIzqSup_foot(tipoBarras, this),

                    pataIzq_foot = AyudaObtenerParametros.ObtenerpataIzq_foot(tipoBarras, this),
                    pataDere_foot = AyudaObtenerParametros.ObtenerpataDere_foot(tipoBarras, this),

                };

            }
            else
            {
                _PathSymbol_REbarshape_FxxDTO = new PathSymbol_REbarshape_FxxDTO() { IsOK = false, CopiarFamiliasDiferentesPatas = false };

            }

            //guardar
            _CargarBarraRoomDTO.Ui_pathSymbolDTO_ = Ui_pathSymbolDTO_;
            _CargarBarraRoomDTO.PathSymbol_REbarshape_FxxDTO_ = _PathSymbol_REbarshape_FxxDTO;

            return _CargarBarraRoomDTO;
        }

        private UbicacionLosa ObtenerDireccion()
        {
            return ubicacion;
        }



        private double ObtenerLargoDere_foot()
        {
            if (FormaDibujar_ != FormaDibujarPAth.mouse)
                return 0;

            return Util.CmToFoot(Util.ConvertirStringInDouble(dtLargoDere.Text));
        }

        private double ObtenerLargoIZq_foot()
        {
            if (FormaDibujar_ != FormaDibujarPAth.mouse)
                return 0;

            return Util.CmToFoot(Util.ConvertirStringInDouble(dtLargoIzq.Text));
        }

        private double ObtenerLargoL_foot()
        {
            if (FormaDibujar_ == FormaDibujarPAth.Normal || FormaDibujar_ == FormaDibujarPAth.mouse)
                return 0;

            return Util.CmToFoot(Util.ConvertirStringInDouble(dtLargoP.Text));
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

        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;

            BotonOprimido = bton.Name;
           
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }

        #endregion

        #region Non-External Project Methods

        private void UserAlert()
        {
            //TaskDialog.Show("Non-External Method", "Non-External Method Executed Successfully");
            MessageBox.Show("Non-External Method Executed Successfully", "Non-External Method");

            //Dispatcher.Invoke(() =>
            //{
            //    TaskDialog mainDialog = new TaskDialog("Non-External Method")
            //    {
            //        MainInstruction = "Hello, Revit!",
            //        MainContent = "Non-External Method Executed Successfully",
            //        CommonButtons = TaskDialogCommonButtons.Ok,
            //        FooterText = "<a href=\"http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975 \">"
            //                     + "Click here for the Revit API Developer Center</a>"
            //    };


            //    TaskDialogResult tResult = mainDialog.Show();
            //    Debug.WriteLine(tResult.ToString());
            //});
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





        private void dtTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void Rbt_op_PtoInicial_Checked(object sender, RoutedEventArgs e)
        {
            IsvisibleLargo = System.Windows.Visibility.Visible;
            IsvisibleMouse = System.Windows.Visibility.Hidden;
            FormaDibujar_ = FormaDibujarPAth.Inicial;
        }

        private void Rbt_op_nomral_Checked(object sender, RoutedEventArgs e)
        {
            IsvisibleLargo = System.Windows.Visibility.Hidden;
            IsvisibleMouse = System.Windows.Visibility.Hidden;
            FormaDibujar_ = FormaDibujarPAth.Normal;
        }

        private void Rbt_op_PtoFinal_Checked(object sender, RoutedEventArgs e)
        {
            IsvisibleLargo = System.Windows.Visibility.Visible;
            IsvisibleMouse = System.Windows.Visibility.Hidden;
            FormaDibujar_ = FormaDibujarPAth.Final;
        }

        private void Rbt_op_Ptomouse_Checked(object sender, RoutedEventArgs e)
        {
            IsvisibleLargo = System.Windows.Visibility.Hidden;
            IsvisibleMouse = System.Windows.Visibility.Visible;
            FormaDibujar_ = FormaDibujarPAth.mouse;
        }

        private void Rbt_op_Defaul_f16_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var botton= (Button)sender;

            if(botton.Name=="botonIzq")
                ubicacion = UbicacionLosa.Izquierda;
            else if (botton.Name == "botonDere")
                ubicacion = UbicacionLosa.Derecha;
            else if (botton.Name == "botonInf")
                ubicacion = UbicacionLosa.Inferior;
            else if (botton.Name == "botonSupe")
                ubicacion = UbicacionLosa.Superior;
        }
    }


}