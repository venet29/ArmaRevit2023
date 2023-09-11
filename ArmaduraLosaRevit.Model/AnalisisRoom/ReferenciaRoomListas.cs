using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.Elemento_Losa;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    // tarea 1: obtiene lista con room pasados a los Objeto 'RefereciaRoom'
    // tarea 2: obtener lista con los  'BoundarySegment' de los cada 'RefereciaRoom' de la lista Lista_RefereciaRoom
    public class ReferenciaRoomListas
    {


        // public static List<NH_RefereciaLosaParaSuple> ListaPtos_Vertical_Suples { get; set; }
        //public static List<NH_RefereciaLosaParaSuple> ListaPtos_Horizontal_Suples { get; set; }
        public UIDocument _uidoc { get; set; }
        //almacena los elemtos de referencia de losa
        public List<ReferenciaRoom> Lista_RefereciaRoom { get; set; }
        public List<NH_RefereciaCrearBarra> ListaPtos_Vertical_Barra { get; set; }
        public List<NH_RefereciaCrearBarra> ListaPtos_Horizontal_Barra { get; set; }


        public bool IsImprimirayuda = false;
        private  UIApplication _uiapp;
        private Document _doc;

        public bool Isok { get; set; }



        public ReferenciaRoomListas(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this.Lista_RefereciaRoom = new List<ReferenciaRoom>();
            this.ListaPtos_Vertical_Barra = new List<NH_RefereciaCrearBarra>();
            this.ListaPtos_Horizontal_Barra = new List<NH_RefereciaCrearBarra>();
   
        }

        #region 0) METODOS PRICIPALES   n:2
        /// <summary>
        /// obtiene lista con room pasados a los Objeto 'RefereciaRoom'
        ///  tres tipos de seleccion :
        ///  1) SelectConMouse  : seleccionacon el mouse los room a seleccionar
        ///  2>) selecconar dos los nivels (AUN NO IMPLEMENTADO)
        ///  3) selecciona los room del nivel analizado
        /// </summary>
        /// <param name="tipoSelecion"></param>
        /// <param name="largoIzquierdo"></param>
        /// <param name="largoDerecho"></param>
        /// <param name="v4"></param>
        /// <param name="uidoc"></param>
        /// <returns></returns>

        public List<ReferenciaRoom> GetLista_RefereciaRoom(string tipoSelecion)
        {
            Isok = true;
            //crear objeto seleccionar
            RoomSeleccionar roomSeleccionar = new RoomSeleccionar();

            Lista_RefereciaRoom = new List<ReferenciaRoom>();

            IEnumerable<Element> ListaRooms = new List<Element>();

            if (tipoSelecion == "SelectConMouse")
            {
                ListaRooms = roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(_uidoc);
            }
            else if (tipoSelecion == "Select1ElementoConMouse")
            {
                ListaRooms = roomSeleccionar.GetRoomSeleccionados1Room(_uidoc);
            }
            else if (tipoSelecion == "SelectAllTodosLosNiveles")
            {

            }
            else if (tipoSelecion == "GetSelectionAllNivelActual") //selecicona todos los del planta analizados
            {
                ListaRooms = roomSeleccionar.GetRoomNivelActual(_uidoc);
            }


            if (roomSeleccionar.Isok == false)
            {
                Isok = false;
                return new List<ReferenciaRoom>();
            }

            List<Room> ListaRoomBorrar = new List<Room>();

            // analiza los wall sewleccionados
            if (ListaRooms.Count() > 0)
            {
                foreach (Room room in ListaRooms)
                {

                    var Losa = room.GetGeometricObjects2();

                    ReferenciaRoom newRegistroLosa = new ReferenciaRoom(_doc ,room);


                    if (newRegistroLosa.IsOk == false) continue;
                    // Obtiene los vertices de Room y los borde de room
                    newRegistroLosa.GetVerticesYBordesDeRoom();//obtiene vertices

                    // si nuevoregistroLosa tiene menos de 2 vertices salta y si tiene 0 vertices entonces no esta asignado
                    // se guarda en listaroomborrar para borrar
                    if (newRegistroLosa.ListaVerticesPoligonoLosa.Count < 2)
                    {
                        //si vertitices =0 guardar en listaroomborrarnn
                        if (newRegistroLosa.ListaVerticesPoligonoLosa.Count == 0) ListaRoomBorrar.Add(room);
                        continue;
                    }

                    //obtener punto central de roomnn
                    XYZ roomCenter = newRegistroLosa.RefereciaRoomDatos.posicionPelota + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT;

                    //obtiene y asigna element floor del room
                    Element elementRoomSelec = RoomFuncionesSeleccionarLosa.ObtenerElement_DeLosaConUnPto(_uidoc, roomCenter);

                   
                    if (elementRoomSelec == null)
                    {
                        Lista_RefereciaRoom.Clear();
                        return Lista_RefereciaRoom;
                    }
                    if (!RoomFuncionesSeleccionarLosa.NoEsStructural(elementRoomSelec))
                    {
                        Lista_RefereciaRoom.Clear();
                        return Lista_RefereciaRoom;
                    }

                    newRegistroLosa.RefereciaRoomDatos.PelotaLosa = elementRoomSelec;


                    //obtiene los parametros intenos de losa
                    newRegistroLosa.RefereciaRoomDatos.GetParametrosUnRoom();
                    //obtiene largo minimo
                    newRegistroLosa.RefereciaRoomDatos.GetLargoMin();

                    //-determinar tipo de losas
                    //var aux = ListasSuples_Todos.Where(c => c.Plosa1.PelotaLosa == item.PelotaLosa).Select(m => m.Plosa1.tipo).Distinct().ToList();
                    Lista_RefereciaRoom.Add(newRegistroLosa);
                }
            }

            #region Borrar room no asignada


            //borras los room que no estan asignados
            if (ListaRoomBorrar.Count > 0)
            {
                using (Transaction transaction = new Transaction(_uidoc.Document))
                {
                    try
                    {
                        transaction.Start("External Tool-NH");
                        //delete all the unpinned dimensions
                        foreach (var roomBorrar in ListaRoomBorrar) _uidoc.Document.Delete(roomBorrar.Id);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        TaskDialog.Show("Revit", ex.Message);
                    }
                }
            }
            #endregion


            return Lista_RefereciaRoom;
        }



        #region 2) obtener lista con los  'BoundarySegment' de los cada 'RefereciaRoom' de la lista Lista_RefereciaRoom
        /// <summary>__
        /// descompone los obj 'RefereciaLosa' (que viene de room) obtiene de cada objeto  ´BoundarySegment´
        /// y crea  obj: 'BoundarySegmentRoomsGeom'
        /// 
        /// devuelve una lista 'List<NH_RefereciaRoom>' con los elemtos 'BoundarySegment'  analizados
        /// </summary>
        /// <param name="largominimo"></param>
        /// <param name="porcentajeTramosBarraInferior"></param>
        /// <param name="porcentajeTramosBarraSuple"></param>
        /// <param name="dibujar_pto_horizontal"></param>
        /// <param name="dibujar_pto_vertical"></param>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        internal List<ReferenciaRoom> M1_GetLista_BoundarySegment()
        {

            if (Lista_RefereciaRoom == null) return new List<ReferenciaRoom>();
            if (Lista_RefereciaRoom.Count == 0) return Lista_RefereciaRoom;
            //si lista esta vacia
            // if (Lista_RefereciaRoom.Count==0) return Lista_RefereciaRoom;
            // Lista_RefereciaRoom.Clear();
            M1_1_Reordenar();
            M1_2_GenerarMatrizPtoBarraSUple();
            //asigna el tipo de de empotramiento para room
            M1_3_GetTipoEmpotramiento();

            return Lista_RefereciaRoom;
            // throw new NotImplementedException();
        }

        private void M1_1_Reordenar()
        {
            if (Lista_RefereciaRoom.Count == 0) return;
            foreach (ReferenciaRoom NH_RefereciaLosa_ in Lista_RefereciaRoom)
            {
                #region factorizar bordes de room
                //genera un objeto para unir los bordes de room similares
                Document _doc = NH_RefereciaLosa_.RefereciaRoomDatos.Room1.Document;
                IList<IList<BoundarySegment>> boundaries = NH_RefereciaLosa_.RefereciaRoomDatos.Room1.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012
                if (0 < boundaries.Count)
                {
                    //selcciona la losa que contiene el room
                    Floor floorSeleccionada = RoomFuncionesSeleccionarLosa.ObtenerFloor_ConUNRoom(NH_RefereciaLosa_.RefereciaRoomDatos.Room1);
                    if (floorSeleccionada == null) continue;

                    NH_RefereciaLosa_.ListaVerticesPoligonoLosa.Clear();
                    foreach (IList<BoundarySegment> b in boundaries) // 2012
                    {
                        BoundarySegmentRoomsGeom newlistaBS = new BoundarySegmentRoomsGeom(b, _uiapp, NH_RefereciaLosa_.RefereciaRoomDatos.Room1.Name, null);
                        //reorden los lados del poligonos para juntos los similes, y regenera los puntos del room con los bordes mejroada

                        NH_RefereciaLosa_.ListaVerticesPoligonoLosa.AddRange(newlistaBS.M1_Reordendar());
                        NH_RefereciaLosa_.ListBoundarySegmentRoomsGeom.Add(newlistaBS);

                    }
                }
                #endregion



            }
        }

        private void M1_2_GenerarMatrizPtoBarraSUple()
        {

            foreach (ReferenciaRoom NH_RefereciaLosa_ in Lista_RefereciaRoom)
            {
                // metodos que enfuncionde de sus vertices y el angulo de losa debe general matriz de ptos para generar barras inferior
                //generar NH_RefereciaLosa_.ListaPtos_Barra_inferior
                NH_RefereciaLosa_.GenerarMatrizPtosParaBarras();
                NH_RefereciaLosa_.GenerarMatrizPtosParaSuples();
            }

        }

        private void M1_3_GetTipoEmpotramiento()
        {
            List<NH_RefereciaCrearSuple> ListasSuples_Todos = new List<NH_RefereciaCrearSuple>();

            foreach (ReferenciaRoom NH_RefereciaLosa_ in Lista_RefereciaRoom)
            {
                ListasSuples_Todos.AddRange(NH_RefereciaLosa_.ListaSuplesVerticalLosa);
                ListasSuples_Todos.AddRange(NH_RefereciaLosa_.ListaSuplesHorizontalLosa);
            }

            //asigna el tipo de de empotramiento para room
            M1_3_1_GetTipoEmpotramiento(ListasSuples_Todos);

        }

        /// <summary>
        /// asigna el tipo de de empotramiento para room
        /// </summary>
        /// <param name="ListasSuples_Todos"></param>
        private void M1_3_1_GetTipoEmpotramiento(List<NH_RefereciaCrearSuple> ListasSuples_Todos)
        {
            //asignar tipo de losas (tipo de empotramiento)
            foreach (ReferenciaRoom NH_RefereciaLosa_ in Lista_RefereciaRoom)
            {
                //-determinar tipo de losas
                var aux = ListasSuples_Todos.Where(c => c.nombreLosa1 == NH_RefereciaLosa_.RefereciaRoomDatos.nombreLosa_1).Select(m => m.UbicacionEnLosa).Distinct().ToList();
                if (aux.Count == 0)
                {
                    NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 1;
                }
                else if (aux.Count == 1)
                {
                    if ((aux[0] == UbicacionLosa.Derecha) || (aux[0] == UbicacionLosa.Izquierda))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 3; }
                    else if ((aux[0] == UbicacionLosa.Superior) || (aux[0] == UbicacionLosa.Inferior))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 2; }
                }
                else if (aux.Count == 2)
                {

                    if (aux.Contains(UbicacionLosa.Derecha) && aux.Contains(UbicacionLosa.Izquierda))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 5; }
                    else if (aux.Contains(UbicacionLosa.Superior) && aux.Contains(UbicacionLosa.Inferior))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 4; }
                    else
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 6; }
                }
                else if (aux.Count == 3)
                {
                    if (aux.Contains(UbicacionLosa.Derecha) && aux.Contains(UbicacionLosa.Izquierda))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 8; }
                    else if (aux.Contains(UbicacionLosa.Superior) && aux.Contains(UbicacionLosa.Inferior))
                    { NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 7; }
                }
                else if (aux.Count == 4)
                {
                    NH_RefereciaLosa_.RefereciaRoomDatos.tipoEmpotramiento = 9;
                }



            }
        }

        #endregion


        #endregion
    }



}
