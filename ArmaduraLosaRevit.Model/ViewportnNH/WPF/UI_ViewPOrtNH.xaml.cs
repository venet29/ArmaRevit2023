using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using ArmaduraLosaRevit.Model.ViewportnNH.Servicios;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ViewportnNH.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_ViewPOrtNH : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;
        private readonly List<ViewSheetNH> listaSheet;
        public ObservableCollection<ViewDTO> _listaEstructura;
        public ObservableCollection<ViewDTO> ListaEstructura
        {
            get { return _listaEstructura; }
            set
            {
                _listaEstructura = value;
                RaisePropertyChanged("ListaEstructura");
            }
        }


        public ObservableCollection<ViewDTO> _listalosa;
        public ObservableCollection<ViewDTO> ListaLosa
        {
            get { return _listalosa; }
            set
            {
                _listalosa = value;
                RaisePropertyChanged("ListaLosa");
            }
        }

        public ObservableCollection<ViewDTO> _listaElev;
        public ObservableCollection<ViewDTO> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaElev");
            }
        }



        private string paraElevacion;
        public string ParaElevacion
        {
            get { return paraElevacion; }
            set
            {
                if (paraElevacion != value)
                {
                    paraElevacion = value;
                    RaisePropertyChanged("ParaElevacion");
                }
            }
        }
        //*

        private string paraLosa;
        public string ParaLosa
        {
            get { return paraLosa; }
            set
            {
                if (paraLosa != value)
                {
                    paraLosa = value;
                    RaisePropertyChanged("ParaLosa");
                }
            }
        }
        //****
        private string paraEstructura;
        private List<string> ListaSheetNombreEstruc;

        public string ParaEstructura
        {
            get { return paraEstructura; }
            set
            {
                if (paraEstructura != value)
                {
                    paraEstructura = value;
                    RaisePropertyChanged("ParaEstructura");
                }
            }
        }


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public UI_ViewPOrtNH(UIApplication uiApp,
            EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiapp = uiApp;
            this.listaSheet = new List<ViewSheetNH>();
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;
            ParaEstructura = "100_PLANTAS DE ESTRUCTURAS Y FUNDACIONES";
            ParaLosa = "200_PLANTAS ARMADURAS DE LOSAS";
            ParaElevacion = "300_ARMADURA DE ELEVACIONES";

            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            LoadData();
        }

        public void LoadData()
        {
            if (!ObtenerSheetEnArchivo()) return;
            //************* sheet
            var ListacantidadParametros = listaSheet.GroupBy(c => c.TipoEstructura).Select(r => r.Key).ToList();

            var viewStadoAvances = listaSheet.FirstOrDefault();

            //EstadosViewDTO _newEstadosViewDTO = new EstadosViewDTO(viewStadoAvances._view);
            //_newEstadosViewDTO.ObtenerDatos();

            var ListaParaParaEstructura = listaSheet.Where(v => !v._view.IsTemplate && v.TipoEstructura == ParaEstructura).ToList();
            var ListaParaParaLosa = listaSheet.Where(v => !v._view.IsTemplate && v.TipoEstructura == ParaLosa).ToList();
            var ListaParaElevacion = listaSheet.Where(v => !v._view.IsTemplate && v.TipoEstructura == ParaElevacion).ToList();

            
            //************** elevaciones
            var aux_ListaEstructura = SeleccionarView.ObtenerView_losa_elev_fund_estructura(_uiapp.ActiveUIDocument);

            // estructuras
            var ListaSheetNumeroEstruc = ObtenerLIstaBAse("EST-1", 0, 18);
            var ListaSheetNombreEstruc = ListaParaParaEstructura.Where(c=> !c._view.IsTemplate && c.ValorZ!=null)
                                                                .OrderBy(r => r.ValorZ.valorz).Where(c => c.Sheetnombre != "Sheet")
                                                                .Select(c => c.Sheetnombre).ToList();
            var ListaSheetNumeroEstruc_AUX = ListaParaParaEstructura.Where(r => (!ListaSheetNumeroEstruc.Contains(r.SheetNumber)))
                                                                .Select(c => c.SheetNumber).ToList();
            ListaSheetNumeroEstruc.AddRange(ListaSheetNumeroEstruc_AUX);

            string PArametroBusqueda_estruc = ParaEstructura;
            ListaEstructura = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => !c.IsTemplate && (c.ViewType == ViewType.CeilingPlan || c.ViewType == ViewType.EngineeringPlan) &&
                                                                                              c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_estruc))
                                                                                    .Select(c => new ViewDTO(c, ListaParaParaEstructura, ListaSheetNombreEstruc, ListaSheetNumeroEstruc ))
                                                                                    .OrderBy(c => c.ValorZ.valorz));

            ListaEstructura.ForEach(r => r.BuscarSiVIewInSheet());

            //losa
            var ListaSheetNumeroLOSA = ObtenerLIstaBAse("EST-2", 0, 18);
            var ListaSheetNombreLOSA = ListaParaParaLosa.Where(c => !c._view.IsTemplate && c.ValorZ != null).OrderBy(r => r.ValorZ.valorz).Where(c => c.Sheetnombre != "Sheet").Select(c => c.Sheetnombre).ToList();
            var ListaSheetNumeroLOSA_AUx = ListaParaParaLosa.Where(r => (!ListaSheetNumeroLOSA.Contains(r.SheetNumber)))
                                                        .Select(c => c.SheetNumber).ToList();
            ListaSheetNumeroLOSA.AddRange(ListaSheetNumeroLOSA_AUx);
            string PArametroBusqueda_armadura = ParaLosa;
            ListaLosa = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => c.ViewType == ViewType.FloorPlan &&
                                                                                         (c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_armadura)))
                                                                                    .Select(c => new ViewDTO(c, ListaParaParaLosa, ListaSheetNombreLOSA, ListaSheetNumeroLOSA))
                                                                                    .OrderBy(c => c.ValorZ.valorz));
            ListaLosa.ForEach(r => r.BuscarSiVIewInSheet());

            //elevaciones
            var ListaSheetNumeroELEVACION = ObtenerLIstaBAse("EST-3", 0, 18);
            var ListaSheetNombreELEVACION = ListaParaElevacion.Where(c => !c._view.IsTemplate && c.Sheetnombre != "Sheet").Select(c => c.Sheetnombre).ToList();
            var ListaSheetNumeroELEVACION_AUx = ListaParaElevacion.Where(r => (!ListaSheetNumeroELEVACION.Contains(r.SheetNumber)))
                                                              .Select(c => c.SheetNumber).ToList();
            ListaSheetNombreELEVACION.AddRange(ListaSheetNumeroELEVACION_AUx);
            string PArametroBusqueda_Elev = ParaElevacion;
            ListaElev = new ObservableCollection<ViewDTO>(aux_ListaEstructura.Where(c => c.ViewType == ViewType.Section &&
                                                                                         c.ObtenerNombre_TipoEstructura().Contains(PArametroBusqueda_Elev))
                                                                                    .Select(c => new ViewDTO(c, ListaParaElevacion, ListaSheetNombreELEVACION, ListaSheetNumeroELEVACION))
                                                                                    .OrderBy(c => c.Nombre));
            ListaElev.ForEach(r => r.BuscarSiVIewInSheet());
        }

        public List<string> ObtenerLIstaBAse(string ValorBase, int Inicial, int cantidodad)
        {
            List<string> Listastrings = new List<string>();

            for (int i = Inicial; i < cantidodad; i++)
            {
                if (i < 10)
                    Listastrings.Add(ValorBase + "0" + i);
                else
                    Listastrings.Add(ValorBase + i);

            }
            return Listastrings;
        }

        private void LoadNumeroSheetData(string caso1)
        {

            if (caso1 == "estructuras")
            {
                // estructuras
                var ListaParaParaEstructura = listaSheet.Where(v => v.TipoEstructura == ParaEstructura).ToList();
                var ListaSheetNumeroEstruc = ListaParaParaEstructura.Select(c => c.SheetNumber).ToList();
                //var ListaSheetNumeroEstruc=

                //  var listAux = ListaEstructura.ToList();
                int cont = 0;
                for (int i = 0; i < ListaEstructura.Count; i++)
                {
                    if (ListaEstructura[i].IsSelected)
                    {
                        ListaEstructura[i].NumeroSheet = "EST-" + (100 + cont).ToString();
                        cont += 1;
                    }
                    else
                        ListaEstructura[i].NumeroSheet = "";

                }

                ListaEstructura = new ObservableCollection<ViewDTO>(ListaEstructura);
            }
            else if (caso1 == "losa")
            {
                //losa
                var ListaParaParaLosa = listaSheet.Where(v => v.TipoEstructura == ParaLosa).ToList();
                var ListaSheetNumeroLOSA = ListaParaParaLosa.Select(c => c.SheetNumber).ToList();
                int cont = 0;
                for (int i = 0; i < ListaLosa.Count; i++)
                {

                    if (ListaLosa[i].IsSelected)
                    {
                        ListaLosa[i].NumeroSheet = "EST-" + (200 + cont).ToString();
                        cont += 1;
                    }
                    else
                        ListaLosa[i].NumeroSheet = "";
                }
                ListaLosa = new ObservableCollection<ViewDTO>(ListaLosa);
            }
            else if (caso1 == "elevaciones")
            {      //elevaciones
                var ListaParaElevacion = listaSheet.Where(v => v.TipoEstructura == ParaElevacion).ToList();
                var ListaSheetNumeroELEVACION = ListaParaElevacion.Select(c => c.SheetNumber).ToList();
                int cont = 0;
                for (int i = 0; i < ListaElev.Count; i++)
                {
                    if (ListaElev[i].IsSelected)
                    {
                        ListaElev[i].NumeroSheet = "EST-" + (300 + cont).ToString();
                        cont += 1;
                    }
                    else
                        ListaElev[i].NumeroSheet = "";
                }

                ListaElev = new ObservableCollection<ViewDTO>(ListaElev);
            }



        }


        private bool ObtenerSheetEnArchivo()
        {
            try
            {
                // para sheet
                listaSheet.Clear();
                var listaView = TiposView.ObtenerTodosSegunTipo(_doc, BuiltInCategory.OST_Sheets);
                for (int i = 0; i < listaView.Count; i++)
                {
                    var item = listaView[i];
                    if (item is Autodesk.Revit.DB.ViewSheet)
                    {
                        var viewPort = new ViewSheetNH(_uiapp, (Autodesk.Revit.DB.View)item);
                        viewPort.ObtenerDatos();
                        listaSheet.Add(viewPort);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return (listaSheet.Count> 0?true: false);
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void RecargarView(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void RecargarNumeroSheetView(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        #region External Project Methods


        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {          
            Button bton = (Button)sender;

            BotonOprimido = bton.Name;

            if (BotonOprimido == "CrearEstructutar")
            {
                if (!ValidarListaEstructural(ListaEstructura.ToList())) return;
            }
            else if (BotonOprimido == "CrearArmaduraLosa")
            {
                if (!ValidarListaEstructural(ListaLosa.ToList())) return;
            }
            else if (BotonOprimido == "CrearArmaduraEleva")
            {
                if (!ValidarListaEstructural(ListaElev.ToList())) return;
            }
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }

        private bool ValidarListaEstructural(List<ViewDTO> lista)
        {
            var listaIsChecket = lista.Where(c => c.IsSelected).ToList();

                                  
            for (int i = 0; i < listaIsChecket.Count; i++)
            {
                var item = listaIsChecket[i];

                if (item.NumeroSheet == "")
                {
                    Util.ErrorMsg($"View '{item.Nombre}' no tiene sheet asignado.\nDesmarcar o asignar un numero de Sheet.");
                    return false;
                }
                // BUSCAR SI PERTENECEN A OTRO SHHET
                var result =listaSheet.Where(c=> c.listaPortInSheet.Exists(r=> r.Name== item.Nombre)).FirstOrDefault();

                if (result != null)
                {
                    Util.ErrorMsg($"View '{item.Nombre}' se encontro adjunto en :\n\nSheet: {result.NombreVista}\nNumero:{result.SheetNumber}");
                    return false;
                }               
            }

            //buscar si tiene mas de una escala
            var ListaSheet_ISchecket = listaIsChecket.GroupBy(c => c.NumeroSheet).Select(r => r.Key).ToList();
            for (int j = 0; j < ListaSheet_ISchecket.Count; j++)
            {
                var _NumeroSheet = ListaSheet_ISchecket[j];
                int cantidadEscala = listaIsChecket.Where(v => v.NumeroSheet == _NumeroSheet).GroupBy(c => c.View_.Scale).Select(r => r.Key).Count();
                if (cantidadEscala != 1)
                {
                    Util.ErrorMsg($"Sheet con numero: '{_NumeroSheet}' tiene vistas con escalas diferente.\n\nNOTA: Los sheet tiene que tener vistas con igual escala");
                    return false;
                }
            }

            return true;
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

        private void SeleccionTodos_elev_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, true);
            ListaElev = new ObservableCollection<ViewDTO>(ListaElev);
        }
        private void SeleccionTodos_elev_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaElev, false);
            ListaElev = new ObservableCollection<ViewDTO>(ListaElev);
        }

        private void CambiarValor(ObservableCollection<ViewDTO> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsSelected = valor;
            }



        }

        private void SeleccionTodos_planta_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, true);
            ListaLosa = new ObservableCollection<ViewDTO>(ListaLosa);
        }
        private void SeleccionTodos_planta_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaLosa, false);
            ListaLosa = new ObservableCollection<ViewDTO>(ListaLosa);
        }
        private void SeleccionTodos_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, true);
            ListaEstructura = new ObservableCollection<ViewDTO>(ListaEstructura);
        }

        private void SeleccionTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEstructura, false);
            ListaEstructura = new ObservableCollection<ViewDTO>(ListaEstructura);
        }

        private void ActualizarEstruc_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null) return;
            if (button.Name == "ActualizarEstruc")
                LoadNumeroSheetData("estructuras");
            else if (button.Name == "ActualizarLosa")
                LoadNumeroSheetData("losa");
            else if (button.Name == "ActualizarElev")
                LoadNumeroSheetData("elevaciones");
        }

        private void VisulizarGeo(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Usamos el DataContext del botón para obtener el objeto de la lista
            var miObjeto = button.DataContext as ViewDTO;
            var listaMismoSheet= ListaElev.Where(c=> c.NumeroSheet==miObjeto.NumeroSheet).ToList();
            ServicioGenerarGoemtria _ServicioGenerarGoemtria = new ServicioGenerarGoemtria(_uiapp, miObjeto.NumeroSheet, listaMismoSheet);
            _ServicioGenerarGoemtria.Calcular();
            // Ahora puedes usar miObjeto para acceder a las propiedades del objeto de la lista
            // var propiedad = miObjeto.TuPropiedad;
        }
    }


}