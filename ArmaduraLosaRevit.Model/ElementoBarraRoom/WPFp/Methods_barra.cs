using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.WPF;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.UpdateGenerar;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_barra
    {
        /// <summary>
        /// Method for collecting sheets as an asynchronous operation on another thread.
        /// </summary>
        /// <param name="doc">The Revit Document to collect sheets from.</param>

   


        public static void M1_EjecutarRutinas(Ui_barra ui_pelotaLosa, UIApplication uiapp)

        {

            try
            {
            string tipoPosiicon = ui_pelotaLosa.ImageOprimido;
            ui_pelotaLosa.Hide();
            if (verificarBArrasEscaleras(uiapp, tipoPosiicon)) { };
            if (verificarBArrasInclinada(uiapp, tipoPosiicon, ui_pelotaLosa)) { };
            ui_pelotaLosa.Show();
            }
            catch (Exception)
            {

                ui_pelotaLosa.Show();
                ui_pelotaLosa.Close();
            }
            //CargarCambiarPathReinfomenConPto_Wpf
        }

        private static bool verificarBArrasInclinada(UIApplication _uiapp, string tipoPosiicon, Ui_barra ui_pelotaLosa)
        {
#pragma warning disable CS0219 // The variable '_ubicacionPtoMouse' is assigned but its value is never used
            UbicacionPtoMouse _ubicacionPtoMouse = UbicacionPtoMouse.superior;
#pragma warning restore CS0219 // The variable '_ubicacionPtoMouse' is assigned but its value is never used
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            bool result = false;

            ManejadorRebarLosaInclinada barraManejador = new ManejadorRebarLosaInclinada(_uiapp);
            switch (tipoPosiicon)
            {
                case "f1_LosaInclinadaIzq_b2":
                case "f1_LosaInclinadaIzq_b":
                    barraManejador.BarraInferiores(TipoBarra.f1_incliInf, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f1_LosaInclinadaDer_b2":
                case "f1_LosaInclinadaDer_b":
                    barraManejador.BarraInferiores(TipoBarra.f1_incliInf, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f1_LosaInclinadaInf_b2":
                case "f1_LosaInclinadaInf_b":
                    barraManejador.BarraInferiores(TipoBarra.f1_incliInf, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f1_LosaInclinadaSup_b2":
                case "f1_LosaInclinadaSup_b":
                    barraManejador.BarraInferiores(TipoBarra.f1_incliInf, UbicacionLosa.Superior);
                    result = true;
                    break;

                case "f1_LosaInclinadaIzq_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f1_incliInf, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f1_LosaInclinadaDer_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f1_incliInf, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f1_LosaInclinadaInf_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f1_incliInf, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f1_LosaInclinadaSup_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f1_incliInf, UbicacionLosa.Superior);
                    result = true;
                    break;
                //*******************
                case "f3_LosaInclinadaDer_b":
                case "f3_LosaInclinadaIzq_b":
                    barraManejador.BarraInferiores(TipoBarra.f3_incli, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f3_LosaInclinadaInf_b":
                case "f3_LosaInclinadaSup_b":
                    barraManejador.BarraInferiores(TipoBarra.f3_incli, UbicacionLosa.Inferior);
                     result = true;
                    break;
                case "f3_LosaInclinadaH_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f3_incli, UbicacionLosa.Izquierda);
                    result = true;
                    break;

                case "f3_LosaInclinadaV_b_ahorro":
                    barraManejador.BarraInferiores_ConAhorrro(TipoBarra.f3_incli, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f3_LosaInclinadaH_multiple_Sup":
                    barraManejador.BarraInferiores_MUltiples(TipoBarra.f3_incli, UbicacionLosa.Izquierda, UbicacionPtoMouse.superior, (bool)ui_pelotaLosa.rbt_caso_intervalo.IsChecked);
                    result = true;
                    break;
                case "f3_LosaInclinadaH_multiple_inf":
                    barraManejador.BarraInferiores_MUltiples(TipoBarra.f3_incli, UbicacionLosa.Izquierda, UbicacionPtoMouse.inferior, (bool)ui_pelotaLosa.rbt_caso_intervalo.IsChecked);
                    result = true;
                    break;
                case "f3_LosaInclinadaV_multiple_sup":
                    barraManejador.BarraInferiores_MUltiples(TipoBarra.f3_incli, UbicacionLosa.Inferior, UbicacionPtoMouse.superior, (bool)ui_pelotaLosa.rbt_caso_intervalo.IsChecked);
                    result = true;
                    break;
                case "f3_LosaInclinadaV_multiple_Inf":
                    barraManejador.BarraInferiores_MUltiples(TipoBarra.f3_incli, UbicacionLosa.Inferior, UbicacionPtoMouse.inferior, (bool)ui_pelotaLosa.rbt_caso_intervalo.IsChecked);
                    result = true;
                    break; 

                //*******************
                case "f4_LosaEspesorVarH":
                case "f4_LosaInclinadaH_b":
                    barraManejador.BarraInferiores(TipoBarra.f4_incli, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f4_LosaEspesorVarV":
                case "f4_LosaInclinadaV_b":
                    barraManejador.BarraInferiores(TipoBarra.f4_incli, UbicacionLosa.Inferior);
                    result = true;
                    break;
                //*******************
                case "s4_InclinadaIzq":   
                    barraManejador.BarraInferiores(TipoBarra.s4_Inclinada, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "s4_InclinadaInf":
                    barraManejador.BarraInferiores(TipoBarra.s4_Inclinada, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "s4_InclinadaDere":
                    barraManejador.BarraInferiores(TipoBarra.s4_Inclinada, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "s4_InclinadaSup":
                    barraManejador.BarraInferiores(TipoBarra.s4_Inclinada, UbicacionLosa.Superior);
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            UpdateGeneral.M2_CargarBArras(_uiapp);
            return result;
        }

        private static bool verificarBArrasEscaleras(UIApplication _uiapp, string tipoPosiicon)
        {
            ManejadorRebarLosaInclinada barraManejador = new ManejadorRebarLosaInclinada(_uiapp);

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            bool result = false;
            switch (tipoPosiicon)
            {
                case "f1_EscInfIzq":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc45_conpata, UbicacionLosa.Izquierda);
                    result= true;
                    break;
                case "f1_EscInfDer":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc45_conpata, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f1_EscInfInf":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc45_conpata, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f1_EscInfSup":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc45_conpata, UbicacionLosa.Superior);
                    result = true;
                    break;

                //*******************
                case "f1_EscSupIzq":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc135_sinpata, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f1_EscSupDer":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc135_sinpata, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f1_EscSupInf":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc135_sinpata, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f1_EscSupSup":
                    barraManejador.BarraInferiores(TipoBarra.f1_esc135_sinpata, UbicacionLosa.Superior);
                    result = true;
                    break;

                //*******************
                case "f3_EscSupIzq":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc135, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f3_EscSupDer":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc135, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f3_EscSupInf":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc135, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f3_EscSUpSup":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc135, UbicacionLosa.Superior);
                    result = true;
                    break;
                //*******************
                case "f3_EscInfIzq":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc45, UbicacionLosa.Izquierda);
                    result = true;
                    break;
                case "f3_EscInfDer":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc45, UbicacionLosa.Derecha);
                    result = true;
                    break;
                case "f3_EscInfInf":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc45, UbicacionLosa.Inferior);
                    result = true;
                    break;
                case "f3_EscInfSup":
                    barraManejador.BarraInferiores(TipoBarra.f3_esc45, UbicacionLosa.Superior);
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
            return result;
        }


    }
}