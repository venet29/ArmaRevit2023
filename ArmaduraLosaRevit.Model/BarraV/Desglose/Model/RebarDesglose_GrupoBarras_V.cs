using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Model
{
    public class RebarDesglose_GrupoBarras_V
    {
        static int nextId = 0;
        public int contGrupo { get; private set; }
        public List<RebarDesglose_Barras_V> _GrupoRebarDesglose { get; set; }

        public XYZ _ptoInicial { get; set; }
        public XYZ _ptoFinal { get; set; }
        public bool Isok { get; set; }
        public Line _curvePrincipal { get; set; }
        public casoAgrupar _casoAgrupar { get; set; }
        public string replaceWithText { get; set; }
        public string textobelow { get; set; }
        public List<RebarDesglose_GrupoBarras_V> _ListaRebarDesglose_GrupoBarrasRepetidas { get; set; }

        public static RebarDesglose_GrupoBarras_V Creador_RebarDesglose_GrupoBarras(List<RebarDesglose_Barras_V> _grupoRebarDesglose)
        {

            if (_grupoRebarDesglose == null) return new RebarDesglose_GrupoBarras_V();
            if (_grupoRebarDesglose.Count == 0) return new RebarDesglose_GrupoBarras_V();

            XYZ ptoInicial = _grupoRebarDesglose[0].ptoInicial;
            XYZ ptoFinal = _grupoRebarDesglose.Last().ptoFinal;
            Line _NewcurvePrincipal = Line.CreateBound(ptoInicial, ptoFinal);

            return new RebarDesglose_GrupoBarras_V(_grupoRebarDesglose, _NewcurvePrincipal);
        }

        private RebarDesglose_GrupoBarras_V(List<RebarDesglose_Barras_V> _grupoRebarDesglose, Line _curvePrincipal)
        {
            nextId += 1;
            this.contGrupo = nextId;
            this._GrupoRebarDesglose = _grupoRebarDesglose;
            this._curvePrincipal = _curvePrincipal;
            this._ptoInicial = _curvePrincipal.GetEndPoint(0);
            this._ptoFinal = _curvePrincipal.GetEndPoint(1);
            this._ListaRebarDesglose_GrupoBarrasRepetidas = new List<RebarDesglose_GrupoBarras_V>();
            this.Isok = true;
            this._casoAgrupar = casoAgrupar.NoAnalizada;

        }

        public bool ObtenerTextos()
        {
            try
            {
                replaceWithText = "";
                textobelow = "";

                //estribo
                int cantidadEstribo = _GrupoRebarDesglose.Count(c => c._tipoBarraEspecifico == Enumeraciones.TipoRebar.ELEV_ES);

                if (cantidadEstribo != 0)
                {
                    var primer= _GrupoRebarDesglose.Find(c => c._tipoBarraEspecifico == Enumeraciones.TipoRebar.ELEV_ES);
                    int diametro=primer._rebarDesglose._rebar.ObtenerDiametroInt();
                    int Espacia= (int)primer._rebarDesglose._rebar.ObtenerEspaciento_cm();
                    replaceWithText = $"{cantidadEstribo}E.Ø{diametro}a{Espacia}";
                }


                //trabas
                int cantidadTrab = _GrupoRebarDesglose.Count(c => c._tipoBarraEspecifico == Enumeraciones.TipoRebar.ELEV_ES_T);

                if (cantidadTrab != 0)
                {
                    var primer = _GrupoRebarDesglose.Find(c => c._tipoBarraEspecifico == Enumeraciones.TipoRebar.ELEV_ES_T);
                    int diametro = primer._rebarDesglose._rebar.ObtenerDiametroInt();
                    int Espacia = (int)primer._rebarDesglose._rebar.ObtenerEspaciento_cm();
                    textobelow = $"+{cantidadTrab}TR.Ø{diametro}a{Espacia}";
                }

                if (replaceWithText == "" && textobelow != "")
                {
                    replaceWithText = textobelow;
                    textobelow = "";
                }

            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public RebarDesglose_GrupoBarras_V()
        {
            this.Isok = false;
        }
    }
}
