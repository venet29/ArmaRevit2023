using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Empotramiento
{
   public class EmpotramientoVertical
    {
        private readonly int _cantidad;
        private TipoPataBarra _inicial_tipoBarraH;
        private DireccionTraslapoH _ubicacionTraslapo;

        public EmpotramientoPatasDTO _empotramientoPatasDTO { get;  set; }
        public TipoEmpotramiento _conEmpotramientoIzq_aux { get;  set; }
        public TipoPataBarra TipoPataIzq_aux { get;  set; }
        public TipoEmpotramiento _conEmpotramientoDere_aux { get;  set; }
        public TipoPataBarra TipoPataDere_aux { get;  set; }

        public EmpotramientoVertical(int cantidad, TipoPataBarra inicial_tipoBarraH, DireccionTraslapoH ubicacionTraslapo )
        {
            this._cantidad = cantidad;
            this._inicial_tipoBarraH = inicial_tipoBarraH;
            this._ubicacionTraslapo = ubicacionTraslapo;
        }
        private void M3_AsignarTipoTraslapo(int i)
        {

            M3_1_AsignarTipoTraslapoIzquierdo(i);


            M3_2_AsignarTipoTraslapoDereco(i);

            _empotramientoPatasDTO = new EmpotramientoPatasDTO()
            {
                _conEmpotramientoIzqInf = _conEmpotramientoIzq_aux,
                _conEmpotramientoDereSup = _conEmpotramientoDere_aux,
                TipoPataIzqInf = TipoPataIzq_aux, //(i==0? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar)
                TipoPataDereSup = TipoPataDere_aux,//((_listaptoTramo.Count - 2)==i ? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar),
            };
        }

        private void M3_1_AsignarTipoTraslapoIzquierdo(int i)
        {
            TipoPataIzq_aux = TipoPataBarra.BarraVPataAUTO;

            if (i == 0) //inicio
            {

                switch (_inicial_tipoBarraH)
                {
                    case TipoPataBarra.BarraVPataAUTO:
                        TipoPataIzq_aux = TipoPataBarra.BarraVPataAUTO;
                        break;
                    case TipoPataBarra.BarraVPataInicial:
                        TipoPataIzq_aux = TipoPataBarra.BarraVPataInicial;
                        break;
                    case TipoPataBarra.BarraVPataFinal:
                        TipoPataIzq_aux = TipoPataBarra.NoBuscar;
                        break;
                    case TipoPataBarra.BarraVPataAmbos:
                        TipoPataIzq_aux = TipoPataBarra.BarraVPataInicial;
                        break;
                    case TipoPataBarra.NoBuscar:
                        TipoPataIzq_aux = TipoPataBarra.NoBuscar;
                        break;
                };

                _conEmpotramientoIzq_aux = TipoEmpotramiento.total;
            }
            else
            {
                TipoPataIzq_aux = TipoPataBarra.NoBuscar;
                switch (_ubicacionTraslapo)
                {
                    case DireccionTraslapoH.derecha:
                        _conEmpotramientoIzq_aux = TipoEmpotramiento.sin;
                        break;
                    case DireccionTraslapoH.central:
                        _conEmpotramientoIzq_aux = TipoEmpotramiento.mitad;
                        break;
                    case DireccionTraslapoH.izquierda:
                        _conEmpotramientoIzq_aux = TipoEmpotramiento.total;
                        break;
                    default:
                        _conEmpotramientoIzq_aux = TipoEmpotramiento.mitad;
                        break;
                }

            }


        }

        private void M3_2_AsignarTipoTraslapoDereco(int i)
        {
            TipoPataDere_aux = TipoPataBarra.BarraVPataAUTO;
            //final
            if ((_cantidad - 2) == i) //fin
            {
                switch (_inicial_tipoBarraH)
                {
                    case TipoPataBarra.BarraVPataAUTO:
                        TipoPataDere_aux = TipoPataBarra.BarraVPataAUTO;
                        break;
                    case TipoPataBarra.BarraVPataInicial:
                        TipoPataDere_aux = TipoPataBarra.NoBuscar;
                        break;
                    case TipoPataBarra.BarraVPataFinal:
                        TipoPataDere_aux = TipoPataBarra.BarraVPataFinal;
                        break;
                    case TipoPataBarra.BarraVPataAmbos:
                        TipoPataDere_aux = TipoPataBarra.BarraVPataFinal;
                        break;
                    case TipoPataBarra.NoBuscar:
                        TipoPataDere_aux = TipoPataBarra.NoBuscar;
                        break;
                };

                _conEmpotramientoDere_aux = TipoEmpotramiento.total;
            }
            else
            {
                TipoPataDere_aux = TipoPataBarra.NoBuscar;

                switch (_ubicacionTraslapo)
                {
                    case DireccionTraslapoH.derecha:
                        _conEmpotramientoDere_aux = TipoEmpotramiento.total;
                        break;
                    case DireccionTraslapoH.central:
                        _conEmpotramientoDere_aux = TipoEmpotramiento.mitad;
                        break;
                    case DireccionTraslapoH.izquierda:
                        _conEmpotramientoDere_aux = TipoEmpotramiento.sin;
                        break;
                    default:
                        _conEmpotramientoDere_aux = TipoEmpotramiento.mitad;
                        break;
                }
            }
        }

    }
}
