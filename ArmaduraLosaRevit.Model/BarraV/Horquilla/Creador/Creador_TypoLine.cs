using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.UTILES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Creador
{
    public class Creador_TypoLine
    {
     

        public static Element ObtenerLineStyle_Horq(Document _doc)
        {
            Element line_styles_BARRA = null;
            try
            {
                line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Horq", _doc);
                if (line_styles_BARRA == null)
                {
                    CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, "Horq", 1, new Color(255, 255, 255), "IMPORT-CENTER");
                    CrearLineStyle.CreateLineStyleConTrans();
                    
                    line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Horq", _doc);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener tipo de barra ex:{ex.Message}" );
            }

            return line_styles_BARRA;
        }
    }
}
