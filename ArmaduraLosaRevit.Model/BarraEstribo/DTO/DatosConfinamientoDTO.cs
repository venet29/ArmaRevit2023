using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public class DatosConfinamientoDTO
    {
        //estribo
        public int DiamtroEstriboMM { get; set; }
        public double espaciamientoEstriboCM { get; set; }
        public string cantidadEstribo { get;  set; }
        public TipoConfiguracionEstribo tipoConfiguracionBarra { get;  set; }

        //laterales
        public int DiamtroLateralEstriboMM { get; set; }
        public int cantidadLaterales { get; set; }

        //traba
        public int DiamtroTrabaEstriboMM { get; set; }   
        public int cantidadTraba { get; set; }
        public double espaciamientoTrabaCM { get; set; }


        public bool? IsLateral { get;  set; }
        public bool? IsEstribo { get; set; }
        public bool? IsTraba { get; set; }

        public string ObtenerTextBarra_Borrar()
        {
            return $"+{cantidadTraba}TR.";
            
        }
    }
}
