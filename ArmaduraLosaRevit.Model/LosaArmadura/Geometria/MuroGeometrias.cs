using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.LosaArmadura.Ayuda;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaArmadura.Geometria
{
    public class MuroGeometrias
    {
        private UIApplication _uiapp;
        private HelperSeleccinarMuro _seleccionarMuroConMouse;

        private Document _doc { get; }

        private Level _levelLOsa;
#pragma warning disable CS0414 // The field 'MuroGeometrias._findReferenceTarget' is assigned but its value is never used
        private FindReferenceTarget _findReferenceTarget;
#pragma warning restore CS0414 // The field 'MuroGeometrias._findReferenceTarget' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'MuroGeometrias.contador' is assigned but its value is never used
        private int contador = 0;
#pragma warning restore CS0414 // The field 'MuroGeometrias.contador' is assigned but its value is never used

        private double _elevacionLosa;

        public List<LosaBorde> listaBorde { get; set; }
        public Wall wallseleccionado { get; set; }
        public MuroGeometrias(UIApplication uiapp, HelperSeleccinarMuro seleccionarMuroConMouse)
        {
            this._uiapp = uiapp;
            this._seleccionarMuroConMouse = seleccionarMuroConMouse;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._findReferenceTarget = FindReferenceTarget.Face;
            this.listaBorde = new List<LosaBorde>();
        }
        public MuroGeometrias(UIApplication uiapp, Wall wallseleccionado)
        {
            this.wallseleccionado = wallseleccionado;            
            this._uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            this._levelLOsa = _doc.GetElement(this.wallseleccionado.LevelId) as Level;
            this._elevacionLosa = _levelLOsa.ProjectElevation;
            this._findReferenceTarget = FindReferenceTarget.Face;
            this.listaBorde = new List<LosaBorde>();
        }
  
        public bool M1_ObtenerCaraSuperiorMuro()
        {
            if (_seleccionarMuroConMouse.SoloSeleccionarMuro())
            { 
                wallseleccionado = _seleccionarMuroConMouse.MuroSeleccionado;
                return true;
            }
            else
                return false;

        }
        public bool M2_ObtenerBordeLosa(double _elevacionLosa)
        {
            //int idPathd = 840425;
            //ElementId PAthElement = new ElementId(idPathd);
            //floorseleccionado = (Floor)_doc.GetElement(PAthElement);
            try
            {

               // contador = 0;
                BuscarCaraLosa buscarCaraLosa = new BuscarCaraLosa();
                Face _faceSuperior = wallseleccionado.ObtenerPLanarFAce_superior();// buscarCaraLosa.ObtenerCaraLosa(wallseleccionado, buscarCaraLosa.PointsUpwards);
               // ConstantesGenerales.sbLog.Clear();
               // ConstantesGenerales.sbLog.AppendLine("referencia usada: " + _findReferenceTarget.ToString());
                IList<CurveLoop> listaCurva = _faceSuperior.GetEdgesAsCurveLoops();

                // busca la mayor 
               // _elevacionLosa=Math.Max( listaCurva.Max(cc => cc.Max(cur => Math.Max(cur.GetEndPoint(0).Z, cur.GetEndPoint(1).Z))), _elevacionLosa);
                foreach (CurveLoop cl in listaCurva)
                {
                    foreach (Curve item in cl)
                    {
                      
                       //if (M2_1_ObtenerReferenciasMuroViga(item, _elevacionLosa) )
                            if (M2_1_BuscarRecorridosSINElement(item.GetEndPoint(0).AsignarZ(_elevacionLosa), item.GetEndPoint(1).AsignarZ(_elevacionLosa)))
                                listaBorde.Add(new LosaBorde(item.GetEndPoint(0).AsignarZ(_elevacionLosa), item.GetEndPoint(1).AsignarZ(_elevacionLosa), TipoExtSeparateRoom.None));
                      //  contador += 1;
                    }
                }
//#if DEBUG
//                LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
//#endif
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        //buscar refereica muro viga
        // cantidad referencia==0 -> true  'NO se encontro muro donde se dibuja la linea'
        //          referencia>0 ->  false  'se encontro muro donde se dibuja la linea'
        private bool M2_1_ObtenerReferenciasMuroViga(Curve curve,double elevacion)
        {
            Func<View3D, bool> isNotTemplate = v3 => !(v3.IsTemplate);
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            View3D view3D = collector
              .OfClass(typeof(View3D))
              .Cast<View3D>()
              .First<View3D>(isNotTemplate);
            ReferenceComparer reference1 = new ReferenceComparer();

            ReferenceIntersector refIntersector = new ReferenceIntersector(M2_1_2_ObtenerFiltroWallViga(), FindReferenceTarget.Face, view3D);

            IList<ReferenceWithContext> referenceWithContext = refIntersector.Find(curve.GetEndPoint(0).AsignarZ(elevacion), (curve as Line).Direction);

            List<Reference> references = referenceWithContext
                                             .Select(p => p.GetReference())
                                              .Distinct(reference1)
                                             .Where(p => p.GlobalPoint.DistanceTo(
                                              curve.GetEndPoint(0)) < curve.Length)   
                                             .ToList();
            return (references.Count()==0?true:false);
        }
        private ElementFilter M2_1_2_ObtenerFiltroWallViga()
        {
            ElementCategoryFilter fwall = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter fbeam = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalOrFilter filtrosWallViga = new LogicalOrFilter(fwall, fbeam);
            return filtrosWallViga;
        }

        //buscar refereica muro viga   MEJOR OPCION
        // cantidad referencia==0 -> true  'NO se encontro muro donde se dibuja la linea'
        //          referencia>0 ->  false  'se encontro muro donde se dibuja la linea'
        private bool M2_1_BuscarRecorridosSINElement(XYZ ptoInicial, XYZ ptoFinal)
        {
            //mayor 3 cm
            if (ptoInicial.DistanceTo(ptoFinal) < Util.CmToFoot(3)) return false;
            ObtenerRefereciasCercanasLosa obtenerRefereciasCercanasLosa = new ObtenerRefereciasCercanasLosa(_doc, ptoInicial, ptoFinal);
            double espesor = obtenerRefereciasCercanasLosa.GetElementIntersectingBordeLosa();
            return (espesor > 0 ? false : true);

        }
        public void M3_DibujarLineasSeparacionRoom(Level lv)
        {

            foreach (LosaBorde lb in listaBorde)
            {
                CurveArray cArray = new CurveArray();
                cArray.Append(lb.roomSeparateCurve);
                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {

                        trans.Start("CrearBordeLibre-NH");
                        SketchPlane skP = SketchPlane.Create(_doc, lv.Id);
                        var linea = _doc.Create.NewRoomBoundaryLines(skP, cArray, _doc.ActiveView);
                        trans.Commit();
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
            }
        }


    }




}
