/*
 * Created by SharpDevelop.
 * User: Master2
 * Date: 30/04/2020
 * Time: 18:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
//using Excel = Microsoft.Office.Interop.Excel;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public partial class ThisApplication
    {

        static Document doc_global = null;
        //private ExternalCommandData commandData;
        private UIApplication _Application;
        private UIDocument uidoc;
        private Document doc;

        public ThisApplication(UIApplication uiapp)
        {
            //this.commandData = null;
            _Application = uiapp;
            uidoc = uiapp.ActiveUIDocument;
            doc = uiapp.ActiveUIDocument.Document;
        }
        public void Mi_Primer_Macro()
        {
            //			TaskDialog.Show("CURSO REVIT API","HOLA PERU");
            //			EspacioExterior.nuevaclase CLASEEXTERNA = new EspacioExterior.nuevaclase();
            //			CLASEEXTERNA.Mi_Segunda_Macro();
            int entero1 = 3;
            int entero2 = 1;
            double valoraltura = 16.126165;
#pragma warning disable CS0219 // The variable 'cadena1' is assigned but its value is never used
            string cadena1 = "REVIT ES EL MEJOR SOFTWARE";
#pragma warning restore CS0219 // The variable 'cadena1' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'resultadopregunta' is assigned but its value is never used
            bool resultadopregunta = true;
#pragma warning restore CS0219 // The variable 'resultadopregunta' is assigned but its value is never used

            int suma = entero1 + entero2;
            double suma2 = entero1 + valoraltura;
            double division = entero2 / entero1;

            if (entero1 < entero2)
            {
                //				TaskDialog.Show("PROISAC","IF ENTERO 1 ES MENOR");
            }
            else
            {
                //				TaskDialog.Show("PROISAC","IF ENTERO 2 ES MENOR");
            }

            while (entero2 < 10)
            {
                //				TaskDialog.Show("PROISAC","WHILE "+entero2.ToString());
                //entero2 = entero2+1;
                entero2++;
            }

            for (int i = 0; i < 10; i++)
            {
                valoraltura = valoraltura * Math.PI;
                //				TaskDialog.Show("PROISAC",valoraltura.ToString());
            }

            List<double> listadatos = new List<double>();
            listadatos.Add(5.23);
            listadatos.Add(1.34);
            listadatos.Add(4.67676);
            listadatos.Add(4654.6);
            listadatos.Add(5);
            listadatos.Add(4565.1561);
            listadatos.Add(456.2123);

            double sumadedatos = 0;
            foreach (double datodouble in listadatos)
            {
                sumadedatos = datodouble + sumadedatos;
            }
            //			TaskDialog.Show("PROISAC","SUMA DE DATOS:"+sumadedatos.ToString());

            string tipodesoftware = "Revit";

            switch (tipodesoftware)
            {
                case "EXCEL":
                    TaskDialog.Show("PROISAC", "CREADOR ES MICROSOFT");
                    break;
                case "Tekla":
                    TaskDialog.Show("PROISAC", "CREADOR ES TRIMBLE");
                    break;
                case "Revit":
                    TaskDialog.Show("PROISAC", "CREADOR ES AUTODESK");
                    break;
                case "Archicad":
                    TaskDialog.Show("PROISAC", "CREADOR ES GRAPHISOFT");
                    break;
                default:
                    TaskDialog.Show("PROISAC", "NO SE QUIEN ES EL CREADOR");
                    break;
            }

        }
        public void C2_M1_Seleccion()
        {
            ;

            ICollection<ElementId> seleccionactual = uidoc.Selection.GetElementIds();

            if (seleccionactual.Count == 0)
            {
                TaskDialog.Show("CURSO REVIT API", "NO HAY NADA SELECCIONADO");
            }
            else
            {
                StringBuilder sbuilder = new StringBuilder();//PERMITE ALMACER LISTAS DE TEXTOS
                sbuilder.AppendLine("Los elementos selecciondos son:");

                foreach (ElementId idelem in seleccionactual)
                {
                    Element e_seleccionado = doc.GetElement(idelem);
                    Category categoria = e_seleccionado.Category;
                    sbuilder.AppendLine(categoria.Name + ": " + e_seleccionado.Name);
                }
                TaskDialog.Show("CURSO REVIT API", sbuilder.ToString());
            }


        }

        public void C2_M2_SeleccionManual()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            #endregion

            StringBuilder sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto
            try
            {

                #region SELECCION CON PICKBOX
                //NOS PIDE SELECCIONAR UN AREA EN UN RECTANGULO. OBTIENE LOS DATOS DE LOS PUNTOS SELECCIONADOS en MIN y MAX
                PickedBox pickedbox_seleccion = uidoc.Selection.PickBox(PickBoxStyle.Crossing, "SELECCION PICKBOX");
                XYZ puntominimo = pickedbox_seleccion.Min;
                XYZ puntomaximo = pickedbox_seleccion.Max;
                //Environment.NewLine es equivalente a un cambio de linea o a dar ENTER en un texto
                sbuilder.AppendLine("PICKEDBOX: " + Environment.NewLine + puntomaximo.ToString() + "   " + puntominimo.ToString());
                sbuilder.AppendLine();
                #endregion

                #region SELECCION CON PICKED BY RECTANGLE
                //Nos permite seleccionar todos los elementos dentro de un area rectangular que seleccionemos
                //Obteniene un listado de elementos tipo ELEMENT que debe ser guardado una variable ILIST
                IList<Element> pickedbyrectangle_seleccion = uidoc.Selection.PickElementsByRectangle("SELECCIONAR ELEMENTOS EN UN RECTANGULO");
                sbuilder.AppendLine("PICKED BY RENTANGLE");
                foreach (Element elem in pickedbyrectangle_seleccion)
                {
                    sbuilder.AppendLine("--------" + elem.Name);
                }
                sbuilder.AppendLine();
                #endregion

                #region SELECCION CON PICKOBJECT
                //Nos permite seleccionar un ELEMENTO de forma individual
                //El elemento elegido es reconocido como una REFERENCIA que luego es usada para acceder al ELEMENTO elegido
                Reference ref_pickobject_element = uidoc.Selection.PickObject(ObjectType.Element, "SELECCION PICKOBJECT: SELECCIONA UN ELEMENTO");
                Element elemento_pickobject_element = doc.GetElement(ref_pickobject_element);
                sbuilder.AppendLine("PICKOBJECT: ELEMENTO" + Environment.NewLine + elemento_pickobject_element.Category.Name);

                //Nos permite seleccionar el borde de un elemento
                //El borde elegido es reconocido como una REFERNCIA que luego es usada para acceder a la geometria del borde
                Reference ref_pickobject_borde = uidoc.Selection.PickObject(ObjectType.Edge, "SELECCION PICKOBJECT: SELECCIONA UN BORDE");
                //Usando la referencia accedemos al elemento que contiene el borde
                Element elemento_pickobject_borde = doc.GetElement(ref_pickobject_borde);
                //Usando el metodo GetGeometryObjectFromReference y la REFERENCIA de la seleccion accedemos a la geometria del BORDE o EDGE.
                GeometryObject geometria_borde = elemento_pickobject_borde.GetGeometryObjectFromReference(ref_pickobject_borde);
                sbuilder.AppendLine("PICKOBJECT: BORDE" + Environment.NewLine + geometria_borde.GetType().Name);

                //Nos permite seleccionar la CARA de un elemento
                //La cara elegida es reconocida como una REFERNCIA que luego es usada para acceder a la geometria de la CARA
                Reference ref_pickobject_cara = uidoc.Selection.PickObject(ObjectType.Face, "SELECCION PICKOBJECT: SELECCIONA UNA CARA");
                //Usando la referencia accedemos al elemento que contiene la cara
                Element elemento_pickobject_cara = doc.GetElement(ref_pickobject_cara);
                //Usando el metodo GetGeometryObjectFromReference y la REFERENCIA de la seleccion accedemos a la geometria de la CARA o FACE.
                GeometryObject geometria_cara = elemento_pickobject_cara.GetGeometryObjectFromReference(ref_pickobject_cara);
                sbuilder.AppendLine("PICKOBJECT: CARA" + Environment.NewLine + geometria_cara.GetType().Name);

                //Nos permite seleccionar un PUNTO un elemento
                //El Punto elegido es reconocido como una REFERNCIA que luego es usada para acceder a la geometria del punto
                Reference ref_pickobject_punto = uidoc.Selection.PickObject(ObjectType.PointOnElement, "SELECCION PICKOBJECT: SELECCIONA UN PUNTO EN UN ELEMENTO");
                //Usando la referencia accedemos al elemento que contiene el punto
                Element elemento_pickobject_punto = doc.GetElement(ref_pickobject_punto);
                //Usando el metodo GetGeometryObjectFromReference y la REFERENCIA de la seleccion accedemos a la geometria del PUNTO o POINT.
                GeometryObject geometria_punto = elemento_pickobject_punto.GetGeometryObjectFromReference(ref_pickobject_punto);
                sbuilder.AppendLine("PICKOBJECT: PUNTO" + Environment.NewLine + geometria_punto.GetType().Name);

                //Nos permite seleccionar un SUBELEMENTO
                Reference ref_pickobject_subelemento = uidoc.Selection.PickObject(ObjectType.Subelement, "SELECCION PICKOBJECT: SELECCIONA UN SUBELEMENTO");
                Element elemento_pickobject_subelemento = doc.GetElement(ref_pickobject_subelemento);
                sbuilder.AppendLine("PICKOBJECT: SUBELEMENTO" + Environment.NewLine + elemento_pickobject_subelemento.Category.Name);

                //Nos permite seleccionar un VINCULO
                Reference ref_pickobject_vinculo = uidoc.Selection.PickObject(ObjectType.LinkedElement, "SELECCION PICKOBJECT: SELECCIONA UN VINCULO");
                Element elemento_pickobject_vinculo = doc.GetElement(ref_pickobject_vinculo);
                sbuilder.AppendLine("PICKOBJECT: VINCULO" + Environment.NewLine + elemento_pickobject_vinculo.Name);
                #endregion

                #region SELECCION CON PICKOBJECTS
                //Nos permite seleccionar multiples elementos de forma individual agregando o excluyendo elementos
                //Los elementos elegidos son leidos como REFERENCIAS y se guardan en un listado ILIST
                IList<Reference> refs_pickobjects = uidoc.Selection.PickObjects(ObjectType.Element, "SELECCION PICKOBJECTS: SELECCIONA UNO o VARIOS");

                sbuilder.AppendLine("PICKED BY PICKOBJECTS");
                foreach (Reference ref_elem in refs_pickobjects)
                {
                    Element e_selecionado = doc.GetElement(ref_elem);//Cada REFERENCIA sirve para recuperar el ELEMENTO verdadero elegido.
                    sbuilder.AppendLine("--------" + e_selecionado.Name);
                }
                #endregion

                #region SELECCION CON PICKPOINT
                //Definimos primero los tipos de OSNAP para seleccionar los puntos
                ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
                //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                XYZ punto_pickpoint = uidoc.Selection.PickPoint(snapTypes, "SELECCIONA PICKPOINT: EXTREMOS O INTERSECCIONES DE ELEMENTOS");
                sbuilder.AppendLine("PICKPOINT: " + Environment.NewLine + punto_pickpoint.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            TaskDialog.Show("CURSO REVIT API", sbuilder.ToString());


        }

        public void C3_M1_FiltrosSeleccion()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            #endregion

            WallSelectionFilter filtromurosutilitario = new WallSelectionFilter();
            FiltrosSeleccion.FloorSelectionFilter filtropisos = new FiltrosSeleccion.FloorSelectionFilter();

            //			WallSelectionFilter filtro_muros = new ThisApplication.WallSelectionFilter();
            IList<Reference> refs_pickobjects = uidoc.Selection.PickObjects(ObjectType.Element, filtropisos, "SELECCIONA OBJETOS");
            TaskDialog.Show("PROISAC", refs_pickobjects.Count.ToString());

        }
        public class WallSelectionFilter : Autodesk.Revit.UI.Selection.ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                if (element.Category.Name == "Walls" || element.Category.Name == "Stacked Walls")
                {
                    return true;
                }
                return false;
            }
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }

        public void C3_M2_BusquedaElementos()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            #endregion
            StringBuilder sbuilder = new StringBuilder();

            #region COLECTORES
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            sbuilder.AppendLine("MUROS TOTALES EN EL PROYECTO: " + collector1.GetElementCount().ToString());

            View vistaactual = doc.ActiveView;
            FilteredElementCollector collector2 = new FilteredElementCollector(doc, vistaactual.Id);
            collector2.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            sbuilder.AppendLine("MUROS TOTALES EN LA VISTA ACTUAL: " + collector2.GetElementCount().ToString());

            FilteredElementCollector collector3 = new FilteredElementCollector(doc);
            collector3.OfClass(typeof(WallType));
            sbuilder.AppendLine("TIPOS DE MURO EN EL PROYECTO: " + collector3.GetElementCount().ToString());

            FilteredElementCollector collector4 = new FilteredElementCollector(doc);
            collector4.OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();
            sbuilder.AppendLine("CANTIDAD DE VISTA EN EL PROYECTO: " + collector4.GetElementCount().ToString());

            FilteredElementCollector collector5 = new FilteredElementCollector(doc);
            collector5.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType();
            sbuilder.AppendLine("VENTANAS EN EL PROYECTO: " + collector5.GetElementCount().ToString());
            #endregion

            ICollection<ElementId> listadoseleccion = collector1.ToElementIds();
            uidoc.Selection.SetElementIds(listadoseleccion);

            TaskDialog.Show("PROISAC", sbuilder.ToString());


        }
        public void C3_M3_BusquedaRooms()
        {

            StringBuilder sbuilder = new StringBuilder();

            Autodesk.Revit.DB.Architecture.RoomFilter filtroroom = new Autodesk.Revit.DB.Architecture.RoomFilter();
            FilteredElementCollector collectorrooms = new FilteredElementCollector(doc);
            collectorrooms.WherePasses(filtroroom);

            IList<Element> listadorooms = collectorrooms.ToElements();

            foreach (Element eroom in listadorooms)
            {
                string nombre_room = eroom.Name;
                System.Diagnostics.Debug.Print(nombre_room);
                sbuilder.AppendLine(eroom.Name);
            }
            //			TaskDialog.Show("PROISAC",sbuilder.ToString ());

        }
        public void C3_M4_BusquedaFamilias()
        {

            StringBuilder sbuilder = new StringBuilder();

            #region OBTENER NIVEL
            FilteredElementCollector collectorniveles = new FilteredElementCollector(doc);
            collectorniveles.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Element nivel = collectorniveles.First();
            #endregion

            ElementLevelFilter filtronivel = new ElementLevelFilter(nivel.Id);//FILTRO LENTO SLOW FILTER

            //			FilteredElementCollector collectormuros = new FilteredElementCollector(doc);
            //			collectormuros.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().WherePasses(filtronivel);//FILTRO RAPIDO

            ElementClassFilter filtrofamilias = new ElementClassFilter(typeof(FamilyInstance));
            FilteredElementCollector collectorfamilias = new FilteredElementCollector(doc);
            collectorfamilias.WherePasses(filtrofamilias).WherePasses(filtronivel);

            List<FamilyInstance> familiasseleccionadas = new List<FamilyInstance>();
            List<ElementId> ids_seleccion = new List<ElementId>();
            foreach (Element efamilia in collectorfamilias)
            {
                FamilyInstance fi = efamilia as FamilyInstance;//CONVERSION DE GENERICO A ESPECIFICO
                string nombre = fi.Name;
                //if(nombre.Contains("Golden"))
                if (fi.Category.Name == "Furniture")
                {
                    familiasseleccionadas.Add(fi);
                    ids_seleccion.Add(fi.Id);
                    Location ubicacion_familia = fi.Location;
                    LocationPoint lpoint = ubicacion_familia as LocationPoint;
                    System.Diagnostics.Debug.Print(lpoint.Point.ToString());
                }
            }

            //TaskDialog.Show("PROISAC",familiasseleccionadas.Count.ToString());
            //uidoc.Selection .SetElementIds(collectorfamilias.ToElementIds());
            uidoc.Selection.SetElementIds(ids_seleccion);

        }
        public void C3_M5_InfoLocation()
        {

            StringBuilder sbuilder = new StringBuilder();

            Reference ref_pickobject = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN ELEMENTO");
            Element elemento_pickobject = doc.GetElement(ref_pickobject);

            LocationPoint lpoint = elemento_pickobject.Location as LocationPoint;
            LocationCurve lcurve = elemento_pickobject.Location as LocationCurve;

            if (lpoint != null)//SI NO ES NULO
            {
                TaskDialog.Show("PROISAC", "LOCATION POINT: " + lpoint.Point.ToString());
            }
            if (lcurve != null)
            {
                double largocurva_pies = lcurve.Curve.Length;// VALOR EN PIES
                                                             //double largocurva_metros = largocurva_pies*0.3048;

                double largocurva_metros =  Util.FootToMetre(largocurva_pies);

                TaskDialog.Show("PROISAC", "LOCATION CURVE: " + largocurva_metros.ToString());
            }
            if (lpoint == null && lcurve == null)
            {
                TaskDialog.Show("PROISAC", "LOCATION ES NULA");
            }
        }

        public void tarea2()
        {

            FilteredElementCollector collectopuertas = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType();
            double cantidadpuertas = collectopuertas.Count();

            FiltrosSeleccion.DoorWindowFilter filtropuerta = new FiltrosSeleccion.DoorWindowFilter();
            IList<Reference> ref_seleccon = uidoc.Selection.PickObjects(ObjectType.Element, filtropuerta, "SELECCIONAR");
            double cant_refs = ref_seleccon.Count();

            FilteredElementCollector collectoventanas = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType();
            double cantidadventanas = collectoventanas.Count();

            double total = cantidadpuertas + cantidadventanas;

            TaskDialog.Show("PUERTAS Y VENTANAS", total.ToString());

        }
        public void C4_M1_InfoGeometriaMuro()
        {
            #region VARIABLES INDISPENSABLES

            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER ELEMENTO
            FiltrosSeleccion.WallSelectionFilter filtromuro = new FiltrosSeleccion.WallSelectionFilter();
            Reference ref_muro = null;
            try
            {
                ref_muro = uidoc.Selection.PickObject(ObjectType.Element, filtromuro, "SELECCIONA UN MURO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return; //SALIMOS DE LA MACRO
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_muro);
            Wall muro = elem_pickobject as Wall;
            sbuilder.AppendLine(muro.Name);
            #endregion

            #region PASO 3 OPCIONES DE GEOMETRIA
            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            #endregion

            GeometryElement geo = muro.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects

            foreach (GeometryObject obj in geo)
            {
                Solid solid = obj as Solid;
                //				if(solid ==null){continue;}
                //				if(solid.Faces.Size ==0){continue;}

                if (solid != null && solid.Faces.Size > 0)
                {
                    //CONTINUO TRABAJANDO
                    sbuilder.AppendLine("VOLUMEN SOLIDO: " + solid.Volume.ToString());
                    sbuilder.AppendLine("CANTIDAD DE CARAS: " + solid.Faces.Size.ToString());
                    foreach (Face face in solid.Faces)
                    {
                        sbuilder.AppendLine("----CARA:" + face.Area.ToString());
                        sbuilder.AppendLine("---------PERIMETROS CERRADOS:" + face.EdgeLoops.Size.ToString());
                        foreach (EdgeArray erray in face.EdgeLoops)
                        {
                            sbuilder.AppendLine("--------Perimetro");
                            foreach (Edge borde in erray)
                            {
                                sbuilder.AppendLine("----------------BORDE:" + borde.ApproximateLength.ToString());
                            }
                        }

                    }


                }
            }
            TaskDialog.Show("PROISAC", sbuilder.ToString());

        }

        public void C4_M2_InfoGeometriaAnidada()
        {
            #region VARIABLES INDISPENSABLES

            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER ELEMENTO
            Reference ref_muro = null;
            try
            {
                ref_muro = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN MURO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return; //SALIMOS DE LA MACRO
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_muro);
            sbuilder.AppendLine(elem_pickobject.Name);
            #endregion

            #region PASO 3 OPCIONES DE GEOMETRIA
            Options opt = new Options();
            opt.ComputeReferences = true; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            #endregion

            GeometryElement geo = elem_pickobject.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects

            foreach (GeometryObject obj in geo)
            {
                Solid solid = obj as Solid;
                #region SOLIDO
                if (solid != null && solid.Faces.Size > 0)
                {
                    //CONTINUO TRABAJANDO
                    sbuilder.AppendLine("VOLUMEN SOLIDO: " + solid.Volume.ToString());
                    sbuilder.AppendLine("CANTIDAD DE CARAS: " + solid.Faces.Size.ToString());
                    foreach (Face face in solid.Faces)
                    {
                        sbuilder.AppendLine("----CARA:" + face.Area.ToString());
                        sbuilder.AppendLine("---------PERIMETROS CERRADOS:" + face.EdgeLoops.Size.ToString());
                        foreach (EdgeArray erray in face.EdgeLoops)
                        {
                            sbuilder.AppendLine("--------Perimetro");
                            foreach (Edge borde in erray)
                            {
                                sbuilder.AppendLine("----------------BORDE:" + borde.ApproximateLength.ToString());
                            }
                        }
                    }
                }
                #endregion

                #region GEOMETRYINSTANCE O GEOMETRIA ANIDADA
                if (obj is GeometryInstance)
                {
                    GeometryInstance instanciaanidada = obj as GeometryInstance; //INSTANCIA ANIDADA
                    GeometryElement geo2 = instanciaanidada.GetInstanceGeometry();
                    sbuilder.AppendLine("CANTIDAD DE ELEMENTOS: " + geo2.Count().ToString());
                    foreach (GeometryObject obj2 in geo2)
                    {
                        Solid solid2 = obj2 as Solid;
                        if (solid2 != null && solid2.Faces.Size > 0)
                        {
                            sbuilder.AppendLine("VOLUMEN SOLIDO: " + solid2.Volume.ToString());
                            sbuilder.AppendLine("CANTIDAD DE CARAS: " + solid2.Faces.Size.ToString());
                            foreach (Face face in solid2.Faces)
                            {
                                sbuilder.AppendLine("--CARA: " + face.Area.ToString());
                                sbuilder.AppendLine("---PERIMETROS CERRADOS: " + face.EdgeLoops.Size.ToString());
                                foreach (EdgeArray erray in face.EdgeLoops) //PERIMETROS CERRADOS O EDGELOOPS
                                {
                                    sbuilder.AppendLine("----Perimetro");
                                    foreach (Edge borde in erray) //COLECCION DE LINEAS DE BORDE
                                    {
                                        sbuilder.AppendLine("-------BORDE:" + borde.ApproximateLength.ToString());
                                    }
                                }
                            }
                        }

                    }


                }

                #endregion

            }
            TaskDialog.Show("PROISAC", sbuilder.ToString());

        }
        public void C4_M3_LeerParametros(Element elem_pickobject = null)
        {
            #region VARIABLES INDISPENSABLES

            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            Reference ref_elemento = null;
            if (elem_pickobject == null)
            {

                try
                {
                    ref_elemento = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN ELEMENTO");
                }
                catch (Exception)
                {
                    TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                    return;
                }
                #endregion
                #region PASO 2 OBTENER EL ELEMENTO
                elem_pickobject = doc.GetElement(ref_elemento);
                #endregion
            }
            ParameterSet setparametros = elem_pickobject.Parameters;
            sbuilder.AppendLine("PARAMETROS");
            sbuilder.AppendLine();
            foreach (Parameter para in setparametros)
            {
                string datodelparametro = InformacionParametro(para, doc);
                Debug.WriteLine(datodelparametro);
                sbuilder.AppendLine(datodelparametro);
            }

            TaskDialog.Show("PROISAC", sbuilder.ToString());

        }

        public string InformacionParametro(Parameter para, Document doc)
        {
            string defName = para.Definition.Name;
            switch (para.StorageType)
            {
                case StorageType.Double:
                    defName = defName + " DOUBLE " + para.AsDouble().ToString() + esdelectura(para);
                    break;
                case StorageType.ElementId:
                    ElementId id = para.AsElementId();
                    int valorid = id.IntegerValue;
                    if (valorid > 0)
                    {
                        Element e_paraid = doc.GetElement(id);
                        defName = defName + " : ELEMENT ID " + e_paraid.Name + "  " + valorid.ToString() + esdelectura(para);
                    }
                    else
                    {
                        defName = defName + " : ELEMENT ID " + valorid.ToString() + esdelectura(para);
                    }

                    break;
                case StorageType.Integer:
                    int datointeger = para.AsInteger();
                    if (para.Definition.GetDataType() == SpecTypeId.Boolean.YesNo)//SI ES BOOLEANO?
                    {
                        if (datointeger == 0)
                        {
                            defName = defName + " :INTEGER(FALSO)" + esdelectura(para);
                        }
                        else
                        {
                            defName = defName + " :INTEGER(VERDADERO)" + esdelectura(para);
                        }
                    }
                    else
                    {
                        defName = defName + ": INTEGER " + datointeger.ToString() + esdelectura(para);
                    }

                    break;
                case StorageType.String:
                    defName = defName + " STRING " + para.AsString() + esdelectura(para);
                    break;
            }
            return defName;
        }

        private string esdelectura(Parameter paradato)
        {
            if (paradato.IsReadOnly)
            {
                return " (NO ES EDITABLE)";
            }
            else
            {
                return " (ES EDITABLE)";
            }
        }
        public void C4_M4_ParametrosEspecificos()
        {
            #region VARIABLES INDISPENSABLES


            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            Reference ref_elemento = null;
            try
            {
                ref_elemento = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN ELEMENTO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return;
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_elemento);
            #endregion

            //			Parameter para1 = elem_pickobject.LookupParameter("Offset");//PERMITE ACCEDER A PARAMETROS DE PROYECTO O PARAMETROS COMPARTIDOS

            Parameter para1 = elem_pickobject.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM);  //ES UN METODO UNIVERSAL SIN IMPORTAR EL IDIOMA

            if (para1 == null)
            {
                TaskDialog.Show("PROISAC", "PARAMETRO ES NULO");
            }
            else
            {
                TaskDialog.Show("PROISAC", para1.Definition.ParameterGroup + ": " + para1.Definition.Name);
            }

        }
        public void C5_M1_Multidata()
        {
            #region VARIABLES INDISPENSABLES


            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            Reference ref_elemento = null;
            try
            {
                ref_elemento = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN ELEMENTO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return;
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_elemento);
            #endregion

            #region WORKSETS
            WorksetTable worksetTable = doc.GetWorksetTable();
            Workset workset_elemento = null;
            WorksetId worksetIDbyElement = null;
            if (doc.IsWorkshared)
            {
                #region Metodo 1
                //				worksetIDbyElement = doc.GetWorksetId(elem_pickobject.Id);
                //				if(worksetIDbyElement != WorksetId.InvalidWorksetId)
                //				{
                //					workset_elemento =  worksetTable.GetWorkset(worksetIDbyElement);
                //					sbuilder.AppendLine("WORKSET: "+workset_elemento.Name);
                //				}
                #endregion

                #region Metodo 2
                Parameter param = elem_pickobject.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                if (param != null)
                {
                    int paraValor = param.AsInteger();
                    worksetIDbyElement = new WorksetId(paraValor);
                    if (worksetIDbyElement != WorksetId.InvalidWorksetId)
                    {
                        workset_elemento = worksetTable.GetWorkset(worksetIDbyElement);
                        sbuilder.AppendLine("WORKSET: " + workset_elemento.Name);
                    }
                }


                #endregion
            }
            else
            {
            }
            #endregion

            #region OPCIONES DE DISEÑO
            DesignOption opciondiseno = elem_pickobject.DesignOption;
            string nombreopcionactual = string.Empty;
            if (opciondiseno == null)
            {
                nombreopcionactual = "MAIN MODEL";
            }
            else
            {
                nombreopcionactual = opciondiseno.Name;
            }
            sbuilder.AppendLine("OPCION DE DISEÑO: " + nombreopcionactual);
            #endregion

            #region FASE
            Parameter parafase = elem_pickobject.get_Parameter(BuiltInParameter.PHASE_CREATED);
            ElementId id_fase = parafase.AsElementId();
            Phase fase = doc.GetElement(id_fase) as Phase;
            string nombredefase = string.Empty;
            if (fase == null)
            {
                nombredefase = "NO TIENE FASE";
            }
            else
            {
                nombredefase = fase.Name;
            }
            sbuilder.AppendLine("FASE: " + nombredefase);
            #endregion

            TaskDialog.Show("PROISAC", sbuilder.ToString());
        }
        public void C5_M2_DatoParametroID()
        {
            #region VARIABLES INDISPENSABLES

            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            IList<Reference> listadorefs = new List<Reference>();
            try
            {
                listadorefs = uidoc.Selection.PickObjects(ObjectType.Element, "SELECCIONA VARIOS ELEMENTOS");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return;
            }
            #endregion

            Transaction tr = new Transaction(doc, "COPIAR ID");
            tr.Start();
            int contadorasignacion = 0;
            for (int i = 0; i < listadorefs.Count; i++)
            {
                Reference ref_actual = listadorefs[i];
                Element e_actual = doc.GetElement(ref_actual);
                Parameter para_id = e_actual.get_Parameter(BuiltInParameter.ID_PARAM);
                if (para_id == null)
                {
                    continue; // PASAR AL SIGUIENTE ELEMENTO DEL FOR DE LA LISTA
                }
                else
                {
                    int dato_id = para_id.AsElementId().IntegerValue;
                    Parameter parametropersonalizado = e_actual.LookupParameter("ID");
                    if (parametropersonalizado == null)
                    {
                        continue;
                    }
                    else
                    {
                        parametropersonalizado.Set(dato_id.ToString());//UNICA LINEA QUE ESTA MODIFICANDO EL MODELO
                        contadorasignacion++;
                    }
                }
            }
            tr.Commit();

            TaskDialog.Show("PROISAC", "SE ESCRIBIO EL ID DE " + contadorasignacion.ToString() + " ELEMENTOS");
        }
        public void C5_M3_BorrarElementos()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            #region COLLECTOR PARA OBTENER TODOS LOS MUROS
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            #endregion

            List<ElementId> id_murosborrar = new List<ElementId>();

            foreach (Element ee in collector)
            {
                Wall muro = ee as Wall;
                Parameter para_alineamiento = muro.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
                if (para_alineamiento == null) { continue; }
                int resultadoalineamiento = para_alineamiento.AsInteger();
                if (resultadoalineamiento == 0)//FINISH FACE INTERIOR
                {
                    id_murosborrar.Add(muro.Id);
                }

            }

            Transaction tr = new Transaction(doc, "BORRAR MUROS");
            tr.Start();
            doc.Delete(id_murosborrar);
            tr.Commit();

            TaskDialog.Show("PROISAC", "SE BORRARON " + id_murosborrar.Count.ToString() + " MUROS");
        }
        public void C5_M4_CREARMUROS()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level nivel1 = collector.First() as Level;

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level nivel2 = collector2.Cast<Level>().Where<Level>(l => l.Name == "01 - Entry Level").FirstOrDefault();

            Transaction tr = new Transaction(doc, "CREARMURO");
            tr.Start();

            for (int i = 0; i < 100; i++)
            {
                #region MURO RECTANGULAR

                //				XYZ p1 = new XYZ(100*i,100*i,0 );// 		0,0,0    	100,100,0      	200,200,0
                //				XYZ p2 = new XYZ(100*(i+1),100*(i+1),0);// 100,100,0	200,200,0		300,300,0
                //				Line linea = Line.CreateBound(p1,p2);
                //				Wall muro = Wall.Create(doc,linea,nivel2.Id,false);
                #endregion

                #region MURO NO RECTANGULAR
                XYZ puntocentral = new XYZ(5 * i, 0, 0);
                XYZ p1, p2, p3, p4;
                p1 = puntocentral.Add(new XYZ(1, 0, 4));
                p2 = puntocentral.Add(new XYZ(3, 0, 7));
                p3 = puntocentral.Add(new XYZ(5, 0, 4));
                p4 = puntocentral.Add(new XYZ(3, 0, 1));
                Curve c1 = Line.CreateBound(p1, p2);
                Curve c2 = Line.CreateBound(p2, p3);
                Curve c3 = Line.CreateBound(p3, p4);
                Curve c4 = Line.CreateBound(p4, p1);
                IList<Curve> curvasmuro = new List<Curve> { c1, c2, c3, c4 };
                Wall muronuevo = Wall.Create(doc, curvasmuro, false);
                #endregion

                System.Windows.Forms.Application.DoEvents();
                doc.Regenerate();
                uidoc.RefreshActiveView();
                System.Windows.Forms.Application.DoEvents();
            }
            tr.Commit();

        }
        public void C6_M1_CrearPisos()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            #region OBTENER UN NIVEL
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level nivel1 = collector.ToElements().First() as Level;
            #endregion

            #region TIPO DE PISO
            FilteredElementCollector collectortipopiso = new FilteredElementCollector(doc).OfClass(typeof(FloorType));
            FloorType floortype = collectortipopiso.FirstElement() as FloorType;
            #endregion

            XYZ p1, p2, p3, p4;
            p1 = new XYZ(100, 0, 0);
            p2 = new XYZ(200, 0, 0);
            p3 = new XYZ(200, 150, 0);
            p4 = new XYZ(150, 20, 0);
            Curve c1 = Line.CreateBound(p1, p2);
            Curve c2 = Line.CreateBound(p2, p3);
            Curve c3 = Line.CreateBound(p3, p4);
            Curve c4 = Line.CreateBound(p4, p1);

            CurveArray profile_floor = new CurveArray();
            profile_floor.Append(c1);
            profile_floor.Append(c2);
            profile_floor.Append(c3);
            profile_floor.Append(c4);

            #region Crear PISO
            Transaction tr = new Transaction(doc, "CREAR PISOS");
            tr.Start();
            Floor piso = null;// doc.Create.NewFloor(profile_floor, floortype, nivel1, false);  error por diferencia de versiones 
            #region RAILING
            CurveLoop cloop = new CurveLoop();
            cloop.Append(c1);
            cloop.Append(c2);
            cloop.Append(c3);
            cloop.Append(c4);
            Railing.Create(doc, cloop, new FilteredElementCollector(doc).OfClass(typeof(RailingType)).FirstElementId(), nivel1.Id);
            #endregion

            doc.Regenerate();
            uidoc.RefreshActiveView();
            TaskDialog.Show("PROISAC", piso.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble().ToString());
            tr.Commit();
            #endregion

        }
        public void C6_M2_EditarPisos()
        {
            #region VARIABLES INDISPENSABLES


            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            Reference ref_elemento = null;
            try
            {
                ref_elemento = uidoc.Selection.PickObject(ObjectType.Element, new FiltrosSeleccion.FloorSelectionFilter(), "SELECCIONA UN ELEMENTO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return;
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_elemento);
            #endregion

            Floor piso = elem_pickobject as Floor;

            Transaction tr = new Transaction(doc, "EDITAR PISO");
            tr.Start();
            SlabShapeEditor sseditor = piso.SlabShapeEditor;
            sseditor.Enable();

            //			for (int i = 0; i < 5; i++)
            //			{
            //				foreach (SlabShapeCrease crease in sseditor.SlabShapeCreases)
            //				{
            //					try
            //					{
            //						Curve curvacrease = crease.Curve;
            //						XYZ p0 = curvacrease.GetEndPoint(0);
            //						XYZ p1 = curvacrease.GetEndPoint(1);
            //						XYZ puntomedio = p0.Add(p1).Divide(2);
            //						Random random = new Random();
            //						XYZ puntonuevo = puntomedio.Add(new XYZ(0,0,random.Next(-1,1)));
            //						sseditor.DrawPoint(puntonuevo);
            //					}
            //					catch{}
            //				}
            //				doc.Regenerate();uidoc.RefreshActiveView();
            //			}

            TaskDialog tdialog = new TaskDialog("PROISAC");
            tdialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            tdialog.MainIcon = TaskDialogIcon.TaskDialogIconError;
            tdialog.MainInstruction = "¿QUIERE ELEGIR SOPORTES PARA LA LOSA?";

            TaskDialogResult tdresult = tdialog.Show();
            if (tdresult == TaskDialogResult.Yes)
            {
                IList<Element> listadosoportes = uidoc.Selection.PickElementsByRectangle(new FiltrosSeleccion.FramingSelectionFilter(), "SELECCIONA LOS SOPORTES");
                foreach (Element esoporte in listadosoportes)
                {
                    LocationCurve lcurve = esoporte.Location as LocationCurve;
                    Line linea = lcurve.Curve as Line;
                    sseditor.PickSupport(linea);
                }

            }
            tr.Commit();

        }
        public void C6_M3_InsertarFamilias()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            #region OBTENER PUNTOS DESDE LOS ROOMS
            Autodesk.Revit.DB.Architecture.RoomFilter filterroom = new Autodesk.Revit.DB.Architecture.RoomFilter();
            FilteredElementCollector collectorroom = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filterroom).WhereElementIsNotElementType();
            IList<Element> listadorooms = collectorroom.ToElements();
            #endregion

            FilteredElementCollector collectorfamiliasfurniture = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol));
            FamilySymbol symbolofurniture = null;

            foreach (FamilySymbol fsymbol in collectorfamiliasfurniture)
            {
                if (fsymbol.Category.Name == "Furniture")
                {
                    symbolofurniture = fsymbol;
                    break; //ROMPER FOR FOREACH WHILE SWITCH
                }
            }

            TransactionGroup tgroup = new TransactionGroup(doc, "GRUPO CREAR FAMILIAS");
            tgroup.Start();
            if (symbolofurniture.IsActive == false) { symbolofurniture.Activate(); }
            IList<ElementId> listaseleccion = new List<ElementId>();
            foreach (Element eroom in listadorooms)
            {
                Transaction tr = new Transaction(doc, "CREAR FAMILIAS EN ROOMS");
                tr.Start();
                Room ambient = eroom as Room;
                LocationPoint lpoint = ambient.Location as LocationPoint;
                if (lpoint == null) { continue; }
                XYZ puntoroom = lpoint.Point;
                FamilyInstance finstance = doc.Create.NewFamilyInstance(puntoroom, symbolofurniture, ambient.Level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                //				doc.Regenerate();uidoc.RefreshActiveView();
                listaseleccion.Add(finstance.Id);
                System.Windows.Forms.Application.DoEvents();
                tr.Commit();
            }

            tgroup.Assimilate();
            uidoc.Selection.SetElementIds(listaseleccion);
            uidoc.ShowElements(listaseleccion); // MUESTRE LA FAMILIAS NUEVAS

        }
        public void C6_M4_InsertarPuertas()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            #region COLLECTOR DE TODOS LOS MUROS DE LA VISTA
            FilteredElementCollector collectormuros = new FilteredElementCollector(doc, doc.ActiveView.Id);
            IList<Element> listadomuros = collectormuros.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
            #endregion

            #region COLLECTOR PUERTA FAMILYSYMBOL
            FamilySymbol symbolopuerta = null;
            FilteredElementCollector collectorpuertas = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_Doors);
            symbolopuerta = collectorpuertas.FirstOrDefault() as FamilySymbol;
            if (symbolopuerta == null) { return; }
            #endregion

            Transaction tr = new Transaction(doc, "CREAR PUERTAS");
            tr.Start();
            if (symbolopuerta.IsActive == false) { symbolopuerta.Activate(); }
            IList<ElementId> listaseleccion = new List<ElementId>();
            foreach (Element emuro in listadomuros)
            {
                Wall muro = emuro as Wall;
                if (muro.CurtainGrid != null) { continue; } //CASO QUE MURO ES MURO CORTINA

                LocationCurve lcurve = emuro.Location as LocationCurve;
                Curve curvamuro = lcurve.Curve;

                #region SOLO COLOCAR PUERTAS EN MUROS DE MAS DE 3 METROS
                double largomuropies = curvamuro.Length;
#pragma warning disable CS0618 // 'UnitUtils.ConvertFromInternalUnits(double, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ConvertFromInternalUnits(double, ForgeTypeId)` overload instead.'
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
                double largomurometros = Util.FootToMetre(largomuropies);
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning restore CS0618 // 'UnitUtils.ConvertFromInternalUnits(double, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ConvertFromInternalUnits(double, ForgeTypeId)` overload instead.'
                if (largomurometros < 3) { continue; }
                #endregion


                XYZ p0 = curvamuro.GetEndPoint(0);
                XYZ p1 = curvamuro.GetEndPoint(1);
                XYZ puntomedio = p0.Add(p1).Divide(2);
                FamilyInstance finstance = doc.Create.NewFamilyInstance(puntomedio, symbolopuerta, muro, doc.ActiveView.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                listaseleccion.Add(finstance.Id);
            }
            tr.Commit();
            uidoc.Selection.SetElementIds(listaseleccion);
            uidoc.ShowElements(listaseleccion); // MUESTRE LA FAMILIAS NUEVAS

        }
        public void C6_M5_TipoInsercion()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            #region Obtenemos todos los familysymbol del documento
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol)); // OBTENGO TODOS LOS SIMBOLOS DE FAMILIA DEL PROYECTO
            #endregion

            foreach (FamilySymbol fsymbol in collector)
            {
                Family familia = fsymbol.Family;
                Category categoria = familia.FamilyCategory;
                if (categoria == null) { continue; }
                int hosttype = familia.get_Parameter(BuiltInParameter.FAMILY_HOSTING_BEHAVIOR).AsInteger();
                if (hosttype == 1)
                {
                    System.Diagnostics.Debug.Print(categoria.Name + ": " + fsymbol.Name + " NECESITA UN HOST");
                }
                else if (hosttype == 0)
                {
                    System.Diagnostics.Debug.Print(categoria.Name + ": " + fsymbol.Name + " NO NECESITA UN HOST");
                }
                else
                {
                    System.Diagnostics.Debug.Print(hosttype.ToString());
                }
            }
            System.Diagnostics.Debug.Print("FIN DE IMPRESION");
        }
        public void C6_M6_CreacionTextos()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            FilteredElementCollector collectorpuertas = new FilteredElementCollector(doc, doc.ActiveView.Id);
            IList<Element> listadopuertas = collectorpuertas.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements();

            FilteredElementCollector collectortextotipos = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType));
            TextNoteType tipotexto = collectortextotipos.First() as TextNoteType;

            TextNoteOptions tno = new TextNoteOptions();
            tno.HorizontalAlignment = HorizontalTextAlignment.Left;
            tno.VerticalAlignment = VerticalTextAlignment.Middle;
            tno.KeepRotatedTextReadable = true;
            tno.TypeId = tipotexto.Id;

            IList<ElementId> listaseleccion = new List<ElementId>();
            Transaction tr = new Transaction(doc, "CREAR TEXTOS");
            tr.Start();
            foreach (Element epuerta in collectorpuertas)
            {
                FamilyInstance finstance = epuerta as FamilyInstance;
                LocationPoint lpoin = finstance.Location as LocationPoint;
                if (lpoin == null) { continue; }
                TextNote texto = TextNote.Create(doc, doc.ActiveView.Id, lpoin.Point, "EJEMPLO:" + finstance.Name, tno);
                Leader l1 = texto.AddLeader(TextNoteLeaderTypes.TNLT_ARC_R);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_ARC_R);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_ARC_L);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_ARC_L);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                texto.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                listaseleccion.Add(texto.Id);
            }
            tr.Commit();
            uidoc.Selection.SetElementIds(listaseleccion);
            uidoc.ShowElements(listaseleccion); // MUESTRE LA FAMILIAS NUEVAS
        }


        public void C6_M7_CrearEtiquetas()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            FilteredElementCollector collectorpuertas = new FilteredElementCollector(doc, doc.ActiveView.Id);
            IList<Element> listadopuertas = collectorpuertas.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements();

            IList<ElementId> listaseleccion = new List<ElementId>();
            Transaction tr = new Transaction(doc, "CREAR TEXTOS");
            tr.Start();
            foreach (Element epuerta in collectorpuertas)
            {
                FamilyInstance finstance = epuerta as FamilyInstance;
                LocationPoint lpoin = finstance.Location as LocationPoint;
                if (lpoin == null) { continue; }
                XYZ ubicacion = lpoin.Point;
                Reference referencianueva = new Reference(epuerta);
                IndependentTag newtag = IndependentTag.Create(doc, doc.ActiveView.Id, referencianueva, true, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, ubicacion);
                listaseleccion.Add(newtag.Id);
            }
            tr.Commit();
            uidoc.Selection.SetElementIds(listaseleccion);
            uidoc.ShowElements(listaseleccion); // MUESTRE LA FAMILIAS NUEVAS
        }
        public void C7_M1_LineasDetalle()
        {
            #region VARIABLES INDISPENSABLES


            #endregion
            Transaction tr = new Transaction(doc, "LINEAS");
            tr.Start();

            #region EJES
            XYZ peje0 = new XYZ(-100, 0, 0);
            XYZ peje1 = new XYZ(100, 0, 0);
            Curve curvaejeX = Line.CreateBound(peje0, peje1);
            DetailCurve lineaejeX = doc.Create.NewDetailCurve(doc.ActiveView, curvaejeX);

            peje0 = new XYZ(0, -100, 0);
            peje1 = new XYZ(0, 100, 0);
            Curve curvaejeY = Line.CreateBound(peje0, peje1);
            DetailCurve lineaejeY = doc.Create.NewDetailCurve(doc.ActiveView, curvaejeY);
            #endregion

            #region GRAFICAR ECUACION
            XYZ punto_anterior = new XYZ(0, 0, 0);
            for (int i = 1; i < 100; i++)
            {
                double x = -i;
                //				double y = x*x;
                double y = Math.Tan(x);
                //double y = Math.Sin(x);
                XYZ nuevopunto = new XYZ(x, y, 0);
                Curve curva = Line.CreateBound(punto_anterior, nuevopunto);
                DetailCurve linea1 = doc.Create.NewDetailCurve(doc.ActiveView, curva);
                punto_anterior = nuevopunto;
            }
            #endregion
            tr.Commit();
        }
        public void C7_M2_Transformacion()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            Element elementopicked = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, "Selecciona Cualquier elemento"));
            List<Element> elementoscopiados = new List<Element>();

            TransactionGroup tgroup = new TransactionGroup(doc, "SUPERTRANSFORMACION");
            tgroup.Start();

            using (Transaction tr = new Transaction(doc, "TRANSFORM MIRROR"))
            {
                tr.Start();
                if (ElementTransformUtils.CanMirrorElement(doc, elementopicked.Id))
                {
                    Plane planomirror = Plane.CreateByNormalAndOrigin(new XYZ(1, 0, 0), new XYZ(-100, 0, 0));
                    ElementTransformUtils.MirrorElement(doc, elementopicked.Id, planomirror);
                    doc.Regenerate(); uidoc.RefreshActiveView();
                    System.Windows.Forms.Application.DoEvents();
                }
                tr.Commit();
            }

            using (Transaction tr = new Transaction(doc, "TRANSFORM COPIAR ROTAR"))
            {
                tr.Start();
                for (int i = 1; i < 31; i++)
                {
                    ICollection<ElementId> copia = ElementTransformUtils.CopyElement(doc, elementopicked.Id, new XYZ(0, -200 * i, 0));
                    Element ecopiado = doc.GetElement(copia.First());
                    elementoscopiados.Add(ecopiado);
                    BoundingBoxXYZ bbox = ecopiado.get_BoundingBox(doc.ActiveView);
                    XYZ puntocentro = (bbox.Max + bbox.Min) / 2;
                    XYZ puntocentro2 = puntocentro + new XYZ(0, 0, 1);
                    Line linearotacion = Line.CreateBound(puntocentro, puntocentro2);
                    ElementTransformUtils.RotateElement(doc, ecopiado.Id, linearotacion, i);
                    doc.Regenerate(); uidoc.RefreshActiveView();
                    System.Windows.Forms.Application.DoEvents();
                }
                tr.Commit();
            }
            using (Transaction tr = new Transaction(doc, "TRANSFORM MOVE"))
            {
                tr.Start();

                foreach (Element e in elementoscopiados)
                {
                    ElementTransformUtils.MoveElement(doc, e.Id, new XYZ(500, 0, 0));
                    doc.Regenerate(); uidoc.RefreshActiveView();
                }
                tr.Commit();
            }
            tgroup.Assimilate();
        }
        public void C7_M3_CrearMurosFalla()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level nivel1 = collector.First() as Level;

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level nivel2 = collector2.Cast<Level>().Where<Level>(l => l.Name == "01 - Entry Level").FirstOrDefault();

            #region ASIGNAR CONTROLADOR DE FALLAS
            UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            doc.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);
            //uidoc.Application.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(utilfallas.SuprimirCuadroDialogo);
            #endregion


            Transaction tr = new Transaction(doc, "CREARMURO");
            tr.Start();

            for (int i = 0; i < 10; i++)
            {
                #region MURO RECTANGULAR
                XYZ p1 = new XYZ(100 * i, 100 * i, 0);
                XYZ p2 = new XYZ(100 * (i + 1) + 10, 100 * (i + 1) + 10, 0);
                Line linea = Line.CreateBound(p1, p2);
                Wall muro = Wall.Create(doc, linea, nivel2.Id, false);
                #endregion

                doc.Regenerate();
                uidoc.RefreshActiveView();
                System.Windows.Forms.Application.DoEvents();
            }
            tr.Commit();
            TaskDialog.Show("PROISAC", "SE CREARON 10 MUROS");

            #region REMOVER EL CONTROLADOR DE FALLAS Y DIALOGOS
            doc.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);
            uidoc.Application.DialogBoxShowing -= new EventHandler<DialogBoxShowingEventArgs>(utilfallas.SuprimirCuadroDialogo);
            #endregion
        }
        public void C7_M4_InfoAmbientes()
        {
            #region VARIABLES INDISPENSABLES


            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region OBTENER ROOMS
            Autodesk.Revit.DB.Architecture.RoomFilter filterroom = new Autodesk.Revit.DB.Architecture.RoomFilter();
            FilteredElementCollector collectorroom = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filterroom).WhereElementIsNotElementType();
            IList<Element> listadorooms = collectorroom.ToElements();
            #endregion

            List<string> listadonombres = new List<string>();
            List<string> listadoareas = new List<string>();
            List<string> listadopuntos = new List<string>();

            foreach (Element eroom in listadorooms)
            {
                Room ambiente = eroom as Room;
                if (ambiente.Area == 0) { continue; }


                LocationPoint lpoint = ambiente.Location as LocationPoint;
                XYZ punto = lpoint.Point;

                #region DATOS PARA FORMULARIO
                listadonombres.Add(ambiente.Name);
                listadoareas.Add(ambiente.Area.ToString());
                listadopuntos.Add(punto.ToString());
                #endregion

                sbuilder.AppendLine("Nombre: " + ambiente.Name);
                sbuilder.AppendLine("Posicion: " + punto.ToString());

                IList<IList<BoundarySegment>> segments = ambiente.GetBoundarySegments(new SpatialElementBoundaryOptions());
                if (segments != null)
                {
                    sbuilder.AppendLine("LOOPS: " + segments.Count.ToString());

                    foreach (IList<BoundarySegment> segmentlist in segments)
                    {
                        sbuilder.AppendLine("----Loop:" + segmentlist.Count.ToString());
                    }
                }

            }
            //			TaskDialog.Show("REVIT API",sbuilder.ToString ());
            //	Form_Ambientes formulario = new Form_Ambientes(listadonombres,listadoareas,listadopuntos);//CREAR EL FORMULARIO
            //.ShowDialog();

        }

        public static string NOMBREROOMS = string.Empty;
        public static int contadornumeroroom = 1;

        public void C7_M5_CrearAmbientes()
        {
            #region VARIABLES INDISPENSABLES


            #endregion

            View vista = doc.ActiveView;
            if (vista.ViewType != ViewType.FloorPlan) { return; }

            Level nivel = vista.GenLevel;
            PlanTopology planTOPO = doc.get_PlanTopology(nivel);
            Phase faseactual = doc.GetElement(vista.get_Parameter(BuiltInParameter.VIEW_PHASE).AsElementId()) as Phase;

            //Form_CrearRooms formulario = new Form_CrearRooms();
            //formulario.ShowDialog();

            using (Transaction tr = new Transaction(doc, "CREAR ROOMS"))
            {
                tr.Start();
                foreach (PlanCircuit circuit in planTOPO.Circuits)
                {
                    if (circuit != null)
                    {
                        Room ambienteenfase = doc.Create.NewRoom(faseactual);
                        ambienteenfase.Name = NOMBREROOMS;
                        ambienteenfase.Number = contadornumeroroom.ToString();
                        contadornumeroroom++;

                        Room ambientefinal = doc.Create.NewRoom(ambienteenfase, circuit);
                        doc.Regenerate(); uidoc.RefreshActiveView();
                    }
                }

                tr.Commit();
            }

        }
        public void C8_M1_GeometriaAmbientes()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            View vista = doc.ActiveView;
            if (vista.ViewType != ViewType.FloorPlan) { return; }

            #region Listado ROOMS
            Autodesk.Revit.DB.Architecture.RoomFilter filterroom = new Autodesk.Revit.DB.Architecture.RoomFilter();
            FilteredElementCollector collectorroom = new FilteredElementCollector(doc, doc.ActiveView.Id);
            IList<Element> listadorooms = collectorroom.WherePasses(filterroom).ToElements();
            #endregion
            StringBuilder sbuilder = new StringBuilder();

            foreach (Element element in listadorooms)
            {
                Room eroom = element as Room;
                if (eroom.Area == 0) { continue; }
                sbuilder.AppendLine(eroom.Name);

                Options opt = new Options();
                opt.ComputeReferences = false;
                opt.DetailLevel = ViewDetailLevel.Coarse;
                opt.IncludeNonVisibleObjects = false;

                GeometryElement geo = eroom.get_Geometry(opt);

                foreach (GeometryObject obj in geo)
                {
                    Solid solid = obj as Solid;
                    if (null != solid && solid.Faces.Size > 0)
                    {
                        sbuilder.AppendLine("---" + "SOLIDO " + solid.Volume.ToString());
                        sbuilder.AppendLine("---" + solid.Faces.Size.ToString() + " CARAS");
                        int contadorcaras = 1;
                        foreach (Face face in solid.Faces) //CARAS DEL ELEMENTO
                        {
                            sbuilder.AppendLine("------CARA " + contadorcaras.ToString()); contadorcaras++;
                            EdgeArrayArray eaa = face.EdgeLoops;
                            sbuilder.AppendLine("--------LOOPS " + eaa.Size.ToString());
                            foreach (EdgeArray erray in eaa) //ARRAY DE BORDES DEL ELEMENTO
                            {
                                sbuilder.AppendLine("-----------" + erray.Size.ToString() + " BORDES");
                                foreach (Edge edge in erray) //BORDES DEL ELEMENTO
                                {

                                    double longitudenmetros = Util.FootToMetre(edge.ApproximateLength);

                                    longitudenmetros = Math.Round(longitudenmetros, 2);
                                    sbuilder.AppendLine("-----------" + "BORDE " + longitudenmetros.ToString());
                                }
                            }
                        }


                    }

                }




            }
            TaskDialog.Show("PROISAC", sbuilder.ToString());

        }
        public void C8_M2_ModificarPuntoAmbiente()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            View vista = doc.ActiveView;
            if (vista.ViewType != ViewType.FloorPlan) { return; }

            #region Listado ROOMS
            Autodesk.Revit.DB.Architecture.RoomFilter filterroom = new Autodesk.Revit.DB.Architecture.RoomFilter();
            FilteredElementCollector collectorroom = new FilteredElementCollector(doc, doc.ActiveView.Id);
            IList<Element> listadorooms = collectorroom.WherePasses(filterroom).ToElements();
            #endregion

            using (Transaction tr = new Transaction(doc, "CENTRO ROOM"))
            {
                tr.Start();
                foreach (Element element in listadorooms)
                {
                    Room eroom = element as Room;
                    if (eroom.Area == 0) { continue; }

                    Options opt = new Options();
                    opt.ComputeReferences = false;
                    opt.DetailLevel = ViewDetailLevel.Coarse;
                    opt.IncludeNonVisibleObjects = false;

                    GeometryElement geo = eroom.get_Geometry(opt);
                    Face carainferior = null;
                    foreach (GeometryObject obj in geo)
                    {
                        Solid solid = obj as Solid;
                        if (null != solid && solid.Faces.Size > 0)
                        {
                            foreach (Face face in solid.Faces) //CARAS DEL ELEMENTO
                            {
                                PlanarFace planarface = face as PlanarFace;//CARA PLANA
                                if (planarface != null)
                                {
                                    if (planarface.FaceNormal.Z == -1)
                                    {
                                        carainferior = face;
                                        goto SALIDA;
                                    }
                                }
                            }
                        }
                    }
                SALIDA://PUNTO DE SALTO MANUAL
                    if (carainferior == null) { continue; }
                    BoundingBoxUV bboxUV = carainferior.GetBoundingBox();
                    UV center = (bboxUV.Min + bboxUV.Max) / 2;
                    XYZ locationnuevo = carainferior.Evaluate(center);
                    LocationPoint puntoroom = eroom.Location as LocationPoint;
                    puntoroom.Point = locationnuevo;
                }
                doc.Regenerate();
                List<Element> listaetiquetas = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsNotElementType().ToList();
                foreach (Element e_etiqueta in listaetiquetas)
                {
                    RoomTag tag = e_etiqueta as RoomTag;
                    Room ambiente = tag.Room;
                    LocationPoint point_tag = tag.Location as LocationPoint;

                    LocationPoint point_ambiente = ambiente.Location as LocationPoint;
                    point_tag.Point = point_ambiente.Point;
                }
                tr.Commit();
            }
        }
        public void C8_M3_BusquedaVistas()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            StringBuilder sbuilder = new StringBuilder();
            #endregion
            FilteredElementCollector collectorvistas = new FilteredElementCollector(doc);
            collectorvistas.OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();

            Dictionary<string, List<View>> diccionariovista = new Dictionary<string, List<View>>();

            foreach (View vista in collectorvistas)
            {
                if (vista.IsTemplate) { continue; }
                ViewType tipodevista = vista.ViewType;
                string nombretipovista = tipodevista.ToString(); //FLOORPLAN CEILINGPLAN 3DVIEW
                                                                 //FLOORPLAN
                if (diccionariovista.ContainsKey(nombretipovista))//SI CONTIENE
                {
                    List<View> listaexistente = diccionariovista[nombretipovista];
                    listaexistente.Add(vista);
                    diccionariovista[nombretipovista] = listaexistente;
                }
                else //NO CONTIENE
                {
                    List<View> listanueva = new List<View>();
                    listanueva.Add(vista);
                    diccionariovista.Add(nombretipovista, listanueva);
                }
            }

            sbuilder.AppendLine("CANTIDAD DE LISTAS EN EL DICCIONARIO: " + diccionariovista.Count.ToString());

            foreach (var item in diccionariovista)
            {
                string tipov = item.Key;
                List<View> listav = item.Value;
                sbuilder.AppendLine("TIPO DE VISTA:" + tipov);
                foreach (View vv in listav)
                {
                    sbuilder.AppendLine("----" + vv.Name);
                    sbuilder.AppendLine("--------ESCALA:" + vv.Scale.ToString());
                    sbuilder.AppendLine("--------NIVEL DE DETALLE:" + vv.DetailLevel.ToString());
                    sbuilder.AppendLine("--------LAMINA:" + vv.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NAME).AsString());
                }

            }





            TaskDialog.Show("REVIT API", sbuilder.ToString());



        }
        public void C8_M4_CrearVistas()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            View vistaactual = doc.ActiveView;
            if (vistaactual.ViewType != ViewType.FloorPlan) { return; }
            Level nivelactual = vistaactual.GenLevel;

            #region OBTENER TODOS LOS MUROS DE LA VISTA DE PLANTA
            FilteredElementCollector collector1 = new FilteredElementCollector(doc, vistaactual.Id);
            collector1.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            #endregion

            IList<Element> listatiposdevista = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).ToElements();
            ViewFamilyType familiafloorplan = null;
            ViewFamilyType familia3D = null;
            ViewFamilyType familiaSection = null;

            foreach (Element evista in listatiposdevista)
            {
                ViewFamilyType vfamilytype = evista as ViewFamilyType;
                switch (vfamilytype.ViewFamily)
                {
                    case ViewFamily.FloorPlan:
                        familiafloorplan = vfamilytype;
                        break;
                    case ViewFamily.ThreeDimensional:
                        familia3D = vfamilytype;
                        break;
                    case ViewFamily.Section:
                        familiaSection = vfamilytype;
                        break;
                }
            }

            using (Transaction tr = new Transaction(doc, "CREAR VISTAS MUROS"))
            {
                tr.Start();
                foreach (Element emuro in collector1)
                {
                    Wall muro = emuro as Wall;
                    BoundingBoxXYZ cajamuro = muro.get_BoundingBox(null);//NULL SIGNIFICA QUE OBTIENE LA CAJA SIN IMPORTAR LA VISTA 3D

                    ViewPlan vistaplanta = ViewPlan.Create(doc, familiafloorplan.Id, nivelactual.Id);
                    vistaplanta.Name = "PLANTA MURO " + muro.Id.IntegerValue.ToString();
                    if (vistaplanta.CropBoxActive == false)
                    {
                        vistaplanta.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION).Set(1);
                    }
                    vistaplanta.CropBox = cajamuro;
                    vistaplanta.DetailLevel = ViewDetailLevel.Fine;
                    vistaplanta.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    vistaplanta.Scale = 50;

                    View3D vista3D = View3D.CreateIsometric(doc, familia3D.Id);
                    vista3D.Name = "VISTA 3D MURO " + muro.Id.IntegerValue.ToString();
                    vista3D.SetSectionBox(cajamuro);
                    vista3D.DetailLevel = ViewDetailLevel.Fine;
                    vista3D.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    vista3D.Scale = 50;

                    ViewSection vistaSeccion = ViewSection.CreateSection(doc, familiaSection.Id, cajamuro);
                    vistaSeccion.Name = "SECCION " + muro.Id.IntegerValue.ToString();
                    vistaSeccion.DetailLevel = ViewDetailLevel.Fine;
                    vistaSeccion.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    vistaSeccion.Scale = 50;
                }




                tr.Commit();
            }



        }
        public void C8_M5_EsconderMurosVistas()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            #region OBTENER TODOS LOS MUROS
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            #endregion

            IList<Element> listadovistas = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).ToElements();

            //			using (Transaction tr = new Transaction(doc,"OCULTAR MUROS"))
            //			{
            //				tr.Start();
            //				foreach (Element evista in listadovistas)
            //				{
            //					View vista = evista as View;
            //					List<ElementId> listadoesconder = new List<ElementId>();
            //					foreach (Element emuro in collector1)
            //					{
            //						if(emuro.CanBeHidden(vista))
            //						{
            //							listadoesconder.Add(emuro.Id);
            //						}
            //					}
            //					vista.HideElements(listadoesconder);
            //				}
            //				tr.Commit();
            //			}
            using (Transaction tr = new Transaction(doc, "MOSTRAR MUROS"))
            {
                tr.Start();
                foreach (Element evista in listadovistas)
                {
                    View vista = evista as View;
                    List<ElementId> listadomostrar = new List<ElementId>();
                    foreach (Element emuro in collector1)
                    {
                        if (emuro.IsHidden(vista))
                        {
                            listadomostrar.Add(emuro.Id);
                        }
                    }
                    vista.UnhideElements(listadomostrar);
                }
                tr.Commit();
            }
        }
        public void C8_M6_Materiales()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            StringBuilder sbuilder = new StringBuilder();

            List<Material> listadomateriales = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().ToList();

            for (int i = 0; i < listadomateriales.Count; i++)
            {
                Material material = listadomateriales[i];
                string datosmaterial = InformacionMaterial(material);
                sbuilder.AppendLine(datosmaterial);
            }
            TaskDialog.Show("REVIT API", sbuilder.ToString());
        }

        private string InformacionMaterial(Material material)
        {
            StringBuilder sbuildermaterial = new StringBuilder();
            sbuildermaterial.AppendLine("Material: " + material.Name);
            string colormaterial = string.Format("Color: Red[{0}] Green[{1}] Blue[{2}]", material.Color.Red, material.Color.Green, material.Color.Blue);
            sbuildermaterial.AppendLine("COLOR MATERIAL: " + colormaterial);
            Document doc = material.Document;

            //FOREGROUND CUT PATTERN
            FillPatternElement cutForegroundPattern = doc.GetElement(material.CutForegroundPatternId) as FillPatternElement;
            if (cutForegroundPattern != null)
            {
                sbuildermaterial.AppendLine("---Cut Foreground Pattern: " + cutForegroundPattern.Name);
                string colorpattern = string.Format("---Cut Foreground Pattern Color: Red[{0}]; Green[{1}]; Blue[{2}]", material.CutForegroundPatternColor.Red, material.CutForegroundPatternColor.Green, material.CutForegroundPatternColor.Blue);
                sbuildermaterial.AppendLine(colorpattern);
            }

            //foreground surface pattern y color
            FillPatternElement surfaceForegroundPattern = material.Document.GetElement(material.SurfaceForegroundPatternId) as FillPatternElement;
            if (null != surfaceForegroundPattern)
            {
                sbuildermaterial.AppendLine("---Surface Foreground Pattern: " + surfaceForegroundPattern.Name);
                sbuildermaterial.AppendLine(string.Format("---Surface Foreground Pattern Color: Red[{0}]; Green[{1}]; Blue[{2}]", material.SurfaceForegroundPatternColor.Red, material.SurfaceForegroundPatternColor.Green, material.SurfaceForegroundPatternColor.Blue));
            }

            //background cut pattern y color
            FillPatternElement cutBackgroundPattern = material.Document.GetElement(material.CutBackgroundPatternId) as FillPatternElement;
            if (null != cutBackgroundPattern)
            {
                sbuildermaterial.AppendLine("---Cut Background Pattern: " + cutBackgroundPattern.Name);
                sbuildermaterial.AppendLine(string.Format("---Cut Background Pattern Color: Red[{0}]; Green[{1}]; Blue[{2}]", material.CutBackgroundPatternColor.Red, material.CutBackgroundPatternColor.Green, material.CutBackgroundPatternColor.Blue));
            }

            //background surface pattern y color
            FillPatternElement surfaceBackgroundPattern = material.Document.GetElement(material.SurfaceBackgroundPatternId) as FillPatternElement;
            if (null != surfaceBackgroundPattern)
            {
                sbuildermaterial.AppendLine("---Surface Background Pattern: " + surfaceBackgroundPattern.Name);
                sbuildermaterial.AppendLine(string.Format("---Surface Background Pattern Color: Red[{0}]; Green[{1}]; Blue[{2}]", material.SurfaceBackgroundPatternColor.Red, material.SurfaceBackgroundPatternColor.Green, material.SurfaceBackgroundPatternColor.Blue));
            }

            //PROPIEDADES SHADING SMOOTHNESS Y TRANSPARENCIA
            int shininess = material.Shininess;
            sbuildermaterial.AppendLine("---Shininess: " + shininess);
            int smoothness = material.Smoothness;
            sbuildermaterial.AppendLine("---Smoothness: " + smoothness);
            int transparency = material.Transparency;
            sbuildermaterial.AppendLine("---Transparency: " + transparency);


            return sbuildermaterial.ToString();
        }
        public void C8_M7_CrearMateriales()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            StringBuilder sbuilder = new StringBuilder();
            Random random = new Random();//GENERAD DATOS ALEATORIOS

            using (Transaction tr = new Transaction(doc, "CREAR MATERIALS"))
            {
                tr.Start();
                for (int i = 0; i < 100; i++)
                {
                    string nombrematerial = "MATERIAL NUEVO " + i.ToString();
                    ElementId idmaterialnuevo = Material.Create(doc, nombrematerial);
                    Material materialnuevo = doc.GetElement(idmaterialnuevo) as Material;
                    Color colormaterial = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    materialnuevo.Color = colormaterial;
                    string datocolormaterial = string.Format("COLORES MATERIAL: RED {0} ; GREEN {1}; BLUE {2}", colormaterial.Red.ToString(), colormaterial.Green.ToString(), colormaterial.Blue.ToString());
                    sbuilder.AppendLine(nombrematerial);
                    sbuilder.AppendLine(datocolormaterial);
                    sbuilder.AppendLine();
                }
                tr.Commit();
            }
            TaskDialog.Show("REVIT", sbuilder.ToString());

        }
        public void C9_M1_MaterialesPropiedades()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            StringBuilder sbuilder = new StringBuilder();

            List<Material> listadomateriales = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().ToList();

            for (int i = 0; i < listadomateriales.Count; i++)
            {
                Material material = listadomateriales[i];
                sbuilder.AppendLine(material.Name);
                //				#region PARAMETROS
                //				foreach (Parameter paramat in material.Parameters)
                //				{
                //					sbuilder.AppendLine("-----"+paramat.Definition.Name+":   "+paramat.AsValueString() +"   "+paramat.Definition.ParameterType.ToString()+"    "+paramat.Definition.UnitType.ToString());
                //				}
                //				#endregion
                ElementId id_appearaceasset = material.AppearanceAssetId;
                ElementId id_structuralasset = material.StructuralAssetId;
                ElementId id_thermalasset = material.ThermalAssetId;
                if (id_appearaceasset.IntegerValue > 0)
                {
                    AppearanceAssetElement appearaceasset = doc.GetElement(id_appearaceasset) as AppearanceAssetElement;
                    sbuilder.AppendLine("--" + "APARIENCIA ASSET:" + appearaceasset.Name);
                    Asset renderasset = appearaceasset.GetRenderingAsset();
                    sbuilder.AppendLine("--" + "RENDER ASSET:" + renderasset.Name);
                    string ruta = rutaimagen(renderasset);
                    if (ruta != string.Empty)
                    {
                        sbuilder.AppendLine("--" + "IMAGEN:" + ruta);
                    }
                }

                if (id_structuralasset.IntegerValue > 0)
                {
                    PropertySetElement pse = doc.GetElement(id_structuralasset) as PropertySetElement;
                    if (pse != null)
                    {
                        StructuralAsset structuralasset = pse.GetStructuralAsset();
                        sbuilder.AppendLine("--" + "STRUCTURAL ASSET:" + structuralasset.Name + "   " + structuralasset.Behavior.ToString());
                    }
                }
                if (id_thermalasset.IntegerValue > 0)
                {
                    PropertySetElement pse = doc.GetElement(id_thermalasset) as PropertySetElement;
                    if (pse != null)
                    {
                        ThermalAsset thermalasset = pse.GetThermalAsset();
                        sbuilder.AppendLine("--" + "THERMAL ASSET:" + thermalasset.Name + "   " + thermalasset.Behavior.ToString());
                    }
                }

            }

            TaskDialog.Show("REVIT API", sbuilder.ToString());
        }
        private string rutaimagen(Asset asset)
        {
            string ruta = string.Empty;
            int size = asset.Size;
            for (int assetIdx = 0; assetIdx < size; assetIdx++)
            {
                AssetProperty aProperty = asset.Get(assetIdx);

                if (aProperty.NumberOfConnectedProperties < 1)
                    continue;

                // Find first connected property.
                // Should work for all current (2018) schemas.
                // Safer code would loop through all connected
                // properties based on the number provided.

                Asset connectedAsset = aProperty.GetConnectedProperty(0) as Asset;

                // We are only checking for bitmap connected assets.

                if (connectedAsset.Name == "UnifiedBitmapSchema")
                {
                    // This line is 2018.1 & up because of the
                    // property reference to UnifiedBitmap
                    // .UnifiedbitmapBitmap.  In earlier versions,
                    // you can still reference the string name
                    // instead: "unifiedbitmap_Bitmap"

                    AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;

                    // This will be a relative path to the
                    // built -in materials folder, addiitonal
                    // render appearance folder, or an
                    // absolute path.

                    ruta = String.Format("{0} from {2}: {1}", aProperty.Name, path.Value, connectedAsset.LibraryName);
                }
            }
            return ruta;
        }
        public void C9_M3_MaterialesProyecto()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            StringBuilder sbuilder = new StringBuilder();

            #region OBTENER TODOS LOS ELEMENTOS DEL MODELO
            List<Element> elementostodo = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElements().ToList();
            #endregion

            #region DICCIONARIOS
            Dictionary<string, double> dic_datosmatVOLUMEN = new Dictionary<string, double>();
            Dictionary<string, double> dic_datosmatAREA = new Dictionary<string, double>();
            Dictionary<string, List<string>> dic_datosmatPOS = new Dictionary<string, List<string>>();
            Dictionary<string, Material> listamaterialesusados = new Dictionary<string, Material>();
            #endregion

            foreach (Element eee in elementostodo)
            {
                if (eee.ViewSpecific) { continue; }//SALTANDO TEXTOS COTAS ETIQUETAS LINEAS DETALLE DETAIL ITEMS REGIONES

                #region POSICION
                BoundingBoxXYZ bbox = eee.get_BoundingBox(null);
                XYZ puntocentral = null;
                if (bbox != null)
                {
                    puntocentral = (bbox.Max + bbox.Min) / 2;
                }
                #endregion

                ICollection<ElementId> id_materiales = eee.GetMaterialIds(false);
                if (id_materiales.Count == 0) { continue; }
                foreach (ElementId idm in id_materiales)
                {
                    Material material = doc.GetElement(idm) as Material;
                    string nombrematerial = material.Name;
                    double volumen = 0;
                    double area = 0;
                    if (dic_datosmatVOLUMEN.ContainsKey(nombrematerial)) // YA EXISTE
                    {
                        volumen = dic_datosmatVOLUMEN[nombrematerial] + eee.GetMaterialVolume(idm);
                        dic_datosmatVOLUMEN[nombrematerial] = volumen;

                        area = dic_datosmatAREA[nombrematerial] + eee.GetMaterialArea(idm, false);
                        dic_datosmatAREA[nombrematerial] = area;

                        List<string> listaposicion = dic_datosmatPOS[nombrematerial];
                        if (puntocentral != null)
                        {
                            listaposicion.Add(eee.Name + " " + puntocentral.ToString());
                            dic_datosmatPOS[nombrematerial] = listaposicion;
                        }
                        else
                        {
                            listaposicion.Add("eee.Name" + "  NO POSICION");
                            dic_datosmatPOS[nombrematerial] = listaposicion;
                        }
                    }
                    else//PRIMERA VEZ QUE AGREGA EL ELEMENTO AL DICCIONARIO
                    {
                        volumen = eee.GetMaterialVolume(idm);
                        dic_datosmatVOLUMEN.Add(nombrematerial, volumen);

                        area = eee.GetMaterialArea(idm, false);
                        dic_datosmatAREA.Add(nombrematerial, area);

                        List<string> listaposicion = new List<string>();
                        if (puntocentral != null)
                        {
                            listaposicion.Add(eee.Name + " " + puntocentral.ToString());
                            dic_datosmatPOS.Add(nombrematerial, listaposicion);
                        }
                        else
                        {
                            listaposicion.Add("eee.Name" + "  NO POSICION");
                            dic_datosmatPOS.Add(nombrematerial, listaposicion);
                        }
                    }
                }
            }

            var targetVolumen = dic_datosmatVOLUMEN.ToList();
            var targetAREA = dic_datosmatAREA.ToList();
            var targetPOSICION = dic_datosmatPOS.ToList();

            for (int i = 0; i < dic_datosmatVOLUMEN.Count; i++)
            {
                string nombre = targetVolumen[i].Key;
                double volumen = dic_datosmatVOLUMEN[nombre];
                double area = dic_datosmatAREA[nombre];
                List<string> listapos = dic_datosmatPOS[nombre];

                sbuilder.AppendLine(nombre);
                sbuilder.AppendLine("--VOLUMEN: " + volumen.ToString());
                sbuilder.AppendLine("--AREA: " + area.ToString());
                foreach (string sspos in listapos)
                {
                    sbuilder.AppendLine("-----" + sspos);
                }
            }

            TaskDialog.Show("REVIT API", sbuilder.ToString());
        }
        public void C9_M4_LeerTXT()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            System.Windows.Forms.OpenFileDialog dialogoabrir = new System.Windows.Forms.OpenFileDialog();
            dialogoabrir.Filter = "ARCHIVOS TXT|*.txt|ARCHIVOS CSV|*.csv|ARCHIVOS XLSX|*.xlsx";
            dialogoabrir.Multiselect = false;
            dialogoabrir.Title = "ABRIR ARCHIVOS TXT PARA CREAR MATERIALES PARA REVIT";

            System.Windows.Forms.DialogResult resultado = dialogoabrir.ShowDialog();
            if (resultado == System.Windows.Forms.DialogResult.Cancel) { return; }

            string nombrearchivo = dialogoabrir.FileName;

            string[] lineastexto = System.IO.File.ReadAllLines(nombrearchivo);
            StringBuilder sbuilder = new StringBuilder();
            using (Transaction tr = new Transaction(doc, "CREAR MAT TXT"))
            {
                tr.Start();
                Random random = new Random();
                foreach (string dato in lineastexto)
                {
                    try
                    {
                        ElementId idmaterialnuevo = Material.Create(doc, dato);
                        Material materialnuevo = doc.GetElement(idmaterialnuevo) as Material;
                        Color colormaterial = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                        materialnuevo.Color = colormaterial;
                        string datocolormaterial = string.Format("COLORES MATERIAL: RED {0} ; GREEN {1}; BLUE {2}", colormaterial.Red.ToString(), colormaterial.Green.ToString(), colormaterial.Blue.ToString());
                        sbuilder.AppendLine(dato);
                        sbuilder.AppendLine(datocolormaterial);
                        sbuilder.AppendLine();
                    }
                    catch { }
                }
                tr.Commit();
            }
            TaskDialog.Show("REVIT API", sbuilder.ToString());

        }
        public void C9_M5_EscribirTXT()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            System.Windows.Forms.SaveFileDialog dialogosalvar = new System.Windows.Forms.SaveFileDialog();
            dialogosalvar.Filter = "ARCHIVOS TXT|*.txt|ARCHIVOS CSV|*.csv";
            dialogosalvar.Title = " SALVAR ARCHIVO TXT CON MATERIALES";
            dialogosalvar.OverwritePrompt = false; //PREGUNTAR AL SOBREESCRIBIR

            System.Windows.Forms.DialogResult resultado = dialogosalvar.ShowDialog();
            if (resultado == System.Windows.Forms.DialogResult.Cancel) { return; }

            List<Material> listamat_proyecto = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().ToList();
            List<string> nombresmateriales = new List<string>();

            foreach (Material matproy in listamat_proyecto)
            {
                nombresmateriales.Add(matproy.Name);
            }

            string rutaarchivo = dialogosalvar.FileName;

            System.IO.File.WriteAllLines(rutaarchivo, nombresmateriales);

            System.Diagnostics.Process.Start(rutaarchivo);//ABRE EL ARCHIVO CON EL PROGRAMA POR DEFECTO

        }

        public void C9_M6_LeerExcel()
        {
#if false
            #region DEFINICION DE VARIABLES DE DOCUMENTO

			Document doc = uidoc.Document;
            #endregion
			System.Windows.Forms.OpenFileDialog dialogoabrir = new System.Windows.Forms.OpenFileDialog();
			dialogoabrir.Filter = "ARCHIVOS XLSX|*.xlsx|ARCHIVOS XLS|*.xls";
			dialogoabrir.Multiselect = false;
			dialogoabrir.Title = "ABRIR ARCHIVOS EXCEL PARA CREAR MATERIALES PARA REVIT";

			System.Windows.Forms.DialogResult resultado = dialogoabrir.ShowDialog();
			if (resultado == System.Windows.Forms.DialogResult.Cancel) { return; }

			string nombrearchivo = dialogoabrir.FileName;

            #region CREAR OBJECTOS DE EXCEL
			Excel.Application xlApp = new Excel.ApplicationClass();
			Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(nombrearchivo);
			xlApp.Visible = false;
            #endregion

			Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1] as Excel.Worksheet;//HEMOS OBTENIDO LA HOJA 1
			Excel.Range xlRange = xlWorksheet.UsedRange;//RANGO DE CELDAS USADAS

			int filas = xlRange.Rows.Count;
			int columnas = xlRange.Columns.Count;
			Random ranran = new Random();
			using (Transaction tr = new Transaction(doc, "MAT DE EXCEL"))
			{
				tr.Start();
				for (int i = 1; i <= filas; i++)
				{
					for (int j = 1; j <= columnas; j++)
					{
						Excel.Range rangocelda = xlWorksheet.Cells[i, j] as Excel.Range;
						object valorcelda = rangocelda.Value;
						if (valorcelda != null)
						{
							string dato = valorcelda.ToString();
							try
							{
								ElementId idmaterialnuevo = Material.Create(doc, dato);
								Material materialnuevo = doc.GetElement(idmaterialnuevo) as Material;
								Color colormaterial = new Color((byte)ranran.Next(0, 255), (byte)ranran.Next(0, 255), (byte)ranran.Next(0, 255));
								materialnuevo.Color = colormaterial;
							}
							catch { }
						}
					}
				}
				tr.Commit();
			}
			xlApp.Quit();
#endif
        }


        public void C9_M7_EscribirExcel()
        {
#if false

            #region DEFINICION DE VARIABLES DE DOCUMENTO

			Document doc = uidoc.Document;
            #endregion
			
			System.Windows.Forms.SaveFileDialog dialogosalvar = new System.Windows.Forms.SaveFileDialog();
			dialogosalvar.Filter ="ARCHIVOS XLSX|*.xlsx";
			dialogosalvar.Title ="SALVAR ARCHIVOS EXCEL CON MATERIALES DE REVIT";
			dialogosalvar.OverwritePrompt = false; //PREGUNTAR AL SOBREESCRIBIR
			
			System.Windows.Forms.DialogResult resultado = dialogosalvar.ShowDialog();
			if(resultado == System.Windows.Forms.DialogResult.Cancel){return;}
			
			List<Material> listamat_proyecto = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().ToList();
			
            #region CREAR OBJETOS EXCEL
			Excel.Application xlApp = new Excel.ApplicationClass();
			object misValue = System.Reflection.Missing.Value;//VALOR VACIO PARA DATOS DE OFFICE
			Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(misValue);
			xlApp.Visible = true;
            #endregion
			
			Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1] as Excel.Worksheet; //HEMOS OBTENIDO LA HOJA 1
			
			for (int i = 0; i < listamat_proyecto.Count; i++)
			{
				Material matproy = listamat_proyecto[i];
				xlWorksheet.Cells[i+1,1] = matproy.Name;
				System.Threading.Thread.Sleep(100);
			}
			xlWorkbook.SaveAs(dialogosalvar.FileName,Excel.XlFileFormat.xlOpenXMLWorkbook,misValue,misValue,misValue,misValue,Excel.XlSaveAsAccessMode.xlExclusive,misValue,misValue,misValue,misValue,misValue);
			
#endif
        }


        public void C9_M8_ConfigProyecto()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            StringBuilder sbuilder = new StringBuilder();
            #endregion

            DisplayUnit dunit = doc.DisplayUnitSystem;
            sbuilder.AppendLine(dunit.ToString());

            Units unidades = doc.GetUnits();
            sbuilder.AppendLine(unidades.DecimalSymbol.ToString());
//#pragma warning disable CS0618 // 'Units.GetFormatOptions(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetFormatOptions(ForgeTypeId)` overload instead.'
//#pragma warning disable CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
//            FormatOptions foarea = unidades.GetFormatOptions(UnitType.UT_Area);
//#pragma warning restore CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
//#pragma warning restore CS0618 // 'Units.GetFormatOptions(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetFormatOptions(ForgeTypeId)` overload instead.'
//#pragma warning disable CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'
//            sbuilder.AppendLine(foarea.DisplayUnits.ToString());
//#pragma warning restore CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'

            TaskDialog.Show("PROISAC", sbuilder.ToString());

            //ProjectInfo pinformacion2 = doc.ProjectInformation;
            ProjectInfo pinformacion = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ProjectInformation).ToElements().First() as ProjectInfo;

            using (Transaction tr = new Transaction(doc, "CONFIGPROYECTO"))
            {
                tr.Start();
                Units unew = new Units(UnitSystem.Imperial);
#pragma warning disable CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'Units.SetFormatOptions(UnitType, FormatOptions)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetFormatOptions(ForgeTypeId, FormatOptions)` overload instead.'
#pragma warning disable CS0618 // 'FormatOptions.FormatOptions(DisplayUnitType)' is obsolete: 'This constructor is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `FormatOptions(ForgeTypeId)` overload instead.'
                //unew.SetFormatOptions(UnitType.UT_Area, new FormatOptions(FormatOptions.  DisplayUnitType.DUT_SQUARE_FEET));
#pragma warning restore CS0618 // 'FormatOptions.FormatOptions(DisplayUnitType)' is obsolete: 'This constructor is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `FormatOptions(ForgeTypeId)` overload instead.'
#pragma warning restore CS0618 // 'Units.SetFormatOptions(UnitType, FormatOptions)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetFormatOptions(ForgeTypeId, FormatOptions)` overload instead.'
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning restore CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
                doc.SetUnits(unew);

                pinformacion.ClientName = "ROLANDO HIJAR";
                pinformacion.BuildingName = "EDIFICIO BAXTER";
                pinformacion.Name = "PROYECTO 4F";

                pinformacion.Author = "RKHP";

                BasePoint puntobase = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ProjectBasePoint).First() as BasePoint;

#pragma warning disable CS0618 // 'UnitUtils.ConvertToInternalUnits(double, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ConvertToInternalUnits(double, ForgeTypeId)` overload instead.'
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
              //  puntobase.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM).Set(UnitUtils.ConvertToInternalUnits(300000, DisplayUnitType.DUT_METERS));
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning restore CS0618 // 'UnitUtils.ConvertToInternalUnits(double, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ConvertToInternalUnits(double, ForgeTypeId)` overload instead.'

                string direccionjournal = doc.Application.RecordingJournalFilename;
                doc.Application.WriteJournalComment("CURSO REVIT API 4ta EDICION", true);
                doc.Application.WriteJournalComment("SE CAMBIO LA CONFIGURACION DEL PROYECTO", true);
                System.Diagnostics.Process.Start(direccionjournal);
                tr.Commit();

            }

        }
        public void C10_M1_TopoCreate()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region OBTENER LA DATA DE PUNTOS. MINIMO 3 PUNTOS
            List<XYZ> puntostopo = new List<XYZ>();
            puntostopo.Add(new XYZ(10, 20, 3));
            puntostopo.Add(new XYZ(100, 200, 5));
            puntostopo.Add(new XYZ(140, 300, 4));
            puntostopo.Add(new XYZ(500, 600, 15));
            puntostopo.Add(new XYZ(200, 300, 0));
            #endregion

            List<ElementId> listaseleccion = new List<ElementId>();
            using (Transaction tr = new Transaction(doc, "NEWTOPO"))
            {
                tr.Start();
                TopographySurface toponueva = TopographySurface.Create(doc, puntostopo);
                listaseleccion.Add(toponueva.Id);
                tr.Commit();
            }
            uidoc.Selection.SetElementIds(listaseleccion);
            uidoc.ShowElements(listaseleccion);
        }
        public void C10_M2_TopoEdit()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            StringBuilder sbuilder = new StringBuilder();
            #endregion

            FiltrosSeleccion.TopoSelectionFilter filtro = new FiltrosSeleccion.TopoSelectionFilter();
            Reference reftopo = null;
            try
            {
                reftopo = uidoc.Selection.PickObject(ObjectType.Element, filtro, "Selecciona una topografia");
            }
            catch { return; }

            TopographySurface topografia = doc.GetElement(reftopo) as TopographySurface;

            using (TopographyEditScope editortopografia = new TopographyEditScope(doc, "EDITSCOPE"))
            {
                editortopografia.Start(topografia.Id);
                Random random = new Random();
                using (Transaction tr = new Transaction(doc, "EDITTOPO"))
                {
                    tr.Start();
                    IList<XYZ> puntostopo = topografia.GetPoints();
                    IList<XYZ> puntosborrar = new List<XYZ>();
                    IList<XYZ> puntosnuevos = new List<XYZ>();
                    foreach (XYZ punto in puntostopo)
                    {
                        if (topografia.IsBoundaryPoint(punto)) { continue; }//SI ES PUNTO DE BORDE
                        puntosborrar.Add(punto);
                        XYZ puntonuevo = new XYZ(punto.X, punto.Y, punto.Z + random.Next(-2, 2));
                        puntosnuevos.Add(puntonuevo);
                    }
                    topografia.DeletePoints(puntosborrar);
                    doc.Regenerate();
                    topografia.AddPoints(puntosnuevos);
                    tr.Commit();
                }
                editortopografia.Commit(new FailuresTopo());
            }


        }

        #region FAILURE PROCESSOR TOPOGRAFIA
        public class FailuresTopo : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                return FailureProcessingResult.Continue;
            }
        }
        #endregion



        public void C10_M3_MEP()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            StringBuilder sbuilder = new StringBuilder();
            #endregion

            FiltrosSeleccion.TopoSelectionFilter filtro = new FiltrosSeleccion.TopoSelectionFilter();
            Reference reftopo = null;
            try
            {
                reftopo = uidoc.Selection.PickObject(ObjectType.Element, filtro, "Selecciona una topografia");
            }
            catch { return; }

            TopographySurface topografia = doc.GetElement(reftopo) as TopographySurface;

            IList<XYZ> puntosboundary = topografia.GetBoundaryPoints();

            #region OBTENER INFORMACION PARA CREAR ELEMENTOS MEP
            ElementId pipetypeid = new FilteredElementCollector(doc).OfClass(typeof(PipeType)).First().Id;
            ElementId systempipetypeid = new FilteredElementCollector(doc).OfClass(typeof(PipingSystemType)).First().Id;

            ElementId ducttypeid = new FilteredElementCollector(doc).OfClass(typeof(DuctType)).First().Id;
            ElementId systemdycttypeid = new FilteredElementCollector(doc).OfClass(typeof(MEPSystemType)).OfCategory(BuiltInCategory.OST_DuctSystem).First().Id;

            ElementId cabletraytypeid = new FilteredElementCollector(doc).OfClass(typeof(CableTrayType)).First().Id;

            ElementId conduittypeid = null;//new FilteredElementCollector(doc).OfClass(typeof(ConduitType)).First().Id;
            IList<Element> listatiposconduits = new FilteredElementCollector(doc).OfClass(typeof(ConduitType)).ToElements();
            foreach (Element tconduit in listatiposconduits)
            {
                ConduitType tipoc = tconduit as ConduitType;
                if (tipoc.Name == "ACERO") { conduittypeid = tipoc.Id; break; }
            }
            //

            ElementId levelid = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First().Id;
            #endregion


            using (Transaction tr = new Transaction(doc, "MEP"))
            {
                tr.Start();
                for (int i = 0; i < puntosboundary.Count - 1; i++)
                {
                    XYZ punto1 = puntosboundary[i];
                    XYZ punto2 = puntosboundary[i + 1];
                    //					Pipe tuberia = Pipe.Create(doc,systempipetypeid, pipetypeid,levelid,punto1,punto2);
                    //					tuberia.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM).Set(1.96850393700787);

                    //					Duct ducto = Duct.Create(doc,systemdycttypeid,ducttypeid,levelid,punto1,punto2);
                    //					ducto.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).Set(10);

                    //					CableTray cabletray = CableTray.Create(doc,cabletraytypeid,punto1,punto2,levelid);

                    Conduit conduit = Conduit.Create(doc, conduittypeid, punto1, punto2, levelid);

                }
                tr.Commit();
            }



        }
        public void C10_M4_CONECTARTUBOS()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            Reference reftub1 = uidoc.Selection.PickObject(ObjectType.Element, "TBO1");
            Reference reftub2 = uidoc.Selection.PickObject(ObjectType.Element, "TBO2");

            Pipe pipe1 = doc.GetElement(reftub1) as Pipe;
            Pipe pipe2 = doc.GetElement(reftub2) as Pipe;

            ConnectorManager cman1 = pipe1.ConnectorManager;
            ConnectorManager cman2 = pipe2.ConnectorManager;

            using (Transaction tr = new Transaction(doc, "CODO"))
            {
                tr.Start();
                foreach (Connector cone1 in cman1.Connectors)
                {
                    if (cone1.IsConnected) { continue; }
                    XYZ puntoconector1 = cone1.Origin;
                    XYZ vectorZcone1 = cone1.CoordinateSystem.BasisZ;

                    XYZ puntoaux1 = puntoconector1 + vectorZcone1;
                    ModelCurve mc1 = modelarlineas(doc, puntoconector1, puntoaux1);

                    foreach (Connector cone2 in cman2.Connectors)
                    {
                        if (cone2.IsConnected) { continue; }
                        XYZ puntoconector2 = cone2.Origin;
                        XYZ vectorZcone2 = cone2.CoordinateSystem.BasisZ;

                        XYZ puntoaux2 = puntoconector2 + vectorZcone2;
                        ModelCurve mc2 = modelarlineas(doc, puntoconector2, puntoaux2);
                        //						mc1.GeometryCurve.Intersect(mc2.GeometryCurve);
                        //0.20 m
                        double distancia = puntoconector1.DistanceTo(puntoconector2);
                        if (distancia > Util.CmToFoot(20))
                        {
                            continue;
                        }
                        try
                        {
                            FamilyInstance codo = doc.Create.NewElbowFitting(cone1, cone2);
                        }
                        catch
                        {
                        }
                    }


                }



                tr.Commit();
            }


        }


        public ModelCurve modelarlineas(Document doc, XYZ p1, XYZ p2)
        {
            Curve curvamodel = Line.CreateBound(p1, p2);
            if (curvamodel.Length < doc.Application.ShortCurveTolerance) { return null; }
            XYZ vectorlinea = new XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            Plane plano = Plane.CreateByNormalAndOrigin(p1.CrossProduct(p2), p1);
            SketchPlane sk = SketchPlane.Create(doc, plano);
            ModelCurve mc = doc.Create.NewModelCurve(curvamodel, sk);
            return mc;
        }


        public void C10_M5_AUTOCAD()
        {
            #region VARIABLES INDISPENSABLES


            StringBuilder sbuilder = new StringBuilder();
            #endregion

            #region PASO 1 OBTENER LA REFERENCIA AL ELEMENTO
            Reference ref_muro = null;
            try
            {
                ref_muro = uidoc.Selection.PickObject(ObjectType.Element, "SELECCIONA UN MURO");
            }
            catch (Exception)
            {
                TaskDialog.Show("PROISAC", "USTED NO SELECCIONO NADA. SALIENDO DEL PROGRAMA");
                return;
            }
            #endregion

            #region PASO 2 OBTENER EL ELEMENTO
            Element elem_pickobject = doc.GetElement(ref_muro);
            sbuilder.AppendLine(elem_pickobject.Name);
            #endregion

            #region PASO 3 OPCIONES DE GEOMETRIA
            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = false;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            #endregion

            GeometryElement geo = elem_pickobject.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects

            ElementId pipetypeid = new FilteredElementCollector(doc).OfClass(typeof(PipeType)).First().Id;
            ElementId systempipetypeid = new FilteredElementCollector(doc).OfClass(typeof(PipingSystemType)).First().Id;
            ElementId levelid = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First().Id;

            Transaction tr = new Transaction(doc, "PIPES FROM CAD");
            tr.Start();
            List<string> tiposelementos = new List<string>();
            foreach (GeometryObject obj in geo)
            {

                #region GEOMETRYINSTANCE
                if (obj is GeometryInstance)
                {
                    GeometryInstance instanciaanidada = obj as GeometryInstance;
                    sbuilder.AppendLine(instanciaanidada.Symbol.Name);
                    foreach (GeometryObject obj2 in instanciaanidada.GetInstanceGeometry())
                    {
                        string tipo = obj2.GetType().ToString();
                        if (!tiposelementos.Contains(tipo)) { tiposelementos.Add(tipo); }
                        if (obj2 is Line)
                        {
                            //sbuilder.AppendLine("-----------" + "LINEA ");
                            Line linea = obj2 as Line;
                            GraphicsStyle estilo = doc.GetElement(linea.GraphicsStyleId) as GraphicsStyle;
                            string nombrecapa = estilo.GraphicsStyleCategory.Name;

                            System.Diagnostics.Debug.Print(nombrecapa);
                            XYZ p0 = linea.GetEndPoint(0);
                            XYZ p1 = linea.GetEndPoint(1);
                            if (p0.DistanceTo(p1) > doc.Application.ShortCurveTolerance)
                            {
                                modelarlineas(doc, linea.GetEndPoint(0), linea.GetEndPoint(1));
                                Pipe tuberia = Pipe.Create(doc, systempipetypeid, pipetypeid, levelid, linea.GetEndPoint(0), linea.GetEndPoint(1));
                            }
                        }

                    }
                }
                #endregion
            }
            string lineatipos = string.Join(Environment.NewLine, tiposelementos);
            sbuilder.AppendLine(lineatipos);
            TaskDialog.Show("PROISAC", sbuilder.ToString());
            tr.Commit();

        }
        public void C10_M6_EventosAplicacion()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            doc_global = doc;

            #endregion

            uidoc.Application.Application.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(Controladorevento_DocumentOpened);
            uidoc.Application.Application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(Controladorevento_DocumentChanged);

        }

        public void quitareventosaplicacion()
        {

            Document doc = uidoc.Document;
            uidoc.Application.Application.DocumentOpened -= new EventHandler<DocumentOpenedEventArgs>(Controladorevento_DocumentOpened);
            uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(Controladorevento_DocumentChanged);
        }



        public void Controladorevento_DocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            Document doc = args.Document;

            TaskDialog.Show("PROISAC", doc.PathName);

        }
        public void Controladorevento_DocumentChanged(object sender, DocumentChangedEventArgs args)
        {
            List<ElementId> elementosmodificados = args.GetModifiedElementIds().ToList();
            StringBuilder sbuilder = new StringBuilder();
            if (elementosmodificados.Count > 0)
            {
                foreach (ElementId id in elementosmodificados)
                {
                    Element e_mod = doc_global.GetElement(id);
                    sbuilder.AppendLine(e_mod.Name);
                }
            }

            TaskDialog.Show("PROISAC", sbuilder.ToString());
        }

        public void C11_M1_UpdaterMuroRegistrar()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion
            UpdaterOpeningMuros updateopen = new ThisApplication.UpdaterOpeningMuros(_Application.ActiveAddInId);
            UpdaterRegistry.RegisterUpdater(updateopen, doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

            #region ELEMENTOS DISPARADORES
            ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeGeometry());
            #endregion
            TaskDialog.Show("PROISAC", "SE REGISTRO EL UPDATER");
        }

        public void C11_M2_UpdaterMuroBorrar()
        {
            #region DEFINICION DE VARIABLES DE DOCUMENTO

            Document doc = uidoc.Document;
            #endregion

            try
            {
                UpdaterOpeningMuros updateopen = new ThisApplication.UpdaterOpeningMuros(_Application.ActiveAddInId);
                UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                TaskDialog.Show("PROISAC", "SE BORRO EL UPDATER");
            }
            catch { }

        }


        public class UpdaterOpeningMuros : IUpdater
        {
            static AddInId _appId;
            static UpdaterId _updaterId;

            public UpdaterOpeningMuros(AddInId id)//codigo interno del Updater 145689
            {
                _appId = id;
                _updaterId = new UpdaterId(_appId, new Guid("451040aa-0a80-475c-af28-5112e6964d09"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO
            }
            public void Execute(UpdaterData data)
            {
                //				TaskDialog.Show("PROISAC","SE AGREGO UN MURO");
                Document doc = data.GetDocument();

                foreach (ElementId id in data.GetAddedElementIds())
                {
                    Wall muro = doc.GetElement(id) as Wall;
                    BoundingBoxXYZ cajamuro = muro.get_BoundingBox(null);
                    XYZ pmax = cajamuro.Max;
                    XYZ pmin = cajamuro.Min;
                    XYZ vector = (pmax - pmin).Normalize();//NORMALIZE HACE QUE EL VECTOR SEA UNITARIO
                    double distancia = pmax.DistanceTo(pmin);
                    XYZ puntoopen1 = pmin + vector * distancia / 3;
                    XYZ puntoopen2 = pmin + vector * 2 * distancia / 3;
                    doc.Create.NewOpening(muro, puntoopen1, puntoopen2);
                }
                foreach (ElementId id in data.GetModifiedElementIds())
                {
                    Wall muro = doc.GetElement(id) as Wall;
                    BoundingBoxXYZ cajamuro = muro.get_BoundingBox(null);
                    XYZ pmax = cajamuro.Max;
                    XYZ pmin = cajamuro.Min;
                    XYZ vector = (pmax - pmin).Normalize();//NORMALIZE HACE QUE EL VECTOR SEA UNITARIO
                    double distancia = pmax.DistanceTo(pmin);
                    XYZ puntoopen1 = pmin + vector * distancia / 3;
                    XYZ puntoopen2 = pmin + vector * 2 * distancia / 3;
                    doc.Create.NewOpening(muro, puntoopen1, puntoopen2);
                }
                UpdaterOpeningMuros updateopen = new ThisApplication.UpdaterOpeningMuros(doc.Application.ActiveAddInId);
                try
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                }
                catch { }
                try
                {
                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeGeometry());
                    #endregion
                }
                catch { }
            }
            public string GetUpdaterName()
            {
                return "UPDATER OPENING MURO";
            }

            public string GetAdditionalInformation()
            {
                return "CURSO REVIT API 3ra edicion";
            }

            public ChangePriority GetChangePriority()
            {
                return ChangePriority.FloorsRoofsStructuralWalls;
            }

            public UpdaterId GetUpdaterId()
            {
                return _updaterId;
            }
        }


        public void C11_M3_INTERCEPTOR()
        {

            Document doc = uidoc.Document;

            XYZ punto = uidoc.Selection.PickPoint("PICAR PUNTO DE ORIGEN PARA EL INTERCEPTOR");
            XYZ vectordireccion = new XYZ(1, 1, 0.2);

            View3D vista3D = doc.ActiveView as View3D;
            if (vista3D == null) { return; }
            List<ElementId> listaseleccion = new List<ElementId>();

            #region PRIMER ELEMENTO ENCONTRADO
            //			ReferenceIntersector ref_intersector = new ReferenceIntersector(vista3D);
            //			ReferenceWithContext ref_encontrada = ref_intersector.FindNearest(punto,vectordireccion);
            //			if(ref_encontrada !=null)
            //			{
            //				Transaction tr = new Transaction(doc,"CLASH UNO");
            //				tr.Start();
            //				Element elemento = doc.GetElement(ref_encontrada.GetReference());
            //				listaseleccion.Add(elemento.Id);
            //				modelarlineas(doc,punto,ref_encontrada.GetReference().GlobalPoint);
            //				tr.Commit();
            //			}
            #endregion

            #region VARIOS ELEMENTOS CHOCADOS
            ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter filtrolosas = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            LogicalOrFilter filtro_o = new LogicalOrFilter(filtromuros, filtrolosas);
            ReferenceIntersector ref_intersector = new ReferenceIntersector(filtro_o, FindReferenceTarget.Element, vista3D);
            IList<ReferenceWithContext> referenciasencontradas = ref_intersector.Find(punto, vectordireccion);
            Transaction tr = new Transaction(doc, "CLASH VARIOS");
            tr.Start();
            double distanciamaxima = double.NegativeInfinity;
            ElementId idelementolejano = null;
            XYZ puntolejano = new XYZ();
            foreach (ReferenceWithContext refcontext in referenciasencontradas)
            {
                Element elemento = doc.GetElement(refcontext.GetReference());

                double distancia = refcontext.Proximity;//DISTANCIA DEL PUNTO ORIGEN AL PUNTO EN QUE CHOCA AL ELEMENTO
                if (distancia > distanciamaxima)
                {
                    distanciamaxima = distancia;
                    idelementolejano = elemento.Id;
                    puntolejano = refcontext.GetReference().GlobalPoint;
                }
            }
            listaseleccion.Add(idelementolejano);
            listaseleccion.Add(modelarlineas(doc, punto, puntolejano).Id); ;
            tr.Commit();
            #endregion

            uidoc.Selection.SetElementIds(listaseleccion);
        }
    }
}
namespace EspacioExterior
{
    public class nuevaclase
    {
        public void Mi_Segunda_Macro()
        {
            TaskDialog.Show("CURSO REVIT API", "SEGUNDA MACRO");
        }

        private void Tercer_macro()
        {
            TaskDialog.Show("CURSO REVIT API", "TERCERA MACRO");

        }

    }
}