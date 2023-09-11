using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.Geom
{
    public class FactoryGenerarGeometriaAhorro
    {
        private static XYZ _p1;
        private static XYZ _p2;
        private static XYZ _p3;
        private static XYZ _p4;
        private static XYZ _dir_p1_p2;
        private static XYZ _dir_p2_p3;
#pragma warning disable CS0169 // The field 'FactoryGenerarGeometriaAhorro.largo_L3' is never used
        private double largo_L3;
#pragma warning restore CS0169 // The field 'FactoryGenerarGeometriaAhorro.largo_L3' is never used

        public static List<XYZ> ObtenerPrimerGeoPath(List<XYZ>  ListaPtosPerimetroBarras_, string _TipoBarra)
        {
            _p1 = ListaPtosPerimetroBarras_[0];
            _p2 = ListaPtosPerimetroBarras_[1];
            _p3 = ListaPtosPerimetroBarras_[2];
            _p4 = ListaPtosPerimetroBarras_[3];
            _dir_p1_p2 = (_p1 - _p2).Normalize();
            _dir_p2_p3 = (_p2 - _p3).Normalize();

            List<XYZ> listaPtos = new List<XYZ>();
            switch (EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _TipoBarra))
            {
                case TipoBarra.f1:
                    { 


                        //       ListaPtosPerimetroBarras1.Add(_p1);
                        //ListaPtosPerimetroBarras1.Add(_p2_1);
                        //ListaPtosPerimetroBarras1.Add(_p3_1);
                        //ListaPtosPerimetroBarras1.Add(_p4_1);
                        break;
                    }
                case TipoBarra.f3:
                    break;


            }


            return listaPtos;
        }

    }
}
