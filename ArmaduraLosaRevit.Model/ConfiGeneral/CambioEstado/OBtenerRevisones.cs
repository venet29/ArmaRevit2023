using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado
{
    public class OBtenerRevisones
    {
        private ICollection<ElementId> revisions;
        private readonly UIApplication _uiapp;
        private readonly string toMatch;
        private Document _doc;

        public OBtenerRevisones(UIApplication uiapp, string toMatch)
        {
            this._uiapp = uiapp;
            this.toMatch = toMatch;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.revisions = new List<ElementId>();
        }
        public void Analisis(ViewSheet _viewSheet, string toMatch, Document _doc)
        {


            List<Revision> revisions = _viewSheet.GetAdditionalRevisionIds().Select(c => _doc.GetElement(c) as Revision).ToList();
            List<Revision> ListaTodos = _viewSheet.GetAllRevisionIds().Select(c => _doc.GetElement(c) as Revision).ToList();

            if (revisions.Count > 0)
            {
                // Apply the new list of revisions
                using (Transaction t = new Transaction(_doc, "Add revisions to sheet"))
                {
                    t.Start();
                    _doc.Delete(revisions[0].Id);
                    t.Commit();
                }
            }

        }

        private bool M0_ObtnerNuevaRevision()
        {
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                collector.OfCategory(BuiltInCategory.OST_Revisions);
                collector.WhereElementIsNotElementType();

                if (revisions.Count > 0)
                    collector.Excluding(revisions);


                var encontrado = FactoryEstados.ListaEstadosProyecto.Find(c => c.CODIGOESTADODEAVANCE == toMatch);
                if (encontrado == null) return false;



                //if(toMatch=="AC")
                //    ListaActual = FactoryEstados.ListaEstadosProyecto.Where(c => c.posicion <= encontrado.posicion).Select(c=> c.CurrentRevision).ToList();
                //else

                var ListaActual = FactoryEstados.ListaEstadosProyecto.Where(c =>
                {
                    if (toMatch == "AC")
                        return (c.posicion == encontrado.posicion ? true : false);
                    else
                        return (c.posicion <= encontrado.posicion ? true : false);
                }
                ).Select(c => c.CurrentRevision).ToList();

                // Check if revision should be added
                foreach (Revision revision in collector)
                {
                    
                    if (revision == null) continue;
                    //if (revision.NumberType == RevisionNumberType.Alphanumeric)
                    //{

                    //    Util.ErrorMsg($"Proyecto tiene revisiones asignadas 'por planos', cambiar a numeracion 'por proyecto'.\n\n    View>Revision> Numbering >Per Project ");
                    //    //return false;
                    //}
                    if (ListaActual.Contains(revision.RevisionNumber))
                        revisions.Add(revision.Id);
                }

                if (revisions.Count == 0) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_1_ObtenerNuevaRevision'. ex:{ex.Message}");
                return false;
            }
            return true;
        }




        public bool M1_AddAdditionalRevisionsToSheet_Todos(List<ViewSheetNH> _ListviewSheet)
        {
            try
            {
                if (!M0_ObtnerNuevaRevision()) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Asignar estado visibilidad-NH");
                    for (int i = 0; i < _ListviewSheet.Count; i++)
                    {
                        if (revisions.Count > 0)
                        {
                            if (_ListviewSheet[i]._view is ViewSheet)
                                ((ViewSheet)_ListviewSheet[i]._view).SetAdditionalRevisionIds(revisions);
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_AddAdditionalRevisionsToSheet_Todos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
