using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Intervalos
{
    public class GenerarListaIntervalosBordeLosa : GenerarListaIntervalos
    {
        private readonly XYZ _direccionHaciaLosa;
        public List<DatosBordeLosaDTO> ListaDatosIntervalosDTO { get; set; }

        public GenerarListaIntervalosBordeLosa(ParametrosListaIntervalosDTo _parametrosListaIntervalosDTo, XYZ _DireccionHaciaLosa)
            : base(_parametrosListaIntervalosDTo)
        {

            factor = 0;
            this._direccionHaciaLosa = _DireccionHaciaLosa;
            ListaDatosIntervalosDTO = new List<DatosBordeLosaDTO>();
        }

        public override bool M1_ObtenerIntervalos()
        {
            try
            {
                //MOVER POLIGO MEDIO ESPACIAMIENTO
                if (!M1_3_ObtenerPtosDefinirPath()) return false;
                double largoTraslapo = UtilBarras.largo_traslapoFoot_diamMM(diametroMM);
                // List<XYZ> intervalo = new List<XYZ>();
                XYZ pto_p1 = ListaPtosPerimetroBarras[0];
                XYZ pto_p2 = ListaPtosPerimetroBarras[1];

                if (listaPtos.Count == 0)
                {
                    ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                    {
                        ListaIntervalos = ListaPtosPerimetroBarras,

                    });
                    return false;
                }

                int dire = 1;

                List<XYZ> intervalo = new List<XYZ>();
                XYZ direccionP2_p1 = (pto_p2 - pto_p1).Normalize();
                bool isInicial = true;

                XYZ desface = _direccionHaciaLosa * Util.CmToFoot(diametroMM / 20.0f);

                foreach (XYZ pto_ in listaPtos)
                {

                    intervalo = new List<XYZ>();
                    intervalo.Add(pto_p1 + desface * dire);
                    intervalo.Add(pto_ + desface * dire);

                    if (isInicial)
                    {
                        ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                        {
                            ListaIntervalos = intervalo,
                        });
                        isInicial = false;
                    }
                    else
                    {
                        ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                        {
                            ListaIntervalos = intervalo,

                        }); //(intervalo[0]+ intervalo[2])/2 + dire_0_to_1*Util.CmToFoot(1)*dire   _  ObtenerPtoMouse(intervalo, dire)
                    }
                    //recalcular
                    pto_p1 = pto_.GEtNewXYZNH();


                    dire = dire * -1;
                }


                //linea final
                intervalo = new List<XYZ>();
                intervalo.Add(pto_p1 + desface * dire);
                intervalo.Add(pto_p2 + desface * dire);

                ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                {
                    ListaIntervalos = intervalo,

                });
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al  'ObtenerIntervalos' ex:{ex.Message}");
                return false;
            }
            return true;
        }



        public bool M1_ObtenerIntervalosBordeLosa()
        {
            try
            {
                //se ejecuta en GenerarListaIntervalos
                if (!M1_ObtenerIntervalos()) return false;

                foreach (GenerarListaIntervalosDTo item in ListaIntervalosDTO)
                {
                    DatosBordeLosaDTO _DatosBordeLosaDTO = new DatosBordeLosaDTO()
                    {
                        DireccionHaciaLosa = _direccionHaciaLosa,
                        ptoInicial = item.ListaIntervalos[0],
                        ptoFinal = item.ListaIntervalos[1]
                    };
                    ListaDatosIntervalosDTO.Add(_DatosBordeLosaDTO);
                }


            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


    }
}
