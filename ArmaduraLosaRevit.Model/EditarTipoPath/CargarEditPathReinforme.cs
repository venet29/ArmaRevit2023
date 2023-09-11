using ArmaduraLosaRevit.Model.EditarTipoPath.WPF;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarTipoPath
{
    public class CargarEditPathReinforme
    {
        private ExternalCommandData commandData;
        private readonly TabEditarPath tabEditarPat;

        public CargarEditPathReinforme(ExternalCommandData commandData, TabEditarPath _TabEditarPat)
        {
            this.commandData = commandData;
            tabEditarPat = _TabEditarPat;
        }


        public Result Cargar()
        {
            try
            {

                Debug.Print("probando debug");
                Element el = Util.GetSingleSelectedElement(commandData);
                SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto;
                if (el is PathReinSpanSymbol)
                {
                    seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(commandData.Application.ActiveUIDocument, commandData.Application.Application);
                    if (!seleccionarPathReinfomentConPto.AsignarPathReinformentSymbol(el))
                    {
                        //Util.ErrorMsg("Error al obtener parametros del PathReinforment previamente selecionado");
                        seleccionarPathReinfomentConPto = null;
                        return Result.Failed;
                    }

                    if (seleccionarPathReinfomentConPto.PathReinforcement == null) seleccionarPathReinfomentConPto = null;
                }
                else
                {
                    seleccionarPathReinfomentConPto = null;
                }

                ManejadorWPF manejadorWPF = new ManejadorWPF(commandData, seleccionarPathReinfomentConPto, tabEditarPat);
                return manejadorWPF.Execute();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return Result.Failed;
            }
        }
    }
}
