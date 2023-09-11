
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using rmaduraLosaRevit.Model.BarraV.TipoBarra;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Barras
{
    public class BarraRefuerzoEstriboMuroSinTras : ARebarsSinTrans
    {
        private double recubrimiento;
        private string _candidadEstribo;

        #region 0)propiedades
        public double LargoCurvaRutaFoor { get; set; }
        public double espesorRealEstriboFoot { get; set; }
        private double anchoEstribo;
        private string _nombreREbarshape;

        private XYZ yvecLat;
        private XYZ xvecLat;
        private double espaciamientoFoot;
        private double recorridoBrrar;
        private double _largoCurva;
        private double espesorMuroVigaFoot;
        private readonly UIApplication _uiapp;
        private readonly GenerarDatosIniciales_Service _generarDatosIniciales_Service;
        private readonly EstriboMuroDTO _estriboMuroDTO;


        public double espaciamientoEntreEstriboFoot { get; set; }
        #endregion

        #region 1) Constructores
        public BarraRefuerzoEstriboMuroSinTras(UIApplication uiapp, View view, GenerarDatosIniciales_Service generarDatosIniciales_Service, EstriboMuroDTO er, IGeometriaTag _newGeometriaTag) : base(uiapp,_newGeometriaTag)
        {
            this._uiapp = uiapp;
            this._generarDatosIniciales_Service = generarDatosIniciales_Service;
            this._estriboMuroDTO = er;
            this._newGeometriaTag = _newGeometriaTag;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.anchoEstribo = er.AnchoVisibleFoot;
            this.espaciamientoEntreEstriboFoot = er.EspaciamientoEntreEstriboFoot;
            this.diamtroBarraMM = er.DiamtroBarraEnMM;
            this.espesorMuroVigaFoot = er.Espesor_ElementHostFoot;
            this.ElementoHost = er.ElementHost;
            this.origen_forCreateFromRebarShape = er.OrigenEstribo;
            this.recubrimiento = ConstNH.RECUBRIMIENTO_MURO_CM;
            this._candidadEstribo = er.cantidadEstribo;
            view3D_Visualizar = generarDatosIniciales_Service._view3D_VISUALIZAR;
            //vista actual
            viewActual = view;

            LargoCurvaRutaFoor = er.largoRecorridoEstriboFoot;
            yvec = er.direccionPerpenEntradoHaciaViewSecction;
            xvec = er.direccionParalelaViewSecctioin;

            xvecLat = er.direccionPerpenEntradoHaciaViewSecction;
            yvecLat = er.direccionParalelaViewSecctioin;
            espesorRealEstriboFoot = er._anchoEstribo1Foot;
            _nombreREbarshape = "M_T1";

        }
        #endregion

        #region 2) metodos
        public void M1_GenerarBarra()
        {
            //BarraTipo = BarraTipo.ELEV_ES;
            //OBTENER Y CONFIGURAR REBARSHAPE
            M1_1_ConfigurarRebarShape();
            M1_Configurar_cambiarTipoBarraEStribo();
            // ConfigurarParametrosRebarshapeANTESDibujuhar();
            //DIBUJAR
            M1_2_DibujarBarraRebarShape();
            //configura espaciemientore
            M1_3_ConfiguraEspaciamientoEstribo();



        }
        public void M1_GenerarBarra_2parte()
        {
            if (EstriboRebarCreado == null) return;
            //cambiar dimensiones
            M1_4_ConfigurarAsignarParametrosRebarshape();
            // visualizar en 3d
            M1_6_visualizar();
        }
        public List<ElementId> GenerarLaterales()
        {
            List<ElementId> listaIdLaterales = new List<ElementId>();
            if (_estriboMuroDTO.ListaLateralesDTO == null) return listaIdLaterales;
            foreach (BarraLateralesDTO item in _estriboMuroDTO.ListaLateralesDTO)
            {
                M1_1_AsignarEspaciamientoYrecorrido();
                M2_ConfigurarLateralCurve(item._diamtroLat);
                M0_CalcularCurva(item);
                M3_DibujarBarraCurve();
                M1_5__ConfiguraEspaciamientoLaterales();
                item.LateralCreada = EstriboRebarCreado;

                listaIdLaterales.Add(EstriboRebarCreado.Id);
            }

            return listaIdLaterales;
        }

        public List<ElementId> GenerarLaterales_2parte()
        {
            List<ElementId> listaIdLaterales = new List<ElementId>();
            if (_estriboMuroDTO.ListaLateralesDTO == null) return listaIdLaterales;
            // BarraTipo = BarraTipo.ELEV_ES_L;
            foreach (BarraLateralesDTO item in _estriboMuroDTO.ListaLateralesDTO)
            {
                EstriboRebarCreado = item.LateralCreada as Rebar;
                if (EstriboRebarCreado == null) continue;
                M4_ConfigurarAsignarParametrosRebarLaterales(item);
                M1_6_visualizar();

            }

            return listaIdLaterales;
        }

        //en desarrollo
        public List<ElementId> GenerarTrabas()
        {
            List<ElementId> listaIdTrabas = new List<ElementId>();

            if (_estriboMuroDTO.ListaTrabasDTO == null) return listaIdTrabas;
            foreach (BarraTrabaDTO item in _estriboMuroDTO.ListaTrabasDTO)
            {
                M2_ConfigurarTrabaCurve(item);
                //M1_Configurar_cambiarTipoBarraEStribo();
                M0_CalcularCurvaTraba(item);

                M3_DibujarBarraCurve();
                M1_3_ConfiguraEspaciamientoTraba();
                //M1_5__ConfiguraEspaciamientoLaterales();
                item.TrabaCreada = EstriboRebarCreado;
                // M4_ConfigurarAsignarParametrosRebarTraba(item);
                // M1_6_visualizar();
                listaIdTrabas.Add(EstriboRebarCreado.Id);
            }

            return listaIdTrabas;
        }
        public List<ElementId> GenerarTrabas_2parte()
        {
            List<ElementId> listaIdTrabas = new List<ElementId>();

            if (_estriboMuroDTO.ListaTrabasDTO == null) return listaIdTrabas;
            // BarraTipo = BarraTipo.ELEV_ES_T;
            foreach (BarraTrabaDTO item in _estriboMuroDTO.ListaTrabasDTO)
            {
                EstriboRebarCreado = item.TrabaCreada;
                if (EstriboRebarCreado == null) continue;
                M4_ConfigurarAsignarParametrosRebarTraba(item);
                M1_6_visualizar();
            }
            return listaIdTrabas;
        }

        private void M0_CalcularCurvaTraba(BarraTrabaDTO item)
        {

            listcurve = new System.Collections.Generic.List<Curve>();
            Curve ladoCentral = Line.CreateBound(item._startPont_, item._endPoint);
            _largoCurva = Math.Round(ladoCentral.Length, 2);
            listcurve.Add(ladoCentral);
        }

        public override void M2_ConfigurarBarraCurve()
        {
            GenerarLaterales();
        }
        public bool M2_ConfigurarLateralCurve(int diametro)
        {

            startHookOrient = RebarHookOrientation.Left; //defecto
            endHookOrient = RebarHookOrientation.Left; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;

            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametro, _doc, true);
            norm = _estriboMuroDTO.direccionPerpenEntradoHaciaViewSecction;


            if (rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo {"Ø " + diametro}");
                return false;
            }
            return true;
        }
        private void M1_1_AsignarEspaciamientoYrecorrido()
        {
            int _nuevaLineaCantidadbarra = 2;
            if (_nuevaLineaCantidadbarra == 1)
            {
                throw new NotImplementedException("Solo Una linea de barra.Caso no implementado");
            }
            else
            {
                this.espaciamientoFoot = (this.espesorMuroVigaFoot
                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                                        - Util.MmToFoot(diamtroBarraMM)) / (_nuevaLineaCantidadbarra - 1);
                this.recorridoBrrar = (this.espesorMuroVigaFoot
                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                                        - Util.MmToFoot(diamtroBarraMM));
            }

        }
        public bool M2_ConfigurarTrabaCurve(BarraTrabaDTO item)
        {

            startHookOrient = item._ubicacionHook; //defecto
            endHookOrient = item._ubicacionHook; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;

            EndHook = TipoRebarHookType.ObtenerHook("Stirrup/Tie - 135 degNH", _doc); ;
            StartHook = TipoRebarHookType.ObtenerHook("Stirrup/Tie - 135 degNH", _doc); ;

            rebarStyle = RebarStyle.StirrupTie;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + item._diamtroTraba, _doc, true);
            norm = new XYZ(0, 0, 1);


            if (rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo {"Ø " + item._diamtroTraba}");
                return false;
            }
            return true;
        }
        public void M0_CalcularCurva(BarraLateralesDTO item)
        {
            //  listcurve.Clear();
            listcurve = new System.Collections.Generic.List<Curve>();

            //  CrearModeLineAyuda.modelarlinea_sinTrans(_doc,item._startPont_, item._endPoint);
            Curve ladoCentral = Line.CreateBound(item._startPont_, item._endPoint);
            _largoCurva = Math.Round(ladoCentral.Length, 2);
            listcurve.Add(ladoCentral);

        }
        private void M1_1_ConfigurarRebarShape()
        {
            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diamtroBarraMM, _doc, true);
            // rebar shape
            rebarShape = TiposFormasRebarShape.getRebarShape(_nombreREbarshape, _doc);
            //ConfigurarAsignarParametrosRebarshapev2();

        }
        public Result M1_3_ConfiguraEspaciamientoEstribo()
        {
            Result result = Result.Failed;

            try
            {
                if (EstriboRebarCreado == null) return Result.Failed;
                Element paraElem = _doc.GetElement(EstriboRebarCreado.Id);
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = EstriboRebarCreado.GetShapeDrivenAccessor();
                // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                int numerobarras = (int)(LargoCurvaRutaFoor / espaciamientoEntreEstriboFoot) + 1;
                XYZ aux_xvect = xvec * anchoEstribo;//*espesorEstribo;  0.5 es para que en rebarshape inicial no que fuera y generen mensaje advertencia
                XYZ aux_yvect = yvec * espesorRealEstriboFoot; //ancho del extremos de cada lado-- largo mas extremo
                rebarShapeDrivenAccessor.ScaleToBox(origen_forCreateFromRebarShape, aux_xvect, aux_yvect);

                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoEntreEstriboFoot, true, true, true);
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                result = Result.Cancelled;
            }

            return result;
        }

        public Result M1_3_ConfiguraEspaciamientoTraba()
        {
            Result result = Result.Failed;

            //solucion parche
            double espaciamientolateralFoot = Util.CmToFoot(_generarDatosIniciales_Service._configuracionInicialEstriboDTO.espaciamientoTrabaCM);
            try
            {
                Element paraElem = _doc.GetElement(EstriboRebarCreado.Id);
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = EstriboRebarCreado.GetShapeDrivenAccessor();
                int numerobarras = (int)(LargoCurvaRutaFoor / espaciamientolateralFoot) + 1;
                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientolateralFoot, true, true, true);
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }



        public Result M1_5__ConfiguraEspaciamientoLaterales()
        {


            Result result = Result.Failed;
            try
            {
                //using (Transaction t = new Transaction(_doc))
                //  {
                //  t.Start("ConfiguraEspaciamiento barra refuerzo");
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = EstriboRebarCreado.GetShapeDrivenAccessor();
                //rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espaciamientoFoot, recorridoBrrar, true, true, true);
                //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2,1,  true, true, true);
                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(2, espaciamientoFoot, true, true, true);
                //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2, 1, true, true, true);
                result = Result.Succeeded;
                // t.Commit();
                //  }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_3_ConfiguraEspaciamientoLaterales()
        {
            int _nuevaLineaCantidadbarra = 2;
            this.espaciamientoFoot = (this.espesorRealEstriboFoot - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diamtroBarraMM) / 10)) * 2.0) / (_nuevaLineaCantidadbarra - 1);
            this.recorridoBrrar = (this.espesorRealEstriboFoot - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diamtroBarraMM) / 10)) * 2.0);
            Result result = Result.Failed;
            try
            {
                //  Element paraElem = _doc.GetElement(_rebar.Id);
                //   ParameterUtil.SetParaInt(paraElem, $"CantidadEstriboCONF", _candidadEstribo);
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = EstriboRebarCreado.GetShapeDrivenAccessor();
                // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                int numerobarras = 2;
                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoFoot, true, true, true);
                //   rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espesorRealEstriboFoot, true, true, true);
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }

            return result;
        }


        private void M1_4_ConfigurarAsignarParametrosRebarshape()
        {
            try
            {
                if (viewActual != null && ParameterUtil.FindParaByName(EstriboRebarCreado, "NombreVista") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "B") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "B", anchoEstribo);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "C") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "C", espesorRealEstriboFoot);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "D") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "D", anchoEstribo);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "E") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "E", espesorRealEstriboFoot);

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "B_") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "B_", anchoEstribo);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "C_") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "C_", espesorRealEstriboFoot);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "D_") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "D_", anchoEstribo);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "E_") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "E_", espesorRealEstriboFoot);

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CantidadEstriboCONF") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, $"CantidadEstriboCONF", _candidadEstribo);


                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CantidadEstriboLAT") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "CantidadEstriboLAT", tipoEstribo + "Ø" + diamtroBarraMM + "a" + espaciamientoEntreEstriboFoot);


                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CantidadEstriboTRABA") != null && _estriboMuroDTO.TextoAUXTraba != "")
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "CantidadEstriboTRABA", _estriboMuroDTO.TextoAUXTraba);
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }



        private void M4_ConfigurarAsignarParametrosRebarLaterales(BarraLateralesDTO item)
        {

            try
            {
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "B") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "B", _largoCurva);

                if (viewActual != null && ParameterUtil.FindParaByName(EstriboRebarCreado, "NombreVista") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CantidadEstriboLAT") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "CantidadEstriboLAT", item._textoLat);

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }




        private void M4_ConfigurarAsignarParametrosRebarTraba(BarraTrabaDTO item)
        {
            try
            {
                if (viewActual != null && ParameterUtil.FindParaByName(EstriboRebarCreado, "NombreVista") != null)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));

                //if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", _largoCurva);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CantidadEstriboTRABA") != null && item._textoTraba != "")
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "CantidadEstriboTRABA", item._textoTraba);
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }

        #endregion

    }
}
