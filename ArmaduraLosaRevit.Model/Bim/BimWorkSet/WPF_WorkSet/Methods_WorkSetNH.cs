using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Bim.BimWorkSet.Factory;
using System.Diagnostics;
using System.IO;

namespace ArmaduraLosaRevit.Model.Bim.BimWorkSet.WPF_WorkSet
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_WorkSetNH
    {

        public static void M1_CrearCopiaLocal(UI_WorkSetNH uI_CopiaLocal, UIApplication _uiapp)
        {

            //if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = uI_CopiaLocal.BotonOprimido;
            ManajeadorWorkSet _ManajeadorWorkSet = new ManajeadorWorkSet(_uiapp);
            uI_CopiaLocal.Hide();

            if (tipoPosiicon == "btn_CrearWorkSet_General")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaGenerales);
            else if (tipoPosiicon == "btn_CrearWorkSet_Arquitectura")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaArquitectura);
            else if (tipoPosiicon == "btn_CrearWorkSet_Estructura")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaEstructura);
            else if (tipoPosiicon == "btn_CrearWorkSet_Electricidad")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaElectricidad);
            else if (tipoPosiicon == "btn_CrearWorkSet_AguaPotable")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaAguaPotable);
            else if (tipoPosiicon == "btn_CrearWorkSet_AguaServidas")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaAguaServidas);
            else if (tipoPosiicon == "btn_CrearWorkSet_AguasLLuvias")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaAguasLLuvias);
            else if (tipoPosiicon == "btn_CrearWorkSet_Gas")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaGas);
            else if (tipoPosiicon == "btn_CrearWorkSet_Mecanico")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaMecanico);
            else if (tipoPosiicon == "btn_CrearWorkSet_Piscina")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaPiscina);
            else if (tipoPosiicon == "btn_CrearWorkSet_Pavimento")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaPavimento);
            else if (tipoPosiicon == "btn_CrearWorkSet_Entibaciones")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaEntibaciones);
            else if (tipoPosiicon == "btn_CrearWorkSet_Coordinacion")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaCoordinacion);
            else if (tipoPosiicon == "btn_CrearWorkSet_Sitio")
                _ManajeadorWorkSet.CrearLIstaWorkset_GENERALES(FactorEspecialidades.ListaSitios);
            else if (tipoPosiicon == "btn_CrearWorkSet_Info")
            {
                string ruta = @"\\Server-cdv\bim\VideoInfo\WORKSETS REQUERIDOS.xlsx";
                if (File.Exists(ruta))
                    Process.Start(ruta);
                else
                    Util.ErrorMsg($"Archivo 'WORKSETS REQUERIDOS.xlsx' no encontrado.");
            };


            uI_CopiaLocal.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}