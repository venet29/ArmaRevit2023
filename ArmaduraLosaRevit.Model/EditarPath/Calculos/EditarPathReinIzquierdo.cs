using ArmaduraLosaRevit.Model.EditarPath.Calculos.TipoBarras;
using ArmaduraLosaRevit.Model.EditarPath.CambiarLetras;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos
{
    public class EditarPathReinIzquierdo : EditarPathRein_Base
    {


        public EditarPathReinIzquierdo(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto) : base(uiapp, _seleccionarPathReinfomentConPto)
        {

        }


        public void M1_moverIzq(double _desplazamientoFoot)
        {
            ILetraCambiar newILetraCambiar = FactoryLetraCambiar.ObtenerParametroLetra(_doc, _seleccionarPathReinfomentConPto.PathReinforcement, _tipobarra);

            try
            {
                using (Transaction tr = new Transaction(_doc, "AlargarIzq"))
                {
                    tr.Start();
                    SeguntipoBarra(_desplazamientoFoot);

                    if (_seleccionarPathReinfomentConPto._barraTipo.ToString().Contains("FUND_BA"))
                    {
                        ActualizarLargoBarras _ActualizarBarraTipo = new ActualizarLargoBarras(_uiapp, _view);
                        _ActualizarBarraTipo.Ejecutar_SinTrans(_seleccionarPathReinfomentConPto.PathReinforcement);
                    }
                    if (newILetraCambiar.Isok) newILetraCambiar.Ejecutar();

                    tr.Commit();
                }

                _manejadorEditarREbarShapYPAthSymbol.M1_ActualizarPAtaPAthReinforment_ConTrans(TipoDireccion.Izquierda);
                //  _seleccionarPathReinfomentConPto._AyudaObtenerLArgoPata.ActualizarPAtaPAthReinforment_ConTrans();
            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($"Error al desplazar path izquierda ex:{ex.Message}");
            }
        }

        private void SeguntipoBarra(double _desplazamientoFoot)
        {
            if (_tipobarra == "f22" || _tipobarra == "f22a" || _tipobarra == "f22aInv")
            {
                EditBarraF22A _EditBarraF20A = new EditBarraF22A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20A.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            if (_tipobarra == "f22b" || _tipobarra == "f22bInv")
            {
                EditBarraF22B _EditBarraF20B = new EditBarraF22B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20B.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f21" || _tipobarra == "f21a")
            {
                EditBarraF21A _EditBarraF21 = new EditBarraF21A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF21.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f21b")
            {
                EditBarraF21B _EditBarraF21 = new EditBarraF21B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF21.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f20" || _tipobarra == "f20a" || _tipobarra == "f20aInv" )
            {
                EditBarraF20A _EditBarraF20 = new EditBarraF20A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if ( _tipobarra == "f20b" || _tipobarra == "f20bInv")
            {
                EditBarraF20B _EditBarraF20 = new EditBarraF20B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF20.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f18")
            {
                EditBarraF18 _EditBarraF18 = new EditBarraF18(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF18.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f19")
            {
                EditBarraF19 _EditBarraF19 = new EditBarraF19(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF19.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if ( _tipobarra == "f17a")
            {
                EditBarraF17A _EditBarraF17A = new EditBarraF17A(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF17A.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f17b")
            {
                EditBarraF17B _EditBarraF17B = new EditBarraF17B(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF17B.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }
            else if (_tipobarra == "f16" || _tipobarra == "f16a" || _tipobarra == "f16b")
            {
                EditBarraF16 _EditBarraF16 = new EditBarraF16(_uiapp, _seleccionarPathReinfomentConPto);
                _EditBarraF16.M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(_desplazamientoFoot);
            }

            else
                M1_3_MOdificarPArametrPrimary_Bar_Length(_desplazamientoFoot);
        }


    }
}
