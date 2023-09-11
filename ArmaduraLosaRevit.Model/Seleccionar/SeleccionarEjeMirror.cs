using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarEjeMirror
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private View _view;
        public XYZ pt1 { get; set; }
        public XYZ pt2 { get; set; }
        public Line lineReferncia { get; private set; }
        public XYZ _ptoInterseccion { get; private set; }

        public SeleccionarEjeMirror(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            pt1 = XYZ.Zero;
            pt2 = XYZ.Zero;
        }


        public bool Ejecutar_SeleccionarEjeMirror()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                //selecciona un objeto floor
                Reference R = null;

                try
                {
                    R = _uidoc.Selection.PickObject(ObjectType.PointOnElement);
                }
                catch
                {
                }
                if (R == null)
                    return false;

                Element elementoSlecionado = _uidoc.Document.GetElement(R.ElementId);


                if (elementoSlecionado == null)
                    return false;



                if (elementoSlecionado is Wall)
                {

                    ObtenerPtosLocationCurve(elementoSlecionado.Location as LocationCurve);
                  //  Util.ErrorMsg($"selecciono muro   p1:{pt1.ToString()}     p2:{pt2.ToString()}");
                }
                else if (elementoSlecionado is ModelLine)
                {
                    ObtenerPtosLocationCurve(elementoSlecionado.Location as LocationCurve);
                  //  Util.ErrorMsg($"selecciono linea       p1:{pt1.ToString()}     p2:{pt2.ToString()}");
                }
                else if (elementoSlecionado.Category.Name == "Structural Framing")
                {
                    ObtenerPtosLocationCurve(elementoSlecionado.Location as LocationCurve);
                  //  Util.ErrorMsg($"selecciono viga    p1:{pt1.ToString()}     p2:{pt2.ToString()}");
                }
                else
                {
                    Util.ErrorMsg("No se pudo obtener alguna referencia. Seleccionar segundo pto para generar eje");
                    pt1 = R.GlobalPoint;

                    try
                    {
                        ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                        //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                        pt2 = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Punto Inferior Barra");
                    }
                    catch (Exception)
                    {
                        return false;
                    }                                           
                }

                 lineReferncia = Line.CreateBound(pt1.AsignarZ(0), pt2.AsignarZ(0));
                 //_ptoInterseccion = lineReferncia.ProjectExtendida(ptoPath.AsignarZ(0));
               // Util.ErrorMsg($"PtoInterseccion { _ptoInterseccion.ToString()}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }

            return true;

        }

        private  void ObtenerPtosLocationCurve(LocationCurve lc)
        {
           
            Line linea_ = (Line)lc.Curve;
            pt1 = linea_.Origin;
            pt2 = linea_.Origin + linea_.Direction * 5;
        }
    }
}
