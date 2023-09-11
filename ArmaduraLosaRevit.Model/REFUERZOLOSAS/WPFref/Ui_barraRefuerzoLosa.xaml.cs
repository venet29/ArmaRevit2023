using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.WPFref
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_barraRefuerzoLosa : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
      
        private readonly Document _doc;
   
        private UIApplication _uiApp;

        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<float> ListaCantidadBArrasCabezaMuro { get;  set; }
        public ObservableCollection<float> ListaCantidadBArras { get; set; }
        public ObservableCollection<string> ListaTipoPosiBarra { get; set; }
        public ObservableCollection<string> ListaTipoLosa { get; set; }
        public ObservableCollection<float> ListaCantidadBArrasBorde { get; set; }
        public ObservableCollection<string> ListaCantidadBArrasString { get; set; }
       
        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }


        private string tipoLosa_;

        public string TipoLosa_
        {
            get { return tipoLosa_; }
            set
            {
                if (tipoLosa_ != value)
                {
                    tipoLosa_ = value;
                    TipoTitulo_ = "Refuerzo " + (tipoLosa_.ToLower() == "losa" ? "Losa" : "Fundaciones");
                    TipoTituloSub_= "2-Refuer" + (tipoLosa_.ToLower()=="losa"?"Losa":"Fund");
                    Isvisible_ = (tipoLosa_.ToLower() == "losa" ? true : false);
                    RaisePropertyChanged("TipoLosa_");
                }
            }
        }

        private bool isvisible_;
        public bool Isvisible_
        {
            get { return isvisible_; }
            set
            {
                if (isvisible_ != value)
                {
                    isvisible_ = value;
                    RaisePropertyChanged("Isvisible_");
                }
            }
        }


        private string tipoTitulo_;

        public string TipoTitulo_
        {
            get { return tipoTitulo_; }
            set
            {
                if (tipoTitulo_ != value)
                {
                    tipoTitulo_ = value;
                    RaisePropertyChanged("TipoTitulo_");
                }
            }
        }


        private string tipoTituloSub_;

        public string TipoTituloSub_
        {
            get { return tipoTituloSub_; }
            set
            {
                if (tipoTituloSub_ != value)
                {
                    tipoTituloSub_ = value;
                    RaisePropertyChanged("TipoTituloSub_");
                }
            }
        }



        private string espaciamiento;

        public string Espaciamiento
        {
            get { return espaciamiento; }
            set
            {
                if (espaciamiento != value)
                {
                    espaciamiento = value;
                    RaisePropertyChanged("Espaciamiento");
                }
            }
        }
        private TipoBarraTraslapoDereArriba tipoBarra_;

        public TipoBarraTraslapoDereArriba TipoBarra_
        {
            get { return tipoBarra_; }
            set
            {
                if (tipoBarra_ != value)
                {
                    tipoBarra_ = value;
                    RaisePropertyChanged("TipoBarra_");
                }
            }
        }


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_barraRefuerzoLosa(UIApplication uiApp, TipoRefuerzoLOSA _TipoLosa_, EventHandlerWithStringArg evExternalMethodStringArg,
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


            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };

            ListaCantidadBArrasCabezaMuro = new ObservableCollection<float>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            ListaCantidadBArras = new ObservableCollection<float>() { 2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
            ListaCantidadBArrasBorde = new ObservableCollection<float>() { 1,2, 3, 4, 5, 6, 7 };
            ListaCantidadBArrasString = new ObservableCollection<string>() {"DF", "2", "3", "4", "5", "6", "7" };
            ListaTipoPosiBarra = new ObservableCollection<string>() { "Central", "Superior", "Inferior" };
            ListaTipoLosa = new ObservableCollection<string>() { "Losa", "Fundaciones"};

            TipoLosa_ =(_TipoLosa_==TipoRefuerzoLOSA.fundacion? "Fundaciones": _TipoLosa_.ToString());

            TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
            Espaciamiento = "20";
            this.Topmost = true;
 
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

        private void BNonExternal3_Click(object sender, RoutedEventArgs e)
        {
            // the sheet takeoff + delete method won't work here because it's not in a valid Revit api context
            // and we need to do a transaction
            //Methods.SheetRename(this, _doc); //<- WON'T WORK HERE
            UserAlert();
        }


        private void BNonExternal1_Click(object sender, RoutedEventArgs e)
        {
           
            UserAlert();
        }

        private void BNonExternal2_Click(object sender, RoutedEventArgs e)
        { 
            UserAlert();
        }

        internal EditarBarraDTO EditarBarraDTO(TipoCasobarra tipoCasobarra)
        {
            return new EditarBarraDTO()
            {
                cantidad = Util.ConvertirStringInInteger(dtCantidadBarrasViga.Text),
                diametro = Util.ConvertirStringInInteger(dtDiamRefuerzoVIga.Text),
                tipobarraV = ObtenerTipoBarraCambiar(),
                TipoCasobarra = tipoCasobarra,
                IsCambiarDiametroYEspacia = ObtenerIsDiametroYCantidad(),
                ModificadorDireccionEnfierrado = ObtenerDireccionEnfierradoModificador()
            };
        }

        private int ObtenerDireccionEnfierradoModificador()
        {

                switch (BotonOprimido)
                {
                    case "barraInferiorInf":
                    case "barraSuperiorInf":
                    case "barraAmbosInf":
                        return -1;
                    default:
                        return 1;
                }

       
        }

        public TipoPataBarra ObtenerTipoBarraCambiar()
        {
            switch (BotonOprimido)
            {
                case "barraSin":
                    return TipoPataBarra.BarraVSinPatas;
                case "barraInferiorSup":
                case "barraInferiorInf":
                    return TipoPataBarra.BarraVPataInicial;
                case "barraSuperiorSUp":
                case "barraSuperiorInf":
                    return TipoPataBarra.BarraVPataFinal;
                case "barraAmbosSup":
                case "barraAmbosInf":
                    return TipoPataBarra.BarraVPataAmbos;
                default:
                    return TipoPataBarra.BarraVSinPatas;
            }

        }

        public bool ObtenerIsDiametroYCantidad()
        {
            switch (BotonOprimido)
            {
                case "barraSin":
                case "barraInferiorV":
                case "barraAmbosV":
                case "barraSuperiorV":

                    return false;// IsCambiarDiamCantV.IsChecked ?? false;
                case "barraSuperiorH":
                case "barraInferiorH":
                case "barraAmbosH":
                case "barraSinH":
                    return false;//IsCambiarDiamCantH.IsChecked ?? false;
                default:
                    return false;
            }

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

            BotonOprimido = bton.Name;

            if (BotonOprimido == "btnAceptar_refCabMuro" ||
                BotonOprimido == "btnAceptar_refTipoViga" ||
                BotonOprimido == "btnAceptar_refTipoBorde" ||
                BotonOprimido == "btnAceptar_Cuantia" ||
                 BotonOprimido == "barraRefAmbos")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else
            {
                Close();
            }
      
        }

        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image bton = (Image)sender;

            BotonOprimido = bton.Name;
            _mExternalMethodWpfArg.Raise(this);
        }
    }


}