using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class Tipos_Barras
    {
        public static List<EntidadBarras> ListaBarraTipo { get; set; }

        public static EntidadBarras elemetEncontrado;

        public static string M1_Buscar_nombreTipoBarras_porTipoRebar(TipoRebar name)
        {
            BuscarDiccionario(name);
            return (elemetEncontrado != null ? elemetEncontrado.nombre : "");
        }

        public static TipoBarraGeneral M2_Buscar_TipoGrupoBarras_pornombre(string name)
        {
            BuscarDiccionario(name);
            return (elemetEncontrado != null ? elemetEncontrado.grupo : TipoBarraGeneral.NONE);
        }

        public static EntidadBarras M3_Buscar_EntidadBarras_porTipoRebar(TipoRebar TipoRebar)
        {
            BuscarDiccionario(TipoRebar);
            if (elemetEncontrado == null) BuscarDiccionario(TipoRebar.NONE);
            return elemetEncontrado;
        }

        public static void Limpiar() => ListaBarraTipo = new List<EntidadBarras>();

        private static void GenerarLista() => ListaBarraTipo = FactoryEntidadBarras.ObtenerListaEntidades();



        private static bool BuscarDiccionario(object nombre)
        {
            elemetEncontrado = null;
            if (ListaBarraTipo == null)
            {
                GenerarLista();

            }
            else if (ListaBarraTipo.Count == 0)
            {
                GenerarLista();
            }

            EntidadBarras result = null;
            if (nombre is string)
                result = ListaBarraTipo.Where(c => c.nombre == ((string)nombre)).FirstOrDefault();
            else if (nombre is TipoRebar)
                result = ListaBarraTipo.Where(c => c.tipoRebar == ((TipoRebar)nombre)).FirstOrDefault();
            else if (nombre is TipoBarraGeneral)
                result = ListaBarraTipo.Where(c => c.grupo == ((TipoBarraGeneral)nombre)).FirstOrDefault();


            if (result != null)
                elemetEncontrado = result;
            else
            {
                if (nombre is string)
                    result = ObtenerValorOpcion((string)nombre);
                else
                    result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.NONE)).FirstOrDefault();
            }




            return (elemetEncontrado == null ? false : true);
        }

        private static EntidadBarras ObtenerValorOpcion(string nombre)
        {
            EntidadBarras result = default;
            if (nombre.Contains("ELEV"))
            {
                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.ELEV_BA_V)).FirstOrDefault();
                result.nombre = nombre;
            }
            else if (nombre.Contains("LOSA_"))
            {

                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.LOSA_INF)).FirstOrDefault();
                result.nombre = nombre;
            }
            else if ( nombre.Contains("REFUERZO_"))
            {

                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.REFUERZO_BA)).FirstOrDefault();
                result.nombre = nombre;
            }
            else if (nombre.Contains("FUND") || nombre.Contains("FUND"))
            {
                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.FUND_BA)).FirstOrDefault();
                result.nombre = nombre;
            }
            else
                result = ListaBarraTipo.Where(c => c.tipoRebar == (TipoRebar.NONE)).FirstOrDefault();
          
            return result;
        }
    }


}
