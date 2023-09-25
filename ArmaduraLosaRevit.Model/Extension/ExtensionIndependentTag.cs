using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{


    public static class ExtensionIndependentTag
    {

        public static void Set_LeaderElbow(this IndependentTag tag, UIApplication _uiapp, XYZ pto)
        {
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                //version22 hacia arriba
                MethodInfo meth = tag.GetType().GetMethod("SetLeaderElbow", new Type[] { typeof(Reference), typeof(XYZ) });
                if (meth != null)
                {
                    //var referenc = tag.GetTaggedReferences().FirstOrDefault();
                    MethodInfo methRefe = tag.GetType().GetMethod("GetTaggedReferences");
                    List<Reference> methRefevalor = methRefe.Invoke(tag, new object[] { }) as List<Reference>;

                    var referenc = methRefevalor.FirstOrDefault();
                    object valor = meth.Invoke(tag, new object[] { referenc, pto });
                    return;
                }
            }
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderElbow");
                if (prop2 != null)
                {
                    prop2.SetValue(tag, pto, null);
                    return;
                }
            }
        }

        //version 2021 hacia abajo
        public static XYZ Obtener_LeaderElbow(this IndependentTag tag, UIApplication _uiapp)
        {
            XYZ punto = default;

            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                //version22 hacia arriba
                MethodInfo prop = tag.GetType().GetMethod("GetLeaderElbow", new Type[] { typeof(Reference) });
                if (prop != null)
                {
                    MethodInfo methRefe = tag.GetType().GetMethod("GetTaggedReferences");
                    List<Reference> methRefevalor = methRefe.Invoke(tag, new object[] { }) as List<Reference>;
                    var referenc = methRefevalor.FirstOrDefault();
                    //object valor = prop.GetValue(tag, new Type[] { typeof(referenc) });
                    object valor = prop.Invoke(tag, new object[] { referenc });
                    if (valor != null)
                    {
                        punto = (XYZ)valor;
                        return punto;
                    }
                }
            }
            else
            {
                //version 2021 hacia abajo
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderElbow");
                if (prop2 != null)
                {
                    object valor = prop2.GetValue(tag, null);
                    if (valor != null)
                    {
                        punto = (XYZ)valor;
                        return punto;
                    }
                }
            }
            return punto;
        }

        //**************************

        public static void Set_LeaderEnd(this IndependentTag tag, UIApplication _uiapp, XYZ pto)
        {

            //XYZ punto = default;
            //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            //    tag.SetLeaderElbow(tag.GetTaggedReferences().FirstOrDefault(), pto);
            //else
            //{
            //    PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
            //    if (prop2 != null)
            //    {
            //        prop2.SetValue(tag, pto, null);
            //    }
            //}
            //return punto;

            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                //version22 hacia arriba
                MethodInfo meth = tag.GetType().GetMethod("SetLeaderEnd", new Type[] { typeof(Reference), typeof(XYZ) });
                if (meth != null)
                {
                    //var referenc = tag.GetTaggedReferences().FirstOrDefault();
                    //   independentTag.SetLeaderElbow(independentTag.GetTaggedReferences().FirstOrDefault(), ptoelbow);
                    MethodInfo methRefe = tag.GetType().GetMethod("GetTaggedReferences");
                    List<Reference> methRefevalor = methRefe.Invoke(tag, new object[] { }) as List<Reference>;

                    var referenc = methRefevalor.FirstOrDefault();
                    object valor = meth.Invoke(tag, new object[] { referenc, pto });
                    return;
                }
            }
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
                if (prop2 != null)
                {
                    prop2.SetValue(tag, pto, null);
                    return;
                }
            }
        }
        public static XYZ Obtener_LeaderEnd(this IndependentTag tag, UIApplication _uiapp)
        {
            XYZ punto = default;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                //version22 hacia arriba
                MethodInfo prop = tag.GetType().GetMethod("GetLeaderEnd", new Type[] { typeof(Reference) });
                if (prop != null)
                {
                    MethodInfo methRefe = tag.GetType().GetMethod("GetTaggedReferences");
                    List<Reference> methRefevalor = methRefe.Invoke(tag, new object[] { }) as List<Reference>;
                    var referenc = methRefevalor.FirstOrDefault();
                    //object valor = prop.GetValue(tag, new Type[] { typeof(referenc) });
                    object valor = prop.Invoke(tag, new object[] { referenc });
                    if (valor != null)
                    {
                        punto = (XYZ)valor;
                        return punto;
                    }
                }
            }
            else
            {
                //version 2021 hacia abajo
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
                if (prop2 != null)
                {
                    object valor = prop2.GetValue(tag, null);
                    if (valor != null)
                    {
                        punto = (XYZ)valor;
                        return punto;
                    }
                }
            }
            //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            //    punto = tag.GetLeaderEnd(tag.GetTaggedReferences().FirstOrDefault());
            //else
            //{
            //    PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
            //    if (prop2 != null)
            //    {
            //        object valor = prop2.GetValue(tag, null);
            //        if (valor != null)
            //            punto = (XYZ)valor;
            //    }
            //}
            return punto;
        }
        //******************

        //public static ElementId Obtener_GetTaggedLocalElementID(this IndependentTag tag, UIApplication _uiapp)
        //{
        //    Element result = Obtener_GetTaggedLocalElement(tag);

        //    if (result != null)
        //        return result.Id;
        //    else
        //        return null;
        //}

        public static ElementId Obtener_GetTaggedLocalElementID(this IndependentTag tag)
        {
            Element result = Obtener_GetTaggedLocalElement(tag);

            if (result != null)
                return result.Id;
            else
                return null;
        }

        //public static Element Obtener_GetTaggedLocalElement(this IndependentTag tag, UIApplication _uiapp)
        //{
        //    Document _doc = tag.Document;
        //    if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
        //    {
        //        var elementId = tag.GetTaggedLocalElementIds().FirstOrDefault();
        //        Element REbarDeTag = _doc.GetElement(elementId);
        //        return REbarDeTag;
        //    }
        //    else
        //    {
        //        MethodInfo prop2 = tag.GetType().GetMethod("GetTaggedLocalElement");
        //        //MethodInfo prop2 = tag.GetType().GetMethod("get_Element", new Type[] { typeof(ElementId) });   //con parametros
        //        if (prop2 != null)
        //        {
        //            Element REbarDeTag = prop2.Invoke(tag, null) as Element;
        //            return REbarDeTag;
        //        }
        //    }
        //    return null;
        //}
        public static Element Obtener_GetTaggedLocalElement(this IndependentTag tag)
        {


            MethodInfo meth = tag.GetType().GetMethod("GetTaggedLocalElementIds");
            //MethodInfo prop2 = tag.GetType().GetMethod("get_Element", new Type[] { typeof(ElementId) });   //con parametros
            if (meth != null)
            {
                Document _doc = tag.Document;
                var ListaId = meth.Invoke(tag, null) as ISet<ElementId>;

                var elementId = ListaId.ToList().FirstOrDefault();
                Element REbarDeTag = _doc.GetElement(elementId);
                return REbarDeTag;
            }

            MethodInfo prop2 = tag.GetType().GetMethod("GetTaggedLocalElement");
            //MethodInfo prop2 = tag.GetType().GetMethod("get_Element", new Type[] { typeof(ElementId) });   //con parametros
            if (prop2 != null)
            {
                Element REbarDeTag = prop2.Invoke(tag, null) as Element;
                return REbarDeTag;
            }

            return null;
        }
    }
}
