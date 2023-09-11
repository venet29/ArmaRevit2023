using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo
{
    public class PlanoParaUbicDimensiones
    {
        private readonly UIApplication _uiapp;

        

        public EnvoltoriosPlanos _planoPasada { get; set; }
        public ElementId Id_Pasada { get; }
        public TipoCara TipoCara_ { get;  set; }
        public bool IsOk { get;  set; }
        public bool IsIntersectado { get; set; }
        public DimensionParaUbicDimensiones Dimension_LadoIzqInf { get; private set; }
        public DimensionParaUbicDimensiones Dimension_LadoDereSup { get; private set; }
        public double MaximoLargo { get; internal set; }

        public PlanoParaUbicDimensiones(Autodesk.Revit.UI.UIApplication _uiapp, EnvoltoriosPlanos planoPasada, Autodesk.Revit.DB.ElementId id_Pasada)
        {
            this._uiapp = _uiapp;
            this._planoPasada = planoPasada;
            Id_Pasada = id_Pasada;
            TipoCara_ = _planoPasada.TipoCara_;
            IsOk = false;
            IsIntersectado = false;
        }

  
        public bool ObtenerDatos()
        {
            try
            {
                Dimension_LadoIzqInf = new DimensionParaUbicDimensiones(_uiapp, _planoPasada, Id_Pasada);
                if (!Dimension_LadoIzqInf.CalcularPtos_Izq())
                    return false;

                Dimension_LadoDereSup = new DimensionParaUbicDimensiones(_uiapp, _planoPasada, Id_Pasada);
                if (!Dimension_LadoDereSup.CalcularPtos_Dere())
                    return false;

                MaximoLargo = Math.Max(Dimension_LadoIzqInf.MaximoLargo, Dimension_LadoDereSup.MaximoLargo);

                IsOk = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerDatos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool DIbujarRectagulo(string nobreLinea)
        {
            try
            {
                if(Dimension_LadoIzqInf.EstadoIteracion_==EstadoIteracion.PermitidoIterar) 
                    Dimension_LadoIzqInf.DibujarLIneas_sinTransa(nobreLinea);

                if(Dimension_LadoDereSup.EstadoIteracion_==EstadoIteracion.PermitidoIterar)
                    Dimension_LadoDereSup.DibujarLIneas_sinTransa(nobreLinea);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarRectagulo'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
