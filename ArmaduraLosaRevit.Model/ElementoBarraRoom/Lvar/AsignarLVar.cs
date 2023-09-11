using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Lvar
{
    public class AsignarLVar
    {
        private UIApplication _uiapp;
        private SeleccionarPathReinforment_InfoCompleta _SeleccionarPathReinformentParaCopiar;

        public AsignarLVar(UIApplication uiapp)
        {
            _uiapp = uiapp;
        }

        public void Ejecutar()
        {
            try
            {
                _SeleccionarPathReinformentParaCopiar = new SeleccionarPathReinforment_InfoCompleta(_uiapp);

                if (!_SeleccionarPathReinformentParaCopiar.GenerarListaSeleccionado()) return;

                foreach (ElementoPathRein item in _SeleccionarPathReinformentParaCopiar.ListaElementoPathRein)
                {
                    item.ListTagpath.Where(c => c.Name == "");
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en cambiar largo de parametro ex:{ex.Message}");
                
            }
        
        }
    }
}
