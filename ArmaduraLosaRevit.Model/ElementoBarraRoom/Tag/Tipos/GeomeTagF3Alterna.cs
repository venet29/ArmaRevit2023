using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF3Alterna : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF3Alterna(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF3Alterna(Document doc, SolicitudBarraDTO _solicitudBarraDTO) : base(doc, _solicitudBarraDTO) { }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                _anguloBarraRad = args.angulorad;
                _anguloBArraGrado = Convert.ToInt32(Math.Round(Util.RadianeToGrados(_anguloBarraRad), 0));


                //  M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                //  M2_CAlcularPtosDeTAg();
                //  M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF3Alterna  ex:${ex.Message}");
                return false;
            }
            return true;


        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3Alterna> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {


            _geomeTagBase.TagP0_L.nombreFamilia = _geomeTagBase.TagP0_L.nombreFamilia.Replace("_L_", "_L2_");
            TagP0_L2 = M1_1_ObtenerTAgPathBarra(_geomeTagBase.TagP0_L.posicion, "L2", _geomeTagBase.TagP0_L.nombreFamilia, 50);
            _geomeTagBase.TagP0_L.CAmbiar(TagP0_L2);
            _geomeTagBase.TagP0_L.IsOk = true;

            _geomeTagBase.TagP0_F.nombreFamilia = _geomeTagBase.TagP0_F.nombreFamilia.Replace("_F_", "_F2_");
            TagP0_F2 = M1_1_ObtenerTAgPathBarra(_geomeTagBase.TagP0_F.posicion, "F2", _geomeTagBase.TagP0_F.nombreFamilia, 50);
            _geomeTagBase.TagP0_F.CAmbiar(TagP0_F2);
            _geomeTagBase.TagP0_F.IsOk = true;


            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_B.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
