using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
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
    public class SeleccionMuroAreaPathAuto : SeleccionarElementosV
    {
        public List<XYZ> _listaPtosPAthArea;
#pragma warning disable CS0169 // The field 'SeleccionMuroAreaPathAuto._planoDeLAcaraDelMuro' is never used
        private Plane _planoDeLAcaraDelMuro;
#pragma warning restore CS0169 // The field 'SeleccionMuroAreaPathAuto._planoDeLAcaraDelMuro' is never used
        private IntervalosMallaDTOAuto newIntervalosMallaDTOAuto;
        public bool Isok { get; set; }
        public XYZ ptoDiseñoMuro { get; set; }

        //  private readonly DatosMallasDTO _datosMallasDTO;

        public bool IsOk { get; set; }

        public SeleccionMuroAreaPathAuto(UIApplication _uiapp, IntervalosMallaDTOAuto newIntervalosMallaDTOAuto, View3D view3D_paraBuscar) : base(_uiapp)
        {
            // _uidoc = _uiapp.ActiveUIDocument;
            _listaPtosPAthArea = new List<XYZ>();
            _view3D_paraBuscar = view3D_paraBuscar;
            this.newIntervalosMallaDTOAuto = newIntervalosMallaDTOAuto;
            this.IsOk = true;
            //  this._datosMallasDTO = _datosMallasDTO;
        }



        public bool Ejecutar_SeleccionarMuroPtoAuto()
        {
            try
            {

                XYZ _p1 = newIntervalosMallaDTOAuto.ListaPtos[0];
                XYZ _p2 = newIntervalosMallaDTOAuto.ListaPtos[1];
                XYZ ptoCentro = (_p1 + _p2) / 2;

                M1_ObtenerPtoEnmuroAuto(ptoCentro);
                          
                if (!M1_4_BuscarPtoInicioBase(_p1)) return IsOk = false;
                newIntervalosMallaDTOAuto.ListaPtos[0] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;

                if (!M1_4_BuscarPtoInicioBase(_p2)) return IsOk = false;
                newIntervalosMallaDTOAuto.ListaPtos[1] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;

                if (!M1_4_BuscarPtoInicioBase(newIntervalosMallaDTOAuto.ListaPtos_mallaVertical[0])) return IsOk = false;
                newIntervalosMallaDTOAuto.ListaPtos_mallaVertical[0] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;

                if (!M1_4_BuscarPtoInicioBase(newIntervalosMallaDTOAuto.ListaPtos_mallaVertical[1])) return IsOk = false;
                newIntervalosMallaDTOAuto.ListaPtos_mallaVertical[1] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;
            }
            catch (Exception)
            {
                IsOk = false;                
            }
            return IsOk;
        }

        public XYZ M1_ObtenerPtoEnmuroAuto(XYZ ptoBuscarMuro)
        {
            try
            {


                if (!M1_1_CrearWorkPLane_EnCentroViewSecction())
                {
                    if (UtilBarras.IsConNotificaciones)
                        Util.ErrorMsg("No se pudo obtener pto sobre cara de muro:");
                    else
                        Debug.WriteLine("No se pudo obtener pto sobre cara de muro:");

                    IsOk = false;
                    return XYZ.Zero;
                }

                //NOTA: XYZ direccionBusqueda : _ViewNormalDirection   ... SOLO COMO VALOR DEFAUL PQ AL SELECCIONAR EL CENTRO, SE SUPONE QUE SE ENCUENTRA MURO EN 1ER ITERACION, SIN NECESIDAD MOVER
                if (!M1_3_SeleccionarMuroElementAuto(ptoBuscarMuro + _ViewNormalDirection6 * ConstNH.CONST_DISTANCIA_RETROCESO_BUSQUEDA_FOOT_PERPENDICULAR_VIEW, _ViewNormalDirection6))

                {
                    if (UtilBarras.IsConNotificaciones)
                        Util.ErrorMsg("No se pudo obtener pto sobre cara de muro:");
                    else
                        Debug.WriteLine("No se pudo obtener pto sobre cara de muro:");
                    IsOk = false;
                    return XYZ.Zero;
                }

                newIntervalosMallaDTOAuto._datosMallasDTO.espesorFoot = _espesorMuroFoot;
            }
            catch (Exception ex)
            {

                Util.ErrorMsg("Error, No se pudo obtener pto sobre cara de muro ex:"+ ex.Message);
                IsOk=false;
                return XYZ.Zero;
            }

            return _ptoSeleccionMouseCentroCaraMuro;
        }


        public virtual bool M1_4_BuscarPtoInicioBase(XYZ pto)
        {
            try
            {

                Plane plano = Plane.CreateByNormalAndOrigin(_view.ViewDirection, _ptoSeleccionMouseCentroCaraMuro);
                ptoDiseñoMuro = plano.ProjectOnto(pto);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
