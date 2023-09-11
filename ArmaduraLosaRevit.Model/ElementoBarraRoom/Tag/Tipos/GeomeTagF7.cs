using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF7 : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF7(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {
        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF7  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        public void M3_DefinirRebarShape()
        {

            if (_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Izquierda)
            {
                TagP0_E.IsOk = false;
            }
            else
            {
                TagP0_A.IsOk = false;
                TagP0_E.CAmbiar(TagP0_A);
            }
        }

        public bool M4_IsFAmiliaValida() => true;
    }
}
