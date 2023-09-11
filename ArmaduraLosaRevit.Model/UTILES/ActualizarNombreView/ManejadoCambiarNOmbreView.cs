using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ActualizarNombreView
{
    public class ManejadoCambiarNOmbreView
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        private View _view;
        private Document _doc;

        public ManejadoCambiarNOmbreView(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._doc = _uidoc.Document;
        }

        public bool Ejecutar()
        {
            try
            {
                ActualizarNombreViewFrm _ActualizarNombreViewFrm = new ActualizarNombreViewFrm(_view.Name);
                _ActualizarNombreViewFrm.ShowDialog();
                if (_ActualizarNombreViewFrm.Isok) ActualizarConElementoID(_ActualizarNombreViewFrm.ActualizarNombreView);
            }
            catch (Exception)
            {
            }
            return true;
        }

        private bool ActualizarCOnelemento(string nombrevista)
        {
            bool continuar = true;

            while (continuar)
            {
                try
                {
                    continuar = false;
                    ISelectionFilter f = new FiltroRebar_PathRein();
                    //selecciona un objeto floor
                    var pathReinSpanSymbol_Lista = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar (pathreinforment y rebar) barras ");

                    //  var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar barra");
                    using (Transaction trans2 = new Transaction(_doc))
                    {
                        trans2.Start("ActualizarNOmbreView-NH");
                        foreach (Element ref_elem in pathReinSpanSymbol_Lista)
                        {
                            if (_view != null && ParameterUtil.FindParaByName(ref_elem, "NombreVista") != null)
                                ParameterUtil.SetParaInt(ref_elem, "NombreVista", nombrevista);  //"nombre de vista"
                        }
                        trans2.Commit();
                    }
                    continuar = true;
                }
                catch (Exception)
                {
                    continuar = false;
                }
            }

            return continuar;
        }


        private bool ActualizarConElementoID(string nombrevista)
        {
            bool continuar = true;
            int contador = 0;
            while (continuar)
            {
                try
                {
                    continuar = false;
                    ISelectionFilter f = new FiltroRebar_PathRein();
                    //selecciona un objeto floor
                    // var pathReinSpanSymbol_Lista = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar (pathreinforment y rebar) barras ");

                    var pathReinSpanSymbol_Lista = _uidoc.Selection.PickObjects(ObjectType.Element, f, $"Seleccionar barra   (#barras preselecionadas {contador})");

                    contador = pathReinSpanSymbol_Lista.Count;
                    using (Transaction trans2 = new Transaction(_doc))
                    {
                        trans2.Start("ActualizarNOmbreView-NH");

                        for (int i = 0; i < pathReinSpanSymbol_Lista.Count; i++)
                        {
                            Reference ref_elem = pathReinSpanSymbol_Lista[i];
                            Element elem = _doc.GetElement(ref_elem);
                            if (!(elem is PathReinforcement)) continue;

                            if (_view != null && ParameterUtil.FindParaByName(elem, "NombreVista") != null)
                                ParameterUtil.SetParaInt(elem, "NombreVista", nombrevista);  //"nombre de vista"

                            ActualizarRebarInSystem.AgregarParametroRebarSystem_sinTrans((PathReinforcement)elem, nombrevista, "");

                        }
                        trans2.Commit();
                    }
                    continuar = true;
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error   erx:{ex.Message}");
                    continuar = false;
                }
            }

            return continuar;
        }


    }
}
