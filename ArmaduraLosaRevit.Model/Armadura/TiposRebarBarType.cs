
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Armadura.DTO;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposRebarBarType
    {
        private static List<DiametrosBarrasDTO> ListDiametrosBarrasDTO;
        public static Dictionary<string, RebarBarType> ListaFamilias { get; set; }

        public static RebarBarType elemetEncontrado;




        public static RebarBarType getRebarBarType(string name, Document rvtDoc, bool IsConMje)
        {
            if (BuscarDiccionario(name)) return elemetEncontrado;

            RebarBarType elemento = ObtenerRebarPornombre(name, rvtDoc, IsConMje);
            AgregarDiccionario(name, elemento);



            return elemento;
        }

        private static RebarBarType ObtenerRebarPornombre(string name, Document rvtDoc, bool IsConMje)
        {


            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(RebarBarType));
            RebarBarType m_rebarBarType = filteredElementCollector.Cast<RebarBarType>().Where(c => c.Name == name).FirstOrDefault();

            if (m_rebarBarType == null && IsConMje == true) TaskDialog.Show("Error", "Tipo Barra :" + name + " No encontrada");

            return m_rebarBarType;
        }



        //busca RebarBarType y devuelve el primero que encuentre
        public static RebarBarType getCualquierRebarBarType(Document rvtDoc)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(RebarBarType));
            RebarBarType m_rebarBarType = filteredElementCollector.Cast<RebarBarType>().FirstOrDefault();


            return m_rebarBarType;
        }


        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarBarType>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            if (elemetEncontrado != null && elemetEncontrado.IsValidObject == false)
            {
                ListaFamilias.Remove(nombre);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }

        private static void AgregarDiccionario(string nombre, RebarBarType element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarBarType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

        public static void DuplicarFamilasReBarBarv2(Document _doc)
        {
            ListDiametrosBarrasDTO = TipoDiametrosBarrasDTO.ObtenerListaDiametro();

            //buscar una bbara referencia
            RebarBarType rebarBarType = TiposRebarBarType.getCualquierRebarBarType(_doc);

            if (rebarBarType == null)
            {
                TaskDialog.Show("Error", "No se pudo encontrar ningun 'RebarBarType' para poder generar los diametros de barra");
                return;
            }

            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("CrearTiposRebartype-NH");
                    //copiar barra

                    foreach (DiametrosBarrasDTO dia in ListDiametrosBarrasDTO)
                    {
                        RebarBarType rebarBarTypeNew = TiposRebarBarType.getRebarBarType("Ø" + dia.diametro_mm, _doc, false);
                        if (rebarBarTypeNew == null)
                        {
                            //rebarBarTypeNew = TiposRebarBarType.getRebarBarType("@" + dia.diametro_mm, _doc, false);
                            ElementType elementType = rebarBarType.Duplicate("Ø" + dia.diametro_mm);
                            ParameterUtil.SetParaInt(elementType, "Bar Diameter", Util.MmToFoot(dia.diametro_mm));

                            //FALTA ASIGNAR VALORES CORRECTOS
                            ConstNH.VersionSObre2022();
                            // para buscar la propiedad 'BarNominalDiameter'  para versiones 2022 hacia arriba
                            PropertyInfo prop = elementType.GetType().GetProperty("BarModelDiameter");
                            if (prop != null)
                            {
                                prop.SetValue(elementType, Util.MmToFoot(dia.diametro_mm));

                                //pra las versiones 2022 , BarDiameter se cambia a Bar Nominal Diameter
                                PropertyInfo prop_diamte = elementType.GetType().GetProperty("BarDiameter");
                                if (prop_diamte != null)
                                {
                                    prop_diamte.SetValue(elementType, Util.MmToFoot(dia.diametro_mm));
                                }

                            }

                            ParameterUtil.SetParaInt(elementType, "Standard Bend Diameter", Util.MmToFoot(dia.Standard_Bend_Diameter_mm));
                            ParameterUtil.SetParaInt(elementType, "Standard Hook Bend Diameter", Util.MmToFoot(dia.Standard_Hook_Bend_Diameter_mm));
                            ParameterUtil.SetParaInt(elementType, "Stirrup/Tie Bend Diameter", Util.MmToFoot(dia.StirrupTie_Bend_Diameter_mm));

                            rebarBarTypeNew = elementType as RebarBarType;

                        }
                        else
                        {
                            Parameter Standard_Bend_Diameter = ParameterUtil.FindParaByName(rebarBarTypeNew, "Standard Bend Diameter");
                            if (Standard_Bend_Diameter != null)
                            {
                                var valorMM = Util.FootToMm(Standard_Bend_Diameter.AsDouble());
                                if(!Util.IsSimilarValor(valorMM, dia.Standard_Bend_Diameter_mm,1))
                                    ParameterUtil.SetParaInt(rebarBarTypeNew, "Standard Bend Diameter", Util.MmToFoot(dia.Standard_Bend_Diameter_mm));
                            }


                            Parameter Standard_Hook_Bend_Diameter = ParameterUtil.FindParaByName(rebarBarTypeNew, "Standard Hook Bend Diameter");
                            if (Standard_Hook_Bend_Diameter != null)
                            {
                                var valorMM = Util.FootToMm(Standard_Hook_Bend_Diameter.AsDouble());
                                if (!Util.IsSimilarValor(valorMM, dia.Standard_Hook_Bend_Diameter_mm,1))
                                    ParameterUtil.SetParaInt(rebarBarTypeNew, "Standard Hook Bend Diameter", Util.MmToFoot(dia.Standard_Hook_Bend_Diameter_mm));
                            }

                            Parameter Stirrup_Tie_Bend_Diameter = ParameterUtil.FindParaByName(rebarBarTypeNew, "Stirrup/Tie Bend Diameter");
                            if (Stirrup_Tie_Bend_Diameter != null)
                            {
                                var valorMM = Util.FootToMm(Stirrup_Tie_Bend_Diameter.AsDouble());
                                if (!Util.IsSimilarValor(valorMM, dia.StirrupTie_Bend_Diameter_mm,1))
                                    ParameterUtil.SetParaInt(rebarBarTypeNew, "Stirrup/Tie Bend Diameter", Util.MmToFoot(dia.StirrupTie_Bend_Diameter_mm));
                            }
                        }

                        CategoryNameMap subcats = rebarBarTypeNew.Category.SubCategories;

                        if (subcats.Size > 0)
                        {
                            foreach (Category item in subcats)
                            {
                                if (item.Name == "Ø" + dia.diametro_mm)
                                {
                                    item.LineColor = FactoryColores.ObtenerColoresPorDiametro(dia.diametro_mm);
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);
                return;
            }

        }
    }
}
