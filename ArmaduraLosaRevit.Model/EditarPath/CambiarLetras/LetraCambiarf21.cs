using ArmaduraLosaRevit.Model.EditarPath.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CambiarLetras
{
    public class LetraCambiarf21 : LetraCambiar_base, ILetraCambiar
    {

        public ParametroLetra letraCambiar;
        public LetraCambiarf21(Document doc, PathReinforcement pathReinforcement, string _TipoBarra, ParametroLetra letraCambiar) : base(doc, pathReinforcement, _TipoBarra)
        {

            Isok = true;
        }
        public LetraCambiarf21(Document doc, PathReinforcement pathReinforcement, string _TipoBarra) : base(doc, pathReinforcement, _TipoBarra)
        {
            Isok = true;
        }
        public void Ejecutar()
        {
            ObtenerListaLetraCambiarDTO();
            CambiarParametros();
        }

        private void ObtenerListaLetraCambiarDTO()
        {
            try
            {
                //ALTERNATIVA 0 -> a_    / OTRO SNGULO  LL_, C2_
                double largoCentral = 0;
                Parameter aux_espaciamiento = pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1);
                Parameter aux_espaciamientoAltternativa = pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_2);

                if (_TipoBarra == "f21a")
                {
                    if (aux_espaciamiento != null)
                    {
                        
                        double largoAltternativa = aux_espaciamientoAltternativa.AsDouble();
                        ListaLetraCambiarDTO.Add(
                            new LetraCambiarDTO()
                            {
                                letraCambiar = ParametroLetra.C_,
                                valor = largoAltternativa
                            });

                        largoCentral = aux_espaciamiento.AsDouble();
                        ListaLetraCambiarDTO.Add(
                         new LetraCambiarDTO()
                         {
                             letraCambiar = ParametroLetra.C2_,
                             valor = largoCentral
                         });
                        ListaLetraCambiarDTO.Add(


                        new LetraCambiarDTO()
                        {
                            letraCambiar = ParametroLetra.L2barra,
                            valor = largoCentral
                        });
                    }

                }
                else
                {

                    if (aux_espaciamiento != null)
                    {
                        largoCentral = aux_espaciamiento.AsDouble();
                        ListaLetraCambiarDTO.Add(
                            new LetraCambiarDTO()
                            {
                                letraCambiar = ParametroLetra.C_,
                                valor = largoCentral
                            });
                        double largoAltternativa = aux_espaciamientoAltternativa.AsDouble();
                        ListaLetraCambiarDTO.Add(
                         new LetraCambiarDTO()
                         {
                             letraCambiar = ParametroLetra.C2_,
                             valor = largoAltternativa
                         });
                        ListaLetraCambiarDTO.Add(


                        new LetraCambiarDTO()
                        {
                            letraCambiar = ParametroLetra.L2barra,
                            valor = largoAltternativa
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar ParametroLetra   ex:{ex.Message}");

            }
        }
    }
}
