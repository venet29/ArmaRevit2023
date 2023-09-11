using ArmaduraLosaRevit.Model.EditarPath.CambiarLetras;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos
{
    public class EditarPathRein_Base
    {
        protected readonly PathReinforcement _pathReinforcement;
#pragma warning disable CS0169 // The field 'EditarPathRein_Base.letraPararametroCambiar' is never used
        private readonly LetraCambiarNULL letraPararametroCambiar;
#pragma warning restore CS0169 // The field 'EditarPathRein_Base.letraPararametroCambiar' is never used
        protected Document _doc;
        protected readonly UIApplication _uiapp;
        protected  SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;

        // private UIDocument _uidoc;
        protected View _view;
        protected Level _Level;
        protected DatosPathRinforment _LargoPathRinforment;

        public CoordenadaPath _coordenadaPath { get; private set; }
        protected PathReinSpanSymbol _pathReinSpanSymbo { get; set; }

        protected ManejadorEditarREbarShapYPAthSymbol _manejadorEditarREbarShapYPAthSymbol;
        protected string _tipobarra;

        public EditarPathRein_Base(UIApplication uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
           
            this._seleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
            // this._uidoc = commandData.Application.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._Level = _doc.ActiveView.GenLevel;
            this._pathReinforcement = _seleccionarPathReinfomentConPto.PathReinforcement;
            this._pathReinSpanSymbo = _seleccionarPathReinfomentConPto.PathReinforcementSymbol;
            this._manejadorEditarREbarShapYPAthSymbol = _seleccionarPathReinfomentConPto._AyudaObtenerLArgoPata;
            this._tipobarra = _seleccionarPathReinfomentConPto._tipobarra;
        }

        protected virtual void M1_3_MOdificarPArametrPrimary_Bar_Length(double _desplazamientoFoot)
        {
            _LargoPathRinforment = new DatosPathRinforment(_pathReinforcement);
            _LargoPathRinforment.M1_ObtenerDatosGenerales();
            double LargoPrimaria_Foot = _LargoPathRinforment.LargoPrimaria_Foot + _desplazamientoFoot;// RedonderLargoBarras.RedondearFoot1_mascercano();

            if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && RedonderLargoBarras.RedondearFoot5_AltMascercano(_LargoPathRinforment.LargoPrimaria_Foot + _desplazamientoFoot))
                LargoPrimaria_Foot = RedonderLargoBarras.NuevoLargobarraFoot;
            else if (RedonderLargoBarras.RedondearFoot1_masbajo(_LargoPathRinforment.LargoPrimaria_Foot + _desplazamientoFoot))
                LargoPrimaria_Foot = RedonderLargoBarras.NuevoLargobarraFoot;

            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, LargoPrimaria_Foot);

            if (_LargoPathRinforment.IsAlternative)
            {
                double LargoAlternative_foot = 0;

                if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && RedonderLargoBarras.RedondearFoot5_AltMascercano(_LargoPathRinforment.LargoAlternative_foot + _desplazamientoFoot))
                    LargoAlternative_foot = RedonderLargoBarras.NuevoLargobarraFoot;
                else if (RedonderLargoBarras.RedondearFoot1_masbajo(_LargoPathRinforment.LargoAlternative_foot + _desplazamientoFoot))
                    LargoAlternative_foot = RedonderLargoBarras.NuevoLargobarraFoot;

                ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, LargoAlternative_foot);
            }
        }

       
        protected void ObtenerDatosNUevoPath()
        {
            CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_pathReinforcement, _doc);
            pathReinformeCalculos.Calcular4PtosPathReinf();
            _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();
        }
        public bool moverPathsimbol()
        {
            try
            {
                return _pathReinSpanSymbo.Location.Move(_seleccionarPathReinfomentConPto.UbicacionPathReinforcementSymbol - _pathReinSpanSymbo.TagHeadPosition);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
