using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo
{
    public class EditarBarraVLargos : EditarBarraV
    {
        public List<WraperRebarLargo> ListaCurvaBarras { get; set; }
        public double LargoExtender { get; set; }

        List<parametrosRebar> ListaParametrosRebar;
        private WraperRebarLargo ladoBArraExtender;
        private readonly bool isObtenerLArgoConMouse;

        public EditarBarraVLargos(UIApplication uiapp, EditarBarraDTO editarBarraDTO, SeleccionarRebarElemento seleccionarRebarElemento, bool isObtenerLArgoConMouse) : base(uiapp, editarBarraDTO, seleccionarRebarElemento)
        {
            ListaCurvaBarras = new List<WraperRebarLargo>();
            ListaParametrosRebar = new List<parametrosRebar>();
            this.isObtenerLArgoConMouse = isObtenerLArgoConMouse;
        }



        public bool B_EjecutarListaWrapper()
        {
            try
            {
                CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(RebarSeleccion, seleccionarRebarElemento._ptoRebarSeleccion);
                _CreadorListaWraperRebarLargo.Ejecutar();
                ListaCurvaBarras = _CreadorListaWraperRebarLargo.ListaCurvaBarras;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool C_ObtenerLargoExtenderConMouse()
        {
            try
            {

                if (!CrearWorkPLane.Ejecutar(_doc, _view))
                {
                    Util.ErrorMsg($"Error al crear plano de referencia");
                    return false;
                }

                ObjectSnapTypes snapTypes = ObjectSnapTypes.Intersections | ObjectSnapTypes.Nearest;
                XYZ p2_acotar = _uiapp.ActiveUIDocument.Selection.PickPoint(snapTypes, "1) seleccionar pto");
                // sirefere3ncia es null salir
                if (p2_acotar == XYZ.Zero) return false;

                ladoBArraExtender = ListaCurvaBarras.Where(c => c.IsCurvaSeleccionada).FirstOrDefault();

                if (ladoBArraExtender == null) return false;

                XYZ result = ((Line)ladoBArraExtender._curve).ProjectExtendida3D(p2_acotar);

                LargoExtender = Math.Max(result.DistanceTo(ladoBArraExtender.ptoInicial), result.DistanceTo(ladoBArraExtender.ptoFinal));

                if (ladoBArraExtender.IsBarraPrincipal)
                {
                    if (ladoBArraExtender.alargarFin)
                    {
                        XYZ direccionSeleccion = result - ladoBArraExtender.ptoInicial;

                        double valor = Util.GetProductoEscalar(direccionSeleccion, ladoBArraExtender.direccion);

                        if (valor < 0)
                        {
                            Util.ErrorMsg("Error En seleccionar pto de alargamiento. Largo respecto al punto a extender con valor negativo");
                            return false;
                        }
                    }
                    else
                    {
                        XYZ direccionSeleccion = result - ladoBArraExtender.ptoFinal;

                        double valor = Util.GetProductoEscalar(direccionSeleccion, -ladoBArraExtender.direccion);

                        if (valor < 0)
                        {
                            Util.ErrorMsg("Error En seleccionar pto de alargamiento. Largo respecto al punto a extender con valor negativo");
                            return false;
                        }
                        LargoExtender = result.DistanceTo(ladoBArraExtender.ptoFinal);
                    }

                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'C_ObtenerLargoExtenderConMouse'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool E_recortarDiametros(double DeltaUsarMouse_cm)
        {
            try
            {
                //recortar
                double factor = 0;

                factor = 1.0f / 2.0f;

                if (ListaCurvaBarras.Count == 3)
                {
 
                }
                else if (ListaCurvaBarras.Count == 2)
                {
                    if (ladoBArraExtender.IsBarraPrincipal)
                    {
                        if (ladoBArraExtender.ParametrosRebar.letraOriginal == "B" && ladoBArraExtender.alargarFin)
                        { factor = 0; }
                        else if (ladoBArraExtender.ParametrosRebar.letraOriginal == "C" && ladoBArraExtender.alargarInicio)
                        { factor = 0; }
                    }
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    factor = 0;
                }

                LargoExtender = LargoExtender + Util.MmToFoot(Diametro_mm) * factor + Util.CmToFoot(DeltaUsarMouse_cm);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool D_ExtenderBarra(double largoNuevo)
        {
            try
            {
                if (largoNuevo < Util.CmToFoot(15))
                {
                    Util.ErrorMsg("Largo de tramo de barra no  puede ser menor a de 15cm");
                    return false;
                }


                WraperRebarLargo ladoBArraExtender = ListaCurvaBarras.Where(c => c.IsCurvaSeleccionada).FirstOrDefault();
                if (ladoBArraExtender == null) return false;

                string letra_ = ladoBArraExtender.ParametrosRebar.letraOriginal;
                double largoInicial_ = ladoBArraExtender.ParametrosRebar.largo;
                bool IslargoPrincipal_ = ladoBArraExtender.IsBarraPrincipal;


                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarLargorebar-NH");
                    if (IslargoPrincipal_ && ladoBArraExtender.alargarInicio) //mover y extender
                    {
                        if ((!(ladoBArraExtender.ParametrosRebar.letraOriginal == "B")) || (ladoBArraExtender.ParametrosRebar.letraOriginal == "B" && ListaCurvaBarras.Count == 1))
                            ElementTransformUtils.MoveElement(_doc, RebarSeleccion.Id, ladoBArraExtender.direccion * (largoInicial_ - largoNuevo));
                    }
                    else if (ListaCurvaBarras.Count == 2 &&
                        ladoBArraExtender.ParametrosRebar.letraOriginal == "B" &&
                        ladoBArraExtender.alargarFin && ladoBArraExtender.FijacionFinal == FijacionRebar.fijo)
                    {

                        ElementTransformUtils.MoveElement(_doc, RebarSeleccion.Id, ladoBArraExtender.direccion * (largoNuevo - largoInicial_));
                    }


                    ParameterUtil.SetParaInt(RebarSeleccion, letra_, largoNuevo);
                    t.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


    }
}
