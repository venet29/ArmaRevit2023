using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos.TipoBarras
{
    public class EditBarraF22A : EditBarraF20A
    {
        public EditBarraF22A(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto) : base(uiapp, _seleccionarPathReinfomentConPto)
        {

        }

        /*
        public void M1_3_MOdificarPArametrPrimary_Bar_Length_derecha(double _desplazamientoFoot)
        {
            _LargoPathRinforment = new DatosPathRinforment(_pathReinforcement);
            _LargoPathRinforment.M1_ObtenerDatosGenerales();
            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, _LargoPathRinforment.LargoPrimaria_Foot + _desplazamientoFoot);


            TipoCasoAlternativo TipoCasoAlternativo_ = _seleccionarPathReinfomentConPto.TipoCasoAlternativo_;

            if (_LargoPathRinforment.IsAlternative)
            {
                if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.Definir)
                {
                    ObtenerDatosNUevoPath();


                    double largoBarra = _coordenadaPath.LargosPath + _desplazamientoFoot;

                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largoBarra - TipoCasoAlternativo_.distanciaDefinir_foot);
                    if (_seleccionarPathReinfomentConPto._direccion == "Derecha" || _seleccionarPathReinfomentConPto._direccion == "Superior")
                    {
                        ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, largoBarra - TipoCasoAlternativo_.distanciaDefinir_foot);
                        ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largoBarra);
                        ParameterUtil.SetParaInt(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, 0);
                    }
                    else
                    {
                        double largoOffset = TipoCasoAlternativo_.distanciaDefinir_foot;
                        if (largoOffset > 0)
                            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, largoOffset);
                    }
                }
                else if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.MantenerLargo)
                {
                    //falta ejecutar
                    ObtenerDatosNUevoPath();
                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, _LargoPathRinforment.LargoAlternative_foot);

                    double largoOffset = _LargoPathRinforment.LargoAlternativeOffset_foot + _desplazamientoFoot;
                    if (largoOffset > 0)
                        ParameterUtil.SetParaInt(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, 0);
                }
                else if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.Proporcional)
                {

                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, _LargoPathRinforment.LargoAlternative_foot + _desplazamientoFoot);
                }
            }

        }

        public void M1_3_MOdificarPArametrPrimary_Bar_Length_Izquierda(double _desplazamientoFoot)
        {
            _LargoPathRinforment = new DatosPathRinforment(_pathReinforcement);
            _LargoPathRinforment.M1_ObtenerDatosGenerales();
            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, _LargoPathRinforment.LargoPrimaria_Foot + _desplazamientoFoot);


            TipoCasoAlternativo TipoCasoAlternativo_ = _seleccionarPathReinfomentConPto.TipoCasoAlternativo_;

            if (_LargoPathRinforment.IsAlternative)
            {
                if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.Definir)
                {
                    ObtenerDatosNUevoPath();

                    double largoBarra = _coordenadaPath.LargosPath + _desplazamientoFoot;
                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largoBarra - TipoCasoAlternativo_.distanciaDefinir_foot);

                    if (_seleccionarPathReinfomentConPto._direccion == "Derecha" || _seleccionarPathReinfomentConPto._direccion == "Superior")
                    {
                        //ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, largoBarra - TipoCasoAlternativo_.distanciaDefinir_foot);
                        //ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largoBarra);
                        ParameterUtil.SetParaInt(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, 0);
                    }
                    else
                    {
                      
                        double largoOffset = TipoCasoAlternativo_.distanciaDefinir_foot;
                        if (largoOffset > 0)
                            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, largoOffset);
                    }
                }
                else if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.MantenerLargo)
                {
                    //falta ejecutar
                    ObtenerDatosNUevoPath();
                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, _LargoPathRinforment.LargoAlternative_foot);

                    double largoOffset = _LargoPathRinforment.LargoAlternativeOffset_foot + _desplazamientoFoot;
                    if (largoOffset > 0)
                        ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, largoOffset);
                }
                else if (TipoCasoAlternativo_.TipoCasoAlternativo_ == TipoCasoAlternativo_enum.Proporcional)
                {

                    ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, _LargoPathRinforment.LargoAlternative_foot + _desplazamientoFoot);
                }
            }

        }
        */

    }
}
