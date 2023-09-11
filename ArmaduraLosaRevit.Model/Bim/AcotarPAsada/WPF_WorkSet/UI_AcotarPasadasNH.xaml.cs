using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.WPF_Pasada
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_AcotarPasadasNH : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private string punto_ = "●";
        private string rayaH_ = "▬";
        private string rayaV_ = "▌";

        private UIApplication _uiApp;
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }

        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }
        public EnumPasadasConGrilla DimesionVertical  { get; set; }
        public EnumPasadasConGrilla DimesionHorizonal { get; set; }


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


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public UI_AcotarPasadasNH(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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

            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };

            CargarIZq_Supe();
            CargarArriba_Dere();
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
            if (BotonOprimido != "btn_Cerrar")
            {
                _mExternalMethodWpfArg.Raise(this);
            }
            else
                Close();
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



        #region vertical


        private void Izq_DereSup_Click(object sender, RoutedEventArgs e)
        {
            CargarIZq_Supe();
        }

        private void CargarIZq_Supe()
        {
            DimesionVertical = EnumPasadasConGrilla.Izquieda_sup;

            Izq_DereSup.Content = punto_;
            Izq_central.Content = rayaV_;
            Izq_izqInf.Content = "";

            Dere_DereSup.Content = "";
            Dere_central.Content = "";
            Dere_izqInf.Content = "";
        }

        private void Dere_DereSup_Click(object sender, RoutedEventArgs e)
        {
            DimesionVertical = EnumPasadasConGrilla.Derecha_sup;

            Izq_DereSup.Content = "";
            Izq_central.Content = "";
            Izq_izqInf.Content = "";

            Dere_DereSup.Content = punto_;
            Dere_central.Content = rayaV_;
            Dere_izqInf.Content = "";
        }

        private void Izq_izqInf_Click_1(object sender, RoutedEventArgs e)
        {
            DimesionVertical = EnumPasadasConGrilla.Izquieda_inf;
            Izq_DereSup.Content = "";
            Izq_central.Content = rayaV_;
            Izq_izqInf.Content = punto_;

            Dere_DereSup.Content = "";
            Dere_central.Content = "";
            Dere_izqInf.Content = "";
        }


        private void Dere_izqInf_Click(object sender, RoutedEventArgs e)
        {
            DimesionVertical = EnumPasadasConGrilla.Derecha_inf;
            Izq_DereSup.Content = "";
            Izq_central.Content = "";
            Izq_izqInf.Content = "";

            Dere_DereSup.Content = "";
            Dere_central.Content = rayaV_;
            Dere_izqInf.Content = punto_;
        }

        #endregion



        #region horizontal


        private void Arriba_DereSup_Click(object sender, RoutedEventArgs e)
        {
            CargarArriba_Dere();
        }

        private void CargarArriba_Dere()
        {
            DimesionHorizonal = EnumPasadasConGrilla.Arriba_dere;
            Arriba_DereSup.Content = punto_;
            Arriba_central.Content = rayaH_;
            Arriba_izqInf.Content = "";

            Bajo_DereSup.Content = "";
            Bajo_central.Content = "";
            Bajo_izqInf.Content = "";
        }

        private void Arriba_izqInf_Click(object sender, RoutedEventArgs e)
        {
            DimesionHorizonal = EnumPasadasConGrilla.Arriba_izq;
            Arriba_DereSup.Content = "";
            Arriba_central.Content = rayaH_;
            Arriba_izqInf.Content = punto_;

            Bajo_DereSup.Content = "";
            Bajo_central.Content = "";
            Bajo_izqInf.Content = "";

        }



        private void Bajo_DereSup_Click(object sender, RoutedEventArgs e)
        {
            DimesionHorizonal = EnumPasadasConGrilla.Bajo_dere;
            Arriba_DereSup.Content = "";
            Arriba_central.Content = "";
            Arriba_izqInf.Content = "";

            Bajo_DereSup.Content = punto_;
            Bajo_central.Content = rayaH_;
            Bajo_izqInf.Content = "";
        }

        private void Bajo_izqInf_Click(object sender, RoutedEventArgs e)
        {
            DimesionHorizonal = EnumPasadasConGrilla.Bajo_izq;
            Arriba_DereSup.Content = "";
            Arriba_central.Content = "";
            Arriba_izqInf.Content = "";

            Bajo_DereSup.Content = "";
            Bajo_central.Content = rayaH_;
            Bajo_izqInf.Content = punto_;
        }


        #endregion
    }


}