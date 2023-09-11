using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.UTILES;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom

{


    [DebuggerDisplay("{coordenadasBorde.StartPoint} , {coordenadasBorde.EndPoint}")]
    public class WrapperBoundarySegment
    {
       
        #region 1)propiedades  n:16
        public UIApplication _uiapp { get; set; }
        public Document doc { get; set; }
        public ObtenerRefereciasCercanas obtenerRefereciasCercanas { get; set; }
        public BoundarySegmentCoordenadas coordenadasBorde { get; set; }
        public BoundarySegment boundarySegment { get; set; }
        public Floor floor { get; set; }
        public Curve curveBoundarySegment { get; set; } 

        public string nameRoom { get; private set; }
        public string IDname { get; set; }     
        //angulo de curve
        public double anguloGrado { get; set; }

        public bool IsValid { get; set; }

      //  public bool IsSoloLineaSinVecinos { get; set; }



        // dos variables para cuando se crean suples automaticamente
        public Room rom1_crearSuple { get; set; }
        public Room rom2_crearSuple { get; set; }

        #region POSIBLES ELEMETOS VECINOS       
        public int DiametroBarra { get; set; }
        #endregion

        #endregion

        #region 1)Contructor   n:2+1

        //#1
        public WrapperBoundarySegment()
        {

        }
        //#2
        public WrapperBoundarySegment(string name, BoundarySegment boundarySegment, UIApplication uiapp, string nombreRoom, Floor floor, double offsetMoverbarra, int DiametroBarra)
        {
            this.floor = floor;
            this.nameRoom = nombreRoom;
            this.IDname = boundarySegment.ElementId.IntegerValue.ToString();
            this.doc = uiapp.ActiveUIDocument.Document;
           
            this.boundarySegment = boundarySegment;
            this._uiapp = uiapp;
            this.IsValid = true;
            
            this.DiametroBarra = DiametroBarra;

            obtenerRefereciasCercanas = new ObtenerRefereciasCercanas(this);
            coordenadasBorde = new BoundarySegmentCoordenadas(this, offsetMoverbarra);
            //offset sobre y bajo la losa 
           
            cargando();

            if (name == "8")
            { }

            buscarVecinos();
            
        }

        private void cargando()
        {         
            //Curve curve = s.Curve; // 2015
            curveBoundarySegment = this.boundarySegment.GetCurve(); // 2016
            double length = curveBoundarySegment.Length;
            this.anguloGrado = Util.AnguloEntre2PtosGrado90(coordenadasBorde.StartPoint, coordenadasBorde.EndPoint, true);

        }

        /// <summary>
        /// responsabilidad: buscar al vecino en la direccion de la normal opuesta al centro del room
        /// </summary>
        void buscarVecinos()
        {
            // elemento  que esta contiguo (junto) al room, puede ser un 'muro (wall)'
            //o modeline : para esta caso son 3 otciones =  viga contigua, opening contiguo, u otra room contiguo
            Element neighbour = doc.GetElement(this.boundarySegment.ElementId); // 2016

            obtenerRefereciasCercanas.espesorElemContiguo = -3;
            //ConstantesGenerales.sbLog.AppendLine("------------------------------------------------------------------------------");
            //ConstantesGenerales.sbLog.AppendLine("StartPoint: " + coordenadasBorde.StartPoint + ", EndPoint: " + coordenadasBorde.EndPoint + ",  VectorExteriorRoom :" + coordenadasBorde.VectorExteriorRoom);
            //Curve curve = s.Curve; // 2015
            Curve curve = this.boundarySegment.GetCurve(); // 2016

            //XYZ p_DESFACE = new XYZ(0, 0, 0);
            if (neighbour is Wall)
            {
               // ConstantesGenerales.sbLog.AppendLine("a)Vecinos Muros");
                Wall wall = neighbour as Wall;
                obtenerRefereciasCercanas.elementoContiguo = ElementoContiguo.Muro;

                //  ACI 8.7.4.1.3   - EXPLICA EL MENOS 2
                double wallThickness = wall.Width - Util.CmToFoot(2);
                obtenerRefereciasCercanas.espesorElemContiguo = wallThickness;
        

            }
            else if (neighbour is ModelLine)
            {
                
                Element neighbour_ = (Element)neighbour;
                int valor = neighbour_.Id.IntegerValue;

                if (valor == 2037686) valor = 0;
                //ConstantesGenerales.sbLog.AppendLine("dentro vecino  ID:"+ neighbour_.Id.ToString()+ "    listaRepetidos#: " + AuxiliarBarraRefuerzo.listaRepetidos.Count.ToString());
                string asdas = neighbour_.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                if (TipoPhases_.IsFasesExistenete(neighbour))// && ((Element)neighbour).get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString() == "None")
                {
                    //ConstantesGenerales.sbLog.AppendLine("b)Existing");
                    // busca en las model lineas que se crearon:
                    //a) para vigas : aquyi encuentra vigas y encuentras es pesor 
                    // b)para openin de losas  : aqui no encuentra nada , asi que regresa  espesor = -2
                    obtenerRefereciasCercanas.espesorElemContiguo = obtenerRefereciasCercanas.GetElementIntersectingBordeRoom();

                    //if (obtenerRefereciasCercanas.espesorElemContiguo < 0)
                    //{ obtenerRefereciasCercanas.espesorElemContiguo = 0; }
                }
                else
                {
                    // aqui  pasan la ModelLine = 'New construction' --lineas creadas para cerrar poligono losa
           //         ConstantesGenerales.sbLog.AppendLine("c.1)NO Existing");
                    if (AuxiliarBarraRefuerzo.listaRepetidos.Contains(neighbour_.Id.ToString()) == false) // comprueba que linea no este analiza anteriormente
                    {
           //             ConstantesGenerales.sbLog.AppendLine("c.2)NO Existing");
                        //aqui es cuando el modelline es la inteseccopn entre dos room
                        // entonces prolonga barra la mitad de su traslapo
                        obtenerRefereciasCercanas.espesorElemContiguo = obtenerRefereciasCercanas.GetBordeRoom(neighbour_);
                        if (Math.Abs(obtenerRefereciasCercanas.espesorElemContiguo - -Util.CmToFoot(2)) <= 0.001)
                        {
           //                 ConstantesGenerales.sbLog.AppendLine("IsSoloLineaSinVecinos");
                            //  obtenerRefereciasCercanas.espesorElemContiguo = 0;
                            obtenerRefereciasCercanas.IsSoloLineaSinVecinos = true;
                        }
                        //// si es iguala cero
                        //if (Math.Abs(obtenerRefereciasCercanas.espesorElemContiguo) < 0.001)
                        //{
                        //    ConstantesGenerales.sbLog.AppendLine("espesorElemContiguo = UtilBarras.largo_traslapoFoot(8) / 2");
                        //    obtenerRefereciasCercanas.espesorElemContiguo = UtilBarras.largo_traslapoFoot(10) / 2;
                        //}
     
                    }
                }
            }

            //buscar room vecino




     //       ConstantesGenerales.sbLog.AppendLine($"  espesor : {Util.FootToCm(obtenerRefereciasCercanas.espesorElemContiguo)}");
            #region codigo borrrado           
            //Curve auxCurve = this.boundarySegment.GetCurve();
            //double largo = auxCurve.Length;
            //Transform derivatives = this.boundarySegment.GetCurve().ComputeDerivatives(0.5, true);           
            //Transform derivatives1 = this.boundarySegment.GetCurve().ComputeDerivatives(1, true);
            //XYZ midPoint = derivatives.Origin;
            //XYZ tangent = derivatives.BasisX.Normalize();
            //XYZ normal = new XYZ(tangent.Y, tangent.X * (-1), tangent.Z);
            #endregion

            XYZ normal = Util.GetVectorPerpendicular(this.boundarySegment.GetCurve(), 0.5);
       //     ConstantesGenerales.sbLog.AppendLine($"  normal : {normal.ToString()}");
            // wallThickness = 0;
            coordenadasBorde.desface = (obtenerRefereciasCercanas.espesorElemContiguo !=-3? obtenerRefereciasCercanas.espesorElemContiguo * normal: new XYZ(0,0,0)) ;
      //      ConstantesGenerales.sbLog.AppendLine($"  desface : {coordenadasBorde.desface.ToString()}");

        }


        #endregion

        public void DibujarBarraRefuerzoBordeLibres()
        {
            foreach (BarraRefuerzoBordeLibre item in obtenerRefereciasCercanas.barraRefuerzoBordeLibres)
            {
               // List<Curve> listaCurvas = new List<Curve>() { Line.CreateBound(coordenadasBorde.StartPoint, coordenadasBorde.EndPoint) };
              //  AyudaRevisionCoud.CrearNuve(doc.ActiveView, listaCurvas);

                item.generarBarra_ConTrans();
            }
        }

        public void DibujarBarraRefuerzoTipoVigas()
        {
            foreach (BarraRefuerzoEstribo item in obtenerRefereciasCercanas.barraRefuerzoTipoVigas)
            {
               // List<Curve> listaCurvas = new List<Curve>() { Line.CreateBound(coordenadasBorde.StartPoint, coordenadasBorde.EndPoint) };
               // AyudaRevisionCoud.CrearNuve(doc.ActiveView, listaCurvas);
                item.generarBarra();
            }
        }

        #region 2)metodos  n:1

        public bool CrearPtosSuple()
        {
            bool result = false;
            rom1_crearSuple = null;
            rom2_crearSuple = null;

            if (obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.Muro)
            {

                XYZ PtoCentralBordeRoom = new XYZ((coordenadasBorde.StartPoint.X + coordenadasBorde.EndPoint.X) / 2, (coordenadasBorde.StartPoint.Y + coordenadasBorde.EndPoint.Y) / 2, (coordenadasBorde.StartPoint.Z + coordenadasBorde.EndPoint.Z) / 2);
                XYZ VectorPerpenBordeRoom = Util.GetVectorPerpendicular(coordenadasBorde.StartPoint, coordenadasBorde.EndPoint, 0.5);


                coordenadasBorde.StartPointSuples = PtoCentralBordeRoom + VectorPerpenBordeRoom * (Util.CmToFoot(30) + obtenerRefereciasCercanas.espesorElemContiguo);
                coordenadasBorde.EndPointSuples = PtoCentralBordeRoom - VectorPerpenBordeRoom * (Util.CmToFoot(30) + obtenerRefereciasCercanas.espesorElemContiguo);

                rom1_crearSuple = doc.GetRoomAtPoint(coordenadasBorde.StartPointSuples);
                rom2_crearSuple = doc.GetRoomAtPoint(coordenadasBorde.EndPointSuples);

                if (rom1_crearSuple == null || rom2_crearSuple == null)
                {
                    coordenadasBorde.EndPointSuples = null;
                    coordenadasBorde.StartPointSuples = null;
                    result = false;
                }
                else

                { result = true; }
            }

            return result;
        }

        #endregion


    }
}
