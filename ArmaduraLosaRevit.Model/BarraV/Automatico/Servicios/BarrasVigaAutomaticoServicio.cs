using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Servicios
{
    public class BarrasVigaAutomaticoServicio
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public BarrasVigaAutomaticoServicio(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }

        public bool Ejecutar()
        {
            try
            {
                var nombrearchivo1 = @"J:\_revit\PROYECTO\2022\2022-022-talca\reportevigas\_ARM_Vigas.TXt";
                var _RepositoryAceroElecacion = new RepositoryAceroElecacionVigas(nombrearchivo1);

                if (!_RepositoryAceroElecacion.ObtenerBArras_Info_Barras_Flexion_json()) return false;
                var listaVigas = _RepositoryAceroElecacion.ListaVigaAutomaticoConBArras;
                //
                double deltaElevacionBasePoint = AyudaObtenerDeltaElevacion.ObtenerDeltaElevation(_uiapp);
                ArmaduraTrasformada _armaduraTrasformada = new ArmaduraTrasformada(_view, deltaElevacionBasePoint);

                for (int i = 0; i < listaVigas.Count; i++)
                {
                    var viga = listaVigas[i];
                    for (int j = 0; j < viga.ListaBArras.Count; j++)
                    {
                        var barra = viga.ListaBArras[j];
                        barra.BarraFlexionTramosDTO_.P1_Revit_foot = _armaduraTrasformada.Ejecutar(barra.BarraFlexionTramosDTO_.p1_mm.GetXYZ_mmTofoot());
                        barra.BarraFlexionTramosDTO_.P2_Revit_foot = _armaduraTrasformada.Ejecutar(barra.BarraFlexionTramosDTO_.p2_mm.GetXYZ_mmTofoot());
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en  ejecutar 'BarrasVigaAutomaticoServicio'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
