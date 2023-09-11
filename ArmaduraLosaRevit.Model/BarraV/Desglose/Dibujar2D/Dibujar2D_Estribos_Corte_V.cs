using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Tag;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public class Dibujar2D_Estribos_Corte_V
    {
        private UIApplication _uiapp;
        private  Config_EspecialCorte _config_EspecialCorte;
        private Document _doc;
        private View _view;
#pragma warning disable CS0414 // The field 'Dibujar2D_Estribos_Corte_V._GruposListasTraslapoIguales' is assigned but its value is never used
        private GruposListasTraslapoIguales_V _GruposListasTraslapoIguales;
#pragma warning restore CS0414 // The field 'Dibujar2D_Estribos_Corte_V._GruposListasTraslapoIguales' is assigned but its value is never used
#pragma warning disable CS0169 // The field 'Dibujar2D_Estribos_Corte_V._GruposListasEstribo' is never used
        private GruposListasEstribo_V _GruposListasEstribo;
#pragma warning restore CS0169 // The field 'Dibujar2D_Estribos_Corte_V._GruposListasEstribo' is never used
        private List<IRebarLosa_Desglose> _ListIRebarLosa;
        private RebarDesglose_GrupoBarras_V _rebarDesglose_GrupoBarras;
        private XYZ _puntoCentrealHost;
        double mayorDistancia;

        public XYZ ptoCentroPilarAlturaCOrte { get; private set; }

        public Dibujar2D_Estribos_Corte_V(UIApplication uiapp, GruposListasEstribo_V rebarDesglose_GrupoBarras, Config_EspecialCorte _Config_EspecialCorte)
        {
            _uiapp = uiapp;
            this._config_EspecialCorte = _Config_EspecialCorte;
            this._rebarDesglose_GrupoBarras = rebarDesglose_GrupoBarras.GruposRebarMismaLinea[0];
            this._puntoCentrealHost = rebarDesglose_GrupoBarras._DatosHost.CentroHost;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            _GruposListasTraslapoIguales = null;

            _ListIRebarLosa = new List<IRebarLosa_Desglose>();
        }


        public bool PreDibujar(View _view, View _viewOriginal)
        {

            try
            {
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                if (lista.Count == 0) return false;

                XYZ posicionInicial = lista[0];
                XYZ posicionAUX = XYZ.Zero;

                var listaEstribo= _rebarDesglose_GrupoBarras._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES).ToList();
                var listaTrabas = _rebarDesglose_GrupoBarras._GrupoRebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_T).ToList();


                double ZSleccion = posicionInicial.Z;

                ptoCentroPilarAlturaCOrte= _puntoCentrealHost.ProjectExtendidaXY0(_view.RightDirection, posicionInicial).AsignarZ(ZSleccion);
                posicionInicial =_puntoCentrealHost.ProjectExtendidaXY0(_view.RightDirection, posicionInicial).AsignarZ(ZSleccion);

                mayorDistancia = 0;
                //estribos
                for (int i = 0; i < listaEstribo.Count; i++)
                {
                    posicionAUX = posicionInicial;
                    RebarDesglose_Barras_V item1 = listaEstribo[i];

                    RebarElevDTO _RebarElevDTO = item1.ObtenerRebarCorteDTO(posicionAUX, _puntoCentrealHost, _uiapp, _view, _viewOriginal, _config_EspecialCorte);
                    GenerarBarra_2D(_RebarElevDTO);
                    posicionInicial = posicionInicial + _view.RightDirection *(mayorDistancia+ Util.CmToFoot(30));
                }
                //trabas
          
                for (int i = 0; i < listaTrabas.Count; i++)
                {
                    posicionAUX = posicionInicial;
                    RebarDesglose_Barras_V item1 = listaTrabas[i];

                    RebarElevDTO _RebarElevDTO = item1.ObtenerRebarCorteDTO(posicionAUX, _puntoCentrealHost, _uiapp, _view, _viewOriginal, _config_EspecialCorte);
                    GenerarBarra_2D(_RebarElevDTO);

                    posicionInicial = posicionInicial + _view.RightDirection * (mayorDistancia + Util.CmToFoot(0));
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

        private bool GenerarBarra_2D(RebarElevDTO _RebarElevDTO)
        {
            try
            {
                //3)tag
                IGeometriaTag _newIGeometriaTag = FactoryGeomTagRebarDesglose.CrearIGeomTagRebarDesaglose(_uiapp, _RebarElevDTO);

                //4)barra
                IRebarLosa_Desglose rebarLosa = FactoryIRebarDesglose.CrearIRebarLosa(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                mayorDistancia = rebarLosa.mayorDistancia;
                _ListIRebarLosa.Add(rebarLosa);
                
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool Dibujar()
        {

            try
            {
                if (_ListIRebarLosa.Count == 0) return false;
                using (TransactionGroup t = new TransactionGroup(_uiapp.ActiveUIDocument.Document))
                {
                    t.Start("CrearBarraInclinada-NH");
                    foreach (var rebarLosa in _ListIRebarLosa)
                    {
                        rebarLosa.M2A_GenerarBarra();

                    }
                    t.Assimilate();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
