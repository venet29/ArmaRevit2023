using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.WPF_pathSymbol.Ayuda
{
    class AyudaObtenerParametros
    {

        public static string ObtenerTipoBArras(string tipoPosiicon, Enumeraciones.UbicacionLosa ubicacion)
        {
            if (tipoPosiicon == "btn_crearF16a")
                return "f16a";
            if (tipoPosiicon == "btn_crearF16a")
                return "f16b";
            else if (tipoPosiicon == "btn_crearF17")
                return "f17";
            else if (tipoPosiicon == "btn_crearF18")
                return "f18";
            else if (tipoPosiicon == "btn_crearF19")
                return "f19";
            else if (tipoPosiicon == "btn_crearF20" && (ubicacion==UbicacionLosa.Izquierda || ubicacion == UbicacionLosa.Inferior))
                return "f20a";
            else if (tipoPosiicon == "btn_crearF20" && (ubicacion == UbicacionLosa.Derecha || ubicacion == UbicacionLosa.Superior)) 
                return "f20b";
            else if (tipoPosiicon == "btn_crearF21")
                return "f21";
            else if (tipoPosiicon == "btn_crearF22")
                return "f22";

            return "NONE";
        }

        internal static double ObtenerDesDereInf_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            if (tipoPosiicon == "f16a")
                return 0;//  return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_desplIzq_f16a.Text));
            else if (tipoPosiicon == "f16a")
                return 0;//  return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_desplIzq_f16a.Text));
            else if (tipoPosiicon == "f20a")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplInf_f20.Text));
            return 0;
        }

        internal static double ObtenerDesDereSup_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            if (tipoPosiicon == "f16a")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplInf_f16.Text));
            else if (tipoPosiicon == "f20a")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplSup_f20.Text));
            return 0;
        }

        internal static double ObtenerDesIzqInf_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            if (tipoPosiicon == "f16a")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplInf_f16.Text));
            if (tipoPosiicon == "f16b")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplInf_f16.Text));
            else if (tipoPosiicon == "f20a")
                return 0;
            else if (tipoPosiicon == "f20b")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplInf_f20.Text));

            return 0;
        }

        internal static double ObtenerDesIzqSup_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            if (tipoPosiicon == "f16a")
                return 0;
            if (tipoPosiicon == "f16b")
                return 0;
            else if (tipoPosiicon == "f20a")
                return 0;
            else if (tipoPosiicon == "f20b")
                return Util.CmToFoot(Util.ConvertirStringInDouble(ui_pathSymbol.textBox_DesplSup_f20.Text));
            return 0;
        }

        internal static double ObtenerpataIzq_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            return 0;
        }

        internal static double ObtenerpataDere_foot(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {
            return 0;
        }

        internal static bool ObtenerIscambiarLargos(string tipoPosiicon, Ui_pathSymbol ui_pathSymbol)
        {

           if (tipoPosiicon == "btn_crearF16a")
            {
                if ((bool)ui_pathSymbol.rbt_op_Defaul_f16.IsChecked)
                    return false;
                else
                    return true;
            }
            else if (tipoPosiicon == "btn_crearF16b")
                return  false;
            else if (tipoPosiicon == "btn_crearF17")
                return false;
            else if (tipoPosiicon == "btn_crearF18")
                return false;
            else if (tipoPosiicon == "btn_crearF19")
                return false;
            else if (tipoPosiicon == "btn_crearF20" )
                if ((bool)ui_pathSymbol.rbt_op_Defaul_f20.IsChecked)
                    return false;
                else
                    return true;
            
            else if (tipoPosiicon == "btn_crearF21")
                return false;
            else if (tipoPosiicon == "btn_crearF22")
                return false; ;

            return false;
        }
    
    }
}
