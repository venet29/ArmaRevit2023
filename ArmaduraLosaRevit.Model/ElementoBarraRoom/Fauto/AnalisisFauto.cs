using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Fauto
{
    public class AnalisisFauto
    {
        public List<WrapperBoundarySegment> listBorde;
        private XYZ p1;
        private XYZ p2;
        private XYZ p3;
        private XYZ p4;
        private FautoDTO _confiFauto;

        public TipoOrientacionBarra TipoOrientacion;

        public string tipoBarraPrincipal { get; set; }
        public string tipoBarra_izq_infer { get; set; }
        public string tipoBarra_dere_sup { get; set; }
        public UbicacionLosa ubicacionENlosa { get; set; }

        public AnalisisFauto(List<WrapperBoundarySegment> listBorde, XYZ p1, XYZ p2, XYZ p3, XYZ p4, TipoOrientacionBarra TipoOrientacion)
        {
            this.listBorde = listBorde;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.TipoOrientacion = TipoOrientacion;


            //inseccion de configuracion 
            if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1)
                _confiFauto = FactoryFautoDTO.ObtenerConfig_F1();
            else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_conAhorro)
                _confiFauto = FactoryFautoDTO.ObtenerConfig_F1_conAhorro();
            else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup)
                _confiFauto = FactoryFautoDTO.ObtenerConfig_F1_Sup();
            else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                _confiFauto = FactoryFautoDTO.ObtenerConfig_F1_Sup_conAhorro();
        }



        public string BuscarConfiParaCasoFautoV2()
        {
            //nota :
            //listBorde[0];//  izq  - infe
            //listBorde[1]; // dere - sup/
            tipoBarraPrincipal = "";
            tipoBarra_izq_infer = "";
            tipoBarra_dere_sup = "";

            if (ISCasoVecinosAmbosLados())
            {
                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                    ubicacionENlosa = UbicacionLosa.Izquierda;
                else
                    ubicacionENlosa = UbicacionLosa.Inferior;
                //sin patas
                return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados;
            }
            else if (IsCasoSINVecinos())
            {
                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                    ubicacionENlosa = UbicacionLosa.Izquierda;
                else
                    ubicacionENlosa = UbicacionLosa.Inferior;

                tipoBarra_izq_infer = _confiFauto.tipoBarra_izq_infer;
                tipoBarra_dere_sup = _confiFauto.tipoBarra_dere_sup;

                //si los dos muros tienes espesor menr 15  --> pata ambos lado
                if (UtilBarras.ValidarEspesor(listBorde[0].obtenerRefereciasCercanas.espesorElemContiguo) && UtilBarras.ValidarEspesor(listBorde[1].obtenerRefereciasCercanas.espesorElemContiguo))
                {
                    if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup)
                    { return tipoBarraPrincipal = "f11"; }
                    else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                    { return tipoBarraPrincipal = "f18"; }
                    else
                    { return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados; }
                }
                //si al menos uno de los dos muros tienes espesor menr 15  --> pata un lado
                else if (UtilBarras.ValidarEspesor(listBorde[0].obtenerRefereciasCercanas.espesorElemContiguo) || UtilBarras.ValidarEspesor(listBorde[1].obtenerRefereciasCercanas.espesorElemContiguo))
                {
                    if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup)
                    { return tipoBarraPrincipal = "f11a"; }
                    else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                    { return tipoBarraPrincipal = "f17"; }
                    else
                    { return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados; }
                }
                else
                { return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados; }  // si ambos espesores son mayores a 15cm, sin pata


            }
            else if (IsCasoVecinoLadoIzq_Inf()) // uno u otro
            {
                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                    ubicacionENlosa = UbicacionLosa.Izquierda;
                else
                    ubicacionENlosa = UbicacionLosa.Inferior;

                tipoBarra_izq_infer = _confiFauto.tipoBarra_izq_infer;

                //si  muros tienes espesor menr 15  --> 1 pata un lado
                if (UtilBarras.ValidarEspesor(listBorde[0].obtenerRefereciasCercanas.espesorElemContiguo))
                {
                    if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup)
                    { return tipoBarraPrincipal = "f11a"; }
                    else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                    { return tipoBarraPrincipal = "f17"; }
                    else
                    { return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados; }
                }
                else // sin pata
                    return tipoBarraPrincipal = _confiFauto.tipoBarra;


            }
            else if (IsCasoVecinosLadoDere_Sup()) // uno u otro
            {//f1 0 f13

                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                    ubicacionENlosa = UbicacionLosa.Derecha;
                else
                    ubicacionENlosa = UbicacionLosa.Superior;

                tipoBarra_dere_sup = _confiFauto.tipoBarra_dere_sup;

                //si  muros tienes espesor menr 15  --> 1 pata un lado
                if (UtilBarras.ValidarEspesor(listBorde[1].obtenerRefereciasCercanas.espesorElemContiguo))
                {
                    if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup)
                    { return tipoBarraPrincipal = "f11a"; }
                    else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                    { return tipoBarraPrincipal = "f17"; }
                    else
                    { return tipoBarraPrincipal = _confiFauto.tipoBarraPataAmbosLados; }
                }
                else // sin pata
                    return tipoBarraPrincipal = _confiFauto.tipoBarra;


            }

            else if (listBorde[0].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None ||
                     listBorde[1].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None) // uno u otro
            {//f1 0 f13

                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Izquierda;
                    else
                        ubicacionENlosa = UbicacionLosa.Derecha;
                }
                else
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Inferior;
                    else
                        ubicacionENlosa = UbicacionLosa.Superior;
                }
                tipoBarra_izq_infer = "f1_SUP";
                tipoBarra_dere_sup = "f1_SUP";
                return tipoBarraPrincipal = "f11";

            }
            else if (listBorde[0].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None) // uno u otro
            {//f1 0 f13

                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Izquierda;
                    else
                        ubicacionENlosa = UbicacionLosa.Derecha;
                }
                else
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Inferior;
                    else
                        ubicacionENlosa = UbicacionLosa.Superior;
                }
                tipoBarra_izq_infer = "f1_SUP";
                return tipoBarraPrincipal = "f11";

            }
            else if (listBorde[1].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None) // uno u otro
            {//f1 0 f3

                if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Derecha;
                    else
                        ubicacionENlosa = UbicacionLosa.Izquierda;
                }
                else
                {
                    if (p2.X < p3.X)
                        ubicacionENlosa = UbicacionLosa.Superior;
                    else
                        ubicacionENlosa = UbicacionLosa.Inferior;
                }
                tipoBarra_dere_sup = "f1_SUP";
                return tipoBarraPrincipal = "f11";

            }

            return tipoBarraPrincipal;
        }

        private bool ISCasoVecinosAmbosLados() => listBorde[0].obtenerRefereciasCercanas.roomNeighbour != null && listBorde[1].obtenerRefereciasCercanas.roomNeighbour != null;
        private bool IsCasoSINVecinos() => listBorde[0].obtenerRefereciasCercanas.roomNeighbour == null && listBorde[1].obtenerRefereciasCercanas.roomNeighbour == null;
        private bool IsCasoVecinoLadoIzq_Inf() => listBorde[0].obtenerRefereciasCercanas.roomNeighbour == null;
        private bool IsCasoVecinosLadoDere_Sup() => listBorde[1].obtenerRefereciasCercanas.roomNeighbour == null;
        //private bool ISCasoVecinosAmbosLados() => listBorde[0].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None && listBorde[1].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None;
        //private bool IsCasoSINVecinos() => listBorde[0].obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.None && listBorde[1].obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.None;
        //private bool IsCasoVecinoLadoIzq_Inf() => listBorde[0].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None;
        //private bool IsCasoVecinosLadoDere_Sup() => listBorde[1].obtenerRefereciasCercanas.elementoContiguo != ElementoContiguo.None;




    }
}
