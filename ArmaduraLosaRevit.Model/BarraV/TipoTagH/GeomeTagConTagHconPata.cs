using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTagH
{
    public class GeomeTagConTagHconPata : GeomeTagBaseH, IGeometriaTag
    {
        private XYZ _desplaminetoDirectriz;

        public GeomeTagConTagHconPata(UIApplication uiapp, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(uiapp, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagConTagHconPata() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                _desplaminetoDirectriz= args.DesplaminetoDirectriz_soloRefuerzoVIga;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagPataFinalH  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape()
        {
            XYZ p0_F_SIN = CentroBarra+ _desplaminetoDirectriz;
            TagBarra TagP0_F_SIN = M2_1_ObtenerTAgBarra(p0_F_SIN, "F", nombreDefamiliaBase + "_F_" + escala, escala);
            listaTag.Add(TagP0_F_SIN);

            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagConTagHconPata> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseH _geomeTagBase)
        {

            _geomeTagBase.TagP0_F.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_L.IsOk = false;

        }
    }
}
