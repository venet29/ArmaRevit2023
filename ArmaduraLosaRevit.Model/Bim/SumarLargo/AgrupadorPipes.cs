using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Bim.SumarLargo.Model;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Bim.SumarLargo
{
    internal class AgrupadorPipes
    {
        private UIApplication _uiapp;
        private List<Pipe> listPipe;
        public List<DuctosPipe> ListaPipeDatos { get; set; }
        public string textoSumaLArgos { get; set; }
        public AgrupadorPipes(UIApplication uiapp, List<Pipe> listConduct)
        {
            _uiapp = uiapp;
            this.listPipe = listConduct;
            ListaPipeDatos = new List<DuctosPipe>();
            textoSumaLArgos = "";
        }

        internal bool GEnerarGrupos()
        {
            try
            {
                foreach (Pipe _pipe in listPipe)
                {
                    DuctosPipe _newDuctosDatos = new DuctosPipe(_pipe);
                    if (_newDuctosDatos.ObtenerDatos())
                    {
                        ListaPipeDatos.Add(_newDuctosDatos);
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
               


                var listaGRupos = ListaPipeDatos.GroupBy(c => new { diam = c.Diamtro_mm, name = c.NombreTipo });

                foreach (var itemGrup in listaGRupos)
                {
                    var _listaDuctos = itemGrup.ToList();
                    var diam = itemGrup.Key.diam;
                    var nombreTipo = itemGrup.Key.name;

                     var textoSumaLArgos_aux = _listaDuctos.Sum(c => c.Largo_mt).ToString();

                    var textoIteracion= nombreTipo + " Ø" + diam + "\n L : " + textoSumaLArgos_aux + " m";
                    if (textoSumaLArgos == "")
                        textoSumaLArgos = textoIteracion;
                    else
                        textoSumaLArgos = textoSumaLArgos + "\n\n" + textoIteracion;
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