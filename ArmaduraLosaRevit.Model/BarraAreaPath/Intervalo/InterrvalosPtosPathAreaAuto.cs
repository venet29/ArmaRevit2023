using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo
{
    public class InterrvalosPtosPathAreaAuto: InterrvalosPtosPathArea
    {
        //private readonly List<IntervalosMallaDTOAuto> _ListaIntervalosMallaDTOAuto;
        public List<IntervalosMallaDTOAuto> _ListaIntervalosMallaDTOAuto { get; set; }


        public InterrvalosPtosPathAreaAuto(UIApplication uIApplication, List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto, List<Level> listaLevel, View3D view3D_buscar):base(uIApplication,listaLevel,view3D_buscar)
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
                ListaPtos_mallaVertical = item.ListaPtos_mallaVertical;
                _datosMallasDTO = item._datosMallasDTO;
                Ejecutar();
            }
    
            return true;
        }


    }
}
