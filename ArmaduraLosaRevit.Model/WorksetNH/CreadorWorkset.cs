using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model
{
    public class CreadorWorkset
    {
        private readonly UIApplication _Uiapp;
        private Document _doc;

        public Workset newWorkset { get; set; }
        public CreadorWorkset(UIApplication _uiapp)
        {
            _Uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool CreateWorkset_COnTrasn(List<string> LIstworksetName)
        {
            string worksetName = "";
            try
            {
                // Worksets can only be created in a document with worksharing enabled
                if (_doc.IsWorkshared)
                {
                    using (Transaction worksetTransaction = new Transaction(_doc, "Set preview view id"))
                    {
                        worksetTransaction.Start();
                        for (int i = 0; i < LIstworksetName.Count; i++)
                        {
                            worksetName = LIstworksetName[i];
                            // Workset name must not be in use by another workset
                            if (WorksetTable.IsWorksetNameUnique(_doc, worksetName))
                            {
                                newWorkset = Workset.Create(_doc, worksetName);
                            }
                        }
                        worksetTransaction.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al crear workset {worksetName}  ex:{ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// https://forums.autodesk.com/t5/revit-ideas/delete-worksets-from-revit-api/idi-p/7575512
        /// solo se uede borras desde 2022.1        
        public bool BorrarWorkset_COnTrasn(List<string> listaGeneral)
        {
            string worksetName = "";
            try
            {
                List<ElementId> ListaBorras = new List<ElementId>();
                // Worksets can only be created in a document with worksharing enabled
                if (_doc.IsWorkshared)
                {
                    for (int i = 0; i < listaGeneral.Count; i++)
                    {

                        worksetName = listaGeneral[i];

                        var worksetNH = TiposWorkSet.ObtenerTiposView(worksetName, _doc);
                        if (worksetNH == null)
                        {
                            Util.ErrorMsg($"No se encontro workset {worksetNH} para borrar");
                            continue;
                        }
                        ListaBorras.Add(new ElementId(worksetNH.Id.IntegerValue));
                    }


                    using (Transaction worksetTransaction = new Transaction(_doc, "Set preview view id"))
                    {
                        worksetTransaction.Start();
                        _doc.Delete(ListaBorras);
                        worksetTransaction.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al crear workset {worksetName}  ex:{ex.Message}");
                return false;
            }
        }

        public bool AgregarElementosAHormigon()
        {
            try
            {

            }
            catch (Exception)
            {

                return false;
            }
            return true;

        }
    }
}
