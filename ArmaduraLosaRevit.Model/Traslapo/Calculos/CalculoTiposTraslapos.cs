using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public interface ICalculoTiposTraslapos
    {
        TipoPathReinfDTO TipoPathReinf_IzqBajo { get; set; }
        TipoPathReinfDTO TipoPathReinf_DerArriba { get; set; }
        bool IsOk { get; set; }
        TipoBarra ActualTipobarra { get; set; }
    }
    public class CalculoTiposTraslaposNUll : ICalculoTiposTraslapos
    {
        public bool IsOk { get; set; } = false;
        public TipoPathReinfDTO TipoPathReinf_DerArriba { get; set; }
        public TipoPathReinfDTO TipoPathReinf_IzqBajo { get; set; }
        public TipoBarra ActualTipobarra { get; set; }
    }
    public class CalculoTiposTraslapos : ICalculoTiposTraslapos
    {
        private PathReinforcement _pathReinforcement;
        private CasoBarra _tipoBarraActual_Principal;
        private CasoBarra _tipoBarraActual_Secudarias;
        private TipoBarra _tipoBarra;
        private string _tipoDireccionBarraVocal;
        private Document _doc;
        private UbicacionLosa _tipoBarraDireccion;
        private ControladorContenedorDatosTraslapoV2 _ContenedorDatosTraslapoManejadorV2;

        public TipoPathReinfDTO TipoPathReinf_IzqBajo { get; set; }
        public TipoPathReinfDTO TipoPathReinf_DerArriba { get; set; }
        public TipoBarra ActualTipobarra { get; set; }


        public bool IsOk { get; set; }
        public static ICalculoTiposTraslapos CreatorCalculoTiposTraslapos(PathReinforcement pathReinforcement, Document doc)
        {
            CalculoTiposTraslapos calculoTiposTraslapos = null;
            try
            {
                if (pathReinforcement == null) return new CalculoTiposTraslaposNUll();

                calculoTiposTraslapos = new CalculoTiposTraslapos(pathReinforcement, doc);
                if (!calculoTiposTraslapos.M1_ObtenerBarShape()) return new CalculoTiposTraslaposNUll();
                //calculoTiposTraslapos.ObtenerDatosDelPathReinf();
                if(!calculoTiposTraslapos.M2_ObtenerDatosDelPathReinfConIDtipo()) return new CalculoTiposTraslaposNUll();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error  en 'CreatorCalculoTiposTraslapos' ex:{ex.Message}");
                return new CalculoTiposTraslaposNUll();
            }
            return calculoTiposTraslapos;
        }
        public CalculoTiposTraslapos(PathReinforcement pathReinforcement, Document doc)
        {
            this._pathReinforcement = pathReinforcement;
            this._doc = doc;
            //this._ContenedorDatosTraslapoManejador = new ControladorContenedorDatosTraslapo();
            this._ContenedorDatosTraslapoManejadorV2 = new ControladorContenedorDatosTraslapoV2();
            this.IsOk = true;
        }

        #region 1) metodos 'ObtenerBarShape'


        public bool M1_ObtenerBarShape()
        {
            try
            {


                _tipoBarraActual_Principal = OBtenerTipoActualUtilizandoShape(TipoBarraConfig.primaria);
                _tipoBarraActual_Secudarias = OBtenerTipoActualUtilizandoShape(TipoBarraConfig.alternativa);
                _tipoBarra = OBtenerTipoBarra();
                ActualTipobarra = _tipoBarra;
                _tipoBarraDireccion = OBtenerTipoBarraDireccion();
                _tipoDireccionBarraVocal = OBtenerTipoBarraDireccionVocal(); //p o s
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error    ex:{ex.Message}");
                return false;
            }
            return true;
        }

        #region 1.1) MEtodos 'OBtenerTipoactualUtilizandoShape'
        private CasoBarra OBtenerTipoActualUtilizandoShape(TipoBarraConfig tipobarra)
        {


            string Datobuscar = (tipobarra == TipoBarraConfig.primaria ? "Primary Bar - Shape" : "Alternating Bar - Shape");
            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, Datobuscar, _doc);
            return (result == "" ? CasoBarra.NONE : ObtenerTipoCasoBarra(result));
        }
        private CasoBarra ObtenerTipoCasoBarra(string tipo)
        {
            if (tipo.Contains("NH_F1A")) return CasoBarra.NH_F1A;
            if (tipo.Contains("NH_F1B")) return CasoBarra.NH_F1B;
            if (tipo.Contains("M_00")) return CasoBarra.M_00;
            if (tipo.Contains("NH4_bajo")) return CasoBarra.NH4_bajo;
            if (tipo.Contains("NH_F7A")) return CasoBarra.NH_F7A;
            if (tipo.Contains("NH_F11_v2")) return CasoBarra.NH_F11_v2;
            if (tipo.Contains("NH_F11")) return CasoBarra.NH_F11;
            if (tipo.Contains("NH_F7B")) return CasoBarra.NH_F7B;

            return CasoBarra.NONE;
        }
        #endregion

        private TipoBarra OBtenerTipoBarra()
        {
            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "IDTipo", _doc);
            return (result == "" ? TipoBarra.NONE : EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, ObtenerCasoAoBDeAhorro.ConversortoS4(result)));
        }

        private UbicacionLosa OBtenerTipoBarraDireccion()
        {


            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "IDTipoDireccion", _doc);
            return (result == "" ? UbicacionLosa.NONE : EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, result));
        }
        private string OBtenerTipoBarraDireccionVocal()
        {
            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "TipoDireccionBarra", _doc);
            if (result == null) result = "";
            if ((_tipoBarra.ToString() == "s1" || _tipoBarra.ToString() == "s2" || _tipoBarra.ToString() == "s3" || _tipoBarra.ToString() == "s4" || _tipoBarra.ToString() == "f12"))
                result = "i";
            return result;
        }

        #endregion

        #region 2) metodos  'ObtenerDatosDelPathReinfConIDtipo'
        private bool M2_ObtenerDatosDelPathReinfConIDtipo()
        {

            try
            {
                ContenedorDatosTraslapoV2 contV2 = _ContenedorDatosTraslapoManejadorV2.ObtenerBarraIzqBajoTraslapo(_tipoBarra, _tipoBarraDireccion);

                if (contV2 == null)
                {
                    Util.ErrorMsg("No se encontro Lista de opciones en traslapo");
                    return IsOk = false;
                }

                TipoPathReinf_IzqBajo = contV2._tipoBarraTraslapoIzqBajoResult;
                TipoPathReinf_IzqBajo.TipoDireccionBarraVocal = _tipoDireccionBarraVocal;
                TipoPathReinf_DerArriba = contV2._tipoBarraTraslapoDereArribaResult;
                TipoPathReinf_DerArriba.TipoDireccionBarraVocal = _tipoDireccionBarraVocal;

                IsOk = true;
            }
            catch (Exception ex)
            {
                IsOk = false;
                Util.ErrorMsg($" Error    ex:{ex.Message}");
                return false;
            }
            return IsOk;
        }
        #endregion

    }
}
