
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VARIOS;
using Microsoft.VisualBasic;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;
using planta_aux_C;


using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Diagnostics;

namespace planta_aux_C.Utiles
{
    public class DatosBarras
    {

        public static string[] TIPO_caso_BARRA_LOSA_ind_1barra_v2(ObjectId obj, string tipo_referencia)
        {

            Polyline poli;
            Database db = AcApp.DocumentManager.MdiActiveDocument.Database;

            string[] largo = new string[11];

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                poli = tr.GetObject(obj, OpenMode.ForRead) as Polyline;

                // NOTA: entrega esl tipo de datos de 1 barra

                // si barra es completamente horiozntas estonces son casos 200  ( coordenadas iniciales a la izq)
                // si tiene alguna inclinacion  es caso 100  , donde "Y" mayor es el pto cero o inicial ( coordenadas iniciales con coordenada y mas altai)


                Document acDoc = Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                int cont = poli.NumberOfVertices;

              
                int factor_doble = 0;
                factor_doble = 1;

                // largo(4) :  coordenada 2,3 _  4,5
                switch (cont)
                {
                    case 2:
                        {
                            Point2d pto1 = poli.GetPoint2dAt(0);
                            Point2d pto2 = poli.GetPoint2dAt(1);
                            if (tipo_referencia == "R1")
                                largo[0] = "R1";
                            else
                                largo[0] = "f3";


                            if (Math.Abs(pto1.X - pto2.X) < 0.1)
                            {
                                if (pto1.Y > pto2.Y)
                                {
                                    largo[1] = "AZ";
                                    largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                }
                                else
                                {
                                    largo[1] = "ZA";
                                    largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                }
                                largo[2] = "vertical_b";
                            }
                            else
                            {
                                if (pto1.X > pto2.X)
                                {
                                    largo[1] = "AZ";
                                    largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                }
                                else
                                {
                                    largo[1] = "ZA";
                                    largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                }
                                largo[2] = "horizontal_i";
                            }
                            largo[3] = (pto1.X + pto2.X) / (double)2 + "," + (pto1.Y + pto2.Y) / (double)2;
                            largo[5] = pto1.GetDistanceTo(pto2).ToString();
                            largo[6] = "0";
                            largo[7] = "0";
                            largo[8] = "0";
                            largo[9] = "0";
                            break;
                        }

                    case 3 // f11  y f18
             :
                        {
                            bool aux_horizontal = false;
                            Point2d pto0 = poli.GetPoint2dAt(0);
                            Point2d pto1 = poli.GetPoint2dAt(1);
                            Point2d pto2 = poli.GetPoint2dAt(2);

                            largo[0] = "f11";


                            if (pto0.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto1))
                            {
                            }
                            else
                            {
                            }


                            if (pto0.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto1))
                            {
                                if (Math.Abs(pto1.X - pto2.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto1.Y > pto2.Y)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = "0";
                                        largo[9] = pto0.GetDistanceTo(pto1).ToString();
                                    }

                                    if (pto0.X > pto1.X & largo[0] == "f11")
                                        largo[0] = "f9a_V";
                                }
                                else
                                {
                                    // HORIZONTAL O INCLINADA

                                    if (pto1.X > pto2.X)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = "0";
                                        largo[9] = pto0.GetDistanceTo(pto1).ToString();
                                    }

                                    if (pto0.Y < pto1.Y & largo[0] == "f11")
                                        largo[0] = "f9a_V";
                                }
                                largo[5] = pto1.GetDistanceTo(pto2).ToString();
                            }
                            else
                            {
                                if (Math.Abs(pto1.X - pto0.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto0.Y > pto1.Y)
                                    {
                                        largo[4] = pto0.X + "," + pto0.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = "0";
                                        largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                    }
                                    else
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto0.X + "," + pto0.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }

                                    if (pto2.X > pto1.X & largo[0] == "f11")
                                        largo[0] = "f9a_V";
                                }
                                else
                                {
                                    // HORIZONTAL O INCLINADA

                                    if (pto0.X > pto1.X)
                                    {
                                        largo[4] = pto0.X + "," + pto0.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto0.X + "," + pto0.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = "0";
                                        largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                    }

                                    if (pto2.Y < pto1.Y & largo[0] == "f11")
                                        largo[0] = "f9a_V";
                                }

