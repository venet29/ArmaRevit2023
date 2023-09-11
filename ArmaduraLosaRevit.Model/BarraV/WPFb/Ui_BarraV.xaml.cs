using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Media3D;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.WPFb
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_BarraV : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;


        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }



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
#pragma warning disable CS0169 // The field 'Ui_BarraV.ImageOprimido' is never used
        private string ImageOprimido;
#pragma warning restore CS0169 // The field 'Ui_BarraV.ImageOprimido' is never used

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

        public Ui_BarraV(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg, EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();

            //this.Left = ubicacionVentana.X + 100;
            //this.Top = ubicacionVentana.Y + 100;
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            if (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras == false)
            {
                cbx_diseñoBarras.Text = "Desplazar";
            }
            else
            {
                cbx_diseñoBarras.Text = "Dibujar";
            }

            TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
            Espaciamiento = "20";
            this.Topmost = true;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }

        public ConfiguracionIniciaWPFlBarraVerticalDTO ObtenerConfiguracionInicialBarraVerticalVDTO()
        {
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO = new ConfiguracionIniciaWPFlBarraVerticalDTO()
            {
                inicial_diametroMM = Util.ConvertirStringInInteger(tbx_diametro.Text),
                Inicial_Cantidadbarra = tbx_cantidad.Text,
                incial_ComoIniciarTraslapo_LineaPAr = ObternerTraslapoInicioSegundaBarra(),
                incial_ComoIniciarTraslapo_LineaImpar = Util.ConvertirStringInInteger(cbx_traslapo_inicio.Text),//barra incio, barra mas al borde del muro
                inicial_ComoTraslapo = Util.ConvertirStringInInteger(cbx_traslapo_recorrido.Text),
                Document_ = _doc,
                //cbx_tipopata.Text
                inicial_tipoBarraV = ObtenerTipoPataV(),// Enumeraciones.TipoBarraV.BarraVSinPatas,
                inicial_IsDirectriz = (rbt_si.IsChecked == true ? true : false),
                inicial_ISIntercalar = (cbx_traslapo_intercalar.Text == "Si" ? true : false),
                Inicial_espacienmietoCm_EntreLineasBarras = tbx_espaciamiento.Text,
                TipoSeleccionMousePtoInferior = (rbt_ini_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                TipoSeleccionMousePtoSuperior = (rbt_sup_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                IsDibujarTag = true,
                IsInvertirPosicionTag = (cbx_text_pos.Text == "Normal" ? false : true),
                TipoBarraRebar_ = TipoBarraVertical.Cabeza,
                BarraTipo = TipoRebar.ELEV_BA_V,
                TipoSelecion =  (cbx_text_TipoSeleccionVertical.Text== "Elemento" ? TipoSeleccion.ConElemento: TipoSeleccion.ConMouse)
            };



            return confiEnfierradoDTO;
        }

        public EditarBarraDTO EditarBarraDTO(TipoCasobarra TipoCasobarra)
        {
            return new EditarBarraDTO()
            {
                cantidad = Util.ConvertirStringInInteger(tbx_cantidad.Text),
                diametro = Util.ConvertirStringInInteger(tbx_diametro.Text),
                tipobarraV = ObtenerTipoBarraCambiar(),
                TipoCasobarra = TipoCasobarra,
                IsCambiarDiametroYEspacia = ObtenerIsDiametroYCantidad()

            };
        }

        public ConfiguracionInicialBarraHorizontalDTO Obtener_ManejadoRefuerzoVigaCentral80_YRefuerzoEntreVigas()
        {
            return new ConfiguracionInicialBarraHorizontalDTO()
            {
                IsDibujarBArra = true,
                Inicial_Cantidadbarra = tbx_cantidadH.Text,
                incial_diametroMM = Util.ConvertirStringInInteger(tbx_diametroH.Text),
                Inicial_espacienmietoCm_direccionmuro = tbx_espaciamientoH.Text,
                inicial_tipoBarraH = TipoPataBarra.BarraVSinPatas,
                LineaBarraAnalizada = Util.ConvertirStringInInteger(cbx_Ubicacion_LineaH.Text),
                incial_IsDirectriz = true

            };
        }
        public ConfiguracionInicialBarraHorizontalDTO ObtenerConfiguracionInicialBarraHorizontalVDTO()
        {
            ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO = new ConfiguracionInicialBarraHorizontalDTO()
            {
                incial_diametroMM = Util.ConvertirStringInInteger(tbx_diametroH.Text),
                Inicial_Cantidadbarra = tbx_cantidadH.Text,
                incial_ComoIniciarTraslapo_LineaPAr = 1,
                incial_ComoIniciarTraslapo_LineaImpar = 2,//barra incio, barra mas al borde del muro
                incial_ComoTraslapo = 2,

                inicial_tipoBarraH = ObtenerTipoPataHorizontal(),
                incial_IsDirectriz = false,
                incial_ISIntercalar = false,
                Inicial_espacienmietoCm_direccionmuro = tbx_espaciamientoH.Text,
                BarraTipo = TipoRebar.ELEV_BA_H,
                DireccionTraslapoH_ = ObtenerDireccionTraslapo(),// DireccionTraslapoH.izquierda
                TipoSelecion = (cbx_text_TipoSeleccionHorizontal2.Text == "Elemento" ? TipoSeleccion.ConElemento : TipoSeleccion.ConMouse)
            };
            return confiEnfierradoDTO;
        }

        private DireccionTraslapoH ObtenerDireccionTraslapo()
        {
            if (cbx_DirecTras.Text.ToLower() == "izquierda")
            { return DireccionTraslapoH.izquierda; }
            else if (cbx_DirecTras.Text.ToLower() == "central")
            { return DireccionTraslapoH.central; }
            else if (cbx_DirecTras.Text.ToLower() == "derecha")
            { return DireccionTraslapoH.derecha; }
            else
            { return DireccionTraslapoH.central; }
        }

        public bool ObtenerIsDiametroYCantidad()
        {
            switch (BotonOprimido)
            {
                case "barraSinV":
                case "barraInferiorV":
                case "barraAmbosV":
                case "barraSuperiorV":

                    return IsCambiarDiamCantV.IsChecked ?? false;
                case "barraSuperiorH":
                case "barraInferiorH":
                case "barraAmbosH":
                case "barraSinH":
                    return IsCambiarDiamCantH.IsChecked ?? false;
                default:
                    return false;
            }

        }

        public TipoPataBarra ObtenerTipoBarraCambiar()
        {
            switch (BotonOprimido)
            {
                case "barraSinV":
                case "barraSinH":
                    return TipoPataBarra.BarraVSinPatas;
                case "barraInferiorV":
                case "barraInferiorH":
                    return TipoPataBarra.BarraVPataInicial;
                case "barraSuperiorV":
                case "barraSuperiorH":
                    return TipoPataBarra.BarraVPataFinal;
                case "barraAmbosV":
                case "barraAmbosH":
                    return TipoPataBarra.BarraVPataAmbos;
                default:
                    return TipoPataBarra.BarraVSinPatas;
            }

        }
        public TipoPataBarra ObtenerTipoPataV()
        {

            if (cbx_tipoBusqueda_pataV.Text == "Auto")
            { return TipoPataBarra.BarraVPataAUTO; }
            else if (cbx_tipoBusqueda_pataV.Text == "Inferior")
            { return TipoPataBarra.BarraVPataInicial; }
            else if (cbx_tipoBusqueda_pataV.Text == "Superior")
            { return TipoPataBarra.BarraVPataFinal; }
            else if (cbx_tipoBusqueda_pataV.Text == "Ambos")
            { return TipoPataBarra.BarraVPataAmbos; }
            else if (cbx_tipoBusqueda_pataV.Text == "Sin")
            { return TipoPataBarra.BarraVSinPatas; }
            else
            { return TipoPataBarra.NoBuscar; }

        }

        public TipoPataBarra ObtenerTipoPataHorizontal()
        {

            if (cbx_tipoBusqueda.Text == "Auto")
            { return TipoPataBarra.BarraVPataAUTO; }
            else if (cbx_tipoBusqueda.Text == "Inicial")
            { return TipoPataBarra.BarraVPataInicial; }
            else if (cbx_tipoBusqueda.Text == "Final")
            { return TipoPataBarra.BarraVPataFinal; }
            else if (cbx_tipoBusqueda.Text == "Ambos")
            { return TipoPataBarra.BarraVPataAmbos; }
            else if (cbx_tipoBusqueda.Text == "Sin")
            { return TipoPataBarra.BarraVSinPatas; }
            else
            { return TipoPataBarra.NoBuscar; }

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

        public int ObternerTraslapoInicioSegundaBarra()
        {
            int resul = 1;

            if (cbx_traslapo_inicio.Text == "1")
            {
                resul = Util.ConvertirStringInInteger(cbx_traslapo_recorrido.Text);
            }
            else if (cbx_traslapo_inicio.Text == "2")
            {
                if (cbx_traslapo_recorrido.Text == "3")
                    resul = Util.ConvertirStringInInteger(cbx_traslapo_recorrido.Text);
                else
                    resul = 1;
            }
            else if (cbx_traslapo_inicio.Text == "3")
            {
                resul = 2;
            }

            return resul;
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
            Button boton = (Button)sender;

            if (boton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;
            }

            BotonOprimido = boton.Name;

            if (BotonOprimido == "btnCrearBarra" || BotonOprimido == "btnBorrarRebar" || BotonOprimido == "btnBorrarRebar2" || 
                BotonOprimido == "btnCrearBarraH" || BotonOprimido == "btnCrearBarraHTramo" || BotonOprimido == "btnAgruparHRebar")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else if (BotonOprimido != "btnCerrar_e" || BotonOprimido != "btnCerrar_eH")
            {
                Close();
            }

        }


        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image boton = (Image)sender;

            BotonOprimido = boton.Name;
            _mExternalMethodWpfArg.Raise(this);
        }

        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void tbx_diametroH_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbx_diseñoBarras_DropDownClosed(object sender, EventArgs e)
        {
            if (cbx_diseñoBarras.Text == "Desplazar")
            {
                VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = false;
            }
            else
            {
                VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = true;
            }
        }
    }


}