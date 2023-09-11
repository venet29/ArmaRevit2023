using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Bim.AgruparConductos.Model;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Bim
{
    internal class AgrupadorConduct
    {
        private UIApplication _uiapp;
        private List<Conduit> listConduct;
        public List<DuctosDatos> ListaDuctosDatos { get; set; }
        public string textoGrupo { get; set; }
        public AgrupadorConduct(UIApplication uiapp, List<Conduit> listConduct)
        {
            _uiapp = uiapp;
            this.listConduct = listConduct;
            ListaDuctosDatos = new List<DuctosDatos>();
        }

        internal bool GEnerarGrupos()
        {
            try
            {
                foreach (Conduit _conduit in listConduct)
                {
                    DuctosDatos _newDuctosDatos = new DuctosDatos(_conduit);
                    if (_newDuctosDatos.ObtenerDatos())
                    {
                        ListaDuctosDatos.Add(_newDuctosDatos);
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        internal bool GenerarTExto()
        {
            try
            {
               var listaGRupos= ListaDuctosDatos.GroupBy(c => new { diam=c.Diamtro_mm, name=c.NombreTipo});

                foreach (var itemGrup in listaGRupos)
                {
                    var _listaDuctos = itemGrup.ToList();
                    var diam = itemGrup.Key.diam;
                    var nombreTipo = itemGrup.Key.name;
                    if (textoGrupo == "")
                        textoGrupo = _listaDuctos.Count + " " + diam + " " + nombreTipo;
                    else
                        textoGrupo = textoGrupo + "\n" + _listaDuctos.Count + " " + nombreTipo + " Ø" + diam; 
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

    }
}