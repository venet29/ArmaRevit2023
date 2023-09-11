
using ArmaduraLosaRevit.Model.modeloNH;

using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
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
using DBRevit = Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component
{
    //https://docs.microsoft.com/en-us/answers/questions/489692/tutorial-how-to-access-click-events-from-usercontr.html

    /// <summary>
    /// Interaction logic for Diametros.xaml
    /// </summary>
    public partial class Diametros : UserControl, INotifyPropertyChanged
    {
        //  public TiposDiametros Diametro_8 { get; set; }

        public ObservableCollection<TiposDiametrosNh> _listaTiposDiametros;
        public ObservableCollection<TiposDiametrosNh> ListaTiposDiametros
        {
            get { return _listaTiposDiametros; }
            set
            {
                _listaTiposDiametros = value;
                RaisePropertyChanged("ListaTiposDiametros");
            }
        }
        bool IStodosDiam;

        public Diametros()
        {
            InitializeComponent();
            this.DataContext = this;
            Load();
        }
  
        private void Load()
        {
            IStodosDiam = true;
            ListaTiposDiametros = new ObservableCollection<TiposDiametrosNh>(TiposDiametrosNh.ObtenerDatosTiposDiametros());
        }

        public string BotonOprimido { get; private set; }

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click1", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Diametros));
        // public static readonly RoutedEvent ClickEvent1 = ButtonBase.ClickEvent.AddOwner(typeof(Diametros));


        public event RoutedEventHandler Click1
        {
            add { btn_Diamtros.AddHandler(ClickEvent, value); }
            remove { btn_Diamtros.RemoveHandler(ClickEvent, value); }
        }


        //**********************************************************************************************************
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void Btn_Diamtros_Click(object sender, RoutedEventArgs e)
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
            IStodosDiam = !IStodosDiam;
            for (int i = 0; i < ListaTiposDiametros.Count; i++)
            {
                ListaTiposDiametros[i].IsVisible = IStodosDiam;
            }

        }
    }
}
