using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF3_refuerzoSuple : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF3_refuerzoSuple(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF3_refuerzoSuple() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();

                if (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Vertical)                
                    M3_DefinirRebarShape();
                else
                    M3_DefinirRebarShapeHorizontal();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

    

        public override bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {            
                if (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Vertical)
                {
                    Curve curvePathIzq = Line.CreateBound(_listaPtosPerimetroBarras[0], _listaPtosPerimetroBarras[3]);
                    _p1 = curvePathIzq.Project(_ptoMouse).XYZPoint;
                    //derecha superior
                    Curve curvePathDERE = Line.CreateBound(_listaPtosPerimetroBarras[1], _listaPtosPerimetroBarras[2]);
                    _p2 = curvePathDERE.Project(_ptoMouse).XYZPoint;
                    _anguloBarraRad = Util.angulo_entre_pt_Rad_XY0(_p1, _p2);
                    _anguloBArraGrado = Convert.ToInt32(Math.Round(Util.RadianeToGrados(_anguloBarraRad), 0));
                    _anguloBArraGrado = 90;
                }
                else
                {
                    Curve curvePathInferior = Line.CreateBound(_listaPtosPerimetroBarras[0], _listaPtosPerimetroBarras[1]);
                    _p1 = curvePathInferior.Project(_ptoMouse).XYZPoint;
                    //derecha superior
                    Curve curvePathSuperior = Line.CreateBound(_listaPtosPerimetroBarras[3], _listaPtosPerimetroBarras[2]);
                    _p2 = curvePathSuperior.Project(_ptoMouse).XYZPoint;
                    _anguloBarraRad = Math.PI/2;
                    _anguloBArraGrado = 0;
                }

         
                _signoAngulo = (_anguloBArraGrado < 0 ? "N" : "");
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
        public void M3_DefinirRebarShape()
        {
            _anguloBArraGrado = 90;
            listaTag.RemoveAll(c => c.nombre == "F");
            //XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-100));
            XYZ p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(-50)) + _view.RightDirection * Util.CmToFoot(-10);
            if (_view.Scale == 75)
                p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(-70)) + _view.RightDirection * Util.CmToFoot(-10);
            else if (_view.Scale == 100)
                p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(-85)) + _view.RightDirection * Util.CmToFoot(-15);

            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F2_RefSupleMuro", nombreDefamiliaBase + "_F_RefSupleMuro_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            TagP0_F.IsDIrectriz = false;
            listaTag.Add(TagP0_F);

            listaTag.RemoveAll(c => c.nombre == "L");
            //XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(100));
            XYZ p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+50)) + _view.RightDirection * Util.CmToFoot(-10);
            if (_view.Scale == 75)
                p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+50)) + _view.RightDirection * Util.CmToFoot(-10);
            else if (_view.Scale == 100)
                p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+60)) + _view.RightDirection * Util.CmToFoot(-15);


            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            TagP0_L.IsDIrectriz = false;
            listaTag.Add(TagP0_L);
            AsignarPArametros(this);
        }

        private void M3_DefinirRebarShapeHorizontal()
        {
            _anguloBArraGrado = 0;
            listaTag.RemoveAll(c => c.nombre == "F");
            //XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-100));
            XYZ p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(4)) + _view.RightDirection * Util.CmToFoot(-55);
            if (_view.Scale == 75)
                p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(4)) + _view.RightDirection * Util.CmToFoot(-55);
            else if (_view.Scale == 100)
                p0_F = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(4)) + _view.RightDirection * Util.CmToFoot(-55);

            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F2_RefSupleMuro", nombreDefamiliaBase + "_F_RefSupleMuro_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            listaTag.RemoveAll(c => c.nombre == "L");
            //XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(100));
            XYZ p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+10)) + _view.RightDirection * Util.CmToFoot(40);
            if (_view.Scale == 75)
                p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+10)) + _view.RightDirection * Util.CmToFoot(40);
            else if (_view.Scale == 100)
                p0_L = _ptoMouse + new XYZ(0, 0, Util.CmToFoot(+10)) + _view.RightDirection * Util.CmToFoot(40);


            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
            AsignarPArametros(this);
        }
        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3_refuerzoSuple> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {

            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_B.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
