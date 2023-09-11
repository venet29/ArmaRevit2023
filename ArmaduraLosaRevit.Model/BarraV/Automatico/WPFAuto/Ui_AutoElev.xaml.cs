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
using ComboBox = System.Windows.Controls.ComboBox;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_AutoElev : Window, INotifyPropertyChanged
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


        private string _removerView;
        public string RemoverView
        {
            get { return _removerView; }
            set
            {
                if (_removerView != value)
                {
                    _removerView = value;
                    RaisePropertyChanged("RemoverView");
                }
            }
        }

        private string _removerJson;
        public string RemoverJson
        {
            get { return _removerJson; }
            set
            {
                if (_removerJson != value)
                {
                    _removerJson = value;
                    RaisePropertyChanged("RemoverJson");
                }
            }
        }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_AutoElev(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,EventHandlerWithWpfArg eExternalMethodWpfArg)
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

            if (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras == false)
            {
                cbx_diseñoBarras.Text = "Desplazar";
                cbx_diseñoBarrasM.Text = "Desplazar";
            }
            else
            {
                cbx_diseñoBarras.Text = "Dibujar";
                cbx_diseñoBarrasM.Text = "Dibujar";
            }

            RemoverView = "ELEVACION EJE ";
            RemoverJson = "_";


            //TbDebug.Text = "NOTA: \n1) Ocultar Level intermedio o no necesario \n2)Ocultar Level a nivel o por debajo  de fundaciones";
         //   TbDebugM.Text = "NOTA: \n1) Ocultar Level intermedio o no necesario \n2)Ocultar Level a nivel o por debajo  de fundaciones";

            TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
            Espaciamiento = "20";
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
            Button bton = (Button)sender;

            if (bton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;
            }

            BotonOprimido = bton.Name;

            if (BotonOprimido == "Ejecutar" || BotonOprimido == "EjecutarH" || BotonOprimido == "Ejecutar_variasM" || 
                BotonOprimido == "Ocultar_Level" || BotonOprimido == "Mostrar_Level" ||
               BotonOprimido == "DesAgrupar" || BotonOprimido == "Agrupar" )
             {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else if (BotonOprimido == "Cerrar" || BotonOprimido == "CerrarM")
            {
                Close();
            }
      
        }

        private void cbx_diseñoBarras_DropDownClosed(object sender, EventArgs e)
        {
            var chbox = sender as ComboBox;

            if (chbox.Text == "Desplazar")
            {
                VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = false;
            }
            else
            {
                VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = true;
            }

        }


        private void DesAgrupar_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.F1) return;

            VerVideoAyudas.Ejecutar("AgruparDesAgrupar");
        }

        private void DesAgrupar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DesAgrupar.Focus();       
        }

        private void Agrupar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Agrupar.Focus();
        }
    }


}