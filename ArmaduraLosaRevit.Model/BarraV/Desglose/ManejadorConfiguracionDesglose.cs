using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose
{
    public class ManejadorConfiguracionDesglose
    {

        public static void cargar(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            View _view = _doc.ActiveView;

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
          //  _ManejadorUsuarios.PostInscripcion("NHdelporte");


            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                    DefinirArchivoShare.Ejecutar(_uiapp);

                   //1-parametros compartidos
                    ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicial.AgregarParametrosShareDesglose();

                  


                    transGroup.Assimilate();
                }
                if (IsMje) Util.InfoMsg("Datos cargados correctamente");

            }
            catch (Exception ex)
            {
                 
                Util.InfoMsg($"Error al cargar parametros ex:{ex.Message}");
            }

        
        }
    }
}
