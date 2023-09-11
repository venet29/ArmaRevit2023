using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF
{
    public class FactoryITagF_
    {

        public static ITagF ObtenerICasoyBarraX(Document doc, PathReinforcement m_createdPathReinforcement, string TipoBarraStr)
        {

            try
            {


                var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds().ToList();

                if (Ilist_RebarInSystem == null) return new TagF_NULL();

                if (Ilist_RebarInSystem.Count() == 1) return new TagF_simple(doc, m_createdPathReinforcement);
                else if (TipoBarraStr == "f16") return new TagF_simetrica(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f16"))
                {
                    return new TagF_simetrica(doc, m_createdPathReinforcement);

                }
                else if (TipoBarraStr == "f17") return new TagF_F17(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f17"))
                {
                    //  ICasoBarra iTipoBarra = new BarraF17(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

                }
                else if (TipoBarraStr == "f18") return new TagF_simetrica(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f18"))
                {
                    //   ICasoBarra iTipoBarra = new BarraF18(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

                }
                else if (TipoBarraStr == "f19") return new TagF_simetrica(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f19"))
                {
                    //  ICasoBarra iTipoBarra = new BarraF19(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

                }
                else if (TipoBarraStr == "f20a" || TipoBarraStr == "f20b" || TipoBarraStr == "f20aInv" || TipoBarraStr == "f20bInv") return new TagF_simetrica(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f20"))
                {
                    //  ICasoBarra iTipoBarra = new BarraF20(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

                }
                
                else if (TipoBarraStr == "f21" || TipoBarraStr == "f21a" || TipoBarraStr == "f21b") return new TagF_F21(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f21"))
                {
                    //ICasoBarra iTipoBarra = new Barra11(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

                }
                else if (TipoBarraStr == "f22") return new TagF_simetrica(doc, m_createdPathReinforcement);
                else if (TipoBarraStr.Contains("f22"))
                {
                    return new TagF_simetrica(doc, m_createdPathReinforcement);

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtenerICasoyBarraX' ex:{ex.Message} ");
            }
            return new TagF_NULL();
        }


    }
}
