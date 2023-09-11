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


    public static class ExtensionPathReinSpanSymbol
    {
        public static XYZ Set_LeaderElbow(this PathReinSpanSymbol tag, XYZ pto, UIApplication _uiapp)
        {
            XYZ punto = default;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                tag.SetLeaderElbow(tag.GetTaggedReferences().FirstOrDefault(), pto);
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderElbow");
                if (prop2 != null)
                {
                    prop2.SetValue(tag, pto, null);
                }
            }
            return punto;
        }

        public static XYZ Obtener_LeaderElbow(this PathReinSpanSymbol tag, UIApplication _uiapp)
        {
            XYZ punto = default;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                punto = tag.GetLeaderElbow(tag.GetTaggedReferences().FirstOrDefault());
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderElbow");
                if (prop2 != null)
                {
                    object valor = prop2.GetValue(tag, null);
                    if (valor != null)
                        punto = (XYZ)valor;
                }
            }
            return punto;
        }

        //**************************

        public static XYZ Set_LeaderEnd(this PathReinSpanSymbol tag, XYZ pto, UIApplication _uiapp)
        {
            XYZ punto = default;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                tag.SetLeaderElbow(tag.GetTaggedReferences().FirstOrDefault(), pto);
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
                if (prop2 != null)
                {
                    prop2.SetValue(tag, pto, null);
                }
            }
            return punto;
        }
        public static XYZ Obtener_LeaderEnd(this PathReinSpanSymbol tag, UIApplication _uiapp)
        {
            XYZ punto = default;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                punto = tag.GetLeaderEnd(tag.GetTaggedReferences().FirstOrDefault());
            else
            {
                PropertyInfo prop2 = tag.GetType().GetProperty("LeaderEnd");
                if (prop2 != null)
                {
                    object valor = prop2.GetValue(tag, null);
                    if (valor != null)
                        punto = (XYZ)valor;
                }
            }
            return punto;
        }



        public static Element Obtener_TaggedLocalElement(this PathReinSpanSymbol tag, UIApplication _uiapp)
        {
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                var _doc = _uiapp.ActiveUIDocument.Document;
                var elementId = tag.GetTaggedLocalElementIds().FirstOrDefault();
                Element REbarDeTag = _doc.GetElement(elementId);
                return REbarDeTag ;
            }
            else
            {
                MethodInfo prop2 = tag.GetType().GetMethod("GetTaggedLocalElement");
                //MethodInfo prop2 = tag.GetType().GetMethod("get_Element", new Type[] { typeof(ElementId) });   //con parametros
                if (prop2 != null)
                {
                    Element REbarDeTag = prop2.Invoke(tag, null) as Element;
                    return REbarDeTag ;
                }
            }
            return null;
        }

    }
}
