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
    public class CrearBarraInf_ListaFinal_ptov2
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
        private List<XYZ> _listaPtosLineasAtrasDelante;
        private IList<Curve> _curvesPathreiforment;

        public CrearBarraInf_ListaFinal_ptov2()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _opt = _app.Create.NewGeometryOptions();
            _tipoBarra = TipoBarra.f1;
            _uidoc.ActiveView = (View)_doc.GetElement(new ElementId(1164163));
        }


        //00********************************************************************************
        //0
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerPerimetroPathConMouse_fxV2_Losa112_Horixontal_anguloCero()
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


            //  segundo caso          
            XYZ p0Path = new XYZ(-62.746062992, 9.317585302, 26.541994751);
            XYZ p1Path = new XYZ(-62.746062992, -12.335958005, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);

        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Horixontal_anguloCero()
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

            //  segundo caso          
            XYZ p0Path = new XYZ(-62.746062992, 36.408209390, 26.541994751);
            XYZ p1Path = new XYZ(-62.746062992, 14.763779528, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa112_Verical_anguloCero()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior ,false);
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


            //  segundo caso          
            XYZ p0Path = new XYZ(-80.807086614, 9.908136483, 26.541994751);
            XYZ p1Path = new XYZ(-63.500656168, 9.908136483, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Verical_anguloCero()
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


            //  segundo caso          
            XYZ p0Path = new XYZ(-81.299212598, 36.408209390, 26.541994751);
            XYZ p1Path = new XYZ(-63.500656168, 36.737812287, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);

        }

        //+45********************************************************************************
        //+45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa112_Horixontal_angulo45()
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

            XYZ p1_perimetro = new XYZ(-400.375018169, -27.645410895, 26.541994751);
            XYZ p2_perimetro = new XYZ(-385.063650860, -42.956778205, 26.541994751);
            XYZ p3_perimetro = new XYZ(-371.411015009, -29.304142354, 26.541994751);
            XYZ p4_perimetro = new XYZ(-386.722382318, -13.992775045, 26.541994751);

            //room
            int idRoom = 1575449;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-385.417359764, -30.669806023, 26.541994751);

            //objeto contenedor de corrdenadasd
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso    
            XYZ p0Path = new XYZ(-386.722382318, -13.992775045, 26.541994751);
            XYZ p1Path = new XYZ(-371.411015009, -29.304142354, 26.541994751);

            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);

        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Horixontal_angulo45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-419.530982168, deltaFail + -8.489446896, 26.541994751);
            XYZ p2 = new XYZ(-404.226059037, -23.794370027, 26.541994751);
            XYZ p3 = new XYZ(-390.573423187, -10.141734176, 26.541994751);
            XYZ p4 = new XYZ(-406.111410761, 5.396253398, 26.541994751);


            XYZ p1_perimetro = new XYZ(-419.530982168, -8.489446896, 26.541994751);
            XYZ p2_perimetro = new XYZ(-404.226059037, -23.794370027, 26.541994751);
            XYZ p3_perimetro = new XYZ(-390.573423187, -10.141734176, 26.541994751);
            XYZ p4_perimetro = new XYZ(-405.878346318, 5.163188955, 26.541994751);

            //room
            int idRoom = 1575451;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-403.909563670, -8.208956485, 26.541994751);
            //Act
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso    
            XYZ p0Path = new XYZ(-405.878346318, 5.163188955, 26.541994751);
            XYZ p1Path = new XYZ(-390.573423187, -10.141734176, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa112_Vertical_angulo45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-383.764504542, deltaFail + -42.492797377, 26.541994751);
            XYZ p2 = new XYZ(-371.527010216, -30.255303050, 26.541994751);
            XYZ p3 = new XYZ(-387.673543015, -14.108770252, 26.541994751);
            XYZ p4 = new XYZ(-400.259022962, -26.694250199, 26.541994751);

            XYZ p1_perimetro = new XYZ(-383.764504542, -42.492797377, 26.541994751);
            XYZ p2_perimetro = new XYZ(-371.527010216, -30.255303050, 26.541994751);
            XYZ p3_perimetro = new XYZ(-387.673543015, -14.108770252, 26.541994751);
            XYZ p4_perimetro = new XYZ(-399.911037341, -26.346264578, 26.541994751);

            //room
            int idRoom = 1575449;
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-384.575076277, -30.111194745, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso    
            XYZ p0Path = new XYZ(-399.911037341, -26.346264578, 26.541994751);
            XYZ p1Path = new XYZ(-387.673543015, -14.108770252, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);

        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Vertical_angulo45()
        {
            //Arrage 

            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();

            XYZ p1 = new XYZ(-403.274898341, -23.678374820, 26.541994751);
            XYZ p2 = new XYZ(-390.689418394, -11.092894872, 26.541994751);
            XYZ p3 = new XYZ(-406.958185861, 5.164483608, 26.541994751);
            XYZ p4 = new XYZ(-419.310601365, -7.654060783, 26.541994751);

            XYZ p1_perimetro = new XYZ(-403.274898341, -23.678374820, 26.541994751);
            XYZ p2_perimetro = new XYZ(-390.695112887, -11.098589366, 26.541994751);
            XYZ p3_perimetro = new XYZ(-406.958185861, 5.164483608, 26.541994751);
            XYZ p4_perimetro = new XYZ(-419.305012325, -7.648260835, 26.541994751);
            //room
            int IdLosa = 1575157;
            int idRoom = 1575451;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(-407.865561545, -5.660646992, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso  
            XYZ p0Path = p4_perimetro;
            XYZ p1Path = p3_perimetro;
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }


        //-45********************************************************************************
        //-45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa112_Horixontal_anguloMENOS45()
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

            XYZ p1_perimetro = new XYZ(244.100841292, 38.017462538, 26.541994751);
            XYZ p2_perimetro = new XYZ(228.786796930, 22.753835953, 26.541994751);
            XYZ p3_perimetro = new XYZ(242.439451339, 9.101181544, 26.541994751);
            XYZ p4_perimetro = new XYZ(257.753495702, 24.364808129, 26.541994751);

            int idRoom = 1574612;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso  
            XYZ p0Path = new XYZ(257.753495702, 24.364808129, 26.541994751);
            XYZ p1Path = new XYZ(242.439451339, 9.101181544, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Horixontal_anguloMENOS45()
        {
            //Arrage 
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(263.310912525, 57.164289270, 26.541994751);
            XYZ p2 = new XYZ(247.980774818, 41.884622324, 26.541994751);
            XYZ p3 = new XYZ(261.610881089, 28.209494010, 26.541994751);
            XYZ p4 = new XYZ(277.174467208, 43.721840796, 26.541994751);

            XYZ p1_perimetro = new XYZ(263.310912525, 57.164289270, 26.541994751);
            XYZ p2_perimetro = new XYZ(247.980774818, 41.884622324, 26.541994751);
            XYZ p3_perimetro = new XYZ(261.633429228, 28.231967914, 26.541994751);
            XYZ p4_perimetro = new XYZ(276.963566935, 43.511634860, 26.541994751);

            int idRoom = 1574614;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(262.221105744, 41.420962976, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso  
            XYZ p0Path = new XYZ(276.963566935, 43.511634860, 26.541994751);
            XYZ p1Path = new XYZ(261.633429228, 28.231967914, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa112_Vertical_anguloMENOS45()
        {
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto
            //Arrage 
            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(229.248635041, 21.453926371, 26.541994751);
            XYZ p2 = new XYZ(241.465935054, 9.196271001, 26.541994751);
            XYZ p3 = new XYZ(257.639068965, 25.316158790, 26.541994751);
            XYZ p4 = new XYZ(245.074357577, 37.922373081, 26.541994751);

            XYZ p1_perimetro = new XYZ(229.248635041, 21.453926371, 26.541994751);
            XYZ p2_perimetro = new XYZ(241.465935054, 9.196271001, 26.541994751);
            XYZ p3_perimetro = new XYZ(257.612489801, 25.342825748, 26.541994751);
            XYZ p4_perimetro = new XYZ(245.395189789, 37.600481119, 26.541994751);

            int idRoom = 1574612;
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso  
            XYZ p0Path = new XYZ(245.395189789, 37.600481119, 26.541994751);
            XYZ p1Path = new XYZ(257.612489801, 25.342825748, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void obtenerobtenerPuntosPerimetroPathConMouse_fxV2_Losa115_Vertical_anguloMENOS45()
        {
            //segundoObjeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Superior, TipoConfiguracionBarra.refuerzoInferior, false);


            //Arrage 
            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(248.095201555, 40.933271662, 26.541994751);
            XYZ p2 = new XYZ(260.659912943, 28.327057371, 26.541994751);
            XYZ p3 = new XYZ(276.596160870, 44.209123740, 26.541994751);
            XYZ p4 = new XYZ(263.798001069, 56.582658190, 26.541994751);

            XYZ p1_perimetro = new XYZ(248.121864014, 40.906521134, 26.541994751);
            XYZ p2_perimetro = new XYZ(260.659912943, 28.327057371, 26.541994751);
            XYZ p3_perimetro = new XYZ(276.568613092, 44.235757520, 26.541994751);
            XYZ p4_perimetro = new XYZ(263.798001069, 56.582658190, 26.541994751);

            int IdLosa = 1574320;
            int idRoom = 1574614;

            //pto conmouse
            XYZ ptoMOuse = new XYZ(262.947589884, 41.826750681, 26.541994751);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            //Act
            COmprobandoPtosv2(_listaPto, idRoom, IdLosa, ptoMOuse, _solicitudDTO);

            //  segundo caso  
            XYZ p0Path = new XYZ(263.798001069, 56.582658190, 26.541994751);
            XYZ p1Path = new XYZ(276.568613092, 44.235757520, 26.541994751);
            ComprobandoPATH(p0Path, p1Path, _curvesPathreiforment);
        }


        private void COmprobandoPtosv2(aux_listaPtoDTO listaPto, int idRoom, int IdLosa, XYZ ptoMOuse, SolicitudBarraDTO _solicitudBarraDTO)
        {
   
            //1)obtener datos de room
            //room
            ElementId RoomID = new ElementId(idRoom);
            Room RoomSelecionado = (Room)_doc.GetElement(RoomID);

            //losa
            ElementId LosaID = new ElementId(IdLosa);
            Floor LosaSelecionado = (Floor)_doc.GetElement(LosaID);

            //objeto seleccion  'RefereciaRoomDatos'
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uidoc, _solicitudBarraDTO) { LosaSeleccionada1 = LosaSelecionado, RoomSelecionado1 = RoomSelecionado, PtoConMouseEnlosa1 = ptoMOuse };

            //objeto roomAnalizado
            ReferenciaRoomDatos roomAnalizado = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO) { PtoSeleccionMouse1 = ptoMOuse, Losa = LosaSelecionado };
            roomAnalizado.GetParametrosUnRoom();
                  
 
            //3) crear lista con ptos de borde de room de atras y delante que intersecta atras y delante de 
            /* p1 |                 | p3
                  |---------------  |
               P2 |                 | p3
*/
            _listaPtosLineasAtrasDelante = new List<XYZ>();
            _listaPtosLineasAtrasDelante.Add(listaPto.p1);
            _listaPtosLineasAtrasDelante.Add(listaPto.p2);
            _listaPtosLineasAtrasDelante.Add(listaPto.p3);
            _listaPtosLineasAtrasDelante.Add(listaPto.p4);
            //obtener orientacion

            //assert
            //   Assert.Multiple(() =>
            //   {rev
            if (listaPto.IsAssertPerimetro)
            {
                _curvesPathreiforment = new List<Curve>();
                IList<Curve> curvesPathreiforment = new List<Curve>();
                //comprobar ptos perimetro
                var result = BarraRoomGeometria.ListaFinal_ptov2(_listaPtosLineasAtrasDelante, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, roomAnalizado, true);

                List<XYZ> ListaPtosPerimetroBarras = result.Item1;
                _curvesPathreiforment = curvesPathreiforment= result.Item2;

                Assert.IsTrue(ListaPtosPerimetroBarras.Count == 4);
                Assert.IsTrue(listaPto.p1_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[0], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p2_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[1], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p3_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[2], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p4_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[3], ConstNH.CONST_FACT_TOLERANCIA_3Deci));          
            }           
        }


        private void ComprobandoPATH(XYZ p0, XYZ p1, IList<Curve> _curvesPathreiforment)
        {
            Assert.IsTrue(_curvesPathreiforment[0].GetEndPoint(0).IsAlmostEqualTo(p0, ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(_curvesPathreiforment[0].GetEndPoint(1).IsAlmostEqualTo(p1, ConstNH.CONST_FACT_TOLERANCIA_3Deci));
        }


    }


}
