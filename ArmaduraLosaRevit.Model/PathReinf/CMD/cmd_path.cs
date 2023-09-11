using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.PathReinf.Barras;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.PathReinf.CMD
{


    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Crear_PathReinf_nh_4lados : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication _uiapp = commandData.Application;
            Document _doc = _uiapp.ActiveUIDocument.Document;


            DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
            {
                DiametroMM = 8,
                EspaciamientoFoot = Util.CmToFoot(20),
                TipoCaraObjeto_ = TipoCaraObjeto.Inferior
            };

            //dato iniales
            Element fund = _doc.GetElement(new ElementId(1078189));

            List<XYZ> ListaPtosPerimetroBarras = new List<XYZ>();
            XYZ p1 = new XYZ(-39, -36, -50);
            XYZ p2 = new XYZ(-39, -43, -50);
            XYZ p3 = new XYZ(-36, -43, -50);
            XYZ p4 = new XYZ(-36, -36, -50);
            ListaPtosPerimetroBarras.Add(p1);
            ListaPtosPerimetroBarras.Add(p2);
            ListaPtosPerimetroBarras.Add(p3);
            ListaPtosPerimetroBarras.Add(p4);
            // vertical    _     
            //p1    p4      |
            //p2    p3     _|
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);

            // vertical      _     
            //p1    p4      |
            //p2    p3      |_
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);

            //*******************************************************************************************************************************************

            ListaPtosPerimetroBarras.Clear();
            
            ListaPtosPerimetroBarras.Add(p2);
            ListaPtosPerimetroBarras.Add(p3);
            ListaPtosPerimetroBarras.Add(p4);
            ListaPtosPerimetroBarras.Add(p1);
            //horizo
            //  p4   p3     |________|
            //  p1   p2     
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);



            //horizo         ________
            //  p4   p3     |        |
            //  p1   p2     
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);
            return Result.Succeeded;
        }

    
    }

    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Crear_PathReinf_nh_vertical : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication _uiapp = commandData.Application;
            Document _doc = _uiapp.ActiveUIDocument.Document;


            DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
            {
                DiametroMM = 8,
                EspaciamientoFoot = Util.CmToFoot(20),
                TipoCaraObjeto_ = TipoCaraObjeto.Inferior
            };

            //dato iniales
            Element fund = _doc.GetElement(new ElementId(1078189));

            List<XYZ> ListaPtosPerimetroBarras = new List<XYZ>();
            XYZ p1 = new XYZ(-39, -36, -50);
            XYZ p2 = new XYZ(-39, -43, -50);
            XYZ p3 = new XYZ(-36, -43, -50);
            XYZ p4 = new XYZ(-36, -36, -50);
            ListaPtosPerimetroBarras.Add(p1);
            ListaPtosPerimetroBarras.Add(p2);
            ListaPtosPerimetroBarras.Add(p3);
            ListaPtosPerimetroBarras.Add(p4);
            // vertical    _     
            //p1    p4      |
            //p2    p3     _|
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);

            // vertical      _     
            //p1    p4      |
            //p2    p3      |_
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);

            //*******************************************************************************************************************************************


            return Result.Succeeded;
        }


    }

    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Crear_PathReinf_nh_Horizontal : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication _uiapp = commandData.Application;
            Document _doc = _uiapp.ActiveUIDocument.Document;

            DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
            {
                DiametroMM = 8,
                EspaciamientoFoot = Util.CmToFoot(20),
                TipoCaraObjeto_ = TipoCaraObjeto.Inferior
            };

            //dato iniales
            Element fund = _doc.GetElement(new ElementId(1078189));

            List<XYZ> ListaPtosPerimetroBarras = new List<XYZ>();
            XYZ p1 = new XYZ(-39.5, -35.54, -50);
            XYZ p2 = new XYZ(-39.5, -44.6, -50);
            XYZ p3 = new XYZ(-35.6, -44.6, -50);
            XYZ p4 = new XYZ(-35.6, -35.54, -50);


            ListaPtosPerimetroBarras.Add(p2);
            ListaPtosPerimetroBarras.Add(p3);
            ListaPtosPerimetroBarras.Add(p4);
            ListaPtosPerimetroBarras.Add(p1);
            //horizo
            //  p4   p3     |________|
            //  p1   p2     
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);

            //horizo         ________
            //  p4   p3     |        |
            //  p1   p2     
            _datosNuevaBarraDTOIniciales.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
            CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales);


            return Result.Succeeded;
        }
    }

}

