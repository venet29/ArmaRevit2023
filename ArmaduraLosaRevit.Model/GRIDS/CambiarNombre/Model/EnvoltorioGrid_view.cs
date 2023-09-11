using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.model;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Model
{
    public enum TipoGrid
    {
        Horizonal,
        Vertical,
        Otros

    }

    public class EnvoltorioGrid_view : INotifyPropertyChanged
    {


        public bool IsSelected { get; set; }
        public string Nombre_Actual { get; set; }
        // public string Nombre_Nuevo { get; set; }


        public TipoGrid TipoGrid_ { get; set; }

        public double CoordeParaOrden { get; set; }
        /*
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _nombre_Actual;
        public string Nombre_Actual
        {
            get { return _nombre_Actual; }
            set
            {
                if (_nombre_Actual != value)
                {
                    _nombre_Actual = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _nombre_Nuevo;
        public string Nombre_Nuevo
        {
            get { return _nombre_Nuevo; }
            set
            {
                if (_nombre_Nuevo != value)
                {
                    _nombre_Nuevo = value;
                    OnPropertyChanged();
                }
            }
        }
       */
        private string _nombre_Nuevo;
        public string Nombre_Nuevo
        {
            get { return _nombre_Nuevo; }
            set
            {
                if (_nombre_Nuevo != value)
                {
                    string previoCAmbio = _nombre_Nuevo;
                    if (value == "")
                    {
                        Util.ErrorMsg($"Grid :'{Nombre_Actual}' no puede tener nombre vacio ''");
                        return;
                    }
                    _nombre_Nuevo = value;

                    if (_nombre_Nuevo != Nombre_Actual)
                        IsCambiarColor = true;
                    else
                        IsCambiarColor = false;

                    if (previoCAmbio != null && _nombre_Nuevo != null && ListaGridAsociadosObs != null)
                    {
                        //for (int i = 0; i < ListaGridAsociadosObs.Count; i++)
                        //{
                        //    ListaGridAsociadosObs[i].Nombre_Nuevo
                        //}
                        ListaGridAsociadosObs.ForEach(ob => { ob.Nombre_Nuevo = ob.Nombre_Nuevo.Replace(" "+previoCAmbio , " " + _nombre_Nuevo); });
                    }
                    OnPropertyChanged();
                }
            }
        }
        private bool _isCambiarColor;
        public bool IsCambiarColor
        {
            get { return _isCambiarColor; }
            set
            {
                if (_isCambiarColor != value)
                {

                    _isCambiarColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAnalizado { get; set; }// se utiliza al momento de cmabiar los nombre para ver si ya se analizo
        public EnvoltorioGrid Grid_ { get; set; }

        public ObservableCollection<EnvoltorioViewAsosciadoGrid> ListaGridAsociadosObs { get; set; }
        public List<EnvoltorioViewAsosciadoGrid> ListaGridAsociados { get; set; }

        public EnvoltorioGrid_view(EnvoltorioGrid view)
        {
            this.Grid_ = view;
            this.Nombre_Actual = view.Nombre;
            this.Nombre_Nuevo = view.Nombre;
            ListaGridAsociados = new List<EnvoltorioViewAsosciadoGrid>();
            this.IsSelected = true;

        }

        public void DireccionarGrid()
        {

            XYZ[] lista = Util.Ordena2Ptos(Grid_.p1, Grid_.p2);

            var valor = Util.AnguloEntre2PtosGrado90(lista[0], lista[1],true);

            if (Util.IsSimilarValor(valor, 0, 1))
            {
                TipoGrid_ = TipoGrid.Horizonal;
                CoordeParaOrden = lista[0].Y;
            }
            else if (Util.IsSimilarValor(valor, 90, 1))
            {
                TipoGrid_ = TipoGrid.Vertical;
                CoordeParaOrden = lista[0].X;
            }
            else
            {
                TipoGrid_ = TipoGrid.Otros;
                CoordeParaOrden = (lista[0].X + lista[1].X) / 2;
            }

        }

        public void AgregraView(ViewGeom viewGeom)
        {
            if (ListaGridAsociados.Exists(c => c.Nombre_Actual == viewGeom.viewDTO_.Nombre)) return;

            EnvoltorioViewAsosciadoGrid _EnvoltorioViewAsosciadoGrid = new EnvoltorioViewAsosciadoGrid(viewGeom);
            ListaGridAsociados.Add(_EnvoltorioViewAsosciadoGrid);
            ListaGridAsociadosObs = new ObservableCollection<EnvoltorioViewAsosciadoGrid>(ListaGridAsociados);
        }

        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
