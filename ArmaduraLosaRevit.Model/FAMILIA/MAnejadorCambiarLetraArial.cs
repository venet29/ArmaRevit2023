using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.FAMILIA
{
    class MAnejadorCambiarLetraArial
    {
        private UIApplication _uiapp;
        private Document _doc;

        public MAnejadorCambiarLetraArial(UIApplication application)
        {
            this._uiapp = application;
            this._doc = application.ActiveUIDocument.Document;
        }

        internal bool Cambiar(bool isMejsaje = false)
        {
            try
            {
                List<string> listaOtro = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_paraOtrosCasos("").Select(c => c.Item1).ToList();

                var tagmuro = TiposWallTagsEnView.cargarListaDetagWall_model(_doc);
                List<Family> IndependentTagPathMOtros = TiposPathReinTagsFamilia.M2_TodasLAsFamilias(_doc)
                                                                                .Where(c => listaOtro.Contains(c.Name))
                                                                                .Select(c => c.Family).ToList();

                List<Family> IndependentTagPathM_Path = TiposPathReinTagsFamilia.M2_TodasLAsFamilias(_doc)
                                                                                .Where(c => c.Family.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_"))
                                                                                .Select(c => c.Family).ToList();

                foreach (var item in IndependentTagPathM_Path)
                {
                    if (item.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_"))
                    { 
                    }
                }

                List<Family> IndependentTagPath = TiposRebarTag.M2_TodasLAsFamilias(_doc).Where(c => c.Name.Contains("MRA Rebar")).Select(c => c.Family).ToList();

                IndependentTagPath.AddRange(IndependentTagPathM_Path);
                IndependentTagPath.AddRange(IndependentTagPathMOtros);
                if(tagmuro!=null)
                    IndependentTagPath.Add(tagmuro.Family);
               
                //var ress = TiposRebarTag.M2_TodasLAsFamilias(_doc).Where(c => c.Name.Contains("MRA Rebar_PILARL_50")).Select(c => c.Family).ToList();
                IndependentTagPath.AddRange(IndependentTagPathMOtros);

                using (TransactionGroup trans = new TransactionGroup(_doc))
                {
                    trans.Start("cambiarArial-NH");

                    for (int i = 0; i < IndependentTagPath.Count; i++)
                    {
                        var fam = IndependentTagPath[i];
                        if (fam == null) continue;
                        if (!fam.IsValidObject) continue;


                        ExportarFamilias _ExportarFamilias = new ExportarFamilias(_uiapp);
                        Debug.WriteLine($"---------------------------------------");
                        Debug.WriteLine($"A) {i} fam:{fam.Name}");
                        if (!_ExportarFamilias.cmbiarLetraArial(fam))
                        {
                            trans.RollBack();
                            return false;

                        }
                    }
                    trans.Assimilate();
                }
            }
            catch (Exception)
            {

                return false;
            }

            if (isMejsaje) Util.InfoMsg("Fin del comando");
            return true;
        }
    }
}
