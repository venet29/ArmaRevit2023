
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
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura.DTO;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TipoRebarHookType
    {

        public static int[] ListaHook { get; set; } = new int[11] { 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90 };

        private static Dictionary<string, RebarHookType> ListaFamilias { get; set; }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, RebarHookType>();

        protected static RebarHookType m_RebarHookType;


        public static RebarHookType ObtenerHook(string name, Document rvtDoc, bool IsMesaje = true)
        {

            if (M1_BuscarDiccionario_(name)) return m_RebarHookType;

            //Debug.WriteLine($" ---->   name:{name}");
            RebarHookType elemento = M2_getRebarHookType(name, rvtDoc);

            M3_AgregarDiccionario(name, elemento);

            if (elemento == null && IsMesaje)
            {
                Util.ErrorMsg($"No se encontro gancho de nombre '{name}'.\n Carge familias");

            }

            return elemento;
        }

        #region metodos HOOK

        protected static bool M1_BuscarDiccionario_(string nombre)
        {
            m_RebarHookType = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarHookType>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                m_RebarHookType = ListaFamilias[nombre];

            if (m_RebarHookType == null)
                return false;
            else if (!m_RebarHookType.IsValidObject)
            {
                ListaFamilias.Remove(nombre);
                return false;
            }
            else
                return true;
        }
        public static RebarHookType M2_getRebarHookType(string name, Document _doc)
        {
            RebarHookType m_rebarHookType = null;
            List<RebarHookType> m_rebarShapes = new List<RebarHookType>();

            m_rebarShapes = M2_getAllRebarHookType(_doc);

            foreach (var item in m_rebarShapes)
            {
                if (item.Name == name)
                {
                    m_rebarHookType = item;
                    return m_rebarHookType;
                }
            }
            return m_rebarHookType;

        }
        public static List<RebarHookType> M2_getAllRebarHookType(Document rvtDoc)
        {

            List<RebarHookType> m_rebarShapes = new List<RebarHookType>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(RebarHookType));
            m_rebarShapes = filteredElementCollector.Cast<RebarHookType>().ToList<RebarHookType>();
            //   if (m_rebarHookType == null) TaskDialog.Show("Error", "Barra tipo:" + name + " No encontrada");

            return m_rebarShapes;

        }

        protected static void M3_AgregarDiccionario(string nombre, RebarHookType element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, RebarHookType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        #endregion



        public static bool CrearHookIniciales(Document _doc)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");

                    RebarHookType _tipodeHook = ObtenerHook("Stirrup/Tie - 135 deg.", _doc);

                    if (_tipodeHook == null)
                    {
                        RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(135), 6);
                        tipodeHook.Style = RebarStyle.StirrupTie;
                        tipodeHook.Name = "Stirrup/Tie - 135 degNH";
                    }

                    _tipodeHook = ObtenerHook("Rebar Hook 135", _doc);
                    if (_tipodeHook == null)
                    {
                        RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(135), 6);
                        tipodeHook.Style = RebarStyle.Standard;
                        tipodeHook.Name = "Rebar Hook 135";
                    }

                    _tipodeHook = ObtenerHook("Rebar Hook 90", _doc);
                    if (_tipodeHook == null)
                    {
                        RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(90), 6);
                        tipodeHook.Style = RebarStyle.Standard;
                        tipodeHook.Name = "Rebar Hook 90";
                    }

                    _tipodeHook = ObtenerHook("Hook_PataFundacion", _doc,false);
                    if (_tipodeHook == null)
                    {
                        RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(90), ConstNH.LARGO_PATA_FUNDACIONES_CM);
                        tipodeHook.Style = RebarStyle.Standard;
                        tipodeHook.Name = "Hook_PataFundacion";
                    }




                    for (int i = 0; i < ListaHook.Length; i++)
                    {
                        var hookk = ListaHook[i];
                        _tipodeHook = ObtenerHook($"Hook_PataFundacion_{hookk}", _doc, false);
                        if (_tipodeHook == null)
                        {
                            RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(90), hookk);
                            tipodeHook.Style = RebarStyle.Standard;
                            tipodeHook.Name = $"Hook_PataFundacion_{hookk}";
                        }
                    }

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }
            return true;
        }


        public static bool CrearHook(Document _doc, double Largo_cm)
        {
            try
            {
                var _tipodeHook = ObtenerHook($"Hook_PataFundacion_{Largo_cm}", _doc, false);

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");
                    if (_tipodeHook == null)
                    {
                        double largo_Foot = (Largo_cm > 99 ? 99 : Largo_cm);
                        RebarHookType tipodeHook = RebarHookType.Create(_doc, Util.GradosToRadianes(90), largo_Foot);
                        tipodeHook.Style = RebarStyle.Standard;
                        tipodeHook.Name = $"Hook_PataFundacion_{Largo_cm}";
                    }

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }
            return true;
        }


        public static void CrearHook_asignarLargosARebar(Document _doc, RebarHookType _tipodeHook, double largohook_cm)
        {
            var ListDiametrosBarrasDTO = TipoDiametrosBarrasDTO.ObtenerListaDiametro();
            if (_tipodeHook == null) return;
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("AsignarLargoHook-NH");
                    //copiar barra

                    foreach (DiametrosBarrasDTO dia in ListDiametrosBarrasDTO)
                    {
                        RebarBarType rebarBarTypeNew = TiposRebarBarType.getRebarBarType("Ø" + dia.diametro_mm, _doc, false);
                        if (rebarBarTypeNew == null) continue;

                        // asignar largo de los hook
                        //for (int i = 0; i < TipoRebarHookType.ListaHook.Length; i++)
                        //{
                        //   var hookk = TipoRebarHookType.ListaHook[i];
                        //   var _tipodeHook = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{hookk}", _doc, false);
                        if (_tipodeHook != null)
                        {
                            rebarBarTypeNew.SetAutoCalcHookLengths(_tipodeHook.Id, false);
                            rebarBarTypeNew.SetHookLength(_tipodeHook.Id, Util.CmToFoot(largohook_cm));
                            rebarBarTypeNew.SetHookTangentLength(_tipodeHook.Id, Util.CmToFoot(largohook_cm));
                        }
                        //}
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
