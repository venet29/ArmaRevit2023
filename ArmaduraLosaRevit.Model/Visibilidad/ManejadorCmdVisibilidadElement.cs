using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;

namespace ArmaduraLosaRevit.Model.Visibilidad
{

    public class ManejadorCmdVisibilidadElement
    {
        public static Result CommandCambiarColor(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;
            ElementId id;

            try
            {
                Selection sel = uidoc.Selection;
                Reference r = sel.PickObject(ObjectType.Element, "Pick element to change its colour");
                id = r.ElementId;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            List<ElementId> unicoElem = new List<ElementId>();
            unicoElem.Add(id);
            VisibilidadElementOrientacion visibilidadElement = new VisibilidadElementOrientacion(commandData.Application);
            visibilidadElement.ChangeListaElementColorConTrans(unicoElem, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));

            return Result.Succeeded;

        }


        public static Result CommandOcultarElemento(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;
            ElementId id;
            Element ee;

            try
            {
                Selection sel = uidoc.Selection;
                Reference r = sel.PickObject(ObjectType.Element, "Seleccionar elemento a ocultar");
                id = r.ElementId;
                ee = doc.GetElement(id);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;

            }
            if (ee == null) return Result.Succeeded; ;

            VisibilidadElementBase visibilidadElement = new VisibilidadElementDiametro(commandData.Application);
            // VisibilidadElement visibilidadElement = new VisibilidadElementOrientacion(commandData);
            IList<Element> listee = new List<Element>();
            listee.Add(ee);
            visibilidadElement.vi1_OcultarElemento(listee, view);

            return Result.Succeeded;

            //}


        }

        public static Result CommandDESOcultarElemento(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;
            ElementId id;
            Element ee;


            try
            {
                Selection sel = uidoc.Selection;
                Reference r = sel.PickObject(ObjectType.Element, "Seleccionar elemento a DesOcultar");
                id = r.ElementId;
                ee = doc.GetElement(id);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }


            if (ee == null) return Result.Succeeded;
            if (!(ee is PathReinforcement)) return Result.Cancelled;




            PathReinforcement m_createdPathReinforcement = (PathReinforcement)ee;
            if (!m_createdPathReinforcement.IsValidObject) return Result.Cancelled;

            var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds();
            List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();
            RebarInSystem rebarInSystem = (RebarInSystem)doc.GetElement(ListElemId[0]);


            VisibilidadElementBase visibilidadElement = new VisibilidadElementDiametro(commandData.Application);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementOrientacion(commandData);
            IList<Element> listee = new List<Element>();
            listee.Add(rebarInSystem);

            visibilidadElement.Vi2_DesOcultarElemento_conTrans(listee, view);

            return Result.Succeeded;

        }


        public static Result CmdM1_MostrarPorOrientacion(UIApplication _uiapp,View _view)
        {
           // UIApplication _uiapp = commandData.Application;
           // var jurnal = commandData.JournalData;
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp,_view);
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view);

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementOrientacion(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M1_MostrarPorOrientacion();
            return Result.Succeeded;

        }

