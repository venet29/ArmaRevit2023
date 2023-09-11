using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.EditarPath;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag
{
    public class ManejarEditTagUpdate_MoverInterCAmbiar_F_L : ManejarEditTagUpdate_MoverConfiguracionInicial
    {




        public ManejarEditTagUpdate_MoverInterCAmbiar_F_L(UIApplication uiapp, PathReinforcement pathReinforcement, XYZ ptoPathSymbol, List<IndependentTag> listaIndependentTag)
           : base(uiapp, pathReinforcement, ptoPathSymbol, listaIndependentTag)
        {



        }


        public bool Ejecutar_F_L()
        {
            try
            {

                if (!M1_ObtenerTipos()) return false;
                // M2_SeleccionarRoom();
                if (!M3_ObtenerBordeDepath()) return false;
                if (!M4_ObtenerTag()) return false;

                var resul = _listaTAgBArra.listaTag.Where(c => c.nombreFamilia == "" && c.IsOk == true).FirstOrDefault();
                //M5_Intercambiar();

            }
            catch (Exception ex)
            {

                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }
            return IsOK;
        }


        private void M5_Intercambiar()
        {
            try
            {
                //filtrar
                var _listaIndependentTagAnoy = _listaIndependentTag.Select(c => new TagPathReinProcesado() { tagPath = c, Isprocesado = false }).ToList();


                for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                {
                    var item = _listaIndependentTagAnoy[i];

                    if (item.Isprocesado == true) continue;

                    var resul = _listaTAgBArra.listaTag.Where(c => c.nombreFamilia == item.tagPath.Name && c.IsOk == true).ToList();

         
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
            }
        }

    }
}
