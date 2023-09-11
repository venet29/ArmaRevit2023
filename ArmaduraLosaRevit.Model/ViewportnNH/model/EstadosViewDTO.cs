using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewportnNH.model
{
    public class EstadosViewDTO
    {
        public string asas  { get; set; }
        public string TipoEstructura { get; private set; }
        public string EstadoDeAvance { get; private set; }
        public string CurrentRevision { get; private set; }
        public string CODIGOESPECIALIDAD { get; private set; }
        public string CODIGOESTADODEAVANCE { get; private set; }
        public string ESPECIALIDAD { get; private set; }
        public View View_ { get; }

        public EstadosViewDTO(View _view)
        {
            View_ = _view;
        }
        public bool ObtenerDatos()
        {

            try
            {
                if (View_ == null)
                    return false;

                TipoEstructura = ParameterUtil.FindValueParaByName(View_, "TIPO DE ESTRUCTURA (VISTA)");
                EstadoDeAvance = ParameterUtil.FindValueParaByName(View_, "ESTADO DE AVANCE");
                CurrentRevision = ParameterUtil.FindValueParaByName(View_, "Current Revision");
                CODIGOESPECIALIDAD = ParameterUtil.FindValueParaByName(View_, "CODIGO ESPECIALIDAD");
                CODIGOESTADODEAVANCE = ParameterUtil.FindValueParaByName(View_, "CODIGO ESTADO DE AVANCE");
                ESPECIALIDAD = ParameterUtil.FindValueParaByName(View_, "ESPECIALIDAD");
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
    }

 
}
