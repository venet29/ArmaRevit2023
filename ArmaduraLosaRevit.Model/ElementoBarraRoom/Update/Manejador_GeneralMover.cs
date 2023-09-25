using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update
{
    public class Manejador_GeneralMover
    {
        public static void EjecutarMOver(UIApplication _uiapp, PathReinforcement pathRein, XYZ tagHeadPosition, List<IndependentTag> listaIndependentTag, TipoMoverTag _TipoMoverTag )
        {
            if (_TipoMoverTag == TipoMoverTag.MoverConfiguracionInicial)
            {
                //mantener tag como configuracion inicial
                ManejarEditTagUpdate_MoverConfiguracionInicial _ManejarEditTagUpdate_move = new ManejarEditTagUpdate_MoverConfiguracionInicial(_uiapp, pathRein, tagHeadPosition, listaIndependentTag);
                _ManejarEditTagUpdate_move.Ejecutar_SinTrans();
            }
            else if (_TipoMoverTag == TipoMoverTag.MoverMantenerTODO)
            {
                ManejarEditTagUpdate_MoverMantenerTODO _ManejarEditTagUpdate_MoverMantenerTODO = new ManejarEditTagUpdate_MoverMantenerTODO(_uiapp, pathRein, tagHeadPosition, listaIndependentTag);
                _ManejarEditTagUpdate_MoverMantenerTODO.Ejecutar();
            }
            else if (_TipoMoverTag == TipoMoverTag.MoverMantenerSOLOExtremos)
            {
                ManejarEditTagUpdate_MoverMantenerSOLOExtremos _ManejarEditTagUpdate_MoverMantenerSOLOExtremos = new ManejarEditTagUpdate_MoverMantenerSOLOExtremos(_uiapp, pathRein, tagHeadPosition, listaIndependentTag);
                _ManejarEditTagUpdate_MoverMantenerSOLOExtremos.Ejecutar();
            }
        }
    }
}
