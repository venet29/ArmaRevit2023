using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion
{
    public class EditarPathReinMouse_ExtederPathA2punto
    {
        //  private ExternalCommandData commandData;
        private UIApplication _uiApplication;
        private Document _doc;
        private UIDocument uidoc;
        private SeleccionPath _seleccionPath;
        private double distanciaAlargarPAth;
        private XYZ _ptoSelecMouseInicial;
        private XYZ _ptoSelecMouseFinal;
        private double distanciaAlargarPAthFinal;
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;
        private List<ObjectSnapTypes> listaSnap;
        public EditarPathReinMouse_ExtederPathA2punto(UIApplication uiapp, System.Collections.Generic.List<ObjectSnapTypes> listaSnap = null)
        {
            //  this.commandData = commandData;
            this._uiApplication = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.uidoc = uiapp.ActiveUIDocument;
            this.distanciaAlargarPAth = 0;
            this.distanciaAlargarPAthFinal = 0;
            this.listaSnap = (listaSnap == null ? new List<ObjectSnapTypes>() : listaSnap);
        }


        public Result M1_ExtederPathApunto(TipoCasoAlternativo _tipoCasoAlternativo)
        {
            try
            {
                double distFoot = _tipoCasoAlternativo.distancia_foot;
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                _seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!_seleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (!_seleccionarPathReinfomentConPto.Crear_AyudaObtenerLArgoPata(_tipoCasoAlternativo.distanciaDefinir_foot)) return Result.Cancelled;


                if (_seleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                _seleccionarPathReinfomentConPto.TipoCasoAlternativo_ = _tipoCasoAlternativo;

                _seleccionPath = new SeleccionPath(_uiApplication, _seleccionarPathReinfomentConPto);
                _seleccionPath.ObtenerBordeDepath();

                var result = _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein2Ptos();

                if (result == DireccionEdicionPathRein.NONE) return Result.Failed;

                return M1_ExtederPathApuntov2(_seleccionPath, distFoot);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error M1_ExtederPathApunto ex:{ex.Message} ");
                return Result.Failed;
            }

        }

        public Result M1_ExtederPathApunto(SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto, double distFoot = 0)
        {
            try
            {
                this._seleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
                _seleccionPath = new SeleccionPath(_uiApplication, _seleccionarPathReinfomentConPto);
                _seleccionPath.ObtenerBordeDepath();
                _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein2Ptos();

                return M1_ExtederPathApuntov2(_seleccionPath, distFoot);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error M1_ExtederPathApunto ex:{ex.Message} ");
                return Result.Failed;
            }

        }

        private Result M1_ExtederPathApuntov2(SeleccionPath _seleccionPath, double distFoot = 0)
        {
            try
            {

                if (_seleccionPath._direccionEdicionPathRein == DireccionEdicionPathRein.Superior || _seleccionPath._direccionEdicionPathRein == DireccionEdicionPathRein.Inferior)
                {

                    if (!SeleccionarPtoInicial()) return Result.Cancelled;

                    if (!SeleccionarPtoFinal()) return Result.Cancelled;

                    ObtenerDistanciaAlargar(distFoot, _ptoSelecMouseInicial);
                    ObtenerDistanciaAlargarPtoFinal(distFoot, _ptoSelecMouseFinal);

                    if (distanciaAlargarPAthFinal < 0 && distanciaAlargarPAth < 0)
                    {
                        RedefinirPtosMouse();

                        ObtenerDistanciaAlargar(distFoot, _ptoSelecMouseInicial);
                        ObtenerDistanciaAlargarPtoFinal(distFoot, _ptoSelecMouseFinal);
                    }

                    if (!RedimensionarVectores()) return Result.Cancelled;
                }
                else
                {
                    if (!SeleccionarPtoInicial()) return Result.Cancelled;

                    ObtenerDistanciaAlargar(distFoot, _ptoSelecMouseInicial);
                }

                _seleccionarPathReinfomentConPto._coordenadaPath = _seleccionPath._coordenadaPath;


                using (TransactionGroup transGroup = new TransactionGroup(uidoc.Document))
                {
                    transGroup.Start("ExtederPathApunto-NH");
                    EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, _seleccionarPathReinfomentConPto);

                    if (_seleccionPath._direccionEdicionPathRein == DireccionEdicionPathRein.Superior || _seleccionPath._direccionEdicionPathRein == DireccionEdicionPathRein.Inferior)
                        editarPathRein.EditarPath(distanciaAlargarPAth, distanciaAlargarPAthFinal, _seleccionPath._direccionEdicionPathRein);
                    else
                        editarPathRein.EditarPath(distanciaAlargarPAth, _seleccionPath._direccionEdicionPathRein);

                    transGroup.Assimilate();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error M1_ExtederPathApunto ex:{ex.Message} ");
                return Result.Failed;
            }

        }

        private void RedefinirPtosMouse()
        {
            double distaciaPtoIniciaLineaInicia = Util.ObtenerDistanciaPerpendicularDesdePtoALine_XY0(_seleccionPath._lineBordeSeleccionadoInicial, _ptoSelecMouseInicial);
            double distaciaPtoIniciaLineaFinal = Util.ObtenerDistanciaPerpendicularDesdePtoALine_XY0(_seleccionPath._lineBordeSeleccionadoInicial, _ptoSelecMouseFinal);
            if (distaciaPtoIniciaLineaFinal < distaciaPtoIniciaLineaInicia)
            {
                XYZ _ptoSelecMouseInicial_aux = _ptoSelecMouseInicial;
                _ptoSelecMouseInicial = _ptoSelecMouseFinal;
                _ptoSelecMouseFinal = _ptoSelecMouseInicial_aux;

            }
        }

        // tambien corregir  BarraRoom.AcortarCUrva
        private bool RedimensionarVectores()
        {
            try
            {

                double LargoRecorridoFoot = _seleccionPath._coordenadaPath.p1.DistanceTo(_seleccionPath._coordenadaPath.p2);
                double largoTotal = distanciaAlargarPAthFinal + distanciaAlargarPAth + LargoRecorridoFoot;

                double espaciamiento = Convert.ToDouble(_seleccionarPathReinfomentConPto._espaciamiento);
                double diametro_mm = Convert.ToDouble(_seleccionarPathReinfomentConPto._diametro);

                double cantidadBArras = Math.Round((largoTotal) / Util.CmToFoot(espaciamiento), 5);
                long CantidadBarra = (long)cantidadBArras;

                double parteDecimal = Util.ParteDecimal(cantidadBArras);

                if (parteDecimal < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) CantidadBarra -= 1;
                if (CantidadBarra < 1) CantidadBarra = 1;
                double largoREcorridoborrar = largoTotal - (CantidadBarra * Util.CmToFoot(espaciamiento) + Util.MmToFoot(diametro_mm));

                distanciaAlargarPAth = distanciaAlargarPAth - largoREcorridoborrar / 2;
                distanciaAlargarPAthFinal = distanciaAlargarPAthFinal - largoREcorridoborrar / 2;
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"RedimensionarVectores  ex:{ex.Message} ");
                return false;
            }
            return true;
        }

        private bool SeleccionarPtoInicial()
        {
            ObjectSnapTypes snapTypes = ObjectSnapTypes.None;

            if (!CrearWorkPLane.Ejecutar(_doc, _doc.ActiveView))
            {
                Util.ErrorMsg($"Error al crear plano de referencia");
                return false;
            }

            if (listaSnap.Count > 0)
            {
                foreach (ObjectSnapTypes item in listaSnap)
                {
                    snapTypes |= item;
                }
            }
            else
                snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Nearest;

            try
            {
                _ptoSelecMouseInicial = uidoc.Selection.PickPoint(snapTypes, "Element:Seleccionar primer punto ");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }
        private bool SeleccionarPtoFinal()
        {
            ObjectSnapTypes snapTypes = ObjectSnapTypes.None;
            if (listaSnap.Count > 0)
            {
                foreach (ObjectSnapTypes item in listaSnap)
                {
                    snapTypes |= item;
                }
            }
            else
                snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Nearest;
            try
            {
                _ptoSelecMouseFinal = uidoc.Selection.PickPoint(snapTypes, "Element:Seleccionar segundo punto ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        //resum= si valor es negativo se acorta el lado del path
        //       si es positivo el path se alarga
        private void ObtenerDistanciaAlargar(double distFoot, XYZ _ptoSelecMouse)
        {
            distanciaAlargarPAth = _seleccionPath.ObtenerDistanciaPerpendicularDesdePtoABorde(_ptoSelecMouse);
            distanciaAlargarPAth = Util.ObtenerDistanciaPerpendicularDesdePtoALine_XY0(_seleccionPath._lineBordeSeleccionadoInicial, _ptoSelecMouse);
            
            // si el despalzamiento es menor a 0.01mm no lo considera
            distanciaAlargarPAth = (Math.Abs(distanciaAlargarPAth) < Util.CmToFoot(0.01) ? 0 : distanciaAlargarPAth);

            XYZ DireccionDesdeCentroAlBorde = _seleccionPath.ObtenerDireccionDesdeCentroAlBorde(_seleccionPath._coordenadaPath.centro);
            XYZ DireccionBordePtoMouse = _seleccionPath.ObtenerDireccionBordeAlPtoMouse(_ptoSelecMouse);

            if (Util.GetProductoEscalar(DireccionDesdeCentroAlBorde.Normalize(), DireccionBordePtoMouse.Normalize()) > 0)
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * 1;
            else
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * -1;

            if (RedonderLargoBarras.RedondearFoot1_masbajo(distanciaAlargarPAth))
            {
                distanciaAlargarPAth = RedonderLargoBarras.NuevoLargobarraFoot;
            }
        }

        private void ObtenerDistanciaAlargarPtoFinal(double distFoot, XYZ _ptoSelecMouse)
        {
            distanciaAlargarPAthFinal = Util.ObtenerDistanciaPerpendicularDesdePtoALine_XY0(_seleccionPath._lineBordeSeleccionadoFinal, _ptoSelecMouse);

            XYZ DireccionDesdeCentroAlBorde = _seleccionPath.ObtenerDireccionDesdeCentroAlBordePtoFinal(_seleccionPath._coordenadaPath.centro);
            XYZ DireccionBordePtoMouse = _seleccionPath.ObtenerDireccionBordeAlPtoMousePtoFinal(_ptoSelecMouse);

            if (Util.GetProductoEscalar(DireccionDesdeCentroAlBorde.Normalize(), DireccionBordePtoMouse.Normalize()) > 0)
                distanciaAlargarPAthFinal = distFoot + distanciaAlargarPAthFinal * 1;
            else
                distanciaAlargarPAthFinal = distFoot + distanciaAlargarPAthFinal * -1;

            if (RedonderLargoBarras.RedondearFoot1_masbajo(distanciaAlargarPAthFinal))
            {
                distanciaAlargarPAthFinal = RedonderLargoBarras.NuevoLargobarraFoot;
            }
        }
    }
}
