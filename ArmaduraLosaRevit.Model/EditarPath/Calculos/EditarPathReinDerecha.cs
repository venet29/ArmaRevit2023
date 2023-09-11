using ArmaduraLosaRevit.Model.EditarPath.Calculos.TipoBarras;
using ArmaduraLosaRevit.Model.EditarPath.CambiarLetras;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos
{
    public class EditarPathReinDerecha : EditarPathRein_Base
    {

        public EditarPathReinDerecha(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto) : base(uiapp, _seleccionarPathReinfomentConPto)
        {
        }

        public void M1_moverDerecha(double _desplazamientoFoot)
        {
            try
            {
                ILetraCambiar newILetraCambiar = FactoryLetraCambiar.ObtenerParametroLetra(_doc, _seleccionarPathReinfomentConPto.PathReinforcement, _tipobarra);

                XYZ ptOrigen = UtilPathReinforment.ObtenerOrigenPathReinf(_pathReinforcement, _doc);
                XYZ ptDireccionPath = UtilPathReinforment.ObtenerDireccionPathReinf(_pathReinforcement, _doc);
                XYZ direcion1_to_4 = _seleccionarPathReinfomentConPto._coordenadaPath.Obtenerdirecion1_to_4();
                XYZ ptoDesplazadoDesdeOrigen = ptOrigen + direcion1_to_4 * _desplazamientoFoot; //M1_2_ObtenerPtoDesplazadoDerechaPAth(_desplazamientoFoot, ptOrigen, direcion4_to_1);
                XYZ DeltaDesplazamiento = ptoDesplazadoDesdeOrigen - ptOrigen;

             //   

                using (Transaction tr = new Transaction(_doc, "AlargarDere"))
                {
                    tr.Start();
                    //desplazar path
                    _pathReinforcement.Location.Move(DeltaDesplazamiento);

                    SegunTipoBArra(_desplazamientoFoot);

                    if (_seleccionarPathReinfomentConPto._barraTipo.ToString().Contains("FUND_BA"))
                    {
                        ActualizarLargoBarras _ActualizarBarraTipo = new ActualizarLargoBarras(_uiapp, _view);
                        _ActualizarBarraTipo.Ejecutar_SinTrans(_seleccionarPathReinfomentConPto.PathReinforcement);
                    }

                    if (newILetraCambiar != null) newILetraCambiar.Ejecutar();

                    tr.Commit();
                }

                _manejadorEditarREbarShapYPAthSymbol.M1_ActualizarPAtaPAthReinforment_ConTrans(TipoDireccion.Derecha);
            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($"Error al desplazar path derecha ex:{ex.Message}");
            }
        }

        private void SegunTipoBArra(double _desplazamientoFoot)
        {
            if (_tipobarra == "f22" || _tipobarra == "f22a" || _tipobarra == "f22Inv" || _tipobarra == "f22aInv")
            {
                EditBarraF22A _EditBarraF22A = new EditBarraF22A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF22A.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            //else if (_tipobarra == "f22Inv" || _tipobarra == "f22aInv")
            //{
            //    EditBarraF22A _EditBarraF22A = new EditBarraF22A(_uiapp, _seleccionarPathReinfomentConPto);
            //    _EditBarraF22A.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            //}
            else if (_tipobarra == "f22b" || _tipobarra == "f22bInv")
            {
                EditBarraF22B _EditBarraF22B = new EditBarraF22B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF22B.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f21" || _tipobarra == "f21a")
            {
                EditBarraF21A _EditBarraF21 = new EditBarraF21A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF21.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f21b")
            {
                EditBarraF21B _EditBarraF21 = new EditBarraF21B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF21.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f20" || _tipobarra == "f20a" || _tipobarra == "f20aInv")
            {
                EditBarraF20A _EditBarraF20A = new EditBarraF20A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20A.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f20b" || _tipobarra == "f20bInv")
            {
                EditBarraF20B _EditBarraF20B = new EditBarraF20B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20B.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }

            else if (_tipobarra == "f18")
            {
                EditBarraF18 _EditBarraF18 = new EditBarraF18(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF18.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f19")
            {
                EditBarraF19 _EditBarraF19 = new EditBarraF19(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF19.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f16")
            {
                EditBarraF16 _EditBarraF16 = new EditBarraF16(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF16.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f17a")
            {
                EditBarraF17A _EditBarraF17A = new EditBarraF17A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF17A.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f17b")
            {
                EditBarraF17B _EditBarraF17B = new EditBarraF17B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF17B.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f16a")
            {
                EditBarraF16A _EditBarraF16A = new EditBarraF16A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF16A.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else if (_tipobarra == "f16b")
            {
                EditBarraF16B _EditBarraF16B = new EditBarraF16B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF16B.M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(_desplazamientoFoot);
            }
            else
                M1_3_MOdificarPArametrPrimary_Bar_Length(_desplazamientoFoot);
        }
    }
}
