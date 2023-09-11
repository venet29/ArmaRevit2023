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
    public  class CrearSuple_ListaPoligonosRooms_S1_S3V2
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

        private IList<Curve> _curvesPathreiforment;

        public CrearSuple_ListaPoligonosRooms_S1_S3V2()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _uiapp = RevitTestExecutive.CommandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _tipoBarra = TipoBarra.s1;
            _uidoc.ActiveView = (View)_doc.GetElement(new ElementId(1164163));
        }

        /// <summary>
        /// Implements the FailuresProcessing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor failuresAccessor = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> failures = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureSeverity fseverity = failuresAccessor.GetSeverity();

                if (fseverity == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(f);
                }
                else
                {
                    failuresAccessor.ResolveFailure(f);
                    e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
                }
            }
            e.SetProcessingResult(FailureProcessingResult.Continue);
        }

        //0 grados*************************************************************************************************************
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa410Losa115_Horizontal_anguloCero()
        {

            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse1 = new XYZ(-85.435298935, 21.592288854, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-77.933408244, 21.298097063, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-82.119422572, 14.435695538, 26.541994751);
            XYZ p2 = new XYZ(-82.119422572, 33.464566929, 26.541994751);
            XYZ p3 = new XYZ(-81.299212598, 36.408209390, 26.541994751);
            XYZ p4 = new XYZ(-81.299212598, 14.763779528, 26.541994751);

            XYZ p1_perimetro = new XYZ(-78.374212598, 14.763779528, 26.541994751);
            XYZ p2_perimetro = new XYZ(-78.374212598, 33.464566929, 26.541994751);
            XYZ p3_perimetro = new XYZ(-85.044422572, 33.464566929, 26.541994751);
            XYZ p4_perimetro = new XYZ(-85.044422572, 14.763779528, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);

            
            //pto conmouse
      
            //Act
            COmprobandoPtosv2(_listaPto,  IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(-85.044422572, 14.763779528, 26.541994751);
            XYZ p1Path = new XYZ(-85.044422572, 33.464566929, 26.541994751);
            double largo = 18.7;
            ComprobandoPATH(p0Path, p1Path, largo,_curvesPathreiforment);

        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa113Losa410_vertical_anguloCero()
        {

            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse1 = new XYZ(-92.601982825, 7.038109898, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-93.479574712, 13.912579684, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-82.119420000, 10.236220000, 26.541990000);
            XYZ p2 = new XYZ(-101.689630000, 10.236220000, 26.541990000);
            XYZ p3 = new XYZ(-101.689630000, 10.892390000, 26.541990000);
            XYZ p4 = new XYZ(-82.119420000, 10.892390000, 26.541990000);

            XYZ p1_perimetro = new XYZ(-101.689630000, 7.300690000, 26.541990000);
            XYZ p2_perimetro = new XYZ(-82.119420000, 7.300690000, 26.541990000);
            XYZ p3_perimetro = new XYZ(-82.119420000, 13.827920000, 26.541990000);
            XYZ p4_perimetro = new XYZ(-101.689630000, 13.827920000, 26.541990000);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);


            //pto conmouse

            //Act
            COmprobandoPtosv2(_listaPto, IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(-101.689630000, 13.827920000, 26.541990000);
            XYZ p1Path = new XYZ(-82.119420000, 13.827920000, 26.541990000);
            double largo = 19.5702099737532;
            ComprobandoPATH(p0Path, p1Path, largo, _curvesPathreiforment);

        }

        //45 grados*************************************************************************************************************
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa410Losa115_Horizontal_angulo45()
        {

            int IdLosa = 1575157;
            XYZ ptoMOuse1 = new XYZ(-412.676361248, -20.177877601, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-406.126218899, -14.072890557, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-404.040466706, -24.072758523, 26.541994751);
            XYZ p2 = new XYZ(-417.495910705, -10.617314524, 26.541994751);
            XYZ p3 = new XYZ(-418.997404217, -7.955868944, 26.541994751);
            XYZ p4 = new XYZ(-403.692481086, -23.260792075, 26.541994751);

            XYZ p1_perimetro = new XYZ(-419.564198040, -12.685601859, 26.541994751);
            XYZ p2_perimetro = new XYZ(-406.340744455, -25.909055444, 26.541994751);
            XYZ p3_perimetro = new XYZ(-401.624193751, -21.192504740, 26.541994751);
            XYZ p4_perimetro = new XYZ(-414.847647336, -7.969051155, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);


            //pto conmouse

            //Act
            COmprobandoPtosv2(_listaPto, IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(-414.847647336, -7.969051155, 26.541994751);
            XYZ p1Path = new XYZ(-401.624193751, -21.192504740, 26.541994751);
            double largo = 18.7;
            ComprobandoPATH(p0Path, p1Path, largo, _curvesPathreiforment);

        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa113Losa410_vertical_angulo45()
        {

            int IdLosa = 1575157;
            XYZ ptoMOuse1 = new XYZ(-406.571374204, -38.937994039, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-414.202608009, -32.578632535, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(-401.070989410, -27.042235820, 26.541994751);
            XYZ p2 = new XYZ(-414.909217592, -40.880464001, 26.541994751);
            XYZ p3 = new XYZ(-415.373198419, -40.416483174, 26.541994751);
            XYZ p4 = new XYZ(-401.534970000, -26.578250000, 26.541990000);

            XYZ p1_perimetro = new XYZ(-412.833483414, -42.956198179, 26.541994751);
            XYZ p2_perimetro = new XYZ(-398.995260000, -29.117970000, 26.541990000);
            XYZ p3_perimetro = new XYZ(-403.610700000, -24.502520000, 26.541990000);
            XYZ p4_perimetro = new XYZ(-417.448930000, -38.340750000, 26.541990000);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);


            //pto conmouse

            //Act
            COmprobandoPtosv2(_listaPto, IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(-417.448930000, -38.340750000, 26.541990000);
            XYZ p1Path = new XYZ(-403.610700000, -24.502520000, 26.541990000);
            double largo = 19.570209973753244;
            ComprobandoPATH(p0Path, p1Path, largo, _curvesPathreiforment);

        }


        //-45 grados*************************************************************************************************************
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa410Losa115_Horizontal_anguloMENOS45()
        {

            int IdLosa = 1574320;
            XYZ ptoMOuse1 = new XYZ(251.748445936, 50.255902900, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(257.112429118, 46.274389610, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(247.702080688, 41.699489263, 26.541994751);
            XYZ p2 = new XYZ(261.179692280, 55.132729088, 26.541994751);
            XYZ p3 = new XYZ(263.843609967, 56.629832258, 26.541994751);
            XYZ p4 = new XYZ(248.513472260, 41.350165312, 26.541994751);

            XYZ p1_perimetro = new XYZ(250.578346507, 39.278470512, 26.541994751);
            XYZ p2_perimetro = new XYZ(263.823585486, 52.480102754, 26.541994751);
            XYZ p3_perimetro = new XYZ(259.114818033, 57.204423887, 26.541994751);
            XYZ p4_perimetro = new XYZ(245.869579055, 44.002791646, 26.541994751);
            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);


            //pto conmouse

            //Act
            COmprobandoPtosv2(_listaPto, IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(245.869579055, 44.002791646, 26.541994751);
            XYZ p1Path = new XYZ(259.114818033, 57.204423887, 26.541994751);
            double largo = 18.7;
            ComprobandoPATH(p0Path, p1Path, largo, _curvesPathreiforment);

        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSuple_Losa113Losa410_vertical_anguloMENOS45()
        {

            int IdLosa = 1574320;
            XYZ ptoMOuse1 = new XYZ(231.724428505, 48.113637919, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(235.931885901, 52.321095315, 26.541994751);


            //primer objeto
            _solicitudDTO = new SolicitudBarraDTO(_uiapp, _tipoBarra.ToString(), UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);
            //segundoObjeto


            ConstNH.sbLog = new StringBuilder();
            XYZ p1 = new XYZ(244.727710000, 38.734910000, 26.541990000);
            XYZ p2 = new XYZ(230.912320000, 52.595940000, 26.541990000);
            XYZ p3 = new XYZ(231.377060000, 53.059150000, 26.541990000);
            XYZ p4 = new XYZ(245.192460000, 39.198130000, 26.541990000);

            XYZ p1_perimetro = new XYZ(228.833160000, 50.523630000, 26.541990000);
            XYZ p2_perimetro = new XYZ(242.648560000, 36.662600000, 26.541990000);
            XYZ p3_perimetro = new XYZ(247.271610000, 41.270440000, 26.541990000);
            XYZ p4_perimetro = new XYZ(233.456220000, 55.131460000, 26.541990000);

            aux_listaPtoDTO _listaPto = new aux_listaPtoDTO(p1, p2, p3, p4, p1_perimetro, p2_perimetro, p3_perimetro, p4_perimetro, true);


            //pto conmouse

            //Act
            COmprobandoPtosv2(_listaPto, IdLosa, ptoMOuse1, ptoMOuse2, _solicitudDTO);


            XYZ p0Path = new XYZ(233.456220000, 55.131460000, 26.541990000);
            XYZ p1Path = new XYZ(247.271610000, 41.270440000, 26.541990000);
            double largo = 19.5702099737532;
            ComprobandoPATH(p0Path, p1Path, largo, _curvesPathreiforment);

        }





        private void COmprobandoPtosv2(aux_listaPtoDTO listaPto,  int IdLosa, XYZ ptoMOuse1, XYZ ptoMOuse2, SolicitudBarraDTO _solicitudBarraDTO)
        {
       

            //losa
            ElementId LosaID = new ElementId(IdLosa);
            Floor LosaSelecionado = (Floor)_doc.GetElement(LosaID);

            //objeto 'RefereciaRoomDatos'

            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uidoc, _solicitudBarraDTO);
            _seleccionarLosaBarraRoom.AsignarDOSRoom(ptoMOuse1, ptoMOuse2, LosaSelecionado);
            _seleccionarLosaBarraRoom.ObtenerDOSRoom();
            //obtener datos de room
            ReferenciaRoomDatos _refereciaRoomDatos = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO);
            _refereciaRoomDatos.GetParametrosUnRoom();

            //obtener orientacion


            //Act
            //obtiene los putnos de dos segmentos roomm, para gnerar barras
            Tuple< List<XYZ>,double> result1 = BarraRoomGeometria.ListaPoligonosRooms_S1_S3V2(_solicitudBarraDTO.commandData.Application, _refereciaRoomDatos,  _solicitudBarraDTO.TipoOrientacion);

           List<XYZ> ListaPtosPoligonoLosa = result1.Item1;
           double anguloBordeYsegundoPtoMouse = result1.Item2;

            //assert
            //   Assert.Multiple(() =>
            //   {rev
            Assert.IsTrue(ListaPtosPoligonoLosa.Count == 4);
            Assert.IsTrue(listaPto.p1.IsAlmostEqualTo(ListaPtosPoligonoLosa[0], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(listaPto.p2.IsAlmostEqualTo(ListaPtosPoligonoLosa[1], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(listaPto.p3.IsAlmostEqualTo(ListaPtosPoligonoLosa[2], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(listaPto.p4.IsAlmostEqualTo(ListaPtosPoligonoLosa[3], ConstNH.CONST_FACT_TOLERANCIA_3Deci));



            //assert
            //   Assert.Multiple(() =>
            //   {rev
            if (listaPto.IsAssertPerimetro)
            {
         
                //comprobar ptos perimetro
                //List<XYZ> ListaPtosPerimetroBarras = BarraRoomGeometria.ListaFinal_pto(ListaPtosPoligonoLosa, _app, LosaSelecionado, _refereciaRoomDatos.anguloBarraLosaGrado_1, _solicitudBarraDTO.TipoOrientacion, _solicitudBarraDTO.TipoBarra.ToString(), ref curvesPathreiforment,
                //                                                          _refereciaRoomDatos.largomin_1, _refereciaRoomDatos.largomin_1, TipoConfiguracionBarra.suple, true);

                var result = BarraRoomGeometria.ListaFinal_ptov2(ListaPtosPoligonoLosa, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, _refereciaRoomDatos, true);

                List<XYZ> ListaPtosPerimetroBarras = result.Item1;
                _curvesPathreiforment = result.Item2;

                Assert.IsTrue(ListaPtosPerimetroBarras.Count == 4);
                Assert.IsTrue(listaPto.p1_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[0], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p2_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[1], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p3_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[2], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
                Assert.IsTrue(listaPto.p4_perimetro.IsAlmostEqualTo(ListaPtosPerimetroBarras[3], ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            }
            //  });
        }

        private void ComprobandoPATH(XYZ p0, XYZ p1, double largo, IList <Curve> _curvesPathreiforment)
        {
            Assert.IsTrue(_curvesPathreiforment[0].GetEndPoint(0).IsAlmostEqualTo(p0, ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(_curvesPathreiforment[0].GetEndPoint(1).IsAlmostEqualTo(p1, ConstNH.CONST_FACT_TOLERANCIA_3Deci));
            Assert.IsTrue(Math.Abs(_curvesPathreiforment[0].Length-largo)<0.1);
        }

    }
}
