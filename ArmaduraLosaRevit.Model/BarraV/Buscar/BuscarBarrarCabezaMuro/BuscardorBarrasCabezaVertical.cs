using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro
{
    public class BuscardorBarrasCabezaVertical
    {
        private Document _doc;
        private View _view;
        private View3D _view3D_paraBuscar;
        private XYZ ptoInicial;
        private double largoRecorrido;
        private double diametroMM;
        private XYZ ptoFinal;
        private List<rebarVerticalEncontrada> listaRebarVerticalEncontrada;
        rebarVerticalEncontrada _NewRebarVerticalAnterior;
        private int contador;
        List<ReferenceWithContext> listaObjetos;
        XYZ ptoAnterior;
        public XYZ ptoInicial_resultado { get; set; }

        public double largoRecorrido_resultado { get; private set; }

        public XYZ ptoRebar_resultado { get; set; }
        public double DesplazamientoRespecInicio { get; internal set; }

        public BuscardorBarrasCabezaVertical(Document doc, View3D view3D_paraBuscar, XYZ ptoInicial, double largoRecorrido, double diametroMM)
        {
            this._doc = doc;
            this._view = _doc.ActiveView;
            _view3D_paraBuscar = view3D_paraBuscar;
            this.ptoInicial = ptoInicial;
            this.largoRecorrido = largoRecorrido + Util.MmToFoot(diametroMM) * 2;
            this.ptoFinal = ptoInicial + _view.RightDirection * largoRecorrido;
            this.diametroMM = diametroMM;
            this.listaRebarVerticalEncontrada = new List<rebarVerticalEncontrada>();

        }

        public bool BuscarREbar(XYZ VectorBusqueda)
        {
            this.ptoInicial = ptoInicial + -_view.ViewDirection * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (diametroMM / 2.0f) / 10f);
            // + _view.RightDirection *Util.MmToFoot(diametroMM);
            try
            {
                if (!M1_ObtenerListaDeBarrasEncontradsa(VectorBusqueda)) return false;

                if (!M2_ObtenerlargoRecorrido_resultado()) return false;
            }
            catch (System.Exception ex)
            {
             
                Debug.WriteLine($"ex : {ex.Message}");
                return false;
            }
            return true;
        }

        private bool M2_ObtenerlargoRecorrido_resultado()
        {
            try
            {
                if (listaRebarVerticalEncontrada.Count >= 1)
                {
                    var primer = listaRebarVerticalEncontrada.OrderBy(c=>c.Contador).Where(c => c.DesplazamietoRespetoPosterior > ConstNH.CONST_DESPLAMIENTO_RESPECTO_POSTERIOR_FOOT).FirstOrDefault();
                    if (primer == null) return false;

                    var final = listaRebarVerticalEncontrada.OrderByDescending(c => c.Contador).Where(c => c.DesplazamietoRespetoAnterior > ConstNH.CONST_DESPLAMIENTO_RESPECTO_ANTERIOR_FOOT).FirstOrDefault();
                    if (final == null) return false;

                    // ptoInicial_resultado = result.ptoAnterior.AsignarZ(ptoInicial.Z);
                    largoRecorrido_resultado = primer.ptoActual.DistanceTo(final.ptoActual);
                    DesplazamientoRespecInicio = primer.DesplazamientoRespecInicio;

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool M1_ObtenerListaDeBarrasEncontradsa(XYZ VectorBusqueda)
        {
            try
            {
                M1_1_BuscarBarrasConRayoProyeccion(VectorBusqueda);

                contador = 0;
                ptoAnterior = ptoInicial;

                //a)
                M1_2_crearPrimerPTolista();


                //b)
                foreach (var ref2 in listaObjetos)

                {
                    if (ref2 != null)
                    {
                        if (ref2.Proximity < largoRecorrido)
                        {
                            Reference RebarRef = ref2.GetReference();
                            XYZ ptoSeleccion = RebarRef.GlobalPoint;
                            Rebar RebarElement = (Rebar)_doc.GetElement(RebarRef);

                            if (RebarElement.Category.Name == "Structural Rebar")// && listaRebarVerticalEncontrada.Find(c=> c._Rebar.Id== RebarElement.Id)==null)
                            {
                                rebarVerticalEncontrada _NewRebarVerticalEncontrada = new rebarVerticalEncontrada()
                                {
                                    Proximidad = ref2.Proximity,
                                    ptoAnterior = ptoAnterior,
                                    ptoActual = ptoSeleccion,
                                    Contador = contador,
                                    _Rebar = RebarElement,
                                    DesplazamietoRespetoAnterior = ptoSeleccion.DistanceTo(ptoAnterior),
                                    DesplazamietoRespetoPosterior = 0,
                                    DesplazamientoRespecInicio = ptoSeleccion.DistanceTo(ptoInicial),
                                    DesplazamientoRespecFinal = ptoSeleccion.DistanceTo(ptoFinal)


                                };
                                ptoAnterior = ptoSeleccion;

                                if (_NewRebarVerticalAnterior != null)
                                    _NewRebarVerticalAnterior.DesplazamietoRespetoPosterior = _NewRebarVerticalEncontrada.ptoActual.DistanceTo(_NewRebarVerticalAnterior.ptoActual);

                                _NewRebarVerticalAnterior = _NewRebarVerticalEncontrada;

                                contador += 1;
                                listaRebarVerticalEncontrada.Add(_NewRebarVerticalEncontrada);

                            }
                        }
                    }
                }

                //c
                M1_3_crearFinal();



                //d
              //  M1_4_CrearComentarios_borrar();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool M1_1_BuscarBarrasConRayoProyeccion(XYZ VectorBusqueda)
        {
            try
            {

                ptoInicial_resultado = ptoInicial;
                largoRecorrido_resultado = largoRecorrido;

                double[] lista = new double[] { 0, 0.4, 0.5, 0.6, 0.8, 0.9, 1.1, 1.25, 1.4, 1.8 };

                listaObjetos = new List<ReferenceWithContext>(); // CrearLineaAux(ptoInicial, VectorBusqueda, 0, false);
                int cont = -1;
                while (listaObjetos.Count == 0 && lista.Length - 1 > cont)
                {
                    ++cont;
                    listaObjetos = M1_1_1_CrearLineaAux(ptoInicial, VectorBusqueda, lista[cont], false);
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error: 'BuscarBarrasConRayoProyeccion'");
                return false;
            }

            return true;
        }
        private List<ReferenceWithContext> M1_1_1_CrearLineaAux(XYZ ptoInicial, XYZ VectorBusqueda, double valorHaciaDentroView_Cm, bool ISDibujaLinea = false)
        {

            List<ReferenceWithContext> listaObjetos;
            //_view.ViewDirection apuna saliendo de pantalla
            XYZ _newpto = ptoInicial + -_view.ViewDirection * Util.CmToFoot(valorHaciaDentroView_Cm);

            if (ISDibujaLinea)
            {
                CrearLIneaAux _CrearLIneaAu1x = new CrearLIneaAux(_doc);
                _CrearLIneaAu1x.CrearLinea(_newpto, _newpto + VectorBusqueda * largoRecorrido_resultado);
            }

            ElementCategoryFilter filterRebar = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
            ReferenceIntersector ri = new ReferenceIntersector(filterRebar, FindReferenceTarget.Element, _view3D_paraBuscar);
            listaObjetos = ri.Find(_newpto, VectorBusqueda)
                             .Where(x => x.Proximity < largoRecorrido).OrderBy(c => c.Proximity).ToList();
            if (listaObjetos.Count > 0) ConstNH.sbLog.AppendLine($"Malla Entro en :{valorHaciaDentroView_Cm} ");
            return listaObjetos;
        }

        private bool M1_2_crearPrimerPTolista()
        {
            try
            {
                rebarVerticalEncontrada _NewRebarVerticalEncontrada = new rebarVerticalEncontrada()
                {
                    Proximidad = 0,
                    ptoAnterior = ptoInicial,
                    ptoActual = ptoInicial,
                    Contador = 0,
                    _Rebar = null,
                    DesplazamietoRespetoAnterior = 0,
                    DesplazamietoRespetoPosterior = 0,
                    DesplazamientoRespecInicio = 0,
                    DesplazamientoRespecFinal = ptoInicial.DistanceTo(ptoFinal)


                };
                listaRebarVerticalEncontrada.Add(_NewRebarVerticalEncontrada);

                _NewRebarVerticalAnterior = _NewRebarVerticalEncontrada;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool M1_3_crearFinal()
        {
            try
            {
                rebarVerticalEncontrada _NewRebarVerticalEncontrada = new rebarVerticalEncontrada()
                {
                    Proximidad = ptoFinal.DistanceTo(ptoInicial),
                    ptoAnterior = ptoAnterior,
                    ptoActual = ptoFinal,
                    Contador = contador,
                    _Rebar = null,
                    DesplazamietoRespetoAnterior = ptoFinal.DistanceTo(ptoAnterior),
                    DesplazamietoRespetoPosterior = 0,
                    DesplazamientoRespecInicio = ptoFinal.DistanceTo(ptoInicial),
                    DesplazamientoRespecFinal = 0
                };
                listaRebarVerticalEncontrada.Add(_NewRebarVerticalEncontrada);

                _NewRebarVerticalAnterior = _NewRebarVerticalEncontrada;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private void M1_4_CrearComentarios_borrar()
        {
            if (listaRebarVerticalEncontrada.Count < 1) return;
            listaRebarVerticalEncontrada.ForEach(c => Debug.WriteLine($"id:{ c._Rebar?.Id}    " +
       $"Proximidad:{Math.Round(c.Proximidad, 3)}   " +
       $"ptoAnterior:{c.ptoAnterior.REdondearString_foot(1)}    " +
       $"ptoActual:{c.ptoActual.REdondearString_foot(1)}   " +
       $"DesplaRespAnterior:{Math.Round(Util.FootToCm(c.DesplazamietoRespetoAnterior), 3)}     " +
       $"DesplaRespPosterior:{Math.Round(Util.FootToCm(c.DesplazamietoRespetoPosterior), 3)}   " +
       $"DesplaRespInicio{Math.Round(Util.FootToCm(c.DesplazamientoRespecInicio), 3)}      " +
       $"DesplaRespFinal:{Math.Round(Util.FootToCm(c.DesplazamientoRespecFinal), 3)}   "));
        }

    }
}