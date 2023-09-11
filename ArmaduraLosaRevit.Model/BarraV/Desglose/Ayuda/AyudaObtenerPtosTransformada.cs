using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
    public class AyudaObtenerPtosTransformada
    {

        public static List<PtosCurvaAuxDTO> ObtenerPtosTransformados(List<WraperRebarLargo> listaCuvas, View _view)
        {

            List<PtosCurvaAuxDTO> listPtosCurvaAuxDTO = new List<PtosCurvaAuxDTO>();


           double zincial = listaCuvas[0].ptoInicial.Z;
            var xprom = listaCuvas.Average(c => c.PtoMedioTransformada.X);
            var yprom = listaCuvas.Average(c => c.PtoMedioTransformada.Y);

            XYZ centroDeestribo = new XYZ(xprom, yprom, zincial);

            CrearTrasformadaSobreVectorDesg _Trasform = new CrearTrasformadaSobreVectorDesg(centroDeestribo, -90, _view.RightDirection);

            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];

                PtosCurvaAuxDTO _NewPtosCurvaAuxDTO = new PtosCurvaAuxDTO()
                {
                    PtoInicial = item.PtoInicialTransformada.AsignarZ(zincial),
                    PtoMedio = item.PtoMedioTransformada.AsignarZ(zincial),
                    PtoFinal = item.PtoFinalTransformada.AsignarZ(zincial),
                    PtoInicialTransformada = _Trasform.EjecutarTransform(item.PtoInicialTransformada.AsignarZ(zincial)),
                    PtoMedioTransformada = _Trasform.EjecutarTransform(item.PtoMedioTransformada.AsignarZ(zincial)),
                    PtoFinalTransformada = _Trasform.EjecutarTransform(item.PtoFinalTransformada.AsignarZ(zincial)),
                    largoCurve = item._curve.Length
                };
                listPtosCurvaAuxDTO.Add(_NewPtosCurvaAuxDTO);
            }

            return listPtosCurvaAuxDTO;
        }


        public static List<PtosCurvaAuxDTO> ObtenerPtosSINTransformados(List<WraperRebarLargo> listaCuvas, View _view)
        {

            List<PtosCurvaAuxDTO> listPtosCurvaAuxDTO = new List<PtosCurvaAuxDTO>();


            double zincial = listaCuvas[0].ptoInicial.Z;
            var xprom = listaCuvas.Average(c => c.PtoMedioTransformada.X);
            var yprom = listaCuvas.Average(c => c.PtoMedioTransformada.Y);

            XYZ centroDeestribo = new XYZ(xprom, yprom, zincial);

            //nota agregar metodo para obtener angulo cara de corte y ejez
            double angleRADNormalHostYEJeZ = 0;// Math.PI / 2 - curvaPrinciplar.direccion.GetAngleEnZ_respectoPlanoXY(false);

            CrearTrasformadaSobreVectorDesg _Trasform = new CrearTrasformadaSobreVectorDesg(centroDeestribo, -angleRADNormalHostYEJeZ, _view.RightDirection);


             double anguloXY = _view.RightDirection.GetAngleXY0(false);
            //para dejar todos sobre eje X
            CrearTrasformadaSobreVectorDesg _TrasformSobreZ = new CrearTrasformadaSobreVectorDesg(new XYZ(0,0,0), -anguloXY, new XYZ(0,0,1));

            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];

                PtosCurvaAuxDTO _NewPtosCurvaAuxDTO = new PtosCurvaAuxDTO()
                {
                    PtoInicial = _view.NH_ObtenerPtoSObreVIew(item.PtoInicialTransformada),
                    PtoMedio = _view.NH_ObtenerPtoSObreVIew(item.PtoMedioTransformada),
                    PtoFinal = _view.NH_ObtenerPtoSObreVIew(item.PtoFinalTransformada),
                    PtoInicialTransformada = _TrasformSobreZ.EjecutarTransform(  _Trasform.EjecutarTransform(item.PtoInicialTransformada)),
                    PtoMedioTransformada = _TrasformSobreZ.EjecutarTransform(_Trasform.EjecutarTransform(item.PtoMedioTransformada)),
                    PtoFinalTransformada = _TrasformSobreZ.EjecutarTransform(_Trasform.EjecutarTransform(item.PtoFinalTransformada)),
                    largoCurve = item._curve.Length
                };
                listPtosCurvaAuxDTO.Add(_NewPtosCurvaAuxDTO);
            }

            return listPtosCurvaAuxDTO;
        }
    }
}
