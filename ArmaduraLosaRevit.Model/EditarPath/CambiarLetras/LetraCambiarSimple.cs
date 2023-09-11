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
    public class LetraCambiarSimple: LetraCambiar_base, ILetraCambiar
    {

        public ParametroLetra letraCambiar;
        public LetraCambiarSimple(Document doc, PathReinforcement pathReinforcement, string _TipoBarra, ParametroLetra letraCambiar):base(doc,pathReinforcement, _TipoBarra)
        {
            this.letraCambiar = letraCambiar;
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
                double largoCentral = 0;
                Parameter aux_espaciamiento = pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1);
                if (aux_espaciamiento != null)
                {
                    largoCentral = aux_espaciamiento.AsDouble();
                    ListaLetraCambiarDTO.Add(
                        new LetraCambiarDTO()
                        {
                            letraCambiar = letraCambiar,
                            valor = largoCentral
                        });
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar ParametroLetra   ex:{ex.Message}");

            }
        }
    }
}
