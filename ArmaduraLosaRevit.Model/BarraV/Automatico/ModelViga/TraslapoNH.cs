using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class TraslapoNH
    {
        private Tabla03_Info_Traslapos_Vigas item;
        public string ID_Name_REVIT { get; private set; }
        public string Fila_Barra { get; set; }
        public List<CasosTraslapoDTO> ListaCasosTraslapos { get; set; }
        public TraslapoNH(Tabla03_Info_Traslapos_Vigas item)
        {
            this.item = item;
            this.ID_Name_REVIT = item.ID_Name_REVIT;
            this.Fila_Barra = item.Fila_Barra;
        }
        public bool ObtenerDatos()
        {
            try
            {
                ListaCasosTraslapos = item.OBtenerTIpov2();
                if (ListaCasosTraslapos.Count==0) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
