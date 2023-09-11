
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;

namespace ArmaduraLosaRevit.Model.FAMILIA
{
    public class ChangeFamilyRebar
    {


        class JtFamilyLoadOptions : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }
        /// <summary>
        /// Logging helper class to keep track of the result
        /// of updating the font of all text note types in a
        /// family. The family may be skipped or not. If not,
        /// keep track of all its text note types and a flag 
        /// for each indicating whether it was updated.
        /// </summary>
        class SetDimensionInRebarShapeInFamilyResult
        {
            class RebarShapeTypeResult
            {
                public string Name { get; set; }
                public bool Updated { get; set; }
            }

            /// <summary>
            /// The Family element name in the project database.
            /// </summary>
            public string FamilyName { get; set; }

            /// <summary>
            /// The family document used to reload the family.
            /// </summary>
            public Document FamilyDocument { get; set; }

            /// <summary>
            /// Was this family skipped, e.g. this family is not editable.
            /// </summary>
            public bool Skipped { get; set; }

            /// <summary>
            /// List of text note type names and updated flags.
            /// </summary>
            List<RebarShapeTypeResult> RebarshapeTypeResults;

            public SetDimensionInRebarShapeInFamilyResult(Family f)
            {
                FamilyName = f.Name;
                RebarshapeTypeResults = null;
            }

            public void AddRebarshapeType(RebarShape tnt, bool updated)
            {
                if (null == RebarshapeTypeResults)
                {
                    RebarshapeTypeResults = new List<RebarShapeTypeResult>();
                }
                RebarShapeTypeResult r = new RebarShapeTypeResult();
                r.Name = tnt.Name;
                r.Updated = updated;
                RebarshapeTypeResults.Add(r);
            }

            int NumberOfUpdatedRebarshapeTypes
            {
                get
                {
                    return null == RebarshapeTypeResults
                      ? 0
                      : RebarshapeTypeResults.Count<RebarShapeTypeResult>(r => r.Updated);
                }
            }

            public bool NeedsReload
            {
                get
                {
                    return 0 < NumberOfUpdatedRebarshapeTypes;
                }
            }

            public override string ToString()
            {
                // FamilyDocument.Title

                string s = FamilyName + ": ";

                if (Skipped)
                {
                    s += "skipped";
                }
                else
                {
                    if (RebarshapeTypeResults == null) return s;
                    int nTotal = RebarshapeTypeResults.Count;
                    int nUpdated = NumberOfUpdatedRebarshapeTypes;

                    s += string.Format("{0} text note types processed, " + "{1} updated", nTotal, nUpdated);
                }
                return s;
            }
        }


        /// <summary>
        /// DESDE UN ELEMNTO FAMILIA REBARSHAPE CAMBIA UN PARAMETRO (DIMENSION) DE VALOR
        /// </summary>
        /// <param name="f"></param>
        /// <param name="rvtDoc"></param>
        /// <param name="parametro"></param>
        /// <param name="valorParametro"></param>
        /// <param name="nameFamilia"></param>
        /// <remarks> f =familia se obtiene con la funcion estantica  fam = <c>TiposFamilyRebar.getFamilyRebarShape</c> </remarks>
        /// <example> 
        /// <code>Family fam = TiposFamilyRebar.getFamilyRebarShape("NH4_bajo", doc); 
        ///             ChangeFamilyRebar.SetDimensionRebarShape(fam, doc,"A", 2, "NH4_bajo");
        /// </code>
        /// </example>

