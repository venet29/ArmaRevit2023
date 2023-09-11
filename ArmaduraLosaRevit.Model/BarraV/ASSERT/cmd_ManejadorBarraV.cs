#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using System.Collections.Generic;
using System;
using ArmaduraLosaRevit.Model.BarraV.Automatico;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    //[Transaction(TransactionMode.Manual)]
    //[Regeneration(RegenerationOption.Manual)]
    //public class cmd_ManejadorBarraV : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {


    //        ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(commandData.Application);
    //        ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO = new ConfiguracionIniciaWPFlBarraVerticalDTO()
    //        {
    //            Inicial_Cantidadbarra = "3",
    //            incial_ComoIniciarTraslapo_LineaPAr = 1,
    //            incial_ComoIniciarTraslapo_LineaImpar = 2,//barra incio
    //            incial_ComoTraslapo = 2,
    //            Document_ = commandData.Application.ActiveUIDocument.Document,
    //            incial_diametroMM = 22,
    //            inicial_tipoBarraV = Enumeraciones.TipoPataBarra.BarraVSinPatas,
    //            incial_IsDirectriz = false,
    //            incial_ISIntercalar = false,
    //            Inicial_espacienmietoCm_EntreLineasBarras = "15",
    //            IsDibujarTag = true,
    //            TipoBarraRebar_ = TipoBarraVertical.Cabeza
    //        };

    //        //configuracion barra verticales cabeza muro
    //        DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(commandData.View, DireccionRecorrido_.PerpendicularEntradoVista);

    //        ManejadorBarraV ManejadorBarraV = new ManejadorBarraV(commandData.Application, _seleccionarNivel, confiEnfierradoDTO, _DireccionRecorrido);
    //        ManejadorBarraV.CrearBArraVErtical();
    //        //  List<Level> list = _seleccionarNivel.ObtenerListaNivelPOrelevacionNombre(uiapp.ActiveUIDocument);

    //        return Result.Succeeded;

    //    }
    //}



    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class cmd_ManejadorBarraV_cambiar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            EditarBarraDTO newEditarBarraDTO = new EditarBarraDTO() { 
            cantidad=2,
            diametro=16,
            tipobarraV=Enumeraciones.TipoPataBarra.BarraVPataAmbos,
            

            };


            ManejadorBarraV_CambiarBarra ManejadorBarraV_CambiarBarra = new ManejadorBarraV_CambiarBarra(commandData.Application, newEditarBarraDTO);
            ManejadorBarraV_CambiarBarra.CambiarFormaBarra();
            //  List<Level> list = _seleccionarNivel.ObtenerListaNivelPOrelevacionNombre(uiapp.ActiveUIDocument);

            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class cmd_ManejadorBarraV_cambiarRebarShape : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(commandData.Application);

            EditarBarraDTO newEditarBarraDTO = new EditarBarraDTO()
            {
                cantidad = 2,
                diametro = 16,
                tipobarraV = Enumeraciones.TipoPataBarra.BarraVPataAmbos,
            };


            ManejadorBarra_CambiarBarraRebarShape ManejadorBarraV_CambiarBarra = new ManejadorBarra_CambiarBarraRebarShape(commandData.Application, newEditarBarraDTO);
            ManejadorBarraV_CambiarBarra.CambiarBarraRebarShape();
            //  List<Level> list = _seleccionarNivel.ObtenerListaNivelPOrelevacionNombre(uiapp.ActiveUIDocument);

            return Result.Succeeded;

        }
    }
    

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class cmd_ManejadorBarraVAuto : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ManejadorBarrasAutomaticas manejadorBarrasAutomaticas = new ManejadorBarrasAutomaticas(commandData.Application);
            manejadorBarrasAutomaticas.EjecutarImportacion();

            return Result.Succeeded;

        }

        private IntervalosBarraAutoDto DatosLaDosDere()
        {
            return new IntervalosBarraAutoDto()
            {
                PtoBordeMuro = new XYZ(-82.385, -29.461, 46.313),
                PtoCentralSobreMuro = new XYZ(-86.00, -29.461, 46.313),

                Inicial_Cantidadbarra = 2,
                Inicial_diametroMM = 12,
                ListaCoordenadasBarra = new List<CoordenadasBarra>()
                                {
                                    new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-82.5, -29.461, 10.335),
                                        ptoFin_foot=new XYZ(-82.5, -29.461, 18.537),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    }
                                    ,
                                    new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-82.5, -29.461,34.941),
                                        ptoFin_foot=new XYZ(-82.5, -29.461,  43.143),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas,
                                        IsNoProloganLosaArriba=true,
                                        IsProloganLosaBajo=false
                                    }
                                    ,
                                    new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-82.5, -29.461, 43.143),
                                        ptoFin_foot=new XYZ(-82.5, -29.461, 51.345),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas,
                                           IsNoProloganLosaArriba=false,
                                        IsProloganLosaBajo=true
                                    }

                    }
            };
        }

        private IntervalosBarraAutoDto DatosLaDosIzq()
        {
            return new IntervalosBarraAutoDto()
            {
                PtoBordeMuro = new XYZ(-87.385, -29.461, 46.313),
                PtoCentralSobreMuro = new XYZ(-86.00, -29.461, 46.313),

                Inicial_Cantidadbarra = 2,
                Inicial_diametroMM = 12,
                ListaCoordenadasBarra = new List<CoordenadasBarra>()
                                {
                                    new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-87.0, -29.461, 10.335),
                                        ptoFin_foot=new XYZ(-87.0, -29.461, 18.537),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    },
                                    new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-87.0, -29.461, 43.143),
                                        ptoFin_foot=new XYZ(-87.0, -29.461, 51.345),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    },
                                     new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-87.0, -29.461,51.345),
                                        ptoFin_foot=new XYZ(-87.0, -29.461,59.547 ),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    },
                                      new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-87.0, -29.461, 59.547),
                                        ptoFin_foot=new XYZ(-87.0, -29.461, 67.749),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    },
                                       new CoordenadasBarra()
                                    {
                                        ptoIni_foot= new XYZ(-87.0, -29.461, 67.749),
                                        ptoFin_foot=new XYZ(-87.0, -29.461, 75.951),
                                        tipoBarraV=Enumeraciones.TipoPataBarra.BarraVSinPatas
                                    }
                                }
            };
        }



    }

}

