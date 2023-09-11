using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.Servicio
{
    public class ServicioActivarFamiliaPasada
    {

        private UIApplication _uiapp;
        private Document _doc;
        private FamilySymbol FamilySymbolPAsada_GRIS;

        private static FamilySymbol FamilySymbolPAsada_NARANJO { get; set; }
        private static FamilySymbol FamilySymbolPAsada_VERDE { get; set; }
        private static FamilySymbol FamilySymbolPAsada_AZUL { get; set; }
        private static FamilySymbol FamilySymbolPAsada_ROJA { get; set; }
        public FamilySymbol FamilySymbolPAsada { get; set; }

        public ServicioActivarFamiliaPasada(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }


        public  FamilySymbol ObtenerFamilia(ColorTipoPasada _ColorTipoPasada)
        {
            try
            {
                if (FamilySymbolPAsada_VERDE == null || FamilySymbolPAsada_ROJA == null && FamilySymbolPAsada_AZUL==null || FamilySymbolPAsada_NARANJO==null || FamilySymbolPAsada_GRIS==null)
                    Ejecutar();

                if (_ColorTipoPasada == ColorTipoPasada.PASADA_VERDE)
                    return FamilySymbolPAsada_VERDE;
                else if (_ColorTipoPasada == ColorTipoPasada.PASADA_NARANJO)
                    return FamilySymbolPAsada_NARANJO;
                else if (_ColorTipoPasada == ColorTipoPasada.PASADA_AZUL)
                    return FamilySymbolPAsada_AZUL;
                else if (_ColorTipoPasada == ColorTipoPasada.PASADA_ROJA)
                    return FamilySymbolPAsada_ROJA;
                else if (_ColorTipoPasada == ColorTipoPasada.PASADA_GRIS)
                    return FamilySymbolPAsada_GRIS;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_ObtenerFamilia_ModelGeneric' EX:{ex.Message}");

            }
            return null;
        }
        public bool Ejecutar()
        {

            try
            {
                if (FamilySymbolPAsada_VERDE != null && FamilySymbolPAsada_ROJA != null && FamilySymbolPAsada_AZUL != null && FamilySymbolPAsada_NARANJO!= null && FamilySymbolPAsada_GRIS != null)
                    return true;

                if (M1_ObtenerFamilia_ModelGeneric(ColorTipoPasada.PASADA_VERDE.ToString()))
                    FamilySymbolPAsada_VERDE = FamilySymbolPAsada;
                else
                    return false;

                if (M1_ObtenerFamilia_ModelGeneric(ColorTipoPasada.PASADA_ROJA.ToString()))
                    FamilySymbolPAsada_ROJA = FamilySymbolPAsada;
                else
                    return false;

                if (M1_ObtenerFamilia_ModelGeneric(ColorTipoPasada.PASADA_AZUL.ToString()))
                    FamilySymbolPAsada_AZUL = FamilySymbolPAsada;
                else
                    return false;

                if (M1_ObtenerFamilia_ModelGeneric(ColorTipoPasada.PASADA_NARANJO.ToString()))
                    FamilySymbolPAsada_NARANJO = FamilySymbolPAsada;
                else
                    return false;

                if (M1_ObtenerFamilia_ModelGeneric(ColorTipoPasada.PASADA_GRIS.ToString()))
                    FamilySymbolPAsada_GRIS = FamilySymbolPAsada;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_ObtenerFamilia_ModelGeneric' EX:{ex.Message}");

            }
            return true;

        }
        public bool M1_ObtenerFamilia_ModelGeneric(string nombrePasada)
        {
            try
            {
                FamilySymbolPAsada = TiposGenericFamilySymbol.M1_GenericFamilySymbol_nh(nombrePasada, BuiltInCategory.OST_GenericModel, _doc);

                if (FamilySymbolPAsada == null)
                {
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                    _ManejadorCargarFAmilias.cargarFamilias_Pasada();

                    FamilySymbolPAsada = TiposGenericFamilySymbol.M1_GenericFamilySymbol_nh(nombrePasada, BuiltInCategory.OST_GenericModel, _doc);
                }

                if (!FamilySymbolPAsada.IsActive)
                    M2_ActivarFamiliaPasada();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_ObtenerFamilia_ModelGeneric' EX:{ex.Message}");

            }
            return true;
        }


        public bool M2_ActivarFamiliaPasada()
        {
            try
            {
                if (FamilySymbolPAsada == null) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ActivarPAsada-NH");


                    if (!FamilySymbolPAsada.IsActive)
                        FamilySymbolPAsada.Activate();

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_ActivarFamiliaPasada'  EX:{ex.Message}");
            }
            return true;
        }

    }
}
