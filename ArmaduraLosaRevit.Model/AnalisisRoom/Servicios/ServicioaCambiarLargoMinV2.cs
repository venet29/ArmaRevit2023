using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Servicios
{
    public class ServicioaCambiarLargoMinV2
    {
        #region 0)propiedad
        ExternalCommandData _commandData;
        private UIApplication _uiapp;
        UIDocument _uidoc;
        Document _doc;
        //estilo textonote
        static Element red_secundaria_line_styles;
        static Element verde_primaria_line_styles;

        //lista RoomObj seleccionados
        List<RoomsObj> listaRoomsObj;
        public RoomSeleccionar roomSeleccionar;
        string id4Elementos = "";
        double nivelz;
        #endregion

        #region 1) constructor

        public ServicioaCambiarLargoMinV2(ExternalCommandData commandData)
        {
            this._commandData = commandData;
            this._uiapp =_commandData.Application;
            this._uidoc = commandData.Application.ActiveUIDocument;
            this._doc = commandData.Application.ActiveUIDocument.Document;
            this.roomSeleccionar = new RoomSeleccionar();

            listaRoomsObj = new List<RoomsObj>();
            //*obtenerestilos textos
            ServicioaCambiarLargoMin_ObtieneEstilosTextos();

        }
        public ServicioaCambiarLargoMinV2(Document doc_)
        {

            _doc = doc_;
            roomSeleccionar = new RoomSeleccionar();

            listaRoomsObj = new List<RoomsObj>();
            //*obtenerestilos textos
            ServicioaCambiarLargoMin_ObtieneEstilosTextos();

        }

        private void ServicioaCambiarLargoMin_ObtieneEstilosTextos()
        {
            if ((red_secundaria_line_styles != null) && (verde_primaria_line_styles != null)) return;

            red_secundaria_line_styles = TiposLineaPattern.ObtenerTipoLinea("RojaSecundario", _doc);
            verde_primaria_line_styles = TiposLineaPattern.ObtenerTipoLinea("VerdePrimaria", _doc);
            //obtener linestyle 'ROJO'
            //FilteredElementCollector graphic_styles = new FilteredElementCollector(_doc).OfClass(typeof(GraphicsStyle));
            //if (red_line_styles == null)
            //    red_line_styles = graphic_styles.Where<Element>(e => e.Name.ToString() == "RojaSecundario").ToList();
            //if (verde_line_styles == null)
            //    verde_line_styles = graphic_styles.Where<Element>(e => e.Name.ToString() == "VerdePrimaria").ToList();//"LINEA VACIOS"
        }

        #endregion

        #region 2) metodos

        /// <summary>
        /// obtienen los estilos de los textnote
        /// </summary>

        #region dIBUJAR LINEA Y TEXTO DE LARGO MINIMO

        public void DibujarLargosMin(IEnumerable<Element> ListaRooms)
        {

            //  Util.ErrorMsg("solo prueba");

            List<RoomsObj> listaRoomsObj = new List<RoomsObj>();

            foreach (Room room in ListaRooms)
            {
                id4Elementos = "";
                nivelz = (room.Location as LocationPoint).Point.Z;
                //double zz = (room.Level.Location as LocationPoint).Point.Z;
                //  obtiene roomObjnhsnhsnhsnhs
                RoomsObj objRoom = ObtenerRoomObj(room);
                if (!objRoom.Isok) continue;
                listaRoomsObj.Add(objRoom);
                //1) dibuja las lineas


            }

            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("DibujarLargosMin-NH");

                    foreach (RoomsObj objRoom in listaRoomsObj)
                    {
                        id4Elementos = "";
                        CrearLineas(objRoom);
                        //2)crea el texto del largo con cada linea
                        CrearTextoLineas(objRoom.ListaInterHorizontal);
                        CrearTextoLineas(objRoom.ListaInterLargoVertical);

                        //objRoom.ActualizarLargoMin();
                        //3)agergar oarametro internos para poder borrarlos posteriormente
                        ModificaParametroElementoBorrarYActualizarLargoMin(objRoom, id4Elementos);
                    }

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

        }
        public void DibujarLargosMin()
        {


            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("DibujarLargosMin-NH");

                    foreach (RoomsObj objRoom in listaRoomsObj)
                    {
                        id4Elementos = "";
                        //nivelz = (room.Location as LocationPoint).Point.Z;
                        //double zz = (room.Level.Location as LocationPoint).Point.Z;
                        //  obtiene roomObjnhsnhsnhsnhs
                        // RoomsObj objRoom = ObtenerRoomObj(room);
                        //1) dibuja las lineas
                        CrearLineas(objRoom);
                        //2)crea el texto del largo con cada linea
                        CrearTextoLineas(objRoom.ListaInterHorizontal);
                        CrearTextoLineas(objRoom.ListaInterLargoVertical);

                        //3)agergar oarametro internos para poder borrarlos posteriormente
                        ModificaParametroElementoBorrar(objRoom.room, id4Elementos);

                    }

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

        }

        private RoomsObj ObtenerRoomObj(Room room)
        {
            //crear referecnia room
            ReferenciaRoom newRegistroLosa = new ReferenciaRoom(_doc,room);
            //obtiene largo minimo
            RoomsObj objRoom = new RoomsObj(newRegistroLosa);
            objRoom.GenerarDatos();
            //  objRoom.GetDireccionDireccionPrincipales();
            return objRoom;
        }

        private void CrearLineas(RoomsObj objRoom)
        {

            Creator creator = new Creator(_doc);
            //crear linea

            //int n1 = graphic_styles.Count<Element>();
            //obtiene el line style con filter

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Dibujar LargoMin-NH");

                    //crea lkna horoizntal
                    ModelCurve mcurveHorizontal = CrearLineas_aux(objRoom.room, objRoom.ListaInterHorizontal, creator);
                    //crea linea vertical
                    ModelCurve mcurveVertical = CrearLineas_aux(objRoom.room, objRoom.ListaInterLargoVertical, creator);

                    //cambia el color
                    if (objRoom.refereciaRoomDatos.direccionHorizontal == "1")
                    {
                        if (verde_primaria_line_styles!=null) mcurveHorizontal.LineStyle = verde_primaria_line_styles;
                        if (red_secundaria_line_styles != null) mcurveVertical.LineStyle = red_secundaria_line_styles;
                    }
                    else
                    {
                        if (verde_primaria_line_styles != null) mcurveVertical.LineStyle = verde_primaria_line_styles;
                        if (red_secundaria_line_styles != null) mcurveHorizontal.LineStyle = red_secundaria_line_styles;
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }



        }

        private ModelCurve CrearLineas_aux(Room room, List<XYZ> ListaXYZ, Creator creator)
        {
            List<List<XYZ>> lista = new List<List<XYZ>>();
            lista.Clear();
            lista.Add(ListaXYZ);
            List<Line> elem = creator.DrawLIne(lista, room.Level);
            ModelCurve mcurvel = creator.CreateModelCurve(elem[0]);

            id4Elementos = (id4Elementos == "" ? mcurvel.Id.ToString() : id4Elementos + "," + mcurvel.Id.ToString());
            return mcurvel;
        }

        private void CrearTextoLineas(List<XYZ> listPto)
        {
            CrearTexNote crearTexNote = new CrearTexNote(_uiapp, "5mm Arial");
            //obtiene pto de ubicacion del texto
            XYZ pto = ObtenerPtoTexto(listPto);
            //obtiene direccion normal
            XYZ ptoNormal = ServicioModificarCoordenadas.ObtenerPtoReferenciaLines(listPto[0], listPto[1], new XYZ(0, 1, 0));
            //angulo linea radianes
            double anguloBarra_ = Util.AnguloEntre2PtosGrado90(listPto[0], listPto[1], EnGrados: false);
            //crear texto
            TextNote txtnote = crearTexNote.M1_CrearConTrans(pto, Math.Round(Util.FootToCm(listPto[0].DistanceTo(listPto[1])), 0).ToString(), anguloBarra_);

            id4Elementos = (id4Elementos == "" ? txtnote.Id.ToString() : id4Elementos + "," + txtnote.Id.ToString());
        }

        private void ModificaParametroElementoBorrar(Room room, string id4Elementos)
        {


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("AgregarParametrosInternosParaBorrar-NH");

                    ParameterUtil.SetParaInt(room, "ElementoBorrar", id4Elementos);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }




        }
        private void ModificaParametroElementoBorrarYActualizarLargoMin(RoomsObj objRoom, string id4Elementos)
        {


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("AgregarParametrosInternosParaBorrar-NH");
                    ParameterUtil.SetParaInt(objRoom.room, "LargoMin", objRoom.largoMIn);
                    ParameterUtil.SetParaInt(objRoom.room, "ElementoBorrar", id4Elementos);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }




        }
        #endregion


        #region bORRAR LARGOS MINIMOS


        public void BorrarLargosMin(IEnumerable<Element> ListaRooms)
        {

            listaRoomsObj.Clear();

            //b) se borra los datos

            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("BorrarLargosMin-NH");

                    foreach (Room room in ListaRooms)
                    {
                        RoomsObj objRoom = ObtenerRoomObj(room);
                        if (!objRoom.Isok) continue;
                        listaRoomsObj.Add(objRoom);
                        objRoom.BorrarLineaTextoLargoMin();
                        ModificaParametroElementoBorrar(room, "");
                    }

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                string msje = ex.Message;
            }


        }


        public void BorrarLargosMinUpdate(Room room)
        {

            listaRoomsObj.Clear();

            //b) se borra los datos

            try
            {
                RoomsObj objRoom = ObtenerRoomObj(room);
                if (!objRoom.Isok) return;
                listaRoomsObj.Add(objRoom);
                objRoom.BorrarLineaTextoLargoMin();
                ModificaParametroElementoBorrar(room, "");
            }
            catch (Exception ex)
            {
                string msje = ex.Message;

            }




        }

        #endregion




        private XYZ ObtenerPtoTexto(List<XYZ> listPto)
        {
            XYZ ptoResil = null;
            XYZ[] list = Util.Ordena2Ptos(listPto[0], listPto[1]);

            double coorX = listPto[0].DistanceTo(listPto[1]) * 0.7;
            double coory = Util.CmToFoot(40);
            ptoResil = ServicioModificarCoordenadas.ObtenerPtoReferenciaLines(listPto[0], listPto[1], new XYZ(coorX, coory, nivelz));

            return ptoResil;
        }



        #endregion
    }
}
