using ArmaduraLosaRevit.Model.BarraMallaRebar.TIpoBarra;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Tag;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.TIpoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Horizontal;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoBarra;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public class FactoryBarraVertical
    {
        public static IbarraBase FActoryIGeometriaTagVertical(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO)
        {
            IGeometriaTag _newGeometriaTag = null;
            if (itemIntervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.Cabeza || itemIntervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.Cabeza_BarraVHorquilla)
            {
                if (_confiEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Automatico)
                {
                    _newGeometriaTag = FactoryGeomTagRebarV.CrearGeometriaTagV(uiapp,
                                                                               itemIntervaloBarrasDTO.tipobarraV,
                                                                               itemIntervaloBarrasDTO.ptoini,
                                                                               itemIntervaloBarrasDTO.ptofinal,
                                                                               itemIntervaloBarrasDTO.ptoPosicionTAg,
                                                                              (itemIntervaloBarrasDTO.OrientacionTagGrupoBarras != itemIntervaloBarrasDTO.Orientacion ? -1 : 1) * -itemIntervaloBarrasDTO.DireccionPataEnFierrado * 2);
                }
                else
                {
                    _newGeometriaTag = FactoryGeomTagRebarV.CrearGeometriaTagV(uiapp,
                                                                                itemIntervaloBarrasDTO.tipobarraV,
                                                                                itemIntervaloBarrasDTO.ptoini,
                                                                                itemIntervaloBarrasDTO.ptofinal,
                                                                                itemIntervaloBarrasDTO.ptoPosicionTAg,
                                                                               (_confiEnfierradoDTO.IsInvertirPosicionTag ? -1 : 1) * -itemIntervaloBarrasDTO.DireccionPataEnFierrado * 2);
                }
            }
            else if (itemIntervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.MallaV ||
                itemIntervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.MallaH )
                _newGeometriaTag = new GeomeTagMalla(uiapp.ActiveUIDocument.Document, itemIntervaloBarrasDTO.ptoPosicionTAg, itemIntervaloBarrasDTO.ptoPosicionTAg, itemIntervaloBarrasDTO.ptoPosicionTAg);
            else if (itemIntervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.Cabeza_Horquilla)
                _newGeometriaTag = new GeomeTagPataInicialHOrquilla(uiapp, itemIntervaloBarrasDTO );

            _newGeometriaTag.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });

            return ObtenerIbarrav(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
        }




        //serve para caso vertical como horizon tal
        public static IbarraBase FActoryIGeneraraTagVertical_Horizontal_cambiarBarras(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, TipoCasobarra tipoCasobarra)
        {
            IGeometriaTag _newGeometriaTag = null;
            if (tipoCasobarra == TipoCasobarra.BarraVertical)
                _newGeometriaTag = FactoryGeomTagRebarV.CrearGeometriaTagV(uiapp, itemIntervaloBarrasDTO.tipobarraV, itemIntervaloBarrasDTO.ptoini, itemIntervaloBarrasDTO.ptofinal, itemIntervaloBarrasDTO.ptoPosicionTAg, new XYZ(0, 0, 0));
            else if (tipoCasobarra == TipoCasobarra.BarrasHorizontal)
                _newGeometriaTag = FactoryGeomTagRebarH.CrearGeometriaTagH(uiapp, itemIntervaloBarrasDTO.tipobarraV, itemIntervaloBarrasDTO.ptoini, itemIntervaloBarrasDTO.ptofinal, itemIntervaloBarrasDTO.ptoPosicionTAg,
                       -itemIntervaloBarrasDTO.DireccionPataEnFierrado * (itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z == 1 ? 0.8 : 0.2));
            else if (tipoCasobarra == TipoCasobarra.BarraRefuerzoLosa)
                _newGeometriaTag = FactoryGeomTagRebarH.CrearGeometriaTagH(uiapp, itemIntervaloBarrasDTO.tipobarraV, itemIntervaloBarrasDTO.ptoini, itemIntervaloBarrasDTO.ptofinal, itemIntervaloBarrasDTO.ptoPosicionTAg,
                                                                                               -itemIntervaloBarrasDTO.DireccionPataEnFierrado * (itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z == 1 ? 0.8 : 0.2));
            _newGeometriaTag.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });

            if (tipoCasobarra == TipoCasobarra.BarraVertical)
                return ObtenerIbarrav(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
            else if (tipoCasobarra == TipoCasobarra.BarrasHorizontal)
                return ObtenerIbarraH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
            else // if (tipoCasobarra == TipoCasobarra.BarraRefuerzoLosa)
                return ObtenerIbarrarRefuerzo(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag); //caso refuerzo-- revisar
        }

        private static IbarraBase ObtenerIbarrarRefuerzo(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, IGeometriaTag _newGeometriaTag)
        {
            switch (itemIntervaloBarrasDTO.tipobarraV)
            {
                case Enumeraciones.TipoPataBarra.BarraVPataInicial:
                    // IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataInicialRefuerzo(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataFinal:
                    //  IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataFinalRefuerzo(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVSinPatas:

                    return new BarraVSinPatasRefuerzo(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataAmbos:
                    //IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataAmbosRefuerzo(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                default:
                    return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
            }
        }

        private static IbarraBase ObtenerIbarrav(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, IGeometriaTag _newGeometriaTag)
        {
            if (itemIntervaloBarrasDTO.BarraTipo == TipoRebar.ELEV_MA_H)
            {
                switch (itemIntervaloBarrasDTO._tipoLineaMallaH)
                {
                    case TipoPataBarra.BarraVPataInicial:
                        return new MallaHPataInicial(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataFinal:
                        return new MallaHPataFinal(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVSinPatas:
                        return new MallaHSinPatas(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataAmbos:
                        return new MallaHPataAmbos(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    default:
                        return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                }

            }
            else if (itemIntervaloBarrasDTO.BarraTipo == TipoRebar.ELEV_MA_V)
            {
                switch (itemIntervaloBarrasDTO.tipobarraV)
                {
                    case TipoPataBarra.BarraVPataInicial:
                        return new MallaVPataInicial(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataFinal:
                        return new MallaVPataFinal(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVSinPatas:
                        return new MallaVSinPatas(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataAmbos:
                        return new MallaVPataAmbos(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    default:
                        return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                }

            }
            else if (itemIntervaloBarrasDTO.BarraTipo == TipoRebar.ELEV_BA_HORQ)
            {
                switch (itemIntervaloBarrasDTO._tipoLineaMallaH)
                {
                    case TipoPataBarra.BarraVPataInicial:
                        return new HorquillaPataInicial(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataFinal:// no se utiliza, horqui  se dibuja a la izq o dere siempre usar  'BarraVPataInicial'
                        return new HorquillaPataFinal(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case TipoPataBarra.BarraVPataAmbos_Horquilla:
                        return new BarraVPataAmbos_Horquilla(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);

                    default:
                        return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                }

            }
            else
            {

                switch (itemIntervaloBarrasDTO.tipobarraV)
                {
                    case Enumeraciones.TipoPataBarra.BarraVPataInicial:
                        // IGeometriaTag _newGeometriaTag = null;
                        return new BarraVPataInicial(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case Enumeraciones.TipoPataBarra.BarraVPataFinal:
                        //  IGeometriaTag _newGeometriaTag = null;
                        return new BarraVPataFinal(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case Enumeraciones.TipoPataBarra.BarraVSinPatas:

                        return new BarraVSinPatas(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case Enumeraciones.TipoPataBarra.BarraVPataAmbos:
                        //IGeometriaTag _newGeometriaTag = null;
                        return new BarraVPataAmbos(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    case Enumeraciones.TipoPataBarra.BarraVPataAmbos_Horquilla:
                        return new BarraVPataAmbos_Horquilla(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);

                    default:
                        return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                }
            }
        }

        private static IbarraBase ObtenerIbarraH(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, IGeometriaTag _newGeometriaTag)
        {
            switch (itemIntervaloBarrasDTO.tipobarraV)
            {
                case Enumeraciones.TipoPataBarra.BarraVPataInicial:
                    // IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataInicialH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataFinal:
                    //  IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataFinalH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVSinPatas:

                    return new BarraVSinPatasH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataAmbos:
                    //IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataAmbosH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                default:
                    return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
            }
        }

    }
}
