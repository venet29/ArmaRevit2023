using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA
{
    public class FactoryCargarFamilias
    {


        public static Dictionary<string, string> CrearDiccionarioImagenes(string rutaRaiz)
        {
            Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);

            listaImagenes.Add("f1", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1\Shape 31.png");
            listaImagenes.Add("f1_SUP", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1SUP\Shape 31SUP.png");
            listaImagenes.Add("f3", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3\Shape 01.png");
            listaImagenes.Add("f4", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F4\Shape 41.png");
            listaImagenes.Add("f7", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F7\Shape 31.png");
            listaImagenes.Add("f9", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9\Shape 41_inv.png");
            listaImagenes.Add("f9a", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9A\Shape f9a_inv.png");

            listaImagenes.Add("f10", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10\Shape f10.png");
            listaImagenes.Add("f10a", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10A\Shape f9b_inv.png");

            listaImagenes.Add("f11", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f10.png");
            listaImagenes.Add("f11a", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f16", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f17", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f18", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f19", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f20", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f21", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("f22", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("s3", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\Shape f11.png");
            listaImagenes.Add("s2", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3\Shape 01.png");
            listaImagenes.Add("s1", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\S1\Shape 41_inv.png");

            return listaImagenes;
        }


        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_TODAS(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();

            var paraBarras = CrearDiccionarioRutasFamilias_paraDIbujarBarras(rutaRaiz);
            listaRutasFamilias.AddRange(paraBarras);

            var paraOtros = CrearDiccionarioRutasFamilias_paraOtrosCasos(rutaRaiz);
            listaRutasFamilias.AddRange(paraOtros);

            var pasadas = CrearDiccionarioRutasFamilias_paraPasadas(rutaRaiz);
            listaRutasFamilias.AddRange(pasadas);

            return listaRutasFamilias;

        }

        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_paraDIbujarBarras(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();


            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1\M_Path Reinforcement Symbol_F1A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1\M_Path Reinforcement Symbol_F1B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1_ESC", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1_ESCALERA\M_Path Reinforcement Symbol_F1_ESC.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1A_PATA", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1_PATA\M_Path Reinforcement Symbol_F1A_PATA.rfa"));// losa inclinada ok en 19,20
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1B_PATA", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1_PATA\M_Path Reinforcement Symbol_F1B_PATA.rfa"));// losa inclinada ok en 19,20



            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1SupA", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1SUP\M_Path Reinforcement Symbol_F1SupA.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1SupB", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1SUP\M_Path Reinforcement Symbol_F1SupB.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1SupAInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1SUP\M_Path Reinforcement Symbol_F1SupAInv.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F1SupBInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F1SUP\M_Path Reinforcement Symbol_F1SupBInv.rfa"));


            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F3", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3\M_Path Reinforcement Symbol_F3.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F3_PATA", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3_PATA\M_Path Reinforcement Symbol_F3_PATA.rfa")); // losa inclinada  ok en 19,20

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F3A_ESC", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3_ESCALERA\M_Path Reinforcement Symbol_F3A_ESC.rfa"));// escalera pata liso inferior  ok en 19,20
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F3B_ESC", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F3_ESCALERA\M_Path Reinforcement Symbol_F3B_ESC.rfa"));// escalera pata con gancho inferior  ok en 19,20

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F4", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F4\M_Path Reinforcement Symbol_F4.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F6", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F6\M_Path Reinforcement Symbol_F6.rfa"));


            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F7A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F7\M_Path Reinforcement Symbol_F7A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F7B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F7\M_Path Reinforcement Symbol_F7B.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F9", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9\M_Path Reinforcement Symbol_F9.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F9A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9A\M_Path Reinforcement Symbol_F9A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F9B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9A\M_Path Reinforcement Symbol_F9B.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F9BDer", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9A\M_Path Reinforcement Symbol_F9BDere.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F9BIzq", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F9A\M_Path Reinforcement Symbol_F9BIzq.rfa"));


            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F10", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10\M_Path Reinforcement Symbol_F10.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F10AIzq", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10A\M_Path Reinforcement Symbol_F10AIzq.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F10ADer", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10A\M_Path Reinforcement Symbol_F10ADer.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F11", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11\M_Path Reinforcement Symbol_F11.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F11A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11A\M_Path Reinforcement Symbol_F11A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F11B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11A\M_Path Reinforcement Symbol_F11B.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F12", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F12\M_Path Reinforcement Symbol_F12.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F16", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F16\M_Path Reinforcement Symbol_F16.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F16A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F16\M_Path Reinforcement Symbol_F16A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F16B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F16\M_Path Reinforcement Symbol_F16B.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F17A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F17\M_Path Reinforcement Symbol_F17A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F17B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F17\M_Path Reinforcement Symbol_F17B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F17ATras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F17\M_Path Reinforcement Symbol_F17ATras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F17BTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F17\M_Path Reinforcement Symbol_F17BTras.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F18", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F18\M_Path Reinforcement Symbol_F18.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F19Dere", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F19\M_Path Reinforcement Symbol_F19Izq.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F19Izq", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F19\M_Path Reinforcement Symbol_F19Dere.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F19", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F19\M_Path Reinforcement Symbol_F19.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20AInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20AInv.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20AIzqTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20AIzqTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20ADereTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20ADereTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20BInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20BInv.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20BIzqTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20BIzqTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F20BDereTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F20\M_Path Reinforcement Symbol_F20BDereTras.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21AIzqTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21AIzqTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21ADereTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21ADereTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21BIzqTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21BIzqTras.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F21BDereTras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F21\M_Path Reinforcement Symbol_F21BDereTras.rfa"));

            //borra en fututo 03-05-2022
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22Tras", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22Tras.rfa"));
            //borra en fututo 03-05-2022
            //listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22AInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22AInv.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F22BInv", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F22\M_Path Reinforcement Symbol_F22BInv.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_S1", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\S1\M_Path Reinforcement Symbol_S1.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_S3A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\S3\M_Path Reinforcement Symbol_S3A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_S3B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\S3\M_Path Reinforcement Symbol_S3B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_S1INV", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\S1\M_Path Reinforcement Symbol_S1INV.rfa"));

            // listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(1)", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(1).rfa"));
            // listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(2)", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(2).rfa"));
            // listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(3)", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(3).rfa"));
            // listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(4)", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(4).rfa"));
            //antes
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo).rfa"));
            //listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)B", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo)B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)B_InfIzq", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo)B_InfIzq.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)B_DereSup", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo)B_DereSup.rfa"));

            //listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F.rfa"));
            //listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_A_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_A_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_B_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_B_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_C_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_C_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_CVar_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_CVar_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_C2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_C2_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_D_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_D_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_E_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_E_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_L_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_L_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LVar_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_LVar_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_Var_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_Var_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_L2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_L2_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_F_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_F_.rfa"));
            // listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_F_Fund_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_F_Fund_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_F_RefSupleMuro_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_F_RefSupleMuro_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_Ffund_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_Ffund_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FfundSOLO_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_FfundSOLO_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_F2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_F2_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_G_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_G_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_H_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_H_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LTotal_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_LTotal_.rfa"));//nuevo
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LTotalParcial_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letras\M_Path Reinforcement Tag(ID_cuantia_largo)_LTotalParcial_.rfa"));//nuevo

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_AA_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_AA_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_BB_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_BB_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_CC_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_CC_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_CC2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_CC2_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_DD_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_DD_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_EE_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_EE_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LL_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_LL_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LL2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_LL2_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_LLVar_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_LLVar_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FF_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_FF_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FF2_RefSupleMuro_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_FF2_RefSupleMuro_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FFfund_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_FFfund_.rfa"));
            //listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FFfundSOLO_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_FFfundSOLO_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_FF2_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_FF2_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_LL_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_LL_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_S1_FF_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_S1_FF_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_GG_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_GG_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Tag(ID_cuantia_largo)_HH_", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\tag cuantia\letrasConAngulo\M_Path Reinforcement Tag(ID_cuantia_largo)_HH_.rfa"));




            listaRutasFamilias.Add(new Tuple<string, string>("M_Area Reinforcement TagNH", rutaRaiz + @"Familia rebar shape\FamiliaMuro\M_Area Reinforcement TagNH.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Area Reinforcement SymbolNH", rutaRaiz + @"Familia rebar shape\FamiliaMuro\M_Area Reinforcement SymbolNH.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_A_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_A_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_B_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_B_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_CLosa_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_CLosa_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_D_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_D_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_E_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_E_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_G_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_G_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_H_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_H_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_2da_RefViga_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_2da_RefViga_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_RefViga_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_RefViga_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_RefVigaPata_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_RefVigaPata_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FH_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_FH_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FH_SIN_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_FH_SIN_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FHorq_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_FHorq_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_L_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_L_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_LVar_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_LVar_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_Var_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_Var_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_C_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_C_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_Malla_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_Malla_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FIncli_", rutaRaiz + @"Familia rebar shape\Familia Losas\M_Structural MRA Rebar_FIncli_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FLosaEsc_", rutaRaiz + @"Familia rebar shape\Familia Losas\M_Structural MRA Rebar_FLosaEsc_.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_Fund_", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_Fund_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_F_SIN", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_F_SIN.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_SIN", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_SIN.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_CONF", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_CONF.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_PILAR", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_PILAR.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_PILARL", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_PILARL.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_PILART", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_PILART.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_VIGA", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_VIGA.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_VIGAL", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_VIGAL.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_VIGAT", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\M_Structural MRA Rebar_VIGAT.rfa"));


            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_CRef_", rutaRaiz + @"Familia rebar shape\FamiliaRefuerzoLosa\M_Structural MRA Rebar_CRef_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_EstRef_", rutaRaiz + @"Familia rebar shape\FamiliaRefuerzoLosa\M_Structural MRA Rebar_EstRef_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_FRef_", rutaRaiz + @"Familia rebar shape\FamiliaRefuerzoLosa\M_Structural MRA Rebar_FRef_.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Structural MRA Rebar_LRef_", rutaRaiz + @"Familia rebar shape\FamiliaRefuerzoLosa\M_Structural MRA Rebar_LRef_.rfa"));




            listaRutasFamilias.Add(new Tuple<string, string>("M_00", rutaRaiz + @"Familia rebar shape\Familia Losas\M_00.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_T1", rutaRaiz + @"Familia rebar shape\Familia Losas\M_T1.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F1A", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F1A.rfa"));
            // listaRutasFamilias.Add(new Tuple<string, string>("NH_F1A_pata", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F1A_PATA.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F1B", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F1B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH4_bajo", rutaRaiz + @"Familia rebar shape\Familia Losas\NH4_bajo.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F7A", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F7A.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F7B", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F7B.rfa"));
            //listaRutasFamilias.Add(new Tuple<string, string>("NH_F9A", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F7B.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F10", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F10.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F11", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F11.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("NH_F11_v2", rutaRaiz + @"Familia rebar shape\Familia Losas\NH_F11_v2.rfa"));

            return listaRutasFamilias;

        }

        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_paraOtrosCasos(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();


            // familias no barras
            listaRutasFamilias.Add(new Tuple<string, string>(TAGMUROS, rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\" + TAGMUROS + ".rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("TAG VIGA ELEVACION", rutaRaiz + @"Familia rebar shape\FamiliaElevaciones\TAG VIGA ELEVACION.rfa"));

            listaRutasFamilias.Add(new Tuple<string, string>("PELOTA DE LOSAS (NH)", rutaRaiz + @"Familia rebar shape\GeomeLosa\PELOTA DE LOSAS (NH).rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("PELOTA DE LOSAS (NH)ARMA", rutaRaiz + @"Familia rebar shape\GeomeLosa\PELOTA DE LOSAS (NH)ARMA.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("PELOTA DE LOSAS (NH)Var", rutaRaiz + @"Familia rebar shape\GeomeLosa\PELOTA DE LOSAS (NH)Var.rfa"));
            

            //listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH)", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH).rfa"));            
            //listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH)Var", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH)Var.rfa"));
            //listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH) V2", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH) V2.rfa"));
            var  auxPelotasArmadura= CrearDiccionarioRutasFamilias_PelotasArmadura(rutaRaiz);
            listaRutasFamilias.AddRange(auxPelotasArmadura);


            return listaRutasFamilias;

        }


        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_PelotasArmadura(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();

            listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH)", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH).rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH)Var", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH)Var.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("TagRoomFAmilia (NH) V2", rutaRaiz + @"Familia rebar shape\GeomRoom\TagRoomFAmilia (NH) V2.rfa"));

            return listaRutasFamilias;
        }


        public static string TAGMUROS { get; set; } = "TAG MUROS (CORTO)_elev";



        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_paraPasadas(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();
            // familias no barras
            listaRutasFamilias.Add(new Tuple<string, string>("PASADA_RECTANGULAR", rutaRaiz + @"Familia rebar shape\Pasadas\PASADA_RECTANGULAR.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("PASADA_RECTANGULAR_COLOR", rutaRaiz + @"Familia rebar shape\Pasadas\PASADA_RECTANGULAR_COLOR.rfa"));
            return listaRutasFamilias;

        }

        public static List<Tuple<string, string>> CrearDiccionarioRutasFamilias_F10A(string rutaRaiz)
        {
            //    var ssd = (1, 2);
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();
            // familias no barras
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F10AIzq", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F10A\M_Path Reinforcement Symbol_F10AIzq.rfa"));
            listaRutasFamilias.Add(new Tuple<string, string>("M_Path Reinforcement Symbol_F11A", rutaRaiz + @"Familia rebar shape\Symbolo Rebar V3\F11A\M_Path Reinforcement Symbol_F11A.rfa"));
            return listaRutasFamilias;

        }


        public static List<Tuple<string, string>> CrearDiccionarioRutasFamiliasBIM_Varios(string rutaRaiz)
        {
            List<Tuple<string, string>> listaRutasFamilias = new List<Tuple<string, string>>();
            // familias no barras
            listaRutasFamilias.Add(new Tuple<string, string>("GRUPO_CONDUIT", rutaRaiz + @"Familia rebar shape\BIM\M_Etiqueta GrupoConduit.rfa"));
            return listaRutasFamilias;
        }
    }
}
