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
using ArmaduraLosaRevit.Model.GEOM;

namespace ArmaduraLosaRevit.Model.Stairsnh.Entidades
{
    public class GeometrisStair: GeometriaBase
    {
        //private readonly UIApplication _uiapp;
     //   private Document _doc;
        //private StringBuilder _sbuilder;
        private int element;
#pragma warning disable CS0108 // 'GeometrisStair.listaPtoBorde' hides inherited member 'GeometriaBase.listaPtoBorde'. Use the new keyword if hiding was intended.
        private List<XYZ> listaPtoBorde;
#pragma warning restore CS0108 // 'GeometrisStair.listaPtoBorde' hides inherited member 'GeometriaBase.listaPtoBorde'. Use the new keyword if hiding was intended.

      //  private GeometryElement geo;

        public GeometrisStair(UIApplication _uiapp):base(_uiapp)
        {
          //  this._uiapp = _uiapp;
          //  this._doc = _uiapp.ActiveUIDocument.Document;

        //    _sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto
            element = 0;
            listaPtoBorde = new List<XYZ>();
        }

     

        public override void M3_AnalizarGeometrySolid(GeometryObject obj2)
        {
            Solid solid2 = obj2 as Solid;
            if (solid2 != null && solid2.Faces.Size > 0)
            {
                _sbuilder.AppendLine($" Ele{element}) VOLUMEN SOLIDO: " + solid2.Volume.ToString());
                _sbuilder.AppendLine($" Elem{element}) CANTIDAD DE CARAS: " + solid2.Faces.Size.ToString());

                int cara = 0;
                foreach (PlanarFace face in solid2.Faces)
                {
                    _sbuilder.AppendLine($"--CARA {cara}:   Area: " + face.Area.ToString() + " id:" + face.GetHashCode());
                    _sbuilder.AppendLine($"--FaceNormal: {face.FaceNormal}    Origen: {face.Origin}");
                    _sbuilder.AppendLine("---PERIMETROS CERRADOS: " + face.EdgeLoops.Size.ToString());


                    // cara += 1;
                    //continue;
                    listaPtoBorde.Clear();
                    int perim = 0;
                    foreach (EdgeArray erray in face.EdgeLoops) //PERIMETROS CERRADOS O EDGELOOPS
                    {
                        _sbuilder.AppendLine($"----Perimetro{perim}");
                        int canborde = 0;
                        foreach (Edge borde in erray) //COLECCION DE LINEAS DE BORDE
                        {
                            listaPtoBorde.Add(borde.AsCurve().GetEndPoint(0));
                            listaPtoBorde.Add(borde.AsCurve().GetEndPoint(1));
                            _sbuilder.AppendLine($"-------BORDE {canborde}:" + Math.Round(borde.ApproximateLength, 4).ToString());
                            _sbuilder.AppendLine($"-------ini : {borde.AsCurve().GetEndPoint(0)} -------fin: {borde.AsCurve().GetEndPoint(1)}");
                            _sbuilder.AppendLine($"-------Origin : {((Line)borde.AsCurve()).Origin} -------Direction: {((Line)borde.AsCurve()).Direction}");
                            canborde += 1;
                        }
                        perim += 1;

                        cara += 1;



                    }

                    ////para graficar los ptos
                    //if (face.Area.ToString() == "71.4807942383114" && false)
                    //{
                    //    GraficarPtos DibujoPtosTrasladados5 = new GraficarPtos(listaPtoBorde);
                    //    DibujoPtosTrasladados5.ShowDialog();
                    //}

                }



                element += 1;
            }

            
        }



        //private void GetGeometrObject(Stairs stairs)
        //{
        //    #region PASO 3 OPCIONES DE GEOMETRIA
        //    View3D _view3D_parabUSCAR = Util.GetFirstElementOfTypeNamed(_doc, typeof(View3D), ConstantesGenerales.CONST_NOMBRE_3D_PARA_BUSCAR) as View3D;
        //    Options opt = new Options();
        //    opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
        //    opt.DetailLevel = ViewDetailLevel.Coarse;
        //    opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
        //    geo = stairs.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
        //    #endregion
        //}
    }
}