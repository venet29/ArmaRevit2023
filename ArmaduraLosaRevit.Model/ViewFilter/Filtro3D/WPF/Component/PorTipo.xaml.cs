using ArmaduraLosaRevit.Model.modeloNH;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component
{
    /// <summary>
    /// Interaction logic for PorTipo.xaml
    /// </summary>
    public partial class PorTipo : UserControl, INotifyPropertyChanged
    {
        //1
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


        bool IStodosElev;
        bool IStodosLosa;
        bool IStodosFund;
        public PorTipo()
        {
            InitializeComponent();
            this.DataContext = this;
            Load();
        }

        private void Load()
        {
            var lst1 = PorTiposNh.ObtenerTiposELev();
            ListaTipoElev = new ObservableCollection<PorTiposNh>(lst1);
            var lst2 = PorTiposNh.ObtenerTiposLosa();
            ListaTipoLosa = new ObservableCollection<PorTiposNh>(lst2);
            var lst3 = PorTiposNh.ObtenerTiposFund();
            ListaTipoFund = new ObservableCollection<PorTiposNh>(lst3);

            List<PorTiposNh> ListaTodos = new List<PorTiposNh>();
            ListaTodos.AddRange(lst1);
            ListaTodos.AddRange(lst2);
            ListaTodos.AddRange(lst3);
            ListaTipoTodos = new ObservableCollection<PorTiposNh>(ListaTodos);

            IStodosElev = true;
            IStodosLosa = true;
            IStodosFund = true;
        }

        public string BotonOprimido { get; private set; }

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click1", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PorTipo));
        // public static readonly RoutedEvent ClickEvent1 = ButtonBase.ClickEvent.AddOwner(typeof(Diametros));

        // public event RoutedEventHandler Click1;
        public event RoutedEventHandler Click1
        {
            add { btn_Tipo_.AddHandler(ClickEvent, value); }
            remove { btn_Tipo_.RemoveHandler(ClickEvent, value); }
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
                IStodosLosa = !IStodosLosa ;
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
    }
}
