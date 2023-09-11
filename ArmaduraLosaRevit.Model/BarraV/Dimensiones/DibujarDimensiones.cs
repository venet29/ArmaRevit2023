using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Dimensiones
{
    public class DibujarDimensiones
    {
        private  UIApplication _uiapp;
        private View _view;
        private Document _doc;
        List<PtoDimesionesTraslapoDTO> _listaDimensiones;
        private  XYZ _direccionEnFierrado;

        public DibujarDimensiones(UIApplication uiapp, List<PtoDimesionesTraslapoDTO> _listaDimensiones, XYZ DireccionBarra)
        {
            this._uiapp = uiapp;

            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._listaDimensiones = _listaDimensiones;
            _direccionEnFierrado = DireccionBarra;
        }
        public void Ejecutar()
        {
            try
            {
                if (_listaDimensiones.Count < 2) return;
                for (int i = 1; i < _listaDimensiones.Count; i++)
                {
                    double DireccionBarra = _direccionEnFierrado.Z;
                    double MAxZ = Math.Max(_listaDimensiones[i - 1].P2.Z, _listaDimensiones[i].P1.Z);
                    double desfase = 20;

                    if (DireccionBarra > 0) desfase = 35;

                    XYZ aux_p1 = _listaDimensiones[i - 1].P2.AsignarZ(MAxZ) + -_direccionEnFierrado * Util.CmToFoot(desfase);
                    XYZ aux_p2 = ObtenerP2(i, MAxZ, desfase, aux_p1);

                    CreadorDimensiones _creadorDimensiones = new CreadorDimensiones(_doc, aux_p1, aux_p2, "COTA 50 (J.D.)");
                    _creadorDimensiones.Crear_sintrans("LineaDimension");
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);

            }
        }

        private XYZ ObtenerP2(int i, double MAxZ, double desfase, XYZ aux_p1)
        {
            XYZ aux_p2 = _listaDimensiones[i].P1.AsignarZ(MAxZ) + -_direccionEnFierrado * Util.CmToFoot(desfase);

            double largoDIme = aux_p1.DistanceTo(aux_p2);
            XYZ direccionDIm = (aux_p2 - aux_p1).Normalize();
            aux_p2 = aux_p1 + (Util.IsIgualSentido(direccionDIm, _view.RightDirection) ? 1 : -1) * _view.RightDirection * largoDIme;
            return aux_p2;
        }

    }


}
