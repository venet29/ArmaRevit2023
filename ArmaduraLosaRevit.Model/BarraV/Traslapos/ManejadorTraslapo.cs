using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos
{
    public class ManejadorTraslapo
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        private View _view;

        public TraslapoDTO _traslapoDTO { get; set; }
        public List<TraslapoDTO> ListaTraslapo { get; set; }
        private DimensionType _dimensionType;

        public ManejadorTraslapo(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
            ListaTraslapo = new List<TraslapoDTO>();
        }

        public void AgregarbarrasInicialTraslpoa()
        {
            if (_traslapoDTO == null)
            { _traslapoDTO = new TraslapoDTO(); }
            else if (_traslapoDTO.RebarFinal != null)
            {
                var barraFinalAnterior = _traslapoDTO.RebarFinal;
                _traslapoDTO = new TraslapoDTO();
                _traslapoDTO.RebarInicial = barraFinalAnterior;
            }
        }
        public void AgregarbarrasFinalTraslpoa(int i, Rebar _rebar)
        {

            if (Util.IsPar(i))
            {
                if (_traslapoDTO.RebarInicial == null)
                {
                    _traslapoDTO.RebarInicial = _rebar;
                    AyudaManejadorTraslapo.UltimaBarra = _rebar;
                    AyudaManejadorTraslapo.IsInicialIntervalo = false;
                }
                else
                {
                    _traslapoDTO.RebarFinal = _rebar;
                    _traslapoDTO.IsOK = true;
                    ListaTraslapo.Add(_traslapoDTO);
                    AyudaManejadorTraslapo.UltimaBarra = _rebar;
                    AyudaManejadorTraslapo.IsInicialIntervalo = true;
                }
            }
            else
            {
                if (_traslapoDTO == null) return;
                if (_traslapoDTO.RebarInicial == null) return;

                if (!_traslapoDTO.RebarInicial.IsBarrarIntersectadaParalella(_rebar)) return;

                _traslapoDTO.RebarFinal = _rebar;
                _traslapoDTO.IsOK = true;


                ListaTraslapo.Add(_traslapoDTO);

                AyudaManejadorTraslapo.UltimaBarra = _rebar;
                AyudaManejadorTraslapo.IsInicialIntervalo = true;
            }


        }
        internal void AsignarBarrasIntervaloAnterior()
        {
            _traslapoDTO.RebarInicial = AyudaManejadorTraslapo.UltimaBarra;
            AyudaManejadorTraslapo.IsInicialIntervalo = false;
        }

        public void M4_DibujarTraslapoConTrans()
        {
            if (!Validar()) return;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CreadoDImension");
                    EjecutarFor();
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
        }


        public void M4_DibujarTraslapoSinTrans()
        {
            if (!Validar()) return;

            try
            {
                EjecutarFor();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
        }
        private bool Validar()
        {
            try
            {
                if (!VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras) return false;

                _dimensionType = SeleccionarDimensiones.ObtenerDimensionTYpo_Traslapo(_doc);
                if (_dimensionType == null) return false;

                if (ListaTraslapo.Count == 0) return false;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool EjecutarFor()
        {
            try
            {
                foreach (TraslapoDTO tr in ListaTraslapo)
                {
                    if (!tr.IsOK) continue;
                    //1
                    if (!tr.M1_ObtenerREfIncial(_view)) continue;
                    //2
                    if (!tr.M2_ObtenerREfFinal(_view)) continue;

                    if (!tr.M3_ObtenerPtos(_view)) continue;

                    tr.M4_CrearDimension_SinTrans(_doc, _dimensionType);
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }



        internal void Reset() => _traslapoDTO = null;
    }
}
