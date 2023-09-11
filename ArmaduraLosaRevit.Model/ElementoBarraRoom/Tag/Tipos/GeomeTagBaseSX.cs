using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
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
    public class GeomeTagBaseSX
    {

        protected readonly List<XYZ> _listaPtosPerimetroBarras;
        protected readonly SolicitudBarraDTO _solicitudBarraDTO;
        protected readonly XYZ _ptoMouse;
        protected View _view;

        //pto inical y final de barra( linea inferior)
        protected XYZ _p1;
        protected XYZ _p2;

        protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        protected Document _doc;
        protected readonly int escala;

        protected double AnguloRadian;
        
        public UbicacionLosa _ubicacionEnlosa { get; set; }
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }
        public TagBarra TagP0_A { get;  set; }
        public TagBarra TagP0_B { get; set; }
        public TagBarra TagP0_C { get; set; }
        public TagBarra TagP0_D { get; set; }
        public TagBarra TagP0_E { get; set; }
        public TagBarra TagP0_F { get; set; }
        public TagBarra TagP0_L { get; set; }


        public GeomeTagBaseSX() { }
        public GeomeTagBaseSX(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
        {
            this._doc = doc;
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            this._ubicacionEnlosa = _solicitudBarraDTO.UbicacionEnlosa;
            this._ptoMouse = ptoMOuse;
            this._view = _doc.ActiveView;
            this.escala = _view.ObtenerNombre_EscalaConfiguracion();  //+ConstNH.CONST_ESCALA_BASE;// _view.Scale;
        }


        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {          
                Curve curvePathIzq = Line.CreateBound(_listaPtosPerimetroBarras[0], _listaPtosPerimetroBarras[1]);
                _p1 = curvePathIzq.Project(_ptoMouse).XYZPoint;
                //derecha superior
                Curve curvePathDERE = Line.CreateBound(_listaPtosPerimetroBarras[3], _listaPtosPerimetroBarras[2]);
                _p2 = curvePathDERE.Project(_ptoMouse).XYZPoint;
                var ptoOrdenado = Util.Ordena2Ptos(_p1, _p2);
                _p1= ptoOrdenado[0];
                _p2= ptoOrdenado[1];
                _anguloBarraRad = Util.angulo_entre_pt_Rad_XY0(_p1, _p2);
                _anguloBArraGrado = Convert.ToInt32(Math.Round(Util.RadianeToGrados(_anguloBarraRad), 0));
                _signoAngulo = (_anguloBArraGrado < 0 ? "N" : "");
                _largoMedioEnFoot = _p1.DistanceTo(_p2);
                listaTag = new List<TagBarra>();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        //obs4
        public virtual void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {


            if (_view.ObtenerNombre_EscalaConfiguracion() == 50)
                M2_CAlcularPtosDeTAg_escala50();
            else if (_view.ObtenerNombre_EscalaConfiguracion() == 75)
                M2_CAlcularPtosDeTAg_escala75();
            else if (_view.ObtenerNombre_EscalaConfiguracion() == 100)
                M2_CAlcularPtosDeTAg_escala100();


        }

        private void M2_CAlcularPtosDeTAg_escala50()
        {

            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(10), Util.CmToFoot(-28));
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(-7));
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-9));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-50), Util.CmToFoot(7));
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(70), Util.CmToFoot(8));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(-7));
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-10), Util.CmToFoot(-28));
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);

            //foreach (var item in listaTag)
            //{
            //    Debug.Print("Posicion:" + item.posicion + " Lado:" + item.nombre + " Familia:" + item.nombreFamilia);
            //}
        }

        private void M2_CAlcularPtosDeTAg_escala75()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(10), Util.CmToFoot(-32));
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(-12));
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-15));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-75), Util.CmToFoot(7));
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(70), Util.CmToFoot(8));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(-12));
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-10), Util.CmToFoot(-32));
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void M2_CAlcularPtosDeTAg_escala100()
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(10), Util.CmToFoot(-25));
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);


            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-15), Util.CmToFoot(-9));
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);


            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-12));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-100), Util.CmToFoot(15));
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(70), Util.CmToFoot(15));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(15), Util.CmToFoot(-9));
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-10), Util.CmToFoot(-25));
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        protected TagBarra M1_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {

            Element IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(NombreFamilia,  _doc);

            //si no la cuentr lCRE
            if (IndependentTagPath == null)
            {
                string NombreFAmiliaGenerico = "";

                if (nombreLetra=="F" || nombreLetra == "L")
                    NombreFAmiliaGenerico = "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_" + nombreLetra + nombreLetra + "_";
                else
                    NombreFAmiliaGenerico = "M_Path Reinforcement Tag(ID_cuantia_largo)_" + nombreLetra + nombreLetra + "_";

                CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
                IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }



    }

}
