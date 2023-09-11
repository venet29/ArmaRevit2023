using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF1 : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF1()
        {
        }

        public GeomeTagF1(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
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
                Util.ErrorMsg($"Error ejecutar TagF1  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            if (_geomeTagBase._ubicacionEnlosa == UbicacionLosa.Inferior || _geomeTagBase._ubicacionEnlosa == UbicacionLosa.Izquierda)
            {
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }
            else
            {
                _geomeTagBase.TagP0_A.IsOk = false;
                _geomeTagBase.TagP0_B.IsOk = false;

                _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_B);
                _geomeTagBase.TagP0_E.CAmbiar(_geomeTagBase.TagP0_A);
            }

        }


    }


}
