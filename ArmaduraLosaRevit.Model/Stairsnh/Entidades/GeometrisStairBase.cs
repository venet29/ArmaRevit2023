using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using System;

using System.Linq;
using System.Collections.Generic;
using System.Text;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;

namespace ArmaduraLosaRevit.Model.Stairsnh.Entidades
{
    public abstract class GeometrisStairBase 
    {
        protected readonly UIApplication _uiapp;
        protected Document _doc;
        protected StringBuilder _sbuilder;
        protected int element;
        protected List<XYZ> list;
        protected PlanarFace planarFaceMaxArea;
        protected GeometryElement geo;

        public Stairs Stairs { get; set; }
        public GeometrisStairBase(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;

            _sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto
            element = 0;
            list = new List<XYZ>();
        }

        public void M1_1_GetGeometrObject(Stairs stairs)
        {
            this.Stairs = stairs;
 
            M1_1_AsignarGeometriaObjecto(stairs);

            foreach (GeometryObject obj in geo)
            {
                if (obj is Solid)
                {
                    M3_AnalizarGeometrySolid(obj);
                }
                else if (obj is GeometryInstance)
                {
                    #region GEOMETRYINSTANCE O GEOMETRIA ANIDADA
                    AnalizarInstanceGeometry(obj);
                    #endregion
                }
            }

          //  LogNH.guardar_registro_StringBuilder(_sbuilder, ConstantesGenerales.rutaLogNh);
          //  Util.InfoMsg(_sbuilder.ToString());
        }

        protected void M1_1_AsignarGeometriaObjecto(Element element)
        {

            #region PASO 3 OPCIONES DE GEOMETRIA
            View3D _view3D_parabUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            geo = element.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            #endregion
        }


        private void AnalizarInstanceGeometry(GeometryObject obj)
        {
            if (obj is GeometryInstance)
            {
                GeometryInstance instanciaanidada = obj as GeometryInstance; //INSTANCIA ANIDADA

                GeometryElement instanceGeometry = instanciaanidada.GetInstanceGeometry();  // en cooordenadas del proyecto
                                                                                            //  GeometryElement symbolGeometry = instanciaanidada.GetSymbolGeometry();  // en coordenas locales de familia
                //2) seguir las instancia
                foreach (GeometryObject obj2 in instanceGeometry)
                {
                    if (obj2 is Solid)
                    {
                        M3_AnalizarGeometrySolid(obj2);
                    }
                    else if (obj2 is GeometryInstance)
                    {
                        AnalizarInstanceGeometry(obj2);
                    }
                }

            }
        }

       public  virtual void M3_AnalizarGeometrySolid(GeometryObject obj2)
        {
            Solid solid2 = obj2 as Solid;
            if (solid2 != null && solid2.Faces.Size > 0)
            {

                int cara = 0;
                foreach (PlanarFace face in solid2.Faces)
                {


                    // cara += 1;
                    //continue;
                    list.Clear();
                    int perim = 0;
                    foreach (EdgeArray erray in face.EdgeLoops) //PERIMETROS CERRADOS O EDGELOOPS
                    {
                        int canborde = 0;
                        foreach (Edge borde in erray) //COLECCION DE LINEAS DE BORDE
                        {
                            list.Add(borde.AsCurve().GetEndPoint(0));
                            list.Add(borde.AsCurve().GetEndPoint(1));
                            canborde += 1;
                        }
                        perim += 1;
                        cara += 1;
                    }
                    //para graficar los ptos
                    //if (face.Area.ToString() == "71.4807942383114" && false)
                    //{
                    //    GraficarPtos DibujoPtosTrasladados5 = new GraficarPtos(list);
                    //    DibujoPtosTrasladados5.ShowDialog();
                    //}

                }

                element += 1;
            }
        }
    }
}