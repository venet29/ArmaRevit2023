#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    //[Transaction(TransactionMode.Manual)]
    //[Regeneration(RegenerationOption.Manual)]
    //public class cmd_ManejadorBarraH : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {


    //        ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(commandData.Application);
    //        ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO = new ConfiguracionInicialBarraHorizontalDTO() {
    //            Inicial_Cantidadbarra = "2+2+2",
    //            incial_ComoIniciarTraslapo_LineaPAr = 1,
    //            incial_ComoIniciarTraslapo_LineaImpar = 2,//barra incio
    //            incial_ComoTraslapo = 2,
    //            incial_diametroMM = 22,
    //            inicial_tipoBarraH = Enumeraciones.TipoPataBarra.BarraVSinPatas,
    //            incial_IsDirectriz = false,
    //            incial_ISIntercalar=false,
    //           Inicial_espacienmietoCm_direccionmuro="6"
    //        };
        
    //        ManejadorBarraH ManejadorBarraH = new ManejadorBarraH(commandData.Application, _seleccionarNivel, confiEnfierradoDTO);
    //        ManejadorBarraH.CrearBArraHorizontalTramov2();
    //        //  List<Level> list = _seleccionarNivel.ObtenerListaNivelPOrelevacionNombre(uiapp.ActiveUIDocument);

    //        return Result.Succeeded;

    //    }
    //}
}

