using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.AnalisisRoom.Tag;
using ArmaduraLosaRevit.Model.Extension;
using System.Linq;

using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    /// <summary>
    /// se utliza para crear CreateRoom
    /// </summary>
    public class RoomsObj
    {

        //0) propiedades
        private Document _doc;
        public Room room { get; set; }

        private ReferenciaRoom _newRegistroLosa;

        public ReferenciaRoomDatos refereciaRoomDatos { get; set; }
        public RoomTag roomTag { get; set; }
        public double AngleRad { get; set; }
        public string Numero { get; set; }
        public double Espesor { get; set; }

        public List<XYZ> ListaInterHorizontal { get; set; }
        public List<XYZ> ListaInterLargoVertical { get; set; }
        public XYZ UbicacionRoom { get; set; }
        public double LargoHorizontal { get; set; }
        public double LargoVertical { get; set; }

        public string DireLosaHor { get; set; }
        public string DireLosaVert { get; set; }

        public string CuantiaHor { get; set; }
        public string CuantiaVert { get; set; }

        public double largoMIn { get; set; }
        public string Message { get; set; }
        public bool Isok { get; set; }

        // 1)constructores 
        /// <summary>
        /// cosntructor
        /// </summary>
        /// <param name="room"></param>
        /// <param name="AngleRad"></param>
        /// <param name="numero"></param>
        /// <param name="espesor"></param>
        public RoomsObj(Document doc, Room room, double AngleRad, string numero, double espesor, string CuantiaHor, string CuantiaVert)
        {
            this._doc = doc;
            this.AngleRad = AngleRad;
            this.room = room;
            this.Numero = numero;
            this.Espesor = espesor;
            this.CuantiaHor = CuantiaHor;
            this.CuantiaVert = CuantiaVert;
            ListaInterHorizontal = new List<XYZ>();
            ListaInterLargoVertical = new List<XYZ>();
        }
        public RoomsObj(ReferenciaRoom newRegistroLosa)
        {
            this._newRegistroLosa = newRegistroLosa;
            //this.refereciaRoomDatos = _newRegistroLosa.RefereciaRoomDatos; ;
            //this._doc = refereciaRoomDatos.Room1.Document;
            //this.AngleRad = Util.GradosToRadianes(refereciaRoomDatos.anguloPelotaLosaGrado_1);
            //this.room = refereciaRoomDatos.Room1;
            //this.Numero = refereciaRoomDatos.nombreLosa_1;
            //this.Espesor = refereciaRoomDatos.espesorCM_1;
            //this.CuantiaHor = refereciaRoomDatos.cuantiaHorizontal;
            //this.CuantiaVert = refereciaRoomDatos.cuantiaVertical;
            //this.DireLosaHor = refereciaRoomDatos.direccionHorizontal;
            //this.DireLosaVert = refereciaRoomDatos.direccionVertical;

            //ListaInterHorizontal = new List<XYZ>();
            //ListaInterLargoVertical = new List<XYZ>();

            //GetDireccionLargoMin(refereciaRoomDatos.posicionPelota,
            //                           (refereciaRoomDatos.direccionHorizontal == "1" ? TipoOrientacionBarra.Horizontal : TipoOrientacionBarra.Horizontal));


        }



        public bool GenerarDatos()
        {
            Isok = true;
            try
            {
                this.refereciaRoomDatos = _newRegistroLosa.RefereciaRoomDatos; ;
                this._doc = refereciaRoomDatos.Room1.Document;
                this.AngleRad = Util.GradosToRadianes(refereciaRoomDatos.anguloPelotaLosaGrado_1);
                this.room = refereciaRoomDatos.Room1;
                this.Numero = refereciaRoomDatos.nombreLosa_1;
                this.Espesor = refereciaRoomDatos.espesorCM_1;
                this.CuantiaHor = refereciaRoomDatos.cuantiaHorizontal;
                this.CuantiaVert = refereciaRoomDatos.cuantiaVertical;
                this.DireLosaHor = refereciaRoomDatos.direccionHorizontal;
                this.DireLosaVert = refereciaRoomDatos.direccionVertical;

                ListaInterHorizontal = new List<XYZ>();
                ListaInterLargoVertical = new List<XYZ>();

                Isok = GetDireccionLargoMin(refereciaRoomDatos.posicionCruzRoom,
                                           (refereciaRoomDatos.direccionHorizontal == "1" ? TipoOrientacionBarra.Horizontal : TipoOrientacionBarra.Horizontal));
            }
            catch (Exception)
            {
                Isok = false;
                return false;
            }
            return true;
        }


        public RoomsObj(Document doc)
        {
            this._doc = doc;
        }

        #region GenerarRoom

        public Room CreateRoom(Level level, XYZ pto)
        {
            //this._doc = doc;
            UbicacionRoom = pto;
            UV roomLocation = new UV(pto.X, pto.Y);
            room = null;

            try
            {
                using (Transaction t = new Transaction(this._doc, "Create CreateRoom_v3"))
                {
                    t.Start();
                    room = _doc.Create.NewRoom(level, roomLocation);
                    //if (null == room)
                    //{
                    //    throw new Exception("Create a new room failed.");
                    //}

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);

            }

            // Create a new room
            return room;
        }




        public void CargarDatosRoom(bool IsEspesorVariable)
        {
            //  if (room.Volume < 0.0001) return;
            // obtiener los datos largos minimos 
            if (!GetDireccionLargoMin(UbicacionRoom, TipoOrientacionBarra.Horizontal)) return; ;
            //genera los direccioness principales en funciion de los largos minimos
            GetDireccionDireccionPrincipales();
            //ageragr parametros
            AgregraParametros(_doc);


            var tipoTag = TiposRoomTagType.getRoomTagType(ConstNH.CONST_TAGROOMFAMILIA + (IsEspesorVariable ? "Var" : "") + "_" + ConstNH.CONST_ESCALA_BASE, _doc);// _doc.ActiveView.Scale
            //   roomData.CreateTagRoom(doc, roomCreat, tipoTag, null);

            AgregarTagRoom AgregarTagRoom = new AgregarTagRoom(_doc, AngleRad);
            AgregarTagRoom.M1CreaTag(room, tipoTag, UbicacionRoom);


        }
        #endregion


        // 2) metodos 
        /// <summary>
        /// 1)analiza por losas y 1ero si pto dentro de poligono e losa o room
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="pto1"></param>
        /// <param name="orientacion"></param>
        /// <param name="Angle_1"></param>
        /// <param name="m_roomSelecionado_1"></param>
        /// <returns></returns>
        public bool GetDireccionLargoMin(XYZ pto1, TipoOrientacionBarra orientacion)
        {


            //Limpia los valores de las listas       


            //float LargoMin_1 = 0;
            // LD.1) comprobra si punto esta al interior del volumen de un room
            if (!room.IsPointInRoom(new XYZ(pto1.X, pto1.Y, pto1.Z + 1)))
            { return false; }

            //LD.2) obtiene puntos del poligono de room
            List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);

            Document doc = room.Document;
            IList<IList<BoundarySegment>> boundaries = room.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012



            int n = 0;

            if (null != boundaries)
            {
                //n = boundaries.Size; // 2011
                n = boundaries.Count; // 2012
            }


            if (0 < n)
            {


                float largoBarra = 200f;
                double aux_angleRad = AngleRad;
                if (orientacion == TipoOrientacionBarra.Vertical)
                { aux_angleRad = aux_angleRad + Math.PI / 2; }
                //LD.3) genera linea auxiliar para buscar las intersecciones con el room
                Line LineAuxBuscarInters = null;
                Line LinePerpenAuxBuscarInters = null;



                XYZ p1 = new XYZ(pto1.X - largoBarra * Math.Cos(aux_angleRad), pto1.Y - largoBarra * Math.Sin(aux_angleRad), 0);
                XYZ p2 = new XYZ(pto1.X + largoBarra * Math.Cos(aux_angleRad), pto1.Y + largoBarra * Math.Sin(aux_angleRad), 0);
                //linea en la direccion de losa
                LineAuxBuscarInters = Line.CreateBound(p1, p2);

                XYZ p3 = new XYZ(pto1.X - largoBarra * Math.Cos(aux_angleRad + Math.PI / 2), pto1.Y - largoBarra * Math.Sin(aux_angleRad + Math.PI / 2), 0);
                XYZ p4 = new XYZ(pto1.X + largoBarra * Math.Cos(aux_angleRad + Math.PI / 2), pto1.Y + largoBarra * Math.Sin(aux_angleRad + Math.PI / 2), 0);
                //linea perpendicular a la direccion de losa
                LinePerpenAuxBuscarInters = Line.CreateBound(p3, p4);



                int iBoundary = 0, iSegment;

             //   List<List<PtosBoundering>> _listaDeLista = new List<List<PtosBoundering>>();
                // LD.4) iteras por los segemtos de room
                //foreach( BoundarySegmentArray b in boundaries ) // 2011
                foreach (IList<BoundarySegment> b in boundaries) // 2012
                {
                    ++iBoundary;
                    iSegment = 0;
                    //   List<PtosBoundering> _lista = new List<PtosBoundering>();
                    Debug.Write($"//************************* nuevo bunderin {iBoundary}");
                    foreach (BoundarySegment s in b)
                    {
                        ++iSegment;
                        // LD.4.1) Element neighbour = s.Element; // 2015
                        Element neighbour = doc.GetElement(s.ElementId); // 2016

                        //Curve curve = s.Curve; // 2015
                        Curve curve = s.GetCurve(); // 2016
                        if (curve == null) continue;
                        double length = curve.Length;

                        XYZ p_DESFACE = new XYZ(0, 0, 0);
                        // LD.4.2) si encuentra wall contiguo al segmento
                        if (neighbour is Wall)
                        {
                            Wall wall = neighbour as Wall;

                            double wallThickness = wall.Width - Util.CmToFoot(2);

                            // obtiene una transform q contiene un pto (Origin), q contiene la ubicacion dentro de la linea
                            // en funcion del rango [0-1] 
                            Transform derivatives = s.GetCurve().ComputeDerivatives(0.5, true);
                            XYZ midPoint = derivatives.Origin;
                            XYZ tangent = derivatives.BasisX.Normalize();
                            XYZ normal = new XYZ(tangent.Y, tangent.X * (-1), tangent.Z);
                            p_DESFACE = wallThickness * normal;
                        }


                        //// LD.4.3) generando una  linea auxiliar que representa una segmento del poligono de losa
                        XYZ startPont = s.GetCurve().GetEndPoint(0);
                        XYZ EndPoint = s.GetCurve().GetEndPoint(1);
                        Debug.Write($" segmento_{iSegment}  startPont:{startPont.REdondearString_foot(2)}    EndPoint:{EndPoint.REdondearString_foot(2)}");
                        Line LineAuxRoom = Line.CreateBound(new XYZ(startPont.X, startPont.Y, 0), new XYZ(EndPoint.X, EndPoint.Y, 0));

                        //a)linea en la direccion de la losa                             
                        IntersectionResultArray results;
                        SetComparisonResult result = LineAuxBuscarInters.Intersect(LineAuxRoom, out results);

                      
                        if (result == SetComparisonResult.Overlap)
                        {
                            IntersectionResult iResult = results.get_Item(0);  // iResult.XYZPoint
                                                                               // agrega pt de intesccion entre lines

                            XYZ nv = new XYZ(iResult.XYZPoint.X, iResult.XYZPoint.Y, pto1.Z);
                            if (!ListaInterHorizontal.Contains(nv))
                                ListaInterHorizontal.Add(nv);
                        }


                        //b)linea en la direccion perpendicular de la losa                             
                        IntersectionResultArray resultsPerpe;
                        SetComparisonResult resultPerpe = LinePerpenAuxBuscarInters.Intersect(LineAuxRoom, out resultsPerpe);
                        if (resultPerpe == SetComparisonResult.Overlap)
                        {
                            IntersectionResult iResult = resultsPerpe.get_Item(0);  // iResult.XYZPoint
                                                                                    // agrega pt de intesccion entre lines
                            XYZ nv = new XYZ(iResult.XYZPoint.X, iResult.XYZPoint.Y, pto1.Z);
                            if (!ListaInterLargoVertical.Contains(nv))
                                ListaInterLargoVertical.Add(nv);

                        }
                      //  _lista.Add(new PtosBoundering(startPont, EndPoint, result, resultPerpe));



                    }// fin for

                 //   _listaDeLista.Add(_lista);

                    if (ListaInterHorizontal.Count == 0 || ListaInterLargoVertical.Count == 0)
                    {
                        Util.ErrorMsg($"Error al buscar puntos interseccion de room de losa {Numero}");
                        return false;
                    }

                    if (ListaInterHorizontal.Count != 2)
                    {
                        XYZ ptomouse = pto1.GetXY0();
                        ListaInterHorizontal.Clear();
                        var listaMinPtoHor = ListaInterHorizontal.Where(pt1 => (Math.Abs(Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1)) > 90)).ToList();
                        if (listaMinPtoHor.Count > 0)
                        {
                            XYZ ptominH = listaMinPtoHor.MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            // XYZ ptominH = ListaInterHorizontal.Where(pt1 => (Math.Abs(Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1)) > 90)).MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            ListaInterHorizontal.Add(ptominH);
                        }

                        var listaptoMaxH = ListaInterHorizontal.Where(pt1 => (Math.Abs(Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1)) < 90)).ToList();
                        if (listaptoMaxH.Count > 0)
                        {
                            XYZ ptoMaxH = listaptoMaxH.MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            //XYZ ptoMaxH = ListaInterHorizontal.Where(pt1 => (Math.Abs(Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1)) < 90)).MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));                          
                            ListaInterHorizontal.Add(ptoMaxH);
                        }

                    }

                    if (ListaInterLargoVertical.Count != 2)
                    {
                        XYZ ptomouse = pto1.GetXY0();
                        ListaInterLargoVertical.Clear();

                        var ListaptominH = ListaInterLargoVertical.Where(pt1 => (Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1.GetXY0()) < 0)).ToList();
                        if (ListaptominH.Count > 0)
                        {
                            XYZ ptominH = ListaptominH.MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            //XYZ ptominH = ListaInterLargoVertical.Where(pt1 => (Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1.GetXY0()) < 0)).MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            ListaInterLargoVertical.Add(ptominH);
                        }

                        var listaptoMaxH = ListaInterLargoVertical.Where(pt1 => (Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1.GetXY0()) > 0)).ToList();
                        if (listaptoMaxH.Count > 0)
                        {
                            XYZ ptoMaxH = listaptoMaxH.MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            //XYZ ptoMaxH = ListaInterLargoVertical.Where(pt1 => (Util.AnguloEntre2PtosGrados_enPlanoXY(ptomouse, pt1.GetXY0()) > 0)).MinBy(pt => ptomouse.DistanceTo(pt.GetXY0()));
                            ListaInterLargoVertical.Add(ptoMaxH);
                        }


                    }
                    //LD.4.4) busca las direccion principal
                    if (ListaInterHorizontal.Count != 2)
                    {
                        //TaskDialog.Show("Error ", " Error en " + this.Numero.ToString() + " obtener largo horizontal de room. No se calculo Largo minimo y direcciones de losa Default   ");

                    }

                    else if (ListaInterLargoVertical.Count != 2)
                    {
                        //   TaskDialog.Show("Error ", " Error en " + this.Numero.ToString() + "  obtener largo Vertical de room. No se calculo Largo minimo y direcciones de losa Default  ");

                    }
                    else
                    {
                        LargoHorizontal = ListaInterHorizontal[0].DistanceTo(ListaInterHorizontal[1]);
                        LargoVertical = ListaInterLargoVertical[0].DistanceTo(ListaInterLargoVertical[1]);


                        if (LargoHorizontal < LargoVertical) // la direccion princiapl en la direccion de la pelota de losa
                        {
                            DireLosaHor = "1";
                            DireLosaVert = "2";
                            largoMIn = LargoHorizontal;
                        }
                        else // la direccion princiapl en la direccion de la perpendicular a  pelota de losa
                        {
                            DireLosaHor = "2";
                            DireLosaVert = "1";
                            largoMIn = LargoVertical;
                        }
                        break;
                    }

                }
            }

            return true;
        }


        /// <summary>
        /// obtiene la direccion pricipales segun los largos minimos
        /// </summary>
        public void GetDireccionDireccionPrincipales()
        {

            //LD.4.4) busca las direccion principal
            if (ListaInterHorizontal.Count != 2)
            {
                TaskDialog.Show("Error ", " Error en " + this.Numero.ToString() + " obtener largo horizontal de room. No se calculo Largo minimo y direcciones de losa Default   ");
                DireLosaHor = "1";
                DireLosaVert = "2";
            }
            else if (ListaInterLargoVertical.Count != 2)
            {
                TaskDialog.Show("Error ", " Error en " + this.Numero.ToString() + "  obtener largo Vertical de room. No se calculo Largo minimo y direcciones de losa Default  ");
                DireLosaHor = "1";
                DireLosaVert = "2";
            }
            else
            {
                if (LargoHorizontal < LargoVertical) // la direccion princiapl en la direccion de la pelota de losa
                {
                    DireLosaHor = "1";
                    DireLosaVert = "2";
                    largoMIn = LargoHorizontal;
                }
                else // la direccion princiapl en la direccion de la perpendicular a  pelota de losa
                {
                    DireLosaHor = "2";
                    DireLosaVert = "1";
                    largoMIn = LargoVertical;
                }

            }
        }

        /// <summary>
        /// Agrega parametros a room
        /// </summary>
        /// <param name="doc"></param>
        internal void AgregraParametros(Document doc)
        {
            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create AgregraParametros-NH");


                    ParameterUtil.SetParaInt(room, "Numero Losa", Numero.ToString());
                    ParameterUtil.SetParaInt(room, "Espesor", Espesor);
                    ParameterUtil.SetParaInt(room, "Angulo", AngleRad);
                    ParameterUtil.SetParaInt(room, "Cuantia Horizontal", CuantiaHor);
                    ParameterUtil.SetParaInt(room, "Cuantia Vertical", CuantiaVert);
                    ParameterUtil.SetParaInt(room, "Direccion Horizontal", DireLosaHor);
                    ParameterUtil.SetParaInt(room, "Direccion Vertical", DireLosaVert);
                    ParameterUtil.SetParaInt(room, "LargoMin", largoMIn);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);
                return;
            }


            //throw new NotImplementedException();
        }


        public void BorrarLineaTextoLargoMin()
        {
            ICollection<ElementId> elem = new List<ElementId>();
            //obtener la lista de id a boorar por cada room
            // esta gusraddo como texto dentro de un parametro compartido

            ParameterSet paras = room.Parameters;

            string listaStringId = ParameterUtil.FindValueParaByName(paras, "ElementoBorrar", _doc);

            if (listaStringId == null) return;

            string[] listaLEmentosBorrrar = listaStringId.Split(',');

            foreach (string id in listaLEmentosBorrrar)
            {
                ElementId nw = new ElementId(Util.ConvertirStringInInteger(id));
                if ((nw == null)) continue;
                Element elementos = _doc.GetElement2(nw);
                if ((elementos == null)) continue;
                if (elementos.IsValidObject) elem.Add(nw);
            }
            
            _doc.Delete(elem);



        }

    }

    public class PtosBoundering
    {
        static int coont = 0;
        private XYZ startPont;
        private XYZ endPoint;

        public PtosBoundering(XYZ startPont, XYZ endPoint, SetComparisonResult result, SetComparisonResult resultPerpe)
        {
            coont += 1;
            this.startPont = startPont;
            this.endPoint = endPoint;
            Result = result;
            ResultPerpe = resultPerpe;
        }


        public SetComparisonResult Result { get; }
        public SetComparisonResult ResultPerpe { get; }
    }
}

