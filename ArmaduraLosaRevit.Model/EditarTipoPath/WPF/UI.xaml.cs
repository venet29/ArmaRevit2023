using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.EditarTipoPath.WPF.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ArmaduraLosaRevit.Model.EditarTipoPath.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;
        private UIApplication _uiApp;
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }


        public EstadoCambioTIpoBarras EstadoCambioTIpoBarras_ { get; set; }

        private string _id;

        public string _Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("_Id");
                }
            }
        }
        private string orientacion;

        public string Orientacion
        {
            get { return orientacion; }
            set
            {
                if (orientacion != value)
                {
                    orientacion = value;
                    RaisePropertyChanged("Orientacion");
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

        private string diametro;
        public string Diametro
        {
            get { return diametro; }
            set
            {
                if (diametro != value)
                {
                    diametro = value;
                    RaisePropertyChanged("Diametro");
                }
            }
        }




        private string _tipoDireccionBarra;
        public string _TipoDireccionBarra
        {
            get { return _tipoDireccionBarra; }
            set
            {
                if (_tipoDireccionBarra != value)
                {
                    _tipoDireccionBarra = value;
                    RaisePropertyChanged("_TipoDireccionBarra");
                }
            }
        }



        private TipoEditarPAth tipoBarra_;
        public TipoEditarPAth TipoBarra_
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

        private System.Windows.Visibility isvisibletipoDesp;
        public System.Windows.Visibility IsvisibletipoDesp
        {
            get { return isvisibletipoDesp; }
            set
            {
                if (isvisibletipoDesp != value)
                {
                    isvisibletipoDesp = value;
                    RaisePropertyChanged("IsvisibletipoDesp");
                }
            }
        }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public DireccionEdicionPathRein direccion { get; internal set; }
        public double valorCm { get; internal set; }

        public Ui(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto
            , TabEditarPath tabEditarPath)
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
            this._seleccionarPathReinfomentConPto = seleccionarPathReinfomentConPto;

            if (_seleccionarPathReinfomentConPto != null)
            {
                TipoBarra_ = EnumeracionBuscador.ObtenerEnumGenerico(TipoEditarPAth.NONE, _seleccionarPathReinfomentConPto._tipobarra);
                Espaciamiento = _seleccionarPathReinfomentConPto._espaciamiento;
                Diametro = _seleccionarPathReinfomentConPto._diametro;
                Orientacion = _seleccionarPathReinfomentConPto._direccion;
                _Id = _seleccionarPathReinfomentConPto.PathReinforcement.Id.IntegerValue.ToString();
            }
            else
            {
                TipoBarra_ = TipoEditarPAth.f4;
                Espaciamiento = "20";
                Diametro = "8";
                Orientacion = "Derecha";
            }
            if (tabEditarPath == TabEditarPath.Datos)
            {
                tbDatos.IsSelected = true;
            }
            else if (tabEditarPath == TabEditarPath.Forma)
            {
                EditPAth.IsSelected = true;
            }

            this.Topmost = true;

            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            ListaEstribo = new ObservableCollection<string>() { "Derecha", "Izquierda", "Superior", "Inferior" };
            IsvisibletipoDesp = System.Windows.Visibility.Hidden;
            EstadoCambioTIpoBarras_ = new EstadoCambioTIpoBarras();
        }


        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }
        private void TipoDesp_DropDownClosed(object sender, EventArgs e)
        {
            if (tipoDesp.Text == "Definir")
                IsvisibletipoDesp = System.Windows.Visibility.Visible;
            else
                IsvisibletipoDesp = System.Windows.Visibility.Hidden;
        }

        internal void ActualizarDatos()
        {
            EstadoCambioTIpoBarras_ = new EstadoCambioTIpoBarras()
            {
                TipoBarraActual = dtTipo.Text,
                diametroActual = dtDia.Text,
                EspacimeientoActual = dtEsp.Text,
                OrientacionActual = dtorient.Text
            };
        }

        internal bool IsSOloCAmbioDiamtroOEspaciamieto() => (EstadoCambioTIpoBarras_.IsCambioOrientacion == false && EstadoCambioTIpoBarras_.IsCambioTipoBarra == false ? true : false);
        internal bool IsNoCambioNAda() => (EstadoCambioTIpoBarras_.IsCambioOrientacion == false && EstadoCambioTIpoBarras_.IsCambioTipoBarra == false &&
                                           EstadoCambioTIpoBarras_.IsCambioDiametro == false && EstadoCambioTIpoBarras_.IsCambioEspacimeiento == false
            ? true : false);

        internal List<ObjectSnapTypes> ObtenerListaSnap()
        {
            List<ObjectSnapTypes> list = new List<ObjectSnapTypes>();

            if ((bool)PathToPto_ptofnal.IsChecked == true) list.Add(ObjectSnapTypes.Endpoints);
            if ((bool)PathToPto_Nearest.IsChecked == true) list.Add(ObjectSnapTypes.Nearest);
            if ((bool)PathToPto_Medio.IsChecked == true) list.Add(ObjectSnapTypes.Midpoints);
            if ((bool)PathToPto_Intersections.IsChecked == true) list.Add(ObjectSnapTypes.Intersections);

            if (list.Count == 0)
            {
                list.Add(ObjectSnapTypes.Points);
                list.Add(ObjectSnapTypes.Nearest);
            }

            return list;
        }

        internal TipoCasoAlternativo ObtenerTipoCasoAlternativo()
        {
            double dist = Util.ConvertirStringInDouble(distanPtoPto.Text);
            double distDefinir = 0;// Util.ConvertirStringInDouble(distaDefinir.Text);



            TipoCasoAlternativo_enum TipoCasoAlternativo_enum_ = TipoCasoAlternativo_enum.Proporcional;

            if (tipoDesp.Text == "Mantener Largo")
                TipoCasoAlternativo_enum_ = TipoCasoAlternativo_enum.MantenerLargo;
            else if (tipoDesp.Text == "Definir")
            {
                TipoCasoAlternativo_enum_ = TipoCasoAlternativo_enum.Definir;
                distDefinir = Util.ConvertirStringInDouble(distaDefinir.Text);
            }



            return new TipoCasoAlternativo()
            {
                distancia_foot = Util.CmToFoot(dist),
                distanciaDefinir_foot = Util.CmToFoot(distDefinir),
                TipoCasoAlternativo_ = TipoCasoAlternativo_enum_,
                IsAjustar= (bool)IsAjustar.IsChecked
            };
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

            if (BotonOprimido == "btnActualizar")
            {
                EstadoCambioTIpoBarras_.OrientacionNuevo = dtTipo.Text;
                EstadoCambioTIpoBarras_.ComprobarCambioTipoBarra();

                EstadoCambioTIpoBarras_.diametroNuevo = dtDia.Text;
                EstadoCambioTIpoBarras_.ComprobarCambiodiametro();

                EstadoCambioTIpoBarras_.EspaciamientoNuevo = dtEsp.Text;
                EstadoCambioTIpoBarras_.ComprobarCambioEspacimeiento();

                EstadoCambioTIpoBarras_.OrientacionNuevo = dtorient.Text;
                EstadoCambioTIpoBarras_.ComprobarCambioOrientacion();
            }

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

        private void BNonExternal3_Click(object sender, RoutedEventArgs e)
        {
            // the sheet takeoff + delete method won't work here because it's not in a valid Revit api context
            // and we need to do a transaction
            //Methods.SheetRename(this, _doc); //<- WON'T WORK HERE
            UserAlert();
        }


        private void BNonExternal1_Click(object sender, RoutedEventArgs e)
        {
            Methods.DocumentInfo(this, _uiApp);
            UserAlert();
        }

        private void BNonExternal2_Click(object sender, RoutedEventArgs e)
        {
            Methods.WallInfo(this, _doc);
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




    }


}