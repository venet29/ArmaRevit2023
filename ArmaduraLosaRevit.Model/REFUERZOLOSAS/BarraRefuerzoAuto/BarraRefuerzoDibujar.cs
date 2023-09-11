using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoAuto
{
   public class BarraRefuerzoDibujar
    {

        #region 0) propiedades

        #endregion
        #region 1) propiedades
        public BarraRefuerzoDibujar()
        {

        }
        #endregion
        #region 2) metodos

        public void DibujarREfuerzosLosa(List<BoundarySegmentHandler> listaRoom)
        {

            //dibujar barras
            foreach (BoundarySegmentHandler roomDatosGenerales_ in listaRoom)
            {
                foreach (WrapperBoundarySegment item in roomDatosGenerales_.newlistaBS.ListaWrapperBoundarySegment)
                {
                    item.DibujarBarraRefuerzoBordeLibres();
                    item.DibujarBarraRefuerzoTipoVigas();
                }
            }

        } 
        #endregion

    }
}
