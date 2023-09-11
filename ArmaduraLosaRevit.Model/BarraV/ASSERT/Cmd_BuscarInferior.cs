#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using System.Diagnostics;
using System.Linq;
using opcionalesNh;
using ArmaduraLosaRevit.Model.Extension;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_BuscarInferior : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            View3D _view3D = Util.GetFirstElementOfTypeNamed(commandData.Application.ActiveUIDocument.Document, typeof(View3D), ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR) as View3D;
            if (_view3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return Result.Failed;
            }

            XYZ ptoBusqueda = new XYZ(-18.94685, 14.21, -42.355643);
            BuscarElementosBajo BuscarElementosAbajo = new BuscarElementosBajo(commandData.Application,UtilBarras.largo_L9_DesarrolloFoot_diamMM(10), _view3D,  ptoBusqueda + 1.5* new XYZ(0, 1, 0));           
            BuscarElementosAbajo.BuscarObjetosInferiorBArra(ptoBusqueda, new XYZ(0, 0, -1));

            ConstNH.sbLog.AppendLine($"primer item----------------------------------------------------------");
            foreach (var item in BuscarElementosAbajo.listaObjEncontrados)
            {
                ConstNH.sbLog.AppendLine($"item.elemtid:{item.elemtid}    item.nombre:{item.nombreTipo}");
            }
            Debug.Assert(BuscarElementosAbajo.listaObjEncontrados.Count == 4);
            Debug.Assert(BuscarElementosAbajo.listaObjEncontrados.First(c => c.elemtid == 833476 && c.nombreTipo == TipoElementoBArraV.losa) != null);
            Debug.Assert(BuscarElementosAbajo.listaObjEncontrados.First(c=>c.elemtid== 833238 && c.nombreTipo== TipoElementoBArraV.muro) !=null);
            Debug.Assert(BuscarElementosAbajo.listaObjEncontrados.First(c => c.elemtid == 1072754 && c.nombreTipo == TipoElementoBArraV.fundacion) != null);
            



            ConstNH.sbLog.AppendLine($"segundo item----------------------------------------------------------");
            ptoBusqueda = new XYZ(-19.6981627296588, 8.55643044619419, 10.00656167979);
            BuscarElementosBajo BuscarElementosAbajo2 = new BuscarElementosBajo(commandData.Application, UtilBarras.largo_L9_DesarrolloFoot_diamMM(10), _view3D,ptoBusqueda + 2* new XYZ(0, -1, 0));
            BuscarElementosAbajo2.BuscarObjetosInferiorBArra(new XYZ(-19.6981627296588, 8.55643044619419, 10.00656167979), new XYZ(0, 0, -1));
            Debug.Assert(BuscarElementosAbajo2.listaObjEncontrados.Count == 1);
            Debug.Assert(BuscarElementosAbajo2.listaObjEncontrados.First(c => c.elemtid == 779042 && c.nombreTipo == TipoElementoBArraV.losa) != null);

            foreach (var item in BuscarElementosAbajo2.listaObjEncontrados)
            {
                ConstNH.sbLog.AppendLine($"item.elemtid:{item.elemtid}    item.nombre:{item.nombreTipo}");
            }

            ptoBusqueda = new XYZ(-19.0288713910762, 14.2125984251968, 59.6128608923884);
            BuscarElementosBajo BuscarElementosAbajo3 = new BuscarElementosBajo(commandData.Application, UtilBarras.largo_L9_DesarrolloFoot_diamMM(10), _view3D, ptoBusqueda + 2 * new XYZ(0, -1, 0));


    
            BuscarElementosAbajo3.BuscarObjetosInferiorBArra(new XYZ(-19.0288713910762, 14.2125984251968, 59.6128608923884), new XYZ(0, 0, -1));
            Debug.Assert(BuscarElementosAbajo3.listaObjEncontrados.Count == 2);
            Debug.Assert(BuscarElementosAbajo3.listaObjEncontrados.First(c => c.elemtid == 790874 && c.nombreTipo == TipoElementoBArraV.muro) != null);
            Debug.Assert(BuscarElementosAbajo3.listaObjEncontrados.First(c => c.elemtid == 796524 && c.nombreTipo == TipoElementoBArraV.losa) != null);


            foreach (var item in BuscarElementosAbajo3.listaObjEncontrados)
            {
                ConstNH.sbLog.AppendLine($"item.elemtid:{item.elemtid}    item.nombre:{item.nombreTipo}");
            }

            //ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtosPerimetroBarras");
            //ConstantesGenerales.sbLog.AppendLine($"resul: ptoFAceMuroInicial: {resul.PtoInicioSobrePLanodelMuro} \nespesorElemeto:{ resul.EspesorElemetoHost}\nDireccionEnFierrado:{resul.DireccionEnFierrado}\nDireccionPerpendicularElemeto:  {resul.NormalInversoView}\n elemetoID:{resul.IdelementoContenedor}");
#if DEBUG
            LogNH.guardar_registro_StringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
#endif


            return Result.Succeeded;
            
        }
    }
}

