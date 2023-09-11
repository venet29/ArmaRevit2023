using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
   public class ConfiguracionInicialBarraHorizontalDTO
    {
        #region Analisis lineas de barras
        //para las linea de barra impar : 1,3,5
        public int incial_ComoIniciarTraslapo_LineaPAr { get; set; }
        //para las linea de barra impar : 2,4,6
        public int incial_ComoIniciarTraslapo_LineaImpar { get; set; }
        public TipoBarraRefuerzoViga TipoBarraRefuerzoViga { get; set; } = TipoBarraRefuerzoViga.NONE;

        //guarda la posicion de la bbara que se esta dibujando   
        //si es par parte con'incial_ComoIniciarTraslapo_LineaImpar'
        //si es inpar parte 'incial_ComoIniciarTraslapo'
        public int LineaBarraAnalizada { get; set; } 
        #endregion


        public int incial_ComoTraslapo { get; set; }
        public bool IsDibujarBArra { get; set; }
        public bool incial_ISIntercalar { get; set; }
        public bool incial_IsDirectriz { get; set; }
        public int incial_diametroMM { get; set; }
        // espaciamiento  en el sentido del recorrido dde la barra. cabela muro: perpendicular view, malla: paralelo a view
        public string EspaciamietoRecorridoBarraFoot { get; set; }
        public string Inicial_Cantidadbarra { get; set; }
        public string  Inicial_espacienmietoCm_direccionmuro { get; set; }
        public double[] IntervalosEspaciamiento { get; set; }
        public double[] IntervalosCantidadBArras { get; set; }
        public View3D view3D_paraBuscar { get;  set; }
        public View3D view3D_paraVisualizar { get; set; }
        public View viewActual { get; set; }
        public Document _Doc { get; set; }

        public TipoPataBarra inicial_tipoBarraH { get; set; }
      
        public double EspaciamientoREspectoBordeFoot { get; set; }
        public double NuevaLineaCantidadbarra { get; set; }
        public int _NUmeroLinea_paraTagRefuerzo { get;  set; }
        public double NumeroBarraLinea { get;  set; }
        public  EmpotramientoPatasDTO _empotramientoPatasDTO { get; set; }
        public TipoRebar BarraTipo { get;  set; }
        public DireccionTraslapoH DireccionTraslapoH_ { get;  set; }
        public TipoSeleccion TipoSelecion { get;  set; }
        public TipoPataBarra TipoPataIzqInf { get;  set; }
        public TipoPataBarra TipoPataDereSup { get;  set; }

        public ConfiguracionInicialBarraHorizontalDTO()
        {
        }

        public void M1_ObtenerIntervalosDireccionMuro()
        {
            var resulCantidad = Inicial_Cantidadbarra.Split('+');
            var resultEspaciamiento = Inicial_espacienmietoCm_direccionmuro.Split('+');

            IntervalosCantidadBArras = new double[resulCantidad.Length];
            IntervalosEspaciamiento = new double[resulCantidad.Length];

            // as
            if (resulCantidad.Length == resultEspaciamiento.Length)
            {
                M1_1_AsignarEspaciamientoDireccionMuroDefinidoPOrUsuario(resulCantidad, resultEspaciamiento);
            }
            else
            {
                M1_2_AsignarEspaciamientoFijo(resulCantidad, resultEspaciamiento);
            }
        }

        //cuandu usario asigna 
        // cantidad =2+3+3+3
        //espaciemineto = 20
        private void M1_2_AsignarEspaciamientoFijo(string[] resulCantidad, string[] resultEspaciamiento)
        {
            for (int i = 0; i < resulCantidad.Length; i++)
            {
                IntervalosCantidadBArras[i] = Util.ConvertirStringInDouble(resulCantidad[i]);
                IntervalosEspaciamiento[i] = Util.ConvertirStringInDouble(resultEspaciamiento[0]);
            }
        }

        //cuandu usario asigna 
        // cantidad =2+3+3+3
        //espaciemineto = 15+20+15+20
        private void M1_1_AsignarEspaciamientoDireccionMuroDefinidoPOrUsuario(string[] resulCantidad, string[] resultEspaciamiento)
        {
            for (int i = 0; i < resulCantidad.Length; i++)
            {
                IntervalosCantidadBArras[i] = Util.ConvertirStringInDouble(resulCantidad[i]);
                IntervalosEspaciamiento[i] = Util.ConvertirStringInDouble(resultEspaciamiento[i]);
            }
        }
    }
}
