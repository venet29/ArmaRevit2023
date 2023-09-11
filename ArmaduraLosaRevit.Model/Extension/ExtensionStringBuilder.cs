using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
  public static  class ExtensionStringBuilder
    {

        public static void AgregarListaPtos(this StringBuilder sb, List<XYZ> ListaTos,string titulo)
        {
            sb.AppendLine("-------------------------------------------------------------------------------------------------------------------------" );
            sb.AppendLine(titulo);
            for (int i = 0; i < ListaTos.Count; i++)
            {
                sb.AppendLine(" P" + i + ":" + new XYZ(Math.Round(ListaTos[i].X,5), Math.Round(ListaTos[i].Y, 5), Math.Round(ListaTos[i].Z, 5)));
            }
        }


    }
}
