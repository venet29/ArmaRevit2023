using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarTipoPath.Verificar
{
    public class VerificarTiposEncambiosDeBarra
    {
        private TipoBarra _nuevotipobarra;
        private TipoBarra _actualTipobarra;

        public VerificarTiposEncambiosDeBarra(TipoBarra Nuevotipobarra, TipoBarra actualTipobarra)
        {
            this._nuevotipobarra = Nuevotipobarra;
            this._actualTipobarra = actualTipobarra;
        }

        public bool Verificar()
        {

            switch (_actualTipobarra)
            {
                case TipoBarra.s1:
                    return VerificarSX();
                case TipoBarra.s2:
                    return VerificarSX();
                case TipoBarra.s4:
                    return VerificarSX();
                default:
                    return Verificarfx();
            }

        }

        private bool Verificarfx()
        {
            if (_nuevotipobarra == TipoBarra.s1 || _nuevotipobarra == TipoBarra.s2 ||
                 _nuevotipobarra == TipoBarra.s3 || _nuevotipobarra == TipoBarra.s4)
            {
                Util.ErrorMsg($"No se puede cambiar una barra tipo {_actualTipobarra.ToString()}  --> en barra tipo {_nuevotipobarra.ToString()}");
                return false; 
            }

            return true;
        }

        //private bool VerificarS2()
        //{
        //    if (_nuevotipobarra == TipoBarra.s1)
        //        return true;
        //    else if (_nuevotipobarra == TipoBarra.s2)
        //        return true;
        //    else
        //    {
        //        Util.ErrorMsg($"No se puede cambiar una barra tipo {_actualTipobarra.ToString()}  --> en barra tipo {_nuevotipobarra.ToString()}");
        //        return false;
        //    }

        //}

        private bool VerificarSX()
        {
            if (_nuevotipobarra == TipoBarra.s1)
                return true;
            else if (_nuevotipobarra == TipoBarra.s2)
                return true;
            else if (_nuevotipobarra == TipoBarra.s4)
                return true;
            else
            {
                Util.ErrorMsg($"No se puede cambiar una barra tipo {_actualTipobarra.ToString()}  --> en barra tipo {_nuevotipobarra.ToString()}");
                return false;
            }
        }
    }
}