                                largo[5] = pto0.GetDistanceTo(pto1).ToString();
                            }

                            // If largo(0) = "f16" Or largo(0) = "f17" Or largo(0) = "f18" Or largo(0) = "f9a_V" Then
                            // largo(2) = datos_barra(9)
                            // End If


                            largo[3] = (pto1.X + pto2.X) / (double)2 + "," + (pto1.Y + pto2.Y) / (double)2;
                            break;
                        }

                    case 4 // f1  , f10  , f19  y f9a
             :
                        {
                            Point2d pto0 = poli.GetPoint2dAt(0);
                            Point2d pto1 = poli.GetPoint2dAt(1);
                            Point2d pto2 = poli.GetPoint2dAt(2);
                            Point2d pto3 = poli.GetPoint2dAt(3);
                            bool aux_horizontal = false;



                            if ((pto0.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto3) & pto2.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto3)))
                            {
                                largo[0] = "f1";
                                // If factor_doble = 1 Then
                                // Else
                                // largo(0) = "f19"
                                // End If

                                if (Math.Abs(pto3.X - pto2.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto2.Y > pto3.Y)
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "AZ";
                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[8] = "0";
                                        largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                    }
                                }
                                else
                                    // HORIZONTAL O INCLINADA

                                    if (pto2.X > pto3.X)
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "AZ";
                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[8] = "0";
                                        largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                    }
                                largo[3] = (pto3.X + pto2.X) / (double)2 + "," + (pto3.Y + pto2.Y) / (double)2;

                                largo[5] = pto3.GetDistanceTo(pto2).ToString();
                            }
                            else if ((pto3.GetDistanceTo(pto2) < pto0.GetDistanceTo(pto1) & pto2.GetDistanceTo(pto1) < pto0.GetDistanceTo(pto1)))
                            {
                                largo[0] = "f1";
                                // If factor_doble = 1 Then

                                // Else
                                // largo(0) = "f19"
                                // End If

                                if (Math.Abs(pto1.X - pto0.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto0.Y > pto1.Y)
                                    {
                                        largo[4] = pto0.X + "," + pto0.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[8] = "0";
                                        largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                    }
                                    else
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto0.X + "," + pto0.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "ZA";
                                        largo[6] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                }
                                else
                                    // HORIZONTAL O INCLINADA

                                    if (pto0.X > pto1.X)
                                    {
                                        largo[4] = pto0.X + "," + pto0.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "AZ";
                                        largo[6] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto0.X + "," + pto0.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "ZA";
                                        largo[6] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[7] = "0";
                                        largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                        largo[9] = "0";
                                    }
                                largo[3] = (pto1.X + pto0.X) / (double)2 + "," + (pto1.Y + pto0.Y) / (double)2;
                                largo[5] = pto1.GetDistanceTo(pto0).ToString();
                            }
                            else if ((pto3.GetDistanceTo(pto2) < pto1.GetDistanceTo(pto2) & pto0.GetDistanceTo(pto1) < pto1.GetDistanceTo(pto2)))
                            {
                                if ((pto0.Y > pto1.Y | pto0.X < pto1.X))
                                    largo[0] = "f10";
                                else
                                    largo[0] = "f9a";


                                if (Math.Abs(pto1.X - pto2.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto1.Y > pto2.Y)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[9] = pto2.GetDistanceTo(pto3).ToString();
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[9] = pto0.GetDistanceTo(pto1).ToString();
                                    }
                                }
                                else
                                    // HORIZONTAL O INCLINADA

                                    if (pto1.X > pto2.X)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "AZ";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto2.GetDistanceTo(pto3).ToString();
                                        largo[9] = pto0.GetDistanceTo(pto1).ToString();
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "ZA";
                                        largo[6] = "0";
                                        largo[7] = "0";
                                        largo[8] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[9] = pto2.GetDistanceTo(pto3).ToString();
                                    }
                                largo[3] = (pto1.X + pto2.X) / (double)2 + "," + (pto1.Y + pto2.Y) / (double)2;
                                largo[5] = pto1.GetDistanceTo(pto2).ToString();
                            }

                            break;
                        }

                    case 5:
                        {
                            Point2d pto0 = poli.GetPoint2dAt(0);
                            Point2d pto1 = poli.GetPoint2dAt(1);
                            Point2d pto2 = poli.GetPoint2dAt(2);
                            Point2d pto3 = poli.GetPoint2dAt(3);
                            Point2d pto4 = poli.GetPoint2dAt(4);



                            if ((pto0.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto3) & pto2.GetDistanceTo(pto1) < pto2.GetDistanceTo(pto3)))
                            {
                                if (pto1.GetDistanceTo(pto2) != pto3.GetDistanceTo(pto4))
                                    largo[0] = "s3";
                                else
                                    largo[0] = "f7";



                                if (Math.Abs(pto3.X - pto2.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto2.Y > pto3.Y)
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "ZA";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3b";

                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "AZ";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3a";

                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                    }
                                }
                                else
                                    // HORIZONTAL O INCLINADA

                                    if (pto2.X > pto3.X)
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "ZA";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3b";

                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                    }
                                    else
                                    {
                                        largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "AZ";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3a";

                                        largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                        largo[7] = "0";
                                    }

                                largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                largo[9] = pto3.GetDistanceTo(pto4).ToString();

                                largo[3] = (pto3.X + pto2.X) / (double)2 + "," + (pto3.Y + pto2.Y) / (double)2;
                                largo[5] = pto3.GetDistanceTo(pto2).ToString();
                            }
                            else if ((pto4.GetDistanceTo(pto3) < pto2.GetDistanceTo(pto1) & pto3.GetDistanceTo(pto2) < pto2.GetDistanceTo(pto1)))
                            {
                                if (tipo_referencia.IndexOf("s", 0) != -1)
                                    largo[0] = "s3";
                                else if (Math.Abs(pto0.GetDistanceTo(pto1) - pto3.GetDistanceTo(pto2)) < 0.5)
                                    largo[0] = "f7";                               
                                else
                                {
                                    Application.ShowAlertDialog("Posible error, asignando barra s3 sin referencia inicial 's'.");
                                    largo[0] = "s3";
                                }


                                if (Math.Abs(pto1.X - pto2.X) < 0.1)
                                {
                                    // VERTICAL
                                    if (pto1.Y > pto2.Y)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "vertical_b";
                                        largo[1] = "AZ";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3a";
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto1.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "vertical_a";
                                        largo[1] = "ZA";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3b";
                                    }
                                }
                                else
                                    // HORIZONTAL O INCLINADA

                                    if (pto1.X > pto2.X)
                                    {
                                        largo[4] = pto1.X + "," + pto1.Y + "_" + pto2.X + "," + pto2.Y;
                                        largo[2] = "horizontal_i";
                                        largo[1] = "AZ";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3a";
                                    }
                                    else
                                    {
                                        largo[4] = pto2.X + "," + pto2.Y + "_" + pto1.X + "," + pto1.Y;
                                        largo[2] = "horizontal_d";
                                        largo[1] = "ZA";
                                        if (largo[0] == "s3")
                                            largo[0] = "s3b";
                                    }
                                largo[6] = pto3.GetDistanceTo(pto4).ToString();
                                largo[7] = "0";
                                largo[8] = pto2.GetDistanceTo(pto3).ToString();
                                largo[9] = pto0.GetDistanceTo(pto1).ToString();

                                largo[3] = (pto1.X + pto2.X) / (double)2 + "," + (pto1.Y + pto2.Y) / (double)2;
                                largo[5] = pto1.GetDistanceTo(pto2).ToString();
                            }

                            break;
                        }

                    case 6  // f9 f4
             :
                        {
                            Point2d pto0 = poli.GetPoint2dAt(0);
                            Point2d pto1 = poli.GetPoint2dAt(1);
                            Point2d pto2 = poli.GetPoint2dAt(2);
                            Point2d pto3 = poli.GetPoint2dAt(3);
                            Point2d pto4 = poli.GetPoint2dAt(4);
                            Point2d pto5 = poli.GetPoint2dAt(5);


                            if (tipo_referencia.IndexOf("s", 0) != -1)
                                largo[0] = "s1";
                            else if ((pto1.Y > pto2.Y | pto1.X < pto2.X))
                                largo[0] = "f4";
                            
                            else
                                largo[0] = "f9";


                            if (Math.Abs(pto2.X - pto3.X) < 0.1)
                            {
                                // VERTICAL
                                if (pto2.Y > pto3.Y)
                                {
                                    largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                    largo[2] = "vertical_a";
                                    largo[1] = "ZA";

                                    largo[6] = pto4.GetDistanceTo(pto5).ToString();
                                    largo[7] = pto0.GetDistanceTo(pto1).ToString();
                                    largo[8] = pto4.GetDistanceTo(pto3).ToString();
                                    largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                }
                                else
                                {
                                    largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                    largo[2] = "vertical_a";
                                    largo[1] = "AZ";


                                    largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                    largo[7] = pto4.GetDistanceTo(pto5).ToString();
                                    largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                    largo[9] = pto4.GetDistanceTo(pto3).ToString();
                                }
                            }
                            else
                                // HORIZONTAL O INCLINADA

                                if (pto2.X > pto3.X)
                                {
                                    largo[4] = pto2.X + "," + pto2.Y + "_" + pto3.X + "," + pto3.Y;
                                    largo[2] = "horizontal_i";
                                    largo[1] = "AZ";

                                    largo[6] = pto4.GetDistanceTo(pto5).ToString();
                                    largo[7] = pto0.GetDistanceTo(pto1).ToString();
                                    largo[8] = pto4.GetDistanceTo(pto3).ToString();
                                    largo[9] = pto1.GetDistanceTo(pto2).ToString();
                                }
                                else
                                {
                                    largo[4] = pto3.X + "," + pto3.Y + "_" + pto2.X + "," + pto2.Y;
                                    largo[2] = "horizontal_i";
                                    largo[1] = "ZA";


                                    largo[6] = pto0.GetDistanceTo(pto1).ToString();
                                    largo[7] = pto4.GetDistanceTo(pto5).ToString();
                                    largo[8] = pto1.GetDistanceTo(pto2).ToString();
                                    largo[9] = pto4.GetDistanceTo(pto3).ToString();
                                }

                            largo[3] = (pto3.X + pto2.X) / (double)2 + "," + (pto3.Y + pto2.Y) / (double)2;
                            largo[5] = pto3.GetDistanceTo(pto2).ToString();
                            break;
                        }

                    default:
                        {

                            ed.WriteMessage("Not between 1 and 10, inclusive");
                            break;
                        }
                }

            }
            return largo;
        }


    }
}
