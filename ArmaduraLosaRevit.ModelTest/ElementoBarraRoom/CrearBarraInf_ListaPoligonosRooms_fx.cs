using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.ModelTest.ElementoBarraRoom
{
    [TestFixture]
    public class CrearBarraInf_ListaPoligonosRooms_fx
    {
        private Document _doc;
        private UIApplication _uiapp;

        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;
        private TipoBarra _tipoBarra;
        private double deltaFail = 0;
        private SolicitudBarraDTO _solicitudDTO;
        private SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom;

        public CrearBarraInf_ListaPoligonosRooms_fx()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _uiapp = RevitTestExecutive.CommandData.Application;
            //_uiapp2 = RevitTestExecutive.CommandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _opt = _app.Create.NewGeometryOptions();
            _tipoBarra = TipoBarra.f1;
            _uidoc.ActiveView = (View)_doc.GetElement(new ElementId(1164163));
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fx()
        {
            //Arrage 

            XYZ p1 = new XYZ(-82.053805774, 36.408209390, 26.541994751);
            XYZ p2 = new XYZ(-82.053805774, 14.763779528, 26.541994751);
            XYZ p3 = new XYZ(-62.746062992, 14.763779528, 26.541994751);
            XYZ p4 = new XYZ(-62.746062992, 36.737812287, 26.541994751);




            ElementId neId = new ElementId(1167266);
            Room RoomSelecionado = (Room)_doc.GetElement(neId);
            XYZ ptoMOuse = new XYZ(-72.042132861, 25.892386852, 26.541994751);
            Room m_roomSelecionado_1 = null;

            FilteredElementCollector collector = new FilteredElementCollector(_doc).OfClass(typeof(SpatialElement));
            List<Element> rooms = new List<Element>();
            rooms.Add(RoomSelecionado);

            double largo1 = 0;
            double largo2 = 0;

            //Act
            //obtiene los putnos de dos segmentos roomm, para generar barras
            //List<XYZ> ListaPtosPoligonoLosa = BarraRoomGeometria.ListaPoligonosRooms_MASROOM_fx(rooms, _opt, ptoMOuse, TipoOrientacionBarra.Horizontal, 0,
            //                                                     ref m_roomSelecionado_1,
            //                                                      ref largo1, ref largo2);

            ////assert
            //Assert.IsTrue(ListaPtosPoligonoLosa.Count == 4);
            //Assert.IsTrue(p1.IsAlmostEqualTo(ListaPtosPoligonoLosa[0], 0.001));
            //Assert.IsTrue(p2.IsAlmostEqualTo(ListaPtosPoligonoLosa[1], 0.001));
            //Assert.IsTrue(p3.IsAlmostEqualTo(ListaPtosPoligonoLosa[2], 0.001));
            //Assert.IsTrue(p4.IsAlmostEqualTo(ListaPtosPoligonoLosa[3], 0.001));
            //Assert.IsTrue(RoomSelecionado.Id == m_roomSelecionado_1.Id);

            Assert.IsTrue(m_roomSelecionado_1 != null);

        }

       

        //00********************************************************************************
        //0
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Horixontal_anguloCero()
        {
            //Arrage 
            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
 

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-82.053805774, 9.317585302, 26.541994751);
            XYZ p2 = new XYZ(-82.053805774, -12.335958005, 26.541994751);
            XYZ p3 = new XYZ(-62.746062992, -12.335958005, 26.541994751);
            XYZ p4 = new XYZ(-62.746062992, 9.317585302, 26.541994751);

            XYZ p1_perimetro = new XYZ(-82.053805774, 9.317585302, 26.541994751);
            XYZ p2_perimetro = new XYZ(-82.053805774, -12.335958005, 26.541994751);
            XYZ p3_perimetro = new XYZ(-62.746062992, -12.335958005, 26.541994751);
            XYZ p4_perimetro = new XYZ(-62.746062992, 9.317585302, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            int idRoom = 1167264;
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.640365984, -4.052643633, 26.541994751);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Horixontal_anguloCero()
        {
            //Arrage 

            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
         

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-82.053805774, 36.408209390, 26.541994751);
            XYZ p2 = new XYZ(-82.053805774, 14.763779528, 26.541994751);
            XYZ p3 = new XYZ(-62.746062992, 14.763779528, 26.541994751);
            XYZ p4 = new XYZ(-62.746062992, 36.737812287, 26.541994751);

            XYZ p1_perimetro = new XYZ(-82.053805774, 36.408209390, 26.541994751);
            XYZ p2_perimetro = new XYZ(-82.053805774, 14.763779528, 26.541994751);
            XYZ p3_perimetro = new XYZ(-62.746062992, 14.763779528, 26.541994751);
            XYZ p4_perimetro = new XYZ(-62.746062992, 36.408209390, 26.541994751);
            int idRoom = 1167266;
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.008372696, 25.965871711, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);
            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Verical_anguloCero()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
       

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-80.807086614, -12.926509186, 26.541994751);
            XYZ p2 = new XYZ(-63.500656168, -12.926509186, 26.541994751);
            XYZ p3 = new XYZ(-63.500656168, 9.908136483, 26.541994751);
            XYZ p4 = new XYZ(-81.299212598, 9.908136483, 26.541994751);

            XYZ p1_perimetro = new XYZ(-80.807086614, -12.926509186, 26.541994751);
            XYZ p2_perimetro = new XYZ(-63.500656168, -12.926509186, 26.541994751);
            XYZ p3_perimetro = new XYZ(-63.500656168, 9.908136483, 26.541994751);
            XYZ p4_perimetro = new XYZ(-80.807086614, 9.908136483, 26.541994751);

            int IdLosa = 1111503;
            int idRoom = 1167264;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(-71.456242179, -4.139254403, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);
            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Verical_anguloCero()
        {
            //Arrage 
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
            

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-81.299212598, 14.173228346, 26.541994751);
            XYZ p2 = new XYZ(-63.500656168, 14.173228346, 26.541994751);
            XYZ p3 = new XYZ(-63.500656168, 36.737812287, 26.541994751);
            XYZ p4 = new XYZ(-81.299212598, 36.408209390, 26.541994751);

            XYZ p1_perimetro = new XYZ(-81.299212598, 14.173228346, 26.541994751);
            XYZ p2_perimetro = new XYZ(-63.500656168, 14.173228346, 26.541994751);
            XYZ p3_perimetro = new XYZ(-63.500656168, 36.737812287, 26.541994751);
            XYZ p4_perimetro = new XYZ(-81.299212598, 36.408209390, 26.541994751);


            int IdLosa = 1111503;
            int idRoom = 1167266;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.778919408, 26.371574159, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);
            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }


        //+45********************************************************************************
        //+45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Horixontal_angulo45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
          

            ConstNH.sbLog = new StringBuilder();
            //pto solucion
            XYZ p1 = new XYZ(-400.375018169, deltaFail + -27.645410895, 26.541994751);
            XYZ p2 = new XYZ(-385.063650860, -42.956778205, 26.541994751);
            XYZ p3 = new XYZ(-371.411015009, -29.304142354, 26.541994751);
            XYZ p4 = new XYZ(-386.722382318, -13.992775045, 26.541994751);

            //room
            int idRoom = 1575449;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-385.417359764, -30.669806023, 26.541994751);

            //objeto contenedor de corrdenadasd
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);
            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Horixontal_angulo45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
        

            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-419.530982168, deltaFail + -8.489446896, 26.541994751);
            XYZ p2 = new XYZ(-404.226059037, -23.794370027, 26.541994751);
            XYZ p3 = new XYZ(-390.573423187, -10.141734176, 26.541994751);
            XYZ p4 = new XYZ(-406.111410761, 5.396253398, 26.541994751);

            //room
            int idRoom = 1575451;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-403.909563670, -8.208956485, 26.541994751);
            //Act
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);
            
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Vertical_angulo45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
     

            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-383.764504542, deltaFail +- 42.492797377, 26.541994751);
            XYZ p2 = new XYZ(-371.527010216, -30.255303050, 26.541994751);
            XYZ p3 = new XYZ(-387.673543015, -14.108770252, 26.541994751);
            XYZ p4 = new XYZ(-400.259022962, -26.694250199, 26.541994751);



            //room
            int idRoom = 1575449;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-386.359787791, -31.003550502, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Vertical_angulo45()
        {
            //Arrage 

            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto

            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-403.274898341, -23.678374820, 26.541994751);
            XYZ p2 = new XYZ(-390.689418394, -11.092894872, 26.541994751);
            XYZ p3 = new XYZ(-406.958185861, 5.164483608, 26.541994751);
            XYZ p4 = new XYZ(-419.310601365, -7.654060783, 26.541994751);
            //room
            int IdLosa = 1575157;
            int idRoom = 1575451;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(-406.794734636, -6.909945052, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }


        //-45********************************************************************************
        //-45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Horixontal_anguloMENOS45()
        {
            //Arrage 

            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uidoc, _solicitudDTO);

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(244.123389432, 38.039936443, 26.541994751);
            XYZ p2 = new XYZ(228.786796930, 22.753835953, 26.541994751);
            XYZ p3 = new XYZ(242.416903200, 9.078707639, 26.541994751);
            XYZ p4 = new XYZ(257.753495702, 24.364808129, 26.541994751);

            int idRoom = 1574612;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Horixontal_anguloMENOS45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
     

            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(263.310912525, 57.164289270, 26.541994751);
            XYZ p2 = new XYZ(247.980774818, 41.884622324, 26.541994751);
            XYZ p3 = new XYZ(261.610881089, 28.209494010, 26.541994751);
            XYZ p4 = new XYZ(277.174467208, 43.721840796, 26.541994751);

            int idRoom = 1574614;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(262.221105744, 41.420962976, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa112_Vertical_anguloMENOS45()
        {
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
            //Arrage 
            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(229.248635041, 21.453926371, 26.541994751);
            XYZ p2 = new XYZ(241.465935054, 9.196271001, 26.541994751);
            XYZ p3 = new XYZ(257.639068965, 25.316158790, 26.541994751);
            XYZ p4 = new XYZ(245.074357577, 37.922373081, 26.541994751);

            int idRoom = 1574612;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPuntosDelantesAtrasRoomConMouse_fxV2_Losa115_Vertical_anguloMENOS45()
        { 
            //segundoObjeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
           

            //Arrage 
            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(248.095201555, 40.933271662, 26.541994751);
            XYZ p2 = new XYZ(260.659912943, 28.327057371, 26.541994751);
            XYZ p3 = new XYZ(276.944093930, 44.568996897, 26.541994751);
            XYZ p4 = new XYZ(264.145934130, 56.942531347, 26.541994751);

            int IdLosa = 1574320;
            int idRoom = 1574614;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(264.883593677, 43.350522254, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1, p2, p3, p4, false);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);
        }


       


        private void COmprobandoPtosv2(aux_listaPtoDTO listaPto, int idRoom, int IdLosa, XYZ ptoMOuse, SolicitudBarraDTO _solicitudBarraDTO)
        {
            //room
            ElementId RoomID = new ElementId(idRoom);
            Room RoomSelecionado = (Room)_doc.GetElement(RoomID);

            //losa
            ElementId LosaID = new ElementId(IdLosa);
            Floor LosaSelecionado = (Floor)_doc.GetElement(LosaID);

            //objeto 'RefereciaRoomDatos'
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uidoc, _solicitudBarraDTO) { LosaSeleccionada1= LosaSelecionado 
                                                                                                  ,RoomSelecionado1=RoomSelecionado
                                                                                                  ,PtoConMouseEnlosa1=ptoMOuse};

            //obtener datos de room
            ReferenciaRoomDatos roomAnalizado = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO) { PtoSeleccionMouse1 = ptoMOuse, Losa = LosaSelecionado };
            roomAnalizado.GetParametrosUnRoom();

            //obtener orientacion
          

            //Act
            //obtiene los putnos de dos segmentos roomm, para gnerar barras
            //List<XYZ> ListaPtosPoligonoLosa = BarraRoomGeometria.ListaPoligonosRooms_fx(roomAnalizado, _opt, _solicitudBarraDTO.TipoOrientacion);
            ////assert
            ////   Assert.Multiple(() =>
            ////   {rev
            //Assert.IsTrue(ListaPtosPoligonoLosa.Count == 4);
            //Assert.IsTrue(listaPto.p1.IsAlmostEqualTo(ListaPtosPoligonoLosa[0], 0.001));
            //Assert.IsTrue(listaPto.p2.IsAlmostEqualTo(ListaPtosPoligonoLosa[1], 0.001));
            //Assert.IsTrue(listaPto.p3.IsAlmostEqualTo(ListaPtosPoligonoLosa[2], 0.001));
            //Assert.IsTrue(listaPto.p4.IsAlmostEqualTo(ListaPtosPoligonoLosa[3], 0.001));



            //assert
            //   Assert.Multiple(() =>
            ////   {rev
            //if (listaPto.IsAssertPerimetro)
            //{
            //    IList<Curve> curvesPathreiforment = new List<Curve>();
            //    //comprobar ptos perimetro
            //    //List<XYZ> ListaPtosPerimetroBarras = BarraRoomGeometria.ListaFinal_pto(ListaPtosPoligonoLosa, _app, LosaSelecionado, roomAnalizado.anguloBarraLosaGrado_1, _solicitudBarraDTO.TipoOrientacion, _solicitudBarraDTO.TipoBarra.ToString(), ref curvesPathreiforment,
            //    //                                                          roomAnalizado.largomin_1, roomAnalizado.largomin_1, TipoConfiguracionBarra.refuerzoInferior, true);

            //    var result = BarraRoomGeometria.ListaFinal_ptov2(ListaPtosPoligonoLosa, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, roomAnalizado, true);

            //    List<XYZ> ListaPtosPerimetroBarras = result.Item1;
            //   // _curvesPathreiforment = curvesPathreiforment = result.Item2;


            //    Assert.IsTrue(ListaPtosPerimetroBarras.Count == 4);
            //    Assert.IsTrue(listaPto.p1_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[0], 0.001));
            //    Assert.IsTrue(listaPto.p2_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[1], 0.001));
            //    Assert.IsTrue(listaPto.p3_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[2], 0.001));
            //    Assert.IsTrue(listaPto.p4_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[3], 0.001));
            //}
            //  });
        }

  
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void ObtenerPArametrosINternosDeRoomSeleionado()
        {
            //Arrage 

            //--room
            ElementId RoomID = new ElementId(1575453);
            Room RoomSelecionado = (Room)_doc.GetElement(RoomID);

            //--losa
            ElementId LosaID = new ElementId(1575157);
            Floor RoLosaSelecionado = (Floor)_doc.GetElement(LosaID);
            //--pto conmouse
            XYZ ptoMOuse = ((LocationPoint)(RoomSelecionado.Location)).Point;//  new XYZ(-72.042132861, 25.892386852, 26.541994751);
            //
            ReferenciaRoomDatos roomAnalizado = new ReferenciaRoomDatos(_doc,RoomSelecionado) { PtoSeleccionMouse1 = ptoMOuse, Losa = RoLosaSelecionado };
            roomAnalizado.GetParametrosUnRoom();


            Assert.IsTrue(roomAnalizado.espesorCM_1.ToString() == "15");

        }




    }
}