        public static Result CmdM1_MostrarPorOrientacion_H(UIApplication _uiapp, View _view)
        {
      
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.H };
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.H };

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementOrientacion(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M1_MostrarPorOrientacion();
            return Result.Succeeded;

        }

        public static Result CmdM1_MostrarPorOrientacion_MostarPAth(UIApplication _uiapp, View _view, AccionTipoBarraOrientacion _AccionTipoBarraOrientacion)
        {

            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.Ambos };
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.Ambos };

            TiposBarrasDTo tiposBarrasDTo = new TiposBarrasDTo() { Orientacion = _AccionTipoBarraOrientacion };
            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementOrientacion_conPathSYm(_uiapp, tiposBarrasDTo);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M4_CAmbiar_PorTipo();
            return Result.Succeeded;

        }



        public static Result CmdM1_MostrarPorOrientacion_V(UIApplication _uiapp, View _view)
        {
          
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.V };
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view) { OrientacionVisualizacion = OrientacionVisualizacion.V };

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementOrientacion(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M1_MostrarPorOrientacion();
            return Result.Succeeded;

        }


        public static Result CmdM1_MostrarPorDireccion_1(UIApplication _uiapp, View _view)
        {
           // UIApplication _uiapp = commandData.Application;
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view) { _DireccionVisualizacion = DireccionVisualizacion.Uno };
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view) { _DireccionVisualizacion = DireccionVisualizacion.Uno };

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementDireccion(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M1_MostrarPorOrientacion();
            return Result.Succeeded;

        }

        public static Result CmdM1_MostrarPorDireccion_2(UIApplication _uiapp, View _view)
        {
            
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view) { _DireccionVisualizacion = DireccionVisualizacion.Dos };
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view) { _DireccionVisualizacion = DireccionVisualizacion.Dos };

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementDireccion(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementDiametro(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M1_MostrarPorOrientacion();
            return Result.Succeeded;

        }







        public static Result CmdM2_MostrarPAthPorDiametro(UIApplication _uiapp, View _view)
        {
      
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp,_view);
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view);

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementDiametro(_uiapp);
            // VisibilidadElement VisibilidadElement = new VisibilidadElementOrientacion(commandData);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M2_MostrarPorDiametro();
            return Result.Succeeded;

        }





        public static Result CmdM3_MostrarPAthNormal(UIApplication _uiapp, View _view)
        {
          
            SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad( _uiapp, _view);
            SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view);

            SeleccionarRebarElemento _SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _view);
            VisibilidadElementBase VisibilidadElement = new VisibilidadElementDiametro(_uiapp);

            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, SeleccionarPathReinfomentVisibilidad, _SeleccionarRebarElemento, _SeleccionarRebarVisibilidad);
            ManejadorVisibilidad.M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal();
            return Result.Succeeded;

        }





        public static Result CmdM4_MostrarPAthPorTipo(ExternalCommandData commandData)
        {
            Visualizacion Visualizacion = new Visualizacion();
            Visualizacion.ShowDialog();

            if (Visualizacion.DialogResult.HasValue && Visualizacion.DialogResult.Value)
            {
                TiposBarrasDTo tiposBarrasDTo = new TiposBarrasDTo() { tipofx = Visualizacion.fx, tiposx = Visualizacion.sx };

                SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(commandData.Application, commandData.View);
                VisibilidadElementBase VisibilidadElement = new VisibilidadElementTipoBarra(commandData.Application, tiposBarrasDTo);

                ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, VisibilidadElement, SeleccionarPathReinfomentVisibilidad);
                ManejadorVisibilidad.M4_CAmbiar_PorTipo();
            }

            return Result.Succeeded;

        }






        public static Result CmdM5_OcultarBArrasQueNoSeanVista(ExternalCommandData commandData)
        {
            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(commandData.Application, _view);
            SeleccionarAreaPath seleccionarAreaPath = new SeleccionarAreaPath(commandData.Application, _view);
            VisibilidadElemenNoEnView VisibilidadElement = new VisibilidadElemenNoEnView(commandData.Application);


            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, VisibilidadElement, seleccionarRebar, seleccionarAreaPath, _view, _view.Name);
            ManejadorVisibilidad.M5_OcultarBarraNoElevacion();
            return Result.Succeeded;

        }






        public static Result CmdM6_ActualizarNameVista(ExternalCommandData commandData)
        {
            ManejadorVisibilidadActualizarNombreVista _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadActualizarNombreVista(commandData.Application);
            _ManejadorVisibilidadElemenNoEnView.Ejecutar();
            return Result.Succeeded;

        }






        public static Result CmdM7_OcultarBArrasVigasIDem(ExternalCommandData commandData)
        {
            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(commandData.Application, _view);

            //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, seleccionarRebar);
            ManejadorVisibilidad.M7_OcultarBarrasVigaIdem();
            return Result.Succeeded;

        }





        public static Result CmdM8_DesOcultarBArrasVigasIDem(ExternalCommandData commandData)
        {
            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(commandData.Application, _view);

            //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, seleccionarRebar);
            ManejadorVisibilidad.M8_DesOcultarBarrasVigaIdem();
            return Result.Succeeded;

        }





        public static Result CmdM9_M9_Restablecer_Color_BarrasElevacion(ExternalCommandData commandData)
        {
            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(commandData.Application, _view);

            //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, seleccionarRebar);
            ManejadorVisibilidad.M9_Restablecer_Color_BarrasElevacion();
            return Result.Succeeded;

        }



    }

}