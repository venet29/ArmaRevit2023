using ArmaduraLosaRevit.Model.BarraV.Copiar.model;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar
{
    internal class CopiaBarrasElev
    {
        private UIApplication _uiapp;
        private Document _doc;
        private SeleccionarBarrasREbar_InfoComppleta _seleccionarBarrasCopiar;
        private List<ElementoRebar_Elev> _listaWrapperBarrasElevaciones_inicial;

        public List<WrapperFormatoRebar_final> _listaWrapperFormatoRebar_final { get; set; }

        public CopiaBarrasElev(UIApplication uiapp, SeleccionarBarrasREbar_InfoComppleta seleccionarBarrasCopiar)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            _seleccionarBarrasCopiar = seleccionarBarrasCopiar;
            _listaWrapperBarrasElevaciones_inicial = seleccionarBarrasCopiar.ListaBarrasRebar_InfoCompleta;
            _listaWrapperFormatoRebar_final = new List<WrapperFormatoRebar_final>();
        }

        public bool Isok { get; private set; }

        public bool CopiarSinTrasn()
        {
            Isok = true;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"copiarBarrasSinColor-NH");
                    for (int i = 0; i < _listaWrapperBarrasElevaciones_inicial.Count; i++)
                    {
                        ElementoRebar_Elev _wrapperBarrasLosa = _listaWrapperBarrasElevaciones_inicial[i];
                        M1_Copiar(_wrapperBarrasLosa);
                    }
                    t.Commit();
                }

                //cambiar color  y formato            


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg("No se puede mover barra");
                Isok = false;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return Isok;
        }

        private void M1_Copiar(ElementoRebar_Elev wrapperBarrasElevaciones)
        {
            List<ElementId> ele = wrapperBarrasElevaciones.ObtenerListaIdPath();
            var liscopaida = ElementTransformUtils.CopyElements(_doc, ele, wrapperBarrasElevaciones.VectorDesplazamiento).ToList();
            ManejadorCopiaElevBarra.barrasCopiadas += 1;
            WrapperFormatoRebar_final _newWrapperFormatoRebar = new WrapperFormatoRebar_final(_doc, liscopaida, wrapperBarrasElevaciones);
            _newWrapperFormatoRebar.Ejecutar();
            _listaWrapperFormatoRebar_final.Add(_newWrapperFormatoRebar);
        }




    }
}