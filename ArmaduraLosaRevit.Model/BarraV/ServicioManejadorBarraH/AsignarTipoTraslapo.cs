using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.ServicioManejadorBarraH
{
    public class AsignarTipoTraslapo
    {
    
        private  UIApplication uiapp;
        private  ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO;
        private DireccionTraslapoH _ubicacionTraslapo;
        private List<XYZ> _listaptoTramo;
        public EmpotramientoPatasDTO _empotramientoPatasDTO { get; set; }
        public AsignarTipoTraslapo(UIApplication uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO, DireccionTraslapoH _ubicacionTraslapo, List<XYZ> _listaptoTramo)
        {
            this.uiapp = uiapp;
            this._configuracionInicialBarraHorizontalDTO = confiEnfierradoDTO;
            this._ubicacionTraslapo = _ubicacionTraslapo;
            this._listaptoTramo = _listaptoTramo;
        }

     

        public bool M3_AsignarTipoTraslapo(int i)
        {
            try
            {
                TipoPataBarra TipoPataIzq_aux;
                TipoEmpotramiento _conEmpotramientoIzq_aux;
                M3_1_AsignarTipoTraslapoIzquierdo(i, out _conEmpotramientoIzq_aux, out TipoPataIzq_aux);

                TipoEmpotramiento _conEmpotramientoDere_aux;
                TipoPataBarra TipoPataDere_aux;
                M3_2_AsignarTipoTraslapoDereco(i, out _conEmpotramientoDere_aux, out TipoPataDere_aux);

                _empotramientoPatasDTO = new EmpotramientoPatasDTO()
                {
                    _conEmpotramientoIzqInf = _conEmpotramientoIzq_aux,
                    _conEmpotramientoDereSup = _conEmpotramientoDere_aux,
                    TipoPataIzqInf = TipoPataIzq_aux, // TipoPataBarra.BarraVSinPatas //(i==0? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar)
                    TipoPataDereSup = TipoPataDere_aux,//((_listaptoTramo.Count - 2)==i ? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar),
                };
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool M3_AsignarTipoTraslapo_haciaIzq(int i)
        {
            try
            {
                TipoPataBarra TipoPataIzq_aux;
                TipoEmpotramiento _conEmpotramientoIzq_aux;
                M3_1_AsignarTipoTraslapoIzquierdo(i, out _conEmpotramientoIzq_aux, out TipoPataIzq_aux);

                TipoEmpotramiento _conEmpotramientoDere_aux;
                TipoPataBarra TipoPataDere_aux;
                M3_2_AsignarTipoTraslapoDereco(i, out _conEmpotramientoDere_aux, out TipoPataDere_aux);

                _empotramientoPatasDTO = new EmpotramientoPatasDTO()
                {
                    _conEmpotramientoIzqInf = _conEmpotramientoIzq_aux,
                    _conEmpotramientoDereSup = _conEmpotramientoDere_aux,
                    TipoPataIzqInf = TipoPataIzq_aux, //(i==0? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar)
                    TipoPataDereSup = TipoPataBarra.BarraVSinPatas,//((_listaptoTramo.Count - 2)==i ? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar),
                };
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public bool M3_AsignarTipoTraslapo_haciaDere(int i)
        {
            try
            {
                TipoPataBarra TipoPataIzq_aux;
                TipoEmpotramiento _conEmpotramientoIzq_aux;
                M3_1_AsignarTipoTraslapoIzquierdo(i, out _conEmpotramientoIzq_aux, out TipoPataIzq_aux);

                TipoEmpotramiento _conEmpotramientoDere_aux;
                TipoPataBarra TipoPataDere_aux;
                M3_2_AsignarTipoTraslapoDereco(i, out _conEmpotramientoDere_aux, out TipoPataDere_aux);

                _empotramientoPatasDTO = new EmpotramientoPatasDTO()
                {
                    _conEmpotramientoIzqInf = _conEmpotramientoIzq_aux,
                    _conEmpotramientoDereSup = _conEmpotramientoDere_aux,
                    TipoPataIzqInf = TipoPataBarra.BarraVSinPatas, //(i==0? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar)
                    TipoPataDereSup = TipoPataDere_aux,//((_listaptoTramo.Count - 2)==i ? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar),
                };
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        private void M3_1_AsignarTipoTraslapoIzquierdo(int i, out TipoEmpotramiento _conEmpotramientoIzq_aux, out TipoPataBarra TipoPataIzq_aux)
        {
            TipoPataIzq_aux = TipoPataBarra.BarraVPataAUTO;

            if (i == 0) //inicio
            {

                switch (_configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH)
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
                    case TipoPataBarra.BarraVSinPatas:
                        TipoPataIzq_aux = TipoPataBarra.BarraVSinPatas;
                        break;
                    case TipoPataBarra.BuscarSinExtender:
                        TipoPataIzq_aux = TipoPataBarra.BuscarSinExtender;
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

        private void M3_2_AsignarTipoTraslapoDereco(int i, out TipoEmpotramiento _conEmpotramientoDere_aux, out TipoPataBarra TipoPataDere_aux)
        {
            TipoPataDere_aux = TipoPataBarra.BarraVPataAUTO;
            //final
            if ((_listaptoTramo.Count - 2) == i) //fin
            {
                switch (_configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH)
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
                    case TipoPataBarra.BarraVSinPatas:
                        TipoPataDere_aux = TipoPataBarra.BarraVSinPatas;
                        break;
                    case TipoPataBarra.BuscarSinExtender:
                        TipoPataDere_aux = TipoPataBarra.BuscarSinExtender;
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
