using ArmaduraLosaRevit.Model.modeloNH;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.model;
using ArmaduraLosaRevit.Model.Viewnh.posicion;
using System;
//using Autodesk.Revit.DB;
//using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component
{
    /// <summary>
    /// Interaction logic for PorView.xaml
    /// </summary>
    public partial class PorView : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        //  UIApplication _uiapp;
        public ObservableCollection<PorTiposNh> _listaTipoElev;
        public ObservableCollection<PorTiposNh> ListaTipoElev
        {
            get { return _listaTipoElev; }
            set
            {
                _listaTipoElev = value;
                RaisePropertyChanged("ListaTipoElev");
            }
        }

        //2
        public ObservableCollection<PorTiposNh> _listaTipoLosa;
        public ObservableCollection<PorTiposNh> ListaTipoLosa
        {
            get { return _listaTipoLosa; }
            set
            {
                _listaTipoLosa = value;
                RaisePropertyChanged("ListaTipoLosa");
            }
        }
        //3
        public ObservableCollection<PorTiposNh> _listaTipoFund;
        public ObservableCollection<PorTiposNh> ListaTipoFund
        {
            get { return _listaTipoFund; }
            set
            {
                _listaTipoFund = value;
                RaisePropertyChanged("ListaTipoFund");
            }
        }

        //4
        public ObservableCollection<PorTiposNh> _listaTipoTodos;
        public ObservableCollection<PorTiposNh> ListaTipoTodos
        {
            get { return _listaTipoTodos; }
            set
            {
                _listaTipoTodos = value;
                RaisePropertyChanged("ListaTipoTodos");
            }
        }


        public static readonly DependencyProperty myGridSizeProperty =
       DependencyProperty.Register("myGridSize", typeof(string), typeof(PorView), new FrameworkPropertyMetadata(null)); //

        public string myGridSize
        {
            get { return (string)GetValue(myGridSizeProperty); }
            set
            {
                SetValue(myGridSizeProperty, value);
                RaisePropertyChanged("myGridSize");
            }
        }

        //parameter 2
        public static readonly DependencyProperty paraCOnListaProperty =
        DependencyProperty.Register("PorViewDTO", typeof(PorViewDTO_), typeof(PorView), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnValueChanged),
                                              new CoerceValueCallback(CoerceValue))); //

        public PorViewDTO_ PorViewDTO
        {
            get { return (PorViewDTO_)GetValue(paraCOnListaProperty); }
            set
            {

                SetValue(paraCOnListaProperty, value);
                RaisePropertyChanged("PorViewDTO");
            }
        }
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PorView control = (PorView)obj;
            //control.Load();

          //  RoutedPropertyChangedEventArgs<PorViewDTO_> e = new RoutedPropertyChangedEventArgs<PorViewDTO_>(
          //(PorViewDTO_)args.OldValue, (PorViewDTO_)args.NewValue, ValueChangedEvent);
          //  control.OnValueChanged(e);
        }


        private static object CoerceValue(DependencyObject element, object value)
        {
            PorViewDTO_ newValue = (PorViewDTO_)value;
            PorView control = (PorView)element;
            control.Load(newValue);

            return newValue;
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
      "ValueChanged", RoutingStrategy.Bubble,
      typeof(RoutedPropertyChangedEventHandler<PorViewDTO_>), typeof(PorView));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<PorView> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        bool IStodosElev;
        bool IStodosLosa;
        bool IStodosFund;
        public PorView()
        {
            InitializeComponent();
            this.DataContext = this;
            IStodosElev = true;
            IStodosLosa = true;
            // Load();
        }


        private  void Load(PorViewDTO_ PorViewDTO_aux)
        {
            //PorViewDTO = PorViewDTO_aux;
            if (PorViewDTO_aux != null)
            {

                // var list1 = PorViewDTO.ListaElev;
                ListaTipoElev = new ObservableCollection<PorTiposNh>(PorViewDTO_aux.ListaElev);

                ListaTipoLosa = new ObservableCollection<PorTiposNh>(PorViewDTO_aux.ListaLosa);

                //var lst3 = PorTiposNh.ObtenerTiposFund();
                //ListaTipoFund = new ObservableCollection<PorTiposNh>(lst3);

                List<PorTiposNh> ListaTodos = new List<PorTiposNh>();
                ListaTodos.AddRange(PorViewDTO_aux.ListaElev);
                ListaTodos.AddRange(PorViewDTO_aux.ListaLosa);
                //ListaTodos.AddRange(lst3);
                ListaTipoTodos = new ObservableCollection<PorTiposNh>(ListaTodos);
            }

            // IStodosFund = true;

        }
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<PorTiposNh> args)
        {
            
        }


        public string BotonOprimido { get; private set; }

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click2", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PorView));
        // public static readonly RoutedEvent ClickEvent1 = ButtonBase.ClickEvent.AddOwner(typeof(Diametros));

        // public event RoutedEventHandler Click1;
        public event RoutedEventHandler Click2
        {
            add { btn_TipoView_.AddHandler(ClickEvent, value); }
            remove { btn_TipoView_.RemoveHandler(ClickEvent, value); }
        }
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

        private void Btn_Tipo_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;
            if (boton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;

            }
            BotonOprimido = boton.Name;
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }

        private void Cambiar(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Name == "lblElev")
            {
                IStodosElev = !IStodosElev;
                cambiarEstado(ListaTipoElev, IStodosElev);
            }
            else if (lbl.Name == "lblLosa")
            {
                IStodosLosa = !IStodosLosa;
                cambiarEstado(ListaTipoLosa, IStodosLosa);
            }
            else if (lbl.Name == "lblFund")
            {
                IStodosFund = !IStodosFund;
                cambiarEstado(ListaTipoFund, IStodosFund);
            }
        }
        private void cambiarEstado(ObservableCollection<PorTiposNh> lista, bool estado)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].IsVisible = estado;
            }
        }

        private void msje(object sender, RoutedEventArgs e)
        {
          
            MessageBox.Show("", myGridSize);
        }
    }
}
