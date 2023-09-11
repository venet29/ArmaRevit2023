using System;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Estructuras
{
    internal class CrearVigasConSacados
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private View3D _view3D_BUSCAR;
        private FamilySymbol famili;

        public CrearVigasConSacados(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }

        internal bool Crear()
        {

            try
            {
                _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);

                if (_view3D_BUSCAR == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial");
                    return false;
                }

                 famili = TiposGenericFamilySymbol.M1_GenericFamilySymbol_nh("VIGA CON SACADO", BuiltInCategory.OST_StructuralFraming, _doc);
                if (famili == null)
                {
                    Util.ErrorMsg("Error, no se encuentra familia 'VIGA CON SACADO'");
                    return false;
                }

                //seleccionar viga
                SeleccionarViga _SeleccionarViga = new SeleccionarViga();

                if (!_SeleccionarViga.M1_3_SeleccionarVariasViga(_uiapp.ActiveUIDocument)) return false;


                for (int i = 0; i < _SeleccionarViga.listAvigas.Count; i++)
                {
              
                   var viga = _SeleccionarViga.listAvigas[i] as FamilyInstance;

                    if (CrearVigaConsacado(viga)) continue;

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearVigasConSacados'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool CrearVigaConsacado(Element viga)
        {
            try
            {
               var loca = viga.Location as LocationCurve;


                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CrearPelotaLosaEstructural_aux-NH");
                    //agrega la annotation generico de pelota de losa 
                    FamilyInstance familyInstance = _doc.Create.NewFamilyInstance(loca.Curve, famili, viga.Level2() as Level,StructuralType.Beam);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}