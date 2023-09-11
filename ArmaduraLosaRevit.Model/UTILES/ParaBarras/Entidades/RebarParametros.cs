using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    public class RebarParametros
    {
        private readonly Document _doc;
        private readonly Rebar _rebar;
        public List<datosLetras> ListaLsesdfedtras { get; set; }

        public RebarParametros(Document _doc, Rebar rebar)
        {
            this._doc = _doc;
            this._rebar = rebar;
        }
        public bool Ejecutar()
        {
            try
            {
                ListaLsesdfedtras = new List<datosLetras>();
                ListaLsesdfedtras.Add( ObtenerLetras("A"));
                ListaLsesdfedtras.Add(ObtenerLetras("B"));
                ListaLsesdfedtras.Add(ObtenerLetras("C"));
                ListaLsesdfedtras.Add(ObtenerLetras("D"));
                ListaLsesdfedtras.Add(ObtenerLetras("E"));
                ListaLsesdfedtras.Add(ObtenerLetras("F"));
                ListaLsesdfedtras.Add(ObtenerLetras("G"));
                ListaLsesdfedtras.Add(ObtenerLetras("H"));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'RebarParametros'.\n ex:{ex.Message} ");
                return false;
            }
            return true;
        }

        private datosLetras ObtenerLetras(string letra)
        {
            double result_ = 0;
            if (ParameterUtil.FindParaByName(_rebar, letra) != null)
            {

                double.TryParse(ParameterUtil.FindValueParaByName(_rebar, letra, _doc), out result_);
            }
            return new datosLetras(letra, result_);
        }


    }
    public class datosLetras
    {
        public string Letra { get; set; }
        public double Valor { get; set; }
        public datosLetras(string letra, double result_)
        {
            Letra = letra;
            Valor = result_;
        }

      
       
    }
}
