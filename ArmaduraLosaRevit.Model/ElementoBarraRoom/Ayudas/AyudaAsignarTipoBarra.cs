using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public class AyudaAsignarTipoBarra
    {
        public TipoRebar _tipoRebar { get; set; }
        public AyudaAsignarTipoBarra()
        {

        }
        public bool AsignarBarrasInferiore(string tipoBarra)
        {

            try
            {
                if (tipoBarra == "")
                { }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public bool AsignarBarraSuperior(string tipoBarra)
        {

            try
            {
                if (tipoBarra == "s1")
                    _tipoRebar = TipoRebar.LOSA_SUP_S1;
                else if (tipoBarra == "s2")
                    _tipoRebar = TipoRebar.LOSA_SUP_S2;
                else if (tipoBarra == "s3")
                    _tipoRebar = TipoRebar.LOSA_SUP_S3;
                else if (tipoBarra == "s4")
                    _tipoRebar = TipoRebar.LOSA_SUP_S4;
                else
                    return false;

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
