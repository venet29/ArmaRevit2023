using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
    public enum tipoVIewNh { Todas_las_View, Solo_Elevaciones, Solo_Losa_y_Fundacion, NONE }
    public enum FijacionRebar { fijo, movil }

    public enum TipoPAthDefinir { PathInicial, PathFinal, Mitad, Normal }

    public enum TipoDisenoEstriboVIga { AsignarViga, SeleccionarViga, BarramenosRecub, SoloBarra, NONE }

    public enum CasosBorrarFamilias
    {
        TODOS, SOLOBARRAS, Otros,
        NONE
    }

    public enum TipoBarraRefuerzoViga { RefuerzoCentral, RefuerzoBorde, RefuerzoVigas,NONE }

    public enum BuscarMurosEstado { Inicial,MuroEncontrado, MuroNoEncontrado,MurosEncontradoConerror }
    public enum FormaDibujarPAth { Normal, Inicial, Final, mouse }
    public enum TipoCategoria { Principal, Secuandario }
    public enum UbicacionEnPier { izquierda, centro, derecha }
    public enum Orientacion { izquierda, centro, derecha }
    public enum TipoBarraConfig { primaria, alternativa }
    public enum CasoBarra { NH_F1A, NH_F1B, M_00, NH4_bajo, NONE, NH_F7A, NH_F11_v2, NH_F11, NH_F7B }

    public enum VersionREvitNh { v2018, v2019, v2020, v2021, v2022, v2023, v2024, v2025 }
    public enum TipoCasoAlternativo_enum { Proporcional, MantenerLargo, Definir, NONE }

    public enum TipoMoverTag
    {
        MoverConfiguracionInicial, MoverMantenerTODO, MoverMantenerExtremo, NONE,
        MoverMantenerSOLOExtremos
    }

    public enum UbicacionLosa { Derecha, Izquierda, Superior, Inferior, NONE }
    public enum TipoCaraObjeto { Inferior, Superior, Vertical,
        Izquierdo,
        Derecho
    }

    public enum LargoEstriboEnbarra { Inicial=0,Centrol=1, Final=2, Completa=3} // enum se utiliza para definir el largo del estribo cuando se crea automaticamente, una viga puede tener estribo al inicio, centro y fin
    public enum TipoCaraUbicacion { Inferior, Superior }

    public enum TipoCambioFund { CambiarDatos, CambiarGeom }

    public enum TipoSeleccion { ConMouse, ConElemento } //enum utilzado par seleccion borde, ptoinicial,pto final en diseño de barras y malla manual

    public enum TipoRefuerzoMuroSUple { Exterior, Interior }
    public enum DireccionRecorrido_ { DireccionZ, ParaleloDerechaVista, PerpendicularEntradoVista, Manual }

    public enum TipoCasobarra { BarrasHorizontal, BarraVertical, BarraRefuerzoLosa } //ob3)  horizontal  angulo=0 ,  vertical   angulo!=0

    public enum TipoRebar
    {
        ELEV_BA_H, ELEV_BA_V, ELEV_BA_CABEZA_HORQ, ELEV_BA_HORQ,
        ELEV_CO, ELEV_CO_T, ELEV_ES, ELEV_ES_L, ELEV_ES_T, ELEV_ES_V, ELEV_ES_VL, ELEV_ES_VT, ELEV_MA_H, ELEV_MA_V,
        ELEV_ESCA, ELEV_ESC, ELEV_HORQ, ELEV_REFUERZO_MURO, ELEV_MA_T,
        REFUERZO_ES,
        NONE,
        REFUERZO_BA,
        REFUERZO_BA_CAB_MURO,
        REFUERZO_BA_REF_LO,
        REFUERZO_BA_BORDE,
        REFUERZO_SUPLE_CAB_MU,
        REFUERZO_EST_REF_LO,
        REFUERZO_EST_BORDE,
        LOSA_INF,
        LOSA_SUP,
        LOSA_SUP_S1,
        LOSA_SUP_S2,
        LOSA_SUP_S3,
        LOSA_SUP_S4,
        LOSA_ESC_F3_45,
        LOSA_ESC_F3_135,
        LOSA_ESC_F3B_45,
        LOSA_ESC_F3B_135,
        LOSA_ESC_F1_45,
        LOSA_ESC_F1_45_CONPATA,
        LOSA_ESC_F1_135_SINPATA,
        LOSA_ESC_F1B,
        LOSA_ESC_F1A,
        LOSA_ESC_F3_BA,
        LOSA_ESC_F3_AB,
        LOSA_ESC_F3_0A,
        LOSA_ESC_F3_A0,
        LOSA_ESC_F3_0B,
        LOSA_ESC_F3_B0,
        LOSA_INCLI_F3,
        LOSA_INCLI_F1,
        LOSA_INCLI_F4,
        LOSA_SUP_BR,
        LOSA_SUP_BPT,
        FUND_BA,
        FUND_TRABA,
        FUND_ES,
        FUND_ES_T,        
        FUND_BA_INF,
        FUND_BA_SUP,
        FUND_BA_BPT,//borrar
        FUND_SUP_BPT,
        LOSA_ESC_F1_AB,
    }



    public enum TipoBarraVertical
    {
        Cabeza, MallaV, MallaH,
        MallaH_Horq,
        Cabeza_Horquilla,
        Cabeza_BarraVHorquilla
    }
    public enum CasoAnalisasBarrasElevacion { Manual, Automatico }

    public enum ParametroLetra
    {
        A_, B_, C_, D_, E_, F_,
        C2_,
        L2barra,
    }
    public enum TabEditarPath { Ambos, Datos, Forma }

    public enum TipoTerminacionCambioMuro { SinPAta, conpata }

    public enum DireccionVisualizacion { Ambos, Uno, Dos }
    public enum OrientacionVisualizacion { Ambos, H, V }
    public enum TipoDireccionBarra { Primaria, Secundario, NONE }


    public enum MOverSection { Avanzar, Retroceder }

    public enum TipoLineaMallaH { inicial, central, final }
    public enum TipoIntervaloMalla { inicial, central, final, incialFinal }
    public enum TipoPataBarra
    {
        buscar, BarraVPataInicial, BarraVPataFinal, BarraVSinPatas, BarraVPataAmbos, NoBuscar, BarraVPataAUTO,  BuscarSinExtender,
        BarraVHorquilla,
        BarraVPataAmbos_Horquilla
    }

    public enum TipoBarraRefuerzo { buscar, BarraRefPataInicial, BarraRefPataFinal, BarraRefSinPatas, BarraRefPataAmbos, NoBuscar, BarraRefPataAUTO, EstriboRef }
    public enum PosicionDeBusqueda { Inicio, Fin }

    public enum TipoPaTaMalla { bordeMuro, intersecccionMuro, horquilla, SinBordeMuro }

    public enum SentidoMirror
    {
        sentidoParaleloBarra, sentidoPerpendicularBarra,
        sentidoDiagonalBarra
    }
    public enum TipoCOloresTexto
    {
        magenta,
        azul,
        verde,
        rojo,
        ParaMalla,
        Blanco,
        Amarillo
    }
    public enum TipoCOlores
    {
        magenta, Fin,
        azul,
        rojopuro,
        rojo,
        ParaMalla,
        amarillo,
        blanco,
        EstriboConf,
        EstriboMuro,
        EstriboViga,
        MallaMuro,
        negro
    }



    public enum TipoEstado
    {
        SinCalcular,
        Calculada
    }

    public enum DireccionPata
    {
        IzqInf, Ambos, DereSup,
        SoloCentral
    }

    public enum DireccionTraslapoH
    {
        derecha, central, izquierda
    }

    public enum TipoRefuerzoLosa { Refuerzo, Estribo }
    public enum TipoEstriboRefuerzoLosa { E, ED, ET, CT }

    public enum DireccionSeleccionMouse { IzqToDere, DereToIzq }

    public enum TipoEstriboGenera { EConfinamiento, EMuro, Eviga, NONE }
    public enum TipoConfiguracionEstribo
    {
        Estribo, Estribo_Lateral, Estribo_Traba, Estribo_Lateral_Traba, Traba,
        EstriboMuro, EstriboMuro_Lateral, EstriboMuro_Traba, EstriboMuro_Lateral_Traba, EstriboMuroTraba, EstriboMuroSOLOTraba, EstriboMuroSOLOLateral, EstriboMuroSOLOLateralYTraba,
        EstriboViga, EstriboViga_Lateral, EstriboViga_Traba, EstriboViga_Lateral_Traba, VigaTraba, EstriboVigaSOLOTraba, EstriboVigaSOLOLateral, EstriboVigaSOLOLAteralYTraba,
        NONE,
        ElevMallaV, ElevMallaH,
        ELEV_BA_H, ELEV_BA_V
    }


    public enum TipoPataMAlla { Auto, Izquierda, Derecha, Ambos, Sin }

    public enum TipoBarraSoloTag
    {
        Izquierda, Derecha, Ambos, None,
        Centro
    } //extender borde la roomSeparate

    public enum TipoDireccion { Izquierda, Derecha, Ambos, None } //uso generico

    public enum TipoExtSeparateRoom { Izquierda, Derecha, Ambos, None } //extender borde la roomSeparate
    public enum DireccionEdicionPathRein { Derecha, Izquierda, Superior, Inferior, NONE }
    public enum DireccionTraba { Derecha, Izquierda, NONE }
    public enum TipoTraba { Longitudinal, Transversal, NONE }
    public enum TipoTraba_posicion { A, B }
    public enum TipoSeleccionMouse { nivel, mouse }
    public enum TipoPataFund
    {
        IzqInf, DereSup, Ambos,
        Sin,
        Auto
    }
    public enum TipoMAllaMuro { SM, DM, TM, CM }
    public enum CualMAllaDibujar { Ambos, Vertical, Horizontal };

    public enum TipoRefuerzoLOSA { losa, fundacion, none }

    public enum TipoSeleccionPtosBordeLosa { Borde, Selec2Puntos }

    public enum TipoElementoBArraV
    {
        losa, muro, muroPerpeView, viga, fundacion, shaft, none,
        columna,
        GenericModel//  GenericModel: solo se utiliza para obtener pasadas en el pragrma generar pasadas en condicacion con bim
    }

    public enum TipoRefuerzo { AreaRefuerzo, PathRefuerza }
    public enum OpcionVisibilidad { Ocultar, Desocultar }

    public enum TipoOrientacionBarra { Horizontal, Vertical }

    public enum TipoOrientacionBarraSupleMuro { Horizontal, Vertical }
    public enum SentidoSupleMuro { Normal, Invertido }
    public enum PuntoEnLosa { ptoNull, losaNull, PtoFueraLosa, PtoDentroLosa, ptCero }
    public enum TipoBarraGeneral
    {
        Elevacion, Losa, LosaEscalera, NONE,
        LosaInclinida,
        Fundaciones,
        Escalera
    }


    public enum ElementoSeleccionado
    {
        Muro, Viga, Losa, None, Barra,
        Columna
    }

    //ver observacion 1)
    public enum ElementoContiguo { Muro, Viga, Losa, None, Shaft, Opening, RefuerzoViga, RoomContacto, CabezaMuro }
    public enum TipoConfiguracionBarra
    {
        refuerzoInferior, refuerzoInferior_autoInterseccion, suple, refuerzoSuperior,
        NONE
    }


    public enum TipoEstriboConfig { E, EL, ET, ELT, L, LT, T,
        EL_TF,
        E_TF
    }
    public enum TipoEstribo { E, ED, ET, EC }
    public enum TipoEmpotramiento { sin, mitad, total }
    public enum TipoBorde { shaft, losaLibre, estribo }
    public enum TipoElemento { losa, muro, viga }
    public enum TipoParametro { string_, double_, int_ }

    public enum TipoElementoElevancion { barra, malla, estriboMuro, estriboViga, estriboMuroLateral, estriboVigaLateral, confinamiento }


    //diseño

    public enum TipoDiseño_F1 { f1, f1_sup, f1_conAhorro, f1_sup_conAhorro }
    public enum TipoValidarEspesor { VerificarEspesorMenor15, NOVerificarEspesorMenor15 }
}
