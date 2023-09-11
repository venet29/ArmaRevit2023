using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

//using planta_aux_C.Elemento_Losa;
using System.Diagnostics;
using ArmaduraLosaRevit;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Tag;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.Elementos_viga;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.LosaArmadura.Cmd
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Cmd_CrearRoomPtoSelecionado : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        List<XYZ> ListaPtos = new List<XYZ>();
        private static ExternalCommandData m_revit;
        private static Document doc;


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            doc = uidoc.Document;
            Options opt = app.Create.NewGeometryOptions();
            m_revit = commandData;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
            //selecciona un objeto floor

            Reference r;
            try
            {
                r = uidoc.Selection.PickObject(ObjectType.Face, f, "Please select a planar face to define work plane");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                r = null;
            }

            // sirefere3ncia es null salir
            if (r == null)
                return Result.Succeeded;

            //obtiene una referencia floor con la referencia r
            Floor selecFloor = doc.GetElement(r.ElementId) as Floor;

            //obtiene el nivel del la losa
            Level levelLOsa = doc.GetElement(selecFloor.LevelId) as Level;
            //obtiene el pto de seleccion con el mouse sobre la losa
            //   XYZ pt_selec = r.GlobalPoint;
            XYZ pt_selec = new XYZ(r.GlobalPoint.X, r.GlobalPoint.Y, levelLOsa.ProjectElevation + Util.CmToFoot(2));
            UV pt_selecUV = r.UVPoint;

            //View actual
            Autodesk.Revit.DB.View view = doc.ActiveView;
            // busca el nivel del pisos analizado
            ElementId levelId = Util.FindLevelId(doc, view.GenLevel.Name);

            #region 1)creando lista con vigas y sus contornos
            // crea objetos con las vigas los contornos de las vigas del view
            NH_ListaVIgas listaVigas = new NH_ListaVIgas(commandData);
            /// 1)obtiene lista de vigas en el nivel de trabajo 
            /// 2)obtiene su  geometria ProfileBeam
            listaVigas.GetVigaPoligonos(doc.ActiveView);
            if (listaVigas.ListaProfileBeam.Count > 0)
            {
                Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "Classe_prueba_vigas");
                trans.Start();
                try
                {
                    listaVigas.BorrarTodas();
                    //Genera LAs lineas de separacion de rooms
                    listaVigas.DibujarLineasSeparacicionRoom();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    trans.RollBack();
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
                trans.Commit();
                //  return Autodesk.Revit.UI.Result.Succeeded;
            }

            #endregion

            #region 2)generar rooms en losa
            //generar romm
            RoomsObj roomData = new RoomsObj(doc);
            Room roomCreat = roomData.CreateRoom(levelLOsa, pt_selec);

            ParameterSet pars = roomCreat.Parameters;
            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Agregando los parametros internos2-NH");
                    var nm = ParameterUtil.SetParaInt(roomCreat, "Numero Losa", "410");
                    var espesor = ParameterUtil.SetParaInt(roomCreat, "Espesor", 15);
                    var angle = ParameterUtil.SetParaInt(roomCreat, "Angulo", 0.0);
                    var CH = ParameterUtil.SetParaInt(roomCreat, "Cuantia Horizontal", "8a20");
                    var CV = ParameterUtil.SetParaInt(roomCreat, "Cuantia Vertical", "8a20");
                    var DH = ParameterUtil.SetParaInt(roomCreat, "Direccion Horizontal", "1");
                    var DV = ParameterUtil.SetParaInt(roomCreat, "Direccion Vertical", "2");
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            var tipoTag = TiposRoomTagType.getRoomTagType(ConstNH.CONST_TAGROOMFAMILIA + "_" + ConstNH.CONST_ESCALA_BASE, doc);//doc.ActiveView.Scale
                                                                                                                                                       // roomData.CreateTagRoom(doc, roomCreat, tipoTag, null);


            // LocationPoint locPoint = tmpRoom.Location as LocationPoint;
            AgregarTagRoom AgregarTagRoom = new AgregarTagRoom(doc, 0.0);
            AgregarTagRoom.M1CreaTag(roomCreat, tipoTag, ((LocationPoint)roomCreat.Location).Point);

            #endregion

            return Result.Succeeded;
        }



    }



}
