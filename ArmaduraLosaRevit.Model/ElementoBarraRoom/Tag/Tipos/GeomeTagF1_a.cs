using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF1_a : GeomeTagBase, IGeometriaTag
    {
        private double elevacion;
#pragma warning disable CS0108 // 'GeomeTagF1_a._rebarInferiorDTO1' hides inherited member 'GeomeTagBase._rebarInferiorDTO1'. Use the new keyword if hiding was intended.
        private RebarInferiorDTO _rebarInferiorDTO1;
#pragma warning restore CS0108 // 'GeomeTagF1_a._rebarInferiorDTO1' hides inherited member 'GeomeTagBase._rebarInferiorDTO1'. Use the new keyword if hiding was intended.


        public GeomeTagF1_a(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
            base(doc, rebarInferiorDTO1)
        {
           // this.elevacion = doc.ActiveView.GenLevel.ProjectElevation;
            this._rebarInferiorDTO1 = rebarInferiorDTO1;
        }


        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                var resultZ = _view.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok) return false;
                elevacion = resultZ.valorz;

                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M2_1_ReCAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF1_a ex:${ex.Message}");
                return false;
            }
            return true;

        }

        private void M2_1_ReCAlcularPtosDeTAg()
        {

            _p1 = _p1.AsignarZ(elevacion);
            _p2 = _p2.AsignarZ(elevacion);

            XYZ _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);

            if (_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Izquierda)
            {
                TagP0_D = AgregarTagPathreinLitsta("D", 20, 3, _p2);
            }
            else
            {
                TagP0_B = AgregarTagPathreinLitsta("B", -20, -10, _p1);
            }

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            if (_geomeTagBase._ubicacionEnlosa == UbicacionLosa.Inferior || _geomeTagBase._ubicacionEnlosa == UbicacionLosa.Izquierda)
            {
                // _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }
            else
            {
                _geomeTagBase.TagP0_A.IsOk = false;
                // _geomeTagBase.TagP0_B.IsOk = false;

                TagBarra auxTagD = _geomeTagBase.TagP0_D.Copiar();

                _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_B);
                _geomeTagBase.TagP0_E.CAmbiar(_geomeTagBase.TagP0_A);

                _geomeTagBase.TagP0_B.CAmbiar(auxTagD);

            }

        }


    }


}
