using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF11 : GeomeTagBase, IGeometriaTag
    {

        public GeomeTagF11(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF11() { }

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
                Util.ErrorMsg($"Error ejecutar TagF11  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);


        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {

            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;

            _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_C);
            _geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_B);
            _geomeTagBase.TagP0_B.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }


}
