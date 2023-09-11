//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
namespace ArmaduraLosaRevit.Model
{
    using System;
    using Autodesk.Revit.DB;
    using Element = Autodesk.Revit.DB.Element;

    /// <summary>
    /// enum of AreaReinforcement's parameter Layout Rules
    /// </summary>
    public enum LayoutRules
    {
        Fixed_Number = 2,
        Maximum_Spacing = 3
    }

    /// <summary>
    /// contain utility methods find or set certain parameter
    /// </summary>
    public class ParameterUtil
    {
        /// <summary>
        /// find certain parameter in a set
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="name">find by name</param>
        /// <returns>found parameter</returns>
        public static bool SetParaInt(Element elem, string paraName, int value)
        {
            ParameterSet paras = elem.Parameters;
            Parameter findPara = FindParaByName(paras, paraName);

            if (null == findPara)
            {
                return false;
            }

            if (!findPara.IsReadOnly)
            {
                findPara.Set(value);
                return true;
            }

            return false;
        }
        public static bool SetParaInt(Element elem, string paraName, double value)
        {
            try
            {
                ParameterSet paras = elem.Parameters;
                Parameter findPara = FindParaByName(paras, paraName);

                if (null == findPara)
                {
                    return false;
                }

                if (!findPara.IsReadOnly)
                {
                    findPara.Set(value);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al setting parametro :{paraName}    ex:{ex.Message}");
            }
            return false;
        }
        public static bool SetParaInt(Element elem, string paraName, string value)
        {
            try
            {
                ParameterSet paras = elem.Parameters;
                Parameter findPara = FindParaByName(paras, paraName);

                if (null == findPara)
                {
                    return false;
                }

                if (!findPara.IsReadOnly)
                {
                    bool result = findPara.Set(value);
                    return true;
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al setting parametro :{paraName}    ex:{ex.Message}");
            }
            return false;
        }
        public static bool SetParaInt(Element elem, string paraName, ImageType value)
        {
            try
            {
                ParameterSet paras = elem.Parameters;
                Parameter findPara = FindParaByName(paras, paraName);

                if (null == findPara)
                {
                    return false;
                }

                if (!findPara.IsReadOnly)
                {

                    findPara.Set(value.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al setting parametro :{paraName}    ex:{ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// find certain parameter in a set
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="name">find by name</param>
        /// <returns>found parameter</returns>
        public static Parameter FindParaByName(ParameterSet paras, string name)
        {
            Parameter findPara = null;

            foreach (Parameter para in paras)
            {
                if (para.Definition.Name == name)
                {
                    findPara = para;
                    return findPara;
                }
            }

            return findPara;
        }


        /// <summary>
        /// find certain parameter in a set
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="name">find by name</param>
        /// <returns>found parameter</returns>
        public static Parameter FindParaByName(Element Elem, string name)
        {

            Parameter findPara = null;
            if (Elem == null) return findPara;
            ParameterSet paras = Elem.Parameters;
            foreach (Parameter para in paras)
            {
                if (para.Definition.Name == name)
                {
                    findPara = para;
                    return findPara;
                }
            }

            return findPara;
        }


        //public static void FindParaByLosas(Room room, string name)
        //{
        //    Element elem = (Element)room;
        //    Parameter findPara = null;
        //    ParameterSet paras = elem.Parameters;
        //    foreach (Parameter para in paras)
        //    {
        //        if (para.Definition.Name == name)
        //        {
        //            findPara = para;
        //            return findPara;
        //        }
        //    }

        //    return findPara;
        //}

        public static string FindParaByBuiltInParameter(Element elem, BuiltInParameter paraIndex, Document doc)
        {
            Parameter param = elem.get_Parameter(paraIndex);
            string val = "";


            Autodesk.Revit.DB.StorageType type = param.StorageType;
            switch (type)
            {
                case Autodesk.Revit.DB.StorageType.Double:
                    val = param.AsDouble().ToString();
                    return val;
                case Autodesk.Revit.DB.StorageType.ElementId:
                    Autodesk.Revit.DB.ElementId id = param.AsElementId();
                    Autodesk.Revit.DB.Element paraElem = doc.GetElement(id);
                    if (paraElem != null)
                        val = paraElem.Name;
                    return val;
                case Autodesk.Revit.DB.StorageType.Integer:
                    val = param.AsInteger().ToString();
                    return val;
                case Autodesk.Revit.DB.StorageType.String:
                    val = param.AsString();
                    return val;
                default:
                    break;
            }
            return val;
        }

        /// <summary>
        /// find certain parameter in a set
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="name">find by name</param>
        /// <returns>found parameter</returns>
        public static string FindValueParaByName(ParameterSet paras, string name, Document doc)
        {

            string val = "";

            foreach (Parameter param in paras)
            {
                if (param.Definition.Name == name)
                {

                    Autodesk.Revit.DB.StorageType type = param.StorageType;
                    switch (type)
                    {
                        case Autodesk.Revit.DB.StorageType.Double:
                            val = param.AsDouble().ToString();
                            return val;
                        case Autodesk.Revit.DB.StorageType.ElementId:
                            Autodesk.Revit.DB.ElementId id = param.AsElementId();
                            Autodesk.Revit.DB.Element paraElem = doc.GetElement(id);
                            if (paraElem != null)
                                val = paraElem.Name;
                            return val;
                        case Autodesk.Revit.DB.StorageType.Integer:
                            val = param.AsInteger().ToString();
                            return val;
                        case Autodesk.Revit.DB.StorageType.String:
                            val = param.AsString();
                            return val;
                        default:
                            break;
                    }
                }

            }


            return val;
        }




        public static string FindValueParaByName(Element elem, string name, Document doc=null)
        {
            if(elem==null) return "";

            if(doc==null)
                doc = elem.Document;
            string val = "";
            ParameterSet paras = elem.Parameters;

            foreach (Parameter param in paras)
            {
                if (param.Definition.Name == name)
                {

                    Autodesk.Revit.DB.StorageType type = param.StorageType;
                    switch (type)
                    {
                        case Autodesk.Revit.DB.StorageType.Double:
                            val = param.AsDouble().ToString();
                            return val;
                        case Autodesk.Revit.DB.StorageType.ElementId:
                            Autodesk.Revit.DB.ElementId id = param.AsElementId();
                            Autodesk.Revit.DB.Element paraElem = doc.GetElement(id);
                            if (paraElem != null)
                                val = paraElem.Name;
                            return val;
                        case Autodesk.Revit.DB.StorageType.Integer:
                            val = param.AsInteger().ToString();
                            return val;
                        case Autodesk.Revit.DB.StorageType.String:
                            val = param.AsString();
                            return val;
                        default:
                            break;
                    }
                }

            }


            return val;
        }

        /// <summary>
        /// set certain parameter of given element to int value
        /// </summary>
        /// <param name="elem">given element</param>
        /// <param name="paraIndex">BuiltInParameter</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParaInt(Element elem, BuiltInParameter paraIndex, int value)
        {
            Parameter para = elem.get_Parameter(paraIndex);
            if (null == para)
            {
                return false;
            }

            if (!para.IsReadOnly)
            {
                para.Set(value);
                return true;
            }

            return false;
        }
        public static bool SetParaDouble(Element elem, BuiltInParameter paraIndex, double value)
        {
            Parameter para = elem.get_Parameter(paraIndex);
            if (null == para)
            {
                return false;
            }

            if (!para.IsReadOnly)
            {
                para.Set(value);
                return true;
            }

            return false;
        }
        public static bool SetParaString(Element elem, BuiltInParameter paraIndex, string value)
        {
            Parameter para = elem.get_Parameter(paraIndex);
            if (null == para)
            {
                return false;
            }


            if (!para.IsReadOnly)
            {
                para.Set(value);
                return true;
            }

            return false;
        }
        public static bool SetParaElementId(Element elem, BuiltInParameter paraIndex, ElementId value)
        {

            Parameter para = elem.get_Parameter(paraIndex);
            if (null == para)
            {
                return false;
            }


            if (!para.IsReadOnly)
            {
                para.Set(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// set certain parameter of given element to int value
        /// </summary>
        /// <param name="elem">given element</param>
        /// <param name="paraIndex">BuiltInParameter</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParaNullId(Parameter para)
        {
            Autodesk.Revit.DB.ElementId id = new ElementId(-1);

            if (!para.IsReadOnly)
            {
                para.Set(id);
                return true;
            }
            return false;
        }


        public static bool SetParaStringNH(Element eleme, string nombreParametro, string valorParametro)
        {
            bool result = false;
            try
            {
                if (FindParaByName(eleme, nombreParametro) != null)
                    result = SetParaInt(eleme, nombreParametro, valorParametro);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return result;
        }
        public static bool SetParaIntNH(Element eleme, string nombreParametro, int valorParametro)
        {
            bool result = false;
            try
            {
                if (FindParaByName(eleme, nombreParametro) != null)
                    result = SetParaInt(eleme, nombreParametro, valorParametro);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return result;
        }
        public static bool SetParaDoubleNH(Element eleme, string nombreParametro, double valorParametro)
        {
            bool result = false;
            try
            {
                if (FindParaByName(eleme, nombreParametro) != null)
                    result = SetParaInt(eleme, nombreParametro, valorParametro);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return result;
        }

        public static bool SetParaElementIdNH(Element elem, BuiltInParameter paraIndex, ElementId value)
        {
            bool result = false;
            try
            {
                //  if (FindParaByBuiltInParameter(elem, paraIndex, elem.Document) != "")
                SetParaElementId(elem, paraIndex, value);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return result;
        }
    }

}
