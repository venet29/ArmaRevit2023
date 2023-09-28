using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar
{
    public class SeleccionarPathReinfomentREsaltar: SeleccionarPathReinfomentRectangulo
    {
        private SeleccionarTagLosa _SeleccionarTagLosa;
        public List<WrapperBarrasLosa> ListaWrapperBarrasLosa { get; set; }

        public SeleccionarPathReinfomentREsaltar(UIApplication uiapp):base(uiapp)
        {
            ListaWrapperBarrasLosa = new List<WrapperBarrasLosa>();
        }


        public bool Ejecutar_seleccion1()
        {

            try
            {
                if (!Seleccionados1Path()) return false;

                 _SeleccionarTagLosa = new SeleccionarTagLosa(_uiapp);
                BuscarPathreinforment();


            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ ex.Message}");
                return false;
            }
            return true;
        }

        public bool EjecutarDesdeRebarInsystem_seleccion1()
        {

            try
            {
                if (!Seleccionados1Path()) return false;

                _SeleccionarTagLosa = new SeleccionarTagLosa(_uiapp);
                BuscarPathreinforment();


            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }



        public bool Ejecutar_seleccionMultiples()
        {

            try
            {
                if (!SeleccionadosMultiplesPathReinConRectaguloYFiltros()) return false;

                _SeleccionarTagLosa = new SeleccionarTagLosa(_uiapp);
                BuscarPathreinforment();


            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ ex.Message}");
                return false;
            }
            return true;
        }


        private void BuscarPathreinforment()
        {

            List<ElementId> listageneral = new List<ElementId>();

            foreach (var ePathReinSymbol in pathReinSpanSymbol_Lista)
            {

                if (!(ePathReinSymbol is PathReinSpanSymbol)) return;

                var _pathReinSymbol = ePathReinSymbol as PathReinSpanSymbol;
                Element ePathReinforcement = _pathReinSymbol.Obtener_GetTaggedLocalElement(_uiapp);
                //obtiene una referencia floor con la referencia r
                if (!(ePathReinforcement is PathReinforcement)) return;
                WrapperBarrasLosa _WrapperBarrasLosa = null;

              var listaRebarInsystem=  ((PathReinforcement)ePathReinforcement).ObtenerRebarInsystem();

                listageneral.Add(ePathReinforcement.Id);
                listageneral.Add(_pathReinSymbol.Id);
                listageneral.AddRange(listaRebarInsystem.Select(c => c.Id));

                if (_SeleccionarTagLosa.ObtenerTagDePathEnView(ePathReinforcement.Id))
                {
                    _WrapperBarrasLosa = new WrapperBarrasLosa()
                    {
                        pathReinforcement = ePathReinforcement as PathReinforcement,
                        pathReinSpanSymbol = _pathReinSymbol,
                        Listatag = _SeleccionarTagLosa.ListIndependentTag
                       
                     };

                    listageneral.AddRange(_SeleccionarTagLosa.ListIndependentTag.Select(c => c.Id).ToList());

                }
                else
                {
                     _WrapperBarrasLosa = new WrapperBarrasLosa()
                    {
                        pathReinforcement = ePathReinforcement as PathReinforcement,
                        pathReinSpanSymbol = _pathReinSymbol,
                    };
                }

                ListaWrapperBarrasLosa.Add(_WrapperBarrasLosa);
            } //fin foreach

            //  _uidoc.ShowElements(ListaWrapperBarrasLosa.Select(c => c.pathReinforcement.Id).ToList());
            _uidoc.Selection.SetElementIds(listageneral);


        }

    

    }
}
