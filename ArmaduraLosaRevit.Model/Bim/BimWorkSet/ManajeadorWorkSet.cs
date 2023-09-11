using ArmaduraLosaRevit.Model.Bim.BimWorkSet.Factory;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.BimWorkSet
{
    enum TiposEspecialidad
    {
        General,
        Arquitectura,
        Electricidad,
        AguaPotable,
        AguaServidas,
        AguasLLuvias,
        Estructura,
        Coordinacion,
        Entibaciones,
        Pavimento,
        Piscina,
        Mecanico,
        Gas
    }
    class ManajeadorWorkSet
    {
        private readonly UIApplication _uiapp;
        private CreadorWorkset _CreadorWorkset;

        public ManajeadorWorkSet(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
             _CreadorWorkset = new CreadorWorkset(_uiapp);
        }

        public bool CrearLIstaWorkset_GENERALES(List<string> ListaWorkset)
        {
            try
            {
                bool resultado = false;

                resultado = _CreadorWorkset.CreateWorkset_COnTrasn(ListaWorkset);
                if (resultado)
                    Util.InfoMsg($"Workset del grupo {TiposEspecialidad.General} fueron creados correctamente.");
                else
                    Util.ErrorMsg($"Por algun motivo no se pudo crear Workset del grupo {TiposEspecialidad.General}.");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        public bool BorrarLIstaWorkset_GENERALES(TiposEspecialidad nombre_especialidad)
        {
            try
            {
                if (nombre_especialidad == TiposEspecialidad.General)
                {
                    var listaGeneral = FactorEspecialidades.ListaGenerales;
                    _CreadorWorkset.BorrarWorkset_COnTrasn(listaGeneral);
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear especialidad {nombre_especialidad}. \nex:{ex.Message}");
                return false;
            }

            return true;
        }
    }
}
