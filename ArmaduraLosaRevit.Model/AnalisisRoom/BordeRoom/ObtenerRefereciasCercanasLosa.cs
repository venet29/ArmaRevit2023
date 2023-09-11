
using ArmaduraLosaRevit.Model.AnalisisRoom.DTO;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom
{
    /// <summary>
    /// Obtener referecnia de elemtos cercanos
    /// se utila cuando se trabaja con los bordes de los room
    /// determinar si el objeto de revit 'BoundarySegment' que corresponde al borde del room 
    /// tiene vecino  : viga, muro , openin, y otro room (caso pasada de puerta)
    /// lo que permite posteriormente dibujar las barra de refuerzo
    /// </summary>
    public class ObtenerRefereciasCercanasLosa
    {
        private FindReferenceTarget _findReferenceTarget_element = FindReferenceTarget.Element;
        #region 0) propiedades
        private WrapperBoundarySegment boundarySegmentNH;

        public XYZ StartPoint { get; private set; }
        public XYZ EndPoint { get; private set; }
        public XYZ VectorInteriorRoom { get; }
        public Document _doc { get; set; }

        double DISTANCIA_MIN_VECINO = Util.CmToFoot(5);
        public double espesorElemContiguo { get; set; }

        public ElementoContiguo elementoContiguo { get; set; }
        public Element neighbour { get; set; }
        public Room roomNeighbour { get; set; }
        public bool IsSoloLineaSinVecinos { get; set; }


        public List<BarraRefuerzoBordeLibre> barraRefuerzoBordeLibres { get; set; }
    
        public List<BarraRefuerzoEstribo> barraRefuerzoTipoVigas { get; set; }
        public BarraViga barraViga { get; set; }

        #endregion

        #region 1) constructor
        public ObtenerRefereciasCercanasLosa()
        {

        }
        public ObtenerRefereciasCercanasLosa( WrapperBoundarySegment boundarySegmentNH)
        {
              this._doc = boundarySegmentNH.doc;
            this.elementoContiguo = ElementoContiguo.None;
            this.IsSoloLineaSinVecinos = false;
            this.boundarySegmentNH = boundarySegmentNH;
            this.StartPoint = boundarySegmentNH.coordenadasBorde.StartPoint;
            this.EndPoint = boundarySegmentNH.coordenadasBorde.EndPoint;
            this.VectorInteriorRoom = boundarySegmentNH.coordenadasBorde.VectorInteriorRoom;
            //iniciar listas
            barraRefuerzoBordeLibres = new List<BarraRefuerzoBordeLibre>();
            barraRefuerzoTipoVigas = new List<BarraRefuerzoEstribo>();
        }

        public ObtenerRefereciasCercanasLosa(Document doc,XYZ StartPoint,XYZ EndPoint)
        {
            this._doc = doc;
            this.elementoContiguo = ElementoContiguo.None;
            this.IsSoloLineaSinVecinos = false;
            this.boundarySegmentNH = new WrapperBoundarySegment();
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;

            this.VectorInteriorRoom = Util.GetVectorPerpendicular(StartPoint, EndPoint,  0.5).GetXY0(); 

            //this.VectorInteriorRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.REVY) / 2, (StartPoint.Z + EndPoint.Z) / 2 )
         }

        #endregion

        #region 3) metodos

        /// <summary>
        /// 1)busca viga que este en contacto de borde de room
        /// 2)busca muro que este en contacto de borde de room
        /// 3)busca opening que este en contacto de borde de room
        /// 4 criterios)        
        /// </summary>
        /// <param name="LineaSeparacion"></param>
        /// <returns></returns>
        public double GetElementIntersectingBordeLosa()
        {

            double espesor = 0;
            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return espesor;
            }
            //genera pto al centro 'BoundarySegment' y 0.2 pies sobre el nivel de referencia
            var PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 + 0.2);
            XYZ VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);

            //1) busca las viga sobre losa
            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }

            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }


            /// busca la viga bajo losa
            /// //genera pto al centro 'BoundarySegment' y -0.2 pies sobre el nivel de referencia
            PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 - 0.2);
            VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);// vector hacia exterior room

            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }
            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }


            // GENERAR CCODIGO PARA BUSCAR   MUROS , OPENIN Y LOSAS


            // coordenadas medias
            //genera pto al centro 'BoundarySegment' y 0.2 pies sobre el nivel de referencia
            PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 + 0.2) + VectorInteriorRoom * 0.1;
            VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);


            //2)buscar muro
            if (OBtenerRefrenciaMuroPerpendicularALinea(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom)) return espesorElemContiguo;
            if (OBtenerRefrenciaMuroPerpendicularALinea(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1)) return espesorElemContiguo;


            return espesor;

        }

        /// <summary>
        /// 1)busca viga que este en contacto de borde de room
        /// 2)busca muro que este en contacto de borde de room
        /// 3)busca opening que este en contacto de borde de room
        /// 4 criterios)        
        /// </summary>
        /// <param name="LineaSeparacion"></param>
        /// <returns></returns>
        public double GetElementIntersectingBordeRoom()
        {     
            double espesor = 0;
            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return espesor;
            }
            //genera pto al centro 'BoundarySegment' y 0.2 pies sobre el nivel de referencia
            var PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 + 0.2);
            XYZ VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);

            //1) busca las viga sobre losa
            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }

            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }


            /// busca la viga bajo losa
            /// //genera pto al centro 'BoundarySegment' y -0.2 pies sobre el nivel de referencia
            PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 - 0.2);
            VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);// vector hacia exterior room

            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }
            if (OBtenerRefrenciaAViga(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1))
            {
                elementoContiguo = ElementoContiguo.Viga;
                return espesorElemContiguo;
            }


            // GENERAR CCODIGO PARA BUSCAR   MUROS , OPENIN Y LOSAS


            // coordenadas medias
            //genera pto al centro 'BoundarySegment' y 0.2 pies sobre el nivel de referencia
            PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 + 0.2) + VectorInteriorRoom * 0.1;
            VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint - EndPoint);


            //2)buscar muro
            if (OBtenerRefrenciaMuro(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                elementoContiguo = ElementoContiguo.Muro;
                return espesorElemContiguo;
            }
            if (OBtenerRefrenciaMuro(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1))
            {
                elementoContiguo = ElementoContiguo.Muro;
                return espesorElemContiguo;
            }

            // si ninguno de los anteriores es opening
           if(barraRefuerzoBordeLibres!=null)
                barraRefuerzoBordeLibres.Add(new BarraRefuerzoBordeLibre(boundarySegmentNH, TipoBorde.shaft, null, boundarySegmentNH.floor, boundarySegmentNH.DiametroBarra, 2));
            return espesor;

        }

        /// <summary>
        /// busca room contiguo por especio que se genera por la separacion por puerta
        /// </summary>
        /// <param name="LineaSeparacion"></param>
        /// <returns></returns>
        public double GetBordeRoom(Element LineaSeparacion)
        {
            XYZ StartPoint = boundarySegmentNH.coordenadasBorde.StartPoint;
            XYZ EndPoint = boundarySegmentNH.coordenadasBorde.EndPoint;

            double espesor = 0;
            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return espesor;
            }

            XYZ PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2);
            XYZ VectorPerpenBordeRoom = boundarySegmentNH.coordenadasBorde.VectorExteriorRoom;//  Util.GetVectorPerpendicular(StartPoint, EndPoint, 0.5);

            //CASO A)buscar muro perpendicualr a la linea de roomseparation
            if (OBtenerRefrenciaMuroPerpendicularALinea(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom)) return espesor;
            if (OBtenerRefrenciaMuroPerpendicularALinea(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1)) return espesor;

            //CASO B)
    //        ConstantesGenerales.sbLog.AppendLine("caso b");
            //busca RoomSeparationLines  -- caso refuerzo tipo viga con estrivbo y 8 refuerzo
            if (OBtenerRoomSeparationLines(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom, ref espesor))
            {
               
                // si ninguno de los anteriores es opening
                //dibujar primerbarra de refuerzo

                double[] lista = new double[] { Util.CmToFoot(10), Util.CmToFoot(6), Util.CmToFoot(-10) - espesor, Util.CmToFoot(-6) - espesor };

                for (int i = 0; i < lista.Length; i++)
                {
                    boundarySegmentNH.coordenadasBorde.GetOffset(lista[i]);
                    barraRefuerzoBordeLibres.Add(new BarraRefuerzoBordeLibre(boundarySegmentNH, TipoBorde.estribo, null, boundarySegmentNH.floor, boundarySegmentNH.DiametroBarra, 2));
                }

                //dibujar estribo
                barraRefuerzoTipoVigas.Add(new BarraRefuerzoEstribo(boundarySegmentNH, TipoBorde.estribo, boundarySegmentNH.floor, espesor, Util.CmToFoot(20), boundarySegmentNH.DiametroBarra, TipoEstriboRefuerzoLosa.ED, TipoRebar.REFUERZO_EST_REF_LO));

                return espesor;
            }

            ConstNH.sbLog.AppendLine("caso c");
            //busca RoomSeparationLines  -- caso refuerzo tipo viga con estrivbo y 8 refuerzo
            if (OBtenerRoomSeparationLines(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1, ref espesor))
            {


                double[] lista = new double[] { Util.CmToFoot(10), Util.CmToFoot(6), Util.CmToFoot(-10) - espesor, Util.CmToFoot(-6) - espesor };

                //dibuja barras refuerzo
                for (int i = 0; i < lista.Length; i++)
                {
                    boundarySegmentNH.coordenadasBorde.GetOffset(lista[i]);
                    barraRefuerzoBordeLibres.Add(new BarraRefuerzoBordeLibre(boundarySegmentNH, TipoBorde.shaft, null, boundarySegmentNH.floor, boundarySegmentNH.DiametroBarra, 2));
                }

                //dibuja estribo
                barraRefuerzoTipoVigas.Add(new BarraRefuerzoEstribo(boundarySegmentNH, TipoBorde.shaft, boundarySegmentNH.floor, espesor, Util.CmToFoot(20), boundarySegmentNH.DiametroBarra, TipoEstriboRefuerzoLosa.ED, TipoRebar.REFUERZO_EST_REF_LO));
                return espesor;
            }

            //sino encunetra nada es borde li libre
            //CASO 3) si ninguno de los anteriores es opening
            barraRefuerzoBordeLibres.Add(new BarraRefuerzoBordeLibre(boundarySegmentNH, TipoBorde.shaft, null, boundarySegmentNH.floor, 10, 2));

            return espesor;

        }

        #endregion

        #region 6 funciones auxiliares que se utlizan en  'GetBordeRoom' y 'GetBeamsIntersectingBordeRoom'
        public bool OBtenerRefrenciaMuroPerpendicularALinea(View3D elem3d, XYZ ptoCentralBordeRoom, XYZ vectorPerpenBordeRoom)
        {
            bool result = false;
            espesorElemContiguo = -Util.CmToFoot(2);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ReferenceIntersector ri = new ReferenceIntersector(filter, _findReferenceTarget_element, elem3d);


            ReferenceWithContext ref2 = ri.FindNearest(ptoCentralBordeRoom, vectorPerpenBordeRoom);

            if (ref2 != null)
            {
                if (ref2.Proximity < 0.15)
                {
                    Reference ceilingRef = ref2.GetReference();
                    neighbour = _doc.GetElement(ceilingRef);
                    Wall wallPerpenLinea = neighbour as Wall;

                    XYZ listaNormal = wallPerpenLinea.Orientation;
                    espesorElemContiguo = wallPerpenLinea.Width;
                    //

                    //double valor = Util.GetProductoEscalar((boundarySegmentNH.coordenadasBorde.EndPoint - boundarySegmentNH.coordenadasBorde.StartPoint).Normalize(), listaNormal.Normalize());
                    //CODIGO APRA BUJSCAR  SI LINES SON PERPENDIC ULARES

                    /// <summary>
                    //espsor de 500 solo de referencia para elemto corresponde a borde de los 
                    //ver referecia 2
                    /// F:\_revit\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Model\observaciones.docx
                    /// Observacion 1)
                    /// </summary>
                    espesorElemContiguo = Util.CmToFoot(500);
                    IsSoloLineaSinVecinos = true;
                    result = true;
                    string nombreViga = wallPerpenLinea.Name.ToLower().Replace(".", "").Replace("v", "").Replace("s", "").Replace("i", "");
                }
            }

            return result;
        }

        /// <summary>
        /// busca vigas segun que este a una distaciia (Proximity)  menor 0.1
        /// </summary>
        /// <param name="elem3d"></param>
        /// <param name="PtoCentralBordeRoom"></param>
        /// <param name="VectorPerpenBordeRoom"></param>
        /// <param name="espesor"></param>
        /// <returns></returns>
        public bool OBtenerRefrenciaAViga(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {
            bool result = false;
            espesorElemContiguo = -Util.CmToFoot(2);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            ReferenceIntersector ri = new ReferenceIntersector(filter, _findReferenceTarget_element, elem3d);


            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (ref2 != null)
            {
                if (ref2.Proximity < 0.15)
                {
                    Reference ceilingRef = ref2.GetReference();
                    neighbour = _doc.GetElement(ceilingRef);
                    FamilyInstance selectedCFloor = neighbour as FamilyInstance;

                    //guarda viga
                    barraViga = new BarraViga(boundarySegmentNH, selectedCFloor);
                    string nombreViga = selectedCFloor.Name.ToLower().Replace(".", "").Replace("v", "").Replace("s", "").Replace("i", "");
                    var nombreViga_ = nombreViga.Split('/');

                    if (nombreViga_.Length == 2)
                    {
                        espesorElemContiguo = Util.CmToFoot(Util.ConvertirStringInInteger(nombreViga_[0]) - 2);
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// busca openen segun un vector
        /// </summary>
        /// <param name="elem3d"></param>
        /// <param name="PtoCentralBordeRoom"></param>
        /// <param name="VectorPerpenBordeRoom"></param>
        /// <param name="espesor"></param>
        /// <returns></returns>
        public bool OBtenerRefrenciaOpening(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, ref double espesor)
        {
            bool result = false;
            espesor = -Util.CmToFoot(2);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);
            ReferenceIntersector ri = new ReferenceIntersector(filter, _findReferenceTarget_element, elem3d);


            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);


            IList<ReferenceWithContext> ref3 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (ref3 != null)
            {

                foreach (var item in ref3)
                {
                    double vcalor = item.Proximity;
                }
            }

            if (ref2 != null)
            {
                if (ref2.Proximity < 0.2)
                {
                    Reference ceilingRef = ref2.GetReference();
                    neighbour = _doc.GetElement(ceilingRef);
                    Opening selectedOpening = _doc.GetElement(ceilingRef) as Opening;
                    barraRefuerzoBordeLibres.Add( new BarraRefuerzoBordeLibre(boundarySegmentNH, TipoBorde.shaft, selectedOpening, boundarySegmentNH.floor, boundarySegmentNH.DiametroBarra, 2));
                    string nombreViga = selectedOpening.Name.ToLower().Replace(".", "").Replace("v", "").Replace("s", "").Replace("i", "");
                    return true;
                }
            }

            return result;
        }

        /// <summary>
        /// busca 'RoomSeparationLines' segun un vector
        /// </summary>
        /// <param name="elem3d"></param>
        /// <param name="PtoCentralBordeRoom"></param>
        /// <param name="VectorPerpenBordeRoom"></param>
        /// <param name="espesor"></param>
        /// <returns></returns>
        public bool OBtenerRoomSeparationLines(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, ref double espesor)
        {
            bool result = false;
            espesor = -Util.CmToFoot(2);
          
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_RoomSeparationLines);
            ReferenceIntersector ri = new ReferenceIntersector(filter, _findReferenceTarget_element, elem3d);
            IList<ReferenceWithContext> ref3 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            ConstNH.sbLog.AppendLine("PtoCentralBordeRoom: " + PtoCentralBordeRoom + ", VectorPerpenBordeRoom: " + VectorPerpenBordeRoom +", ele encotrado:"+ ref3.Count);
            string resultp = "";

            if (ref3.Count > 0)
            {
       
                double aux = 100000;
                for (int i = 0; i < ref3.Count; i++)
                {
                    resultp = resultp + ", " + ref3[i].Proximity;
                    if (ref3[i].Proximity < aux && ref3[i].Proximity < 1 && ref3[i].Proximity > DISTANCIA_MIN_VECINO)
                    {
                        //obtiene referecnia a model linea para guardar
                        Reference ceilingRef = ref3[i].GetReference();

                        neighbour = _doc.GetElement(ceilingRef);                        
                        //guardar lineamodel vecina para no volver a considerar
                        AuxiliarBarraRefuerzo.listaRepetidos.Add(neighbour.Id.ToString());
                        aux = ref3[i].Proximity - Util.CmToFoot(2);
                        espesor = aux;
                        result = true;
                    }
                    else if (ref3[i].Proximity < aux && ref3[i].Proximity < 1 && Math.Abs(ref3[i].Proximity) < ConstNH.TOLERANCIACERO)
                    {
                        //obtiene referecnia a model linea para guardar
                        Reference ceilingRef = ref3[i].GetReference();

                        if (ceilingRef.ElementId != boundarySegmentNH.boundarySegment.ElementId)
                        {
                            neighbour = _doc.GetElement(ceilingRef);
                            //guardar lineamodel vecina para no volver a considerar
                            AuxiliarBarraRefuerzo.listaRepetidos.Add(neighbour.Id.ToString());
                            aux = ref3[i].Proximity - Util.CmToFoot(2);
                            espesor = aux;
                            result = true;
                        }
                    }
                }
            }

       //     ConstantesGenerales.sbLog.AppendLine(resultp);

            return result;
        }

        /// <summary>
        /// busca room contiguo 
        /// </summary>
        /// <param name="LineaSeparacion"></param>
        /// <returns></returns>
        public bool OBtenerRoomContiguoAmbosLados()
        {
            XYZ StartPoint = boundarySegmentNH.coordenadasBorde.StartPoint;
            XYZ EndPoint = boundarySegmentNH.coordenadasBorde.EndPoint;
            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }
            XYZ PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2 + 0.2);
            XYZ VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint, EndPoint, 0.5);
            //buscar opening
            if (OBtenerRoomContiguo(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom)) return true;
            //if (OBtenerRoomContiguo(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom * -1)) return true;
            return false;
        }
        /// <summary>
        /// busca room contiguo en direccion hacia exterior o hacia fuera del room
        /// </summary>
        /// <param name="LineaSeparacion"></param>
        /// <returns></returns>
        public bool OBtenerRoomContiguoExtRoom()
        {
            if (OBtenerRoomContiguoExtRoom(0.5))  return true; 


            if (OBtenerRoomContiguoExtRoom(0.25)) return true;
            if (OBtenerRoomContiguoExtRoom(0.75)) return true;

            return false;
        }

        public bool OBtenerRoomContiguoExtRoom(double posicionAnalizada)
        {
            XYZ StartPoint = boundarySegmentNH.coordenadasBorde.StartPoint;
            XYZ EndPoint = boundarySegmentNH.coordenadasBorde.EndPoint;
            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }
            //XYZ PtoCentralBordeRoom = new XYZ((StartPoint.X + EndPoint.X)* posicionAnalizada, (StartPoint.Y + EndPoint.Y)* posicionAnalizada, (StartPoint.Z + EndPoint.Z) * posicionAnalizada + 0.2);
            XYZ PtoCentralBordeRoom = StartPoint + (EndPoint - StartPoint) * posicionAnalizada+ new XYZ(0,0,0.2);
            XYZ VectorPerpenBordeRoom = Util.GetVectorPerpendicular(StartPoint, EndPoint, 0.5);
            //buscar opening
            if (OBtenerRoomContiguo(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom))
            {
                if (elementoContiguo != ElementoContiguo.Muro)
                    elementoContiguo = ElementoContiguo.RoomContacto;
                return true;
            }

            return false;
        }
        /// <summary>
        /// busca 'Room' contiiguo segun un vector
        /// </summary>
        /// <param name="elem3d"></param>
        /// <param name="PtoCentralBordeRoom"></param>
        /// <param name="VectorPerpenBordeRoom"></param>
        /// <param name="espesor"></param>
        /// <returns></returns>
        public bool OBtenerRoomContiguo(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {
  
            //espesorElemContiguo = -Util.CmToFoot(2);

            //List< BuiltInCategory > asdf =List<BuiltInCategory>
            //foreach (var item in collection)
            //{
            double desfaseBuscarLosa = 0;  // distancia donde buscar
            if (espesorElemContiguo > 0)
                desfaseBuscarLosa = espesorElemContiguo;
            else
                desfaseBuscarLosa = Util.CmToFoot(35);

            XYZ ptoBusqueda = PtoCentralBordeRoom + VectorPerpenBordeRoom * (Util.CmToFoot(5) + desfaseBuscarLosa);
            var result2 = _doc.GetRoomAtPoint(ptoBusqueda);
            //}

            if (result2 == null)
            {
                return false;
            }
            else ///if (result2.Name != boundarySegmentNH.nameRoom)
            {
                //guarda el room vencino
                roomNeighbour = result2;
                return true;
            }

            /*
            return false;

            if (false)
            {
                // ressar un mejor metodo
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_RoomReference);
                ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
                IList<ReferenceWithContext> ref3 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom);


                if (ref3.Count > 0)
                {
                  
                    double aux = 100000;
                    for (int i = 0; i < ref3.Count; i++)
                    {
                        Reference ceilingRef = ref3[i].GetReference();
                        neighbour = _doc.GetElement(ceilingRef);
                        Room selectedRoom = neighbour as Room;

                        if (ref3[i].Proximity < aux && ref3[i].Proximity > 0.05)
                        {
                            aux = ref3[i].Proximity - Util.CmToFoot(2);
                            espesorElemContiguo = aux;
                            roomNeighbour = selectedRoom;
                            result = true;
                        }
                    }


                }
                ElementClassFilter FloorFilter = new ElementClassFilter(typeof(SpatialElement));
                ReferenceIntersector FloorIntersector = new ReferenceIntersector(FloorFilter, FindReferenceTarget.Element, elem3d);
                ref3 = FloorIntersector.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom);
                int asdda = ref3.Count;
            }


            return result;
            */
        }

        /// <summary>
        /// busca openen segun un vector
        /// </summary>
        /// <param name="elem3d"></param>nhs
        /// <param name="PtoCentralBordeRoom"></param>
        /// <param name="VectorPerpenBordeRoom"></param>
        /// <param name="espesor"></param>
        /// <returns></returns>
        public bool OBtenerRefrenciaMuro(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {
            bool result = false;
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ReferenceIntersector ri = new ReferenceIntersector(filter, _findReferenceTarget_element, elem3d);


            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (ref2 != null)
            {
                if (ref2.Proximity < 0.15)
                {
                    Reference ceilingRef = ref2.GetReference();
                    neighbour = _doc.GetElement(ceilingRef);
                    Wall selectedWall = neighbour as Wall;
                    espesorElemContiguo = selectedWall.Width;
                    string nombreViga = selectedWall.Name.ToLower().Replace(".", "").Replace("v", "").Replace("s", "").Replace("i", "");
                }
            }

            return result;
        }
        #endregion

        
    }
}
