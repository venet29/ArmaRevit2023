using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class UtilitarioFallasDialogosCarga
    {
        private static string caso = "estatico";
        private static UtilitarioFallasAdvertencias _utilfallas;
        private static bool IsCargadorCasoEstatico = false;
        private static bool IsCargadorCasoEstatico_SuprimirCuadroDialogo = false;
        
        public static bool Cargar(UIApplication _uiapp)
        {
            try
            {
                if (caso == "estatico")
                {
                   // Util.InfoMsg("cargando estatico");
                    UtilitarioFallasAdvertencias_static.resetVaribles();
                    if (!IsCargadorCasoEstatico)
                    {
                        _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(UtilitarioFallasAdvertencias_static.ProcesamientoFallas);
                        IsCargadorCasoEstatico = true;
                    }                    
                }
                else if (caso == "claseNormal")
                {
                    if (!IsCargadorCasoEstatico)
                    {
                        //Util.InfoMsg("cargando claseNormal");
                        _utilfallas = new UtilitarioFallasAdvertencias();
                        _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                        IsCargadorCasoEstatico = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'carga Fallas de Dialogos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public static bool DesCargar(UIApplication _uiapp)
        {
            try
            {
                IsCargadorCasoEstatico = false;
                if (caso == "estatico")
                {
                  //  Util.InfoMsg("DEScargando estatico");
                    UtilitarioFallasAdvertencias_static.resetVaribles();
                 
                    _uiapp.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(UtilitarioFallasAdvertencias_static.ProcesamientoFallas);
                }
                else if (caso == "claseNormal")
                {
                    if (_utilfallas != null)
                    {
                        // Util.InfoMsg("DEScargando claseNormal");
                        _uiapp.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Descarga Fallas de Dialogos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        //  pra cuadros de dialogos
        //***********************************************************************************

        public static bool Cargar_SuprimirCuadroDialogo(UIApplication _uiapp)
        {
            try
            {
                if (caso == "estatico")
                {
          
                    if (!IsCargadorCasoEstatico_SuprimirCuadroDialogo)
                    { 
                        _uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(UtilitarioFallasAdvertencias_static.SuprimirCuadroDialogo);
                        IsCargadorCasoEstatico_SuprimirCuadroDialogo = true;
                    }
                }
                else if (caso == "claseNormal")
                {
                    if (!IsCargadorCasoEstatico_SuprimirCuadroDialogo)
                    {
                        _uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(_utilfallas.SuprimirCuadroDialogo);
                        IsCargadorCasoEstatico_SuprimirCuadroDialogo = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'carga Fallas de Dialogos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public static bool DesCargar_SuprimirCuadroDialogo(UIApplication _uiapp)
        {
            try
            {
                IsCargadorCasoEstatico_SuprimirCuadroDialogo = false;
                if (caso == "estatico")
                {                    
                    _uiapp.DialogBoxShowing -= new EventHandler<DialogBoxShowingEventArgs>(UtilitarioFallasAdvertencias_static.SuprimirCuadroDialogo);
                }
                else if (caso == "claseNormal")
                {
                    if (_utilfallas != null)
                    {                        
                        _uiapp.DialogBoxShowing -= new EventHandler<DialogBoxShowingEventArgs>(_utilfallas.SuprimirCuadroDialogo);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Descarga Fallas de Dialogos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
