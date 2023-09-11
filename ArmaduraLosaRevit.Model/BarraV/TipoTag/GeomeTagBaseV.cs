using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MediaNH = System.Windows.Media;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class GeomeTagBaseV
    {

        // private readonly List<XYZ> _listaPtosPerimetroBarras;

        protected readonly XYZ CentroBarra;
        protected View _view;

        //pto inical y final de barra( linea inferior)
        protected XYZ _p1;
        protected XYZ _p2;
        protected XYZ _posiciontag;


        protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        Document _doc;
        //  private RebarInferiorDTO _rebarInferiorDTO1;
        protected int escala;
        protected string nombreDefamiliaBase;


        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }
        //public TagBarra TagP0_A { get; set; }
        //public TagBarra TagP0_B { get; set; }
        public TagBarra TagP0_C { get; set; }
        public TagBarra TagP0_F { get; set; }
        public TagBarra TagP0_F_SIN { get; set; }
        public TagBarra TagP0_L { get; set; }

        public GeomeTagBaseV() { }
        public GeomeTagBaseV(Document doc, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag)
        {
            this._doc = doc;
            _p1 = ptoIni;
            _p2 = ptoFin;
            _posiciontag = posiciontag;
            //this.CentroBarra = (ptoIni + new XYZ(0, 0, Util.CmToFoot(150)));
            this.CentroBarra = _posiciontag;// (ptoFin - new XYZ(0, 0, Util.CmToFoot(50)));
            this._view = _doc.ActiveView;
            //  this.escala = ConstantesGenerales.CONST_ESCALA_BASE;// _view.Scale;
            this.escala = 50;// _view.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = "MRA Rebar";
        }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }

        public virtual void M3_DefinirRebarShape() { }

        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {

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

        //obs4
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            XYZ p0_C = CentroBarra + new XYZ(0, 0, _largoMedioEnFoot * 0.25);
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala, escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = CentroBarra;
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala, escala);
            listaTag.Add(TagP0_F);


            XYZ p0_F_SIN = CentroBarra;
            TagP0_F_SIN = M1_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_F_SIN_" + escala, escala);
            listaTag.Add(TagP0_F_SIN);

            XYZ p0_L = CentroBarra + new XYZ(0, 0, -_largoMedioEnFoot * 0.25);
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala, escala);
            listaTag.Add(TagP0_L);


        }

        protected TagBarra M1_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia, int escala)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(NombreFamilia, _doc);

            //si no la cuentr lCRE
            if (IndependentTagPath == null)
            {
                IndependentTagPath = ObtenertTagGirado(nombreLetra, NombreFamilia, escala);
            }

            if (IndependentTagPath == null) { Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}"); }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;

        }

        private Element ObtenertTagGirado(string nombreLetra, string NombreFamilia, int escala)
        {
            Element IndependentTagPath;
            string NombreFAmiliaGenerico = nombreDefamiliaBase + "_" + nombreLetra + nombreLetra + "_";
            NombreFAmiliaGenerico = $"{nombreDefamiliaBase}_{nombreLetra}{nombreLetra}_";
            CrearFamilySymbolTagRein tiposTagLetrasBarra = new CrearFamilySymbolTagRein(_doc);
            IndependentTagPath = tiposTagLetrasBarra.ObtenerLetraGirada(NombreFamilia, NombreFAmiliaGenerico, _anguloBArraGrado, escala);
            return IndependentTagPath;
        }

        protected TagBarra AgregarTAgLitsta(string nombre, int coorX, int coorY, XYZ ptoReferencia)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);
            XYZ p0_XXX_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoReferencia, _anguloBarraRad, Util.CmToFoot(coorX), Util.CmToFoot(coorY));
            TagBarra TagP0_XXX = M1_1_ObtenerTAgBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}_{nombre}_{escala}_{ _signoAngulo + Math.Abs(_anguloBArraGrado).ToString()}", escala);
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;

        }


    }

}

