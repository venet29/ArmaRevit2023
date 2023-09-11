using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GRIDS.NombreEje

{
    public class MAnejadorCrearTextoEje
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public MAnejadorCrearTextoEje(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }

        public bool GenerarTexto(string texto = "")
        {

            try
            {
                SeleccionarGrid _SeleccionarGrid = new SeleccionarGrid(_uiapp);
                bool seguir = true;
                while (seguir)
                {
                    if (!_SeleccionarGrid.GetSelecionarGrid()) return false;

                    if (texto == "") texto = "VER ELEVACION EJE ";

                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.AcotarBarra, TipoCOloresTexto.rojo);
                    XYZ ptoInsercion = _SeleccionarGrid.PuntoSeleccionMouse + -_view.RightDirection * Util.CmToFoot(25);
                    _CrearTexNote.M1_CrearConTrans(ptoInsercion, $"{texto} {_SeleccionarGrid._grids.Name}", Math.PI / 2);
                }



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
    }
}
