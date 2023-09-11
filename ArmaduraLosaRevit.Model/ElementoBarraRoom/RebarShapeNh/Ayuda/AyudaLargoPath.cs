using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda
{

    class AyudaLargoPathDTO
    {
        public double _EspesorLosa_1 { get; set; }
        public double LargoAhoraDefinidoUsuario_izq { get; set; }
        public double LargoAhoraDefinidoUsuario_Dere { get; set; }
        public AyudaLargoPathDTO()
        {
            LargoAhoraDefinidoUsuario_izq = 0;
            LargoAhoraDefinidoUsuario_Dere = 0;
        }
    }
    class AyudaLargoPath
    {
        private DatosNuevaBarraDTO _datosNuevaBarraDTO;

        private double _LargoMin_1;
#pragma warning disable CS0169 // The field 'AyudaLargoPath._EspesorLosa_1' is never used
        private double _EspesorLosa_1;
#pragma warning restore CS0169 // The field 'AyudaLargoPath._EspesorLosa_1' is never used

        private bool IsLuzSecuandiria;

        public double largoPataInf { get; private set; }

        private double desplazamientoPorLuzSecundaria;

        public AyudaLargoPathDTO _AyudaLargoPathDTO { get; set; }
        public double LargoAhorroIzq { get;  set; }
        public double LargoAhorroDere { get;  set; }
        public double LargoPaTa { get;  set; }
        public double LargoPathreiforment { get;  set; }
        public double _EspesorLosa_EnFoot { get; set; }

        public AyudaLargoPath(DatosNuevaBarraDTO datosNuevaBarraDTO, AyudaLargoPathDTO _AyudaLargoPathDTO)
        {
            this._datosNuevaBarraDTO = datosNuevaBarraDTO;
            this._AyudaLargoPathDTO = _AyudaLargoPathDTO;


            this._LargoMin_1 = datosNuevaBarraDTO.LargoMininoLosa;
            this.LargoPathreiforment = datosNuevaBarraDTO.LargoPathreiforment;
            this.IsLuzSecuandiria = datosNuevaBarraDTO.IsLuzSecuandiria;
            //valor no definido, conversacion conversacion 04/08/2021
            this.largoPataInf = Util.CmToFoot(ConstNH.CONST_PATA_SX);
        }



        public bool CalcularLargosYDesplazamientos()
        {
            try
            {

                // distanciaBordeLosaHastaEscalera = 0;
                desplazamientoPorLuzSecundaria = (IsLuzSecuandiria == true ? ConstNH.CONST_DESPLAZA_PORLUZSECUNDARIO_CM : 0);
                _EspesorLosa_EnFoot = Util.CmToFoot(_AyudaLargoPathDTO._EspesorLosa_1 - ConstNH.RECUBRIMIENTO_LOSA_SUP_cm - ConstNH.RECUBRIMIENTO_LOSA_INF_cm - desplazamientoPorLuzSecundaria);
                //  double _descuentoEspewsoractual_foot = Util.CmToFoot(_diametroBarraDibujada / 10.0f);


                if (_AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq != 0 || _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere!=0)
                {
                    LargoAhorroIzq = _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq;
                    LargoAhorroDere = _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere;

                    if (RedonderLargoBarras.RedondearFoot1_mascercano(LargoAhorroIzq)) LargoAhorroIzq = RedonderLargoBarras.NuevoLargobarraFoot;
                    if (RedonderLargoBarras.RedondearFoot1_mascercano(LargoAhorroDere)) LargoAhorroDere = RedonderLargoBarras.NuevoLargobarraFoot;

                }
                else
                {
                    LargoAhorroIzq = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;//  + _EspesorMuro_Izq_abajo;
                    LargoAhorroDere = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;// + _EspesorMuro_Dere_Sup;

                    if (RedonderLargoBarras.RedondearFoot1_mascercano(LargoAhorroIzq))
                    {
                        LargoAhorroIzq = RedonderLargoBarras.NuevoLargobarraFoot;
                        LargoAhorroDere = RedonderLargoBarras.NuevoLargobarraFoot;
                    }
                }

                // pata barras
                LargoPaTa = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;
                if (RedonderLargoBarras.Redondear5_LargoPAta_L1(LargoPaTa, _EspesorLosa_EnFoot))
                {
                    LargoPaTa = RedonderLargoBarras.largoPata_L1;
                }

                if (LargoPaTa < ConstNH.CONST_LARGOMIN_PATA)
                    LargoPaTa = ConstNH.CONST_LARGOMIN_PATA;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CalcularLargosYDesplazamientos' \n ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }
    }
}
