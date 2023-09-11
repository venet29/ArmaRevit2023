using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.Pasadas.Servicio;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pasadas.WPFPasada
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_Pasadas : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiapp;


        public List<EnvoltorioBase> ListaEnvoltorioPipesTodos { get; set; }




        public ObservableCollection<EnvoltorioBase> _listaEnvoltorioMEP;
        public ObservableCollection<EnvoltorioBase> ListaEnvoltorioMEP
        {
            get { return _listaEnvoltorioMEP; }
            set
            {
                _listaEnvoltorioMEP = value;
                RaisePropertyChanged("ListaEnvoltorioMEP");
            }
        }


        private ObservableCollection<string> _listaFiltroTipo;
        public ObservableCollection<string> ListaFiltroTipo
        {
            get { return _listaFiltroTipo; }
            set
            {
                _listaFiltroTipo = value;
                RaisePropertyChanged("ListaFiltroTipo");
            }
        }

        private ObservableCollection<string> _listaEjes;
        public ObservableCollection<string> ListaEjes
        {
            get { return _listaEjes; }
            set
            {
                _listaEjes = value;
                RaisePropertyChanged("ListaEjes");
            }
        }


        private ObservableCollection<string> _listaFiltroCreados;
        public ObservableCollection<string> ListaFiltroCreados
        {
            get { return _listaFiltroCreados; }
            set
            {
                _listaFiltroCreados = value;
                RaisePropertyChanged("ListaFiltroCreados");
            }
        }


        private int _cantidadElement;
        public int CantidadElement
        {
            get { return _cantidadElement; }
            set
            {
                _cantidadElement = value;
                RaisePropertyChanged("CantidadElement");
            }
        }

        private string _actualView3d;
        public string ActualView3d
        {
            get { return _actualView3d; }
            set
            {
                _actualView3d = value;
                RaisePropertyChanged("ActualView3d");
            }
        }



        private string _actualRevitLink;
        public string ActualRevitLink
        {
            get { return _actualRevitLink; }
            set
            {
                _actualRevitLink = value;
                RaisePropertyChanged("ActualRevitLink");
            }
        }
        private System.Windows.Visibility _isVisibleCanitdadPipes;
        public System.Windows.Visibility IsVisibleCantidadPipes
        {
            get { return _isVisibleCanitdadPipes; }
            set
            {
                _isVisibleCanitdadPipes = value;
                RaisePropertyChanged("IsVisibleCantidadPipes");
            }
        }

        public int IdBarra { get; set; }
        public EnvoltorioBase _EnvoltorioPipesSeleccionado { get; set; }
        public string Nombre3D { get; internal set; }



        public List<string> ListaView3d { get; set; }

        public string SelectView3d { get; set; }

        public string SelectlistaRevitLink { get; set; }

        public List<LinkDOcumentosDTO> listaRevitLink { get; private set; }
        public LinkDOcumentosDTO LinkSeleccionado { get; private set; }


        public string SelectRevitLink { get; set; }

        public double ZminNivel { get; private set; }
        public double ZMaxNivel { get; private set; }
        public bool ISvisibilidad { get; set; }
        public double AreaMaxAutovalid_foot2 { get; set; }
        public int Inicio { get; internal set; }
        public int Cantidad { get; internal set; }
        public double AnchoSeleccionValor_foot { get; private set; }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public UI_Pasadas(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiapp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();

            combobox_ListaFiltroTipo.DropDownClosed += ComboBox_DropDownClosed;
            combobox_ListaFiltroCreado.DropDownClosed += ComboBox_DropDownClosed;
            combobox_ListaEjes.DropDownClosed += ComboBox_DropDownClosed;

            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;
            ISvisibilidad = false;
            IsVisibleCantidadPipes = System.Windows.Visibility.Hidden;
            LoadData();
        }

        internal TipoElementoBArraV ObtenerTIpoElementoIntersectar()
        {
            string NOmbreFiltro = combobox_ListaFiltroInterseccion.Text;


            if (NOmbreFiltro == "Losas")
                return TipoElementoBArraV.losa;
            else if (NOmbreFiltro == "Muros o Vigas")
                return TipoElementoBArraV.muro;
            else
                return TipoElementoBArraV.none;

        }

        private void LoadData()
        {
            ResetarLIstas();

            CantidadElement = 0;

            ListaView3d = TiposView3D.ObtenerTodos(_doc).Select(c => c.Name).ToList();

            listaRevitLink = Tipos_LinkDOcumento.ObtenerLinkDocumentoActual(_doc).ToList();

            if (listaRevitLink.Count == 0)
            {
                Util.InfoMsg($"Proyecto sin link de especialidades ");
                return;
            }

            listaRevitLink.Add(new LinkDOcumentosDTO() { Nombre = "Todos", Pathname = "Todos" });
            if (listaRevitLink.Count - 1 > 1)
            {
                Util.InfoMsg($"Se encontraron {listaRevitLink.Count - 1} linkrevit, si tiene otros proyecto abierto con link, verificar que este utilizando el link correcto. Se utiliza el primero encontrado  ");

                var primeroLink = listaRevitLink.FirstOrDefault();
                //   combobox_Listalink.Text = primeroLink.Pathname;
                ActualRevitLink = primeroLink.Pathname;
            }
            else if (listaRevitLink.Count == 0)
            {
                Util.InfoMsg($"No se encontraron linkrevit en el proyecto");
            }
            else
            {

                var primeroLink = listaRevitLink.FirstOrDefault();
                LinkSeleccionado = primeroLink;
                //combobox_Listalink.Text = primeroLink.Pathname;
                ActualRevitLink = primeroLink.Pathname;
            }

            if (ListaView3d.Count > 0)
            {
                if (_doc.ActiveView is View3D)
                    ActualView3d = _doc.ActiveView.Name;
                else if (ListaView3d.Contains("{3D}"))
                    ActualView3d = "{3D}";
                else
                    ActualView3d = ListaView3d.First();

                SelectView3d = ActualView3d;
            }


            SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
            List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion();

            if (listaLevel.Count > 0)
            {
                ZminNivel = listaLevel.First().ElevacionProjectadaRedondeada;
                ZMaxNivel = listaLevel.Last().ElevacionProjectadaRedondeada;
            }
            else
            {
                ZminNivel = -40;
                ZMaxNivel = 40;
            }

            // cargar y habilitar familias de pasadas


        }

        public void ResetarLIstas()
        {
            ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>();
            ListaEnvoltorioPipesTodos = new List<EnvoltorioBase>();
            ListaFiltroTipo = new ObservableCollection<string>();
            ListaFiltroCreados = new ObservableCollection<string>();
            ListaEjes = new ObservableCollection<string>();
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

            if (inicio_list.Visibility == System.Windows.Visibility.Visible)
            {
                Inicio = Util.ConvertirStringInInteger(inicio_list.Text);
                Cantidad = Util.ConvertirStringInInteger(Cantidad_list.Text);
            }
            else
            {
                Inicio = 0;
                Cantidad = 100000;
            }


            if (BotonOprimido == "Crear_BorrarShafopenin")
            {
                this.Topmost = false;
                string input = Microsoft.VisualBasic.Interaction.InputBox("Seguro desea borrar los 'SHAFT OPENING' creado con la aplicacion.\n\nConfirmar escribiendo : borrar", "Borrar", "", 300, 300);
                this.Topmost = true;
                if (input.Trim().ToLower() != "borrar") return;
            }
            else if (BotonOprimido == "Crear_BorrarSoloPasadas")
            {
                this.Topmost = false;
                string input = Microsoft.VisualBasic.Interaction.InputBox("Seguro desea borrar las 'PASADAS' creado con la aplicacion.\n\nConfirmar escribiendo : borrar", "Borrar", "", 300, 300);
                this.Topmost = true;
                if (input.Trim().ToLower() != "borrar") return;
            }
            else if (BotonOprimido == "Crear_TodasShaft")
            {
                this.Topmost = false;
                string input = Microsoft.VisualBasic.Interaction.InputBox("Seguro desea crear 'SHAFT Y PASADAS ROJAS'.\n\nConfirmar escribiendo : Crear", "Crear", "", 300, 300);
                this.Topmost = true;
                if (input.Trim().ToLower() != "crear") return;
            }


            //*******************************************************************************************

            if (BotonOprimido == "Crear_TodasShaft" && !Util.IsNumeric(TextBox_AreaMIn.Text))
            {
                Util.InfoMsg("Para Crear Shaft se debe un area minima para generar el shaft.");
                return;
            }
            else if (BotonOprimido == "Crear_TodasShaft")
            {
                if (!Util.IsNumeric(TextBox_AreaMIn.Text))
                {
                    Util.InfoMsg("area minima tiene que ser valor numerico entre 0.0004 m2 y 0.36 m2 ");
                    return;
                }

                double areaminMt2 = Util.ConvertirStringInDouble(TextBox_AreaMIn.Text);


                if (0 > areaminMt2 && areaminMt2 > 0.36)
                {
                    Util.InfoMsg("Para Crear Shaft se debe asignar una area minima entre 0.0004 m2 y 0.36 m2 ");
                    return;
                }

                double largo_mt = Math.Sqrt(areaminMt2);
                double largo_mm = largo_mt * 1000;
                AreaMaxAutovalid_foot2 = Util.MmToFoot(largo_mm) * Util.MmToFoot(largo_mm);
            }

            if (BotonOprimido == "Crear_TodasPasadas" || BotonOprimido == "Revision_TodasPasadas" || BotonOprimido == "infoLink")
            {
                if (combobox_Listalink.Text == "")
                {
                    Util.ErrorMsg("Linkrevit no puede estar vacio");
                    return;
                }

                LinkSeleccionado = listaRevitLink.Where(c => c.Pathname == combobox_Listalink.Text).FirstOrDefault();
                if (LinkSeleccionado == null)
                {
                    Util.ErrorMsg("Error al obtener LinkRevit de lista 'LinkRevit'");
                    return;
                }
            }

            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
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

        private void CambiarValor(ObservableCollection<EnvoltorioBase> list, bool valor)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i];
                result.IsSelected = valor;
            }
        }


        private void SeleccionTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEnvoltorioMEP, false);
            ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioMEP);
        }

        private void SeleccionTodos_Checked(object sender, RoutedEventArgs e)
        {
            CambiarValor(ListaEnvoltorioMEP, true);
            ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioMEP);
        }

        private void Vere3D(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl == null) return;
            IdBarra = Util.ConvertirStringInInteger(lbl.Content.ToString());

            if (Util.IsNumeric(AnchoSeleccion.Text.ToString()))
                AnchoSeleccionValor_foot = Util.CmToFoot(Util.ConvertirStringInDouble(AnchoSeleccion.Text.ToString()));
            else
                AnchoSeleccionValor_foot = Util.CmToFoot(30);
            
            _EnvoltorioPipesSeleccionado = ListaEnvoltorioMEP.Where(c => c.NombreId == lbl.Content.ToString()).FirstOrDefault();

            if (_EnvoltorioPipesSeleccionado == null) return;

            BotonOprimido = "SeleccionarPipe";
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }



        private void OnCrearIndividual(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button bton = (Button)sender;

            if (bton == null) return;
            IdBarra = Util.ConvertirStringInInteger(bton.Tag.ToString());



            _EnvoltorioPipesSeleccionado = ListaEnvoltorioMEP.Where(c => c.NombreId == IdBarra.ToString()).FirstOrDefault();

            if (_EnvoltorioPipesSeleccionado == null) return;
            BotonOprimido = "CrearIndividual";
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }

        private void IdFiltrado_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            // your event handler here - true sifnifica que tu evento ya fue contraldo y los siguiente evento a este se pueden ejecutar, por defecto    e.Handled = false;


            if (IdFiltrado.Text == "")
            {
                ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioPipesTodos);
            }
            else
            {
                List<EnvoltorioBase> listaFiltrada = new List<EnvoltorioBase>();
                

                if (tipoFiltro.Text == "Pasada")
                    listaFiltrada = ListaEnvoltorioPipesTodos.Where(c => c.PasadaId.ToString() == IdFiltrado.Text).ToList();
                else
                    listaFiltrada = ListaEnvoltorioPipesTodos.Where(c => c.NombreId == IdFiltrado.Text).ToList();

                if (listaFiltrada.Count != 0)
                {
                    ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(listaFiltrada);
                    CantidadElement = ListaEnvoltorioMEP.Count;
                }
            }
            e.Handled = true;
        }


        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            System.Windows.Controls.ComboBox bton = (System.Windows.Controls.ComboBox)sender;
            if (bton == null) return;

            RecargarLIstaCOmpleta();

        }



        internal void RecargarLIstaCOmpleta()
        {
            // ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioPipesTodos);
            string tipoDucto = combobox_ListaFiltroTipo.Text;
            string tipoCreaion = combobox_ListaFiltroCreado.Text;
            string ejes = combobox_ListaEjes.Text;

            if (tipoDucto == "" && tipoCreaion == "" && ejes == "")
            {
                ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioPipesTodos);
            }
            else
            {
                var listaFiltrada = ListaEnvoltorioPipesTodos.Where(c =>
                                        (tipoDucto == "" ? true : c.NombreDucto == tipoDucto) &&
                                        (tipoCreaion == "" ? true : c.EstadoShaft == tipoCreaion) &&
                                        (ejes == "" ? true : c.ejesGrilla == ejes)
                                        ).ToList();
                ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(listaFiltrada);
            }

            CantidadElement = ListaEnvoltorioMEP.Count;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DibujarLinea(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button bton = (Button)sender;

            if (bton == null) return;
            IdBarra = Util.ConvertirStringInInteger(bton.Tag.ToString());

            _EnvoltorioPipesSeleccionado = ListaEnvoltorioMEP.Where(c => c.NombreId == IdBarra.ToString()).FirstOrDefault();

            if (_EnvoltorioPipesSeleccionado == null) return;
            BotonOprimido = "DibujarLinea";
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Rechasar.Text = "";
        }

        private void LimpiarRevision_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Revisar.Text = "";
        }

        private void AnchoSeleccion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }


}