using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class SelecionarPtoHorizontal
    {
        public UIApplication _uiapp { get; set; }
        private UIDocument _uidoc;
        private View _seccitionView;
        private ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;

        public XYZ _PtoInicioIntervaloBarra6 { get; set; }
        public XYZ _PtoFinalIntervaloBarra { get; set; }
        //  public List<Level> _listaLevel { get; set; }

        //lista con el intervalor de niveles que contiene level selecionado entre '_PtoInicioIntervaloBarra' y '_PtoFinalIntervaloBarra'
        public List<Level> ListaLevelIntervalo { get; set; }
        public bool IsConError { get; private set; }



        public SelecionarPtoHorizontal(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO, XYZ _PtoInicioIntervaloBarra)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._PtoInicioIntervaloBarra6 = _PtoInicioIntervaloBarra;
            this.IsConError = false;
            this._seccitionView = _uidoc.ActiveView;
        }

        public SelecionarPtoHorizontal(UIApplication _uiapp,
                                      ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO,
                                      XYZ _PtoInicioIntervaloBarra,
                                      XYZ ptoFinalIntervaloBarra) : this(_uiapp, confiEnfierradoDTO, _PtoInicioIntervaloBarra)
        {

            this._PtoFinalIntervaloBarra = ptoFinalIntervaloBarra;
        }

        public bool SeleccionarSegundoPtoHorizontal()
        {
            try
            {
                ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;

                //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                _PtoFinalIntervaloBarra = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar siguiente pto");

            }
            catch (Exception)
            {
                IsConError = true;
                return IsConError;
            }

            return IsConError;
        }

        public bool VerificarPtos()
        {
            if (_PtoInicioIntervaloBarra6.DistanceTo(_PtoFinalIntervaloBarra) < Util.CmToFoot(10)) return true;
            return false;
        }

        public void MoverPtosSobrePLanoCaraViga(SeleccionarElementosH _seleccionarElementos)
        {
            try
            {
               // Plane plano = Plane.CreateByNormalAndOrigin(_seccitionView.ViewDirection6(), _seleccionarElementos.PtoInicioBaseBordeViga_ProyectadoCaraMuroHost);
                Plane plano = Plane.CreateByNormalAndOrigin(_seleccionarElementos.DireccionNormalFace, _seleccionarElementos.PtoInicioBaseBordeViga_ProyectadoCaraMuroHost);
                _PtoInicioIntervaloBarra6 = plano.ProjectOntoXY(_PtoInicioIntervaloBarra6).Redondear8();
                _PtoFinalIntervaloBarra = plano.ProjectOntoXY(_PtoFinalIntervaloBarra).Redondear8();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ex : " + ex.Message);
            }
        }
    }
}
