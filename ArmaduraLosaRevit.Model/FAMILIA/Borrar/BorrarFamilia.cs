using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA.Borrar
{
    public class BorrarFamilia
    {
        private CasosBorrarFamilias casosBorrar;
        private readonly UIApplication _uiapp;
        private readonly Document _doc;
        private FilteredElementCollector _fec;
        private List<ElementId> _listaFamilia;
        private List<string> _listaNoEncotrado;
        private List<Element> listaFma;

        public BorrarFamilia(UIApplication uiapp)
        {
            this.casosBorrar = CasosBorrarFamilias.SOLOBARRAS;
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            _listaFamilia = new List<ElementId>();
            _listaNoEncotrado = new List<string>();
        }
        public bool BorrarTodasLasFamilias()
        {

            bool result = true;
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);
            try
            {
                var listaRutasFamilias = ObtenerLAsfamilias();

                ObtenerListaFamilias();
                foreach (var item in listaRutasFamilias)
                {
                    Family famToDelete = OBtenerFamiliaPorNombre(item.Item1);
                    if (famToDelete != null)
                        _listaFamilia.Add(famToDelete.Id);
                    else
                        _listaNoEncotrado.Add(item.Item1);
                }

                BorrarFamilias();
                Util.InfoMsg("Familias borradas correctamente");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al borrar familias, ex:{ex.Message}");
                result= false;
            }
            UpdateGeneral.M4_CargarGenerar(_uiapp);
            return result;
        }

        private  List<Tuple<string, string>> ObtenerLAsfamilias()
        {
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();


            if (casosBorrar == CasosBorrarFamilias.TODOS) return FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS("");

            if (casosBorrar == CasosBorrarFamilias.SOLOBARRAS)
            {
                listaRutasFamilias.AddRange(FactoryCargarFamilias.CrearDiccionarioRutasFamilias_paraDIbujarBarras(""));
                listaRutasFamilias.AddRange(FactoryCargarFamilias.CrearDiccionarioRutasFamilias_PelotasArmadura(""));
            }

            if (casosBorrar == CasosBorrarFamilias.Otros)
                listaRutasFamilias.AddRange(FactoryCargarFamilias.CrearDiccionarioRutasFamilias_paraOtrosCasos(""));

            return listaRutasFamilias;
        }

        private void BorrarFamilias()
        {
            if (_listaFamilia.Count > 0)
            {

                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("Borrar Familias Armadura-NH");
                        _doc.Delete(_listaFamilia);
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    TaskDialog.Show("Error", message);
                    return;
                }

            }

        }

        private Family OBtenerFamiliaPorNombre(string nombreFamilia)
        {

            var result = listaFma.Where(c => c.Name.Equals(nombreFamilia)).FirstOrDefault();
            return (result != null ? (Family)result : null);

        }

        private void ObtenerListaFamilias()
        {
            _fec = new FilteredElementCollector(_doc);
            _fec.OfClass(typeof(Family));
            listaFma = _fec.ToElements().ToList();
        }
    }
}
