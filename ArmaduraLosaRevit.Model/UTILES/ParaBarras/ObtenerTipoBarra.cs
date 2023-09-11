using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class ObtenerTipoBarra
    {

        private readonly Element _rebar;
        public TipoRebar TipoBarra_ { get; set; }
        public TipoBarraGeneral TipoBarraGeneral { get; set; }
        public static bool IsMjs { get; set; } = true;

        public ObtenerTipoBarra(Element rebar)
        {
            this._rebar = rebar;
        }
        public bool Ejecutar()
        {
            string _TipoBarra = "";
            if (!(_rebar is Rebar || _rebar is PathReinforcement || _rebar is RebarInSystem)) return false;

            try
            {
                _TipoBarra = ParameterUtil.FindParaByName(_rebar, "BarraTipo")?.AsString();

                if (_TipoBarra == "" || _TipoBarra == null)
                {
                    //Util.ErrorMsg($"Error  -> No se encontro tipo de barra  id:{_rebar.Id.IntegerValue} ");
                    Debug.WriteLine($" Null en 'ObtenerTipoBarra' -> No se encontro tipo de barra  id:{_rebar.Id.IntegerValue} ");
                    TipoBarraGeneral = TipoBarraGeneral.NONE;
                    TipoBarra_ = TipoRebar.NONE;
                    return true;
                }

                 TipoBarraGeneral = Tipos_Barras.M2_Buscar_TipoGrupoBarras_pornombre(_TipoBarra);

                if (Enum.IsDefined(typeof(TipoRebar), _TipoBarra))
                {
                    TipoBarra_ = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, _TipoBarra, IsMjs);
                }
                else
                {
                    Util.ErrorMsg($"No se encontro tipo de barra:{_TipoBarra} dentro de lista de tipo de barra. Favor corregir o asignar nombre correcto.");
                    TipoBarra_ = TipoRebar.NONE;
                    //TipoRebar pingPong = (TipoRebar)4;
                }



                if (TipoBarra_ == TipoRebar.NONE && IsMjs)
                {

                    var result = Util.ErrorMsgConDesctivar($"NONE en 'ObtenerTipoBarra' -> No se encontro tipo de barra. Barra Id:{_rebar.Id.IntegerValue}\n\n NOTA:para cancelar mensaje de advenrtencia oprimir cancelar");
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        IsMjs = false;
                    }
                }
            }
            catch (Exception ex)
            {
                TipoBarraGeneral = TipoBarraGeneral.NONE;
                TipoBarra_ = TipoRebar.NONE;
                if (IsMjs)
                {
                    var result = Util.ErrorMsgConDesctivar($"Error 'ObtenerTipoBarra' -> Error al encontro tipo de barra '{_TipoBarra}' \n Barra id:{_rebar.Id.IntegerValue}\n\n NOTA:para cancelar mensaje de advenrtencia oprimir cancelar \nex:{ex.Message}");
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        IsMjs = false;
                    }
                }
            }

            return true;
        }


    }
}
