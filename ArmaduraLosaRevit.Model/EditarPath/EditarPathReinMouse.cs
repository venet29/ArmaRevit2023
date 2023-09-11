using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using System;
using ArmaduraLosaRevit.Model.EditarPath.Ayuda;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.BorrarSeleccion;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;

namespace ArmaduraLosaRevit.Model.EditarPath
{
    public class EditarPathReinMouse
    {
        //  private ExternalCommandData commandData;
        private UIApplication _uiApplication;
        private UIDocument uidoc;
#pragma warning disable CS0169 // The field 'EditarPathReinMouse._seleccionPath' is never used
        private SeleccionPath _seleccionPath;
#pragma warning restore CS0169 // The field 'EditarPathReinMouse._seleccionPath' is never used
#pragma warning disable CS0169 // The field 'EditarPathReinMouse.distanciaAlargarPAth' is never used
        private double distanciaAlargarPAth;
#pragma warning restore CS0169 // The field 'EditarPathReinMouse.distanciaAlargarPAth' is never used

        public EditarPathReinMouse(UIApplication uiapp)
        {
            //  this.commandData = commandData;
            this._uiApplication = uiapp;
            this.uidoc = uiapp.ActiveUIDocument;
        }


        public Result M2_ExtenderPathaPath(bool borrarSegundoPath = false, double dist = 0)
        {
            try
            {
                //primer path
                SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!SeleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (SeleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                SeleccionPath SeleccionPath_1 = new SeleccionPath(_uiApplication, SeleccionarPathReinfomentConPto);
                SeleccionPath_1.ObtenerBordeDepath();
                SeleccionarPathReinfomentConPto._coordenadaPath = SeleccionPath_1._coordenadaPath;

                DireccionEdicionPathRein direccionEdicionPathRein_1 = SeleccionPath_1.ObtenerPtoSeleccionConMouseEnPathrein();

                if (direccionEdicionPathRein_1 == DireccionEdicionPathRein.Inferior ||
                    direccionEdicionPathRein_1 == DireccionEdicionPathRein.Superior)
                {
                    Util.ErrorMsg("PathToPath Solo se puede utilizar para extender largo de barras,NO su recorrido");
                    return Result.Failed;
                }

                // segundo path
                SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto_2 = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!SeleccionarPathReinfomentConPto_2.SeleccionarPathReinforment()) return Result.Cancelled;

                if (SeleccionarPathReinfomentConPto_2.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                SeleccionPath SeleccionPath_2 = new SeleccionPath(_uiApplication, SeleccionarPathReinfomentConPto_2);
                SeleccionPath_2.ObtenerBordeDepath();
                SeleccionarPathReinfomentConPto_2._coordenadaPath = SeleccionPath_2._coordenadaPath;

                DireccionEdicionPathRein direccionEdicionPathRein_2 = DireccionEdicionPathRein.NONE;
                if (borrarSegundoPath)
                {

                    if (Math.Abs(Util.GetProductoEscalar(SeleccionPath_1.direccionBarras, SeleccionPath_2.direccionBarras)) < 0.98)
                    {
                        Util.ErrorMsg($"Barras deben tener la misma direccion para unirlas");
                        return Result.Failed;
                    }

                    SelecionarPath_IzqDereParaBorrar _SelecionarPath_IzqDereParaBorrar = new SelecionarPath_IzqDereParaBorrar(uidoc, SeleccionPath_1, SeleccionPath_2);

                    _SelecionarPath_IzqDereParaBorrar.ObtenerLadoMAsLejano();
                    direccionEdicionPathRein_2 = _SelecionarPath_IzqDereParaBorrar.newSeleccionPath_2ParaBorrar._direccionEdicionPathRein;

                    if (direccionEdicionPathRein_1 != direccionEdicionPathRein_2)
                    {
                        direccionEdicionPathRein_1 = direccionEdicionPathRein_2;
                        SeleccionPath_1._direccionEdicionPathRein = direccionEdicionPathRein_1;
                        if (direccionEdicionPathRein_1 == DireccionEdicionPathRein.Inferior || direccionEdicionPathRein_1 == DireccionEdicionPathRein.Izquierda)
                            SeleccionPath_1._lineBordeSeleccionadoInicial = Line.CreateBound(SeleccionPath_1._coordenadaPath.p1.GetXY0(), SeleccionPath_1._coordenadaPath.p2.GetXY0());
                        else
                            SeleccionPath_1._lineBordeSeleccionadoInicial = Line.CreateBound(SeleccionPath_1._coordenadaPath.p3.GetXY0(), SeleccionPath_1._coordenadaPath.p4.GetXY0());
                    }

                    SeleccionPath_2 = _SelecionarPath_IzqDereParaBorrar.newSeleccionPath_2ParaBorrar;
                }
                else
                {
                    direccionEdicionPathRein_2 = SeleccionPath_2.ObtenerPtoSeleccionConMouseEnPathrein();
                }



                // comprobar
                ComprobarDireccionPathPath _comprobarDireccionPathPath = new ComprobarDireccionPathPath(direccionEdicionPathRein_1, direccionEdicionPathRein_2);
                if (!_comprobarDireccionPathPath.VerificarDireccion()) return Result.Failed;

                double distanciaAlargarPAth = dist + SeleccionPath_1.ObtenerDistanciaPerpendicularDesdePtoABorde(SeleccionPath_2.PtoMEdioDeBordePAthSeleccionado());
                // Util.InfoMsg("distancia :" + distanciaAlargarPAth.ToString());

                EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, SeleccionarPathReinfomentConPto);


                try
                {
                    using (TransactionGroup trans = new TransactionGroup(uidoc.Document))
                    {
                        trans.Start("ExtenderPath");
                        editarPathRein.EditarPath(distanciaAlargarPAth, direccionEdicionPathRein_1);
                        if (borrarSegundoPath)
                        { BorrarElemento.Borrar1Elemento(uidoc.Document, SeleccionPath_2.pathReinforcement); }
                        trans.Assimilate();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($" ex:{ex.Message} ");

                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ExtenderPathaPath ex:{ex.Message} ");
                return Result.Failed;
            }

        }


        public Result M3_EjecutarExtenderPath(DireccionEdicionPathRein direccionEdicionPathRein, double desplazamientoFoot)
        {
            try
            {

                //seleccionar
                SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!SeleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (SeleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                //desplzar
                EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, SeleccionarPathReinfomentConPto);
                editarPathRein.EditarPath(desplazamientoFoot, direccionEdicionPathRein);
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg("Error al ejecutar EjecutarExtenderPath() : " + ex);
                return Result.Failed;
            }

        }

        public Result M4_ExtederPathDistancia(double distFoot = 0)
        {
            try
            {


                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!SeleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (SeleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                SeleccionPath SeleccionPath = new SeleccionPath(_uiApplication, SeleccionarPathReinfomentConPto);
                SeleccionPath.ObtenerBordeDepath();
                DireccionEdicionPathRein direccionEdicionPathRein = SeleccionPath.ObtenerPtoSeleccionConMouseEnPathrein();


                double distanciaAlargarPAth = distFoot;



                EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, SeleccionarPathReinfomentConPto);
                editarPathRein.EditarPath(distanciaAlargarPAth, direccionEdicionPathRein);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ExtederPathDistancia ex:{ex.Message} ");
                return Result.Failed;
            }
        }

        public Result M5_EjecutarAmbosLados(double DistaIzq, double DistaDere, TipoCasoAlternativo _tipoCasoAlternativo)
        {
            try
            {
                //seleccionar
                SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!SeleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (SeleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                CalculoCoordPathReinforme pathReinformeCalculos =
                    new CalculoCoordPathReinforme(SeleccionarPathReinfomentConPto.PathReinforcement, uidoc.Document);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                CoordenadaPath _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();
                SeleccionarPathReinfomentConPto._coordenadaPath = _coordenadaPath;
                double LargoPath = _coordenadaPath.p2.DistanceTo(_coordenadaPath.p3);

                SeleccionarPathReinfomentConPto.TipoCasoAlternativo_ = _tipoCasoAlternativo;
                //desplzar
                EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, SeleccionarPathReinfomentConPto);

                using (TransactionGroup t = new TransactionGroup(uidoc.Document))
                {
                    t.Start("EditarLargo-NH");

                    editarPathRein.EditarPath(DistaIzq - LargoPath / 2, DireccionEdicionPathRein.Derecha); //esta correcto q sea derecho
                    editarPathRein.EditarPath(DistaDere - LargoPath / 2, DireccionEdicionPathRein.Izquierda); //esta correcto q sea Izq
                    t.Assimilate();
                }
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg("Error al ejecutar EjecutarExtenderPath() : " + ex);
                return Result.Failed;
            }

        }


    }



}
