using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
    public  class FactoryICasoBarra
    {

        public static ICasoBarraX ObtenerICasoyBarraX( SolicitudBarraDTO _solicitudBarraDTO, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            string TipoBarraStr = _solicitudBarraDTO.TipoBarra;
            ICasoBarraX iTipoBarra = null;

            if (TipoBarraStr.Contains("f16"))
            {
                //   ICasoBarra iTipoBarra = new Barra16(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f3"))
            {
                return new BarraF3(_solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f11"))
            {
                return new BarraF11( _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f17"))
            {
                //  ICasoBarra iTipoBarra = new BarraF17(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f18"))
            {
                //   ICasoBarra iTipoBarra = new BarraF18(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f19"))
            {
                //  ICasoBarra iTipoBarra = new BarraF19(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f20"))
            {
                //  ICasoBarra iTipoBarra = new BarraF20(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }
            else if (TipoBarraStr.Contains("f21"))
            {
                //ICasoBarra iTipoBarra = new Barra11(this, _solicitudBarraDTO, _datosNuevaBarraDTO);

            }

            return iTipoBarra;
        }

        public static ICasoBarraX ObtenerICasoyBarraXFunda(SolicitudBarraDTO _solicitudBarraDTO, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            return new BarraF3(_solicitudBarraDTO, _datosNuevaBarraDTO);
        }




    }
}
