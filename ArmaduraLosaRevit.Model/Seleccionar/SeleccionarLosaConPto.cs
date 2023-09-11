using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

//using planta_aux_C.Elemento_Losa;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using System.Diagnostics;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.Seleccionar
{
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class SeleccionarLosaConPto
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        List<XYZ> ListaPtos = new List<XYZ>();
       // private  ExternalCommandData m_revit;
        private  Document doc;



        public SeleccionarLosaConPto(Document doc)
        {
            this.doc = doc;
        }


        public Func<XYZ, bool> CaraSuperior = (pt) => SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZPositivo(pt);
        public Func<XYZ, bool> CaraInferior = (pt) => SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZNegativo(pt);
        public Func<XYZ, bool> PointsUpwards = (pt) => Util.PointsUpwards(pt);
        /// <summary>
        /// otra alternativa para obtener losa con ptro y nivel
        /// </summary>
        /// <param name="pt_selec"></param>
        /// <param name="levelLOsa"></param>
        /// <returns></returns>
        public Floor EjecturaSeleccionarLosaConPto(XYZ pt_selec,Level levelLOsa)
        {
            Floor floorSeleccionada=null;

            List<Element> floors = SeleccionElement.GetElementoFromLevel(doc, typeof(Floor), levelLOsa);
            //floors.ForEach(f => Debug.WriteLine($" losa id_ {f.Id.IntegerValue}"));
            if (floors.Count > 0)
                floorSeleccionada = floors.Cast<Floor>().Where(c => c.ObtenerEspesorConPtosFloor(pt_selec) !=0).FirstOrDefault();//   BuscarLosaContengaPto(pt_selec, floors, CaraSuperior);
           
            return floorSeleccionada;
        }



        public Floor EjecturaSeleccionarLosaConPtoInclinada(XYZ pt_selec, Level levelLOsa)
        {
            Floor floorSeleccionada = null;

            List<Element> floors = SeleccionElement.GetElementoFromLevel(doc, typeof(Floor), levelLOsa);

            if (floors.Count > 0)
                floorSeleccionada = floors.Cast<Floor>().Where(c => c.ObtenerEspesorConPtosFloor(pt_selec) != 0).FirstOrDefault();// BuscarLosaContengaPto(pt_selec, floors, PointsUpwards);

            return floorSeleccionada;
        }
        public PuntoEnLosa EjecturaEstaPtoDentroDeLosaHorizontal(XYZ pt_selec, Floor levelLOsa)
        {
            if (pt_selec == null) return PuntoEnLosa.ptoNull;
            if (pt_selec.DistanceTo(XYZ.Zero)<0.1) return PuntoEnLosa.ptCero;
            if (!(levelLOsa is Floor)) return PuntoEnLosa.losaNull;


            return (levelLOsa.ObtenerEspesorConPtosFloor(pt_selec, false) == 0
                     ? PuntoEnLosa.PtoFueraLosa
                     : PuntoEnLosa.PtoDentroLosa);

        }


        public PlanarFace SeleccionarCAraInferiorFloor(XYZ pt_selec, Floor levelLOsa)
        {
            if (pt_selec == null) return null;
            if (pt_selec.DistanceTo(XYZ.Zero) < 0.1) return null;
            if (!(levelLOsa is Floor)) return null;

           // PlanarFace floorSeleccionada = null;

            List<Element> floors = new List<Element>();
            floors.Add(levelLOsa);

            return floors.Cast<Floor>().Where(c => c.ObtenerEspesorConPtosFloor(pt_selec) != 0).Select(r => r.ObtenerCaraInferior()).FirstOrDefault();
        }
        public PlanarFace SeleccionarCAraInferiorFoorOFundation(XYZ pt_selec, Element elementSeleccion)
        {
            if (pt_selec == null) return null;
            if (pt_selec.DistanceTo(XYZ.Zero) < 0.1) return null;
           // if (!(elementSeleccion is Floor)) return null;

            return elementSeleccion.ObtenerCaraInferior(pt_selec, XYZ.BasisZ);
        }
        public PlanarFace SeleccionarCAraSuperiorFoorOFundation(XYZ pt_selec, Element elementSeleccion)
        {
            if (pt_selec == null) return null;
            if (pt_selec.DistanceTo(XYZ.Zero) < 0.1) return null;
            // if (!(elementSeleccion is Floor)) return null;

            return elementSeleccion.ObtenerCaraSuperior(pt_selec, new XYZ(0, 0, -1));
        }

        public PuntoEnLosa EjecturaEstaPtoDentroDeLosaInclinada(XYZ pt_selec, Floor levelLOsa)
        {
            if (pt_selec == null) return PuntoEnLosa.ptoNull;
            if (pt_selec.DistanceTo(XYZ.Zero) < 0.1) return PuntoEnLosa.ptCero;
            if (!(levelLOsa is Floor)) return PuntoEnLosa.losaNull;

            return (levelLOsa.ObtenerEspesorConPtosFloor(pt_selec,false) == 0 
                    ? PuntoEnLosa.PtoFueraLosa
                    : PuntoEnLosa.PtoDentroLosa);

            //Floor floorSeleccionada = null;

            //List<Element> floors = new List<Element>();
            //floors.Add(levelLOsa);

            //floorSeleccionada =   BuscarLosaContengaPto(pt_selec, floors, PointsUpwards);

            //return (floorSeleccionada == null ? PuntoEnLosa.PtoFueraLosa : PuntoEnLosa.PtoDentroLosa);
        }


     
        public Floor SeleccionarLosaConRoom(Room room)
        {

            GeometryElement sad = room.ClosedShell;
            XYZ minf = sad.GetBoundingBox().Min;
            XYZ mxf = sad.GetBoundingBox().Max;
            XYZ pt_selec = (mxf + minf) / 2;
            Floor floorSeleccionada = null;

            List<Element> floors = SeleccionElement.GetElementoFromLevel(room.Document, typeof(Floor), room.Level);

            if (floors.Count > 0)
                floorSeleccionada = floors.Cast<Floor>().Where(c => c.ObtenerEspesorConPtosFloor(pt_selec) != 0).FirstOrDefault();//  BuscarLosaContengaPto(pt_selec, floors, CaraSuperior);

            return floorSeleccionada;
        }


        //obsoleto
        /// <summary>
        /// busca la losa (dentro de una lista de losas) que contiene 1 pto
        /// determinado
        /// </summary>
        /// <param name="pto"> pto buscadoi</param>
        /// <param name="floors"> lista de losas </param>
        /// <returns> floor que contiene el pto buscado</returns>
        private Floor BuscarLosaContengaPto(XYZ pto,  List<Element> floors, Func<XYZ, bool> func)
        {
            Floor LosaConPto = null;
            foreach (Floor floor in floors)
            {
                double espesor =floor.ObtenerEspesorConPtosFloor(pto, false);

                if (espesor != 0)
                {
                    return floor;
                }


            }
            return LosaConPto;
        }

        //obsoleto
        private PlanarFace BuscarPlanarfacedeLosaContengaPto_inferior(XYZ pto, List<Element> floors)
        {
            foreach (Floor floor in floors)
            {
                double espesor = floor.ObtenerEspesorConPtosFloor(pto, false);

                if (espesor != 0)
                {
                    return floor.ObtenerCaraInferior();
                }


            }
            return null;
        }

        //obsoleto
        private PlanarFace BuscarPlanarfacedeLosaContengaPto(XYZ pto, List<Element> floors, Func<XYZ, bool> func)
        {
            PlanarFace encontradaface = null;
            foreach (var floor in floors)
            {
                Element floor_1 = null;
                floor_1 = floor as Floor;
                Options gOptions = new Options();
                gOptions.ComputeReferences = false;
                gOptions.DetailLevel = ViewDetailLevel.Coarse;
                gOptions.IncludeNonVisibleObjects = true;
                GeometryElement geo = floor.get_Geometry(gOptions);
                foreach (GeometryObject obj in geo) // 2013
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
#pragma warning disable CS0219 // The variable 'contados' is assigned but its value is never used
                        int contados = 1;
#pragma warning restore CS0219 // The variable 'contados' is assigned but its value is never used
                        foreach (Face face in solid.Faces)
                        {

                            PlanarFace pf = face as PlanarFace;
                            if (SeleccionarPtoDentroPlanarFace.EStaPuntoALInteriroDeCaraDeUnaLosa(pto, pf) &&
                               func(pf.FaceNormal))// SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZPositivo(pf.FaceNormal))
                            {

                                encontradaface=(PlanarFace)face;
                            }
                        }
                    }
                }
            }
            return encontradaface;
        }

    }



}
