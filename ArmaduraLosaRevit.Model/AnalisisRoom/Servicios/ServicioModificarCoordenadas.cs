using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Servicios
{
   public class ServicioModificarCoordenadas
    {


        public static XYZ ObtenerPtoReferenciaLines(XYZ lineaP1, XYZ lineaP2, XYZ ptoREfe)
        {
            XYZ result = null;


            XYZ[] valoresOrdenados = Util.Ordena2Ptos(lineaP1, lineaP2);
            XYZ pto1 = valoresOrdenados[0];
            XYZ pto2 = valoresOrdenados[1];

          

            Transform trans1 = null;
            Transform Invertrans1 = null;
            Transform trans2_rotacion = null;
            Transform InverTrans2_rotacion = null;
            double anguloBarra_ = Util.AnguloEntre2PtosGrado90(pto1, pto2, EnGrados: true);

            trans1 = Transform.CreateTranslation(-pto1);
            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(anguloBarra_), XYZ.Zero);

            Invertrans1 = trans1.Inverse;
            InverTrans2_rotacion = trans2_rotacion.Inverse;
            //trans1.Origin = listaPtos[3];
            result = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(ptoREfe));
    

            return result;


        }
    }
}
