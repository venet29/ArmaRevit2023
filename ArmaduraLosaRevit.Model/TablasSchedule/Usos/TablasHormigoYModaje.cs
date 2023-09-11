using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Usos
{
   public class TablasHormigoYModaje
    {
        private ScheduleLeer _scheduleNH_MUROS;
        private ScheduleLeer _scheduleNH_LOSAS;
        private readonly ScheduleLeer _scheduleNH_ListaPlanos;
        private ScheduleLeer _scheduleNH_VIGA;
        private ScheduleLeer _scheduleNH_FUND;
        public List<object[]> listaPtosObjTodosMOldajes { get; set; }
        public List<object[]> listaPtosObjTodoshORMIGON { get; set; }

        public List<object[]> listadePLanos { get; set; }
        public TablasHormigoYModaje(ScheduleLeer scheduleNH_MUROS, ScheduleLeer scheduleNH_LOSAS, ScheduleLeer scheduleNH_ListaPlanos, ScheduleLeer scheduleNH_VIGA, ScheduleLeer scheduleNH_FUND)
        {
            _scheduleNH_MUROS = scheduleNH_MUROS;
            _scheduleNH_LOSAS = scheduleNH_LOSAS;
            this._scheduleNH_ListaPlanos = scheduleNH_ListaPlanos;
            _scheduleNH_VIGA = scheduleNH_VIGA;
            _scheduleNH_FUND = scheduleNH_FUND;
            listaPtosObjTodosMOldajes = new List<object[]>();
            listaPtosObjTodoshORMIGON = new List<object[]>();
            listadePLanos = new List<object[]>();
        }

        public void obtenerTablas()
        {
            try
            {
                listaPtosObjTodosMOldajes.AddRange(_scheduleNH_FUND.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { -1000000, "FUND", "PF", 0, "FUND" }).ToList());
                listaPtosObjTodosMOldajes.AddRange(_scheduleNH_LOSAS.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "LO", c.nivel, c.area, "LO" }).ToList());
                listaPtosObjTodosMOldajes.AddRange(_scheduleNH_MUROS.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "MU", c.nivel, c.area, "M" }).ToList());
                listaPtosObjTodosMOldajes.AddRange(_scheduleNH_VIGA.listaPtos.Where(c=> (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "VN", c.nivel, c.area, "V" }).ToList());
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al obtener Datos de MOdalje  ex:{ex.Message}");
            }

            try
            {
                listaPtosObjTodoshORMIGON.AddRange(_scheduleNH_FUND.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { -1000000, "FUND", "PF", c.vol, "FUND" }).ToList());
                listaPtosObjTodoshORMIGON.AddRange(_scheduleNH_LOSAS.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "LO", c.nivel, c.vol, "LO" }).ToList());
                listaPtosObjTodoshORMIGON.AddRange(_scheduleNH_MUROS.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "MU", c.nivel, c.vol, "M" }).ToList());
                listaPtosObjTodoshORMIGON.AddRange(_scheduleNH_VIGA.listaPtos.Where(c => (!c.nivel.Contains("TOTAL"))).Select(c => new object[] { c.OrdeElevacion, "VN", c.nivel, c.vol, "V" }).ToList());
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al obtener Datos de MOdalje  ex:{ex.Message}");
            }


            try
            {
           
                if(_scheduleNH_ListaPlanos!=null) listadePLanos.AddRange(_scheduleNH_ListaPlanos.listaPtosObj);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al obtener Datos de MOdalje  ex:{ex.Message}");
            }

        }
    }
}
