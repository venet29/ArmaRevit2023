using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.BarraV.Copiar.wpf;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Visibilidad;
using System.Linq;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.wpf;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar
{
   // [Transaction(TransactionMode.Manual)]
    public class cmd_CopiarEntreLosa : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication _uiapp = commandData.Application;
            View _view = commandData.View;

            List<View>  listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);
            List<string> listaViewName = listaView.Where(c => c.ViewType == ViewType.FloorPlan).Select(c=> c.Name).ToList();

          //  Dictionary<string, double> listaLevelZ = _CalculoValorZ.ObtenerLista_Z_Level(_uiapp, _view);

            CopiarRebarEntreLosas _CopiarRebarWpf = new CopiarRebarEntreLosas(listaViewName);
            _CopiarRebarWpf.ShowDialog();

            if (_CopiarRebarWpf.IsOk)
            {

                 //   ManejadorCopiaElevBarra _ManejadorCopiaElevBarra = new ManejadorCopiaElevBarra(_uiapp, _CopiarRebarWpf);
                  //  _ManejadorCopiaElevBarra.Ejecutar();

            }
            return Result.Succeeded;
        }
    }



}
