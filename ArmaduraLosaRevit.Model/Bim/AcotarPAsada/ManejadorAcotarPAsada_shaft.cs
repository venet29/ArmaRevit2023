using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada
{
    public class ManejadorAcotarPAsada_Shaft
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorAcotarPAsada_Shaft(UIApplication uiapp)//para atuomatico
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;            
        }


        public bool EjecutarAcotarPasadas()
        {

            try
            {
                //seleccionar pasadas
                //-- crear obtejeo
                SeleccionarOpeningConMouse _SeleccionarOpeningConMose = new SeleccionarOpeningConMouse(_uiapp);
               var lista= _SeleccionarOpeningConMose.M2_SelecconaShaftOpeningRectagulo();
                List<ProfileOpening> ListaProfileOpening = new List<ProfileOpening>();
                foreach (var openn in lista)
                {

                    if (null != openn)
                    {
                        Opening op = (Opening)openn;

                        var planar= op.ObtenerListaPlanarFace();
                        var listaPlanarSUp= planar.Where(c=> c.IsNormalEnZ() && c.FaceNormal.Z>0).ToList();

                        EdgeArrayArray edgeArrarr = listaPlanarSUp[0].EdgeLoops;
                        foreach (EdgeArray edgeArr in edgeArrarr)
                        {
                            List<Edge> edgesList = new List<Edge>();
                            foreach (Edge edge in edgeArr)
                            {
                                Curve _curve= edge.AsCurve();
                                edgesList.Add(edge);
                            }
                        }
                            //para los casos se varias 

                            continue;
                        //var listaLineas= planar.ObtenerListaCurvas();
                        //nivel de la opening seleccionado
                        Level nivel = (Level)_doc.GetElement(openn.LevelId);
                        ProfileOpening profileOpening = new ProfileOpening(openn, _uiapp, nivel);

                        if (profileOpening != null)
                        {
                            if (profileOpening != null) { ListaProfileOpening.Add(profileOpening); }
                        }
                    }
                }

                //buscar intersesciones


                // dibujar dimensiones


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar acotar pasadas. ex:{ex.Message}");
                return true;
            }
            return true;
        }

    }
}
