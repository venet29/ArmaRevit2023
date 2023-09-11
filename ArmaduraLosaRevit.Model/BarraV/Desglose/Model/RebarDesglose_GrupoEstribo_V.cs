using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Model
{
    public class RebarDesglose_GrupoEstribo_V
    {
        static int nextId = 0;
        public int contGrupo { get; private set; }
        public List<RebarDesglose_Barras_V> _GrupoRebarDesglose { get; set; }

        public XYZ _ptoInicial { get; set; }
        public XYZ _ptoFinal { get; set; }
        public bool Isok { get; set; }
        public Line _curvePrincipal { get; set; }
        public casoAgrupar _casoAgrupar { get; set; }
        public List<RebarDesglose_GrupoEstribo_V> _ListaRebarDesglose_GrupoBarrasRepetidas { get; set; }

        public static RebarDesglose_GrupoEstribo_V Creador_RebarDesglose_GrupoBarras(List<RebarDesglose_Barras_V> _grupoRebarDesglose)
        {
         
            if (_grupoRebarDesglose == null) return new RebarDesglose_GrupoEstribo_V();
            if (_grupoRebarDesglose.Count == 0) return new RebarDesglose_GrupoEstribo_V();

            XYZ ptoInicial = _grupoRebarDesglose[0].ptoInicial;
            XYZ ptoFinal = _grupoRebarDesglose.Last().ptoFinal;
            Line _NewcurvePrincipal = Line.CreateBound(ptoInicial, ptoFinal);

            return new RebarDesglose_GrupoEstribo_V(_grupoRebarDesglose, _NewcurvePrincipal);
        }

        private RebarDesglose_GrupoEstribo_V(List<RebarDesglose_Barras_V> _grupoRebarDesglose, Line _curvePrincipal)
        {
            nextId += 1;
            this.contGrupo = nextId;
            this._GrupoRebarDesglose = _grupoRebarDesglose;
            this._curvePrincipal = _curvePrincipal;
            this._ptoInicial = _curvePrincipal.GetEndPoint(0);
            this._ptoFinal = _curvePrincipal.GetEndPoint(1);
            this._ListaRebarDesglose_GrupoBarrasRepetidas = new List<RebarDesglose_GrupoEstribo_V>();
            this.Isok = true;
            this._casoAgrupar = casoAgrupar.NoAnalizada;

        }

        public RebarDesglose_GrupoEstribo_V()
        {
            this.Isok = false;
        }
    }
}
