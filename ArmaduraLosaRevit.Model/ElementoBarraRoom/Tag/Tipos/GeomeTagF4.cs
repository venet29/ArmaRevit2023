using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF4 : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF4(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
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
                Util.ErrorMsg($"Error ejecutar TagF4  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        public void M3_DefinirRebarShape()
        {
        }

        public bool M4_IsFAmiliaValida() => true;

        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF4> rutina)
        {
            rutina(this);
        }
    }
}
