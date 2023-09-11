using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using System.Diagnostics;
//using planta_aux_C.Elemento_Losa;

namespace ArmaduraLosaRevit.Model.Prueba
{
    // crea un objetco : RoomsData
    // aun nose analiza  


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class probrarExtenderlinea : IExternalCommand
    {
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


            try
            {
        
                List<XYZ> list = new List<XYZ>();


                XYZ p1 = new XYZ(1, 1, 0);
                XYZ p2 = new XYZ(5, 5, 0);
                double largoInicial = p1.DistanceTo(p2);
                //45 grados
                var r45=  UtilBarras.extenderLineaDistancia(p1, p2, 1);

                list.Clear();
                list.Add(p1); list.Add(p2); list.Add(r45[0]); list.Add(r45[1]);
                GraficarPtos DibujoPtosTrasladados = new GraficarPtos(list);
                DibujoPtosTrasladados.ShowDialog();


                Debug.Assert(r45[0].IsAlmostEqualTo(new XYZ(5.707106781, 5.707106781, 0.000000000)));
                Debug.Assert(r45[1].IsAlmostEqualTo(new XYZ(0.292893219, 0.292893219, 0.000000000)));
                Debug.Assert( Util.IsEqual(r45[0].DistanceTo(r45[1]),largoInicial+2));
                

                //-135
                var resMenos135=UtilBarras.extenderLineaDistancia(p2, p1, 1);
                list.Clear();
                list.Add(p1); list.Add(p2); list.Add(resMenos135[0]); list.Add(resMenos135[1]);
                GraficarPtos DibujoPtosTrasladados2 = new GraficarPtos(list);
                DibujoPtosTrasladados2.ShowDialog();
                Debug.Assert(resMenos135[0].IsAlmostEqualTo(new XYZ(5.707106781, 5.707106781, 0.000000000)));
                Debug.Assert(resMenos135[1].IsAlmostEqualTo(new XYZ(0.292893219, 0.292893219, 0.000000000)));
                Debug.Assert(Util.IsEqual(resMenos135[0].DistanceTo(resMenos135[1]), largoInicial + 2));


                XYZ p3 = new XYZ(1, 5, 0);
                XYZ p4 = new XYZ(5, 1, 0);
                largoInicial = p3.DistanceTo(p4);
                //-45
                var resMenos45=UtilBarras.extenderLineaDistancia(p3, p4, 1);
                list.Clear();
                list.Add(p3); list.Add(p4); list.Add(resMenos45[0]); list.Add(resMenos45[1]);
                GraficarPtos DibujoPtosTrasladados3 = new GraficarPtos(list);
                DibujoPtosTrasladados3.ShowDialog();
                Debug.Assert(resMenos45[0].IsAlmostEqualTo(new XYZ(5.707106781, 0.292893219, 0.000000000)));
                Debug.Assert(resMenos45[1].IsAlmostEqualTo(new XYZ(0.292893219, 5.707106781, 0.000000000)));
                Debug.Assert(Util.IsEqual(resMenos45[0].DistanceTo(resMenos45[1]), largoInicial + 2));
                //135
                var res135= UtilBarras.extenderLineaDistancia(p4, p3, 1);
                list.Clear();
                list.Add(p3); list.Add(p4); list.Add(res135[0]); list.Add(res135[1]);
                GraficarPtos DibujoPtosTrasladados4 = new GraficarPtos(list);
                DibujoPtosTrasladados4.ShowDialog();
                Debug.Assert(res135[0].IsAlmostEqualTo(new XYZ(5.707106781, 0.292893219, 0.000000000)));
                Debug.Assert(res135[1].IsAlmostEqualTo(new XYZ(0.292893219, 5.707106781, 0.000000000)));
                Debug.Assert(Util.IsEqual(res135[0].DistanceTo(res135[1]), largoInicial + 2));



                XYZ p5 = new XYZ(1, 0, 0);
                XYZ p6 = new XYZ(1, 5, 0);
                largoInicial = p5.DistanceTo(p6);
                //90 grados
                var r90 = UtilBarras.extenderLineaDistancia(p5, p6, 1);
                list.Clear();
                list.Add(p5); list.Add(p6); list.Add(r90[0]); list.Add(r90[1]);
                GraficarPtos DibujoPtosTrasladados5= new GraficarPtos(list);
                DibujoPtosTrasladados5.ShowDialog();


                Debug.Assert(r90[0].IsAlmostEqualTo(new XYZ(1, 6, 0.000000000)));
                Debug.Assert(r90[1].IsAlmostEqualTo(new XYZ(1, -1, 0.000000000)));
                Debug.Assert(Util.IsEqual(r90[0].DistanceTo(r90[1]), largoInicial + 2));

                //180 grados
                var r270= UtilBarras.extenderLineaDistancia(p6, p5, 1);
                list.Clear();
                list.Add(p5); list.Add(p6); list.Add(r270[0]); list.Add(r270[1]);
                GraficarPtos DibujoPtosTrasladados6 = new GraficarPtos(list);
                DibujoPtosTrasladados6.ShowDialog();


                Debug.Assert(r270[0].IsAlmostEqualTo(new XYZ(1, 6, 0.000000000)));
                Debug.Assert(r270[1].IsAlmostEqualTo(new XYZ(1, -1, 0.000000000)));
                Debug.Assert(Util.IsEqual(r270[0].DistanceTo(r270[1]), largoInicial + 2));



                XYZ p7 = new XYZ(1, 1, 0);
                XYZ p8 = new XYZ(5, 1, 0);
                largoInicial = p8.DistanceTo(p7);
                //90 grados
                var r0 = UtilBarras.extenderLineaDistancia(p7, p8, 1);
                list.Clear();
                list.Add(p7); list.Add(p8); list.Add(r0[0]); list.Add(r0[1]);
                GraficarPtos DibujoPtosTrasladados7 = new GraficarPtos(list);
                DibujoPtosTrasladados7.ShowDialog();


                Debug.Assert(r0[0].IsAlmostEqualTo(new XYZ(6, 1, 0.000000000)));
                Debug.Assert(r0[1].IsAlmostEqualTo(new XYZ(0, 1, 0.000000000)));
                Debug.Assert(Util.IsEqual(r0[0].DistanceTo(r0[1]), largoInicial + 2));

                //180 grados
                var r180= UtilBarras.extenderLineaDistancia(p8, p7, 1);
                list.Clear();
                list.Add(p8); list.Add(p7); list.Add(r180[0]); list.Add(r180[1]);
                GraficarPtos DibujoPtosTrasladados8 = new GraficarPtos(list);
                DibujoPtosTrasladados8.ShowDialog();


                Debug.Assert(r180[0].IsAlmostEqualTo(new XYZ(6, 1, 0.000000000)));
                Debug.Assert(r180[1].IsAlmostEqualTo(new XYZ(0, 1, 0.000000000)));
                Debug.Assert(Util.IsEqual(r180[0].DistanceTo(r180[1]), largoInicial + 2));
                //creator.Execute();
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                return Autodesk.Revit.UI.Result.Cancelled;
            }
            finally
            {

            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
   
    }

}
