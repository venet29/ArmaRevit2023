using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.GEOM.Casos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public class ExtensionElementDTO
    {
        public XYZ PtoInterseccion { get; set; }
        public PlanarFace Planarface_ { get; set; }
        public double Distancia { get; set; }
    }
    public static partial class ExtensionElement
    {

        public static List<List<PlanarFace>> ListaFace(this Element elemet, bool IsComputeReferences = false)
        {
            if (elemet == null) return new List<List<PlanarFace>>();
            
            GeometriaViga _geometriaBase = new GeometriaViga(elemet.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(elemet, IsComputeReferences);
            return _geometriaBase.listaGrupoPlanarFace;
        }

        public static List<List<Curve>> ObtenerListaVertice(this Element elemet, bool IsConMensajeError = true)
        {
            if (elemet == null) return new List<List<Curve>>();
            GeometriaLosa _geometriaBase = new GeometriaLosa(elemet.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(elemet);
            var ListaVertices = _geometriaBase.listaPlanarFace.Select(c => c.ObtenerListaCurvas()).ToList();

            return ListaVertices;
        }

        public static List<List<RuledFace>> ListaRuledFace(this Element elemet, bool IsComputeReferences = false)
        {
            if (elemet == null) return new List<List<RuledFace>>();

            GeometriaViga _geometriaBase = new GeometriaViga(elemet.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(elemet, IsComputeReferences);
            return _geometriaBase.ListaGrupoRuledFace;
        }
        //*******************************************
        public static double ObtenerEspesorConCaraVerticalVIsible_foot(this Element _element, View direccion)
        {
            if (_element == null) return -1;

            var PlanarVerticalVIsible = _element.ListaFace()[0].Where(c => Util.IsSimilarValor(Util.GetProductoEscalar(c.FaceNormal, direccion.ViewDirection), 1, 0.01)).FirstOrDefault();

            if (PlanarVerticalVIsible == null) return 0;
            var listaPto = PlanarVerticalVIsible.ObtenerListaPuntos();
            XYZ ptocentraPLanarface = new XYZ(listaPto.Average(c => c.X), listaPto.Average(c => c.Y), listaPto.Average(c => c.Z));
            double espesor = _element.ObtenerEspesorConPtos_foot(ptocentraPLanarface, -direccion.ViewDirection);

            return espesor;
        }

        public static PlanarFace ObtenerCaraMasCercanaAPto_soloCArasHorizontales(this Element _element, XYZ pto, XYZ direccion)
        {
            try
            {
                PlanarFace planarSup = ObtenerCaraSuperior(_element, pto, direccion);
                if (planarSup == null)
                    planarSup = ObtenerPLanarFAce_superior(_element, false);

                XYZ ptoSup = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(planarSup.FaceNormal, planarSup.Origin, pto);
                //planarSup.ProjectNH(pto);

                //XYZ  ptoSup = planarSup.ObtenerPtosInterseccionFace(pto, XYZ.BasisZ);

                PlanarFace planarInf = ObtenerCaraInferior(_element, pto, direccion);
                if (planarInf == null)
                    planarInf = ObtenerCaraInferior(_element, false);

                XYZ ptoInf = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(planarInf.FaceNormal, planarInf.Origin, pto);
                //planarInf.ProjectNH(pto);// planarInf.ObtenerPtosInterseccionFace_utilizarPlano(pto);


                //***************************************************
                if (pto.DistanceTo(ptoSup) < pto.DistanceTo(ptoInf))
                    return planarSup;
                else
                    return planarInf;
            }
            catch (Exception)
            {
            }
            return null;
        }

         // utiliza plano que se extiende en el plnao de los planarface
        public static PlanarFace ObtenerCaraMasCercanaAPto_soloCArasHorizontales_utilizPlanoExtendido(this Element _element, XYZ pto, XYZ direccion)
        {
            PlanarFace planarSup = ObtenerPLanarFAce_superior(_element);
            XYZ ptoSup = planarSup.GetPtosIntersFaceUtilizarPlanoNh(pto);

            PlanarFace planarInf = ObtenerCaraInferior(_element);
            XYZ ptoInf = planarInf.GetPtosIntersFaceUtilizarPlanoNh(pto);

            if (pto.DistanceTo(ptoSup) < pto.DistanceTo(ptoInf))
                return planarSup;
            else
                return planarInf;
        }

        public static XYZ ObtenerPtoInterseccionCara_segunDireccion(this Element _element, XYZ pto, XYZ direccion)
        {
            PlanarFace planar = ObtenerCaraSuperior(_element, pto, direccion);
            return planar.ObtenerPtosInterseccionFaceHorizontal(pto);
        }

        //**************************** superior

        public static XYZ ObtenerPtosInterseccionFaceSuperior(this Element floor, XYZ ptoselec, bool _ISMensajes = false)
        {
            Func<PlanarFace, bool> Criterio = pl => pl.FaceNormal.Z > 0;

            return ObtenerPtosInterseccionFace_segunCriterio(floor, ptoselec, Criterio, _ISMensajes);
        }
        public static XYZ ObtenerPtosInterseccionFaceInferior(this Element floor, XYZ ptoselec, bool _ISMensajes = false)
        {
            Func<PlanarFace, bool> Criterio = pl => pl.FaceNormal.Z < 0;

            return ObtenerPtosInterseccionFace_segunCriterio(floor, ptoselec, Criterio, _ISMensajes);
        }

        public static XYZ ObtenerPtosInterseccionFace_segunCriterio(this Element floor, XYZ ptoselec, Func<PlanarFace, bool> Criterio, bool _ISMensajes = false)
        {
            XYZ ptoSUp = XYZ.Zero; ;
            try
            {
                var lsitaPara = floor.ListaFace();
                if (lsitaPara.Count == 0)
                    return null;

                var ListaPlanarFaceSuperior = lsitaPara[0].Where(c => Criterio(c)).ToList();

                Curve lineVertcal = Line.CreateBound(ptoselec + new XYZ(0, 0, +25), ptoselec + new XYZ(0, 0, -25));

                ptoSUp = XYZ.Zero;

                //****************************
                ptoSUp = ExtensionFloorAyuda.ObtenerPto_ConListaPlanarFace(ListaPlanarFaceSuperior, lineVertcal, _ISMensajes);
                //**

                if (ptoSUp.IsAlmostEqualTo(XYZ.Zero))
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(floor);
                    if (losa_Encontrada_RuledFace == null)
                    {
                        string msje = $"Error al obtener punto intersecion con face superior de losa. Se utliza punto 'origen' de face superior";
                        //Util.ErrorMsg_COnverificacion(msje);
                        Debug.WriteLine(msje);
                        return ptoselec.AsignarZ( ListaPlanarFaceSuperior[0].Origin.Z);
                    }

                    return losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(ptoselec);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg_COnverificacion($"Error en 'ObtenerPtosInterseccionFaceSuperior'. Ex:{ex.Message}");
            }
            return ptoSUp;
        }


        public static PlanarFace ObtenerPLanarFAce_superior(this Element elemet, bool IsConMensajeError = true)
        {
            var listaFAce_aux = elemet.ListaFace();
            if (listaFAce_aux.Count == 0)
                return null;

            var listaFAce = listaFAce_aux[0];
            var listFaceSuperior = listaFAce.Where(c => Util.PointsUpwards_soloViga(c.FaceNormal))
                                                                 .OrderByDescending(c => c.Area)
                                                                 .OrderByDescending(c => c.Origin.Z).ToList();
            //se elige dos porquepuede ser una viga semi inveritda entoces se forman dos elemtnos vigas, con dos normales z positvos y 2 normales z negativo
            if (listFaceSuperior.Count > 2 && IsConMensajeError)
            {
                string msje = $"Se encontraron {listFaceSuperior.Count} caras superiores. Revisar seleccion de elemento";
                Util.ErrorMsg_COnverificacion(msje);
                Debug.WriteLine(msje);
            }

            PlanarFace FaceSuperior = listFaceSuperior.FirstOrDefault();

            return FaceSuperior;
        }


        public static PlanarFace ObtenerCaraSuperior(this Element _element, bool IsConMensajeError = false)
        {
            var ListaPlanarFaceSuperior_aux = _element.ListaFace();
            if (ListaPlanarFaceSuperior_aux.Count == 0)
                return null;

            var ListaPlanarFaceSuperior = ListaPlanarFaceSuperior_aux[0].Where(c => c.FaceNormal.Z > 0).OrderByDescending(r => r.FaceNormal.Z).ToList();
            if (ListaPlanarFaceSuperior.Count > 1 && IsConMensajeError)
            {
                Util.ErrorMsg_COnverificacion($"Se encontraron {ListaPlanarFaceSuperior.Count} caras superiores. Revisar seleccion de elemento");
            }

            return ListaPlanarFaceSuperior[0];
        }

        public static PlanarFace ObtenerCaraSuperior(this Element _element, XYZ pto, XYZ direccion, bool IsConMensajeError = false)
        {

            var ListaPlanarFaceSuperior_aux = _element.ListaFace();
            if (ListaPlanarFaceSuperior_aux.Count == 0)
                return null;

            var ListaPlanarFaceSuperior = ListaPlanarFaceSuperior_aux[0].Where(c => c.FaceNormal.Z > 0).ToList();
            if (ListaPlanarFaceSuperior.Count > 1 && IsConMensajeError)
            {
                Util.ErrorMsg_COnverificacion($"Se encontraron {ListaPlanarFaceSuperior.Count} caras superiores. Revisar seleccion de elemento");
            }
            Curve lineVertcal = Line.CreateBound(pto + direccion * 50, pto - direccion * 50);

            return ExtensionFloorAyuda.ObtenerPlanarFace(ListaPlanarFaceSuperior, lineVertcal);
        }

        //****************************inferior        

        public static PlanarFace ObtenerCaraInferior(this Element _element, bool IsConMensajeError = true)
        {
            var ListaPlanarFaceInferior_aux = _element.ListaFace();
            if (ListaPlanarFaceInferior_aux.Count == 0)
                return null;

            var ListFaceInferior = ListaPlanarFaceInferior_aux[0].Where(c => Util.Pointsdownward_soloViga(c.FaceNormal))
                                                                 .OrderByDescending(c => c.Area)
                                                                 .OrderBy(c => c.Origin.Z).ToList();
            //se elige dos porquepuede ser una viga semi inveritda entoces se forman dos elemtnos vigas, con dos normales z positvos y 2 normales z negativo
            if (ListFaceInferior.Count > 2 && IsConMensajeError)
                Util.ErrorMsg_COnverificacion($"Se encontraron {ListFaceInferior.Count} caras inferior.Se utiliza face con origen de z mas bajo. Revisar seleccion de elemento");

            PlanarFace FaceInferior = ListFaceInferior.FirstOrDefault();
            return FaceInferior;
        }
        public static PlanarFace ObtenerCaraInferior(this Element _element, XYZ pto, XYZ direccion)
        {
            var ListaPlanarFaceInferior_aux = _element.ListaFace();
            if (ListaPlanarFaceInferior_aux.Count == 0)
                return null;

            var ListaPlanarFaceInferior = ListaPlanarFaceInferior_aux[0].Where(c => Util.Pointsdownward_soloViga(c.FaceNormal))
                                                      .OrderByDescending(c => c.Area)
                                                      .OrderBy(c => c.Origin.Z).ToList();

            //var ListaPlanarFaceInferior = _element.ListaFace()[0].Where(c => c.FaceNormal.Z < 0).ToList();
            Curve lineVertcal = Line.CreateBound(pto + direccion * 50, pto - direccion * 50);

            return ExtensionFloorAyuda.ObtenerPlanarFace(ListaPlanarFaceInferior, lineVertcal);
        }


        //************************************************************

        public static PlanarFace ObtenerCaraSegun_IgualDireccion_MasLejano(this Element _element, XYZ direccion)
        {
            var lista = ObtenerCaraSegun_IgualDireccion(_element, direccion);
            if (lista == null) return null;
            var result = lista.OrderByDescending(r => r.Distancia).FirstOrDefault();

            return (result != null ? result.Planarface_ : null);

        }

        public static PlanarFace ObtenerCaraSegun_IgualDireccion_MasCercamo(this Element _element, XYZ direccion)
        {
            var lista = ObtenerCaraSegun_IgualDireccion(_element, direccion);
            if (lista == null) return null;
            var result = lista.OrderBy(r => r.Distancia).FirstOrDefault();

            return (result != null ? result.Planarface_ : null);
        }
        public static List<ExtensionElementDTO> ObtenerCaraSegun_IgualDireccion(Element _element, XYZ direccion)
        {

            var listaface = _element.ListaFace(true);
            if (listaface.Count == 0) return new List<ExtensionElementDTO>();

            var ListaPlanarFace = listaface[0];



            if (ListaPlanarFace == null) return null;

            XYZ centroElem = XYZ.Zero;
            ListaPlanarFace.ForEach(c => centroElem = centroElem + c.ObtenerCenterDeCara());
            centroElem = centroElem / ListaPlanarFace.Count;//obtenego centro elemento

            var ListaExtensionElementDTO = ListaPlanarFace.Where(c => UtilDesglose.IsParallelIgualSentido(c.FaceNormal, direccion))
                                                   .Select(c =>
                                                        new ExtensionElementDTO()
                                                        {
                                                            Planarface_ = c,
                                                            Distancia = c.ObtenerDistaciaAFace_ConPtoYDireccion_utilzaPlano(centroElem)// la distancia del centro al plano
                                                        }).ToList();
            //.OrderByDescending(r => r.Distancia);


            return ListaExtensionElementDTO;
        }



        //**************************************************************
        //espesor 
        public static double ObtenerEspesorConPtos_foot(this Element familyIStance, XYZ ptoselec, XYZ direccion, bool _ISMensajes = false)
        {
            double espesor_foot = 0;

            try
            {

                var listaFamilia = familyIStance.ListaFace(_ISMensajes);
                if (listaFamilia.Count == 0) return 0;

                var lsitaPara = listaFamilia[0];
                Curve lineVertcal = Line.CreateBound(ptoselec + direccion.Normalize() * 5, ptoselec - direccion.Normalize() * 5);
                //****************************
                List<IntersectionResultNH> Lista = ExtensionFloorAyuda.ObtenerListaIntersectionResult(lsitaPara, lineVertcal, _ISMensajes);
                //**
                if (Lista.Count != 2)
                {
                    // Util.ErrorMsg($"Error al obtener espesor Losa variable");
                    return 0;
                }
                //***
                espesor_foot = Lista[0].ptoInterseccion.DistanceTo(Lista[1].ptoInterseccion);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg_COnverificacion($"Error al obtener espesor  de elemento 'ObtenerEspesorConPtos Elemento'. Ex:{ex.Message}");
            }
            return espesor_foot;

        }
        //**************************************************************
        //espesor 
        public static PlanarFace ObtenerfaceMAsCercanaConDireccion_foot(this Element familyIStance, XYZ ptoselec, XYZ direccion, bool _ISMensajes = false)
        {

            try
            {
                var listaFamilia = familyIStance.ListaFace(_ISMensajes);
                if (listaFamilia.Count == 0) return null;

                var lsitaPara = listaFamilia[0];
                Curve lineVertcal = Line.CreateBound(ptoselec + direccion.Normalize().Redondear(3) * 100, ptoselec - direccion.Normalize().Redondear(3) * 100);

                //  ((Line)lineVertcal).CrearModelCurve(familyIStance.Document);
                //****************************
                List<IntersectionResultNH> Lista = ExtensionFloorAyuda.ObtenerListaIntersectionResult(lsitaPara, lineVertcal, _ISMensajes);
                //**
                Lista = Lista.Where(c => Util.IsParallel_igualSentido(c.planarInterseccion.FaceNormal, direccion, 0.9)).ToList();
                if (Lista.Count == 0)
                {
                    if (_ISMensajes) Util.ErrorMsg($"No se encontro planarFace en pto:{ptoselec.REdondearString_foot(3)}   con direccion : {direccion.REdondearString_foot(3)}    Nombre:{familyIStance.Name}");
                    return null;
                }
                var planarfaceMAsCercana = Lista.MinBy(c => c.ptoInterseccion.DistanceTo(ptoselec));
                if (planarfaceMAsCercana != null)
                    return planarfaceMAsCercana.planarInterseccion;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener espesor  de elemento 'ObtenerfaceMAsCercanaConDireccion'. Ex:{ex.Message}");
            }
            return null;

        }

        /// <summary>
        /// obtiene el ancho visible de la cara que se ve en la vista.
        /// </summary>
        /// <param name="familyIStance"></param>
        /// <param name="ptoselec"></param>
        /// <param name="direccion"></param>
        /// <param name="_ISMensajes"></param>
        /// <returns></returns>
        public static double ObtenerAnchoConPtos(this Element familyIStance, XYZ ptoselec, XYZ direccion, bool _ISMensajes = false)
        {
            double Largo = 0;

            try
            {
                var listaFanili = familyIStance.ListaFace();
                if (listaFanili.Count == 0) return 0;

                var lsitaPara = listaFanili[0];
                Curve lineVertcal = Line.CreateBound(ptoselec + direccion.Normalize() * 50, ptoselec - direccion.Normalize() * 50);
                //****************************
                List<IntersectionResultNH> Lista = ExtensionFloorAyuda.ObtenerListaIntersectionResult(lsitaPara, lineVertcal, _ISMensajes);
                //**

                if (Lista.Count == 0)
                {
                    // Util.ErrorMsg($"Error al obtener espesor Losa variable");
                    return 0;
                }

                //***
                var largoloop = (Lista[0].planarInterseccion).GetEdgesAsCurveLoops().MinBy(c => c.GetExactLength()).FirstOrDefault();

                Largo = largoloop.Length;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg_COnverificacion($"Error en 'ObtenerAnchoConPtos'. Ex:{ex.Message}");
            }
            return Largo;

        }
        //**
        //NOTA: buscar donde se usa este caso
        public static PlanarFace ObtenerPlanerFaceMasCercano(this Element _element, XYZ ptoInter)
        {


            var planarFaceinf = _element.ListaFace();
            if (planarFaceinf.Count > 1)
            {
                Util.ErrorMsg_COnverificacion("MAs de un objeto contenerde Planarface");
            }

            if (planarFaceinf.Count == 0) return null;

            List<PlanarFace> listaPlanarFaceVerticales = planarFaceinf[0].Where(c => !(c.IsTopFace() || c.IsDownFace())).ToList();
            var PlanarFace = listaPlanarFaceVerticales.MinBy(c => -DistanciaAFace(c, ptoInter, false));
            return PlanarFace;

        }
        public static PlanarFace ObtenerPlanerFaceMasCercano_ConVector(this Element viga, XYZ ptoInter, XYZ direccionNormalBusqueda, bool IsComputeReferences )
        {
            var planarFaceinf = viga.ListaFace(IsComputeReferences).Where(c => c.Count > 0).OrderByDescending(r => r.Count).ToList();
            if (planarFaceinf.Count > 1)
            {
                Util.ErrorMsg_COnverificacion("MAs de un objeto contenerde Planarface");
            }

            if (planarFaceinf.Count == 0) return null;

            List<PlanarFace> listaPlanarFaceVerticales = planarFaceinf[0].
                Where(c => !(c.IsTopFace() || c.IsDownFace()) && c.Area > 0.1 && //area > 10cm*10cm
                            Util.IsParallel(direccionNormalBusqueda, c.FaceNormal) &&
                            !XYZ.Zero.IsAlmostEqualTo(c.ProjectNH(ptoInter, false))).ToList();


            if (listaPlanarFaceVerticales.Count == 0) return null;

            var _PlanarFace = listaPlanarFaceVerticales.MinBy(c => DistanciaAFace(c, ptoInter, false));
            return _PlanarFace;

        }

        public static (bool, PlanarFace) ObtenerCaraVerticalVIsible(this Element _element, View _view)
        {
            if (_element == null)
            {
                Util.ErrorMsg("Elemento null para buscar cara vertical visible");
                return (false, null);
            }
            //var PlanarVerticalVIsiblNoVerticales = _element.ListaFace()[0]
            //                            .Where(c => !Util.IsVertical(c.FaceNormal)).ToList();

            var listaFaces = _element.ListaFace();
            if (listaFaces.Count == 0) return (false, null);

            var PlanarVerticalVIsible = listaFaces[0]
                                                //.Where(c => Util.IsSimilarValor(Util.GetProductoEscalar(c.FaceNormal.GetXY0(), direccion.ViewDirection.GetXY0()), 1, 0.001))
                                                .OrderByDescending(c => Util.GetProductoEscalar(c.FaceNormal.GetXY0(), _view.ViewDirection.GetXY0()))
                                                .ToList();
            if (PlanarVerticalVIsible.Count == 0)
                return (false, null);
            else
                return (true, PlanarVerticalVIsible.FirstOrDefault());
        }

        public static bool IsEstructural(this Element eleme_)
        {
            try
            {
                int restul = 0;
                if (eleme_ is Wall)
                    restul = eleme_.get_Parameter(BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT).AsInteger();

                else if (eleme_ is Floor)
                {
                    restul = eleme_.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL).AsInteger();
                }
                else if (eleme_ is FamilyInstance)
                {
                    // Util.ErrorMsg($"El elemento {eleme_.Name} no esta programado para verificar si es estructural o no. Se asume que 'NO'");
                    return true;
                }
                if (restul == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg_COnverificacion($"Error en 'IsEstructural'. Ex:{ex.Message}");
            }
            return false;
        }
        private static double DistanciaAFace(PlanarFace pl, XYZ ptoInter, bool IsMensaje)
        {
            var ptoInterseccionCOnFAce = pl.ObtenerPtosInterseccionFace(ptoInter, pl.FaceNormal, IsMensaje);
            //var resul = pl.Project(ptoInter);
            //if (resul == null) return 0;

            //XYZ ptoSobreSuperficie = resul.XYZPoint;

            return ptoInterseccionCOnFAce.DistanceTo(ptoInter);
        }


        /// <summary>
        /// Return the curve from a Revit database Element 
        /// location curve, if it has one.
        /// </summary>
        public static Curve ObtenerCurve(this Element e)
        {
            Debug.Assert(null != e.Location, "error el elemento location=null");

            LocationCurve lc = e.Location as LocationCurve;

            Debug.Assert(null != lc, "error el elemento LocationCurve=null");

            return lc.Curve;
        }



    }
}
