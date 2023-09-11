using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo
{
    public class InterrvalosPtosPathRebarAuto : InterrvalosPtosPathRebar
    {
        private readonly List<IntervalosMallaDTOAuto> _ListaIntervalosMallaDTOAuto;

        public InterrvalosPtosPathRebarAuto(UIApplication uIApplication, List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto, List<Level> listaLevel, View3D view3D_buscar):base(uIApplication,listaLevel,view3D_buscar)
        {
            this._ListaIntervalosMallaDTOAuto = ListIntervalosMallaDTOAuto;
            this._OrigenView = uIApplication.ActiveUIDocument.ActiveView.Origin;
            this._dIreccionMuroEnIgualSentidoRightDirection = uIApplication.ActiveUIDocument.ActiveView.RightDirection;

        }



        public bool Ejecutar_auto()
        {

            foreach (IntervalosMallaDTOAuto item in _ListaIntervalosMallaDTOAuto)
            {
                _listaPtos = item.ListaPtos;
                _datosMallasDTO = item._datosMallasDTO;
                Ejecutar();
            }
    
            return true;
        }

        internal void EjecutarRebar_auto()
        {
            throw new NotImplementedException();
        }
    }
}