        ///<exception cref=""></exception>
        public static void SetDimensionRebarShape(Family f, Document rvtDoc, string parametro, int valorParametro, string nameFamilia)
        {
            if (f == null) return;
            Document famdoc;

            if (f.IsEditable && f.Name == nameFamilia)
            {
                famdoc = rvtDoc.EditFamily(f);

                FilteredElementCollector rebarshapes = new FilteredElementCollector(famdoc).OfClass(typeof(RebarShape));
                try
                {
                    using (Transaction tranew = new Transaction(famdoc))
                    {
                        tranew.Start("Update3-NH");

                        foreach (RebarShape rebarshape in rebarshapes)
                        {
                            if (ParameterUtil.FindParaByName(rebarshape, parametro) != null)
                            {
                                //agrega Hook alternative  end
                                ParameterUtil.SetParaInt(rebarshape, parametro, valorParametro);
                                //agrega Hook alternative  startnn
                            }
                        }
                        tranew.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al editar la familia:{nameFamilia}  EX:{ex.Message}");
                }


                IFamilyLoadOptions opt = new JtFamilyLoadOptions();
                Family f2 = famdoc.LoadFamily(rvtDoc, opt);
                ParameterUtil.FindParaByName(f2, "Label");
            }

        }

        /// <summary>
        ///  /// DESDE UN ELEMNTO FAMILIA REBARSHAPE CAMBIA UN PARAMETRO (DIMENSION) DE VALOR
        /// </summary>
        /// <param name="f"> familia obtenida con  <code>  TiposFamilyRebar.getFamilyRebarShape  </code></param>
        /// <param name="rvtDoc"> document </param>
        /// <param name="dimensionesBarras">  objeto con las dimensiones para asignagnar ala familia 'f' </param>
        /// <param name="nameFamilia"> nombre de la familia rebarshape que se cambiar los parametros </param>
        /// <param name="factor"> factor que modifica los largos en funcion del largo principal de la barra</param>
        /// <remarks> f =familia se obtiene con la funcion estantica  <code> fam =  TiposFamilyRebar.getFamilyRebarShape </ccode> </remarks>
        /// <example> 
        /// <code>Family fam = TiposFamilyRebar.getFamilyRebarShape("NH4_bajo", doc); 
        ///             ChangeFamilyRebar.SetDimensionRebarShape(fam, doc,"A", 2, "NH4_bajo");
        /// </code>
        /// </example>
        public static string SetDimensionRebarShape(Family f, Document rvtDoc, DimensionesBarras dimensionesBarras, string nombreFamiliaRebarShape, double factor, bool IsSaltar,string verison)
        {

            string result = nombreFamiliaRebarShape;
            if (f == null) return result;
            Document famdoc;



            if (f.IsEditable && f.Name == nombreFamiliaRebarShape)
            {
                famdoc = rvtDoc.EditFamily(f);

                FilteredElementCollector rebarshapes = new FilteredElementCollector(famdoc).OfClass(typeof(RebarShape));

                foreach (RebarShape rebarshape in rebarshapes)
                {
                    try
                    {
                        using (Transaction tranew = new Transaction(famdoc))
                        {
                            tranew.Start("Update4-NH");
                            DateTime now = DateTime.Now;
                            //result = nombreFamiliaRebarShape + "_" + now.Hour + now.Minute + now.Second;
                            result = nombreFamiliaRebarShape + "_A" + dimensionesBarras.a.valorCM + "_B" + dimensionesBarras.b.valorCM + "_C" + dimensionesBarras.c.valorCM + "_D" + dimensionesBarras.d.valorCM + "_E" + dimensionesBarras.e.valorCM + verison;
                            var newRebarshape = rebarshape.Duplicate(result);

                             //dimensionesBarras.listaDimensiones.Reverse();
                            //asigna los parametros internos a,b,c,d,e
                            foreach (var dimension in dimensionesBarras.listaDimensiones)
                            {

                                ///<summary>si  valor es igual a -1 saltar</summary>nn

                                if (dimension.nombre.ToLower()=="c1") continue;
                                if (dimension.valor == 0 && IsSaltar) continue;

                                if (ParameterUtil.FindParaByName(newRebarshape, dimension.nombre) != null)
                                {
                                    //agrega Hook alternative  end
                                    ParameterUtil.SetParaInt(newRebarshape, dimension.nombre, dimension.valor * factor);
                                    // ParameterUtil.SetParaInt(newRebarshape, dimension.nombre, dimension.valor * dimension.factorSegunFamilia);
                                    //agrega Hook alternative  startnn
                                }
                            }
                            tranew.Commit();

                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error al editar familia:{nombreFamiliaRebarShape} \n  {ex.Message}");
                        break;
                    }
                    
                }

                IFamilyLoadOptions opt = new JtFamilyLoadOptions();
                Family f2 = famdoc.LoadFamily(rvtDoc, opt);

                //re asignar familia
                if (TiposRebarShapeFamilia.ListaFamilias.ContainsKey(f2.Name))
                {
                    TiposRebarShapeFamilia.ListaFamilias.Remove(f2.Name);
                    TiposRebarShapeFamilia.ListaFamilias.Add(f2.Name, f2);
                }
                //ParameterUtil.FindParaByName(f2, "Label");
            }

            return result;

        }
     

    }
}
