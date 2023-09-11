using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Servicio
{
    public class ManejadorResetText
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public ManejadorResetText(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool Ejecutar()
        {

            try
            {

                SeleccionarPathSymbol _SeleccionarPathSymbol = new SeleccionarPathSymbol(_uiapp);
                if (!_SeleccionarPathSymbol.Seleccionados1Path()) return false;



                PathReinSpanSymbol _independentTag = _SeleccionarPathSymbol.PathReinforcementSymbol;
                if (_independentTag == null) return false;

                PathReinforcement PathRein = _doc.GetElement(_independentTag.Obtener_GetTaggedLocalElementID()) as PathReinforcement;
                if (PathRein == null) return false;

                List<IndependentTag> listaIndependentTag = TiposPathReinTagsEnView.M1_GetFamilySymbol_ConPathReinforment(PathRein.Id, _doc, _doc.ActiveView.Id);
                if (listaIndependentTag.Count == 0) return false;


                ManejarEditTagUpdate_MoverConfiguracionInicial _ManejarEditTagUpdate_move = new ManejarEditTagUpdate_MoverConfiguracionInicial(_uiapp, PathRein, _independentTag.TagHeadPosition, listaIndependentTag);
                _ManejarEditTagUpdate_move.Ejecutar_Contrans();

            }
#pragma warning disable CS0168 // The variable 'Ex' is declared but never used
            catch (Exception Ex)
#pragma warning restore CS0168 // The variable 'Ex' is declared but never used
            {

                throw;
            }
            return true;
        }
    }
}
