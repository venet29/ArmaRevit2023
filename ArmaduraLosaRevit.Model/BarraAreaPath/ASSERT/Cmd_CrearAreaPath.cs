#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraAreaPath.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CrearAreaPath_conptosManual : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            XYZ p1 = new XYZ(-18.946850394, 14.238845144, 26.541994751);
            XYZ p2 = new XYZ(-18.946850394, 30, 26.541994751);
            XYZ p3 = new XYZ(-18.946850394, 30, 44.717847769);
            XYZ p4 = new XYZ(-18.946850394, 14.238845144, 44.717847769);
            Curve l1 = Line.CreateBound(p1, p2);

            List<Curve> listaCurva = new List<Curve>();
            listaCurva.Add(Line.CreateBound(p1, p2));
            listaCurva.Add(Line.CreateBound(p2, p3));
            listaCurva.Add(Line.CreateBound(p3, p4));
            listaCurva.Add(Line.CreateBound(p4, p1));

            var view3D = TiposFamilia3D.Get3DBuscar(_doc);
            if (view3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return Result.Failed;
            }
            var view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            SeleccionarElementosV seleccionarElementos = new SeleccionarElementosV(commandData.Application);
            seleccionarElementos._WallSelect = (Wall)_doc.GetElement(new ElementId(784382));
            //  BarraAreasPath BarraAreasPath = new BarraAreasPath(commandData.Application, view3D_paraVisualizar, seleccionarElementos, null);

            //   BarraAreasPath.Ejecutar(listaCurva, new XYZ(0, 1, 0));

            //  ConstantesGenerales.sbLog.AppendLine($"resul: ptoFAceMuroInicial: {resul.PtoInicioSobrePLanodelMuro} \nespesorElemeto:{ resul.EspesorElemetoHost}\nDireccionEnFierrado:{resul.DireccionEnFierrado}\nDireccionPerpendicularElemeto:  {resul.NormalInversoView}\n elemetoID:{resul.IdelementoContenedor}");
#if DEBUG
            // LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif


            return Result.Succeeded;

        }
    }


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CrearAreaPath_ConSeleccion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DatosMallasAutoDTO datosMallasDTO = new DatosMallasAutoDTO()
            {
                diametroH = 8,
                diametroV = 8,
                espaciemientoH = 20,
                espaciemientoV = 20,
                tipoMallaH = TipoMAllaMuro.DM,
                tipoMallaV = TipoMAllaMuro.DM
            };
            ManejadorMallaMuro manejadorMallaMuro = new ManejadorMallaMuro(commandData.Application, datosMallasDTO);
            manejadorMallaMuro.CrearMallaMuro();
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CrearParametroCOmpartido : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            ConfiguracionInicialParametros visibilidad = new ConfiguracionInicialParametros(commandData.Application);
            visibilidad.AgregarParametrosShareLosa();
            return Result.Succeeded;

        }
    }


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CrearPAthAreaAuto : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            {

                DatosMallasAutoDTO datosMallasDTO = new DatosMallasAutoDTO()
                {
                    diametroH = 8,
                    diametroV = 8,
                    espaciemientoH = 20,
                    espaciemientoV = 20,
                    tipoMallaH = TipoMAllaMuro.DM,
                    tipoMallaV = TipoMAllaMuro.DM,
                    tipoSeleccionInf=TipoSeleccionMouse.nivel,
                    tipoSeleccionSup=TipoSeleccionMouse.nivel
                };

                List<XYZ> _ListaPtos = new List<XYZ>();
                _ListaPtos.Add(new XYZ(-80.5719, -35.00, 1905.28));
                _ListaPtos.Add(new XYZ(-80.5719, -20.00, 1913.55));

                IntervalosMallaDTOAuto _intervalosMallaDTOAuto = new IntervalosMallaDTOAuto()
                {
                    _datosMallasDTO = datosMallasDTO,
                    ListaPtos = _ListaPtos

                };

                List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto = new List<IntervalosMallaDTOAuto>();
                ListIntervalosMallaDTOAuto.Add(_intervalosMallaDTOAuto);

                ManejadorMallaMuroAuto manejadorMallaMuro = new ManejadorMallaMuroAuto(commandData.Application, ListIntervalosMallaDTOAuto);
                manejadorMallaMuro.CrearMallaMuro();
                return Result.Succeeded;

            }
        }


    }
}


