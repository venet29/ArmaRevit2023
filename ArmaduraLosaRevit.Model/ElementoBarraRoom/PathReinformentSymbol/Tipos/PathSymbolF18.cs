using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos
{

    class PathSymbolF18 : PathSymbolF19, IPathSymbol
    {
       

        public PathSymbolF18(UIApplication uiapp, DatosNuevaBarraDTO _DatosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _PathSymbolF20ADTO) : base(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20ADTO)
        {
 
        }


        public override bool M1_ObtenerPArametros()
        {
            try
            {
                M1_1_OtenerPArametrosBAsePOrescala();

                double largoDesfase_foot = _datosNuevaBarraDTO.LargoPataFoot();

                PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesDereSup", (largoDesfase_foot + ConstNH.CONST_EXTRADESDFASE_FOOT) / _scaleView);
                _listaPara_pathSymbol.Add(_pDS_DesDereSup);
                PathReinforceSymbolDTO _pDS_DesDereInf = new PathReinforceSymbolDTO("DesIzqInf", (largoDesfase_foot + ConstNH.CONST_EXTRADESDFASE_FOOT) / _scaleView);
                _listaPara_pathSymbol.Add(_pDS_DesDereInf);

                nombreFamiliaCOnPata = _datosNuevaBarraDTO.ObtenerNombrePAthSymbol_SoloEscala(_scala) + "_D" + Util.FootToCm(largoDesfase_foot).ToString();

                ListaTipoFamilia_conpata.Add(new ParametroNewTipoPathSymbol(nombreFamiliaCOnPata, _listaPara_pathSymbol));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'PathSymbolF20'  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
