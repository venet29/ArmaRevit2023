using ArmaduraLosaRevit.Model.Visibilidad.Entidades;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArmaduraLosaRevit.Model.modeloNH
{
    public class TiposDiametrosNh : INotifyPropertyChanged
    {

        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }


        private string _nombreFiltro;
        public string NombreFiltro
        {
            get { return _nombreFiltro; }
            set
            {
                _nombreFiltro = value;
                NotifyPropertyChanged("NombreFiltro");
            }
        }


        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
      
                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private SolidColorBrush _colorBrush;
        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                _colorBrush = value;
                NotifyPropertyChanged("ColorBrush");
            }
        }

        private int _diam;
        public int Diam
        {
            get { return _diam; }
            set
            {
 
                _diam = value;
                NotifyPropertyChanged("Diam");
            }
        }

        public SolidColorBrush ColorBrash { get; private set; }

        public TiposDiametrosNh()
        {
        }
        public TiposDiametrosNh(int diam)
        {
            Nombre = "Ø"+diam.ToString();
            NombreFiltro = "Diam" + diam;
            Color = FactoryColores.ObtenerColoresPorDiametro_wpf(diam);
            Diam = diam;
            ColorBrush = new SolidColorBrush(Color);
            IsVisible = true;
        }


        public static List<TiposDiametrosNh> ObtenerDatosTiposDiametros()
        {

            System.Collections.Generic.List<TiposDiametrosNh> ListaObs = new System.Collections.Generic.List<TiposDiametrosNh>();
            ListaObs.Add(new TiposDiametrosNh(8));
            ListaObs.Add(new TiposDiametrosNh(10));
            ListaObs.Add(new TiposDiametrosNh(12));
            ListaObs.Add(new TiposDiametrosNh(16));
            ListaObs.Add(new TiposDiametrosNh(18));
            ListaObs.Add(new TiposDiametrosNh(22));
            ListaObs.Add(new TiposDiametrosNh(25));
            ListaObs.Add(new TiposDiametrosNh(28));
            ListaObs.Add(new TiposDiametrosNh(32));
            ListaObs.Add(new TiposDiametrosNh(36));

            return ListaObs;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }


}
