using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class GenerarIntervalosNULL : AGenerarIntervalosV, IGenerarIntervalosV
    {


        public GenerarIntervalosNULL(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiEnfierradoDTO,  selecionarPtoSup, muroSeleccionadoDTO)
        {

        }



        public override void M1_ObtenerIntervaloBarrasDTO()
        {
          //  return new List<IntervaloBarrasDTO>();
        }

        public override List<IbarraBase> M2_GenerarListaBarraVertical()
        {
            ListaIbarraVertical = new List<IbarraBase>();
            return ListaIbarraVertical;
        }
    }
}
