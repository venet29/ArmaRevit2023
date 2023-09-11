using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos
{
    public class RecalcularPtosYEspaciamieto_Horquilla
    {
        public XYZ PtoInicial_Original { get; private set; }
        public XYZ PtoFinal_Original { get; private set; }
        public double _CantidadOriginal { get; private set; }

        public XYZ PtoInicial_Corregido { get; private set; }
        public XYZ PtoFinal_Corregido { get; private set; }
        public double EspaciamientoCorregidoCM { get; private set; }

        public List<double> ListaZbarras_menosInicial_foot;
        public DireccionSeleccionMouse DireccionSeleccionMouse_ { get; set; }
        public XYZ Direccion { get; set; }
        public IntervaloBarras_HorqDTO IntervaloBarras_HorqDTO_ { get; set; }
        public RecalcularPtosYEspaciamieto_Horquilla(XYZ PtoInicial_Original_, XYZ PtoFinal_Original_, double CantidadOriginal_)
        {
            this.PtoInicial_Original = PtoInicial_Original_;
            this.PtoFinal_Original = PtoFinal_Original_;
            this._CantidadOriginal = CantidadOriginal_;
            this.ListaZbarras_menosInicial_foot = new List<double>();
            this.IntervaloBarras_HorqDTO_ = new IntervaloBarras_HorqDTO();
        }



        public bool RecalcularPtosYCalcularEspaciamiento(DatosBarraElevacionHorquilla _datosBarraElevacionHorquilla)
        {

            try
            {
                double deltaZ = Math.Abs(PtoFinal_Original.Z - PtoInicial_Original.Z);

                double EspaciamientoCorregidoFoot = deltaZ / (_CantidadOriginal + 1);
                EspaciamientoCorregidoCM = Util.FootToCm(EspaciamientoCorregidoFoot);

                if (PtoFinal_Original.Z > PtoInicial_Original.Z)
                {
                    PtoFinal_Corregido = PtoFinal_Original + new XYZ(0, 0, -EspaciamientoCorregidoFoot);
                    PtoInicial_Corregido = PtoInicial_Original + new XYZ(0, 0, EspaciamientoCorregidoFoot);
                }
                else
                {
                    PtoInicial_Corregido = PtoInicial_Original + new XYZ(0, 0, -EspaciamientoCorregidoFoot);
                    PtoFinal_Corregido = PtoFinal_Original + new XYZ(0, 0, EspaciamientoCorregidoFoot);
                }

                ////crea los las coordenadas de las pelatas  de los tag sin texto
                double Zaux = PtoInicial_Corregido.Z;
                for (int i = 1; i < _CantidadOriginal ; i++)
                {
                    Zaux = Zaux +   EspaciamientoCorregidoFoot;
                    ListaZbarras_menosInicial_foot.Add(Zaux);
                }
                IntervaloBarras_HorqDTO_.ListaZbarras_menosInicial_foot = ListaZbarras_menosInicial_foot;
                IntervaloBarras_HorqDTO_.LargoPataBarra= _datosBarraElevacionHorquilla.LargoPata;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al recalcular valorea  ex: {ex.Message}");
                return false;
            }
            return true;
        }



        public bool CalcularDireccionSeleccion(View _view,int diamMM)
        {
            try
            {
                 XYZ ini= _view.NH_ObtenerPtoSObreVIew(PtoInicial_Original);
                 XYZ fin = _view.NH_ObtenerPtoSObreVIew(PtoFinal_Original);
                 XYZ direc = fin.GetXY0() - ini.GetXY0();

                if (Util.GetProductoEscalar(direc, _view.RightDirection) > 0)
                {
                    DireccionSeleccionMouse_ = DireccionSeleccionMouse.IzqToDere;
                    PtoFinal_Corregido = PtoFinal_Corregido + _view.RightDirection* UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamMM);
                }
                else
                {
                    DireccionSeleccionMouse_ = DireccionSeleccionMouse.DereToIzq;
                    PtoFinal_Corregido = PtoFinal_Corregido - _view.RightDirection * UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamMM);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error     ex: {ex.Message}");
                return false;
            }
            return true;
        }

    }
}
