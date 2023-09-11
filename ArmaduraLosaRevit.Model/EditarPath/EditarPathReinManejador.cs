using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;

namespace ArmaduraLosaRevit.Model.EditarPath

{
    public class EditarPathReinManejador3
    {
   
        private UIApplication _uiapp;
        private UIDocument _uidoc;

        public EditarPathReinManejador3(UIApplication uiapp)
        {

            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;

        }

        public void Ejecutar()
        {
            try
            {
                EditarPathReinForm editarPathReinForm = new EditarPathReinForm();
                editarPathReinForm.ShowDialog();


                //Ejecutar
                if (editarPathReinForm.Estado == "ok")
                {
                    EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
                    if (editarPathReinForm.tipoPosiicon == "pathToPath")
                    {
                        EditarPathReinMouse.M2_ExtenderPathaPath();
                        return;
                    }
                    if (editarPathReinForm.tipoPosiicon == "pathtoPto")
                    {
                        EditarPathReinMouse_ExtederPathApunto _EditarPathReinMouse_ExtederPathApunto =new EditarPathReinMouse_ExtederPathApunto(_uiapp);
                        _EditarPathReinMouse_ExtederPathApunto.M1_ExtederPathApunto();
                        return;
                    }
                    else
                    {
                        EditarPathReinMouse.M3_EjecutarExtenderPath( editarPathReinForm.direccion, Util.CmToFoot(editarPathReinForm.valorCm));
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'EditarPathReinManejado' ex:{ex.Message}");
            }
            return ;
        }
    }
}
