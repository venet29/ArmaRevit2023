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
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using System;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_GenerarTAg : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;
           // Wall wall =(Wall)_doc.GetElement(new ElementId(784382));


            XYZ ptoIni = new XYZ(-18.946850394, 14.212598425, 51.345144357);
            XYZ ptoFin = new XYZ(-18.946850394, 14.212598425, 61.253280840);
            Element _rebar = _doc.GetElement(new ElementId(1167489));
            var _seccionView = _doc.ActiveView;

#pragma warning disable CS0219 // The variable 'nombreDefamiliaBase' is assigned but its value is never used
            string nombreDefamiliaBase = "MRA Rebar_F";
#pragma warning restore CS0219 // The variable 'nombreDefamiliaBase' is assigned but its value is never used
            var _newGeometriaTag = FactoryGeomTagRebarV.CrearGeometriaTagV(commandData.Application, TipoPataBarra.BarraVSinPatas , ptoIni, ptoFin, ptoFin- new XYZ(0,0,3), new XYZ(0,-2,0));
            _newGeometriaTag.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });

            ConfiguracionTAgBarraDTo confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                IsDIrectriz = true,
                LeaderElbow = new XYZ(0, 0, 1.5),
                tagOrientation = TagOrientation.Vertical
            };
            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("creando tag-NH");
                    if (_newGeometriaTag.M4_IsFAmiliaValida())
                    {
                        foreach (TagBarra item in _newGeometriaTag.listaTag)
                        {
                            if (item == null) continue;
                            if (!item.IsOk) continue;
                            item.DibujarTagRebarV(_rebar, commandData.Application, _seccionView, confBarraTag);
                            // if (item.ElementIndependentTagPath != null) listaGrupo_Tag.Add(item.ElementIndependentTagPath.Id);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception)
            {
               
             
            }

            //ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtosPerimetroBarras");
            // ConstantesGenerales.sbLog.AppendLine($"resul: ptoFAceMuroInicial: {resul.ptoFAceMuroInicial} \nespesorElemeto:{ resul.espesorElemeto}\nDireccionEnFierrado:{resul.DireccionEnFierrado}\nDireccionPerpendicularElemeto:  {resul.DireccionPerpendicularElemeto}");
#if DEBUG
            // LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif


            return Result.Succeeded;
            
        }
    }
}

