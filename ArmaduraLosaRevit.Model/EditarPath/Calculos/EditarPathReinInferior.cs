using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos
{
    public class EditarPathReinInferior : EditarPathRein_Base
    {
        protected XYZ _deltaVectorDesplazamiento;

        public EditarPathReinInferior(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto) : base(uiapp, _seleccionarPathReinfomentConPto)
        {
        }



        public void M1_moverAbajo(double _desplazamientoFoot)
        {
            try
            {

                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("AlargarInferiorGroup");

                    XYZ DireccionPath = UtilPathReinforment.ObtenerDireccionPathReinf(_pathReinforcement, _doc);
                    _deltaVectorDesplazamiento = M1_2_ObtenerPtoDesplazadoAbajoPAth(_desplazamientoFoot, DireccionPath);
                    //ob2_EjecutarReAsignarurvaMocel
                    EjecutarReAasignarCurvaModelEnPath(_deltaVectorDesplazamiento);
                    XYZ _deltaVectorDesplazamientoCero = new XYZ(0, 0, 0);
                    EjecutarReAasignarCurvaModelEnPath(_deltaVectorDesplazamientoCero);
                    //  EjecutarLetraPararametroCambia(); no aplica en este sentido
                    moverPathsimbol();
                    EjecutarCambiarValoreF_cuantia();
                    transGroup.Assimilate();
                }
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg($"Error al Extender PathReinforment hacia Abajo ex:{ex.Message}");
            }
        }

        private void EjecutarReAasignarCurvaModelEnPath(XYZ DeltaDesplazamiento)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "AlargarInferior"))
                {
                    tr.Start();
                    //  Line.CreateBound(PtoOrigen,ptoFinal );
                    //Line.GetEndPoint(0) --> ptoOrigen
                    ////Line.GetEndPoint(1) --> ptoFinal
                    CurveElement MLINE = (CurveElement)_doc.GetElement(_pathReinforcement.GetCurveElementIds().ToList().FirstOrDefault());
                    if (MLINE == null)
                    {
                        Util.ErrorMsg($"Error al Extender PathReinforment hacia Abajo. Valor de CurveElement igual null");
                        tr.RollBack();
                    }

                    Curve MLINECurve = MLINE.GeometryCurve;
                    XYZ nuevoPtoOrigen = MLINECurve.GetEndPoint(1) + DeltaDesplazamiento;
                    MLINE.GeometryCurve = (Curve)Line.CreateBound(nuevoPtoOrigen, MLINECurve.GetEndPoint(0));
                    moverPathsimbol();
                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg($"Error al Extender PathReinforment hacia Abajo ex:{ex.Message}");
            }
        }

        private void EjecutarCambiarValoreF_cuantia()
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "cambiandoF"))
                {
                    tr.Start();
                    ITagF newITagF; newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, _seleccionarPathReinfomentConPto.PathReinforcement, _seleccionarPathReinfomentConPto._tipobarra);
                    if (newITagF != null) newITagF.Ejecutar();

                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg($"Error al Extender PathReinforment hacia Abajo ex:{ex.Message}");
            }
        }


        private XYZ M1_2_ObtenerPtoDesplazadoAbajoPAth(double _desplazamientoFoot, XYZ ptDireccionPath)
        {
            XYZ ptOrigen = new XYZ(0, 0, 0);
            double AnguloOrigenGrado = obtenerAnguloOrigenGrado(ptDireccionPath);

           // XYZ ptoDesplzadoUnaDistanciaDeterminada = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptOrigen, Util.GradosToRadianes(AnguloOrigenGrado), _desplazamientoFoot, 0);
            XYZ ptoDesplzadoUnaDistanciaDeterminada = ptDireccionPath * _desplazamientoFoot;
            return ptoDesplzadoUnaDistanciaDeterminada;
        }

        private double obtenerAnguloOrigenGrado(XYZ ptOrigen)
        {
            double AnguloDeOrigenGrado = Util.GetAnguloVectoresEnGrados_enPlanoXY(ptOrigen);
            return AnguloDeOrigenGrado;
        }

        public void MoverPathSymbolSiCorresponde()
        {
            try
            {
                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_seleccionarPathReinfomentConPto.PathReinforcement, _doc);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                CoordenadaPath _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();
                XYZ potTag = _seleccionarPathReinfomentConPto.PathReinforcementSymbol.TagHeadPosition;
                List<XYZ> list = _coordenadaPath.GetListaXYZ();
                List<List<XYZ>> listEnList = new List<List<XYZ>>();
                listEnList.Add(list);
                if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(potTag, listEnList) == false)
                {

                    XYZ direccionHAciaCenrtol = (list[1] - list[0]).Normalize();
                    double distancia = list[0].DistanceTo(list[1]) / 2;
                    XYZ PtoEnlinea1_4 = Line.CreateBound(list[0].AsignarZ(0), list[3].AsignarZ(0)).ProjectExtendidaXY0(potTag.AsignarZ(0)).
                                            AsignarZ(potTag.Z);
                    potTag = PtoEnlinea1_4 + direccionHAciaCenrtol * distancia;


                    using (Transaction tr = new Transaction(_doc, "cambiandoF"))
                    {
                        tr.Start();
                        _seleccionarPathReinfomentConPto.PathReinforcementSymbol.TagHeadPosition = potTag;// _coordenadaPath.centro;
                        tr.Commit();
                    }

                }

            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg($"Error MoverPathSymbolSiCorresponde ex:{ex.Message}");
            }
        }
    }
}
