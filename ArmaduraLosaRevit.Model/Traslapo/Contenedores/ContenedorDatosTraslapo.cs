using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.Contenedores
{
  

    //clase que define el tipo de barra que se analiza
    //y los tipos de barra que se forman por traslapo
    public class ContenedorDatosTraslapoV2
    {
        public TipoPathReinfDTO _tipoBarra { get; set; }
        public TipoPathReinfDTO _tipoBarraTraslapoIzqBajoResult { get; set; }
        public TipoPathReinfDTO _tipoBarraTraslapoIzqBajo { get; set; }
        public TipoPathReinfDTO _tipoBarraTraslapoIzqBajoInv { get; set; }

        public TipoPathReinfDTO _tipoBarraTraslapoDereArribaResult { get; set; }
        public TipoPathReinfDTO _tipoBarraTraslapoDereArriba { get; set; }
        public TipoPathReinfDTO _tipoBarraTraslapoDereArribaInv { get; set; }
        public ContenedorDatosTraslapoV2(TipoPathReinfDTO _tipoBarra,  //nombra de barra Inical analizada
                                         TipoPathReinfDTO _tipoBarraTraslapoIzqBajo, TipoPathReinfDTO _tipoBarraTraslapoDereArriba, //datos de barra izq-abajo y derecha-superior por trslapo en barra Inicial
                                         TipoPathReinfDTO _tipoBarraTraslapoIzqBajoInv, TipoPathReinfDTO _tipoBarraTraslapoDereArribaInv) //datos de barra izq-abajo y derecha-siperior por traslapo en barra inicial si se invierte sentido
        {
            //
            this._tipoBarra = _tipoBarra;
            this._tipoBarraTraslapoIzqBajo = _tipoBarraTraslapoIzqBajo;
            this._tipoBarraTraslapoDereArriba = _tipoBarraTraslapoDereArriba;
            this._tipoBarraTraslapoIzqBajoInv = _tipoBarraTraslapoIzqBajoInv;
            this._tipoBarraTraslapoDereArribaInv = _tipoBarraTraslapoDereArribaInv;
        }

        //metod que asigna la tipos de orientacion de 
        public ContenedorDatosTraslapoV2 M1_AsignaSentidoCorrespondiente(UbicacionLosa _ubicacionLosa)
        {
            if (UbicacionLosa.Izquierda == _ubicacionLosa)
            {
                return M1_1_SiBarraEsUbicacionIzquierda();
            }

            if (UbicacionLosa.Inferior == _ubicacionLosa)
            {
                return M1_2_SiBarraEsUbicacionInferior();
            }

            if (UbicacionLosa.Derecha == _ubicacionLosa)
            {
                return M1_3_SiBarraEsUbicacionDerecha();
            }

            if (UbicacionLosa.Superior == _ubicacionLosa)
            {
                return M1_4_SiBarraEsUbicacionSuperior();
            }

            return this;
        }



        private ContenedorDatosTraslapoV2 M1_1_SiBarraEsUbicacionIzquierda()
        {
            this._tipoBarraTraslapoIzqBajoResult = _tipoBarraTraslapoIzqBajo;
            this._tipoBarraTraslapoDereArribaResult = _tipoBarraTraslapoDereArriba;
            return this;
        }
        private ContenedorDatosTraslapoV2 M1_2_SiBarraEsUbicacionInferior()
        {
            this._tipoBarraTraslapoIzqBajoResult = _tipoBarraTraslapoIzqBajo;
            this._tipoBarraTraslapoDereArribaResult = _tipoBarraTraslapoDereArriba;
            this._tipoBarraTraslapoIzqBajoResult.Direccion = (this._tipoBarraTraslapoIzqBajoResult.Direccion == UbicacionLosa.Izquierda ?
                                                                                                            UbicacionLosa.Inferior :
                                                                                                            UbicacionLosa.Superior);
            this._tipoBarraTraslapoDereArribaResult.Direccion = (this._tipoBarraTraslapoDereArribaResult.Direccion == UbicacionLosa.Derecha ?
                                                                                                            UbicacionLosa.Superior :
                                                                                                            UbicacionLosa.Inferior);
            return this;
        }
        private ContenedorDatosTraslapoV2 M1_3_SiBarraEsUbicacionDerecha()
        {
            this._tipoBarraTraslapoIzqBajoResult = _tipoBarraTraslapoIzqBajoInv;
            this._tipoBarraTraslapoDereArribaResult = _tipoBarraTraslapoDereArribaInv;
            return this;
        }

        private ContenedorDatosTraslapoV2 M1_4_SiBarraEsUbicacionSuperior()
        {
            this._tipoBarraTraslapoIzqBajoResult = _tipoBarraTraslapoIzqBajoInv;
            this._tipoBarraTraslapoDereArribaResult = _tipoBarraTraslapoDereArribaInv;
            this._tipoBarraTraslapoIzqBajoResult.Direccion = (this._tipoBarraTraslapoIzqBajoResult.Direccion == UbicacionLosa.Derecha ?
                                                                                                            UbicacionLosa.Superior :
                                                                                                            UbicacionLosa.Inferior); ;
            this._tipoBarraTraslapoDereArribaResult.Direccion = (this._tipoBarraTraslapoDereArribaResult.Direccion == UbicacionLosa.Izquierda ?
                                                                                                            UbicacionLosa.Inferior :
                                                                                                            UbicacionLosa.Superior);
            return this;
        }

    }
}
