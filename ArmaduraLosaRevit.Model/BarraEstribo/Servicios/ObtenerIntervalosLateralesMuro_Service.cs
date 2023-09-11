using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class ObtenerIntervalosLateralesMuro_Service
    {
        private readonly DatosConfinamientoAutoDTO _ConfiguracionInicialEstriboDTO;

        //  private readonly DatosConfinamientoDTO _configuracionInicialEstriboDTO;
        private readonly XYZ _ptobarra1;
        private readonly XYZ _ptobarra2;
        private int _cantidadLaterales;
        private double _AnchoEstribo;
        private XYZ _direccionAnchoLAT;
        private double _AnchoEstriboMedia;
        private List<double> _espaciamietoLat;
        private double _espaciamientoBase;
        private int DiamtroLateralEstriboMM;
        private XYZ _direccionBarra;
        private string _textoLat;

        public ObtenerIntervalosLateralesMuro_Service(DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO, XYZ _ptobarra1, XYZ _ptobarra2)
        {
            _ConfiguracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            //  this._configuracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            this._ptobarra1 = _ptobarra1;
            this._ptobarra2 = _ptobarra2;
            this._cantidadLaterales = _ConfiguracionInicialEstriboDTO.cantidadLaterales;
            _AnchoEstribo = Math.Abs(_ptobarra1.AsignarZ(0).DistanceTo(_ptobarra2.AsignarZ(0)));
            _direccionAnchoLAT = (_ptobarra2.AsignarZ(0) - _ptobarra1.AsignarZ(0)).Normalize(); ;
            _AnchoEstriboMedia = _AnchoEstribo / 2;
            _espaciamietoLat = new List<double>();
            _espaciamientoBase = Util.CmToFoot(22.5);
            this.DiamtroLateralEstriboMM = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM ;

            this._direccionBarra = new XYZ(0, 0, 1);
        }

        public List<BarraLateralesDTO> M3_ObtenerLateralesEstriboMuroDTO()
        {
            List<BarraLateralesDTO> list = new List<BarraLateralesDTO>();

            List<double> listaIntervalo = ObtenerListaEspacimientoLaterales();

            for (int i = 0; i < listaIntervalo.Count; i++)
            {
                XYZ _startPont_aux = _ptobarra1 + _direccionAnchoLAT * listaIntervalo[i];
                BarraLateralesDTO barraLateralesDTO = new BarraLateralesDTO()
                {
                    _startPont_ = _startPont_aux,
                    _endPoint = _startPont_aux.AsignarZ(_ptobarra2.Z) +
                                    _direccionBarra *  (_ConfiguracionInicialEstriboDTO.IsExtenderLatFin? UtilBarras.largo_L9_DesarrolloFoot_diamMM(DiamtroLateralEstriboMM):0),
                    _diamtroLat = DiamtroLateralEstriboMM,
                    _textoLat = _textoLat
                };

                list.Add(barraLateralesDTO);
            }

            return list;
        }

    
        private List<double> ObtenerListaEspacimientoLaterales()
        {
            int diaLAt = DiamtroLateralEstriboMM;

            if (_AnchoEstribo < Util.CmToFoot(38))
            {
                //sin
            }

            else if (_AnchoEstribo < Util.CmToFoot(53) || _cantidadLaterales==1)
            {
                _textoLat = "LAT.1+1";
                _espaciamietoLat.Add(_AnchoEstriboMedia);
            }
            else if (_AnchoEstribo < Util.CmToFoot(78) || _cantidadLaterales ==2)
            {
                _textoLat = "LAT.2+2";
                _espaciamietoLat.Add(_AnchoEstriboMedia - _AnchoEstribo * 0.5 / 3);
                _espaciamietoLat.Add(_AnchoEstriboMedia + _AnchoEstribo * 0.5 / 3);
            }
            else if (_AnchoEstribo < Util.CmToFoot(103) || _cantidadLaterales == 3)
            {
                _textoLat = "LAT.3+3";
                _espaciamietoLat.Add(_AnchoEstriboMedia - _AnchoEstriboMedia / 2);
                _espaciamietoLat.Add(_AnchoEstriboMedia);
                _espaciamietoLat.Add(_AnchoEstriboMedia + _AnchoEstriboMedia / 2);
            }
            else if (_AnchoEstribo < Util.CmToFoot(128) || _cantidadLaterales == 4)
            {
                _textoLat = "LAT.4+4";
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstribo) * 1.5 / 5);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstribo) * 0.5 / 5);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstribo) * 0.5 / 5);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstribo) * 1.5 / 5);
            }
            else if (_AnchoEstribo < Util.CmToFoot(153) || _cantidadLaterales == 5)
            {
                _textoLat = "LAT.5+5";
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstriboMedia) * 1 / 3);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstriboMedia) * 2 / 3);
                _espaciamietoLat.Add(_AnchoEstriboMedia);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstriboMedia) * 1 / 3);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstriboMedia) * 2 / 3);
            }
            else if (_AnchoEstribo < Util.CmToFoot(178) || _cantidadLaterales == 6)
            {
                _textoLat = "LAT.5+5";
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstribo) * 2.5 / 7);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstribo) * 1.5 / 7);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstribo) * 0.5 / 7);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstribo) * 0.5 / 7);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstribo) * 1.5 / 7);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstribo) * 2.5 / 7);
            }

            else
            {//falsta implementar
                _textoLat = "LAT.7+7";
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstriboMedia) * 3 / 4);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstriboMedia) * 2 / 4);
                _espaciamietoLat.Add(_AnchoEstriboMedia - (_AnchoEstriboMedia) * 1 / 4);
                _espaciamietoLat.Add(_AnchoEstriboMedia);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstriboMedia) * 1 / 4);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstriboMedia) * 2 / 4);
                _espaciamietoLat.Add(_AnchoEstriboMedia + (_AnchoEstriboMedia) * 3 / 4);
            }
            return _espaciamietoLat;

        }

    }
}
