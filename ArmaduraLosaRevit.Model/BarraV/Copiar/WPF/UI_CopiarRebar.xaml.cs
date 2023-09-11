using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace ArmaduraLosaRevit.Model.BarraV.Copiar.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_CopiarRebarElev : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;



        //1)
        public ObservableCollection<EnvoltoriLevel> _listaLevel;
        public ObservableCollection<EnvoltoriLevel> ListaLevel
        {
            get { return _listaLevel; }
            set
            {
                _listaLevel = value;
                RaisePropertyChanged("ListaLevel");
            }
        }

        public  string Linea1 { get; set; }

        internal string ObtenerTexto()
        {
            if (Linea1 == "")
                Linea1 = "ID. ARM";
            if (Linea2 == "")
                Linea2 = "--Nivel mal asignado--";

            return $"{Linea1} \n {Linea2}";
        }

        public string Linea2 { get; set; }

        internal List<double> ObtenerListaLevelZ()
        {
            return _listaLevel.Where(c => c.IsSelecte).Select(c => c.ElevacionProjectadaRedondeada).ToList();
        }



        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;


        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public List<ViewSelected> Listainicial { get; private set; }
        public ObservableCollection<string> ListaView3d { get; private set; }
        public string SelectView3d { get; set; }
        public string ActualView3d { get; set; }
        public bool IsOk { get; internal set; }
        public TipoCaso casoEjecutar { get; internal set; }
        public string NombreLevelRef { get; internal set; }

        public UI_CopiarRebarElev(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg, List<EnvoltoriLevel> listaLevel)
        {
            _uiapp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            Closed += MainWindow_Closed;

            InitializeComponent();
            Linea1 = "ID. ARM";
            Linea2 = "C.P.1°";
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;
            this.ListaLevel = new ObservableCollection<EnvoltoriLevel>(listaLevel);


        }



        private void MainWindow_Closed(object sender, EventArgs e)
        {
            casoEjecutar = TipoCaso.Cerrar;
            Close();
        }
        #region External Project Methods


        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;
            BotonOprimido = bton.Name;


            if ("Cerrar_Level" == BotonOprimido)
            {
                IsOk = false;
                Close();                
            }
            else
            {
                IsOk = true;
                // Raise external event with this UI instance (WPF) as an argument
                if (_mExternalMethodWpfArg != null)
                    _mExternalMethodWpfArg.Raise(this);
            }
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
            var cbx = (System.Windows.Controls.ComboBox)sender;
            var nombre = cbx.Text;
        }


    }


}