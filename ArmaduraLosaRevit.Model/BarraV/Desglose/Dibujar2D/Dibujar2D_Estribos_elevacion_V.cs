using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Dibujar2D
{
    public class Dibujar2D_Estribos_elevacion_V
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
#pragma warning disable CS0414 // The field 'Dibujar2D_Estribos_elevacion_V._GruposListasTraslapoIguales' is assigned but its value is never used
        private GruposListasTraslapoIguales_V _GruposListasTraslapoIguales;
#pragma warning restore CS0414 // The field 'Dibujar2D_Estribos_elevacion_V._GruposListasTraslapoIguales' is assigned but its value is never used
        private GruposListasEstribo_V _GruposListasEstribo;
        private List<IRebarLosa> _ListIRebarLosa;

        public Dibujar2D_Estribos_elevacion_V(UIApplication _uiapp, GruposListasEstribo_V gruposListasEstribo)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales = null;
            _GruposListasEstribo = gruposListasEstribo;
            _ListIRebarLosa = new List<IRebarLosa>();
        }

        public bool Dibujar(XYZ posicionAUX)
        {

            try
            {
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();



                for (int i = 0; i < _GruposListasEstribo.GruposRebarMismaLinea.Count; i++)
                {

                    RebarDesglose_GrupoBarras_V item1 = _GruposListasEstribo.GruposRebarMismaLinea[i];
                    item1.ObtenerTextos();
                    RebarDesglose_Barras_V _primerEstrivo = item1._GrupoRebarDesglose[0];
                    //_primerEstrivo.ObtenerTextos();

                   CreadorDimensiones _CreadorDimensiones = new CreadorDimensiones(_doc, posicionAUX.AsignarZ(item1._ptoInicial.Z), posicionAUX.AsignarZ(item1._ptoFinal.Z), "SRV-Arial Narrow 2mm Flecha CM");
                    _CreadorDimensiones.CrearConref_conTrans("", item1.replaceWithText, item1.textobelow, _primerEstrivo.refenciaInicial, _primerEstrivo.refenciaFinal);

                }




            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }


    }
}
