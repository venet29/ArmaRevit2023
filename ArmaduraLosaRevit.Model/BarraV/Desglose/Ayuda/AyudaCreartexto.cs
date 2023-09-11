using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
   public class AyudaCreartexto
    {
        public static bool M4_CrearTExto(Document doc,  XYZ ptoInserccion, string texto,string tipotexto, double anguloRad, TipoCOloresTexto _color )
        {
            try
            {
                CrearTexNote _CrearTexNote = new CrearTexNote(doc, tipotexto, _color);

                _CrearTexNote.M1_CrearConTrans(ptoInserccion, texto, anguloRad);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear 'CrearTExto' ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public static bool M4_CrearTExtoSinTrans(Document doc, XYZ ptoInserccion, string texto, string tipotexto, double anguloRad, TipoCOloresTexto _color)
        {
            try
            {
                CrearTexNote _CrearTexNote = new CrearTexNote(doc, tipotexto, _color);

                _CrearTexNote.M1_CrearCSintrans(ptoInserccion, texto, anguloRad);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear 'CrearTExto' ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
