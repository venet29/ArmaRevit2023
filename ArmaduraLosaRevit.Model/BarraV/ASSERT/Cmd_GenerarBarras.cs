#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_GenerarBarras : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;
           // Wall wall =(Wall)_doc.GetElement(new ElementId(784382));

            var view3D = TiposFamilia3D.Get3DBuscar(_doc);
            if (view3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return Result.Failed;
            }
            var view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            XYZ ptoInicialDeBorde = new XYZ(-18.881233596, 14.107611549, 26.54);

            IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO();
            interBArraDto.ViewDirectionEntradoView = new XYZ(-1.000000000, 0.000000000, 0.000000000);
            interBArraDto.DireccionPataEnFierrado = new XYZ(0.000000000, 1.000000000, 0.000000000);
            interBArraDto.tipobarraV = TipoPataBarra.BarraVSinPatas;

            interBArraDto.diametroMM = 8;
            interBArraDto.espaciamientoRecorridoBarrasFoot = (0.984251968503937 - Util.CmToFoot(ConstNH.RECUBRIMIENTO_MURO_CM) * 2.0) / 2.0;
            interBArraDto.recorridoBarrar = (0.984251968503937 - Util.CmToFoot(ConstNH.RECUBRIMIENTO_MURO_CM) * 2.0);

            interBArraDto._viewActual = _doc.ActiveView;
            interBArraDto._view3D_paraBuscar = view3D;
            interBArraDto.view3D_paraVisualizar = view3D_paraVisualizar;




            BuscarMuros BuscarMuros = new BuscarMuros(commandData.Application, Util.CmToFoot(5));
            XYZ ptobusquedaMuro = ptoInicialDeBorde.AsignarZ(28) + -interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado * ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT;
            var (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(view3D, ptobusquedaMuro, interBArraDto.ViewDirectionEntradoView);
            if(wallSeleccionado == null) return Result.Failed;  
            interBArraDto.ElementoHost = wallSeleccionado;

            interBArraDto.ptofinal = ptoInicialDeBorde.AsignarZ(44) + interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado * Util.CmToFoot(3.5);
            interBArraDto.ptoini = ptoInicialDeBorde.AsignarZ(26.54) + interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado* Util.CmToFoot(3.5);


            IbarraBase BarraVertical = new BarraVSinPatas(commandData.Application, interBArraDto, null);
            BarraVertical.M1_DibujarBarra();

            //*******************************************************
            ptobusquedaMuro = ptoInicialDeBorde.AsignarZ(46) + -interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado * ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT;
            (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(view3D, ptobusquedaMuro, interBArraDto.ViewDirectionEntradoView);
            if (wallSeleccionado == null) return Result.Failed;
            interBArraDto.ElementoHost = wallSeleccionado;

            interBArraDto.ptofinal = ptoInicialDeBorde.AsignarZ(59) + interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado * (Util.CmToFoot(3.5) + ConstNH.CONST_DESVIACION_TRASLAPOFOOT);
            interBArraDto.ptoini = ptoInicialDeBorde.AsignarZ(43) + interBArraDto.ViewDirectionEntradoView * Util.CmToFoot(2) + interBArraDto.DireccionPataEnFierrado * (Util.CmToFoot(3.5) + ConstNH.CONST_DESVIACION_TRASLAPOFOOT);

            IbarraBase BarraVertical2 = new BarraVSinPatas(commandData.Application, interBArraDto, null);
            BarraVertical2.M1_DibujarBarra();



            //ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtosPerimetroBarras");
            // ConstantesGenerales.sbLog.AppendLine($"resul: ptoFAceMuroInicial: {resul.ptoFAceMuroInicial} \nespesorElemeto:{ resul.espesorElemeto}\nDireccionEnFierrado:{resul.DireccionEnFierrado}\nDireccionPerpendicularElemeto:  {resul.DireccionPerpendicularElemeto}");
#if DEBUG
            // LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif


            return Result.Succeeded;
            
        }
    }
}

