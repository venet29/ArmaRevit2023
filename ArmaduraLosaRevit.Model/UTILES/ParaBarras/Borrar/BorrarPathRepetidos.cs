using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Borrar
{
    public class BorrarPathRepetidos
    {
        private Document _doc;
        private UIApplication _uiapp;


        public BorrarPathRepetidos(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }
        public void EjecutarBarrasRepetidos()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);


                seleccionarRebar.BuscarListaPathReinformetEnVistaActual();
                //    listaRebar.AddRange(seleccionarRebar._lista_A_DePathReinfVistaActual.Select(x => x.element).ToList());
                var listaPAth = seleccionarRebar._lista_A_DePathReinfVistaActual.Select(c => new ElemetoBorrarPAth(new CalculoCoordPathReinforme(c.element as PathReinforcement, _doc))).ToList();

                listaPAth.ForEach(c => c.CalculoCoordPathReinforme.Calcular4PtosPathReinf());

                List<ElementId> listaBorrar = new List<ElementId>();

                for (int i = 0; i < listaPAth.Count; i++)
                {
                    var pathBArra = listaPAth[i];
                    if (!pathBArra.Isok) continue;
                    var encontrado = listaPAth.Where(c => c.Isok == true && SOnElMIsmoPath(pathBArra.CalculoCoordPathReinforme, c.CalculoCoordPathReinforme)).FirstOrDefault();
                    if (encontrado != null)
                    {
                        encontrado.Isok = false;
                        pathBArra.Isok = false;
                        listaBorrar.Add(encontrado.CalculoCoordPathReinforme._pathReinforcement.Id);
                    }

                }


                if (listaBorrar.Count < 1) return;

                int cantidad = listaBorrar.Count;
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("GenerarID_Correlativo-NH");
                    _doc.Delete(listaBorrar);
                    trans2.Commit();
                }

                Util.InfoMsg($"Se an borrado {cantidad} pathreinforment repetido");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");
        }


        public class ElemetoBorrarPAth
        {
            public ElemetoBorrarPAth(CalculoCoordPathReinforme calculoCoordPathReinforme)
            {
                CalculoCoordPathReinforme = calculoCoordPathReinforme;
                Isok = true;
            }

            public bool Isok { get; set; }
            public CalculoCoordPathReinforme CalculoCoordPathReinforme { get; set; }
        }


        public bool SOnElMIsmoPath(CalculoCoordPathReinforme _referencia, CalculoCoordPathReinforme _comparado)
        {

            bool IsDistinto = _referencia._pathReinforcement.Id.IntegerValue != _comparado._pathReinforcement.Id.IntegerValue;
            bool resultNumero = _referencia._pathReinforcement.ObtenerNumeroBarras() == _comparado._pathReinforcement.ObtenerNumeroBarras();
            bool resultLArgO = _referencia._pathReinforcement.ObtenerLargoFoot_SinPAtas() == _comparado._pathReinforcement.ObtenerLargoFoot_SinPAtas();

            if (resultNumero && resultLArgO && IsDistinto && _referencia._4pointPathReinf.centro.DistanceTo(_comparado._4pointPathReinf.centro) < Util.CmToFoot(1))
                return true;
            else
                return false;
        }
    }
}
