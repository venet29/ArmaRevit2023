using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion
{
    public class SeleccionMuroAreaPath : SeleccionarElementosV
    {
        public List<XYZ> _listaPtosPAthArea;
        private Plane _planoDeLAcaraDelMuro;
        private  bool _isConRecobrimiento;

        //  private readonly DatosMallasDTO _datosMallasDTO;

        public bool IsOk { get; set; }

        public SeleccionMuroAreaPath(UIApplication _uiapp,bool IsConRecobrimiento=true) : base(_uiapp)
        {
            // _uidoc = _uiapp.ActiveUIDocument;
            _listaPtosPAthArea = new List<XYZ>();
            _isConRecobrimiento = IsConRecobrimiento;
            //  this._datosMallasDTO = _datosMallasDTO;
        }

        public bool Ejecutar()
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            //  if (!M1_2_SeleccionarBordeMuro()) return false;
            if (!M1_3_SeleccionarMuroElement()) return false;
            if (!M1_4_BuscarPtoInicioBase(_ElemetSelect)) return false;
            if (!M5_SeleccionarPtos()) return false;

            IsOk = true;
            return true;

        }


        public bool Ejecutar_SeleccionarMuroPtoAuto(XYZ ptoCentroMuro , View3D view3D_paraBuscar)
        {
            try
            {
                _view3D_paraBuscar = view3D_paraBuscar;
                return M1_ObtenerPtoinicioAuto(ptoCentroMuro);
            }
            catch (Exception ex)
            {
                if (UtilBarras.IsConNotificaciones)
                    Util.ErrorMsg($"SMAP1 ex:{ex.Message}");
                else
                    Debug.WriteLine($"SMAP1 ex:{ex.Message}");
                return false;
            }
    
        }
        public override bool M1_4_BuscarPtoInicioBase(Element _elemet)
        {
            try
            {

                XYZ normalFAce = _ViewNormalDirection6.Normalize();
                if (AyudaObtenerNormarPlanoVisisible.Obtener(_elemet, _view))
                    normalFAce = AyudaObtenerNormarPlanoVisisible.FaceNormal;

                // CAMBIAR  _ViewDirection---> POR LA NORMAL DEL MURO
                if (_isConRecobrimiento)
                    _ptoSeleccionMouseCentroCaraMuro = _ptoSeleccionMouseCentroCaraMuro - normalFAce * ConstNH.RECUBRIMIENTO_MALLA_foot;
                _planoDeLAcaraDelMuro = Plane.CreateByNormalAndOrigin(-normalFAce, _ptoSeleccionMouseCentroCaraMuro);
                _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = _ptoSeleccionMouseCentroCaraMuro;// _planoDeLAcaraDelMuro.ProjectOnto(_ptoRefereMuro);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

        private bool M5_SeleccionarPtos()
        {
            _listaPtosPAthArea.Clear();
            int contador = 1;
            bool continuar = true;
            while (continuar && contador < 3)
            {
                try
                {
                    continuar = false;
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                    //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                    XYZ PtoSeleccion = _uidoc.Selection.PickPoint(snapTypes, $"Seleccionar Punto{contador} Superior Barra");

                    if (!PtoSeleccion.IsAlmostEqualTo(XYZ.Zero))
                    {
                        XYZ ptoProyectadoSobrePLanoMuro = _planoDeLAcaraDelMuro.ProjectOnto(PtoSeleccion);
                        _listaPtosPAthArea.Add(ptoProyectadoSobrePLanoMuro);
                    }
                    continuar = true;
                    contador += 1;
                }
                catch (Exception)
                {
                    continuar = false;
                }

            }
            return (_listaPtosPAthArea.Count >= 2 ? true : false);
        }

        public SeleccionMuroAreaPathDTO Resultado()
        {
            return new SeleccionMuroAreaPathDTO()
            {
                DIreccionMuroEnIgualSentidoVIewSecction = DIreccionMuroEnIgualSentidoVIewSecction(),
                RightDirection_NORMA = _RightDirection.Normalize(),
                DireccionView_NORMA = _ViewNormalDirection6.Normalize(),
                ListaPtosPAth = _listaPtosPAthArea,
                OrigenView = _origenSeccionView,
                ptoSobreMuro_masRecub = _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost,
                _espesorMuroFoot = _espesorMuroFoot

            };
        }

        //mismo sentido que _RightDirection
        private XYZ DIreccionMuroEnIgualSentidoVIewSecction()
        {
            double valor = Util.GetProductoEscalar(_RightDirection.GetXY0(), _direccionMuro.GetXY0());
            return (valor > 0 ? _direccionMuro : new XYZ(-_direccionMuro.X, -_direccionMuro.Y, _direccionMuro.Z)).Normalize();
        }


    }
}
