using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.BimWorkSet.Factory
{
    public class FactorEspecialidades
    {
        public static List<string> ListaGenerales = new List<string>() { "GEN_LINK" };
        internal static List<string> ListaArquitectura = new List<string>() { "ARQ_HORMIGONES", "ARQ_ESTRUCTURA METALICA", "ARQ_ELEMENTOS NO ESTRUCTURALES" };
        internal static List<string> ListaEstructura = new List<string>() { "EST_HORMIGONES", "EST_ESTRUCTURA METALICA" };
        internal static List<string> ListaElectricidad = new List<string>() {
                "ELE_SC_ACOMETIDA", "ELE_SC_DISTRIBUCIÓN", "ELE_SC_ALUMBRADO", "ELE_SC_ALUMBRADO EXTERIOR","ELE_SC_FUERZA", "ELE_DO_DISTRIBUCIÓN", "ELE_DO_TABLEROS",
            "ELE_DO_ALUMBRADO ", "ELE_DO_FUERZA", "COD_SC_ACOMETIDA", "COD_SC_DISTRIBUCIÓN", "COD_SC_PUNTOS DE CCDD", "COD_DO_DISTRIBUCIÓN",
            "COD_DO_PUNTOS DE CCDD", "TEL_SC_ACOMETIDA", "TEL_SC_DISTRIBUCIÓN", "TEL_SC_PUNTOS DE TELECOMUNICACIONES", "TEL_DO_DISTRIBUCIÓN",
            "TEL_DO_PUNTOS DE TELECOMUNICACIONES" };

        internal static List<string> ListaAguaPotable = new List<string>() {"APF_SC_MATRIZ","APF_SC_DISTRIBUCIÓN","APF_DO_DISTRIBUCIÓN",
            "APC_SC_MATRIZ","APC_SC_DISTRIBUCIÓN","APC_DO_DISTRIBUCIÓN","RHU_DISTRIBUCIÓN","RSE_DISTRIBUCIÓN" };

        internal static List<string> ListaAguaServidas = new List<string>() { "ASE_DISTRIBUCIÓN" };
        internal static List<string> ListaAguasLLuvias = new List<string>() { "ALL_DISTRIBUCIÓN" };
        internal static List<string> ListaGas = new List<string>() {"GAS_MEDIA PRESION","GAS_BAJA PRESION" };
        internal static List<string> ListaMecanico = new List<string>() {"VEN_DISTRIBUCIÓN","CLI_DISTRIBUCIÓN","CTE_DISTRIBUCIÓN","RES_DISTRIBUCIÓN" };
        internal static List<string> ListaPiscina = new List<string>() { "PIS_DISTRIBUCIÓN" };
        internal static List<string> ListaPavimento = new List<string>() { "PAV_INTERIOR","PAV_ACCESO" };
        internal static List<string> ListaEntibaciones = new List<string>() { "ENT_DISTRIBUCIÓN"};
        internal static List<string> ListaCoordinacion = new List<string>() { "PAS_MUROS Y VIGAS","PAS_LOSAS","ARQ_HORMIGONES","ARQ_ESTRUCTURA METALICA","ARQ_ELEMENTOS NO ESTRUCTURALES","EST_HORMIGONES","EST_ESTRUCTURA METALICA"};
        internal static List<string> ListaSitios = new List<string>() { "TOP_TOPOGRAFIA", "ENT_DISTRIBUCION" };
    }
}
