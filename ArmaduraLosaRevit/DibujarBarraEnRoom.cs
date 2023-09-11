using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;

namespace ArmaduraLosaRevit
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class DibujarBarraEnRoom : IExternalCommand
    {
        double angle = 0;
        double espesor = 20;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        //lista con los puntos poligono de los segmentos intersecatados , 4 ptos, inical y final de los dos segmentos
        // [ ptoini1, ptofin1 , ptoini2 , ptofin2]
        //  1 __ 1    sentido horizontal          1 __ 2    sentido vertical
        //  2    2                                1 __ 2 
        public List<XYZ> ListaPtosPoligonoLosa { get; set; }
        //lista con los puntos que circunscribe el area que ocupara la losa
        //0  - 3
        //1  - 2
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }



        private static ExternalCommandData m_revit;
        private static Document doc;
        PathReinforcement m_createdPathReinforcement = null;
        AreaReinforcement m_createdAreaReinforcement = null;
        Room m_roomSelecionado = null;

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


       


            Autodesk.Revit.DB.View view = m_revit.Application.ActiveUIDocument.Document.ActiveView;
            View3D view3D = view as View3D;
            if (null != view3D)
            {
                message += "Only create dimensions in 2D";
                return Autodesk.Revit.UI.Result.Failed;
            }




            // seleccionado vista con nombre  '{3D}'
            View3D elem3d = Util.GetFirstElementOfTypeNamed(doc, typeof(View3D), "{3D}") as View3D;


            // view analizado
            // busca el nivel del pisos analizado
           // ElementId levelId = Util.FindLevelId(doc, view.GenLevel.Name);

            //seleccionar  1 elemto
            var asda = Util.GetSingleSelectedElement(uidoc);

            m_roomSelecionado = null;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
            //selecciona un objeto floor
            Reference r = uidoc.Selection.PickObject(ObjectType.Face, f, "Please select a planar face to define work plane");
            // sirefere3ncia es null salir
            if (r == null)
                return Result.Succeeded;



            return CrearBarra(app, opt, view, elem3d, r);

        }

        private Result CrearBarra(Application app, Options opt, View view, View3D elem3d, Reference r)
        {
            //obtiene una referencia floor con la referencia r
            Floor selecFloor = doc.GetElement(r.ElementId) as Floor;            //obtiene el nivel del la losa
            Level levelLOsa = doc.GetElement(selecFloor.LevelId) as Level;
            //obtiene el pto de seleccion con el mouse sobre la losa
            XYZ pt_selec = r.GlobalPoint;
            UV pt_selecUV = r.UVPoint;


            TipoRefuerzo tiporefuerzo = TipoRefuerzo.PathRefuerza;
            TipoOrientacionBarra tipoOrientacion = TipoOrientacionBarra.Horizontal;
            // obtiene todos los room del nivel analisado
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(SpatialElement));
            IEnumerable<Element> rooms = collector.Where<Element>(e => e is Room && e.LevelId == selecFloor.LevelId);
            //********************************
            //obtiene los putnos de roomm
            ListaPtosPoligonoLosa = ListaPoligonosRooms(rooms, opt, pt_selec, tipoOrientacion);

            //datos internos del room
            ////para metros del room
            if (m_roomSelecionado == null) { return Result.Failed; }
            ParameterSet pars = m_roomSelecionado.Parameters;
            bool result = double.TryParse(ParameterUtil.FindValueParaByName(pars, "Espesor", doc), out espesor);
            bool result2 = double.TryParse(ParameterUtil.FindValueParaByName(pars, "Angulo", doc), out angle);

            if (result2)
            { angle = angle * 180 / Math.PI; }
            string cuantiaH = ParameterUtil.FindValueParaByName(pars, "Cuantia Horizontal", doc);
            string cuantiaV = ParameterUtil.FindValueParaByName(pars, "Cuantia Vertical", doc);
            string cuantiaB = "";
            //-----------------------------------------------------

            if (tipoOrientacion == TipoOrientacionBarra.Horizontal)
            {
                cuantiaB = cuantiaH;
                //nada;
            }
            else if (tipoOrientacion == TipoOrientacionBarra.Vertical)
            {
                cuantiaB = cuantiaV;
                angle = angle + 90;
            }


            if (ListaPtosPoligonoLosa.Count != 4)
            {
                TaskDialog.Show("Error", "Ptos que definen area de refuerzo deber ser 4. Ptos encontrados: " + ListaPtosPoligonoLosa.Count);
                return Result.Failed;
            }
            //*****************************************************************************************************)
            ListaPtosPerimetroBarras = ListaFinal_pto(ListaPtosPoligonoLosa, app, selecFloor, tipoOrientacion);


            //begin Transaction, so action here can be aborted.
            Transaction transaction = new Transaction(doc, "CreatePathReinforcement");
            transaction.Start();



            if (tiporefuerzo == TipoRefuerzo.PathRefuerza && ListaPtosPerimetroBarras.Count > 0)
            {
                //HOOK barra Principal
                RebarHookType tipodeHookStartPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 180 deg.",doc);
                RebarHookType tipodeHookEndPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 180 deg.",doc);
                /// HOOK barra alternativa
                RebarHookType tipodeHookStarAlternativa = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.",doc);
                RebarHookType tipodeHookEndAlternativa = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.",doc);

                // si algunos de los hoiok es null osea no encuentra el tipo o no esta dentro de las familias, crea un tipo nulo
                //throw ("agregar  caso");
                //if (tipodeHookStartPrincipal == null) tipodeHookStartPrincipal = ElementId.InvalidElementId;
                //if (tipodeHookEndPrincipal == null) tipodeHookEndPrincipal = ElementId.InvalidElementId;
                //if (tipodeHookStarAlternativa == null) tipodeHookStarAlternativa = ElementId.InvalidElementId;
                //if (tipodeHookEndAlternativa == null) tipodeHookEndAlternativa = ElementId.InvalidElementId;

                //crea  refuerzo path
                //flip true se dibuja hacia abajo de la curva de trayectoria

                //flip false se dibuja hacia arriba de la curva de trayectoria
                bool aux_flop = true;
                if (angle > 0) aux_flop = false;
                m_createdPathReinforcement = CreatePathReinforcement(ListaPtosPerimetroBarras, aux_flop, doc, selecFloor, cuantiaB);
                if (elem3d != null)
                {
                    //permite que la barra se vea en el 3d como solido
                    m_createdPathReinforcement.SetSolidInView(elem3d, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    m_createdPathReinforcement.SetUnobscuredInView(elem3d, true);
                }




                //Obtiene el largo el ancho del path
                double largobaa = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]);

                // Largo de princial Alternativa. Si es 0 no la activa
                double largoPrincial = largobaa;
                // Largo de barra Alternativa. Si es 0 no la activa 
                double largoAlternative = 0;

                // a) seleciona que barra se botton - inferior
                //b) activa barra alternativa de ser necesario
                //c) asigna largo de barra princiapales y alterniva si corresonde
                //b) asigna largos                
                LayoutRebar_PathReinforcement(ListaPtosPerimetroBarras, 20, largoPrincial, largoAlternative);

                //agrega Hook primario  start
                ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_HOOK_TYPE_1, tipodeHookStartPrincipal.Id);
                //agrega Hook primario  end
                ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1, tipodeHookEndPrincipal.Id);
                //agrega Hook - si esta direccion botton Major activa
                //Botton Major Hook Type
                if (ParameterUtil.FindParaByName(m_createdPathReinforcement, "Alternating Bars").AsInteger() == 1)
                {
                    //agrega Hook alternative  end
                    ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_END_HOOK_TYPE_2, tipodeHookEndAlternativa.Id);
                    //agrega Hook alternative  start
                    ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_HOOK_TYPE_2, tipodeHookStarAlternativa.Id);
                }

                Element elemtoSymboloPath = TiposPathReinSpanSymbol.getPathReinSpanSymbol("M_Path Reinforcement Symbol", doc);

                PathReinSpanSymbol symbolPath = PathReinSpanSymbol.Create(doc, view.Id, new LinkElementId(m_createdPathReinforcement.Id), new XYZ(5, 0, 0), elemtoSymboloPath.Id);


            }
            else if (tiporefuerzo == TipoRefuerzo.AreaRefuerzo && ListaPtosPerimetroBarras.Count > 0)
            {
                IList<Curve> curves = new List<Curve>();
                curves = CrearCurva(ListaPtosPerimetroBarras);

                // encunetra el tipo de hook, se lo asigna a ambos ladios
                RebarHookType tipodeHook = TiposRebar_Shape_Hook.getRebarHookType("Standard - 180 deg.",doc);

                //crea el refuerzo de area
                m_createdAreaReinforcement = CrearAreaRefuerzo(selecFloor, curves, tipoOrientacion, tipodeHook.Id);

                if (elem3d != null)
                {
                    //permite que la barra se vea en el 3d como solido
                    m_createdAreaReinforcement.SetSolidInView(elem3d, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    m_createdAreaReinforcement.SetUnobscuredInView(elem3d, true);
                }

                // desactiva las capa inferior y define que capa inferior aparece botton minor , major
                LayoutRebar_AreaRefuerzo(tipoOrientacion);


                //agrega Hook - si esta direccion botton Major activa
                //Botton Major Hook Type
                if (ParameterUtil.FindParaByName(m_createdAreaReinforcement, "Bottom Major Direction").AsInteger() == 1)
                    ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_1, TiposRebar_Shape_Hook.getRebarHookType("Standard - 180 deg.", doc).Id);

                //agrega Hook - si esta direccion botton minor activa
                //Botton minor Hook Type   
                if (ParameterUtil.FindParaByName(m_createdAreaReinforcement, "Bottom Minor Direction").AsInteger() == 1)
                    ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_2, TiposRebar_Shape_Hook.getRebarHookType("Standard - 90 deg.", doc).Id);

            }

            transaction.Commit();


            return Result.Succeeded;
            // solo se usa para  casos especiales
            Transaction transaction2 = new Transaction(doc, "CreatePathReinforcement2");
            transaction2.Start();
            // cambia AreaReinforcement a Rebar  -----  se puede aplicar tanto a  'AreaReinforcement' como 'PathReinforcement'
            IList<ElementId> aux_rebar_internaId = AreaReinforcement.RemoveAreaReinforcementSystem(doc, m_createdAreaReinforcement);
            // lo pasa a elemnt
            Element aux_rebar_interna = doc.GetElement(aux_rebar_internaId[0]);
            //comprueb q sea rebar
            if (aux_rebar_interna is Rebar)
            {
                Rebar grupoBrras = aux_rebar_interna as Rebar;
                //cambia gancho inicial 
                grupoBrras.SetHookTypeId(0, TiposRebar_Shape_Hook.getRebarHookType("Standard - 180 deg.", doc).Id);
                //cambia gancho final
                grupoBrras.SetHookTypeId(1, TiposRebar_Shape_Hook.getRebarHookType("Standard - 180 deg.", doc).Id);
            }
            transaction2.Commit();


            return Result.Succeeded;
        }

      


        /// <summary>
        /// crea refuerzo de area 
        /// </summary>
        /// <param name="selecFloor">floor seleccionada</param>
        /// <param name="curves">lista con curvas de area a reforzar</param>
        private AreaReinforcement CrearAreaRefuerzo(Floor selecFloor, IList<Curve> curves, TipoOrientacionBarra orien, ElementId rebarHookTypeId)
        {
            AreaReinDataOnFloor dataOnFloor = new AreaReinDataOnFloor();
            XYZ majorDirection = new Autodesk.Revit.DB.XYZ();

            if (orien == TipoOrientacionBarra.Horizontal)
            {
                majorDirection = new Autodesk.Revit.DB.XYZ(
                                ListaPtosPerimetroBarras[1].X - ListaPtosPerimetroBarras[0].X,
                                 ListaPtosPerimetroBarras[1].Y - ListaPtosPerimetroBarras[0].Y,
                                 ListaPtosPerimetroBarras[1].Z - ListaPtosPerimetroBarras[0].Z);
            }
            else if (orien == TipoOrientacionBarra.Vertical)
            {
                majorDirection = new Autodesk.Revit.DB.XYZ(
                                ListaPtosPerimetroBarras[2].X - ListaPtosPerimetroBarras[1].X,
                                 ListaPtosPerimetroBarras[2].Y - ListaPtosPerimetroBarras[1].Y,
                                 ListaPtosPerimetroBarras[2].Z - ListaPtosPerimetroBarras[1].Z);
            }



            int aux_vect = 1;

            //si caso vertical si vector es positivo (hacia arriba)  symbol path area crea una barras patas arriba
            // si caso vertical si vector es negativo (hacia bajo)  symbol path area crea una barras patas abajo --> suple

            //si caso horizontal si vector es positivo (hacia adelante)  symbol path area crea una barras patas arriba
            // si caso horizontal si vector es negativo (hacia atras)  symbol path area crea una barras patas abajo --> suple

            majorDirection = majorDirection * aux_vect;

            //Create AreaReinforcement
            ElementId areaReinforcementTypeId = AreaReinforcementType.CreateDefaultAreaReinforcementType(m_revit.Application.ActiveUIDocument.Document);
            ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_revit.Application.ActiveUIDocument.Document);
            // ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_revit.Application.ActiveUIDocument.Document);
            AreaReinforcement areaRein = AreaReinforcement.Create(m_revit.Application.ActiveUIDocument.Document, selecFloor, curves, majorDirection, areaReinforcementTypeId, rebarBarTypeId, rebarHookTypeId);

            //set AreaReinforcement and it's AreaReinforcementCurves parameters
            dataOnFloor.FillIn(areaRein);

            return areaRein;
        }

        /// <summary>
        /// abstract method to create PathReinforcement
        /// </summary>
        /// <returns>new created PathReinforcement</returns>
        /// <param name="points">points used to create PathReinforcement</param>
        /// <param name="flip">used to specify whether new PathReinforcement is Filp  indica si se dibuja 
        /// hacia arriba o abajo de la curva </param>
        public PathReinforcement CreatePathReinforcement(List<XYZ> points, bool flip, Document m_document, Floor m_data, string cuantia)
        {
            Line curve;
            IList<Curve> curves = new List<Curve>();
            //configuracion
            // 0  - 3
            // 1 -  2
            curve = Line.CreateBound(points[0], points[3]);
            curves.Add(curve);
            string[] auxcuantia = cuantia.Split('a');

            ElementId pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(m_document);

            //1) asigna el tipo de la barra en funcion del diametro, que debe estar creados en la libreria
            RebarBarType rebarBarType = TiposRebar_Shape_Hook.getRebarBarType("@" + auxcuantia[0], m_document,true);
            //RebarHookType rebarShape = TiposBarraYGancho.getRebarHookType("", m_document);

            //1.b)asigna RebarHookType defaul
            ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_document);

            //2)asigna RebarBarType defaul
            //ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_document);

            return PathReinforcement.Create(m_document, m_data, curves, flip, pathReinforcementTypeId, rebarBarType.Id, rebarHookTypeId, rebarHookTypeId);
        }

        /// <summary>
        /// Configura pathReinforment  
        /// a)los largos barra pricipal y altenada
        /// b)posiciona barra en ubicacion inferior losa
        /// 
        /// c) obtiene el ElementId de BarraPrinciapl  y alterna (NOTA: no se usa)
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="espaciamiento"></param>
        /// <param name="largoPrincipal"></param>
        /// <param name="largoALterna"></param>
        public void LayoutRebar_PathReinforcement(List<XYZ> lista, float espaciamiento, double largoPrincipal, double largoALterna)
        {

            if (m_createdPathReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a PathDeRefuerzo null " + ListaPtosPerimetroBarras.Count);
                return;
            }

            // "Face", 0  ->  activa barra superior  - Top ( viene por defecto=
            // "Face", 1  ->  activa barra inferior  - Botton
            ParameterUtil.SetParaInt(m_createdPathReinforcement, "Face", 1);
            //asigna largo de barras principales
            ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, largoPrincipal);

            if (largoALterna == 0.0) return;
            //  m_createdPathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_ALTERNATING);
            //activa la barra altenativa  value=1 --- si es cero solo estan activas las barras principal
            ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_ALTERNATING, 1);

            ////asigna largo de barras Alternativas
            ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largoALterna);





            return;
            // párte de codigo solo de prueb 

            ElementId BarraPrinciapl = m_createdPathReinforcement.PrimaryBarShapeId;
            ElementId BarraAlterna = m_createdPathReinforcement.AlternatingBarShapeId;

            string str = "";
            ParameterSet pars = m_createdPathReinforcement.Parameters;

            var adas = ParameterUtil.FindParaByName(pars, "REBAR_ELEMENT_VISIBILITY");

            foreach (Parameter param in pars)
            {
                string val = "";
                string name = param.Definition.Name;
                Autodesk.Revit.DB.StorageType type = param.StorageType;
                switch (type)
                {
                    case Autodesk.Revit.DB.StorageType.Double:
                        val = param.AsDouble().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.ElementId:
                        Autodesk.Revit.DB.ElementId id = param.AsElementId();
                        Autodesk.Revit.DB.Element paraElem = doc.GetElement(id);
                        if (paraElem != null)
                            val = paraElem.Name;
                        break;
                    case Autodesk.Revit.DB.StorageType.Integer:
                        val = param.AsInteger().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.String:
                        val = param.AsString();
                        break;
                    default:
                        break;
                }
                str = str + name + ": " + val + "\r\n";
            }

        }

        private void LayoutRebar_AreaRefuerzo(TipoOrientacionBarra Orientacion)
        {
            if (m_createdAreaReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a AreaDeRefuerzo null " + ListaPtosPerimetroBarras.Count);
                return;
            }

            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1, 0);// 	"Top Major Direction" 
            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2, 0);// 	"Top Minor Direction" 

            if (Orientacion == TipoOrientacionBarra.Horizontal)
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 0);// 	 	"Bottom Major Direction" 
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 1);// 	 	"Bottom Minor Direction" 
            }
            else
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 1);// 	 	"Bottom Major Direction" 
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 0);// 	 	"Bottom Minor Direction" }

            }

            string str = "";
            ParameterSet pars = m_createdAreaReinforcement.Parameters;

            var adas = ParameterUtil.FindParaByName(pars, "REBAR_ELEMENT_VISIBILITY");

            foreach (Parameter param in pars)
            {
                string val = "";
                string name = param.Definition.Name;
                Autodesk.Revit.DB.StorageType type = param.StorageType;
                switch (type)
                {
                    case Autodesk.Revit.DB.StorageType.Double:
                        val = param.AsDouble().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.ElementId:
                        Autodesk.Revit.DB.ElementId id = param.AsElementId();
                        Autodesk.Revit.DB.Element paraElem = doc.GetElement(id);
                        if (paraElem != null)
                            val = paraElem.Name;
                        break;
                    case Autodesk.Revit.DB.StorageType.Integer:
                        val = param.AsInteger().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.String:
                        val = param.AsString();
                        break;
                    default:
                        break;
                }
                str = str + name + ": " + val + "\r\n";
            }
        }



        /// <summary>
        /// crea curva con puntos del poligono del area a reforzar
        /// </summary>
        /// <param name="lista">lista pto poligono area a reforzar </param>
        /// <returns>curva que se usa en 'AreaReinforcement'</returns>
        private IList<Curve> CrearCurva(List<XYZ> lista)
        {


            IList<Curve> curveList = new List<Curve>();

            XYZ point1 = new XYZ();
            XYZ point2 = new XYZ();
            XYZ point3 = new XYZ();
            XYZ point4 = new XYZ();

            point1 = lista[0];

            point2 = lista[1];

            point3 = lista[2];

            point4 = lista[3];

            Line line1;
            Line line2;
            Line line3;
            Line line4;

            line1 = Autodesk.Revit.DB.Line.CreateBound(point1, point2);
            line2 = Autodesk.Revit.DB.Line.CreateBound(point2, point3);
            line3 = Autodesk.Revit.DB.Line.CreateBound(point3, point4);
            line4 = Autodesk.Revit.DB.Line.CreateBound(point4, point1);

            curveList.Add(line1);
            curveList.Add(line2);
            curveList.Add(line3);
            curveList.Add(line4);

            return curveList;
        }


        /// <summary>
        /// obtiene los puntos que definen el area para reforzar utilizando el mouse.
        ///  genera o cambia los planos  en funcion del poligono de losa. Se utiliza el click de mouse para definir el 
        ///  pto donde se genera refuerzo
        /// </summary>
        /// <param name="listaPtos"> lista de ptos del room que esta definido enla losa</param>
        /// <param name="app"></param>
        /// <param name="floor"> floor-losa seleccioana con el mouse</param>
        /// <returns>lista pto poligono area a reforzar</returns>
        private List<XYZ> ListaFinal_pto(List<XYZ> listaPtos, Application app, Element floor, TipoOrientacionBarra tipoOrientacion)
        {
            List<XYZ> ListaPtoFinal = new List<XYZ>();

            Element floor_1 = null;
            floor_1 = floor as Floor;

            Options gOptions = new Options();
            gOptions.ComputeReferences = true;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;
            GeometryElement geo = floor.get_Geometry(gOptions);

            foreach (GeometryObject obj in geo) // 2013
            {
                GeometryInstance geomInst = null;
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;
                    foreach (Face face in solid.Faces)
                    {
                        ////string s = face.MaterialElement.Name; // 2011
                        //string s = FaceMaterialName(doc, face); // 2012
                        //materials.Add(s);
                    }
                }
                else if (obj is GeometryInstance)
                {
                    geomInst = obj as GeometryInstance;
                    //GeometryInstance i = o as GeometryInstance;
                    //materials.AddRange(GetMaterials1(
                    //  doc, i.SymbolGeometry));
                }

                if (geomInst != null)
                {
                    GeometryElement getInstGeo = geomInst.GetInstanceGeometry();
                    GeometryElement getSymbGeo = geomInst.GetSymbolGeometry();
                }


                Transform trans1 = null;
                Transform Invertrans1 = null;
                Transform trans2_rotacion = null;
                Transform InverTrans2_rotacion = null;
                //var tarsas=getInstGeo.GetTransformed();

                Solid floor_ = obj as Solid;
                if (floor_ != null)
                {
                    foreach (Face f in floor_.Faces)
                    {
                        BoundingBoxUV b = f.GetBoundingBox();
                        UV p = b.Min;
                        UV q = b.Max;
                        UV midparam = p + 0.5 * (q - p);
                        XYZ midpoint = f.Evaluate(midparam);
                        XYZ normal = f.ComputeNormal(midparam);
                        XYZ minxyz = f.Evaluate(b.Min);
                        if (Util.IsVertical(normal) && Util.PointsUpwards(normal))
                        {
                            XYZ ptXAxis = XYZ.BasisX;
                            XYZ ptYAxis = XYZ.BasisY;
                            // Plane plane1 = Plane.(XYZ.Zero, ptYAxis ,ptXAxis);
                            trans1 = Transform.CreateTranslation(-minxyz);
                            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(angle), XYZ.Zero);
                            Invertrans1 = trans1.Inverse;
                            InverTrans2_rotacion = trans2_rotacion.Inverse;
                            //trans1.Origin = listaPtos[3];
                            XYZ pf1 = new XYZ();
                            XYZ pf2 = new XYZ();
                            XYZ pf3 = new XYZ();
                            XYZ pf4 = new XYZ();

                            //p1,p2,p3,p4 pto  del segemneto que 
                            XYZ p1 = new XYZ();
                            XYZ p2 = new XYZ();
                            XYZ p3 = new XYZ();
                            XYZ p4 = new XYZ();
                            p1 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[0]));
                            p2 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[1]));
                            p3 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[2]));
                            p4 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[3]));

                            if (tipoOrientacion == TipoOrientacionBarra.Horizontal)
                            {

                            }
                            else if (tipoOrientacion == TipoOrientacionBarra.Horizontal)
                            {
                                if (Line.CreateBound(p1, p4).Intersect(Line.CreateBound(p2, p3)) == SetComparisonResult.Overlap)
                                {
                                    if (p1.X < p2.X)
                                    {
                                        // p1 es igual
                                        XYZ aux_p2 = p2;
                                        p2 = p3;
                                        p3 = p4;
                                        p4 = aux_p2;
                                    }
                                    else if ((p1.X > p2.X))
                                    {
                                        XYZ aux_p1 = p1;
                                        p1 = p2;
                                        //XYZ aux_p2 = p2;
                                        p2 = p4;
                                        // p3 = p3;
                                        p4 = aux_p1;
                                    }
                                }
                                else
                                {
                                    //ver observacion 1
                                    if (p1.X < p2.X)
                                    {
                                        // p1 es igual
                                        XYZ aux_p2 = p2;
                                        p2 = p4;
                                        //p3 es igual
                                        p4 = aux_p2;
                                    }
                                    else if ((p1.X > p2.X))
                                    {
                                        XYZ aux_p1 = p1;
                                        p1 = p2;
                                        //XYZ aux_p2 = p2;
                                        p2 = p3;
                                        p3 = p4;
                                        p4 = aux_p1;
                                    }
                                }
                            }


                            if (p1.Y > p2.Y)
                            {
                                if (p3.Y > p4.Y)// caso1 
                                {
                                    //analizar sup
                                    if (p3.Y > p1.Y)
                                    {

                                        pf1 = p1;
                                        pf3 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p1.Y, 0), new XYZ(1000, p1.Y, 0)));
                                    }
                                    else if (p3.Y <= p1.Y)
                                    {
                                        pf3 = p3;
                                        pf1 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p3.Y, 0), new XYZ(1000, p3.Y, 0)));
                                    }

                                    //analizar inferiro
                                    if (p2.Y > p4.Y)
                                    {
                                        pf2 = p2;
                                        pf4 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p2.Y, 0), new XYZ(1000, p2.Y, 0)));
                                    }
                                    else if (p2.Y <= p4.Y)
                                    {
                                        pf4 = p4;
                                        pf2 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p4.Y, 0), new XYZ(1000, p4.Y, 0)));
                                    }
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
                                }
                                else
                                { // caso 2
                                    //analizar sup
                                    if (p4.Y > p1.Y)
                                    {
                                        pf1 = p1;
                                        pf4 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p1.Y, 0), new XYZ(1000, p1.Y, 0)));

                                    }
                                    else if (p4.Y <= p1.Y)
                                    {
                                        pf4 = p4;
                                        pf1 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p4.Y, 0), new XYZ(1000, p4.Y, 0)));
                                    }

                                    //analizar inferiro
                                    if (p2.Y > p3.Y)
                                    {
                                        pf2 = p2;
                                        pf3 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p2.Y, 0), new XYZ(1000, p2.Y, 0)));
                                    }
                                    else if (p2.Y <= p3.Y)
                                    {
                                        pf3 = p3;
                                        pf2 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p3.Y, 0), new XYZ(1000, p3.Y, 0)));
                                    }
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));
                                }
                            }
                            else
                            {
                                if (p3.Y > p4.Y)// caso3 
                                {
                                    //analizar sup
                                    if (p3.Y > p2.Y)
                                    {
                                        pf2 = p2;
                                        pf3 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p2.Y, 0), new XYZ(1000, p2.Y, 0)));

                                    }
                                    else if (p3.Y <= p2.Y)
                                    {
                                        pf3 = p3;
                                        pf2 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p3.Y, 0), new XYZ(1000, p3.Y, 0)));
                                    }

                                    //analizar inferiro
                                    if (p1.Y > p4.Y)
                                    {
                                        pf1 = p1;
                                        pf4 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p1.Y, 0), new XYZ(1000, p1.Y, 0)));

                                    }
                                    else if (p1.Y <= p4.Y)
                                    {
                                        pf4 = p4;
                                        pf1 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p4.Y, 0), new XYZ(1000, p4.Y, 0)));
                                    }
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
                                }
                                else
                                { //caso 4
                                    //analizar sup
                                    if (p4.Y > p2.Y)
                                    {
                                        pf2 = p2;
                                        pf4 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p2.Y, 0), new XYZ(1000, p2.Y, 0)));
                                    }
                                    else if (p4.Y <= p2.Y)
                                    {
                                        pf4 = p4;
                                        pf1 = Util.Intersection(Line.CreateBound(p2, p1), Line.CreateBound(new XYZ(-1000, p4.Y, 0), new XYZ(1000, p4.Y, 0)));
                                    }

                                    //analizar inferiro
                                    if (p1.Y > p3.Y)
                                    {
                                        pf1 = p1;
                                        pf3 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, p1.Y, 0), new XYZ(1000, p1.Y, 0)));
                                    }
                                    else if (p1.Y <= p3.Y)
                                    {
                                        pf3 = p3;
                                        pf1 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, p3.Y, 0), new XYZ(1000, p3.Y, 0)));
                                    }
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
                                    ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));
                                }
                            }


                        }
                    }
                }
            }




            return ListaPtoFinal;
        }

        /// <summary>
        /// iterra sobre la lista 'ListaRooms' que contiene los room del view analizadoy busca 
        /// la que contiene al punto pto con la funcion'room.IsPointInRoom'
        /// funcion  para obtener 4 puntos que definen dos lados opuestos del poligono para
        /// posteriormente obtener el area de refuerzo
        /// 
        /// </summary>
        /// <param name="rooms">lisata de room que estan en cierto nivel</param>
        /// <param name="opt">objeto Options a seleccionar</param>
        /// <param name="pto">pto de seleccion con el mouse sobre la losa </param>
        /// <returns>lista de 4 pto que definen 2 lados del poligono de losa</returns>
        private List<XYZ> ListaPoligonosRooms(IEnumerable<Element> Listarooms, Options opt, XYZ pto, TipoOrientacionBarra orientacion)
        {
            List<XYZ> ListaInterseccion = new List<XYZ>();

            foreach (Room room in Listarooms)
            {
                // comprobra si punto esta al interior del volumen de un room
                if (!(room.IsPointInRoom(new XYZ(pto.X, pto.Y, pto.Z + 1)))) continue;

                // obtiene puntos del poligono de room
                List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);

                //si tienes pts analiza  --------- nota: puede que alla room sin asignar
                if (boundary_pts.Count > 0)
                {
                    m_roomSelecionado = room;
                    float largoBarra = 100f;
                    double aux_angle = angle;
                    if (orientacion == TipoOrientacionBarra.Vertical)
                    { aux_angle = aux_angle + 90; }
                    //genera linea auxiliar para buscar las intersecciones con el room
                    Line LineAuxBuscarInters = Line.CreateBound(new XYZ(pto.X - largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto.Y - largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto.Z),
                                                                new XYZ(pto.X + largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto.Y + largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto.Z));


                    for (int i = 0; i <= boundary_pts.Count - 2; i++)
                    {
                        XYZ startPont = boundary_pts[i];
                        XYZ EndPoint = boundary_pts[i + 1];
                        Line LineAuxRoom = Line.CreateBound(startPont, EndPoint);
                        SetComparisonResult result = LineAuxBuscarInters.Intersect(LineAuxRoom);
                        if (result == SetComparisonResult.Overlap)
                        {
                            ListaInterseccion.Add(startPont);
                            ListaInterseccion.Add(EndPoint);
                        }
                    }
                }
            }

            List<XYZ> ListaInterseccion2 = new List<XYZ>();
            return ListaInterseccion;
        }



        /// <summary>
        /// The method is used to create a FamilyElementVisibility instance
        /// </summary>
        /// <returns>FamilyElementVisibility instance</returns>
        private FamilyElementVisibility CreateVisibility()
        {
            FamilyElementVisibility familyElemVisibility = new FamilyElementVisibility(FamilyElementVisibilityType.Model);
            familyElemVisibility.IsShownInCoarse = true;
            familyElemVisibility.IsShownInFine = true;
            familyElemVisibility.IsShownInMedium = true;
            familyElemVisibility.IsShownInFrontBack = true;
            familyElemVisibility.IsShownInLeftRight = true;
            familyElemVisibility.IsShownInPlanRCPCut = false;
            return familyElemVisibility;
        }
    
    }



}
