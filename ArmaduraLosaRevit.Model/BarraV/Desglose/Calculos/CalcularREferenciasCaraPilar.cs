using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.Dimensiones.DTO;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos
{
    public class CalcularREferenciasCaraPilar
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<RebarDesglose> lista_RebarDesglose;
        private View _viewOriginal;
        private Line LineaEntrado_alladoIzquierdo;
        private Line LineaParalela_alladoabajo;

        public bool Isok { get; private set; }
        public List<AuxContenedorPlanos> ListacarasVerticales { get; set; }

        public CalcularREferenciasCaraPilar(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose, View _ViewOriginal)
        {

            this.lista_RebarDesglose = lista_RebarDesglose;
            _viewOriginal = _ViewOriginal;
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;

        }



        public bool Calcular()
        {
            try
            {
                var walloPilar = _doc.GetElement(lista_RebarDesglose[0]._rebar.GetHostId());
                var ListaListaCAras = walloPilar.ListaFace(true);
                if (ListaListaCAras == null)
                {
                    Isok = false;
                    return true;
                }
                List<PlanarFace> Listacaras = ListaListaCAras.FirstOrDefault();
                if (Listacaras == null)
                {
                    Isok = false;
                    return true;
                }

                ListacarasVerticales = Listacaras.Where(c => c.FaceNormal.Z == 0).Select(j => new AuxContenedorPlanos(j)).ToList();

                if (ListacarasVerticales.Count != 4)
                {
                    Isok = false;
                    return true;
                }

                for (int i = 0; i < ListacarasVerticales.Count; i++)
                {
                    var planoAnalisas = ListacarasVerticales[i];
                    if (planoAnalisas.IsAnalisada == true) continue;
                    planoAnalisas.IsAnalisada = true;

                    var resultaPLano = ListacarasVerticales.Where(c => c.IsAnalisada == false &&
                                                     Math.Abs(UtilDesglose.GetProductoEscalar(planoAnalisas.normal, c.normal)) > 0.5)
                                         .FirstOrDefault();
                    if (resultaPLano == null)
                    {
                        Isok = false;
                        return true;
                    }

                    DireccionNormal _auxDireccionNormal = DireccionNormal.paraleloAView;
                    if (UtilDesglose.IsParallelIgualSentido(planoAnalisas.normal, _viewOriginal.ViewDirection))
                        _auxDireccionNormal = DireccionNormal.entrandoEnView;

                    planoAnalisas.DireccionNormal_ = _auxDireccionNormal;
                    planoAnalisas.Grupoplano_ = i;
                    planoAnalisas.Obtenerreferncia();

                    resultaPLano.DireccionNormal_ = _auxDireccionNormal;
                    resultaPLano.Grupoplano_ = i;
                    resultaPLano.IsAnalisada = true;
                    resultaPLano.Obtenerreferncia();
                    Isok = true;

                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public bool GenerarDimesionesPilar4lados(XYZ ptoCentroPilarAlturaCOrte)
        {
            try
            {
                if (Isok == false) return true;

                bool resultENtrado = false;
                bool resultPAralelo = false;

                //1) 
                var lista = ListacarasVerticales.Where(c => c.Grupoplano_ == 0).ToList();

                if (lista[0].DireccionNormal_ == DireccionNormal.entrandoEnView)
                    resultENtrado = M1_ObtenerLineEntradoEnVista(lista);
                else
                    resultPAralelo = M2_ObtenerLineParaleloVista(lista);

                //2)
                var lista2 = ListacarasVerticales.Where(c => c.Grupoplano_ == 1).ToList();
                if (lista2[0].DireccionNormal_ == DireccionNormal.entrandoEnView)
                    resultENtrado = M1_ObtenerLineEntradoEnVista(lista2);
                else
                    resultPAralelo = M2_ObtenerLineParaleloVista(lista2);

                if (resultENtrado)
                {
                    CreadorAligneDimensiones _CreadorDimensiones = new CreadorAligneDimensiones(_uiapp);

                    _CreadorDimensiones.CreateLinearDimensionConrefer_ConTrans(LineaEntrado_alladoIzquierdo.GetEndPoint(0).AsignarZ(ptoCentroPilarAlturaCOrte.Z) - _viewOriginal.RightDirection * 0.8,
                                                                                LineaEntrado_alladoIzquierdo.GetEndPoint(1).AsignarZ(ptoCentroPilarAlturaCOrte.Z) - _viewOriginal.RightDirection * 0.8,
                                                                                lista[0].reference, lista[1].reference, _doc.ActiveView);
                }



                if (resultPAralelo)
                {
                    CreadorAligneDimensiones _CreadorDimensiones2 = new CreadorAligneDimensiones(_uiapp);
                    _CreadorDimensiones2.CreateLinearDimensionConrefer_ConTrans(LineaParalela_alladoabajo.GetEndPoint(0).AsignarZ(ptoCentroPilarAlturaCOrte.Z) + _viewOriginal.ViewDirection * 0.8,
                                                                                LineaParalela_alladoabajo.GetEndPoint(1).AsignarZ(ptoCentroPilarAlturaCOrte.Z) + _viewOriginal.ViewDirection * 0.8,
                                                                                lista2[0].reference, lista2[1].reference,
                                                                                _doc.ActiveView);
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;

        }
        public bool M1_ObtenerLineEntradoEnVista(List<AuxContenedorPlanos> lista)
        {
            try
            {
                XYZ ptoCentro0 = lista[0].Plano_.ObtenerCenterDeCara();
                if (ptoCentro0.IsAlmostEqualTo(XYZ.Zero)) return NosePuedoMsj("Entrando");
                XYZ ptoCentro1 = lista[1].Plano_.ObtenerPtosInterseccionFace(ptoCentro0, -lista[0].normal, true);
                if (ptoCentro1.IsAlmostEqualTo(XYZ.Zero)) return NosePuedoMsj("Entrando");
                double largoDImension = ptoCentro0.DistanceTo(ptoCentro1);

                //buscar cara mas cercana a view
                if (UtilDesglose.IsParallelIgualSentido(lista[0].normal, _viewOriginal.ViewDirection))
                {
                    return M1_1_ObtenerLinea(lista[0], largoDImension);

                }
                else if (UtilDesglose.IsParallelIgualSentido(lista[1].normal, _viewOriginal.ViewDirection))
                {
                    return M1_1_ObtenerLinea(lista[1], largoDImension);
                }
                else
                    return NosePuedoMsj("Entrando");


            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener coordenadas de dimensiones de elemento contenedor de barras (Entrando) ex:{ex.Message}");
                return false;
            }

        }
        private bool M1_1_ObtenerLinea(AuxContenedorPlanos _AuxContenedorPlanos, double largoDImension)
        {
            var listaCurvas = _AuxContenedorPlanos.Plano_.ObtenerListaCurvas();
            var linesHorizontal = listaCurvas.Where(c => UtilDesglose.IsSimilarValor(((Line)c).Direction.Z, 0, 0.001)).FirstOrDefault();
            if (linesHorizontal == null) return NosePuedoMsj("Entrando");
            
            if (UtilDesglose.IsParallelIgualSentido(((Line)linesHorizontal).Direction, _viewOriginal.RightDirection))
            { //linea igual sentido view 
                LineaEntrado_alladoIzquierdo = Line.CreateBound(linesHorizontal.GetEndPoint(0), linesHorizontal.GetEndPoint(0) + -_AuxContenedorPlanos.normal * largoDImension);
            }
            else
            { //linea lsentido opiesto view
                LineaEntrado_alladoIzquierdo = Line.CreateBound(linesHorizontal.GetEndPoint(1), linesHorizontal.GetEndPoint(1) + -_AuxContenedorPlanos.normal * largoDImension);
            }

            return true;
        }

        private bool M2_ObtenerLineParaleloVista(List<AuxContenedorPlanos> lista)
        {
            try
            {
                XYZ ptoCentro0 = lista[0].Plano_.ObtenerCenterDeCara();
                if (ptoCentro0.IsAlmostEqualTo(XYZ.Zero)) return NosePuedoMsj("Paralello");
                XYZ ptoCentro1 = lista[1].Plano_.ObtenerPtosInterseccionFace(ptoCentro0, -lista[0].normal, true);
                if (ptoCentro1.IsAlmostEqualTo(XYZ.Zero)) return NosePuedoMsj("Paralello");
                double largoDImension = ptoCentro0.DistanceTo(ptoCentro1);

                //buscar cara mas a izquierda view
                if (UtilDesglose.IsParallelIgualSentido(lista[0].normal, -_viewOriginal.RightDirection))
                {
                    return M2_1_obtenerLinea_paralelaview(lista[0], largoDImension);

                }
                if (UtilDesglose.IsParallelIgualSentido(lista[1].normal, -_viewOriginal.RightDirection))
                {
                    return M2_1_obtenerLinea_paralelaview(lista[1], largoDImension);
                }
                else
                    return NosePuedoMsj("Paralello");


            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener coordenadas de dimensiones de elemento contenedor de barras (Paralello) ex:{ex.Message}");
                return false;
            }

        }

        private bool M2_1_obtenerLinea_paralelaview(AuxContenedorPlanos _AuxContenedorPlanos, double largoDImension)
        {
            var listaCurvas = _AuxContenedorPlanos.Plano_.ObtenerListaCurvas();
            var linesHorizontal = listaCurvas.Where(c => UtilDesglose.IsSimilarValor(((Line)c).Direction.Z, 0, 0.001)).FirstOrDefault();
            if (linesHorizontal == null) return NosePuedoMsj("Paralello");

            //linea entrado en view
            if (UtilDesglose.IsParallelIgualSentido(((Line)linesHorizontal).Direction, -_viewOriginal.ViewDirection))
            { //linea igual sentido view 
                LineaParalela_alladoabajo = Line.CreateBound(linesHorizontal.GetEndPoint(0), linesHorizontal.GetEndPoint(0) + -_AuxContenedorPlanos.normal * largoDImension);
            }
            else
            { //linea lsentido opiesto view
                LineaParalela_alladoabajo = Line.CreateBound(linesHorizontal.GetEndPoint(1), linesHorizontal.GetEndPoint(1) + -_AuxContenedorPlanos.normal * largoDImension);
            }

            return true;
        }




        private static bool NosePuedoMsj(string v)
        {
            UtilDesglose.ErrorMsg($"No fue posible obtener coordenadas para dimensiones de elemento contenedor de barras");
            return false;
        }
    }

    public class AuxContenedorPlanos
    {
        public bool isok { get; set; }
        public bool IsAnalisada { get; set; }
        public PlanarFace Plano_ { get; set; }
        public XYZ normal { get; private set; }
        public DireccionNormal DireccionNormal_ { get; set; }
        public int Grupoplano_ { get; set; }
        //   public direccion Direccion_ { get; set; }
        public Reference reference { get; set; }

        public AuxContenedorPlanos(PlanarFace plano_)
        {
            this.Plano_ = plano_;
            this.normal = plano_.FaceNormal;
            this.IsAnalisada = false;
            this.isok = true;
        }

        public void Obtenerreferncia()
        {
            reference = ((Face)Plano_).Reference;
        }
    }


}
