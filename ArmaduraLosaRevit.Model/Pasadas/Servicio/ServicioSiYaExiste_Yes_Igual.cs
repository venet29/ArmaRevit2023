using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pasadas.Servicio
{
    class AyudaDistanciaEnvoltorioPasadas
    {

        public double distancia { get; set; }
        public EnvoltorioPasadas EnvoltorioPasadas_ { get; set; }
    }
    internal class ServicioSiYaExiste_Yes_Igual
    {
        private UIApplication _uiapp;
        private readonly List<EnvoltorioPasadas> listaEnvoltorioPasadas;

        public FamilyInstance Pasada { get; internal set; }
        public EnvoltorioPasadas EnvoltorioPasadas_ { get; internal set; }
        public ServicioSiYaExiste_Yes_Igual(UIApplication uiapp, List<EnvoltorioPasadas> listaEnvoltorioPasadas)
        {
            this._uiapp = uiapp;
            this.listaEnvoltorioPasadas = listaEnvoltorioPasadas;
        }


        internal bool Buscar(XYZ ptoInterseccion)
        {
            try
            {
                List<AyudaDistanciaEnvoltorioPasadas> listaResul = new List<AyudaDistanciaEnvoltorioPasadas>();
                var result = listaEnvoltorioPasadas.Select(c => new AyudaDistanciaEnvoltorioPasadas()
                {
                    EnvoltorioPasadas_ = c,
                    distancia = ptoInterseccion.DistanceTo(c.PtoInsercion)
                }).OrderBy(c => c.distancia).FirstOrDefault();

                if (result == null) return false;

                if (result.distancia>Util.CmToFoot(1)) return false;

                Pasada = (FamilyInstance)result.EnvoltorioPasadas_.Pasada;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error  ");
                return false;
            }
            return true;
        }
    }
}