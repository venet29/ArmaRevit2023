using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.IO;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.AnalisisRoom.Json;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    [System.Runtime.InteropServices.Guid("2AED5D2F-9F96-4F19-A8F2-AFF52BAB734D")]



    /// objeto creado para representar los room analizados
    public class ReferenciaRoom
    {
        #region 0)Propiedades



        /// <summary>
        /// nose para que es esto
        /// </summary>
        const double despla = 35 / 30.48;





        public List<XYZ> ListaPtoLineaDireccionLosa { get; set; }
        public List<XYZ> ListaPtoLineaDireccionPerpLosa { get; set; }

        public List<NH_RefereciaCrearSuple> ListaSuplesVerticalLosa { get; set; }
        public List<NH_RefereciaCrearSuple> ListaSuplesHorizontalLosa { get; set; }


        // lista con puntos finales para dibujare barras --> 
        public List<NH_RefereciaCrearBarra> ListaPtos_Vertical_Barra { get; set; }



        public List<NH_RefereciaCrearBarra> ListaPtos_Horizontal_Barra { get; set; }

        //generar la matriz para dibujar los puntos inciales y final para dibuja con circulo
        //solo para visualizar -- no se ocupa enla logica del negocio        
        public List<NH_RefereciaCrearBarra> ListaPtos_Barra_inferior { get; set; }
        // genera objeto para crear objeto 'NH_RefereciaLosaParaBarra'
        public List<NH_RefereciaCrearBarra> ListaPtos_Barra_inferior_H { get; set; }
        // genera objeto para crear objeto 'NH_RefereciaLosaParaBarra'
        public List<NH_RefereciaCrearBarra> ListaPtos_Barra_inferior_V { get; set; }

        //lista de los puntos a 0.3 del inicio de segmento de room, almacena tantos puntos como segmentos del room
        //public List<NH_RefereciaCrearSuple> ListaPtos_Suple_superior { get; set; }


        //public List<NH_RefereciaCrearSuple> ListaPtos_Vertical_Suple { get; set; }
        // public List<NH_RefereciaCrearSuple> ListaPtos_Horizontal_Suple { get; set; }



        /// <summary>
        /// lista con vertices del room
        /// </summary>
        public List<XYZ> ListaVerticesPoligonoLosa { get; set; }

        //obtiene los ptos offset hacia el centro en x pulgada
        public List<XYZ> ListaVerticesPoligonoLosaOffset { get; set; }

        /// <summary>
        /// Guarda los bordes del poligono de losa en lineas
        /// </summary>
        public List<Line> ListasLineBordesRoom { get; set; }

        public List<Line> ListasLineBordesRoomOffset { get; set; }

        /// CONTIENE LOS BoundarySegmentNew, que contiene BoundarySegment (segmentos de room) con varias porpiedades mas
        public List<BoundarySegmentRoomsGeom> ListBoundarySegmentRoomsGeom { get; set; }

        private ReferenciaRoomDatos refereciaRoomDatos;
        public ReferenciaRoomDatos RefereciaRoomDatos
        {
            get
            {
                return refereciaRoomDatos;
            }

            set
            {
                refereciaRoomDatos = value;
            }
        }

        public bool IsOk { get; private set; }



        #endregion

        #region 1) contructor

        public ReferenciaRoom(RefereciaRoom_json obj, Room room, Element PelotaLosa)
        {



            RefereciaRoomInicial();
            refereciaRoomDatos = new ReferenciaRoomDatos(obj,room, PelotaLosa);


            foreach (var item in obj.ListaPtoLineaDireccionLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaPtoLineaDireccionLosa.Add(new XYZ(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1]), 0));
            }
            foreach (var item in obj.ListaPtoLineaDireccionPerpLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaPtoLineaDireccionPerpLosa.Add(new XYZ(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1]), 0));
            }
            foreach (var item in obj.ListaVerticesPoligonoLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaVerticesPoligonoLosa.Add(new XYZ(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1]), 0));
            }
            //******************************************
            //foreach (var item in obj.Lista_Suples_Todos)
            //{
            //    ListaSuplesVerticalLosa.Add(new PtosCrearSuples(item));
            //}
            foreach (var item in obj.ListaSuplesVerticalLosa)
            {
                ListaSuplesVerticalLosa.Add(new NH_RefereciaCrearSuple(item) { PelotaLosa = this.refereciaRoomDatos.PelotaLosa });
            }

            foreach (var item in obj.ListaSuplesHorizontalLosa)
            {
                ListaSuplesHorizontalLosa.Add(new NH_RefereciaCrearSuple(item) { PelotaLosa = this.refereciaRoomDatos.PelotaLosa });
            }


            foreach (var item in obj.ListaPtos_Vertical_Barra)
            {
                ListaPtos_Vertical_Barra.Add(new NH_RefereciaCrearBarra(item, refereciaRoomDatos.posicionPelota.Z) { PelotaLosa = this.refereciaRoomDatos.PelotaLosa });
            }
            foreach (var item in obj.ListaPtos_Horizontal_Barra)
            {
                ListaPtos_Horizontal_Barra.Add(new NH_RefereciaCrearBarra(item, refereciaRoomDatos.posicionPelota.Z) { PelotaLosa = this.refereciaRoomDatos.PelotaLosa });
            }



        }
        
        
        public ReferenciaRoom(Document _doc, Room room)
        {
            refereciaRoomDatos = new ReferenciaRoomDatos(_doc, room);
            RefereciaRoomInicial();
            List<RoomTag> tag= TiposRoomTagEnView.M1_GetFamilySymbol_nh(room.Id, room.Document, room.Document.ActiveView.Id);

            XYZ posicionRoom = RoomFunciones.ptoLocationRoom(room);
            if (tag.Count==1)
                RefereciaRoomDatos.posicionPelota = tag[0].TagHeadPosition.AsignarZ(posicionRoom.Z);
            else
                RefereciaRoomDatos.posicionPelota = posicionRoom;

            RefereciaRoomDatos.posicionCruzRoom = posicionRoom;

          IsOk=  RefereciaRoomDatos.GetParametrosUnRoom();
        }

        private void RefereciaRoomInicial()
        {
           
            ListaPtoLineaDireccionLosa = new List<XYZ>();
            ListaPtoLineaDireccionPerpLosa = new List<XYZ>();

            ListaSuplesVerticalLosa = new List<NH_RefereciaCrearSuple>();
            ListaSuplesHorizontalLosa = new List<NH_RefereciaCrearSuple>();

            ListaPtos_Vertical_Barra = new List<NH_RefereciaCrearBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaCrearBarra>();

            ListaPtos_Barra_inferior = new List<NH_RefereciaCrearBarra>();
            ListaPtos_Barra_inferior_H = new List<NH_RefereciaCrearBarra>();
            ListaPtos_Barra_inferior_V = new List<NH_RefereciaCrearBarra>();

            //ListaPtos_Suple_superior = new List<NH_RefereciaCrearSuple>();
            //ListaPtos_Vertical_Suple = new List<NH_RefereciaCrearSuple>();
            //ListaPtos_Horizontal_Suple = new List<NH_RefereciaCrearSuple>();

            ListaVerticesPoligonoLosa = new List<XYZ>();
            ListaVerticesPoligonoLosaOffset = new List<XYZ>();

            ListBoundarySegmentRoomsGeom = new List<BoundarySegmentRoomsGeom>();

            ListasLineBordesRoom = new List<Line>();
            ListasLineBordesRoomOffset = new List<Line>();
        }
    
        #endregion

        #region 2)Metodos


        /// <summary>
        /// Genera matriz con punto para dibujar barras inferior
        /// </summary>
        internal void GenerarMatrizPtosParaBarras()
        {
            #region Crear Parametros para trasformacion
            // utliza la posicon y angulo pelota losa

            Transform trans1 = null;
            Transform Invertrans1 = null;
            Transform trans2_rotacion = null;
            Transform InverTrans2_rotacion = null;

            trans1 = Transform.CreateTranslation(new XYZ(-refereciaRoomDatos.posicionPelota.X, -refereciaRoomDatos.posicionPelota.Y, -refereciaRoomDatos.posicionPelota.Z));

            if (refereciaRoomDatos.anguloPelotaLosaGrado_1 > 90)
                trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(refereciaRoomDatos.anguloPelotaLosaGrado_1 - 180), XYZ.Zero);
            else
                trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(refereciaRoomDatos.anguloPelotaLosaGrado_1), XYZ.Zero);

            //trasformacion inversa
            Invertrans1 = trans1.Inverse;
            InverTrans2_rotacion = trans2_rotacion.Inverse;
            #endregion
            //

            //guarda solo los que esten separado cierta distancia
            List<double> listaValoresX_inicial = new List<double>();
            List<double> listaValoresX = new List<double>();
            List<double> listaValoresX_todos = new List<double>();
            //guarda solo los que esten separado cierta distancia
            List<double> listaValoresY_inicial = new List<double>();
            List<double> listaValoresY = new List<double>();
            List<double> listaValoresY_todos = new List<double>();

            //creo lista de putos de vertices trasformados en funcion del angulo de pelota de losa
            List<XYZ> ListaVerticesPoligonoLosa_transforma = new List<XYZ>();

            foreach (XYZ pto in ListaVerticesPoligonoLosa)
            {
                ListaVerticesPoligonoLosa_transforma.Add(trans2_rotacion.OfPoint(trans1.OfPoint(pto)));
            }

            //direccion x de los puntos
            listaValoresX_inicial = ListaVerticesPoligonoLosa_transforma.OrderBy(p => p.X).Select(q => Math.Round(q.X, 4)).Distinct().ToList();
            // filtra la lista para obtener solo los que estan separado uno respecto al otro una distancia 'despla'
            listaValoresX = GenerarMatrizPtosParaBarras_ListaFiltrada(listaValoresX_inicial);



            // direccion y de los ptos
            listaValoresY_inicial = ListaVerticesPoligonoLosa_transforma.OrderBy(p => p.Y).Select(q => Math.Round(q.Y, 4)).Distinct().ToList();
            // filtra la lista para obtener solo los que estan separado uno respecto al otro una distancia 'despla'
            listaValoresY = GenerarMatrizPtosParaBarras_ListaFiltrada(listaValoresY_inicial);


            #region a) Pto central del 'roomseparation'

            // define que pto guarda    true= central,   false= extremos
            bool ptoAconsiderar = true;


            if (ptoAconsiderar)
            {


                // en al direccx
                for (int i = 0; i <= listaValoresX.Count - 2; i++)
                {
                    for (int j = 0; j <= listaValoresY_inicial.Count - 2; j++)
                    {


                        XYZ aux_xyz = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ((listaValoresX[i] + listaValoresX[i + 1]) / 2,
                                                                                               (listaValoresY_inicial[j] + listaValoresY_inicial[j + 1]) / 2,
                                                                                                0)));

                        NH_RefereciaCrearBarra auxUnico = new NH_RefereciaCrearBarra()
                        { nombreLosa = refereciaRoomDatos.Room1.Name,
                            PosicionPto_Barra = aux_xyz,
                            LargoRecorridoX = Math.Abs(listaValoresX[i] - listaValoresX[i + 1]),
                            LargoRecorridoY = Math.Abs(listaValoresY_inicial[j] - listaValoresY_inicial[j + 1])
                        };
                        if (refereciaRoomDatos.Room1.IsPointInRoom(aux_xyz)) ListaPtos_Barra_inferior_H.Add(auxUnico);
                    }
                }


                // en al direccy
                for (int i = 0; i <= listaValoresX_inicial.Count - 2; i++)
                {
                    for (int j = 0; j <= listaValoresY.Count - 2; j++)
                    {


                        XYZ aux_xyz = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ((listaValoresX_inicial[i] + listaValoresX_inicial[i + 1]) / 2,
                                                                                               (listaValoresY[j] + listaValoresY[j + 1]) / 2,
                                                                                                0)));

                        NH_RefereciaCrearBarra auxUnico = new NH_RefereciaCrearBarra()
                        {
                            PosicionPto_Barra = aux_xyz,
                            LargoRecorridoX = Math.Abs(listaValoresX_inicial[i] - listaValoresX_inicial[i + 1]),
                            LargoRecorridoY = Math.Abs(listaValoresY[j] - listaValoresY[j + 1])
                        };
                        if (refereciaRoomDatos.Room1.IsPointInRoom(aux_xyz)) ListaPtos_Barra_inferior_V.Add(auxUnico);
                    }
                }


            }

            #endregion


            #region b) pto inicial y pto final


            // asigna los ptos final y inicial los las lineas 'roomseparation'
            if (!ptoAconsiderar)
            {
                for (int i = 0; i <= listaValoresX.Count - 1; i++)
                {
                    for (int j = 0; j <= listaValoresY.Count - 1; j++)
                    {


                        XYZ aux_xyz = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ(listaValoresX[i], listaValoresY[j], 0)));
                        NH_RefereciaCrearBarra auxUnico = new NH_RefereciaCrearBarra()
                        {
                            PosicionPto_Barra = aux_xyz,
                            LargoRecorridoX = Math.Abs(listaValoresX[i] - listaValoresX[i]),
                            LargoRecorridoY = Math.Abs(listaValoresY[j] - listaValoresY[j])
                        };
                        if (refereciaRoomDatos.Room1.IsPointInRoom(aux_xyz)) ListaPtos_Barra_inferior.Add(auxUnico);
                    }
                }
            }


            #endregion




        }

        /// <summary>
        /// Genera putos para obtener los puntos asuples
        /// analiza cada segmento de room y obtiene un pto en cierta posicion (30% del pto de inico)
        /// </summary>
        internal void GenerarMatrizPtosParaSuples()
        {



            foreach (BoundarySegmentRoomsGeom boundarySegmentRoomsGeom in ListBoundarySegmentRoomsGeom)
            {
                foreach (WrapperBoundarySegment bsegmente in boundarySegmentRoomsGeom.ListaWrapperBoundarySegment)
                {

                    //TipoOrientacionBorde tipoOrienta;
                    UbicacionLosa tipoUbicacion;

                    if (!bsegmente.CrearPtosSuple()) continue;


                    XYZ pto = (bsegmente.coordenadasBorde.StartPoint + bsegmente.coordenadasBorde.EndPoint) / 2;

                    var AnguloEntrePtoSupleYPelotaLosa = Util.angulo_entre_pt_Rad_XY0(refereciaRoomDatos.posicionPelota, pto);

                    //if (bsegmente.IsValid == false) continue;
                    // bsegmente.angulo entre  0 y 90
                    if (Math.Abs(refereciaRoomDatos.anguloPelotaLosaGrado_1 - bsegmente.anguloGrado) < 0.1)
                    {
                        //tipoOrienta = TipoOrientacionBorde.Horizontal;
                        var sin_Aux = Math.Sin(AnguloEntrePtoSupleYPelotaLosa);

                        if (sin_Aux > 0)
                            tipoUbicacion = UbicacionLosa.Superior;
                        else
                            tipoUbicacion = UbicacionLosa.Inferior;



                    }
                    else
                    {
                        var cos_Aux = Math.Cos(AnguloEntrePtoSupleYPelotaLosa);
                        // tipoOrienta = TipoOrientacionBorde.Vertical;
                        if (cos_Aux > 0)
                            tipoUbicacion = UbicacionLosa.Derecha;
                        else
                            tipoUbicacion = UbicacionLosa.Izquierda;
                    }


                    Parameter room1 = ParameterUtil.FindParaByName(bsegmente.rom1_crearSuple, "Numero Losa");
                    Parameter room2 = ParameterUtil.FindParaByName(bsegmente.rom2_crearSuple, "Numero Losa");

                    //obs1)
                    NH_RefereciaCrearSuple newSuple = new NH_RefereciaCrearSuple()
                    {
                        nombreLosa1 = ParameterUtil.FindParaByName(bsegmente.rom1_crearSuple, "Numero Losa").AsString(),
                        nombreLosa2 = ParameterUtil.FindParaByName(bsegmente.rom2_crearSuple, "Numero Losa").AsString(),
                        
                        PosicionPtoSupleFinal = bsegmente.coordenadasBorde.EndPointSuples,
                        PosicionPtoSupleInicial = bsegmente.coordenadasBorde.StartPointSuples,
                        UbicacionEnLosa = tipoUbicacion,
                        PosicionPto_suple = pto,
                        diametro = 8,
                        espaciamiento = 20,
                        cuantia = "@8a20"


                    };

                    if (Math.Abs(refereciaRoomDatos.anguloPelotaLosaGrado_1 - bsegmente.anguloGrado) < 0.1)
                    {
                        ListaSuplesVerticalLosa.Add(newSuple);
                    }
                    else
                    {
                        ListaSuplesHorizontalLosa.Add(newSuple);
                    }


                }
            }
        }


        /// <summary>
        /// funcion auxialiar que   --> filtra la lista para obtener solo los que estan separado uno respecto al otro una distancia 'despla'
        /// </summary>
        /// <param name="listaValoresX_aux"></param>
        /// <returns></returns>
        private List<double> GenerarMatrizPtosParaBarras_ListaFiltrada(List<double> listaValoresX_aux)
        {
            List<double> listaValoresX = new List<double>();
            //pto referencia auxiliar del pto inicial

            //a) agerga el primer valor
            listaValoresX.Add(Math.Round(listaValoresX_aux[0], 6));


            //b)agregar datos intermedios
            Double pto_inicial = Math.Round(listaValoresX_aux[1], 6);
            for (int i = 2; i < listaValoresX_aux.Count - 1; i++)
            {
                double valorx = Math.Round(listaValoresX_aux[i], 6);
                //buisca si puntu inicial tiene una diustancia con respecto al sigueinte pto mayor que despla, agergar
                //y cambia punto inicial 
                double resulBUacado = Math.Abs(pto_inicial - valorx);
                if (Math.Abs(pto_inicial - valorx) > despla)
                {
                    if (listaValoresX.Contains(pto_inicial) == false)
                    {
                        listaValoresX.Add(pto_inicial);

                    }
                    pto_inicial = valorx;

                }
                else if (Math.Abs(pto_inicial - Math.Round(listaValoresX[listaValoresX.Count - 1], 6)) > despla)
                {
                    //nif (listaValoresX.Contains(pto_inicial) == false) listaValoresX.Add(pto_inicial);
                    pto_inicial = Math.Round(listaValoresX[listaValoresX.Count - 1], 6);

                }
                else
                {
                    if (pto_inicial != listaValoresX[listaValoresX.Count - 1])
                        pto_inicial = Math.Round(listaValoresX[listaValoresX.Count - 1]);

                    // listaValoresX.RemoveAt(listaValoresX.Count - 1);
                    // i += 1;
                }
            }



            //agrega ultimo guardad                               Y  SI LA DISTANCIA entre el ultimo y penultimo valor es mayor 'deslpa'
            if ((listaValoresX.Contains(pto_inicial) == false) && (Math.Abs(pto_inicial - Math.Round(listaValoresX_aux[listaValoresX_aux.Count - 1], 6)) > despla))
                listaValoresX.Add(pto_inicial);
            // c)agrega el dato finalnhsnhsnhs
            listaValoresX.Add(Math.Round(listaValoresX_aux[listaValoresX_aux.Count - 1], 6));


            //modifica segundo item de la posicion
            if (Math.Abs(Math.Round(listaValoresX[1], 6) - Math.Round(listaValoresX[0], 6)) < despla)
            {
                listaValoresX.RemoveAt(1);
            }



            return listaValoresX;

        }

        /// <summary>
        /// Obtiene los vertices de Room y los borde de room
        /// </summary>
        public void GetVerticesYBordesDeRoom()
        {
            // obtienen los vertices del poligono
            ListaVerticesPoligonoLosa = RoomFuncionesPuntos.ListRoomVertice(refereciaRoomDatos.Room1);
            //obtiene los bordes del room
            ListasLineBordesRoom = RoomFuncionesLine.GetBoundaryToLine(refereciaRoomDatos.Room1);
            refereciaRoomDatos.posicionPelota = RoomFuncionesPuntos.posicionRoom;
        }



        /// <summary>
        /// obtiene un poligono de losa con un offset respecto a al perimertro del room
        /// si valor es posivo el poligono se hace mas chico
        /// si es negativo el poligono es mas grande
        /// </summary>
        public void GetListasLineBordesRoomOffset(List<XYZ> ListaVerticesPoligonoLosaOffset_)
        {
            //obtiene los ptos offset hacia el centro en x pulgada
            this.ListaVerticesPoligonoLosaOffset = ListaVerticesPoligonoLosaOffset_;
            ListasLineBordesRoomOffset = RoomFuncionesLine.GetListaToLine(ListaVerticesPoligonoLosaOffset);

        }

        #endregion

    }

}
