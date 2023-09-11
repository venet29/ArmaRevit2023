using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Armadura;
using System.IO;
using ArmaduraLosaRevit.Model.COMUNES;

using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using System.Linq;
using System.Windows.Documents;

namespace ArmaduraLosaRevit.Model.FAMILIA

{
    public class ManejadorCargarFAmilias
    {
        #region 0)propiedades            
        string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();
        // public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);
        List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();
        public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);

        private static bool FamiliaRunCargada = false;
        private readonly UIApplication _uiapp;

        public Document _doc { get; set; }
        #endregion

        #region 1)constructor

        public ManejadorCargarFAmilias(UIApplication uiapp)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            rutaRaiz = rutaRaiz + AgregarVErsion.Ejecutar(uiapp.Application.VersionNumber);
            this._uiapp = uiapp;
        }


        #endregion

        #region 2)metodos 

        public void cargarFamilias_COnLista(string nombrefamilia,bool IsRecargar = false)
        {
            List<string> lista = new List<string>() { nombrefamilia };
            cargarFamilias_COnLista(lista);
        }
        public void cargarFamilias_COnLista(List<string> lista, bool IsRecargar = false)
        {
            if (!Directory.Exists(ConstNH.CONST_COT)) return;
            LimpiandoListas.Limpiar();

            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz);


            List<Tuple<string, string>> listaRutasFamiliasFiltro = new List<Tuple<string, string>>();
            for (int i = 0; i < lista.Count; i++)
            {
                string resultString = lista[i].Replace("50", "").Replace("75", "").Replace("100", "");

                var elemEncontrado = listaRutasFamilias.Where(c => c.Item1.Contains(resultString)).FirstOrDefault();

                if (elemEncontrado != null)
                    listaRutasFamiliasFiltro.Add(elemEncontrado);
            }

            CargarFamilias(_doc, listaRutasFamiliasFiltro, IsRecargar);


        }

        public void cargarFamilias_run()
        {
            if (!Directory.Exists(ConstNH.CONST_COT)) return;
            LimpiandoListas.Limpiar();

            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz);
            CargarFamilias(_doc, listaRutasFamilias);

            //*** 22-09 -2022 solo se usa para cambiar letra de tag
            //cambiar letras Arieal
            MAnejadorCambiarLetraArial _MAnejadorCambiarLetraArial = new MAnejadorCambiarLetraArial(_uiapp);
            _MAnejadorCambiarLetraArial.Cambiar();

        }


        public void cargarFamiliasBim_run()
        {

            if (FamiliaRunCargada) return;
            if (!Directory.Exists(ConstNH.CONST_COT)) return;
            LimpiandoListas.Limpiar();

            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamiliasBIM_Varios(rutaRaiz);
            CargarFamilias(_doc, listaRutasFamilias);

            //*** 22-09 -2022 solo se usa para cambiar letra de tag
            //cambiar letras Arieal
            MAnejadorCambiarLetraArial _MAnejadorCambiarLetraArial = new MAnejadorCambiarLetraArial(_uiapp);
            _MAnejadorCambiarLetraArial.Cambiar();
            FamiliaRunCargada = false;
        }


        public void cargarFamilias_Pasada()
        {
            if (!Directory.Exists(ConstNH.CONST_COT)) return;
            LimpiandoListas.Limpiar();

            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_paraPasadas(rutaRaiz);
            CargarFamilias(_doc, listaRutasFamilias);
        }


        //29-05-2023 solo para racargar familias f10A  borrar pasado un año
        public void cargarFamilias_F10A()
        {
            if (!Directory.Exists(ConstNH.CONST_COT)) return;
            LimpiandoListas.Limpiar();

            listaRutasFamilias = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_F10A(rutaRaiz);
            CargarFamilias(_doc, listaRutasFamilias, true);
        }


        private void CargarFamilias(Document doc, List<Tuple<string, string>> listafami, bool IsRecargar = false)
        {
            try
            {
                using (Transaction trans = new Transaction(doc))
                {
                    trans.Start("CargarFamilias-NH");

                    //recorrer diccionario con familias
                    foreach (Tuple<string, string> NombreFamilia in listafami)
                    {
                        //buscar si existe familia
                        Family fam = null;
                        string NombreFAmilia = NombreFamilia.Item1;
                        string RutaFAmilis = NombreFamilia.Item2;
                        fam = TiposFamilyRebar.getFamilyRebarShape(NombreFAmilia, doc);

                        // Element elem = FiltroGetFamilySymbolByName.FindElementByName(doc, typeof(Family), NombreFamilia.Key);
                        //si no encuentra familia cargarla
                        if (fam == null && File.Exists(RutaFAmilis))
                        {
                            doc.LoadFamily(RutaFAmilis, out fam);
                        }
                        else if (IsRecargar && File.Exists(RutaFAmilis))
                        {
                            var _familiaOption = new FamilyOption();
                            doc.LoadFamily(RutaFAmilis, _familiaOption, out fam);
                        }
                    }
                    trans.Commit();
                }

                //** para denombrar familia pathsymbol 13-04-2022
                ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
                if (_ManejadorReNombrar.IsFamiliasAntiguas())
                    _ManejadorReNombrar.Renombarra();
                //**

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                return;
            }
        }
        #endregion
    }


    class FamilyOption : IFamilyLoadOptions
    {

        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            if (!familyInUse)
            {
                TaskDialog.Show("SampleFamilyLoadOptions", "The family has not been in use and will keep loading.");

                overwriteParameterValues = true;
                return true;
            }
            else
            {
                TaskDialog.Show("SampleFamilyLoadOptions", "The family has been in use but will still be loaded with existing parameters overwritten.");

                overwriteParameterValues = true;
                return true;
            }
        }


        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            if (!familyInUse)
            {
                TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has not been in use and will keep loading.");

                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
            else
            {
                TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has been in use but will still be loaded from the FamilySource with existing parameters overwritten.");

                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }
    }
}
