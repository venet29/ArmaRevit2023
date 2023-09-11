using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    class AyudaREdondearRecorrdioPath
    {
        private readonly DatosNuevaBarraDTO datosNuevaBarraDTO;
        private  List<XYZ> ListaPtosPerimetroBarras;
     //   private readonly XYZ _ptoConMouseEnlosa1;
        public XYZ PtoConMouseEnlosa1 { get; set; }

        public AyudaREdondearRecorrdioPath(DatosNuevaBarraDTO datosNuevaBarraDTO,
                                            List<XYZ> _ListaPtosPerimetroBarras,
                                            XYZ ptoConMouseEnlosa1)
        {
            this.datosNuevaBarraDTO = datosNuevaBarraDTO;
            this.ListaPtosPerimetroBarras = _ListaPtosPerimetroBarras;
            this.PtoConMouseEnlosa1 = ptoConMouseEnlosa1;
        }

        private IList<Curve> RedondearRecorrido(List<Curve> curvesPathreiforment_)
        {

            if (DatosDiseño.IS_PATHREIN_AJUSTADO == false) return curvesPathreiforment_;

            XYZ p1 = curvesPathreiforment_[0].GetPoint2(0);
            XYZ p2 = curvesPathreiforment_[0].GetPoint2(1);
            XYZ Vector = (p2 - p1).Normalize();


            double cantidadBArras = Math.Round((datosNuevaBarraDTO.LargoRecorridoFoot) / datosNuevaBarraDTO.EspaciamientoFoot, 5);

            if (cantidadBArras < 1) return curvesPathreiforment_;

            double parteDecimal = Util.ParteDecimal(cantidadBArras);

            long CantidadBarra = (long)cantidadBArras;

            if (parteDecimal < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) CantidadBarra -= 1;

            if (CantidadBarra < 1)
                CantidadBarra = 1;

            double largoREcorridoborrar = datosNuevaBarraDTO.LargoRecorridoFoot - (CantidadBarra * datosNuevaBarraDTO.EspaciamientoFoot + Util.MmToFoot(datosNuevaBarraDTO.DiametroMM));

            p1 = p1 + Vector * largoREcorridoborrar / 2;
            p2 = p2 - Vector * largoREcorridoborrar / 2;

            MoverPtoTagSicorresponde(Vector, largoREcorridoborrar);

            Line nuevaline1 = Line.CreateBound(p1, p2);

            return new List<Curve>() { nuevaline1 };
        }

        private void MoverPtoTagSicorresponde(XYZ Vector, double largoREcorridoborrar)
        {
            List<XYZ> ListaPtosPerimetroBarrasCorregida = new List<XYZ>();
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[0] + Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[1] - Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[2] - Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[3] + Vector * largoREcorridoborrar / 2);
            List<List<XYZ>> listInLiits = new List<List<XYZ>>();
            listInLiits.Add(ListaPtosPerimetroBarrasCorregida);
            if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(PtoConMouseEnlosa1, listInLiits) == false)
            {
                XYZ direccionHAciaCenrtol = (ListaPtosPerimetroBarrasCorregida[1] - ListaPtosPerimetroBarrasCorregida[0]).Normalize();
                double distancia = ListaPtosPerimetroBarrasCorregida[0].DistanceTo(ListaPtosPerimetroBarrasCorregida[1]) / 2;
                XYZ PtoEnlinea1_4 = Line.CreateBound(ListaPtosPerimetroBarrasCorregida[0].AsignarZ(0), ListaPtosPerimetroBarrasCorregida[3].AsignarZ(0)).ProjectExtendidaXY0(PtoConMouseEnlosa1.AsignarZ(0)).
                                        AsignarZ(PtoConMouseEnlosa1.Z);
                PtoConMouseEnlosa1 = PtoEnlinea1_4 + direccionHAciaCenrtol * distancia;
                //ptoConMouseEnlosa1 =    new XYZ(ListaPtosPerimetroBarrasCorregida.Average(r => r.X),
                //                             ListaPtosPerimetroBarrasCorregida.Average(r => r.Y),
                //                             ListaPtosPerimetroBarrasCorregida.Average(r => r.Z));
               
            }
        }
    }




}
