using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo
{

    public enum TipoInterseccion
    {
        IntersectaConPAsada,
        SinAnalizar, //INICIAL
        Intersectado, 
        SinInterseccion,
        LibreDeOcuparEspacio
    }
    public enum EstadoIteracion
    {
      PermitidoIterar,
      NoIterrar // para cuando se ocupa un lado de la pasada el otro lado queda libre para poder dibujar dimensones de otra pasada
    }
    public class DimensionParaUbicDimensiones
    {
        public double AnchoDimension;

        public bool IsOk { get; private set; }

        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly EnvoltoriosPlanos planoPasada;
        public bool IsDIbujarRectaguloPorInterseccion { get; set; }
        public TipoInterseccion IsIntersectado { get; set; }


        //public EstadoIteracion EstadoIteracion_ { get; set; }        
        private EstadoIteracion estadoIteracion_;

        public EstadoIteracion EstadoIteracion_
        {
            get { return estadoIteracion_; }
            set {

                if (Id_Pasada.IntegerValue == 3335329 && planoPasada.TipoCara_==TipoCara.Izquierdo)
                { }
                estadoIteracion_ = value;
            }
        }


        public List<XYZ> ListaPuntos { get; set; }
        public double MaximoLargo { get; internal set; }
        public double Largo { get; private set; }
        public double Ancho { get; private set; }
        public ElementId Id_Pasada { get; }

        // public List<XYZ> ListaPuntos_DereSup { get; set; }
        public DimensionParaUbicDimensiones(UIApplication _uiapp, EnvoltoriosPlanos _planoPasada, ElementId id_Pasada)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            planoPasada = _planoPasada;
            Id_Pasada = id_Pasada;
            AnchoDimension = ConstBim.CONST_ANCHO_DIMENSIONES;
            IsOk = false;
            IsIntersectado = TipoInterseccion.SinAnalizar;
            EstadoIteracion_ = EstadoIteracion.PermitidoIterar;
            ListaPuntos = new List<XYZ>();
            IsDIbujarRectaguloPorInterseccion = true;
          //  ListaPuntos_DereSup = new List<XYZ>();
        }

        public bool CalcularPtos_Izq()
        {
            try
            {
                //caso inf izq
                // pa       pd
                // pb       pc

                var pA = planoPasada.p1 + -planoPasada.Direccion_ * planoPasada.refrenciaSeleccionado_Izq_Inf.distanciaInterseccion_foot +
                                            planoPasada.Normal * AnchoDimension;

                var pB = planoPasada.p1 + -planoPasada.Direccion_ * planoPasada.refrenciaSeleccionado_Izq_Inf.distanciaInterseccion_foot;
                var pC = planoPasada.p2;
                var pD = planoPasada.p2 + planoPasada.Normal * AnchoDimension;

                Largo = pB.DistanceTo(pC);
                Ancho = pA.DistanceTo(pC);

                ListaPuntos.Add(pA); ListaPuntos.Add(pB); ListaPuntos.Add(pC); ListaPuntos.Add(pD);


                //caso DERE SUP
                // pa       pd
                // pb       pc


                IsOk = true;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CalcularPtos_Izq'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool CalcularPtos_Dere()
        {
            try
            {
                //caso inf izq
                // pa       pd
                // pb       pc


                //caso DERE SUP
                // pa       pd
                // pb       pc

                var pA_DS = planoPasada.p1 + planoPasada.Normal * AnchoDimension;
                var pB_DS = planoPasada.p1;
                var pC_DS = planoPasada.p2 + planoPasada.Direccion_ * planoPasada.refrenciaSeleccionado_Arriba_Dere.distanciaInterseccion_foot;
                var pD_DS = planoPasada.p2 + planoPasada.Direccion_ * planoPasada.refrenciaSeleccionado_Arriba_Dere.distanciaInterseccion_foot +
                                            planoPasada.Normal * AnchoDimension;

                Largo = pB_DS.DistanceTo(pC_DS);
                Ancho = pA_DS.DistanceTo(pC_DS);

                MaximoLargo = Math.Max(pA_DS.DistanceTo(pC_DS), pB_DS.DistanceTo(pD_DS));
                ListaPuntos.Add(pA_DS); ListaPuntos.Add(pB_DS); ListaPuntos.Add(pC_DS); ListaPuntos.Add(pD_DS);

                IsOk = true;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CalcularPtos_Dere'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool DibujarLIneas_sinTransa(string nobreLinea)
        {
            try
            {
                if (IsOk)
                {
                    var lsita1= CrearDetailLineAyuda.modelarlineas_Sintransacion(_uiapp.ActiveUIDocument.Document, _uiapp.ActiveUIDocument.Document.ActiveView, ListaPuntos, nobreLinea);
                    if(lsita1.Count > 0)
                        _doc.Create.NewGroup(lsita1.Select(c=> c.Id).ToList());

                    //var lsita2 =CrearDetailLineAyuda.modelarlineas_Sintransacion(_uiapp.ActiveUIDocument.Document, _uiapp.ActiveUIDocument.Document.ActiveView, ListaPuntos_DereSup, nobreLinea);

                    //if (lsita2.Count > 0)
                    //    _doc.Create.NewGroup(lsita2.Select(c => c.Id).ToList());
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DibujarLIneas_sinTransa'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
