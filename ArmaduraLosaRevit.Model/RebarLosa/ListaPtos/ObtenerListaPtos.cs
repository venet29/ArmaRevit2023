using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Viewrebar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.ListaPtos
{
    public class ListaPtoTramoResultadoDTo
    {

        public List<XYZ> _listaptoTram { get; set; }
        public bool IsOk { get; set; }
    }

    public  class ObtenerListaPtos
    {


        public static ListaPtoTramoResultadoDTo M2_ListaPtoTramoPorIntervalos(List<XYZ> listaPtosPerimetroBarras, int diametroMM, XYZ _ptoMouse)
        {
            if (listaPtosPerimetroBarras.Count == 4)
                return M2_ListaPtoTramoPorIntervalos4puntos(listaPtosPerimetroBarras, diametroMM, _ptoMouse);
            else if (listaPtosPerimetroBarras.Count == 2)
                return M2_ListaPtoTramoPorIntervalos2puntos(listaPtosPerimetroBarras, diametroMM, _ptoMouse);
            else
                return new ListaPtoTramoResultadoDTo() { IsOk = false };
        }

        private static ListaPtoTramoResultadoDTo M2_ListaPtoTramoPorIntervalos2puntos(List<XYZ> listaPtosPerimetroBarras, int diametroMM, XYZ _ptoMouse)
        {
            List<XYZ> _listaptoTram = new List<XYZ>();
            try
            {
                double Largobarra_Foot = listaPtosPerimetroBarras[0].DistanceTo(listaPtosPerimetroBarras[1]);
                double Largobarra = Math.Round(Util.FootToMetre(listaPtosPerimetroBarras[0].DistanceTo(listaPtosPerimetroBarras[1])), 2);
                double AnchoRecorrido = 0;

                AgregarIntervalos _AgregarIntervalos = new AgregarIntervalos(Largobarra, AnchoRecorrido, diametroMM);
                _AgregarIntervalos.ShowDialog();

                if (_AgregarIntervalos.Isok == false) return new ListaPtoTramoResultadoDTo() { IsOk = false };

                var listaIntervalos = _AgregarIntervalos.ListaIntervalos;

                double largoTraslapo = UtilBarras.largo_traslapoFoot_diamMM(diametroMM);
                XYZ pto3_2mouse = listaPtosPerimetroBarras[0];
                XYZ pto1_0_mouse_inicial = listaPtosPerimetroBarras[0];

                XYZ direccion_2_1 = (listaPtosPerimetroBarras[1] - listaPtosPerimetroBarras[0]).Normalize();

                int factor = 2;
                foreach (double largoTramos in listaIntervalos)
                {

                    pto3_2mouse = pto3_2mouse + direccion_2_1 * Util.CmToFoot(largoTramos * 100) - direccion_2_1 * largoTraslapo / factor;

                    if (pto1_0_mouse_inicial.DistanceTo(pto3_2mouse) + Util.CmToFoot(100) < Largobarra_Foot) // se agregan 100cm para que el utlimo intervalo seade alo menos 100cm
                        _listaptoTram.Add(pto3_2mouse);

                    factor = 1;
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return new ListaPtoTramoResultadoDTo() { IsOk = false };
            }

            return new ListaPtoTramoResultadoDTo() { IsOk = true, _listaptoTram = _listaptoTram };
        }

        private static ListaPtoTramoResultadoDTo M2_ListaPtoTramoPorIntervalos4puntos(List<XYZ> listaPtosPerimetroBarras, int diametroMM, XYZ _ptoMouse)
        {
            List<XYZ> _listaptoTram = new List<XYZ>();
            try
            {
                double Largobarra_Foot = listaPtosPerimetroBarras[1].DistanceTo(listaPtosPerimetroBarras[2]);
                double Largobarra = Math.Round(Util.FootToMetre(listaPtosPerimetroBarras[1].DistanceTo(listaPtosPerimetroBarras[2])), 2);
                double AnchoRecorrido = Math.Round(Util.FootToMetre(listaPtosPerimetroBarras[3].DistanceTo(listaPtosPerimetroBarras[2])), 2);

                AgregarIntervalos _AgregarIntervalos = new AgregarIntervalos(Largobarra, AnchoRecorrido, diametroMM);
                _AgregarIntervalos.ShowDialog();

                if (_AgregarIntervalos.Isok == false) return new ListaPtoTramoResultadoDTo() { IsOk = false };

                var listaIntervalos = _AgregarIntervalos.ListaIntervalos;

                double largoTraslapo = UtilBarras.largo_traslapoFoot_diamMM(diametroMM);
                XYZ pto3_2mouse = Line.CreateBound(listaPtosPerimetroBarras[0], listaPtosPerimetroBarras[1]).ProjectSINExtendida3D(_ptoMouse);
                XYZ pto1_0_mouse_inicial = Line.CreateBound(listaPtosPerimetroBarras[0], listaPtosPerimetroBarras[1]).ProjectSINExtendida3D(_ptoMouse);
                XYZ direccion_2_1 = (listaPtosPerimetroBarras[2] - listaPtosPerimetroBarras[1]).Normalize();

                int factor = 2;
                foreach (double largoTramos in listaIntervalos)
                {

                    pto3_2mouse = pto3_2mouse + direccion_2_1 * Util.CmToFoot(largoTramos * 100) - direccion_2_1 * largoTraslapo/ factor;

                    if(pto1_0_mouse_inicial.DistanceTo(pto3_2mouse)+Util.CmToFoot(100)< Largobarra_Foot) // se agregan 100cm para que el utlimo intervalo seade alo menos 100cm
                        _listaptoTram.Add(pto3_2mouse);

                    factor = 1;
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return new ListaPtoTramoResultadoDTo() { IsOk = false };
            }

            return new ListaPtoTramoResultadoDTo() { IsOk = true, _listaptoTram = _listaptoTram };
        }
    }
}
