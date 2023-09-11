using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace ArmaduraLosaRevit.Model.Cubicacion.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_Cub : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;



        public ObservableCollection<ViewSelected> _listaTodasView_general { get; set; }
        public ObservableCollection<ViewSelected> ListaTodasView_general
        {
            get { return _listaTodasView_general; }
            set
            {
                if (value == null) return; _listaTodasView_general = value;
               // RaisePropertyChanged("ListaTodasView");

            }

        }
        //1)
        public ObservableCollection<LevelDTO> _listaLevel;
        public ObservableCollection<LevelDTO> ListaLevel
        {
            get { return _listaLevel; }
            set
            {
                _listaLevel = value;
                RaisePropertyChanged("ListaLevel");
            }
        }

        //2) losas
        public ObservableCollection<LosaCubDto> _listaLosas;
        public ObservableCollection<LosaCubDto> ListaLosa
        {
            get { return _listaLosas; }
            set
            {
                _listaLosas = value;
                RaisePropertyChanged("ListaLosa");
            }
        }

        //3) Elev
        public ObservableCollection<ElevCubDto> _listaElev;
        public ObservableCollection<ElevCubDto> ListaElev
        {
            get { return _listaElev; }
            set
            {
                _listaElev = value;
                RaisePropertyChanged("ListaLevel");
            }
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
        

        public UI_Cub(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiapp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
           
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;
            LoadData();
        }

        private void LoadData()
        {
            // Level
            SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
            var ListaLevelDTO = _seleccionarNivel.M3_ObtenerListaNivelOrdenadoPorElevacionDeProyecto().Select(c => new LevelDTO(c)).ToList();
            ListaLevel = new ObservableCollection<LevelDTO>(ListaLevelDTO);

            //losa
            var listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);

            var ListaTodasLosaList = listaView.Where(c => c.ViewType == ViewType.FloorPlan)
                                    .Select(c => new ViewSelected( c.Name)).ToList();
            var vacio = new ViewSelected("");
            ListaTodasLosaList.Reverse();
            ListaTodasLosaList.Add(vacio);
            ListaTodasLosaList.Reverse();

            var ListaLosaList = listaView.Where(c => c.ViewType == ViewType.FloorPlan)
                                    .Select(c => new LosaCubDto(c.Name, c, ListaTodasLosaList));


           // 
            ListaLosa = new ObservableCollection<LosaCubDto>(ListaLosaList);

            //ListaLosa.Reverse();
            //ListaLosa.Add(vacio);
            //ListaLosa.Reverse();
            var ListaView3daux= TiposView3D.ObtenerTodos(_doc).Select(c=> c.Name).ToList();

            if (ListaView3daux.Count > 0)
            {
                if (ListaView3daux.Contains("{3D}"))
                    ActualView3d = "{3D}";
                else
                    ActualView3d = ListaView3daux.First();
            }

            ListaView3d = new ObservableCollection<string>(ListaView3daux);
            //elevaciones
            SeleccionarView _SeleccionarView = new SeleccionarView();
            var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);


            List<ElevCubDto> ListaElevList = new List<ElevCubDto>();
            for (int i = 0; i < ListaViewSection.Count; i++)
            {

                var Listainicial_aux = new List<ViewSelected>() { new ViewSelected("REPLICAR SEGUN CORRESPONDA"), new ViewSelected("") };
                //var all = ListaViewSection.Select(c => new ViewSelected(c.Name)).ToList();
                Listainicial_aux.AddRange(Listainicial_aux);

                var resul = Listainicial_aux.Select(c => c);
                ListaTodasView_general = new ObservableCollection<ViewSelected>(resul);

                var nuevoVAso = new ElevCubDto(ListaViewSection[i].Name, ListaViewSection[i], ListaTodasView_general);
                ListaElevList.Add(nuevoVAso);
            }

            //var ListaElevList = from vs in ListaViewSection                                                       
            //                    select new ElevCubDto(vs.Name, vs, ListaTodasView_general);

            ListaElev = new ObservableCollection<ElevCubDto>(ListaElevList);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }
        #region External Project Methods


        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;
            BotonOprimido = bton.Name;

            AnalizarLevel();

            // Raise external event with this UI instance (WPF) as an argument
            if (_mExternalMethodWpfArg != null)
                _mExternalMethodWpfArg.Raise(this);
            Close();
        }

        private void AnalizarLevel()
        {
            var listCOpiaLevel = ListaLevel.Select(c => new LevelDTO(c.Level_)).ToList();
            for (int i = 0; i < ListaLevel.Count; i++)
            {
                var result = ListaLevel[i];

                if (result.Nombre_cub != result.Nombre_cub_orig)
                {
                    var nuevoNivel = listCOpiaLevel.Where(c => c.Nombre_cub_orig == result.Nombre_cub).FirstOrDefault();

                    if (nuevoNivel == null) continue;

                    result.Elevacion = nuevoNivel.Elevacion;
                }
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
            var cbx=(System.Windows.Controls.ComboBox)sender;
            var nombre=cbx.Text;
        }

        private void ComboBox_DropDownClosed_1(object sender, EventArgs e)
        {
            var cbx = (System.Windows.Controls.ComboBox)sender;
            
            var _SelectedItem = cbx.SelectedItem;
            var _DisplayMemberPath = cbx.DisplayMemberPath;
            var _SelectedValue = cbx.SelectedValue;
            var _SelectedValuePath = cbx.SelectedValuePath;




        }
    }


}