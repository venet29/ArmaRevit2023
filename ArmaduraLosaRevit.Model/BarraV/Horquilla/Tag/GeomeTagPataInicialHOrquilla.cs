using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.BarraV.TipoTagH;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Tag
{
    public class GeomeTagPataInicialHOrquilla : GeomeTagBaseH, IGeometriaTag
    {
        private readonly IntervaloBarrasDTO itemIntervaloBarrasDTO;
        private List<double> listaZ_foot;

        public GeomeTagPataInicialHOrquilla(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO) :
            base(uiapp, itemIntervaloBarrasDTO.ptoini, itemIntervaloBarrasDTO.ptoPosicionTAg, itemIntervaloBarrasDTO.ptofinal)
        {
            this.itemIntervaloBarrasDTO = itemIntervaloBarrasDTO;
            listaZ_foot = itemIntervaloBarrasDTO._intervaloBarras_HorqDTO.ListaZbarras_menosInicial_foot;
        }

        public GeomeTagPataInicialHOrquilla() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {

                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                // M2_CAlcularPtosDeTAg();
                M2_1_ReCAlcularPtosDeTAg();
                M3_DefinirRebarShape();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        public override bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {

            try
            {
                    _direccionBarra = (_p2 - _p1).Normalize();
                    _anguloBarraRad = Util.angulo_entre_pt_Rad_XY0(_p1, _p2);
                    _largoMedioEnFoot = _p1.DistanceTo(_p2);

                    listaTag = new List<TagBarra>();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtnerPtosInicialYFinalDeBarra' en { this.GetType().Name}.   ex:{ex.Message} ");
                return false;
            }
            return true;
        }
        private void M2_1_ReCAlcularPtosDeTAg()
        {
            double factor50 = 0.54;
            double factorCAbeza = 0.54;
            if (_view.Scale == 75)
                factorCAbeza = 0.575;
            if (_view.Scale == 100)
                factorCAbeza = 0.61;

            XYZ p0_cabezaTag = _p1 - _direccionBarra * Util.CmToFoot(100)+ new XYZ(0, 0, -1.19- factorCAbeza);
            XYZ p0_LeaderElbow = _p1 + _direccionBarra *( Util.CmToFoot(50)+_largoMedioEnFoot * 0.25 )+ new XYZ(0, 0, -1.1- factor50);// new XYZ(0, 0, _largoMedioEnFoot * 0.25);
            XYZ p0_LeaderElbow_sin = _p1 + _direccionBarra * (Util.CmToFoot(50) + _largoMedioEnFoot * 0.25) + new XYZ(0, 0, -1.1);// new XYZ(0, 0, _largoMedioEnFoot * 0.25);


            TagP0_F = M2_1_ObtenerTAgBarra(p0_cabezaTag, "F", nombreDefamiliaBase + "_FHorq_" + escala, escala);
            TagP0_F.IsDIrectriz=true;
            TagP0_F.PtoCodo_LeaderElbow = p0_LeaderElbow.ObtenerCopia();
            listaTag.Add(TagP0_F);

            //crea los tag sin texto
            TagBarra TagP0_sin1_frre = null;
            foreach (var item in listaZ_foot)
            {           
                TagP0_sin1_frre = M2_1_ObtenerTAgBarra(p0_LeaderElbow_sin, "sin1", nombreDefamiliaBase + "_SIN_" + escala, escala);
                TagP0_sin1_frre.PtoCodo_LeaderElbow = p0_LeaderElbow_sin;
                TagP0_sin1_frre.Ptocodo_LeaderEnd = p0_LeaderElbow_sin.AsignarZ(item);
                TagP0_sin1_frre.IsLibre = true;
                TagP0_sin1_frre.IsDIrectriz = true;

                listaTag.Add(TagP0_sin1_frre);
            }





        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagPataInicialHOrquilla> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseH _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            //_geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
