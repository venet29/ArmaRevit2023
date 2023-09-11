﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.LosaEstructural.WPFp
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_pelotaLosa : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;


        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }




        private System.Windows.Visibility _isEstructura;

        public System.Windows.Visibility IsEstructura
        {
            get { return _isEstructura; }
            set
            {
                if (_isEstructura != value)
                {
                    _isEstructura = value;
                    RaisePropertyChanged("IsEstructura");
                }
            }
        }

        private System.Windows.Visibility _isArmadura;

        public System.Windows.Visibility IsArmadura
        {
            get { return _isArmadura; }
            set
            {
                if (_isArmadura != value)
                {
                    _isArmadura = value;
                    RaisePropertyChanged("IsArmadura");
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

        public Ui_pelotaLosa(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
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

            TipoBarra_ = TipoBarraTraslapoDereArriba.f4;
            Espaciamiento = "20";
            this.Topmost = true;
            IsEstructura = System.Windows.Visibility.Visible;
            IsArmadura = System.Windows.Visibility.Hidden;
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

            if (BotonOprimido != "btnCerrar_e" || BotonOprimido != "btnCerrar_var")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else
            {
                Close();
            }

        }


        private void tipoPElota_DropDownClosed(object sender, EventArgs e)
        {
            System.Windows.Controls.ComboBox actual = sender as System.Windows.Controls.ComboBox;
            if (actual.Text == "Estructura")
            {
                IsEstructura = System.Windows.Visibility.Visible;
                IsArmadura = System.Windows.Visibility.Hidden;
            }
            else if (actual.Text == "Armadura")
            {
                IsEstructura = System.Windows.Visibility.Hidden;
                IsArmadura = System.Windows.Visibility.Visible;
            }
        }
    }


}