using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.UTILES.Cambiar
{
    public  class CambiarUnobscuredInView_path_rebar
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _View;
        private SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad;
        private SeleccionarRebarElemento _SeleccionarRebarElemento;
        private SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad;

        public CambiarUnobscuredInView_path_rebar(UIApplication uiapp,View view)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._View = view;
            this._SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _View);
            this._SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _View);
            this._seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _View);
        }

        public bool Ejecutar()
        {
            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarUnobscuredInView-NH");
                    EjecutarCambiarUnobscuredInView();
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool EjecutarCambiarUnobscuredInView()
        {
            var view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            if (!_SelecPathReinVisibilidad.M1_ejecutar()) return false;
            if (!_seleccionarRebarVisibilidad.M1_Ejecutar_ConrebarSystem()) return false;

            try
            {

                if (view3D_Visualizar != null)
                {
                    foreach (Rebar item in _seleccionarRebarVisibilidad._lista_A_TodasRebarNivelActual_MENOSRebarSystem)
                    {
                        item.SetSolidInView(view3D_Visualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        item.SetUnobscuredInView(view3D_Visualizar, true);
                    }


                    foreach (PathReinforcement item in _SelecPathReinVisibilidad._lista_A_DePathReinfNivelActual)
                    {
                        item.SetSolidInView(view3D_Visualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        item.SetUnobscuredInView(view3D_Visualizar, true);
                    }
                    //permite que la barra se vea en el 3d como solido

                }

            }
            catch (Exception)
            {

                return true;
            }
            return true;
        }
    }
}
