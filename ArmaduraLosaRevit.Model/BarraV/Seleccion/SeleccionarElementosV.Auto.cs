using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Niveles.Vigas;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
    public partial class SeleccionarElementosV
    {
        #region ProgramaAutomatico
        public bool M1_ObtenerPtoinicioAuto(XYZ PtoInicioBaseBordeMuro, XYZ ptoBuscarMuro)
        {
            _PtoInicioBaseBordeMuro = PtoInicioBaseBordeMuro;

            return M1_ObtenerPtoinicioAuto(ptoBuscarMuro);
        }

        public bool M1_ObtenerPtoinicioAuto(XYZ ptoBuscarMuro)
        {
            XYZ direccionBusqueda = XYZ.Zero;

            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            if (_PtoInicioBaseBordeMuro == null)
                direccionBusqueda = _RightDirection; //caso malla automatico
            else
                direccionBusqueda = (ptoBuscarMuro.GetXY0() - _PtoInicioBaseBordeMuro.GetXY0()).Normalize(); //caso barra  cabeza de muro automarico

            if (!M1_3_SeleccionarMuroElementAuto(ptoBuscarMuro, direccionBusqueda)) return false;// + _ViewNormalDirection6 * ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW
            return M1_4_BuscarPtoInicioBase(_ElemetSelect);
        }



        public bool M1_3_SeleccionarMuroElementAuto(XYZ ptoBuscarMuro, XYZ direccionBusqueda)
        {
            try
            {
                BuscarMuros buscarMuros = null;
                bool mantaner = true;

                XYZ ptoUAxBUsqueda = ptoBuscarMuro.ObtenerCopia();

                buscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW * 1.1);
                int cont = 0;
                while (mantaner)
                {
                     _ElemetSelect = buscarMuros.OBtenerRefrenciaMuroOViga_paraAuto(_view3D_paraBuscar, 
                                                                                    ptoBuscarMuro+_ViewNormalDirection6*ConstNH.CONST_DISTANCIA_RETROCESO_BUSQUEDA_FOOT_PERPENDICULAR_VIEW,
                                                                                    -_ViewNormalDirection6);

                    cont += 1;
                    if (_ElemetSelect == null)
                    {   //obs1)cuando se busca un muro y este es perpendicular ala vista, se mover 10 cm el pto de b
                        ptoBuscarMuro = ptoBuscarMuro + (direccionBusqueda.Normalize() + new XYZ(0, 0, -1)) * ConstNH.CONST_MOVER_PTO_BUSCAR_MURO_FOOT;
                    }
                    else if (!_ElemetSelect.IsValidObject)
                    {
                        ptoBuscarMuro = ptoBuscarMuro + (direccionBusqueda.Normalize() + new XYZ(0, 0, -1)) * ConstNH.CONST_MOVER_PTO_BUSCAR_MURO_FOOT;
                    }
                    else if (Util.IsEqual(Math.Abs(Util.GetProductoEscalar(buscarMuros._direccionMuro, _ViewNormalDirection6)), 0, 0.001))
                    { mantaner = false; }
                    else
                    {
                        //obs1)cuando se busca un muro y este es perpendicular ala vista, se mover 10 cm el pto de b
                        ptoBuscarMuro = ptoBuscarMuro + (direccionBusqueda.Normalize() + new XYZ(0, 0, -1)) * ConstNH.CONST_MOVER_PTO_BUSCAR_MURO_FOOT;
                    }


                    if (cont > 10)
                    {
                        Debug.WriteLine($" pto de busqueda :{ ptoBuscarMuro.ToString()}   _ViewNormalDirection: {_ViewNormalDirection6}");
                        return false;
                    }
                }


                _ptoSeleccionMouseCentroCaraMuro = buscarMuros.PuntoSobreFAceHost;
                if (_ElemetSelect == null)
                {
                    string textoError = $"No se puedo encontrar Muro de referencia. Pto busqueda:{ptoBuscarMuro.ToString()}";
                    if (UtilBarras.IsConNotificaciones)
                        Util.ErrorMsg(textoError);
                    else
                        Debug.WriteLine(textoError);

                    return false;
                }
                m1_3_1_AuxObtenerMuros(_ElemetSelect);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

        public void AsignarPtosDeseleccion_soloMallaAuto(XYZ _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost, List<XYZ> listaPtos)
        {
            this._ptoSeleccionMouseCentroCaraMuro = (listaPtos[0] + listaPtos[2]) / 2;
            this._PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost;
        }




        #endregion


    }
}
