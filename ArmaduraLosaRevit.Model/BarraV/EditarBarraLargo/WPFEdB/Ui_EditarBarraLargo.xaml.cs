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

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_EditarBarraLargo : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;




        private double _largoFijo;
        public double LargoFijo
        {
            get { return _largoFijo; }
            set
            {
                if (_largoFijo != value)
                {
                    _largoFijo = value;
                    RaisePropertyChanged("LargoFijo");
                }
            }
        }


        private double _deltaLargoMouse;
        public double DeltaLargoMouse
        {
            get { return _deltaLargoMouse; }
            set
            {
                if (_deltaLargoMouse != value)
                {
                    _deltaLargoMouse = value;
                    RaisePropertyChanged("DeltaLargoMouse");
                }
            }
        }
        public bool IsUseMouse { get; set; }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_EditarBarraLargo(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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

            IsUseMouse = false;
            LargoFijo = 0.0f;
            DeltaLargoMouse = 0.0f;
            this.Topmost = true;
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
            IsUseMouse = false;
            if (bton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;
            }

            BotonOprimido = bton.Name;

            if (BotonOprimido == "btnLargoMouse" || BotonOprimido == "btnLargoFijo")
            {
                if (BotonOprimido == "btnLargoMouse") IsUseMouse = true;
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else if (BotonOprimido == "btnCerrar")
            {
                Close();
            }

        }

        public EditarBarraLargoDTO Obtener()
        {

            EditarBarraLargoDTO _EditarBarraLargoDTO = new EditarBarraLargoDTO()
            {
                largoExtender_cm = _largoFijo,
                IsUsarMouse = IsUseMouse,
                DeltaUsarMouse_cm = _deltaLargoMouse

            };
            return _EditarBarraLargoDTO;
        }

        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }


}