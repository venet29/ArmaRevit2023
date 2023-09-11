using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
   public class DatosDiseño
    {
        public static TipoDiseño_F1 DISENO_TIPO_F1 = TipoDiseño_F1.f1;

        public static TipoValidarEspesor DISENO_VALIDAR_ESPESOR = TipoValidarEspesor.NOVerificarEspesorMenor15;  //VALIDAR SI ESPESOR ES MANOR DE 15( MES ESPESOR) 

        public static bool ISconAHORRO = false;

        public static bool IS_PATHREIN_AJUSTADO = true;
        public static bool IS_PATHREIN_AJUSTADO_LARGO = true;

        public static bool IsMuroParaleloView { get; set; } = true;
    }
}
