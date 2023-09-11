using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MediaNH = System.Windows.Media;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class GeomeTagBaseFund
    {

        protected  List<XYZ> _listaPtosPerimetroBarras;
        protected  SolicitudBarraDTO _solicitudBarraDTO;
        protected  XYZ _ptoMouse;
        protected View _view;

        //pto inical y final de barra( linea inferior)
        protected XYZ _p1;
        protected XYZ _p2;

        protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        Document _doc;
#pragma warning disable CS0649 // Field 'GeomeTagBaseFund._rebarInferiorDTO1' is never assigned to, and will always have its default value null
        private RebarInferiorDTO _rebarInferiorDTO1;
#pragma warning restore CS0649 // Field 'GeomeTagBaseFund._rebarInferiorDTO1' is never assigned to, and will always have its default value null
        protected int escala;
        protected string nombreDefamiliaBase;

        public UbicacionLosa _ubicacionEnlosa { get; set; }
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }
        public TagBarra TagP0_A { get; set; }
        public TagBarra TagP0_B { get; set; }
        public TagBarra TagP0_C { get; set; }
        public TagBarra TagP0_D { get; set; }
        public TagBarra TagP0_E { get; set; }
        public TagBarra TagP0_G { get; set; }
        public TagBarra TagP0_H { get; set; }

        public TagBarra TagP0_F { get; set; }
        public TagBarra TagP0_L { get; set; }

        public GeomeTagBaseFund() { }
        public GeomeTagBaseFund(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)//string nombreDefamiliaBase = "M_Path Reinforcement Tag(ID_cuantia_largo)"
        {
            this._doc = doc;
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            this._ubicacionEnlosa = _solicitudBarraDTO.UbicacionEnlosa;
            this._ptoMouse = ptoMOuse;
            this._view = _doc.ActiveView;
            this.escala = ConstNH.CONST_ESCALA_BASE;// _view.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = _solicitudBarraDTO.nombreDefamiliaBase;
        }


        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {
                var resultZ = _view.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok) return false;
            

                Curve curvePathIzq = Line.CreateBound(_listaPtosPerimetroBarras[0].GetXY0(), _listaPtosPerimetroBarras[1].GetXY0());
                _p1 = curvePathIzq.Project(_ptoMouse.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                //derecha superior
                Curve curvePathDERE = Line.CreateBound(_listaPtosPerimetroBarras[3].GetXY0(), _listaPtosPerimetroBarras[2].GetXY0());
                _p2 = curvePathDERE.Project(_ptoMouse.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);

                _anguloBarraRad = Util.angulo_entre_pt_Rad_XY0(_p1, _p2);
                _anguloBArraGrado = Convert.ToInt32(Math.Round(Util.RadianeToGrados(_anguloBarraRad), 0));
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

        //obs4
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {


            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(24));
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-9));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-50), Util.CmToFoot(7));
            //TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "Ffund", nombreDefamiliaBase + "_Ffund_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(8));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(24));
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);

        }

        protected TagBarra M1_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(NombreFamilia, _doc);

            //si no la cuentra es poruqe es tag girado
            if (IndependentTagPath == null)
            {
                if (nombreLetra == "Ffund")
                    IndependentTagPath = ObtenertTagGirado("FFfund", NombreFamilia, escala);
                else
                    IndependentTagPath = ObtenertTagGirado(nombreLetra, NombreFamilia, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }

        private Element ObtenertTagGirado(string nombreLetra, string NombreFamilia, int escala)
        {
            Element IndependentTagPath;

            string NombreFAmiliaGenerico = $"{nombreDefamiliaBase}_{nombreLetra}_";
            CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
            IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            return IndependentTagPath;
        }

        protected TagBarra AgregarTAgLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia, bool Isdirectriz, XYZ ptoElbo)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M1_1_ObtenerTAgBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}_{nombre}_{escala}_{ _signoAngulo + Math.Abs(_anguloBArraGrado).ToString()}", escala);

            TagP0_XXX.IsDIrectriz = Isdirectriz;
            if (Isdirectriz)
                TagP0_XXX.PtoCodo_LeaderElbow = ptoElbo;


            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }

        //solo caso rebar , no path
        protected XYZ ObtenerNuevoptoMouseANivelView(double elevacion)
        {
            XYZ ptoMouseAnivelVista = Util.IntersectionXYZ(_p1.GetXY0(), _p2.GetXY0(), _rebarInferiorDTO1.PtoDirectriz1.GetXY0(), _rebarInferiorDTO1.PtoDirectriz2.GetXY0()).AsignarZ(elevacion);

            return ptoMouseAnivelVista;
        }

    }

}

