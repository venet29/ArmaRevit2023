using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class ObtenerIntervalosLateralesViga_Service
    {
        private readonly DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;
        private readonly XYZ _ptobarra1;
        private readonly XYZ _ptobarra2;
        private readonly View view;
        private double _alturaEstribo;
        private double _alturaEstriboMedia;
        private List<double> _espaciamietoLat;
        private double _espaciamientoBase;
        private double _diametroEstribo_foot;
        private double _diametroLat_foot;
        private bool _IsExtenderLatInicio;
        private bool _IsExtenderLatFin;
        private XYZ _direccionBarra;
        private string _textoLat;

        public ObtenerIntervalosLateralesViga_Service(DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO, XYZ _ptobarra1, XYZ _ptobarra2, View _view)
        {
            this._configuracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            this._ptobarra1 = _ptobarra1;
            this._ptobarra2 = _ptobarra2;
            view = _view;
            _alturaEstribo = Math.Abs(_ptobarra1.Z - _ptobarra2.Z)+ ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT*2;
            _alturaEstriboMedia = _alturaEstribo / 2;

            _espaciamietoLat = new List<double>();
            _espaciamientoBase = Util.CmToFoot(22.5);
            _diametroEstribo_foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);
            _diametroLat_foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) / 2;
            _IsExtenderLatInicio = _configuracionInicialEstriboDTO.IsExtenderLatInicio;
            _IsExtenderLatFin = _configuracionInicialEstriboDTO.IsExtenderLatFin;
            this._direccionBarra = (_ptobarra2 - _ptobarra1).AsignarZ(0).Normalize();
        }

        public List<BarraLateralesDTO> M3_ObtenerLateralesEstriboVigaDTO()
        {
            List<BarraLateralesDTO> list = new List<BarraLateralesDTO>();

            List<double> listaIntervalo = (_configuracionInicialEstriboDTO.cantidadLaterales == 0
                                                ? ObtenerListaEspacimientoLateralesAuto()
                                                : ObtenerListaEspacimientoLateralesNumero());

            double ProlonInicial = (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);
            double ProlonFinal = (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);

            for (int i = 0; i < listaIntervalo.Count; i++)
            {
                BarraLateralesDTO barraLateralesDTO = new BarraLateralesDTO()
                {
                    _startPont_ = _ptobarra1 - view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) - _direccionBarra * ProlonInicial,
                    _endPoint = _ptobarra2.AsignarZ(_ptobarra1.Z) - view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) + _direccionBarra * ProlonFinal,
                    _diamtroLat = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM,
                    _textoLat = _textoLat
                };

                list.Add(barraLateralesDTO);
            }

            return list;
        }

        private List<double> ObtenerListaEspacimientoLateralesAuto()
        {
            int diaLAt = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM;

            if (_alturaEstribo < Util.CmToFoot(38))
            {
               

            }
           else if (_alturaEstribo < Util.CmToFoot(53))
            {
                _textoLat = "LAT.1+1";
                _espaciamietoLat.Add(_alturaEstriboMedia);

            }
            else if (_alturaEstribo < Util.CmToFoot(78))
            {
                _textoLat = "LAT.2+2";
                //_espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo *0.5/ 3);
                //_espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo *0.5/ 3);

                _espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo * 0.5 / 3);
                _espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo * 0.5 / 3);
            }
            else if (_alturaEstribo < Util.CmToFoot(103))
            {
                _textoLat = "LAT.3+3";
                //_espaciamietoLat.Add(_alturaEstribo / 4);
                //_espaciamietoLat.Add(_alturaEstribo * 2 / 4);
                //_espaciamietoLat.Add(_alturaEstribo * 3 / 4);

                _espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstriboMedia / 2);
                _espaciamietoLat.Add(_alturaEstriboMedia);
                _espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstriboMedia / 2);
            }
            else if (_alturaEstribo < Util.CmToFoot(128))
            {
                _textoLat = "LAT.4+4";
                _espaciamietoLat.Add(_alturaEstribo / 5);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 5);
            }
            else if (_alturaEstribo < Util.CmToFoot(153))
            {
                _textoLat = "LAT.5+5";
                _espaciamietoLat.Add(_alturaEstribo / 6);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 6);
            }
            else if (_alturaEstribo < Util.CmToFoot(178))
            {
                _textoLat = "LAT.6+6";
                _espaciamietoLat.Add(_alturaEstribo / 7);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 7);
            }
            else
            {
                _textoLat = "LAT.7+7";
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            return _espaciamietoLat;

        }


       

        private List<double> ObtenerListaEspacimientoLateralesNumero()
        {

            int diaLAt = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM;
            if (_configuracionInicialEstriboDTO.cantidadLaterales == 1)
            {
                _textoLat = "LAT.1+1Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstriboMedia);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 2)
            {
                _textoLat = "LAT.2+2Ø" + diaLAt;
                //_espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo *0.5/ 3);
                //_espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo *0.5/ 3);

                _espaciamietoLat.Add(_alturaEstribo / 3);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 3);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 3)
            {
                _textoLat = "LAT.3+3Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 4);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 4);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 4);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 4)
            {
                _textoLat = "LAT.4+4Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 5);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 5);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 5)
            {
                _textoLat = "LAT.5+5Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 6);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 6);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 6)
            {
                _textoLat = "LAT.5+5Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 7);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 7);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 7)
            {
                _textoLat = "LAT.7+7Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            else
            {
                _textoLat = "LAT.7+7Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            return _espaciamietoLat;

        }
    }
}
