using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.PathFalso
{
    public class GenerarPtos
    {
        private readonly UIApplication _uiapp;
        private List<XYZ> _listaPtos;
        private List<XYZ> _listaPtosPerimetroBarras_;

        public GenerarPtos(UIApplication uiapp, List<XYZ> listaPtos)
        {
            this._uiapp = uiapp;
            this._listaPtos = listaPtos;
            this._listaPtosPerimetroBarras_ = new List<XYZ>();
        }

        public RebarInferiorDTO Ejecutar()
        {
            RebarInferiorDTO rebarInferiorDTO = new RebarInferiorDTO(_uiapp);

            try
            {
                rebarInferiorDTO.listaPtosPerimetroBarras.AddRange(_listaPtos);

                rebarInferiorDTO.barraIni = _listaPtosPerimetroBarras_[0];
                rebarInferiorDTO.barraFin = _listaPtosPerimetroBarras_[1];
                rebarInferiorDTO.PtoDirectriz1 = _listaPtosPerimetroBarras_[2];
                rebarInferiorDTO.PtoDirectriz2 = _listaPtosPerimetroBarras_[3];
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return new RebarInferiorDTO(_uiapp);

            }
            return rebarInferiorDTO;

        }
    }
}
