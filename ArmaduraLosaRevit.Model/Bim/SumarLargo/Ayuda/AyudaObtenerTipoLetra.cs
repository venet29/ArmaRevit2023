using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.SumarLargo.Ayuda
{
    class AyudaObtenerTipoLetra
    {

        public static bool M2_ObtenerTipoDeTExto(UIApplication _uiapp)
        {
            try
            {
                // creada 16-11-2022-- borra en furtuto
                var tipo = TiposTextNote.ObtenerTextNote(FActoryTipoTextNote.TextoSumaLargoPipe, _uiapp.ActiveUIDocument.Document); ;
                if (tipo == null)
                {
                    var ListaTipoNote = FActoryTipoTextNote.ObtenerLista_sumaLArgoPipe();
                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.TextoSumaLargoPipe, TipoCOloresTexto.Blanco);
                    _CrearTexNote.M2_CrearListaTipoText_ConTrans(ListaTipoNote);
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener tipo texto 'TextoEntreLevel'.\n ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
