using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagS3 : GeomeTagBaseSX, IGeometriaTag
    {
        public GeomeTagS3(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {
        }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);


        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBaseSX _geomeTagBase)
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
    }
}
