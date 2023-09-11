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
    public partial class GeomeTagBase
    {

        protected List<XYZ> _listaPtosPerimetroBarras;
        protected SolicitudBarraDTO _solicitudBarraDTO;
        protected XYZ _ptoMouse;
        protected View _view;

        //pto inical y final de barra( linea inferior)
        protected XYZ _p1;
        protected XYZ _p2;

        protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        protected Document _doc;
        protected RebarInferiorDTO _rebarInferiorDTO1;
        protected int escala;
        protected string nombreDefamiliaBase;

        protected double AnguloRadian;
        public UbicacionLosa _ubicacionEnlosa { get; set; }
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }
        public TagBarra TagP0_A { get; set; }
        public TagBarra TagP0_B { get; set; }
        public TagBarra TagP0_C { get; set; }
        public TagBarra TagP0_C2 { get; set; }
        public TagBarra TagP0_D { get; set; }
        public TagBarra TagP0_E { get; set; }
        public TagBarra TagP0_G { get; set; }
        public TagBarra TagP0_H { get; set; }

        public TagBarra TagP0_F { get; set; }
        public TagBarra TagP0_L { get; set; }
        public TagBarra TagP0_L2 { get; set; }
        public TagBarra TagP0_F2 { get; set; }

        public GeomeTagBase() { }

        public GeomeTagBase(Document doc, SolicitudBarraDTO _solicitudBarraDTO)
        {
            this._doc = doc;
            //this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            this._ubicacionEnlosa = _solicitudBarraDTO.UbicacionEnlosa;
            //this._ptoMouse = _solicitudBarraDTO.p;
            this._view = _doc.ActiveView;
            this.escala = _view.ObtenerNombre_EscalaConfiguracion();// ConstNH.CONST_ESCALA_BASE;// _view.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = _solicitudBarraDTO.nombreDefamiliaBase;
        }
        public GeomeTagBase(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)//string nombreDefamiliaBase = "M_Path Reinforcement Tag(ID_cuantia_largo)"
        {
            this._doc = doc;
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            this._ubicacionEnlosa = _solicitudBarraDTO.UbicacionEnlosa;
            this._ptoMouse = ptoMOuse;
            this._view = _doc.ActiveView;
            this.escala = _view.ObtenerNombre_EscalaConfiguracion();// ConstNH.CONST_ESCALA_BASE;// _view.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = _solicitudBarraDTO.nombreDefamiliaBase;
        }
        public GeomeTagBase(Document doc, RebarInferiorDTO rebarInferiorDTO1)//string nombreDefamiliaBase = "M_Path Reinforcement Tag(ID_cuantia_largo)"
        {
            this._doc = doc;
            this._rebarInferiorDTO1 = rebarInferiorDTO1;
            this._listaPtosPerimetroBarras = rebarInferiorDTO1.listaPtosPerimetroBarras;
            this._solicitudBarraDTO = rebarInferiorDTO1.Obtener_solicitudBarraDTO();
            this._ubicacionEnlosa = rebarInferiorDTO1.ubicacionLosa;
            this._ptoMouse = rebarInferiorDTO1.ptoSeleccionMouse;
            this._view = _doc.ActiveView;
            this.escala = _view.ObtenerNombre_EscalaConfiguracion();// ConstNH.CONST_ESCALA_BASE;// ;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = _solicitudBarraDTO.nombreDefamiliaBase;
        }

        public virtual bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            if (!Util.IsSimilarValor(_view.ViewDirection.Z, 0, 0.001)) // planta
            {
                Curve curvePathIzq = Line.CreateBound(_listaPtosPerimetroBarras[1].GetXY0(), _listaPtosPerimetroBarras[2].GetXY0());

                XYZ ptoInterse = curvePathIzq.Project(_ptoMouse.GetXY0()).XYZPoint;
                XYZ vectorDezpla = _ptoMouse.GetXY0() - ptoInterse;

                var valorZ = _view.Obtener_Z_SoloPLantas();

                if (!valorZ.Isok) return false;

                _p1 = (_listaPtosPerimetroBarras[1] + vectorDezpla).AsignarZ(valorZ.valorz);
                _p2 = (_listaPtosPerimetroBarras[2] + vectorDezpla).AsignarZ(valorZ.valorz);

            }
            else //elevacion
            {
                Curve curvePathIzq = Line.CreateBound(_listaPtosPerimetroBarras[0], _listaPtosPerimetroBarras[3]);
                _p1 = curvePathIzq.Project(_ptoMouse).XYZPoint;
                //derecha superior
                Curve curvePathDERE = Line.CreateBound(_listaPtosPerimetroBarras[1], _listaPtosPerimetroBarras[2]);
                _p2 = curvePathDERE.Project(_ptoMouse).XYZPoint;
            }



            _anguloBarraRad = Util.angulo_entre_pt_Rad_XY0(_p1, _p2);
            _anguloBArraGrado = Convert.ToInt32(Math.Round(Util.RadianeToGrados(_anguloBarraRad), 0));
            _signoAngulo = (_anguloBArraGrado < 0 ? "N" : "");
            _largoMedioEnFoot = _p1.DistanceTo(_p2);
            listaTag = new List<TagBarra>();
            return true;
        }

        //obs4
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {

            var escalaCOnfiguracion = _view.ObtenerNombre_EscalaConfiguracion();

            if (_view.Scale == 50)
            {
                if (escalaCOnfiguracion == 50)
                    M2_CAlcularPtosDeTAg_escala50_50();
                //else if (escalaCOnfiguracion == 75)
                //    M2_CAlcularPtosDeTAg_escala50_75();
                else if (escalaCOnfiguracion == 100)
                    M2_CAlcularPtosDeTAg_escala50_100();
                else
                    M2_CAlcularPtosDeTAg_escala50_50();
            }
            else if (_view.Scale == 75)
            {
                if (escalaCOnfiguracion == 50)
                    M2_CAlcularPtosDeTAg_escala75_50();
                //else if (escalaCOnfiguracion == 75)
                //    M2_CAlcularPtosDeTAg_escala75_75();
                else if (escalaCOnfiguracion == 100)
                    M2_CAlcularPtosDeTAg_escala75_100();
                else
                    M2_CAlcularPtosDeTAg_escala75_75();
            }

            else if (_view.Scale == 100)
            {
                if (escalaCOnfiguracion == 50)
                    M2_CAlcularPtosDeTAg_escala100_50();
                //else if (escalaCOnfiguracion == 75)
                //    M2_CAlcularPtosDeTAg_escala100_75();
                else if (escalaCOnfiguracion == 100)
                    M2_CAlcularPtosDeTAg_escala100_100();
                else
                    M2_CAlcularPtosDeTAg_escala100_100();
            }
        }



        #region ESCALA 50
        private void M2_CAlcularPtosDeTAg_escala50_50()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(26));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            double distC = 0; double distL = 0; double distF = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
            {
                distC = 9;
                distF = -8;
                distL = -8;
            }
            else
            {
                distC = -9;
                distF = 9;
                distL = 9;
            }

            ESCALABase50(distC, distL, distF);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(26));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }
        //private void M2_CAlcularPtosDeTAg_escala50_75()
        //{
        //    XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(25));
        //    TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
        //    listaTag.Add(TagP0_A);


        //    XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
        //    TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
        //    listaTag.Add(TagP0_B);


        //    double distC = 0; double distL = 0; double distF = 0;
        //    if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
        //    {
        //        distC = 11;
        //        distF = -10;
        //        distL = -10;
        //    }
        //    else
        //    {
        //        distC = -12;
        //        distF = 5;
        //        distL = 5;
        //    }

        //    ESCALABase50(distC, distL, distF);

        //    XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
        //    TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
        //    listaTag.Add(TagP0_D);

        //    XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(25));
        //    TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
        //    listaTag.Add(TagP0_E);
        //}
        private void M2_CAlcularPtosDeTAg_escala50_100()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(20));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(4));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            double distC = 0; double distL = 0; double distF = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
            {
                distC = 9;
                distF = -8;
                distL = -8;
            }
            else
            {
                distC = -6;
                distF = 1;
                distL = 1;
            }

            ESCALABase50(distC, distL, distF);


            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(20));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void ESCALABase50(double distC, double distL, double distF)
        {
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(distC));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-60), Util.CmToFoot(distF));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(distL));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }
        #endregion

        #region Escala 75

        private void M2_CAlcularPtosDeTAg_escala75_50()
        {
            double distB = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
                distB = 4;
            else if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado <= 90) //obs 1
                distB = 2;


            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(27+ distB));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            Base75();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(27+ distB));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }


        private void M2_CAlcularPtosDeTAg_escala75_100()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(30));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            Base75();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(30));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void M2_CAlcularPtosDeTAg_escala75_75()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(28));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            Base75();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(7));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(28));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void Base75()
        {

            double distB = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
                distB = 4;
            else if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado <= 90) //obs 1
                distB = 2;

            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-11+ distB));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-80), Util.CmToFoot(9+ distB));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(9+ distB));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }



        #endregion


        #region escala100

        private void M2_CAlcularPtosDeTAg_escala100_50()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(60), Util.CmToFoot(42));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-22), Util.CmToFoot(18));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            BaseEscala100();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(18), Util.CmToFoot(18));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(42));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }


        private void M2_CAlcularPtosDeTAg_escala100_75()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(40));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-15), Util.CmToFoot(24));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            BaseEscala100();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(15), Util.CmToFoot(12));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(40));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }
        private void M2_CAlcularPtosDeTAg_escala100_100()
        {

            double distB = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
                distB = 4;
            else if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado <= 90) //obs 1
                distB = 2;


            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(42+ distB));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", nombreDefamiliaBase + "_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-15), Util.CmToFoot(24));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", nombreDefamiliaBase + "_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            BaseEscala100();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(15), Util.CmToFoot(24));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", nombreDefamiliaBase + "_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(42+ distB));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", nombreDefamiliaBase + "_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }
        private void BaseEscala100()
        {

            double distB = 0;
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado > 90) //obs 1
                distB = 4;
            else if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) && _anguloBArraGrado <= 90) //obs 1
                distB = 2;

            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(70), Util.CmToFoot(2+ distB));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-100), Util.CmToFoot(24+ distB));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(70), Util.CmToFoot(24+ distB));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }


        #endregion


        private Element M2_1_1_ObtenertTagGirado(string nombreLetra, string NombreFamilia, int escala)
        {
            Element IndependentTagPath;
            string NombreFAmiliaGenerico = nombreDefamiliaBase + "_" + nombreLetra + nombreLetra + "_";
            NombreFAmiliaGenerico = $"{nombreDefamiliaBase}_{nombreLetra}{nombreLetra}_";
            CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
            IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            return IndependentTagPath;
        }
        private Element ObtenertTagPathGirado(string nombreLetra, string NombreFamilia, int escala)
        {
            Element IndependentTagPath;
            string NombreFAmiliaGenerico = nombreDefamiliaBase + "_" + nombreLetra + "_";
            NombreFAmiliaGenerico = $"{nombreDefamiliaBase}_{nombreLetra}_";
            CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
            IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            return IndependentTagPath;
        }


        #region Tag para pathreinforment

        protected TagBarra AgregarTagPathreinLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M1_1_ObtenerTAgPathBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}_{nombre}_{escala}_{ _signoAngulo + Math.Abs(_anguloBArraGrado).ToString()}", escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }
        protected TagBarra AgregarTagPathreinLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia, string nombreFamilia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M1_1_ObtenerTAgPathBarra(p0_XXX_, nombre, nombreFamilia, escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }

        protected TagBarra M1_1_ObtenerTAgPathBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {
            //caso sin giraR
            //Element IndependentTagPath = TiposPathReinSpanSymbol.GetFamilySymbol_nh(NombreFamilia, BuiltInCategory.OST_PathReinTags, _doc);

            Element IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(NombreFamilia, _doc);

            //si no la cuentr lCRE
            if (IndependentTagPath == null)
            {
                if (nombreLetra == "L2")
                    IndependentTagPath = ObtenertTagPathGirado("LL2", NombreFamilia, escala);
                else if (nombreLetra == "C2")
                    IndependentTagPath = ObtenertTagPathGirado("CC2", NombreFamilia, escala);
                else if (nombreLetra == "F2")
                    IndependentTagPath = ObtenertTagPathGirado("FF2", NombreFamilia, escala);
                else if (nombreLetra == "F2_RefSupleMuro")
                    IndependentTagPath = ObtenertTagPathGirado("FF2_RefSupleMuro", NombreFamilia, escala);
                else
                    IndependentTagPath = ObtenertTagPathGirado(nombreLetra + nombreLetra, NombreFamilia, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }



        #endregion


        #region Tag para rebar

        protected TagBarra AgregarTagRebarLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M2_1_ObtenerTAgRebarBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}_{nombre}_{escala}_{ _signoAngulo + Math.Abs(_anguloBArraGrado).ToString()}", escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }
        protected TagBarra AgregarTagRebarLista(string nombre, int coorX, int coorY, XYZ ptoReferencia, string nombreFamilia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M2_1_ObtenerTAgRebarBarra(p0_XXX_, nombre, nombreFamilia, escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }
        protected TagBarra M2_1_ObtenerTAgRebarBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(NombreFamilia, _doc);

            //si no la cuentr lCRE
            if (IndependentTagPath == null)
            {
                //no se usa
                // IndependentTagPath = M2_1_1_ObtenertTagGirado(nombreLetra, NombreFamilia, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }

        #endregion

        //solo caso rebar , no path
        protected XYZ ObtenerNuevoptoMouseANivelView(double elevacion)
        {
            XYZ ptoMouseAnivelVista = Util.IntersectionXYZ(_p1.GetXY0(), _p2.GetXY0(), _rebarInferiorDTO1.PtoDirectriz1.GetXY0(), _rebarInferiorDTO1.PtoDirectriz2.GetXY0()).AsignarZ(elevacion);

            return ptoMouseAnivelVista;
        }

    }

}

