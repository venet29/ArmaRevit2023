using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Tag;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Dibujar2D
{
    internal class Dibujar2D_Barra_Corte_TAg_V
    {
        private UIApplication _uiapp;
        private Document _doc;
        private List<RebarDesglose_Barras_V> listaBArrasEnElev;
        private List<Curve> ListaCurvasZcero;
        private readonly PlanarFace caraInferior;
        private View _view;
        private readonly Config_EspecialCorte Config_EspecialCorte;

        public Dibujar2D_Barra_Corte_TAg_V(UIApplication uiapp, List<RebarDesglose_Barras_V> listaBArrasEnElev, PlanarFace caraInferior, Config_EspecialCorte _Config_EspecialCorte )
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
            this.listaBArrasEnElev = listaBArrasEnElev;
            this.caraInferior = caraInferior;
            this.Config_EspecialCorte = _Config_EspecialCorte;
        }

        internal void CrearTAg(ViewSection section)
        {
            try
            {

                //1)conf
                double valorZ = section.Origin.Z;

                ConfiguracionTAgBarraDTo confBarraTag = new ConfiguracionTAgBarraDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    tagOrientation = TagOrientation.Horizontal

                };

                ObtenerPerimetoHostConCero();

                using (Transaction t = new Transaction(_uiapp.ActiveUIDocument.Document))
                {
                    t.Start("CrearTAgBarrasElev");
                    //2)
                    foreach (RebarDesglose_Barras_V item in listaBArrasEnElev)
                    {                        
                        XYZ Direccion = AyudaObtenerDireccionTAgCorte.Obtener(ListaCurvasZcero, item.ptoMedio.AsignarZ(0));

                        XYZ LeaderEnd = item.ptoMedio;
                        XYZ LeaderElbow = LeaderEnd + Direccion * Util.CmToFoot(10); ;
                        XYZ ptoTag = LeaderEnd + Direccion * Util.CmToFoot(15);

                        Rebar _rebar = item._rebarDesglose._rebar;

                        Config_EspecialElev _auxConfig_EspecialElev = new Config_EspecialElev()
                        { direccionMuevenBarrasFAlsa = _view.RightDirection };

                        GeomeTagBarrarElev _GeomeTagBarrarElev = new GeomeTagBarrarElev(_uiapp, item.ObtenerRebarElevDTO(LeaderEnd, _uiapp, false, _auxConfig_EspecialElev));
                        _GeomeTagBarrarElev.M3_DefinirRebarShape();

                        TagBarra _TagBarra = _GeomeTagBarrarElev.TagP0_Tipo;

                        _TagBarra.posicion = ptoTag.AsignarZ(valorZ);
                        _TagBarra.PtoCodo_LeaderElbow = LeaderElbow.AsignarZ(valorZ);
                        _TagBarra.Ptocodo_LeaderEnd = LeaderEnd.AsignarZ(valorZ);
                        _TagBarra.IsDIrectriz = true;
                        _TagBarra.IsLibre = true;

                        _TagBarra.DibujarTagRebar_ConLibre(_rebar, _uiapp, section, confBarraTag);

                    }
                    t.Commit();
                }



            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al generar tag de barras verticales ex:{ex.Message}");
                throw;
            }        
        }

        private void ObtenerPerimetoHostConCero()
        {
            var perimetro = caraInferior.GetEdgesAsCurveLoops();
            ListaCurvasZcero = new List<Curve>();
            foreach (CurveLoop item in perimetro)
            {
                foreach (Curve _curve in item)
                {
                    ListaCurvasZcero.Add(Line.CreateBound(_curve.GetEndPoint(0).AsignarZ(0), _curve.GetEndPoint(1).AsignarZ(0)));
                }
            }
        }
    }
}