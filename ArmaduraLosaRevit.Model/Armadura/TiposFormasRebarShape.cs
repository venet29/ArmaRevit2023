
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
using Autodesk.Revit.DB.Architecture;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Armadura
{
   public class TiposFormasRebarShape
    {
        public static Dictionary<string, RebarShape> ListaFamilias { get; set; }
        protected static RebarShape m_rebarShape;

        public static RebarShape getRebarShape(string name, Document rvtDoc)
        {

           if (BuscarDiccionario_(name)) return m_rebarShape;

            //Debug.WriteLine($" ---->   name:{name}");
            RebarShape elemento = M1_2_BuscarEnColecctor(name, rvtDoc);

           AgregarDiccionario(name, elemento);

            return elemento;
        }


        public static RebarShape M1_2_BuscarEnColecctor(string name, Document rvtDoc)
        {

            RebarShape m_rebarShape = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            var Lista = filteredElementCollector.OfType<RebarShape>().ToList(); //filtro rapido
                                                                                // m_rebarShape = filteredElementCollector.Cast<RebarShape>().Where(c => c.Name == name).FirstOrDefault();

            m_rebarShape = Lista.Where(c => c.Name == name).FirstOrDefault();
            //if (m_rebarShape == null) TaskDialog.Show("Error", "Tipo de Forma de Barra :" + name + " No encontrada");
            return m_rebarShape;

        }


        protected static bool BuscarDiccionario_(string nombre)
        {
            m_rebarShape = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarShape>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
              m_rebarShape = ListaFamilias[nombre];

             if (m_rebarShape == null)
                return false;
            else    if (!m_rebarShape.IsValidObject)
            {
                ListaFamilias.Remove(nombre);
                return false;
            }
            else
                return true;
        }

        protected static void AgregarDiccionario(string nombre, RebarShape element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarShape>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }








    }
}
