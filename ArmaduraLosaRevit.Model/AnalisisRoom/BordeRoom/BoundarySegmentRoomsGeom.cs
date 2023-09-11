using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom
{

    /// <summary>
    /// CONTIENE LOS BoundarySegmentNew, que contiene BoundarySegment (segmentos de room) con varias propiedades.
    /// se utilizan para en enfierrado de refuerzo barras superior
    /// </summary>
    public class BoundarySegmentRoomsGeom
    {
        private  UIApplication _uiapp;
        private Document _doc;
        #region 0)propiedades   n:3

        //  public static List<Element> listaBordeParaEstribo = new List<Element>();

        public Floor floor { get; set; }
        /// <summary>
        /// lista con los segmentos de room, solo se utilza para iterar
        /// </summary>
        public IList<BoundarySegment> ListaBoundarySegmentInicial { get; set; }
        /// <summary>
        ///  lista con 'BoundarySegmentNew' : objeto que contiene obj:'BoundarySegment' 
        ///  pero con mayor propiedades
        /// </summary>
        public List<WrapperBoundarySegment> ListaWrapperBoundarySegment { get; set; }


        //guarda los vertices de  obj:'BoundarySegmentNH' con los valores actualados
        // actualizados significa  despues de aplicar metodo:Reordendar()
        public List<XYZ> ListaVerticesRoomActualizados { get; set; }




        #endregion

        #region 1)COntructor


        // cosntructor
        public BoundarySegmentRoomsGeom(IList<BoundarySegment> Lista,UIApplication uiapp , string nombreRoom, Floor floor)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this.floor = floor;
            this.ListaBoundarySegmentInicial = Lista;
            this._uiapp = uiapp;
            this.ListaVerticesRoomActualizados = new List<XYZ>();

            this.ListaWrapperBoundarySegment = new List<WrapperBoundarySegment>();
            AuxiliarBarraRefuerzo.listaRepetidos.Clear();
            int cont = 1;

            ConstNH.sbLog.AppendLine("BoundarySegmentRoomsGeom");
            foreach (var item in Lista)
            {
                this.ListaWrapperBoundarySegment.Add(new WrapperBoundarySegment(cont.ToString(), item, uiapp, nombreRoom, floor, Util.CmToFoot(2), 10));
                cont += 1;
            }

            //   this.ListaBoundarySegmentNH_paraSuples = new List<BoundarySegmentNH>();
        }

        public BoundarySegmentRoomsGeom(Document doc)
        {
            this.ListaBoundarySegmentInicial = new List<BoundarySegment>();

            ListaVerticesRoomActualizados = new List<XYZ>();

            ListaWrapperBoundarySegment = new List<WrapperBoundarySegment>();
            //  ListaBoundarySegmentNH_paraSuples = new List<BoundarySegmentNH>();
        }


        #endregion

        #region 2)metodos   n:1

        /// <summary>
        /// 1) reordena los BoundarySegment - uniendo elemntos que :
        /// -igual orientacion (angulo)
        /// -tiene 1 pto en comun
        /// -igual espesor espesorElemContiguo
        /// </summary>
        public List<XYZ> M1_Reordendar()
        {
            // ConstantesGenerales.sbLog.Clear();
            int cont = 0;
            double factor_Limite = 0.1;
            //borrar  linea con igual id , igual angulo, y comparten 1 coordenada
            foreach (WrapperBoundarySegment item in ListaWrapperBoundarySegment)
            {


                cont += 1;
            }

            M1_1_UnirLineasIgualId_Isvalid_igualAngulo_Comparten1coordenada(factor_Limite);

            M1_2_UnirPorIgualEspesorVecino_distintoId_igualAngulo_comparteCoordenada(factor_Limite);

            cont = 0;
            //  ConstantesGenerales.sbLog.AppendLine("LISTA FINAL");
            // genera una lista con los vertices del room, con los ptos de borde actualñizado
            foreach (WrapperBoundarySegment item in ListaWrapperBoundarySegment)
            {
                //ConstantesGenerales.sbLog.AppendLine(cont + ")I: " + item.coordenadasBorde.StartPoint.REdondear(5) + ",F:" + item.coordenadasBorde.EndPoint.REdondear(5) + " Largo:" + Math.Round(Util.FootToCm(item.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.StartPoint)), 1) + ", Ang : " + Math.Round(item.anguloGrado, 0) + " , ESpC : " + Math.Round(item.obtenerRefereciasCercanas.espesorElemContiguo, 3) + " , EleCont : " + item.obtenerRefereciasCercanas.elementoContiguo + " ISval:" + item.IsValid);
                cont += 1;
                if (item.obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.None || item.obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.Muro)
                    item.obtenerRefereciasCercanas.OBtenerRoomContiguoExtRoom();

                ListaVerticesRoomActualizados.Add(item.coordenadasBorde.StartPoint);
            }

            //AGREGA la primera pto por si elultimo cn el primer pto son la linea buscada
            ListaVerticesRoomActualizados.Add(ListaVerticesRoomActualizados[0]);

            return ListaVerticesRoomActualizados;
        }

        private void M1_2_UnirPorIgualEspesorVecino_distintoId_igualAngulo_comparteCoordenada(double factor_Limite)
        {
            int cont;
            foreach (WrapperBoundarySegment item in ListaWrapperBoundarySegment)
            {
                // salta cuando no es valido
                if (item.IsValid == false) continue;
                bool aux_continua = true;


                //item.IsSoloLineaSinVecinos es true cuando la linea 'RoomSeparator' es trazada a mano,
                // entonces esta linea debe coolindar con otra room
                while (aux_continua && item.obtenerRefereciasCercanas.IsSoloLineaSinVecinos == false)
                {

                    WrapperBoundarySegment encontradoPtoInical = ListaWrapperBoundarySegment.Where(b => //b.name != item.name &&nhs
                                                                            b.IDname != item.IDname &&
                                                                            b.IsValid == true &&
                                                                            ((b.coordenadasBorde.StartPoint.DistanceTo(item.coordenadasBorde.EndPoint) < factor_Limite || b.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.StartPoint) < factor_Limite) ||
                                                                            (b.coordenadasBorde.StartPoint.DistanceTo(item.coordenadasBorde.StartPoint) < factor_Limite || b.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.EndPoint) < factor_Limite)) &&
                                                                            Math.Abs(b.anguloGrado - item.anguloGrado) < factor_Limite &&
                                                                            (Math.Abs(b.obtenerRefereciasCercanas.espesorElemContiguo - item.obtenerRefereciasCercanas.espesorElemContiguo) < factor_Limite ||
                                                                            Math.Abs(b.obtenerRefereciasCercanas.espesorElemContiguo - Util.CmToFoot(500)) < factor_Limite)
                                                                            ).FirstOrDefault();

                    if (encontradoPtoInical != null)
                    {
                        // invalida elento encontrado
                        encontradoPtoInical.IsValid = false;
                        //al elemento item le cambia la coordenada o extienede la su coordenda incial o final
                        //con los valors de  'encontradoPtoInical'
                        if (item.coordenadasBorde.StartPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.StartPoint) < factor_Limite)
                        { item.coordenadasBorde.StartPoint = encontradoPtoInical.coordenadasBorde.EndPoint; }
                        else if (item.coordenadasBorde.StartPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.EndPoint) < factor_Limite)
                        { item.coordenadasBorde.StartPoint = encontradoPtoInical.coordenadasBorde.StartPoint; }
                        else if (item.coordenadasBorde.EndPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.StartPoint) < factor_Limite)
                        { item.coordenadasBorde.EndPoint = encontradoPtoInical.coordenadasBorde.EndPoint; }
                        else if (item.coordenadasBorde.EndPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.EndPoint) < factor_Limite)
                        { item.coordenadasBorde.EndPoint = encontradoPtoInical.coordenadasBorde.StartPoint; }

                        if (item.obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.None)
                        { item.obtenerRefereciasCercanas.elementoContiguo = encontradoPtoInical.obtenerRefereciasCercanas.elementoContiguo; }
                    }
                    else
                    { aux_continua = false; }

                }
            }

            //ConstantesGenerales.sbLog.AppendLine("SEGUNDA VUELTA");
            cont = 0;
            foreach (WrapperBoundarySegment item in ListaWrapperBoundarySegment)
            {
                //  ConstantesGenerales.sbLog.AppendLine(cont + ")I: " + item.coordenadasBorde.StartPoint.REdondear(5) + ",F:" + item.coordenadasBorde.EndPoint.REdondear(5) + " Largo:" + Math.Round(Util.FootToCm(item.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.StartPoint)), 1) + ", Ang : " + item.anguloGrado + " , ESpC : " + item.obtenerRefereciasCercanas.espesorElemContiguo + " , EleCont : " + item.obtenerRefereciasCercanas.elementoContiguo + " ISval:" + item.IsValid);
                cont += 1;
            }
            //ConstantesGenerales.sbLog.AppendLine("");
            //borrar elemtos que son Isvalida=false
            ListaWrapperBoundarySegment = ListaWrapperBoundarySegment.Where(b => b.IsValid == true).ToList();
        
        }

        private void M1_1_UnirLineasIgualId_Isvalid_igualAngulo_Comparten1coordenada(double factor_Limite)
        {
            for (int i = 0; i < ListaWrapperBoundarySegment.Count; i++)
            {
                WrapperBoundarySegment item = ListaWrapperBoundarySegment[i];
                if (item.IsValid == false) continue;
                WrapperBoundarySegment encontradoPtoInical = ListaWrapperBoundarySegment.Where(c => c.IsValid == true &&
                                                                                                 c.IDname == item.IDname &&
                                                                                                 Math.Abs(c.anguloGrado - item.anguloGrado) < factor_Limite &&
                                                                                                 (!(c.coordenadasBorde.StartPoint.DistanceTo(item.coordenadasBorde.StartPoint) < factor_Limite && c.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.EndPoint) < factor_Limite)) &&
                                                                                                 ((c.coordenadasBorde.StartPoint.DistanceTo(item.coordenadasBorde.EndPoint) < factor_Limite || c.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.StartPoint) < factor_Limite) ||
                                                                                                  (c.coordenadasBorde.StartPoint.DistanceTo(item.coordenadasBorde.StartPoint) < factor_Limite || c.coordenadasBorde.EndPoint.DistanceTo(item.coordenadasBorde.EndPoint) < factor_Limite))).FirstOrDefault();
                if (encontradoPtoInical != null)
                {
                    // invalida elento encontrado
                    encontradoPtoInical.IsValid = false;
                    //al elemento item le cambia la coordenada o extienede la su coordenda incial o final
                    //con los valors de  'encontradoPtoInical'
                    if (item.coordenadasBorde.StartPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.StartPoint) < factor_Limite)
                    { item.coordenadasBorde.StartPoint = encontradoPtoInical.coordenadasBorde.EndPoint; }
                    else if (item.coordenadasBorde.StartPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.EndPoint) < factor_Limite)
                    { item.coordenadasBorde.StartPoint = encontradoPtoInical.coordenadasBorde.StartPoint; }
                    else if (item.coordenadasBorde.EndPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.StartPoint) < factor_Limite)
                    { item.coordenadasBorde.EndPoint = encontradoPtoInical.coordenadasBorde.EndPoint; }
                    else if (item.coordenadasBorde.EndPoint.DistanceTo(encontradoPtoInical.coordenadasBorde.EndPoint) < factor_Limite)
                    { item.coordenadasBorde.EndPoint = encontradoPtoInical.coordenadasBorde.StartPoint; }


                    if (item.obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.None)
                    { item.obtenerRefereciasCercanas.elementoContiguo = encontradoPtoInical.obtenerRefereciasCercanas.elementoContiguo; }
                    i -= 1;

                }

            }

            ListaWrapperBoundarySegment = ListaWrapperBoundarySegment.Where(c => c.IsValid).ToList();
        }
        #endregion
    }
}
