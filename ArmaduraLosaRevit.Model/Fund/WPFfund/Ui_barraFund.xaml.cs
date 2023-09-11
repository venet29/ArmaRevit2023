using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Windows;
using System.Windows.Controls;

using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Fund.WPFfund
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_barraFund : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;

        public ObservableCollection<float> ListaDiam { get; set; }

        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }


        private string imagen_barra;
        private string baseImagen;

        public string Imagen_barra
        {
            get { return imagen_barra; }
            set
            {
                if (imagen_barra != value)
                {
                    imagen_barra = value;
                    RaisePropertyChanged("Imagen_barra");
                }
            }
        }
        //***
        private string imagen_barra_rectH;
        public string Imagen_barra_rectH
        {
            get { return imagen_barra_rectH; }
            set
            {
                if (imagen_barra_rectH != value)
                {
                    imagen_barra_rectH = value;
                    RaisePropertyChanged("Imagen_barra_rectH");
                }
            }
        }


        private string imagen_barra_rectV;
        public string Imagen_barra_rectV
        {
            get { return imagen_barra_rectV; }
            set
            {
                if (imagen_barra_rectV != value)
                {
                    imagen_barra_rectV = value;
                    RaisePropertyChanged("Imagen_barra_rectV");
                }
            }
        }

        //***
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
        //*****************************************************
        private System.Windows.Visibility _visibilidaBorde;
        public System.Windows.Visibility VisibilidaBorde
        {
            get { return _visibilidaBorde; }
            set
            {
                if (_visibilidaBorde != value)
                {
                    _visibilidaBorde = value;
                    RaisePropertyChanged("VisibilidaBorde");
                }
            }
        }

        private System.Windows.Visibility _visibilidalargoPAtaIzq;
        public System.Windows.Visibility VisibilidalargoPAtaIzq
        {
            get { return _visibilidalargoPAtaIzq; }
            set
            {
                if (_visibilidalargoPAtaIzq != value)
                {
                    _visibilidalargoPAtaIzq = value;
                    RaisePropertyChanged("VisibilidalargoPAtaIzq");
                }
            }
        }


        private System.Windows.Visibility _visibilidalargoPAtaDere;
        public System.Windows.Visibility VisibilidalargoPAtaDere
        {
            get { return _visibilidalargoPAtaDere; }
            set
            {
                if (_visibilidalargoPAtaDere != value)
                {
                    _visibilidalargoPAtaDere = value;
                    RaisePropertyChanged("VisibilidalargoPAtaDere");
                }
            }
        }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;



        //private string _rutaImage;

        //public string RutaImage
        //{
        //    get { return _rutaImage; }
        //    set
        //    {
        //        if (_rutaImage != value)
        //        {
        //            _rutaImage = value;
        //            RaisePropertyChanged("RutaImage");
        //        }
        //    }
        //}


        public Ui_barraFund(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg, bool ISCargado_ResetearLineaBArrasMagenta)
        {
            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;


            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            //  RutaImage = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInfv2.png";

            LoadValoresPArametros();

            baseImagen = @"/ArmaduraLosaRevit.Model;component/Resources/BarraHorizontal/";
            Imagen_barra = baseImagen + "ambosAzul.png";
            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };

            TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
            Espaciamiento = "20";
            VisibilidaBorde = System.Windows.Visibility.Hidden;
            this.Topmost = true;
            this.Title = "Barra Fundaciones"+ "(" +ISCargado_ResetearLineaBArrasMagenta+")";
            Closed += MainWindow_Closed;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
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

            if (!ValidadValorePArametros()) return;

            if (!ValidarDatos()) return;

            Button bton = (Button)sender;

            BotonOprimido = bton.Name;

            if (BotonOprimido == "btnCrearPath_Reactangular" || BotonOprimido == "btnCreaRebar_Reactangular")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else if (BotonOprimido == "btnAceptar_Manual" || BotonOprimido == "btnEditar_Manual" || BotonOprimido == "btnBorrar_Manual" )
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }

            else
            {
                Close();
            }

        }
        private  bool ValidarDatos()
        {
            if (!Util.IsNumeric(dtDiaLong.Text))
            {
                Util.ErrorMsg("Diamtro longitudinal no es dato nomerico");
                return false;
            }

            if (!Util.IsNumeric(espalong.Text))
            {
                Util.ErrorMsg("Espaciamiento longitudinal no es dato nomerico");
                return false;
            }

            double largolong = Util.ConvertirStringInDouble(espalong.Text);
            if (!(largolong >= 7.5 && largolong < 30))
            {
                Util.ErrorMsg("Espaciamiento longitudinal debe estar entre 7.5 y 30cm");
                return false;
            }
            return true;
        }

        private void LoadValoresPArametros()
        {

            paraA_cm.Text = Util.FootToCm(FactoresLargoLeader.FactorDesplazaminetoPotFree_foot).ToString();

            if (!FactoresLargoLeader.IsDefinirLargoCOdo)
                paraB_cm.Text = "mouse";
            else
                paraB_cm.Text = Util.FootToCm(FactoresLargoLeader.FactorLargoCOdo_foot).ToString();

            paraC_cm.Text = (Util.FootToCm(FactoresLargoLeader.FactorDesplazaminetoTag_foot) - 60).ToString();

            LargoPAtaIzq.Text = FactoresLargoLeader.LargoPaTaIzq_cm.ToString();
            LargoPAtaDere.Text = FactoresLargoLeader.LargoPaTaDere_cm.ToString();

            Imagen_barra_rectH = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInf.png";
            Imagen_barra_rectV = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInfV.png";

        }
        private bool  ValidadValorePArametros()
        {
            paraA_cm.Text = Util.FootToCm(FactoresLargoLeader.FactorDesplazaminetoPotFree_foot).ToString();

            if (Util.IsNumeric(paraA_cm.Text))
                FactoresLargoLeader.FactorDesplazaminetoPotFree_foot = Util.CmToFoot(Util.ConvertirStringInDouble(paraA_cm.Text));
            else
            {
                Util.ErrorMsg("Valor del parametro 'A' no es numerico. Se utiliza vslor por defecto 15cm");
                FactoresLargoLeader.FactorDesplazaminetoPotFree_foot = Util.CmToFoot(15);
                return false;
            }


            if (Util.IsNumeric(paraB_cm.Text))
            {
                FactoresLargoLeader.FactorLargoCOdo_foot = Util.CmToFoot(Util.ConvertirStringInDouble(paraB_cm.Text));
                FactoresLargoLeader.IsDefinirLargoCOdo = true;
            }
            else
            {
                FactoresLargoLeader.FactorLargoCOdo_foot = -1;
                FactoresLargoLeader.IsDefinirLargoCOdo = false;
            }

            if (Util.IsNumeric(paraC_cm.Text))
                FactoresLargoLeader.FactorDesplazaminetoTag_foot = Util.CmToFoot(Util.ConvertirStringInDouble(paraC_cm.Text));
            else
            {
                Util.ErrorMsg("Valor del parametro 'A' no es numerico. Se utiliza vslor por defecto 15cm");
                FactoresLargoLeader.FactorDesplazaminetoTag_foot = Util.CmToFoot(20);
                return false;
            }

            int diametro = Util.ConvertirStringInInteger(dtDiaLong.Text);
            if (Util.IsNumeric(LargoPAtaIzq.Text))
                FactoresLargoLeader.LargoPaTaIzq_cm = Util.ConvertirStringInDouble(LargoPAtaIzq.Text);
            else
                FactoresLargoLeader.LargoPaTaIzq_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(diametro);

            if (Util.IsNumeric(LargoPAtaDere.Text))
                FactoresLargoLeader.LargoPaTaDere_cm = Util.ConvertirStringInDouble(LargoPAtaDere.Text);
            else
                FactoresLargoLeader.LargoPaTaDere_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(diametro);

            if (checkBox_Elemento.Text == "Fundacion" && (FactoresLargoLeader.LargoPaTaDere_cm < 25 || FactoresLargoLeader.LargoPaTaIzq_cm < 25))
            {
                Util.ErrorMsg("Las patas de fundaciones no pueden ser menores a 25cm por normativa. Se cambia a 25cm.");
                return false;
            }
            FactoresLargoLeader.TipoFundacion = checkBox_Elemento.Text;
            return true;
        }

        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Check_supAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)check_supAuto.IsChecked)
                Imagen_barra = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funSupv2.png";
            else if ((bool)check_infAuto.IsChecked)
                Imagen_barra = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInfv2.png";
        }

        private void cbx_tiposeleccion_DropDownClosed(object sender, EventArgs e)
        {
            cambiarImagen();
        }

        private void cambiarImagen()
        {
            string nombreActual = "ambos.png";

            if (cbx_tiposeleccion.Text == "Izquierda" && check_sup.IsChecked == true)
                nombreActual = "inferiorInf.png";
            if (cbx_tiposeleccion.Text == "Derecha" && check_sup.IsChecked == true)
                nombreActual = "superiorInf.png";

            if (cbx_tiposeleccion.Text == "Ambos" && check_sup.IsChecked == true)
                nombreActual = "ambosInf.png";
            if (cbx_tiposeleccion.Text == "Auto" && check_sup.IsChecked == true)
                nombreActual = "ambosInfAzul.png";

            if (cbx_tiposeleccion.Text == "Sin" && check_sup.IsChecked == true)
                nombreActual = "sin.png";


            if (cbx_tiposeleccion.Text == "Auto" && check_inf.IsChecked == true)
                nombreActual = "ambosAzul.png";
            if (cbx_tiposeleccion.Text == "Izquierda" && check_inf.IsChecked == true)
                nombreActual = "inferior.png";
            if (cbx_tiposeleccion.Text == "Derecha" && check_inf.IsChecked == true)
                nombreActual = "superior.png";
            if (cbx_tiposeleccion.Text == "Ambos" && check_inf.IsChecked == true)
                nombreActual = "ambos.png";
            if (cbx_tiposeleccion.Text == "Sin" && check_inf.IsChecked == true)
                nombreActual = "sin.png";


            if (cbx_tiposeleccion.Text == "Auto")
                VisibilidaBorde = System.Windows.Visibility.Hidden;
            else
                VisibilidaBorde = System.Windows.Visibility.Visible;

            if (cbx_tiposeleccion.Text == "Auto")
            {
                VisibilidalargoPAtaIzq = System.Windows.Visibility.Visible;
                VisibilidalargoPAtaDere = System.Windows.Visibility.Visible;
            }
            else if (cbx_tiposeleccion.Text == "Izquierda")
            {
                VisibilidalargoPAtaIzq = System.Windows.Visibility.Visible;
                VisibilidalargoPAtaDere = System.Windows.Visibility.Hidden;
            }
            else if (cbx_tiposeleccion.Text == "Derecha")
            {
                VisibilidalargoPAtaIzq = System.Windows.Visibility.Hidden;
                VisibilidalargoPAtaDere = System.Windows.Visibility.Visible;
            }
            else if (cbx_tiposeleccion.Text == "Sin")
            {
                VisibilidalargoPAtaIzq = System.Windows.Visibility.Hidden;
                VisibilidalargoPAtaDere = System.Windows.Visibility.Hidden;
            }
            else if (cbx_tiposeleccion.Text == "Ambos")
            {
                VisibilidalargoPAtaIzq = System.Windows.Visibility.Visible;
                VisibilidalargoPAtaDere = System.Windows.Visibility.Visible;
            }


            Imagen_barra = baseImagen + nombreActual;
        }

        private void check_sup_Click(object sender, RoutedEventArgs e)
        {
            cambiarImagen();
        }

        private void check_inf_Click(object sender, RoutedEventArgs e)
        {
            cambiarImagen();
        }



        private void ComboBoxItem_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
            {
                ComboBoxItem _sele = (ComboBoxItem)sender;
                if (_sele == null) return;

                VerVideoAyudas.Ejecutar(_sele.Name);

            }
        }

        private void dtDiaLong_DropDownClosed(object sender, EventArgs e)
        {
            int diam = Util.ConvertirStringInInteger(dtDiaLong.Text);

            int largopataCm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(diam);
            LargoPAtaIzq.Text = largopataCm.ToString();
            LargoPAtaDere.Text = largopataCm.ToString();

        }

        private void check_supAuto_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)check_supAuto.IsChecked)
            {
                Imagen_barra_rectH = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funSup.png";
                Imagen_barra_rectV = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funSupV.png";
            }
            else if ((bool)check_infAuto.IsChecked)
            {
                Imagen_barra_rectH = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInf.png";
                Imagen_barra_rectV = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInfV.png";
            }
        }

        //private void check_supAuto_Click(object sender, RoutedEventArgs e)
        //{
        //    if ((bool)check_supAuto.IsChecked)
        //        Imagen_barra_rect = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funSupv2.png";
        //    else if ((bool)check_infAuto.IsChecked)
        //        Imagen_barra_rect = @"/ArmaduraLosaRevit.Model;component/Resources/fundaciones/funInfv2.png";
        //}
    }


}
