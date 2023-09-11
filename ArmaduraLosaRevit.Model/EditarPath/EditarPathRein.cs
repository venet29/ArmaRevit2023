using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.EditarPath.Ayuda;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.EditarPath.Calculos;
using ArmaduraLosaRevit.Model.EditarPath.CambiarLetras;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.EditarPath
{
    public class EditarPathRein
    {
        private readonly PathReinforcement pathReinforcement;
        private readonly UIApplication _uiapp;
        private readonly SeleccionarPathReinfomentConPto _SeleccionarPathReinfomentConPto;
        private ManejadorEditarREbarShapYPAthSymbol _manejadorEditarREbarShapYPAthSymbol;
        private Application _app;
        private Document _doc;
        private UIDocument _uidoc;
        private View _view;
        private Level _Level;

        public EditarPathRein(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto)
        {
            this._app = uiapp.Application;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uidoc = uiapp.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._Level = _doc.ActiveView.GenLevel;
            this.pathReinforcement = _seleccionarPathReinfomentConPto.PathReinforcement;
            this._uiapp = uiapp;
            this._SeleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
            this._manejadorEditarREbarShapYPAthSymbol = _SeleccionarPathReinfomentConPto._AyudaObtenerLArgoPata;

        }

        public bool EditarPath(double _desplazamientoFoot, DireccionEdicionPathRein direccionEdicionPathRein)
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                switch (direccionEdicionPathRein)
                {
                    case DireccionEdicionPathRein.Derecha:

                        if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && RedonderLargoBarras.RedondearFoot5_AltMascercano(_desplazamientoFoot))
                            _desplazamientoFoot = RedonderLargoBarras.NuevoLargobarraFoot;

                        EditarPathReinDerecha editarBarraMoverDerecha = new EditarPathReinDerecha(_uiapp, _SeleccionarPathReinfomentConPto);
                        editarBarraMoverDerecha.M1_moverDerecha(_desplazamientoFoot);

                        //modificar rebarshape y pathsymbol

                        break;
                    case DireccionEdicionPathRein.Izquierda:

                        if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && RedonderLargoBarras.RedondearFoot5_AltMascercano(_desplazamientoFoot))
                            _desplazamientoFoot = RedonderLargoBarras.NuevoLargobarraFoot;

                        EditarPathReinIzquierdo editarBarraMoverIzquierdo = new EditarPathReinIzquierdo(_uiapp, _SeleccionarPathReinfomentConPto);
                        editarBarraMoverIzquierdo.M1_moverIzq(_desplazamientoFoot);

                        //modificar rebarshape y pathsymbol


                        break;
                    case DireccionEdicionPathRein.Superior:
                        EditarPathReinSuperior editarBarraMoverSuperior = new EditarPathReinSuperior(_uiapp, _SeleccionarPathReinfomentConPto);
                        editarBarraMoverSuperior.M1_moverSuperior(_desplazamientoFoot);

                        break;
                    case DireccionEdicionPathRein.Inferior:
                        EditarPathReinInferior editarBarraMoverInferior = new EditarPathReinInferior(_uiapp, _SeleccionarPathReinfomentConPto);
                        editarBarraMoverInferior.M1_moverAbajo(_desplazamientoFoot);
                        break;
                    case DireccionEdicionPathRein.NONE:
                        break;
                    default:
                        break;
                }
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            }
            catch (System.Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                string msje = ex.Message;
                return false;
            }
            return true;


        }

        public bool EditarPath(double distanciaAlargarPAth, double distanciaAlargarPAthFinal, DireccionEdicionPathRein direccionEdicionPathRein)
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                switch (direccionEdicionPathRein)
                {
                    case DireccionEdicionPathRein.Derecha:

                        break;
                    case DireccionEdicionPathRein.Izquierda:

                        break;
                    case DireccionEdicionPathRein.Superior:
                        EditarPathReinSuperior editarBarraMoverSuperior = new EditarPathReinSuperior(_uiapp, _SeleccionarPathReinfomentConPto);
                        //editarBarraMoverSuperior.M1_moverSuperior(distanciaAlargarPAth);

                        EditarPathReinInferior editarBarraMoverInferior = new EditarPathReinInferior(_uiapp, _SeleccionarPathReinfomentConPto);
                        //editarBarraMoverInferior.M1_moverAbajo(distanciaAlargarPAthFinal);

                        //obsser1)
                        if (distanciaAlargarPAth > distanciaAlargarPAthFinal)
                        {
                            editarBarraMoverSuperior.M1_moverSuperior(distanciaAlargarPAth);
                            editarBarraMoverInferior.M1_moverAbajo(distanciaAlargarPAthFinal);
                        }
                        else
                        {
                            editarBarraMoverInferior.M1_moverAbajo(distanciaAlargarPAthFinal);
                            editarBarraMoverSuperior.M1_moverSuperior(distanciaAlargarPAth);
                        }
                        editarBarraMoverInferior.MoverPathSymbolSiCorresponde();

                        break;
                    case DireccionEdicionPathRein.Inferior:
                        EditarPathReinSuperior editarBarraMoverSuperior_i = new EditarPathReinSuperior(_uiapp, _SeleccionarPathReinfomentConPto);
                        //   editarBarraMoverSuperior_i.M1_moverSuperior(distanciaAlargarPAthFinal);

                        EditarPathReinInferior editarBarraMoverInferior_i = new EditarPathReinInferior(_uiapp, _SeleccionarPathReinfomentConPto);
                        // editarBarraMoverInferior_i.M1_moverAbajo(distanciaAlargarPAth);



                        if (distanciaAlargarPAth > distanciaAlargarPAthFinal)
                        {
                            editarBarraMoverInferior_i.M1_moverAbajo(distanciaAlargarPAth);
                            editarBarraMoverSuperior_i.M1_moverSuperior(distanciaAlargarPAthFinal);
                        }
                        else
                        {
                            editarBarraMoverSuperior_i.M1_moverSuperior(distanciaAlargarPAthFinal);
                            editarBarraMoverInferior_i.M1_moverAbajo(distanciaAlargarPAth);
                        }

                        editarBarraMoverInferior_i.MoverPathSymbolSiCorresponde();

                        break;
                    case DireccionEdicionPathRein.NONE:
                        break;
                    default:
                        break;
                }

                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            }
            catch (System.Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                string msje = ex.Message;
                return false;
            }

            return true;
        }
    }
}
