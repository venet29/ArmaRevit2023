using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.modeloNH;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.model;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.ServiciosNH;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component;
using ArmaduraLosaRevit.Model.ViewFilter.Model;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace ArmaduraLosaRevit.Model.ViewFilter.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_Filtro3D : Window, INotifyPropertyChanged
    {

        public string ImageOprimido { get; set; }
        private readonly Document _doc;
        private View _view;
        private UIApplication _uiapp;

        public string BotonOprimido { get; set; }
        public List<PorTiposNh> ListaTipos { get; private set; }
        public List<TiposDiametrosNh> ListaDiamtr { get; private set; }



        //parametro 1
        public string _sudokuSize;
        public string SudokuSize
        {
            get { return _sudokuSize; }
            set
            {
                _sudokuSize = value;
                RaisePropertyChanged("SudokuSize");
            }
        }
        //parametro 2
        public PorViewDTO_ listaPOrViewDTO;

     

        public PorViewDTO_ ListaPOrViewDTO
        {
            get { return listaPOrViewDTO; }
            set
            {
                listaPOrViewDTO = value;
                RaisePropertyChanged("ListaPOrViewDTO");
            }
        }
        //************

        public bool isSectionBoxActive { get; set; }
        public bool IsView3D { get; private set; }

        public bool IsSectionBoxActive
        {
            get { return isSectionBoxActive; }
            set
            {
                isSectionBoxActive = value;
                RaisePropertyChanged("IsSectionBoxActive");
            }
        }

        public string largoReferencia { get; set; }
        public string LargoReferencia
        {
            get { return largoReferencia; }
            set
            {
                largoReferencia = value;
                RaisePropertyChanged("LargoReferencia");
            }
        }


        
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_Filtro3D(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            
            _uiapp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            _view = _uiDoc.Document.ActiveView;

            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

          
            SudokuSize = "hola";
            Load();

        }

        internal VisibilizacionFilterDTO ObtenerSettingVisibilizacion()
        {
            return new VisibilizacionFilterDTO() {
                IsRebar = (bool)activarRebar.IsChecked,
                IsPAth   = (bool)activarPath.IsChecked,
                SectionBox = (bool)activarSectBox.IsChecked,
                CropRegion = (bool)activarCropRegion.IsChecked,

            };
        }

        private void Load()
        {

            if (_view is View3D)
            {
                IsView3D = true;
                IsSectionBoxActive = ((View3D)_view).IsSectionBoxActive;
            }
            else
            {
                IsSectionBoxActive = false;
                IsView3D = false;
            }
     
            ListaPOrViewDTO = new PorViewDTO_();
            ListaPOrViewDTO.ListaElev= ServiciosSeleccion.ObtenerViewElev(_uiapp).OrderBy(c=> c.Nombre).ToList();
            ListaPOrViewDTO.ListaLosa = ServiciosSeleccion.ObtenerViewLosa(_uiapp).OrderBy(c => c.Nombre).ToList();

            // generar rebar y dar color
           // ManejadorCargarFAmilias manejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
           // manejadorCargarFAmilias.DuplicarFamilasReBarBarv2();

        }

        internal ParametrosFiltro ObtenerOpcionRevision()
        {
            ParametrosFiltro result = FactoryParametrosFiltro.NONE;
            if ((bool)rb_sinTipo.IsChecked) result = FactoryParametrosFiltro.FiltroBarraTipo; //FactoryParametrosFiltro.FiltroSinView, 
            if ((bool)rb_sinNombreVista.IsChecked) result = FactoryParametrosFiltro.FiltroSinView; //FactoryParametrosFiltro.FiltroSinView, 
            if ((bool)rb_largoMayor12.IsChecked) result = FactoryParametrosFiltro.LargoMAyor12Mt(LargoRef.Text); //FactoryParametrosFiltro.FiltroSinView, 

            return result;
        }

        public void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Diametros)
            {
                Diametros _Diametros = (Diametros)sender;
                BotonOprimido = _Diametros.BotonOprimido;
                ListaDiamtr= _Diametros.ListaTiposDiametros.ToList();


            }
            else if (sender is PorTipo)
            {
                PorTipo _PorTipo = (PorTipo)sender;
                BotonOprimido = _PorTipo.BotonOprimido;
                ListaTipos = _PorTipo.ListaTipoTodos.ToList();


            }
            else if (sender is PorView)
            {
                PorView _PorTipo = (PorView)sender;
                BotonOprimido = _PorTipo.BotonOprimido;
                ListaTipos = _PorTipo.ListaTipoTodos.ToList();


            }
            else if (sender is Button)
            {
                Button boton = (Button)sender;
                if (boton == null)
                {
                    Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                    return;
                 
                }
                BotonOprimido = boton.Name;
            }
                 

            if (BotonOprimido == "btn_OpcionRevisiones" || BotonOprimido == "btn_cargarDatosInternos" ||
                BotonOprimido == "btn_ActivarBarras" || BotonOprimido == "btn_SeleccionarBarra" || 
                BotonOprimido == "btn_BorrarTodosFiltre" || BotonOprimido == "btnBorrarFiltreRevision" || BotonOprimido == "btn_BorrarFiltreDiametros" || BotonOprimido == "btn_BorrarTipo_" || BotonOprimido=="btn_BorrarTipoView_" ||
                BotonOprimido == "btn_Diamtros" || BotonOprimido== "btn_Tipo_" || BotonOprimido == "btn_TipoView_")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else if (BotonOprimido != "btnCerrar_e" || BotonOprimido != "btnCerrar_eH")
            {
                Close();
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

        #region Non-External Project Methods

        private void UserAlert()
        {
            //TaskDialog.Show("Non-External Method", "Non-External Method Executed Successfully");
            MessageBox.Show("Non-External Method Executed Successfully", "Non-External Method");

            Dispatcher.Invoke(() =>
            {
                TaskDialog mainDialog = new TaskDialog("Non-External Method")
                {
                    MainInstruction = "Hello, Revit!",
                    MainContent = "Non-External Method Executed Successfully",
                    CommonButtons = TaskDialogCommonButtons.Ok,
                    FooterText = "<a href=\"http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975 \">"
                                 + "Click here for the Revit API Developer Center</a>"
                };


                TaskDialogResult tResult = mainDialog.Show();
                Debug.WriteLine(tResult.ToString());
            });
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

        private void Diametros_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }


}