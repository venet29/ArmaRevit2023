using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra
{
    public partial class DatosNuevaBarraDTO
    {
#pragma warning disable CS0414 // The field 'DatosNuevaBarraDTO.message' is assigned but its value is never used
        private string message;
#pragma warning restore CS0414 // The field 'DatosNuevaBarraDTO.message' is assigned but its value is never used

        //DimensionesBarras
        public DimensionesBarras dimBarras { get; set; }
        public DimensionesBarras dimBarras_parameterSharedLetras { get; set; }
        public DimensionesBarras dimBarrasAlternativa { get; internal set; }

        public string LargoTotal { get; set; }
        public string LargoParciales { get; set; }
        public ElementId tipodeHookStartPrincipal { get; set; }
        public ElementId tipodeHookEndPrincipal { get; set; }

        public RebarShape tipoRebarShapePrincipal { get; set; }
        public RebarShape tipoRebarShapeAlternativa { get; set; }
        public IList<Curve> CurvesPathreiforment { get; set; }

        // nombre de familias

        public string Prefijo_cuantia { get; set; } = "";

        public string nombreFamiliaRebarShape { get; set; }
        public string nombreFamiliaRebarShapeAlternativo { get; set; }
        public string nombreSimboloPathReinforcement { get; set; }



        //datos 
        public double LargoPathreiforment { get; set; }
        public double DiametroMM { get; set; }

        public double EspaciamientoFoot { get; set; }
        private double _LargoMininoLosa;
        public double LargoMininoLosa {
            get
            {
                return this._LargoMininoLosa;
            }
            set
            {
                if (Util.IsSimilarValor(value, 0, Util.CmToFoot(1)) && VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM)
                {
                    Util.ErrorMsg("Largo minimo de losa igual a cero.Revisar.");
                }
                this._LargoMininoLosa = value;
            }
        }

        //
        public XYZ DesplazamientoPathReinSpanSymbol { get; set; }
        public XYZ PtoMouse { get; set; }

     
        //validadores
        public bool IsBarrAlternative { get; internal set; }
        public bool IsLuzSecuandiria { get; internal set; }
        public TipoCaraObjeto TipoCaraObjeto_ { get; set; }
        public TipoPataFund TipoPataFun { get; set; }
        public ElementId _InvalidrebarHookTypeId { get; set; }

        private int _pataBarraFUnd;

        public double LargoRecorridoFoot { get; internal set; }
        public int DiametroOrientacionPrincipal_mm { get; set; } = 0;
        public double EspesorElemento_foot { get;  set; }
        public Element ElementSimboloPathReinforcementElement { get; set; }
        public TipoRefuerzoMuroSUple tipoSuple { get;  set; }
        public bool IsRedefinirTagHeadPosition { get;  set; }

        public TipoRebar _BarraTipo { get; set; } = TipoRebar.NONE;
        public double LargoPaTa_foot { get;  set; }
        public double LargoAhorroDere { get;  set; }
        public double LargoAhorroIzq { get;  set; }
        public double LargoPataInf { get;  set; }
        public XYZ CentroPAth { get;  set; }
        public bool Iscaso_Intervalo { get;  set; }
        public string Tiposeleccion { get;  set; }
        public string TiposElemento { get;  set; }
        
        public bool IsAcortarCUrva { get; set; } = true;
        public RebarHookType rebarBarTypePrincipal_star { get;  set; }
        public RebarHookType rebarBarTypePrincipal_end { get;  set; }
        public TipoOrientacionBarraSupleMuro orientacionBarraSupleMuro { get;  set; }
        public SentidoSupleMuro tipo_sentido { get; internal set; }
        public string nombreSimboloPathReinforcement_escala { get;  set; }
        public PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FXXDTO_ { get;  set; }
        public double EspesorLosaCm_1 { get;  set; }
        public XYZ PtoTag { get; internal set; }
        public string TipoDeDIreccionBarra { get; internal set; }

        public DatosNuevaBarraDTO()
        {
            DesplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0);
            tipodeHookEndPrincipal = ElementId.InvalidElementId;
            tipodeHookStartPrincipal = ElementId.InvalidElementId;
            TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
            _InvalidrebarHookTypeId = ElementId.InvalidElementId;
            _pataBarraFUnd = ConstNH.LARGO_PATA_FUNDACIONES_CM;

      

            PathSymbol_REbarshape_FXXDTO_ = new PathSymbol_REbarshape_FxxDTO() {  IsOK=false};
        }


        public Result ObtenerDiametroEstaciamiento(string CuantiaB)
        {

            string[] auxcuantia = CuantiaB.Split('a');

            int _diametro = 0;
            bool resulDiametro = int.TryParse(auxcuantia[0], out _diametro);
            if (!resulDiametro) { message = "Losa sin Diametro o mal definido"; return Result.Failed; }

            DiametroMM = _diametro;
   
            double _espaciamiento = 0;
            bool resulEspaciemientoo = double.TryParse(auxcuantia[1], out _espaciamiento);
            if (!resulEspaciemientoo) { message = "Losa sin Espaciamiento o mal definido"; return Result.Failed; }
            EspaciamientoFoot = Util.CmToFoot(_espaciamiento);

            return Result.Succeeded;
        }

        public string LargoTotal_(double largoBarra)
        {
            _pataBarraFUnd = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm((int)DiametroMM);
            largoBarra = Math.Round(Util.FootToCm(largoBarra), 0);
            switch (TipoPataFun)
            {
                case TipoPataFund.Sin:
                    return (largoBarra).ToString();
                case TipoPataFund.DereSup:
                case TipoPataFund.IzqInf:
                    return (_pataBarraFUnd + largoBarra).ToString();
                case TipoPataFund.Ambos:
                    return (_pataBarraFUnd * 2 + largoBarra).ToString();
            }
            return "";
        }

        public string LargoParciales_(double largoBarra_foot)
        {
            _pataBarraFUnd = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm((int)DiametroMM);
            largoBarra_foot = Math.Round(Util.FootToCm(largoBarra_foot), 0);
            switch (TipoPataFun)
            {
                case TipoPataFund.DereSup:
                    return $"({_pataBarraFUnd}+{largoBarra_foot})";
                case TipoPataFund.IzqInf:
                    return $"({largoBarra_foot}+{_pataBarraFUnd})";
                case TipoPataFund.Ambos:
                    return $"({_pataBarraFUnd}+{largoBarra_foot}+{_pataBarraFUnd})";
            }
            return "";
        }

        public string ObtenerNombrePAthSymbol(string escala)
        {
            string _NombrePAthSymbol = "";
            try
            {
                if (nombreFamiliaRebarShape == "NH_F1B" || nombreFamiliaRebarShape == "NH_F1A" || nombreFamiliaRebarShape == "NH4_bajo")
                    _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala + "_P" + LargoPataCm().ToString();
                else
                    _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala + "_D" + LargoPataCm().ToString(); ;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala;
            }

            return _NombrePAthSymbol;
        }


        public string ObtenerNombrePAthSymbol_P(string escala)
        {
            string _NombrePAthSymbol = "";
            try
            {
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala + "_P" + LargoPataCm().ToString();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala;
            }

            return _NombrePAthSymbol;
        }


        public string ObtenerNombrePAthSymbol_D(string escala)
        {
            string _NombrePAthSymbol = "";
            try
            {
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala + "_D" + LargoPataCm().ToString();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala;
            }

            return _NombrePAthSymbol;
        }
        public string ObtenerNombrePAthSymbol_SoloEscala(string escala)
        {
            string _NombrePAthSymbol = "";
            try
            {
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _NombrePAthSymbol = nombreSimboloPathReinforcement + "_" + escala;
            }

            return _NombrePAthSymbol;
        }
        public double LargoPataCm()
        {
            double largoCm = Util.FootToCm(LargoPaTa_foot);

            if (largoCm <= 32)
                return 30;
            else if (largoCm <= 37)
                return 35;
            else if (largoCm <= 42)
                return 40;
            else if (largoCm <= 47)
                return 45;
            else if (largoCm <= 52)
                return 50;
            else if (largoCm <= 57)
                return 55;
            else if (largoCm <= 62)
                return 60;
            else if (largoCm <= 67)
                return 65;
            else if (largoCm <= 72)
                return 70;
            else if (largoCm <= 77)
                return 75;
            else if (largoCm <= 82)
                return 80;
            else if (largoCm <= 87)
                return 85;
            else if (largoCm <= 92)
                return 90;
            else
                return 90;

         
        }
        internal double LargoPataFoot() => Util.CmToFoot(LargoPataCm());
   
    }
}
