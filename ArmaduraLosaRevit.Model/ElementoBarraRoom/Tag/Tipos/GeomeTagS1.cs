using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagS1 : GeomeTagBaseSX, IGeometriaTag
    {
        public GeomeTagS1(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
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
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagS1  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);


        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBaseSX _geomeTagBase)
        {
        }
    }
}
