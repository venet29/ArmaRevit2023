using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.ModelTest.CambiarEspesorLosa
{
    [TestFixture]
    public class CambiarEspesorLosa_CambioRoomEspesorTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;
        private Level _Level;

        public CambiarEspesorLosa_CambioRoomEspesorTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _Level = _doc.ActiveView.GenLevel;
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



        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void VerificarSiptosXYZEstanDentroOFueraDeLosa()
        {
 

            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);

            #region Solopara ver los producto cruz y producto pto

            XYZ P1 = new XYZ(0, 0, 1);
            XYZ P2 = new XYZ(0, 0, -1);

            var pr1 = P1.CrossProduct(P2);
            var pr2 = P1.CrossProduct(P1);
            var pr3 = P1.DotProduct(P2);
            var pr4 = P1.DotProduct(P1);
            #endregion


            //crear losa
            int idPathd = 1111503;
            ElementId RoomElem = new ElementId(idPathd);
            Floor floor = (Floor)_doc.GetElement(RoomElem);

            //crear room fuera
            int idRoomFueraLosa = 1575453;
            ElementId ELRoomFueraLosa = new ElementId(idRoomFueraLosa);
            Room roomFuera = (Room)_doc.GetElement(ELRoomFueraLosa);

            XYZ ptobuscadoOut = ((LocationPoint)roomFuera.Location).Point;
            PuntoEnLosa resp1 = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptobuscadoOut, floor);
            Assert.IsTrue(resp1 == PuntoEnLosa.PtoFueraLosa);


            //crear room dentro
            int idRoomDentroLosa = 1167436;
            ElementId ELRoomDentrolLosa = new ElementId(idRoomDentroLosa);
            Room roomDentro = (Room)_doc.GetElement(ELRoomDentrolLosa);

            XYZ ptoBuscadaIN = ((LocationPoint)roomDentro.Location).Point;
            PuntoEnLosa resp2 = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptoBuscadaIN, floor);
            Assert.IsTrue(resp2 == PuntoEnLosa.PtoDentroLosa);

            //pto null
            PuntoEnLosa respPtoNull = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(null, floor);
            Assert.IsTrue(respPtoNull == PuntoEnLosa.ptoNull);
            //pto cero
            PuntoEnLosa respPtoCERO = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(new XYZ(0, 0, 0), floor);
            Assert.IsTrue(respPtoCERO == PuntoEnLosa.ptCero);
            //losa floor
            PuntoEnLosa resptaFLOOR = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptobuscadoOut, null);
            Assert.IsTrue(resptaFLOOR == PuntoEnLosa.losaNull);




        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void VerificarSiSeCambianPArametrosEspesorA16Room()
        {


            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);

            #region Solopara ver los producto cruz y producto pto

            XYZ P1 = new XYZ(0, 0, 1);
            XYZ P2 = new XYZ(0, 0, -1);

            var pr1 = P1.CrossProduct(P2);
            var pr2 = P1.CrossProduct(P1);
            var pr3 = P1.DotProduct(P2);
            var pr4 = P1.DotProduct(P1);
            #endregion


            //crear losa
            int idPathd = 1111503;
            ElementId RoomElem = new ElementId(idPathd);
            Floor floor = (Floor)_doc.GetElement(RoomElem);
            Level levelLOsa = _doc.GetElement(floor.LevelId) as Level;


            List<Room> listaDeRoomEnLosa = SeleccionarRoom.SeleccionarRoomNivelYEnLosa(_doc, floor);
            Assert.IsTrue(listaDeRoomEnLosa.Count == 17);
            //Iguañ a 16  == listaDeRoomEnLosa.count

        }




    }
}
