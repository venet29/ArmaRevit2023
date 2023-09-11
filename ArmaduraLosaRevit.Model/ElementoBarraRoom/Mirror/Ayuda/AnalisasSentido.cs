using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror.Ayuda
{
     public class AnalisasSentido
    {
        private CoordenadaPath _CoordenadaPath;
        private Line _EjeReferencia;
        public SentidoMirror SentidoEje { get; internal set; }
        public AnalisasSentido()
        {

        }

        public AnalisasSentido(CoordenadaPath coordenadaPath, Line ejeReferencia)
        {
            _CoordenadaPath = coordenadaPath;
            _EjeReferencia = ejeReferencia;
        }



        public void M1_ejecutar()
        {
            XYZ VectoparaleloBArras = (_CoordenadaPath.p3 - _CoordenadaPath.p2).Normalize();
            XYZ VectoPerpenBArras = (_CoordenadaPath.p4 - _CoordenadaPath.p3).Normalize();

            if (Math.Abs(Util.GetProductoEscalar(VectoPerpenBArras, _EjeReferencia.Direction.Normalize())) > 0.98)
            {
                SentidoEje = SentidoMirror.sentidoPerpendicularBarra;
            }
            else if (Math.Abs(Util.GetProductoEscalar(VectoparaleloBArras, _EjeReferencia.Direction.Normalize())) > 0.98)
            {
                SentidoEje = SentidoMirror.sentidoParaleloBarra;
            }
            else {
                SentidoEje = SentidoMirror.sentidoDiagonalBarra;
            }

                
        }
    }
}
