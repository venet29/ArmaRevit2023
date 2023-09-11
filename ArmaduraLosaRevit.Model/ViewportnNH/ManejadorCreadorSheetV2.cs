using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace ArmaduraLosaRevit.Model.ViewportnNH
{
   

    public class ManejadorCreadorSheetV2
    {
        private UIApplication _uiapp;
        private readonly List<ViewGeom> listaViewGeom;
        private Document _doc;
        private View _view;



        private Element _elementTItutlo;
        private XYZ puntoSupIZq_Sheet;
        private readonly FamilySymbol _templateSheet;

        public ManejadorCreadorSheetV2(UIApplication uiapp, List<ViewGeom> ListaViewGeom, Viewport _viewPort = null, FamilySymbol templateSheet = null)
        {
            this._uiapp = uiapp;
            this.listaViewGeom = ListaViewGeom;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;

            _elementTItutlo = _viewPort;
            this._templateSheet = templateSheet;
        }



        public bool CrearSheet()
        {
            try
            {
                // ViewSheet.Create();
                //view.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION).Set(1);
                // bool canAddViewToSheett = Viewport.CanAddViewToSheet(_doc, sheetId, viewId); // returns true, regardless of weather Viewport.Create() returns a Viewport or returns just null ... weird
                //   Viewport newViewport = Viewport.Create(_doc, sheetId, viewId, centerOfViewport);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearSheet'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public void CreateSheetView_SinTrans()
        {

            XYZ puntoCentral_Sheet = default;
            XYZ puntoInfIZq_Sheet = default;
            XYZ puntoSupDere_SheetV2 = default;
            ViewSheet viewSheet = default;

            ViewDTO ViewDTONh = listaViewGeom[0].viewDTO_;
            try
            {
                if (_templateSheet != null)
                {
                    //using (Transaction t = new Transaction(_doc, "Create a new ViewSheet"))
                    //{
                    //    t.Start();

                    viewSheet = OBtenerSheet_Y_BorrarViewPOrt_SiTiene();

                    if (viewSheet == null)
                    {
                        // Create a sheet view
                        viewSheet = ViewSheet.Create(_doc, _templateSheet.Id);
                        if (null == viewSheet)
                        {
                            throw new Exception("Create new ViewSheet.");
                        }
                        var result = ViewDTONh.ObtenerViewSheetNH();

                        viewSheet.SheetNumber = ViewDTONh.NumeroSheet;

                        EstadosViewDTO _newEstadosViewDTO = new EstadosViewDTO(ViewDTONh.View_);
                        _newEstadosViewDTO.ObtenerDatos();

                        ParameterUtil.SetParaStringNH(viewSheet, "CODIGO ESPECIALIDAD", _newEstadosViewDTO.CODIGOESPECIALIDAD);
                        ParameterUtil.SetParaStringNH(viewSheet, "ESPECIALIDAD", _newEstadosViewDTO.ESPECIALIDAD);
                        ParameterUtil.SetParaStringNH(viewSheet, "CODIGO ESTADO DE AVANCE", _newEstadosViewDTO.CODIGOESTADODEAVANCE);
                        ParameterUtil.SetParaStringNH(viewSheet, "ESTADO DE AVANCE", _newEstadosViewDTO.EstadoDeAvance);
                        ParameterUtil.SetParaStringNH(viewSheet, "TIPO DE ESTRUCTURA (VISTA)", _newEstadosViewDTO.TipoEstructura);
                    }

                    // Add passed in view onto the center of the sheet
                    UV location = new UV((viewSheet.Outline.Max.U - viewSheet.Outline.Min.U) / 2,
                                         (viewSheet.Outline.Max.V - viewSheet.Outline.Min.V) / 2);

                    XYZ puntoSUperiorIzq_Sheet = new XYZ(viewSheet.Outline.Min.U, viewSheet.Outline.Max.V, 0);

                    puntoCentral_Sheet = new XYZ((viewSheet.Outline.Max.U + viewSheet.Outline.Min.U) / 2,
                                                    (viewSheet.Outline.Max.V + viewSheet.Outline.Min.V) / 2, 0);
                    puntoSupIZq_Sheet = new XYZ(viewSheet.Outline.Min.U, viewSheet.Outline.Max.V, 0);
                    puntoInfIZq_Sheet = new XYZ(viewSheet.Outline.Min.U, viewSheet.Outline.Min.V, 0);
                    puntoSupDere_SheetV2 = new XYZ(viewSheet.Outline.Max.U, viewSheet.Outline.Max.V, 0);

                    XYZ PuntoSuperiorDerecho = null;

                    viewSheet.Name = (ViewDTONh.NombreSheetEditado == "" ? ViewDTONh.Nombre : ViewDTONh.NombreSheetEditado);

                    for (int i = 0; i < listaViewGeom.Count; i++)
                    {
                        var _viewIter = listaViewGeom[i].viewDTO_.View_;

                        var portNh = new ViewportNH(_uiapp, _viewIter, viewSheet);
                        portNh.ObtenerDatos(puntoSUperiorIzq_Sheet);

                        if (ViewDTONh.NumeroSheet.Contains("EST-1") || ViewDTONh.NumeroSheet.Contains("EST-2"))
                        {
                            puntoCentral_Sheet = puntoCentral_Sheet + new XYZ(-5+ listaViewGeom[i].X_sheet, 0, 0) / _viewIter.Scale;
                        }
                        else
                            puntoCentral_Sheet = puntoSupIZq_Sheet + new XYZ(15+listaViewGeom[i].X_sheet+ listaViewGeom[i].Ancho/2, -listaViewGeom[i].Alto / 2, 0) / _viewIter.Scale;
                        //viewSheet.AddView(_viewIter, location);
                        //var box = _viewIter.Outline;
                        bool canAddViewToSheett = Viewport.CanAddViewToSheet(_doc, viewSheet.Id, _viewIter.Id);

                        if (canAddViewToSheett)
                        {
                            portNh.CalcularPtoInsercion(PuntoSuperiorDerecho);
                            var viewPortNuevo = Viewport.Create(_doc, viewSheet.Id, portNh.IdPOrt, puntoCentral_Sheet);// portNh.PtoInsercion);

                            viewPortNuevo.ChangeTypeId(_elementTItutlo.GetTypeId());

                            Parameter paramNamePos = viewPortNuevo.get_Parameter(BuiltInParameter.VIEWPORT_ATTR_SHOW_LABEL);

                            if (paramNamePos != null)
                            {
                                paramNamePos.Set(1);
                                var titulo = _doc.GetElement(paramNamePos.AsElementId());
                                if (titulo != null)
                                {
                                    ElementTransformUtils.MoveElement(_doc, titulo.Id, new XYZ(0, -3, 0));
                                }
                            }
                            Parameter paramNamePos2 = viewPortNuevo.get_Parameter(BuiltInParameter.VIEWPORT_ATTR_SHOW_EXTENSION_LINE);
                            PuntoSuperiorDerecho = portNh.PuntoSuperiorDerecho;
                        }
                    }

                    // Print the sheet out
                    if (viewSheet.CanBePrinted && false)
                    {
                        TaskDialog taskDialog = new TaskDialog("Revit");
                        taskDialog.MainContent = "Print the sheet?";
                        TaskDialogCommonButtons buttons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                        taskDialog.CommonButtons = buttons;
                        TaskDialogResult result = taskDialog.Show();

                        if (result == TaskDialogResult.Yes)
                        {
                            viewSheet.Print();
                        }
                    }

                    //    t.Commit();

                    //}

                    //var listaViewALabel = TiposViewportType.ObtenerTodosLabel(_uiapp.ActiveUIDocument.Document, viewSheet);

                    if (false)
                    {
                        CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(8), puntoSupDere_SheetV2, _uiapp.ActiveUIDocument, XYZ.BasisX, XYZ.BasisY, "BarraCirculo", viewSheet);
                        CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(8), puntoCentral_Sheet, _uiapp.ActiveUIDocument, XYZ.BasisX, XYZ.BasisY, "BarraCirculo", viewSheet);
                        CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(8), puntoInfIZq_Sheet, _uiapp.ActiveUIDocument, XYZ.BasisX, XYZ.BasisY, "BarraCirculo", viewSheet);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear sheet : ex{ex.Message}");
            }
        }

        public ViewSheet OBtenerSheet_Y_BorrarViewPOrt_SiTiene()
        {
            ViewSheet viewSheet = null;
            ViewDTO ViewDTONh = listaViewGeom[0].viewDTO_;
            var ViewSheetNH = ViewDTONh.ListaSheet.Where(c => c.ObtenerNUmeroSheet() == ViewDTONh.NumeroSheet).FirstOrDefault();
            if (ViewSheetNH != null)
            {
                viewSheet = (ViewSheet)ViewSheetNH._view;

                BorrarViewPort(viewSheet);
                ViewSheetNH.SheetNumber = "";
            }

            return viewSheet;
        }

        public void BorrarViewPort(ViewSheet viewSheet)
        {
            var listaViewport = viewSheet.GetAllViewports().Select(c => _doc.GetElement(c) as Viewport).ToList();
            for (int i = 0; i < listaViewport.Count; i++)
            {
                var _viewPOrt = listaViewport[i];
                if (_viewPOrt != null)
                {
                    viewSheet.DeleteViewport(_viewPOrt);
                    viewSheet.Name = "";
                }
            }
        }

        internal bool BorrarPortDeOtroSheet_conIgualNombreView_ConTransa()
        {
            try
            {
                using (Transaction t = new Transaction(_doc, "BorrarViewSheet"))
                {
                    t.Start();
                    BorrarPortDeOtroSheet_conIgualNombreView_SinTrans();
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BorrarPortDeOtroSheet_conIgualNombreView_ConTransa'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool BorrarPortDeOtroSheet_conIgualNombreView_SinTrans()
        {
            try
            {
                for (int i = 0; i < listaViewGeom.Count; i++)
                {
                    ViewDTO _viewDTo = listaViewGeom[i].viewDTO_;
                    var _view = _viewDTo.View_;

                    var sheetContieneView = _viewDTo.ListaSheet.Where(c => c.listaPortInSheet.Exists(r => r.Name == _view.Name)).FirstOrDefault();

                    if (sheetContieneView != null)
                    {
                        var _viewPOrt = ((ViewSheet)sheetContieneView._view).GetAllViewports()
                                                                            .Select(c => _doc.GetElement(c) as Viewport)
                                                                            .FirstOrDefault(c => _doc.GetElement(c.ViewId).Name == _view.Name);
                        if (_viewPOrt != null)
                            ((ViewSheet)sheetContieneView._view).DeleteViewport(_viewPOrt);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BorrarPortDeOtroSheet_conIgualNombreView_SinTrans'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }

}
