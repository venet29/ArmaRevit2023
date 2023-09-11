using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
   public class ConfiguracionIniciaWPFlBarraVerticalDTO: ICloneable
    {
        #region Analisis lineas de barras
        //para las linea de barra impar : 1,3,5
        public int incial_ComoIniciarTraslapo_LineaPAr { get; set; }
        //para las linea de barra impar : 2,4,6
        public int incial_ComoIniciarTraslapo_LineaImpar { get; set; }
        //guarda la posicion de la bbara que se esta dibujando   
        //si es par parte con'incial_ComoIniciarTraslapo_LineaImpar'
        //si es inpar parte 'incial_ComoIniciarTraslapo'
        public int LineaBarraAnalizada { get; set; } 
        #endregion


        public int inicial_ComoTraslapo { get; set; }
        public TipoRebar BarraTipo { get;  set; }
        public bool inicial_ISIntercalar { get; set; }
        public bool inicial_IsDirectriz { get; set; }
        public int inicial_diametroMM { get; set; }

        // espaciamiento  en el sentido del recorrido dde la barra. cabela muro: perpendicular view, malla: paralelo a view
        public string EspaciamietoRecorridoBarraFoot { get; set; }
        public string Inicial_Cantidadbarra { get; set; }
        //si ls barras son de cabeza muro-> espaciamiento en direccion de paralela ala vista
        //si la barra en de malla -> espaciamiento es entrando perpendicular a la vista
        public string  Inicial_espacienmietoCm_EntreLineasBarras { get; set; }

        public double[] IntervalosEspaciamiento { get; set; }
        public int[] IntervalosCantidadBArras { get; set; }
        public View3D view3D_paraBuscar { get; internal set; }
        public View3D view3D_paraVisualizar { get;  set; }
        public View viewActual { get;  set; }
        public Document Document_ { get; set; }
        public bool IsDibujarTag { get; set; }
        public bool IsInvertirPosicionTag { get; set; }
        public List<IntervalosBarraAutoDto> ListaIntervaloBarraAutoDto { get; set; }
        public TipoPataBarra inicial_tipoBarraV { get; set; }
      
        public double EspaciamientoREspectoBordeFoot { get; internal set; }
        public int NuevaLineaCantidadbarra { get;  set; }
        public int NumeroBarraLinea { get;  set; }

        public string  CuantiaVertical { get; set; }
        public string CuantiaHorizontal { get; set; }
        public TipoSeleccionMouse TipoSeleccionMousePtoSuperior { get; set; }
        public TipoSeleccionMouse TipoSeleccionMousePtoInferior { get; set; }
        public TipoBarraVertical TipoBarraRebar_ { get;  set; }
        public string CuantiaMalla { get;  set; }
        public TipoPataBarra tipobarraH { get;  set; }



        public CasoAnalisasBarrasElevacion CasoAnalisasBarrasElevacion_ { get;  set; }
        public TipoSeleccion TipoSelecion { get;  set; }
        public TipoBarraVertical TipoBarraRebarHorizontal_ { get; internal set; }


        public IntervaloBarras_HorqDTO IntervaloBarras_HorqDTO_ { get; set; }
        public DatosMallasAutoDTO DatosMallasDTO { get; internal set; }

        public ConfiguracionIniciaWPFlBarraVerticalDTO()
        {
            IsDibujarTag = true;
            
        }


        public void M1_ObtenerIntervalosDireccionMuro(string Inicial_Cantidadbarra,string  Inicial_espacienmietoCm_EntreLineasBarras)
        {
            this.Inicial_Cantidadbarra = Inicial_Cantidadbarra;
            this.Inicial_espacienmietoCm_EntreLineasBarras = Inicial_espacienmietoCm_EntreLineasBarras;
            M1_ObtenerIntervalosDireccionMuro();

        }
            public void M1_ObtenerIntervalosDireccionMuro()
        {
            var resulCantidad = Inicial_Cantidadbarra.Split('+');
            var resultEspaciamiento = Inicial_espacienmietoCm_EntreLineasBarras.Split('+');

            IntervalosCantidadBArras = new int[resulCantidad.Length];
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
                IntervalosCantidadBArras[i] = Util.ConvertirStringInInteger(resulCantidad[i]);
                IntervalosEspaciamiento[i] = Util.ConvertirStringInInteger(resultEspaciamiento[0]);
            }
        }

        //cuandu usario asigna 
        // cantidad =2+3+3+3
        //espaciemineto = 15+20+15+20
        private void M1_1_AsignarEspaciamientoDireccionMuroDefinidoPOrUsuario(string[] resulCantidad, string[] resultEspaciamiento)
        {
            for (int i = 0; i < resulCantidad.Length; i++)
            {
                IntervalosCantidadBArras[i] = Util.ConvertirStringInInteger(resulCantidad[i]);
                IntervalosEspaciamiento[i] = Util.ConvertirStringInInteger(resultEspaciamiento[i]);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
