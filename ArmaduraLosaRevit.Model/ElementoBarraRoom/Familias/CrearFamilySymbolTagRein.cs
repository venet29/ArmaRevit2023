using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
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



    public class CrearFamilySymbolTagRein
    {
        private DatosNuevaBarraDTO datosNuevaBarra;
        private Document _doc;
        private UbicacionLosa _ubicacionEnlosa;
        private string _TipoBarra;
        private object _LargoMin_1;
        private double _EspesorLosa_1;

        public object LargoPathreiforment { get; }

        public CrearFamilySymbolTagRein(Document doc)
        {
            this._doc = doc;

        }
        public CrearFamilySymbolTagRein(SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, double EspesorLosa_1)
        {
            this.datosNuevaBarra = datosNuevaBarraDTO;

            this._doc = solicitudDTO.UIdoc.Document;
            this._ubicacionEnlosa = solicitudDTO.UbicacionEnlosa;
            this._TipoBarra = solicitudDTO.TipoBarra;

            this._LargoMin_1 = datosNuevaBarraDTO.LargoMininoLosa;
            this._EspesorLosa_1 = EspesorLosa_1;
            this.LargoPathreiforment = datosNuevaBarraDTO.LargoPathreiforment;
        }


        public Element ObtenerLetraGirada(string nombreIndependentTagPath_modif, string nombreFamiliaGEnerica_, double AnguloGRado, int escala)
        {

            Element IndependentTagPath = null;
            try
            {
                Family fam = null;

                //Los buscar segum el nombre

                IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(nombreIndependentTagPath_modif, _doc);


                if (IndependentTagPath != null) return IndependentTagPath;

                //fam = ((FamilySymbol)IndependentTagPath).Family;
                fam = TiposPathReinTagsFamilia.M1_GetFamilySymbol_nh(nombreFamiliaGEnerica_, _doc);


                // si lo encuentra el modifca los parametros
                if (fam != null)
                {

                    //crea una copia de la la familianhs
                    string newNombreFamiliaRebarShape = CreaBuevoTipoIndependentTagPath_pathReinLetra(fam, _doc, nombreIndependentTagPath_modif, nombreFamiliaGEnerica_, Util.GradosToRadianes(AnguloGRado), escala);
                    // rebarshape = ChangeFamilyRebar.SetDimensionRebarShape1(fam, doc, dimBarras_, nombreFamiliaRebarShape, factor / largobaa, true);
                    IndependentTagPath = TiposPathReinTags.M1_GetFamilySymbol_nh(nombreIndependentTagPath_modif, _doc);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLetraGirada'  ex:{ex.Message}");
            }
            return IndependentTagPath;
        }


        public static string CreaBuevoTipoIndependentTagPath_pathReinLetra(Family f, Document _doc, string nombreIndependentTagPath_modif, string NuevoNombreFamiliaGenerica, double Angulo, int escala)
        {
            if (f == null) return NuevoNombreFamiliaGenerica;
            Document famdoc;

            string parametro = "TagAngle";

            if (f.IsEditable && f.Name == NuevoNombreFamiliaGenerica)
            {
                famdoc = _doc.EditFamily(f);

                if (null != famdoc)
                {
                    try
                    {
                        FamilyManager familyManager = famdoc.FamilyManager;

                        if (!VerificarSITipoEstaDentroEstructuraFAmilia(familyManager, nombreIndependentTagPath_modif))
                        {

                            using (Transaction tranew = new Transaction(famdoc))
                            {
                                tranew.Start("TagAngle-NH");


                                FamilyType newFamilyType = familyManager.NewType(nombreIndependentTagPath_modif);

                                FamilyParameter familyParam = familyManager.get_Parameter(parametro);
                                if (null != familyParam) familyManager.Set(familyParam, -Angulo);

                                string parametro100 = "visible_100";
                                FamilyParameter familyParam100 = familyManager.get_Parameter(parametro100);
                                if (null != familyParam100) familyManager.Set(familyParam100, (escala == 100 ? 1 : 0));

                                string parametro75 = "visible_75";
                                FamilyParameter familyParam75 = familyManager.get_Parameter(parametro75);
                                if (null != familyParam75) familyManager.Set(familyParam75, (escala == 75 ? 1 : 0));

                                string parametro50 = "visible_50";
                                FamilyParameter familyParam50 = familyManager.get_Parameter(parametro50);
                                if (null != familyParam50) familyManager.Set(familyParam50, (escala == 50 ? 1 : 0));

                                tranew.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"  EX:{ex.Message}");
                    }
                    IFamilyLoadOptions opt = new JtFamilyLoadOptions();
                    Family f2 = famdoc.LoadFamily(_doc, opt);
                    //ParameterUtil.FindParaByName(f2, "Label");
                }
            }



            return NuevoNombreFamiliaGenerica;

        }

        private static bool VerificarSITipoEstaDentroEstructuraFAmilia(FamilyManager familyManager, string nombreTipo)
        {
            //ConstNH.CONST_CORTO;
            if (familyManager == null) return true; //para q no siga con la transaccion y quede con error
            if (nombreTipo == "") return true; //para q no siga con la transaccion y quede con error
            try
            {
                //***
                //esta seccion esta pq al crear y ahcer undo la familia que queda dentro de las estructura de la familia, PERO NO en el proyecto
                FamilyTypeSet familyTypes = familyManager.Types;
                FamilyTypeSetIterator familyTypesItor = familyTypes.ForwardIterator();
                familyTypesItor.Reset();
                while (familyTypesItor.MoveNext())
                {
                    FamilyType familyType = familyTypesItor.Current as FamilyType;
                    Debug.Print(familyType.Name);

                    if (nombreTipo == familyType.Name)
                        return true;
                }
                //**
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al validar si tipo {nombreTipo} existe dentro de la estructura de familia   \n ex:{ex.Message}");
            }
            return false;
        }
    }
}
