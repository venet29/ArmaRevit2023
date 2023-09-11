using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRevit.ProfileElement;
using ApiRevit.FILTROS;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccinarMuroRefuerzo
    {
        #region 0) propiedes

        UIApplication _uiapp;
        UIDocument uidoc;
        Application app;
        Document doc;


        public Element ElementoSeleccionado { get; set; }

        public TipoElemento TipoElemto { get; set; }

        public XYZ pto1SeleccionadoConMouse { get; set; }



        public List<XYZ> ListaPtoMuroCara { get; set; }

        public List<XYZ> ListaPtosBordeMuroIntersectado { get; set; }
        public XYZ PtoMouseEspejo;

        public XYZ DireccionEnfierrado { get; set; }
        public XYZ PtoInterseccionSobreBorde { get; set; }
        public double AnguloDireccionenfierrado { get; set; }
        public XYZ NormalCaraSuperiorLosa { get; set; }
        #endregion

        #region 1)contructor

        public SeleccinarMuroRefuerzo(UIApplication uiapp)
        {

            _uiapp = uiapp;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
            ListaPtosBordeMuroIntersectado = new List<XYZ>();
            ListaPtoMuroCara = new List<XYZ>();
        }
        #endregion

        #region 2) metodos

        public bool EjecutarSeleccion()
        {
            try
            {
                if (!M1_B_SeleccionarElemento()) return false;
                M2_GetTipoElemento();

                if (!M3_GetPoligonoMuro()) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool EjecutarSeleccion(XYZ ptoSeleccion, int idMuro)
        {
            try
            {
                if (!M1_A_AgregarElementos(ptoSeleccion, idMuro)) return true;
                M2_GetTipoElemento();
                if (!M3_GetPoligonoMuro()) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public SeleccinarMuroRefuerzo ObtenerMuroMirror()
        {
            ObtenerPtoMirrorSeleccionMOuse();

            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = ElementoSeleccionado;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemto;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = PtoMouseEspejo;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaPtosBordeMuroIntersectado;

            return seleccinarMuroRefuerzoMirror;

        }

        //obtiene el pto  proyectado sobre el borde intersectaso. pto referencia es el pto del mouse
        //obs1)
        public bool ObtenerPtoInterseccionSobreBorde()
        {
            try
            {
                if (ListaPtosBordeMuroIntersectado.Count != 2) return false;
                double zmedio = (ListaPtosBordeMuroIntersectado[0].Z + ListaPtosBordeMuroIntersectado[1].Z) / 2;

                Line lineBorde = Line.CreateBound(ListaPtosBordeMuroIntersectado[0].AsignarZ(0), ListaPtosBordeMuroIntersectado[1].AsignarZ(0));
                PtoInterseccionSobreBorde = lineBorde.ProjectExtendidaXY0(pto1SeleccionadoConMouse.AsignarZ(0)).AsignarZ(zmedio);

                AnguloDireccionenfierrado = Util.AnguloEntre2PtosGrado90(PtoInterseccionSobreBorde, pto1SeleccionadoConMouse, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false; ;
            }
            return true;
        }

        //seleccionar elemento
        private bool M1_B_SeleccionarElemento()
        {
            try
            {


                Selection sel = uidoc.Selection;

                Reference pickedReference;
                try
                {
                    pickedReference = sel.PickObject(ObjectType.Element, SelFilter.GetElementFilter(typeof(Wall), typeof(Wall)), "Seleccionar cabeza de muro");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    pickedReference = null;
                }


                if (pickedReference == null) return false;
                ElementoSeleccionado = doc.GetElement(pickedReference);
                pto1SeleccionadoConMouse = Util.PtoDeLevelDeGlobalPoint(pickedReference.GlobalPoint, doc);
            }
            catch (Exception ex)
            {
                return false;
#pragma warning disable CS0162 // Unreachable code detected
                Debug.WriteLine($"  ex: {ex.Message}");
#pragma warning restore CS0162 // Unreachable code detected
            }
            return true;
        }

        private bool M1_A_AgregarElementos(XYZ pto1Seleccionado, int idMuros)
        {
            try
            {
                this.pto1SeleccionadoConMouse = pto1Seleccionado;
                ElementId eleentmuroID = new ElementId(idMuros);
                ElementoSeleccionado = doc.GetElement(eleentmuroID);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        // obtener tipo elento
        private void M2_GetTipoElemento()
        {
            if (ElementoSeleccionado == null) return;

            if (ElementoSeleccionado is Wall)
            { TipoElemto = TipoElemento.muro; }
            else if (ElementoSeleccionado is Floor)
            { TipoElemto = TipoElemento.losa; }
        }

   
        private bool M3_GetPoligonoMuro()
        {
            try
            {
                //pto supérior
                XYZ PuntoIntersectarcaraSuperior = XYZ.Zero;
                PlanarFace planarFaceSUp = ElementoSeleccionado.ObtenerCaraSuperior(pto1SeleccionadoConMouse, new XYZ(0, 0, 1));

                if (planarFaceSUp != null)
                    PuntoIntersectarcaraSuperior = planarFaceSUp.ObtenerPtosInterseccionFace(pto1SeleccionadoConMouse, XYZ.BasisZ, true);
                else
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(ElementoSeleccionado);
                    if (losa_Encontrada_RuledFace == null) return false;
                    PuntoIntersectarcaraSuperior = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(pto1SeleccionadoConMouse);
                }
                if (PuntoIntersectarcaraSuperior.IsAlmostEqualTo(XYZ.Zero)) return false;
                //punto inferior
               
                PlanarFace planarFaceInf = ElementoSeleccionado.ObtenerCaraInferior(pto1SeleccionadoConMouse,new XYZ(0,0,-1));
                if (planarFaceInf == null) return false;


                if (Math.Abs(pto1SeleccionadoConMouse.Z - PuntoIntersectarcaraSuperior.Z) < Math.Abs(pto1SeleccionadoConMouse.Z - planarFaceInf.Origin.Z))
                {
                    if (planarFaceSUp == null) return false;
                    // falta agregar espesor real de losa,pq cara de muro superior esta a nivel inferior de losa
                    var listaPto = AyudaAgregarEspesorLosa.Ejecutar(planarFaceSUp.ObtenerListaPuntos(), pto1SeleccionadoConMouse);
                    ListaPtoMuroCara = listaPto;
                }
                else
                {
                    ListaPtoMuroCara = planarFaceInf.ObtenerListaPuntos();
                    pto1SeleccionadoConMouse = new XYZ(pto1SeleccionadoConMouse.X, pto1SeleccionadoConMouse.Y, planarFaceInf.Origin.Z);

                }

            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al obtener borde de muro _1");
                return false;
            }

            return true;
        }
        //buscar intersecciones del poligo del muro y dosptos
        public bool GetBordeIntersectaConPto(XYZ segundoPto)
        {
            try
            {


                //primero evalua el punto inical y pto final
                if (Util.IsIntersection2(ListaPtoMuroCara[0].GetXY0(), ListaPtoMuroCara[ListaPtoMuroCara.Count - 1].GetXY0(), pto1SeleccionadoConMouse.GetXY0(), new XYZ(segundoPto.X, segundoPto.Y, 0)))
                {

                    (XYZ valaroInicial,XYZ ValorFinal) = Util.Ordena2PtosV2(ListaPtoMuroCara[0], ListaPtoMuroCara[ListaPtoMuroCara.Count - 1]);

                    ListaPtosBordeMuroIntersectado.Add(valaroInicial.AsignarZ(segundoPto.Z));
                    ListaPtosBordeMuroIntersectado.Add(ValorFinal.AsignarZ(segundoPto.Z));
                    return true;
                }

                //recorre elreso de los ptos
                for (int i = 0; i < ListaPtoMuroCara.Count - 1; i++)
                {
                    if (Util.IsIntersection2(ListaPtoMuroCara[i].GetXY0(), ListaPtoMuroCara[i + 1].GetXY0(), pto1SeleccionadoConMouse.GetXY0(), new XYZ(segundoPto.X, segundoPto.Y, 0)))
                    {
                        (XYZ valaroInicial, XYZ ValorFinal) = Util.Ordena2PtosV2(ListaPtoMuroCara[i], ListaPtoMuroCara[i + 1]);
                        ListaPtosBordeMuroIntersectado.Add(valaroInicial.AsignarZ(segundoPto.Z));
                        ListaPtosBordeMuroIntersectado.Add(ValorFinal.AsignarZ(segundoPto.Z));

                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return false;
            }

            Util.ErrorMsg($"No se encuentra interseccion entre poligono de muro y puntos de seleccion");
            return false;
        }


        //public void GetBordeIntersectaConPtoMirror()
        //{
        //    ObtenerPtoMirrorSeleccionMOuse();
        //    GetBordeIntersectaConPto(PtoMouseEspejo);
        //}

        //OBS1)obtiene  ptp morro fuera de muro
        public bool ObtenerPtoMirrorSeleccionMOuse()
        {
            if (ListaPtosBordeMuroIntersectado.Count != 2) return false;

            Line line = Line.CreateBound(ListaPtosBordeMuroIntersectado[0].GetXY0(), ListaPtosBordeMuroIntersectado[1].GetXY0());
            PtoMouseEspejo = line.ProjectExtendidaXY0(pto1SeleccionadoConMouse);
            XYZ direcion = (PtoMouseEspejo - pto1SeleccionadoConMouse).Normalize();
            PtoMouseEspejo = PtoMouseEspejo + (PtoMouseEspejo - pto1SeleccionadoConMouse).Normalize() * pto1SeleccionadoConMouse.DistanceTo(PtoMouseEspejo);

            return true;
        }


        #region Metodos Helper
        /// <summary>
        /// Super-simple test whether a face is planar 
        /// and its normal vector points upwards.
        /// </summary>
        static bool IsTopPlanarFace(Face f)
        {
            return f is PlanarFace && Util.PointsUpwards(((PlanarFace)f).FaceNormal);
        }

        /// <summary>
        /// Simple test whether a given face normal vector 
        /// points upwards in the middle of the face.
        /// </summary>
        static bool IsTopFace(Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.PointsUpwards(normal);
        }

        static bool IsDownFace(Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.Pointsdownwards(normal);
        }

        #endregion


        /// <summary>
        /// Define equality between XYZ objects, ensuring
        /// that almost equal points compare equal. Cf.
        /// CmdNestedInstanceGeo.XyzEqualityComparer,
        /// which uses the native Revit API XYZ comparison
        /// member method IsAlmostEqualTo. We cannot use
        /// it here, because the tolerance built into that
        /// method is too fine and does not recognise
        /// points that we need to identify as equal.
        /// </summary>
        public class XyzEqualityComparer : IEqualityComparer<XYZ>
        {
            double _eps;

            public XyzEqualityComparer(double eps)
            {
                Debug.Assert(0 < eps, "expected a positive tolerance");

                _eps = eps;
            }

            public bool Equals(XYZ p, XYZ q)
            {
                return _eps > p.DistanceTo(q);
            }

            public int GetHashCode(XYZ p)
            {
                return Util.PointString(p).GetHashCode();
            }
        }
        #endregion
    }
}
