using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaEstructural
{
    public class ManejadorDuplicarLosas
    {
        private readonly UIApplication uiapp;
        private Document _doc;

        public ManejadorDuplicarLosas(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }


        public bool Duplicar()
        {
            try
            {
                SeleccionarLosa _SeleccionarLosa = new SeleccionarLosa(uiapp);
                if (!_SeleccionarLosa.ObtenerTodosLosFloot()) return false;

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    var listaCOpia = ElementTransformUtils.CopyElements(_doc, _SeleccionarLosa.ListaFloor.Select(c => c.Id).ToList(), XYZ.Zero).ToList();

                    for (int i = 0; i < listaCOpia.Count; i++)
                    {
                        var fl = _doc.GetElement(listaCOpia[i]);
                        ParameterUtil.SetParaStringNH(fl, "N° Losa", "Duplicadas");
                        ParameterUtil.SetParaIntNH(fl, "Structural", 0);
                    }
                    tr.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al duplicar losas. ex:{ex.Message}");

                return false;
            }
            return true;
        }
    }
}
