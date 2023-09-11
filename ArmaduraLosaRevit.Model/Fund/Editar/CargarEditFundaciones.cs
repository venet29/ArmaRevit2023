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

namespace ArmaduraLosaRevit.Model.Fund.Editar.WPF
{

    /// <summary>
    /// piede seleccionar la barra para editar, luego caga el ManejadorWPF_EditFund.
    /// 
    /// -CargarEditFundaciones.cs
    /// --ManejadorWPF_EditFund.cs
    /// ---UI_FundEditar.xaml
    /// ----Methods_EditFund.cs
    /// 
    /// </summary>
    public class CargarEditFundaciones
    {

        private readonly UIApplication _uiapp;
        private readonly TabEditarPath tabEditarPat;

        public CargarEditFundaciones(UIApplication uiapp, TabEditarPath _TabEditarPat)
        {
       
            _uiapp = uiapp;
            tabEditarPat = _TabEditarPat;
        }


        public Result Cargar()
        {
            try
            {

                Debug.Print("probando debug");
                Element el = Util.GetSingleSelectedElement(_uiapp.ActiveUIDocument);
                SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto;
                if (el is PathReinSpanSymbol)
                {
                    seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp.ActiveUIDocument, _uiapp.Application);
                    if (!seleccionarPathReinfomentConPto.AsignarPathReinformentSymbolFund(el))
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

                ManejadorWPF_EditFund manejadorWPF = new ManejadorWPF_EditFund(_uiapp, seleccionarPathReinfomentConPto, tabEditarPat);
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
