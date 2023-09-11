using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class GeomeTagEstriboBase
    {

        
        protected readonly XYZ CentroBarra;
   
        protected XYZ _posiciontag;


      //  protected double _anguloBarraRad;
        protected int _anguloBArraGrado;
        protected string _signoAngulo;
        protected double _largoMedioEnFoot;
        Document _doc;

        protected int escala;
        protected int escala_realview;
        protected string nombreDefamiliaBase;

      
        //lista con objetos que representan los tag de la barra
        public List<TagBarra> listaTag { get; set; }

        public TagBarra TagP0_Traba { get; set; }
        public TagBarra TagP0_Lateral { get; set; }
        public TagBarra TagP0_Estribo { get; set; }
        public TagBarra TagP0_Espesor { get; private set; }

        public GeomeTagEstriboBase(Document doc, XYZ posiciontag, string nombreDefamiliaBase)
        {
            this._doc = doc;

            this._posiciontag = posiciontag;
            //this.CentroBarra = (ptoIni + new XYZ(0, 0, Util.CmToFoot(150)));
            this.CentroBarra = _posiciontag;// (ptoFin - new XYZ(0, 0, Util.CmToFoot(50)));
                                            //this._view = _doc.ActiveView;
                                            // this.escala = ConstantesGenerales.CONST_ESCALA_BASE;// _view.Scale;
            this.escala = 50;// _doc.ActiveView.Scale;
            this.escala_realview = _doc.ActiveView.Scale;

            //dos opciones  "M_Path Reinforcement Tag(ID_cuantia_largo)"
            this.nombreDefamiliaBase = nombreDefamiliaBase+ "_"+ escala;
            this.listaTag = new List<TagBarra>();
        }

        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            //  _anguloBarraRad = Util.angulo_entre_ptRadXY0(_p1, _p2);
            //_largoMedioEnFoot = _p1.DistanceTo(_p2);
            return true;
        }

        //obs4
        public  void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            int desplazaEESTRIBO = 10;
            int desplazaLATERAL = 5;
            int desplazaTRABA = 20;
            int desplazaEspesor = 70;

            if (escala_realview == 75)
            {
                 desplazaEESTRIBO = 12;
                 desplazaLATERAL = 6;
                 desplazaTRABA = 22;
                desplazaEspesor = 80;
            }
            if (escala_realview == 100)
            {
                desplazaEESTRIBO = 13;
                desplazaLATERAL = 9;
                desplazaTRABA = 24;
                desplazaEspesor = 110;
            }

            XYZ p0_Estribo = CentroBarra + new XYZ(0, 0, Util.CmToFoot(desplazaEESTRIBO));
            TagP0_Estribo = M1_1_ObtenerTAgBarra(p0_Estribo, "ESTRIBO", nombreDefamiliaBase);
            if(TagP0_Estribo!=null)listaTag.Add(TagP0_Estribo);
            
            XYZ p0_Lateral  = CentroBarra - new XYZ(0, 0, Util.CmToFoot(desplazaLATERAL));
            TagP0_Lateral  = M1_1_ObtenerTAgBarra(p0_Lateral , "LATERAL", nombreDefamiliaBase);
            if (TagP0_Lateral != null) listaTag.Add(TagP0_Lateral);

            XYZ p0_Trabas = CentroBarra - new XYZ(0, 0, Util.CmToFoot(desplazaTRABA));
            TagP0_Traba = M1_1_ObtenerTAgBarra(p0_Trabas, "TRABA", nombreDefamiliaBase);
            if (TagP0_Traba != null) listaTag.Add(TagP0_Traba);

            XYZ p0_Espesor = CentroBarra + new XYZ(0, 0, Util.CmToFoot(desplazaTRABA + desplazaEspesor));
            TagP0_Espesor = M1_1_ObtenerTAgBarraEspesor(p0_Espesor, "ESP");
            if (TagP0_Espesor != null) listaTag.Add(TagP0_Espesor);

        }

        protected TagBarra M1_1_ObtenerTAgBarra(XYZ posicion, string nombreLetra, string NombreFamilia)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposRebarTag.M1_GetRebarTag(NombreFamilia, _doc);
     
            if (IndependentTagPath == null) {

                Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}");
                return null;
            }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, NombreFamilia, IndependentTagPath);
            return newTagBarra;
        }

        protected TagBarra M1_1_ObtenerTAgBarraEspesor(XYZ posicion, string nombreLetra)
        {
            //caso sin giraR
            Element IndependentTagPath = TiposWallTagsEnView.cargarListaDetagWall2(_doc, "12mm_50", "TAG MUROS (CORTO)_elev");
    
            if (IndependentTagPath == null)
            {
                Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}");
                return null;
            }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, "familiaEspesor", IndependentTagPath);
            return newTagBarra;
        }
        protected TagBarra AgregaroEditaPosicionTAgLitsta(string nombre, XYZ p0_XXX_)
        {
            listaTag.RemoveAll(c => c.nombre == nombre);           
            TagBarra TagP0_XXX = M1_1_ObtenerTAgBarra(p0_XXX_, nombre, $"{nombreDefamiliaBase}");
            listaTag.Add(TagP0_XXX);
            return TagP0_XXX;
        }
    }

}

