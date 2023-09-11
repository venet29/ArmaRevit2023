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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaArmadura.Geometria
{
    public class LosaGeometrias
    {
        private UIApplication _uiapp;
        private readonly ISeleccionarLosaConMouse seleccionarLosaConMouse;

        private Document _doc { get; }

        private Level _levelLOsa;
        private FindReferenceTarget _findReferenceTarget;
        private int contador = 0;
        private List<Reference> _referencesFiltroMuro;
        private double _elevacionLosa;
        private XYZ _ptoBusquedaAux;

        public List<LosaBorde> listaBorde { get; set; }
        public Floor floorseleccionado { get; set; }
        public LosaGeometrias(UIApplication uiapp, ISeleccionarLosaConMouse seleccionarLosaConMouse)
        {
            this._uiapp = uiapp;
            this.seleccionarLosaConMouse = seleccionarLosaConMouse;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._findReferenceTarget = FindReferenceTarget.Face;
            this.listaBorde = new List<LosaBorde>();
        }
        public LosaGeometrias(UIApplication uiapp, Floor floorseleccionado)
        {
            this.floorseleccionado = floorseleccionado;
            this._uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            this._levelLOsa = _doc.GetElement(floorseleccionado.LevelId) as Level;
            this._elevacionLosa = _levelLOsa.ProjectElevation;
            this._findReferenceTarget = FindReferenceTarget.Face;
            this.listaBorde = new List<LosaBorde>();
        }
        public void Ejecutar()
        {
            M1_ObtenerCaraSuperiorLosa();
            M2_ObtenerBordeLosaSuperior();
            // M3_DibujarSeparateRoomBorde();

        }
        public void EjecutarSinSeleccionLosa()
        {
            //  M1_ObtenerCaraSuperiorLosa();
            M2_ObtenerBordeLosaSuperior();
            // M3_DibujarSeparateRoomBorde();

        }


        public void DibujarLineasSeparacionRoom(Level lv)
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

                        //        cambiar paramatro PHASE_CREATED =Existing
                        //foreach (var item in linea)
                        //{
                        //    Element line_e = item as Element;
                        //    // get the phase id "New construction"
                        //    ElementId idPhase = GetPhaseId("Existing", _doc);
                        //    line_e.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(idPhase);
                        //}
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


        public void M1_ObtenerCaraSuperiorLosa()
        {
            floorseleccionado = seleccionarLosaConMouse.M1_SelecconarFloor();
            //seleccionarLosaConMouse.M2_ObtenerEspesorLosa();
        }
        public bool M2_ObtenerBordeLosaSuperior()
        {
            //int idPathd = 840425;
            //ElementId PAthElement = new ElementId(idPathd);
            //floorseleccionado = (Floor)_doc.GetElement(PAthElement);
            try
            {

                contador = 0;
                BuscarCaraLosa buscarCaraLosa = new BuscarCaraLosa();
                Face _faceSuperior = floorseleccionado.ObtenerPLanarFAce_superior();//  buscarCaraLosa.ObtenerCaraLosa(floorseleccionado, buscarCaraLosa.PointsUpwards);
                //ConstantesGenerales.sbLog.Clear();
                ConstNH.sbLog.AppendLine("referencia usada: " + _findReferenceTarget.ToString());
                IList<CurveLoop> listaCurva = _faceSuperior.GetEdgesAsCurveLoops();

                // busca la mayor 
                _elevacionLosa = Math.Max(listaCurva.Max(cc => cc.Max(cur => Math.Max(cur.GetEndPoint(0).Z, cur.GetEndPoint(1).Z))), _elevacionLosa);
                foreach (CurveLoop cl in listaCurva)
                {
                    foreach (Curve item in cl)
                    {
                        //if (((Line)item).Contains(_ptoBusquedaAux)) Util.ErrorMsg($"Linea   P1:{item.GetEndPoint(0)}  P2:{item.GetEndPoint(1)} ");
                        ConstNH.sbLog.AppendLine("");
                        ConstNH.sbLog.AppendLine("Linea " + contador + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                        ConstNH.sbLog.AppendLine("p1: " + item.GetEndPoint(0) + "p2: " + item.GetEndPoint(1) + "   Angle:" + Util.AnguloEntre2PtosGrados_enPlanoXY(item.GetEndPoint(0), item.GetEndPoint(1)));
                        List<Reference> lista = M2_1_ObtenerBordeLibreLosa(item);
                        M2_2_ObtenerIntervalos(item, lista);
                        contador += 1;
                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
        public Curve M2_ObtenerBordeSuperiorLosaSeleccionado(XYZ PtoBuscar)
        {
            _ptoBusquedaAux = PtoBuscar;
        
            try
            {
                BuscarCaraLosa buscarCaraLosa = new BuscarCaraLosa();
                Face _faceSuperior = floorseleccionado.ObtenerCaraSuperior(PtoBuscar, new XYZ(0,0,-1));//  buscarCaraLosa.ObtenerCaraLosa(floorseleccionado, buscarCaraLosa.PointsUpwards);
                IList<CurveLoop> listaCurva = _faceSuperior.GetEdgesAsCurveLoops();

                foreach (CurveLoop cl in listaCurva)
                {
                    foreach (Curve item in cl)
                    {
                        if (!(item is Line)) continue;
                        var _line = (Line)item;
                        if (_line == null) continue;
                        if (_line.Length < Util.CmToFoot(1)) continue; 

                        Line _aux_line = Line.CreateBound(_line.GetEndPoint(0).GetXY0(), _line.GetEndPoint(1).GetXY0());
                        Debug.WriteLine($"p1:{_line.GetEndPoint(0).GetXY0()}   p2:{_line.GetEndPoint(1).GetXY0()}    -pto busqueda:{_ptoBusquedaAux.GetXY0()}" );
                        if (_aux_line.Contains(_ptoBusquedaAux.GetXY0(),0.0005))
                        {
                            return item;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerBordeSuperiorLosa' ex:{ex.Message}");
                return null;
            }
            return null;

        }
        public Level ObtenerNivelLosa()
        {
            _levelLOsa = _doc.GetElement(floorseleccionado.LevelId) as Level;
            _elevacionLosa = _levelLOsa.ProjectElevation;
            return _levelLOsa;
        }

        public List<Reference> M2_1_ObtenerBordeLibreLosa(Curve curve)
        {
            _referencesFiltroMuro = M2_1_1_ObtenerReferencias(curve, M2_1_2_ObtenerFiltroWallViga(), _findReferenceTarget);
            // List<Reference> referencesFiltroMuro = ObtenerReferencias(curve, ObtenerFiltroMuro());
            //List<Reference> referencesFiltrofINAL= referencesFiltroMuro.AddRange(referencesFiltroMViga).B

            int conINterno = 0;
            foreach (Reference refe in _referencesFiltroMuro)
            {
                var refTipo = refe.ElementReferenceType;
                Element el = _doc.GetElement(refe);
                string tipoSelecionado = "";
                if (el is Wall)
                { tipoSelecionado = "Wall"; }
                else if (el is FamilyInstance)
                { tipoSelecionado = "Beam"; }

                ConstNH.sbLog.AppendLine("  Item Interno :" + conINterno + "   " + tipoSelecionado + "-------------------------------------------------");
                ConstNH.sbLog.AppendLine("      p.GlobalPoint:" + refe.GlobalPoint.Redondear(3));
                ConstNH.sbLog.AppendLine("      ElementReferenceType:" + refe.ElementReferenceType);
                ConstNH.sbLog.AppendLine("      ElementId:" + refe.ElementId.ToString());
                ConstNH.sbLog.AppendLine("      Distancia:" + refe.GlobalPoint.DistanceTo(curve.GetEndPoint(0)));
                conINterno += 1;
            }
            return _referencesFiltroMuro;
        }

        private ElementFilter M2_1_2_ObtenerFiltroMuro() => new ElementCategoryFilter(BuiltInCategory.OST_Walls);
        private ElementFilter M2_1_2_ObtenerFiltroViga()
        {
            LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Beam));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter filtrosViga = new LogicalAndFilter(stFilter, new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete));
            return filtrosViga;

        }
        private ElementFilter M2_1_2_ObtenerFiltroWallViga()
        {
            ElementCategoryFilter fwall = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter fbeam = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalOrFilter filtrosWallViga = new LogicalOrFilter(fwall, fbeam);
            return filtrosWallViga;
        }
        private List<Reference> M2_1_1_ObtenerReferencias(Curve curve, ElementFilter filterWall, FindReferenceTarget findReferenceTarget)
        {
            Func<View3D, bool> isNotTemplate = v3 => !(v3.IsTemplate);
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            View3D view3D = collector
              .OfClass(typeof(View3D))
              .Cast<View3D>()
              .First<View3D>(isNotTemplate);
            ReferenceComparer reference1 = new ReferenceComparer();

            ReferenceIntersector refIntersector = new ReferenceIntersector(filterWall, findReferenceTarget, view3D);

            IList<ReferenceWithContext> referenceWithContext = refIntersector.Find(curve.GetEndPoint(0), (curve as Line).Direction);

            List<Reference> references = referenceWithContext
                                             .Select(p => p.GetReference())
                                              .Distinct(reference1)
                                             .Where(p => p.GlobalPoint.DistanceTo(
                                              curve.GetEndPoint(0)) < curve.Length)
                                             .OrderBy(cc => cc.GlobalPoint.DistanceTo(curve.GetEndPoint(0)))
                                             .ToList();
            return references;
        }


        private void M2_2_ObtenerIntervalos(Curve item, List<Reference> listaRef)
        {
            XYZ ptoInicial = item.GetEndPoint(0);
            XYZ ptoFinal = item.GetEndPoint(1);

            if (listaRef.Count == 0)

            {
                listaBorde.Add(new LosaBorde(ptoInicial.AsignarZ(_elevacionLosa), ptoFinal.AsignarZ(_elevacionLosa), TipoExtSeparateRoom.None));
            }
            else if (listaRef.Count == 1)
            {
                if (M2_2_1_BuscarRecorridosSINElement(ptoInicial, listaRef[0].GlobalPoint))
                    listaBorde.Add(new LosaBorde(ptoInicial.AsignarZ(_elevacionLosa), listaRef[0].GlobalPoint.AsignarZ(_elevacionLosa), _doc.GetElement(listaRef[0].ElementId), TipoExtSeparateRoom.Derecha));

                if (M2_2_1_BuscarRecorridosSINElement(listaRef[0].GlobalPoint, ptoFinal))
                    listaBorde.Add(new LosaBorde(listaRef[0].GlobalPoint.AsignarZ(_elevacionLosa), ptoFinal.AsignarZ(_elevacionLosa), _doc.GetElement(listaRef[0].ElementId), TipoExtSeparateRoom.Izquierda));
            }
            else
            {
                if (M2_2_1_BuscarRecorridosSINElement(ptoInicial, listaRef[0].GlobalPoint))
                    listaBorde.Add(new LosaBorde(ptoInicial.AsignarZ(_elevacionLosa), listaRef[0].GlobalPoint.AsignarZ(_elevacionLosa), _doc.GetElement(listaRef[0].ElementId), TipoExtSeparateRoom.Derecha));

                for (int i = 0; i < listaRef.Count - 1; i++)
                {
                    if (listaRef[i].ElementId.IntegerValue != listaRef[i + 1].ElementId.IntegerValue && M2_2_1_BuscarRecorridosSINElement(listaRef[i].GlobalPoint, listaRef[i + 1].GlobalPoint))
                        listaBorde.Add(new LosaBorde(listaRef[i].GlobalPoint.AsignarZ(_elevacionLosa), listaRef[i + 1].GlobalPoint.AsignarZ(_elevacionLosa), _doc.GetElement(listaRef[i + 1].ElementId), TipoExtSeparateRoom.Ambos));
                }

                if (M2_2_1_BuscarRecorridosSINElement(listaRef[listaRef.Count - 1].GlobalPoint, ptoFinal))
                    listaBorde.Add(new LosaBorde(listaRef[listaRef.Count - 1].GlobalPoint.AsignarZ(_elevacionLosa), ptoFinal.AsignarZ(_elevacionLosa), _doc.GetElement(listaRef[listaRef.Count - 1].ElementId), TipoExtSeparateRoom.Izquierda));
            }
        }

        private bool M2_2_1_BuscarRecorridosSINElement(XYZ ptoInicial, XYZ ptoFinal)
        {
            //mayor 3 cm
            if (ptoInicial.DistanceTo(ptoFinal) < Util.CmToFoot(3)) return false;
            ObtenerRefereciasCercanasLosa obtenerRefereciasCercanasLosa = new ObtenerRefereciasCercanasLosa(_doc, ptoInicial, ptoFinal);
            double espesor = obtenerRefereciasCercanasLosa.GetElementIntersectingBordeLosa();
            return (espesor > 0 ? false : true);

        }


    }



    public class ReferenceComparer : IEqualityComparer<Reference>
    {
        public bool Equals(Reference x, Reference y)
        {
            //if (x.ElementId == y.ElementId)
            //{
            if (x.GlobalPoint.IsAlmostEqualTo(y.GlobalPoint))
            {
                return true;
            }
            return false;
            //}
            //return false;
        }

        public int GetHashCode(Reference obj)
        {
            int hashName = obj.ElementId.GetHashCode();
            int hashId = obj.LinkedElementId.GetHashCode();
            return hashId ^ hashId;
        }
    }

}
