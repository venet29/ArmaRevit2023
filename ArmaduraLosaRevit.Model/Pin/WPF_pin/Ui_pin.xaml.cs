using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pin.WPF_pin
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_pin : Window, INotifyPropertyChanged
    {

        public string ImageOprimido { get; set; }
        private readonly Document _doc;
   
        private UIApplication _uiApp;
        public tipoVIewNh viewModificados { get; set; }
        public ObservableCollection<string> ListaView { get;  set; }
        public bool IsNivelActual { get;  set; }
        public bool IsPinner { get;  set; }


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_pin(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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



            ListaView = new ObservableCollection<string>() {
                tipoVIewNh.Todas_las_View.ToString().Replace("_"," "),
                tipoVIewNh.Solo_Elevaciones.ToString().Replace("_"," "),
                tipoVIewNh.Solo_Losa_y_Fundacion.ToString().Replace("_"," "), };
            //ListaView = new ObservableCollection<string>() {
            //    tipoVIewNh.Todas_las_View.ToString(),
            //    tipoVIewNh.Solo_Elevaciones.ToString(),
            //    tipoVIewNh.Solo_Losa_y_Fundacion.ToString(), };



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

      

        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image bton = (Image)sender;

            ImageOprimido = bton.Name;

            if (ImageOprimido != "PinAll" ||
                ImageOprimido != "Pin1" ||
                ImageOprimido != "DesPinAll" ||
                ImageOprimido != "DesPin1")
            {
                viewModificados = EnumeracionBuscador.ObtenerEnumGenerico(tipoVIewNh.NONE, viewAnalizados.Text.Replace(" ","_").Trim());
               // viewModificados = EnumeracionBuscador.ObtenerEnumGenerico(tipoVIewNh.NONE, viewAnalizados.Text);

                if (ImageOprimido == "PinAll" || ImageOprimido == "DesPinAll")
                    IsNivelActual = false;
                else
                    IsNivelActual = true;

                if (ImageOprimido == "PinAll" || ImageOprimido == "Pin1")
                    IsPinner = true;
                else
                    IsPinner = false;

                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else
            {
                Close();
            }
        }
    }


}