using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Fund.Editar.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_FundEditar : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        public string DireccionImagen { get; set; }
        
        private readonly Document _doc;
   
        private UIApplication _uiApp;
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }
        
        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }


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

        //******
        private string baseImagen; 
        private string imagen_barra_sin;
        public string Imagen_barra_sin
        {
            get { return imagen_barra_sin; }
            set
            {
                if (imagen_barra_sin != value)
                {
                    imagen_barra_sin = value;
                    RaisePropertyChanged("Imagen_barra_sin");
                }
            }
        }

        private string imagen_barra_izq;
        public string Imagen_barra_izq
        {
            get { return imagen_barra_izq; }
            set
            {
                if (imagen_barra_izq != value)
                {
                    imagen_barra_izq = value;
                    RaisePropertyChanged("Imagen_barra_izq");
                }
            }
        }

        private string imagen_barra_dere;
        public string Imagen_barra_dere
        {
            get { return imagen_barra_dere; }
            set
            {
                if (imagen_barra_dere != value)
                {
                    imagen_barra_dere = value;
                    RaisePropertyChanged("Imagen_barra_dere");
                }
            }
        }

        private string imagen_barra_ambos;
        public string Imagen_barra_ambos
        {
            get { return imagen_barra_ambos; }
            set
            {
                if (imagen_barra_ambos != value)
                {
                    imagen_barra_ambos = value;
                    RaisePropertyChanged("Imagen_barra_ambos");
                }
            }
        }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get;  set; }
        public bool IsCambioEspacimiento { get;  set; }
        public double EspacimientoOriginal { get; set; }

        public Ui_FundEditar(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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

            if (_seleccionarPathReinfomentConPto!= null)
            {
                TipoBarra_ = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarraTraslapoDereArriba.NONE, _seleccionarPathReinfomentConPto._tipobarra); 
                Espaciamiento = _seleccionarPathReinfomentConPto._espaciamiento;
                Diametro = _seleccionarPathReinfomentConPto._diametro;
                Orientacion = _seleccionarPathReinfomentConPto._direccion;
                _Id = _seleccionarPathReinfomentConPto.PathReinforcement.Id.IntegerValue.ToString();

            }
            else
            {
                TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
                Espaciamiento = "20";
                Diametro = "8";
                Orientacion = "Derecha";
            }

            EspacimientoOriginal = Util.ConvertirStringInDouble(  Espaciamiento);

            if (tabEditarPath == TabEditarPath.Datos)
            { tbDatos.IsSelected = true;
              //  EditPAth.Visibility = System.Windows.Visibility.Hidden; 
            }
            else if (tabEditarPath == TabEditarPath.Forma)
            {
                EditPAth.IsSelected = true;
              //tbDatos.Visibility = System.Windows.Visibility.Hidden;
            }

            this.Topmost = true;

            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            
            ListaEstribo = new ObservableCollection<string>(){ "Derecha", "Izquierda", "Superior", "Inferior"};
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;

            
            baseImagen = @"/ArmaduraLosaRevit.Model;component/Resources/BarraHorizontal/";
            Imagen_barra_sin = baseImagen + "sin.png";
            Imagen_barra_izq = baseImagen + "inferior.png";
            Imagen_barra_dere = baseImagen + "superior.png";
            Imagen_barra_ambos = baseImagen + "ambos.png";

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

        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image boton = (Image)sender;

            BotonOprimido = boton.Name;
            if ((bool)rb_Arriba.IsChecked)
                DireccionImagen = "Arriba";
            else
                DireccionImagen = "Bajo";

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

        private void Arriba_Checked(object sender, RoutedEventArgs e)
        {
            baseImagen = @"/ArmaduraLosaRevit.Model;component/Resources/BarraHorizontal/";
            Imagen_barra_sin = baseImagen + "sin.png";
            Imagen_barra_izq = baseImagen + "inferior.png";
            Imagen_barra_dere = baseImagen + "superior.png";
            Imagen_barra_ambos = baseImagen + "ambos.png";
        }

        private void Bajo_Checked(object sender, RoutedEventArgs e)
        {
            baseImagen = @"/ArmaduraLosaRevit.Model;component/Resources/BarraHorizontal/";
            Imagen_barra_sin = baseImagen + "sin.png";
            Imagen_barra_izq = baseImagen + "inferiorInf.png";
            Imagen_barra_dere = baseImagen + "superiorInf.png";
            Imagen_barra_ambos = baseImagen + "ambosInf.png";
        }
    }


}