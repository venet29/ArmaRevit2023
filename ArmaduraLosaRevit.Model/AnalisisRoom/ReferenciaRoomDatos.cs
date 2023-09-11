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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{

    /// objeto creado para representar los room analizados
    public class ReferenciaRoomDatos
    {
        #region 0)Propiedades
        public Level level { get; set; }

        private Document _doc;

        const double despla = 35 / 30.48;
        //propiedades generales
        public string nombreLosa_1 { get; set; }
        public string nombreLosa_2 { get; set; }
        public float espesorCM_1 { get; set; }
        public float espesor_2 { get; set; }
        //ubiacon de pelota de losa
        public XYZ posicionPelota { get; set; }
        public XYZ posicionCruzRoom { get; set; }
        public double largomin_1 { get; set; }
        public double largomin_2 { get; set; }
        public float LargoHorizontal { get; set; }
        public float LargoVertical { get; set; }

        public string direccionHorizontal { get; set; }
        public string direccionVertical { get; set; }

        public string cuantiaHorizontal { get; set; }
        public string cuantiaVertical { get; set; }
        public string CuantiaBarra { get; set; }

        public double anguloPelotaLosaGrado_1 { get; set; }
        public double anguloPelotaLosaGrado_2 { get; set; }

        public double anguloPelotaLosaRad_1 { get; set; }
        public double anguloPelotaLosaRad_2 { get; set; }

        public double anguloBarraLosaGrado_1 { get; set; }
        public double anguloBarraLosaGrado_2 { get; set; }

        public int tipoEmpotramiento { get; set; }


        public Room Room1 { get; set; }

       

        public Room Room2 { get; set; }
        public Floor Losa { get; set; }
        public Element PelotaLosa { get; set; }

        public List<XYZ> ListaPtoLineaDireccionLosa { get; set; }
        public List<XYZ> ListaPtoLineaDireccionPerpLosa { get; set; }

        public XYZ PtoSeleccionMouse1 { get; set; }
        public XYZ PtoSeleccionMouse2 { get; set; }

        public bool IsLuzSecuandiria { get; private set; }


        public int diametro { get; private set; }
        public double Espaciamiento { get;  set; }
        public int DiametroOrientacionPrincipal { get;  set; }

        public UbicacionLosa _ubicacionBarraEnlosa { get; set; }


        private SeleccionarLosaConMouse _seleccionarLosaConMouse;

        #endregion

        #region 1) contructor

        public ReferenciaRoomDatos(RefereciaRoom_json obj, Room room, Element PelotaLosa)
        {
            this.LargoHorizontal = obj.LargoHorizontal;
            this.LargoVertical = obj.LargoVertical;
            this.nombreLosa_1 = obj.nombreLosa;
            this.posicionPelota = obj.posicionPelota;
            this.espesorCM_1 = obj.espesor;
            this.direccionHorizontal = obj.direccionHorizontal;
            this.direccionVertical = obj.direccionVertical;
            this.cuantiaHorizontal = obj.cuantiaHorizontal;
            this.cuantiaVertical = obj.cuantiaVertical;
            this.PelotaLosa = PelotaLosa;//
            this.Room1 = room;
            anguloPelotaLosaGrado_1 = obj.anguloPelotaLosa;
            //this.anguloPelotaLosa = obj.anguloPelotaLosa;
            this._doc = room.Document;
            this.tipoEmpotramiento = obj.tipoEmpotramiento;
        }


        public ReferenciaRoomDatos(SeleccionarLosaBarraRoom seleccionarLosaBarraRoom, SolicitudBarraDTO solicitudDTO)
        {

            this._doc = (seleccionarLosaBarraRoom.RoomSelecionado1 != null ? seleccionarLosaBarraRoom.RoomSelecionado1.Document : solicitudDTO.UIdoc.Document);
            this.Room1 = seleccionarLosaBarraRoom.RoomSelecionado1;//this.anguloPelotaLosa = obj.anguloPelotaLosa;
            this.Room2 = seleccionarLosaBarraRoom.RoomSelecionado2;//this.anguloPelotaLosa = obj.anguloPelotaLosa;
            this.Losa = seleccionarLosaBarraRoom.LosaSeleccionada1;
            this.PtoSeleccionMouse1 = seleccionarLosaBarraRoom.PtoConMouseEnlosa1;
            this.PtoSeleccionMouse2 = seleccionarLosaBarraRoom.PtoConMouseEnlosa2;
            this._ubicacionBarraEnlosa = solicitudDTO.UbicacionEnlosa;


        }

        public ReferenciaRoomDatos(Document _doc, Room room)
        {
            this._doc = _doc;
            this.Room1 = room;//this.anguloPelotaLosa = obj.anguloPelotaLosa;
        }

        public ReferenciaRoomDatos() { }

        #endregion

        #region 2)Metodos

        //2) metodos 
        /// <summary>
        /// Obtiene los parameros de Room y se los agrega a  NH_RefereciaLosa
        /// </summary>
        /// <param name="doc"></param>
        public bool GetParametrosUnRoom()
        {
            if (Room1 == null) return false;
            if (_doc == null) return false;
            try
            {


                M1_ObtenerParametrosInternosUNRoom();
                //
                M2_ObtenerAnguloDireccionBarra();

                M3_ObtenerDireccionPrincipal();

                M4_ObternerParametrosCuantias();

                M5_ObternerDiametroPrinciapal();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public bool GetParametrosDOsRoom()
        {

            if (Room1 == null) return false;
            if (Room2 == null) return false;
            if (_doc == null) return false;

            try
            {

                ObtenerParametrosInternosDOSRoom();
                //
                M2_ObtenerAnguloDireccionBarra();

                M3_ObtenerDireccionPrincipal();

                M4_ObternerParametrosCuantias();

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool GetParametrosSINRoom()
        {

            if (Room1 == null) return false;
            if (Room2 == null) return false;
            if (_doc == null) return false;

            try
            {

               // ObtenerParametrosInternosDOSRoom();
                //
                M2_ObtenerAnguloDireccionBarra();

                M3_ObtenerDireccionPrincipal();

                M4_ObternerParametrosCuantias();

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool M4_ObternerParametrosCuantias()
        {
            try
            {

                anguloBarraLosaGrado_1 = 0;
                IsLuzSecuandiria = false;
                if (_ubicacionBarraEnlosa == UbicacionLosa.Derecha || _ubicacionBarraEnlosa == UbicacionLosa.Izquierda)
                {

                    if (direccionHorizontal == "2") IsLuzSecuandiria = true;

                    CuantiaBarra = cuantiaHorizontal;
                    anguloBarraLosaGrado_1 = anguloPelotaLosaGrado_1;
                    //nada;
                }
                else
                {
                    if (direccionVertical == "2") IsLuzSecuandiria = true;
                    CuantiaBarra = cuantiaVertical;
                    anguloBarraLosaGrado_1 = anguloPelotaLosaGrado_1 + 90;
                }

                string[] auxcuantia = CuantiaBarra.Split('a');

                int _diametro = 0;
                bool resulDiametro = int.TryParse(auxcuantia[0], out _diametro);
                if (!resulDiametro) TaskDialog.Show("Error datos Losa", "Losa sin Diametro o mal definido");
                diametro = _diametro;

                double _espaciamiento = 0;
                bool resulEspaciemientoo = double.TryParse(auxcuantia[1], out _espaciamiento);
                if (!resulEspaciemientoo) TaskDialog.Show("Error datos Losa", "Losa sin espaciamiento o mal definido");
                Espaciamiento = Util.CmToFoot(_espaciamiento);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool M5_ObternerDiametroPrinciapal()
        {
            try
            {
                DiametroOrientacionPrincipal = 0;
                string cuantiadPrimario = "";
                if (direccionHorizontal == "1")
                    cuantiadPrimario = cuantiaHorizontal;
                else
                    cuantiadPrimario = cuantiaVertical;


                string[] auxcuantia = cuantiadPrimario.Split('a');

                int _diametro = 0;
                bool resulDiametro = int.TryParse(auxcuantia[0], out _diametro);
                if (!resulDiametro) TaskDialog.Show("Error datos Losa", "Losa sin Diametro o mal definido");
                DiametroOrientacionPrincipal = _diametro;
            }
            catch (Exception)
            {
                DiametroOrientacionPrincipal = 0;
                return false;
            }
            return true;
        }


        public bool M1_ObtenerParametrosInternosUNRoom()
        {
            try
            {
                Element elem = (Element)Room1;
                ParameterSet paras = elem.Parameters;
                foreach (Parameter para in paras)
                {
                    switch (para.Definition.Name)
                    {
                        case "Angulo":
                            anguloPelotaLosaRad_1 = para.AsDouble();
                            anguloPelotaLosaGrado_1 = Util.RadianeToGrados(anguloPelotaLosaRad_1);
                            break;
                        case "Cuantia Vertical":
                            cuantiaVertical = para.AsString();
                            break;
                        case "Direccion Horizontal":
                            direccionHorizontal = para.AsString();
                            break;
                        case "Direccion Vertical":
                            direccionVertical = para.AsString();
                            break;
                        case "Cuantia Horizontal":
                            cuantiaHorizontal = para.AsString();
                            break;
                        case "Level":
                            if (para.AsElementId().ToString() != "-1") level = _doc.GetElement(para.AsElementId()) as Level;
                            break;
                        case "Espesor":
                            if ((float)para.AsDouble() > 0)
                            { espesorCM_1 = (float)para.AsDouble(); }

                            break;
                        case "Numero Losa":
                            nombreLosa_1 = para.AsString();
                            break;
                        case "LargoMin":
                            largomin_1 = (float)para.AsDouble();
                            break;
                        case "Number":

                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool ObtenerParametrosInternosDOSRoom()
        {
            try
            {

                M1_ObtenerParametrosInternosUNRoom();

                Element elem = (Element)Room2;
                ParameterSet paras = elem.Parameters;
                foreach (Parameter para in paras)
                {
                    switch (para.Definition.Name)
                    {
                        case "Angulo":
                            anguloPelotaLosaRad_2 = para.AsDouble();
                            anguloPelotaLosaGrado_2 = Util.RadianeToGrados(anguloPelotaLosaRad_2);
                            break;
                        case "Cuantia Vertical":
                            // cuantiaVertical = para.AsString();
                            break;
                        case "Direccion Horizontal":
                            //  direccionHorizontal = para.AsString();
                            break;
                        case "Direccion Vertical":
                            //  direccionVertical = para.AsString();
                            break;
                        case "Cuantia Horizontal":
                            //  cuantiaHorizontal = para.AsString();
                            break;
                        case "Level":
                            if (para.AsElementId().ToString() != "-1") level = _doc.GetElement(para.AsElementId()) as Level;
                            break;
                        case "Espesor":
                            espesor_2 = (float)para.AsDouble();
                            break;
                        case "Numero Losa":
                            nombreLosa_2 = para.AsString();
                            break;
                        case "LargoMin":
                            largomin_2 = (float)para.AsDouble();
                            break;
                        case "Number":

                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool M2_ObtenerAnguloDireccionBarra()
        {
            try
            {
                anguloBarraLosaGrado_1 = anguloPelotaLosaGrado_1;

                if (_ubicacionBarraEnlosa == UbicacionLosa.Superior || _ubicacionBarraEnlosa == UbicacionLosa.Inferior)
                { anguloBarraLosaGrado_1 = anguloPelotaLosaGrado_1 + 90; }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool M3_ObtenerDireccionPrincipal()
        {
            try
            {
                IsLuzSecuandiria = false;
                if (_ubicacionBarraEnlosa == UbicacionLosa.Derecha || _ubicacionBarraEnlosa == UbicacionLosa.Izquierda)
                {

                    if (direccionVertical == "2") IsLuzSecuandiria = true;
                    CuantiaBarra = cuantiaHorizontal;
                    //nada;
                }
                else if (_ubicacionBarraEnlosa == UbicacionLosa.Inferior || _ubicacionBarraEnlosa == UbicacionLosa.Superior)
                {

                    if (direccionVertical == "2") IsLuzSecuandiria = false;
                    CuantiaBarra = cuantiaVertical;

                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        internal void GetLargoMin()
        {
            RoomsObj objRoom = new RoomsObj(Room1.Document, Room1, anguloPelotaLosaRad_1, this.nombreLosa_1, this.espesorCM_1, "8a20", "8a20");
            // obtiener los datos largos minimos 
           if(!objRoom.GetDireccionLargoMin(posicionPelota, TipoOrientacionBarra.Horizontal)) return;
            //genera los direccioness principales en funciion de los largos minimos
            objRoom.GetDireccionDireccionPrincipales();

            ListaPtoLineaDireccionLosa = objRoom.ListaInterHorizontal;
            ListaPtoLineaDireccionPerpLosa = objRoom.ListaInterLargoVertical;
            LargoHorizontal = (float)objRoom.LargoHorizontal;
            LargoVertical = (float)objRoom.LargoVertical;

        }

        public void ActualizarEspesor(UIApplication Uiapp)
        {
            Document doc = Uiapp.ActiveUIDocument.Document;
            _seleccionarLosaConMouse = new SeleccionarLosaConMouse(Uiapp);
            //selecciona un objeto floor
        
            //seleciona un losa y lo almacen en 'r_fx'

            //obtiene una referencia floor con la referencia r
            if (_seleccionarLosaConMouse.M1_SelecconarFloor()==null) return;

            Floor selecFloor = _seleccionarLosaConMouse.LosaSelecionado;
            if (selecFloor == null) return;

            string espesor = ParameterUtil.FindParaByBuiltInParameter(selecFloor, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, doc);

            double aux_espesor = 0;
            if (double.TryParse(espesor, out aux_espesor))
            {
                espesor = Util.FootToCm(aux_espesor).ToString();
            }

            // agregar parametros
            RoomFunciones.AgregarParametrosRoom(doc, Room1, "Espesor", espesor);

        }

        internal void CambiarDireccionesPrincipal(Document Uidoc)
        {
            //obtener los largos en funcion de la pelota de losa, se recacula pq se pudo mover la pelota losa
            RoomsObj objRoom = new RoomsObj(Room1.Document, Room1, Util.GradosToRadianes(anguloPelotaLosaGrado_1), this.nombreLosa_1, this.espesorCM_1, "8a20", "8a20");
            // obtiener los datos largos minimos 
            if (!objRoom.GetDireccionLargoMin(posicionPelota, TipoOrientacionBarra.Horizontal)) return ; ;



            if (direccionHorizontal == "1")
            {
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "Direccion Horizontal", "2");
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "Direccion Vertical", "1");
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "LargoMin", objRoom.LargoVertical);

                direccionHorizontal = "2";
                direccionVertical = "1";
                largomin_1 = objRoom.LargoVertical;

                objRoom.DireLosaHor = "2";
                objRoom.DireLosaVert = "1";
                objRoom.largoMIn = objRoom.LargoVertical;

            }
            else
            {
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "Direccion Horizontal", "1");
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "Direccion Vertical", "2");
                RoomFunciones.AgregarParametrosRoom(Uidoc, Room1, "LargoMin", objRoom.LargoHorizontal);

                direccionHorizontal = "1";
                direccionVertical = "2";
                largomin_1 = objRoom.LargoHorizontal;

                objRoom.DireLosaHor = "1";
                objRoom.DireLosaVert = "2";
                objRoom.largoMIn = objRoom.LargoHorizontal;

            }
        }




        #endregion





    }

}

