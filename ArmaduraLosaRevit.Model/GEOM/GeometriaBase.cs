using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Text;


namespace ArmaduraLosaRevit.Model.GEOM
{
    public abstract class GeometriaBase
    {
        protected readonly UIApplication _uiapp;
        protected Document _doc;
        protected StringBuilder _sbuilder;

        protected List<XYZ> listaPtoBorde;
        public List<PlanarFace> listaPlanarFace { get; set; }
        public List<List<PlanarFace>> listaGrupoPlanarFace { get; set; }

       // public List<Buscar_elementoEncontradoDTO> _listaMuroEcontradoEncontrado { get; private set; }

        protected GeometryElement geo;
        private bool IsComputeReferences;
        protected bool IsInstanceGeometry; // es solo false cuando se quiere obtener las referencias de las pasadas

        public Element _elemento { get; set; }
        public List<RuledFace> ListaRuledFace { get; private set; }
        public List<List<RuledFace>> ListaGrupoRuledFace { get; private set; }

        public GeometriaBase(UIApplication _uiapp, bool _IsInstanceGeometry = true)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;

            ListaRuledFace = new List<RuledFace>();
            ListaGrupoRuledFace = new List<List<RuledFace>>();

            listaPtoBorde = new List<XYZ>();
            listaPlanarFace = new List<PlanarFace>();
            listaGrupoPlanarFace = new List<List<PlanarFace>>();
            IsComputeReferences = false;
            IsInstanceGeometry = _IsInstanceGeometry;
        }

        public GeometriaBase(Document _doc,bool _IsInstanceGeometry=true)
        {

            this._doc = _doc;
            listaPtoBorde = new List<XYZ>();

            listaPlanarFace = new List<PlanarFace>();
            listaGrupoPlanarFace = new List<List<PlanarFace>>();

            ListaRuledFace = new List<RuledFace>();
            ListaGrupoRuledFace = new List<List<RuledFace>>();

            IsComputeReferences = false;
            IsInstanceGeometry = _IsInstanceGeometry;
        }

        public void M1_AsignarGeometriaObjecto(Element _elemento, bool IsComputeReferences_ = false)
        {
            AUX_M1_AsignarGeometriaObjecto(_elemento, IsComputeReferences_ ,ViewDetailLevel.Coarse);

            if (listaGrupoPlanarFace.Count == 0)
                AUX_M1_AsignarGeometriaObjecto(_elemento, IsComputeReferences_, ViewDetailLevel.Medium);

            if (listaGrupoPlanarFace.Count == 0)
                AUX_M1_AsignarGeometriaObjecto(_elemento, IsComputeReferences_, ViewDetailLevel.Fine);
        }

        private void AUX_M1_AsignarGeometriaObjecto(Element _elemento, bool IsComputeReferences_, ViewDetailLevel _ViewDetailLevel)
        {
            IsComputeReferences = IsComputeReferences_;
            this._elemento = _elemento;

            //NOTA : SI EL SOLIDO NO MUESTRA CARAS NI BORDES PUEDE QUE NO ESTE ASIGNAODO EL 'DETAIL LEVEL'. revisa OBS1)
            M1_1_AsignarGeometriaObjectoOpction(_elemento, _ViewDetailLevel);

            foreach (GeometryObject obj in geo)
            {
                if (obj is Solid)
                {
                    M3_AnalizarGeometrySolid(obj);
                }
                else if (obj is GeometryInstance)
                {
                    #region GEOMETRYINSTANCE O GEOMETRIA ANIDADA
                    M2_AnalizarInstanceGeometry(obj);
                    #endregion
                }
            }
        }

        protected void M1_1_AsignarGeometriaObjectoOpction(Element element, ViewDetailLevel _ViewDetailLevel)
        {

            #region PASO 3 OPCIONES DE GEOMETRIA
            //View3D _view3D_parabUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            Options opt = new Options();
            opt.ComputeReferences = IsComputeReferences; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = _ViewDetailLevel;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            geo = element.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            #endregion
        }


        private void M2_AnalizarInstanceGeometry(GeometryObject obj)
        {
            if (obj is GeometryInstance)
            {
                GeometryInstance instanciaanidada = obj as GeometryInstance; //INSTANCIA ANIDADA
                GeometryElement instanceGeometry = default;
                if(IsInstanceGeometry)
                    instanceGeometry = instanciaanidada.GetInstanceGeometry();  //  se obteniene las coordenadas(globales) reales del planarface y susu contornos
                else
                    instanceGeometry = instanciaanidada.GetSymbolGeometry();  // en coordenas locales de familia  // solo lo he usado para obtener las refenrencias para generar dimensiones, de elementos solidos 

                //2) seguir las instancia
                foreach (GeometryObject obj2 in instanceGeometry)
                {

                    if (obj2 is Solid)
                    {

                        M3_AnalizarGeometrySolid(obj2);
                    }
                    else if (obj2 is GeometryInstance)
                    {
                        M2_AnalizarInstanceGeometry(obj2);
                    }
                  

                }

            }
        }

        public virtual void M3_AnalizarGeometrySolid(GeometryObject obj2)
        {
            Solid solid2 = obj2 as Solid;
            if (solid2 != null && solid2.Faces.Size > 0)
            {
                listaPtoBorde.Clear();

                foreach (var face_ in solid2.Faces)
                {
                    if (face_ is PlanarFace)
                    {

                        PlanarFace face = face_ as PlanarFace;
                        if (face == null) continue;
                        listaPlanarFace.Add(face);

                        foreach (EdgeArray erray in face.EdgeLoops) //PERIMETROS CERRADOS O EDGELOOPS
                        {
                            foreach (Edge borde in erray) //COLECCION DE LINEAS DE BORDE
                            {
                                listaPtoBorde.Add(borde.AsCurve().GetEndPoint(0));
                                listaPtoBorde.Add(borde.AsCurve().GetEndPoint(1));

                            }
                        }
                    }
                    else if (face_ is RuledFace)
                    {
                        ListaRuledFace.Add((RuledFace)face_);
                    }

                }

                if (ListaRuledFace.Count > 0)
                    ListaGrupoRuledFace.Add(ListaRuledFace);

                if (listaPlanarFace.Count > 0)
                    listaGrupoPlanarFace.Add(listaPlanarFace);
            }
        }
    }
}