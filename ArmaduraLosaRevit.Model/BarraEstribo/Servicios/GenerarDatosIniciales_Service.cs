using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion.Vigas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class GenerarDatosIniciales_Service
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
       
        private SeleccionPtosEstriboConfinamientoMuro _seleccionPtosEstribo;
        private SeleccionPtosEstriboViga _seleccionPtosEstriboViga;
        public SeleccionPtosEstriboViga_sinSeleccionBarras _SeleccionPtosEstriboViga_sinSeleccionBarras { get; set; }
        private View3D _view3D_BUSCAR;

        public  DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO { get; set; }

        public TipoEstriboGenera tipoEstriboGenera { get; set; }
        public List<EstriboMuroDTO> resul_EstriboMuroDTO { get; private set; }
        public ConfiguracionTAgEstriboDTo _configuracionTAgEstriboDTo { get; private set; }
        public View3D _view3D_VISUALIZAR { get; private set; }
        

        public GenerarDatosIniciales_Service(UIApplication uiapp, TipoEstriboGenera tipoEstriboGenera, DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._configuracionInicialEstriboDTO = configuracionInicialEstriboDTO;
            this.tipoEstriboGenera = tipoEstriboGenera;
        }
        //public GenerarDatosIniciales_Service(UIApplication uiapp, View3D view3D, TipoEstriboGenera tipoEstriboGenera, ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo )
        //{
        //    this._uiapp = uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //    this._configuracionTAgEstriboDTo = configuracionTAgEstriboDTo;
        //     this._view3D_VISUALIZAR = view3D;
        //    this.tipoEstriboGenera = tipoEstriboGenera;
        //}


        //a) caso manula
        public bool M1_GeneralDatosIniciales()
        {

            if (tipoEstriboGenera == TipoEstriboGenera.NONE)
            {
                Util.ErrorMsg("Error al definir estribo Generico");
                return false;
            }

             _view3D_VISUALIZAR = TiposFamilia3D.Get3DVisualizar(_doc);
            if (_view3D_VISUALIZAR == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }

            _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            //1)datos de form
            Util1_ObtenerTipodeEstriboGEneral();

        

            if (tipoEstriboGenera == TipoEstriboGenera.Eviga)
            {
                if(_configuracionInicialEstriboDTO.TipoDiseñoEstriboViga == TipoDisenoEstriboVIga.SeleccionarViga)
                    resul_EstriboMuroDTO = M1_2_ObteniendoEstribosViga_seleccionviga();
                else if (_configuracionInicialEstriboDTO.TipoDiseñoEstriboViga == TipoDisenoEstriboVIga.AsignarViga)
                    resul_EstriboMuroDTO = M1_2_ObteniendoEstribosViga_Asignarviga();
                else
                    resul_EstriboMuroDTO = M1_2_ObteniendoEstribosViga_SeleccionarREbar();
               
                
                _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
               {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Horizontal,
                };
                return true;
            }
            else if (tipoEstriboGenera == TipoEstriboGenera.EMuro )
            {
                resul_EstriboMuroDTO = M1_1_ObteniendoEstribosMuro();
                _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Vertical,

                };
                return true;
            }
            else if ( tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
            {
                resul_EstriboMuroDTO = M1_1_ObteniendoEstribosConfinamientoMuro();
                _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Vertical,

                };
                return true;
            }

            return false;
        }

        public List<EstriboMuroDTO> M1_1_ObteniendoEstribosMuro()
        {
            List<EstriboMuroDTO> resul_EstriboMuroDTO = new List<EstriboMuroDTO>();
            //2) seleccionando barras y generando coordenadas
            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
            _seleccionPtosEstribo = new SeleccionPtosEstribosMuro(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO, _seleccionarNivel);
            if (!_seleccionPtosEstribo.M1_Ejecutar()) return resul_EstriboMuroDTO;
            //2.1) obtener resultados 
            resul_EstriboMuroDTO = _seleccionPtosEstribo.M2_ObtenerEstriboMuroDTO();

            return resul_EstriboMuroDTO;
        }

        public List<EstriboMuroDTO> M1_1_ObteniendoEstribosConfinamientoMuro()
        {
            List<EstriboMuroDTO> resul_EstriboMuroDTO = new List<EstriboMuroDTO>();
            //2) seleccionando barras y generando coordenadas
            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
            _seleccionPtosEstribo = new SeleccionPtosEstriboConfinamientoMuro(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO, _seleccionarNivel);
            if (!_seleccionPtosEstribo.M1_Ejecutar()) return resul_EstriboMuroDTO;
            //2.1) obtener resultados 
            resul_EstriboMuroDTO = _seleccionPtosEstribo.M2_ObtenerEstriboMuroDTO();

            return resul_EstriboMuroDTO;
        }

        //
        public List<EstriboMuroDTO> M1_2_ObteniendoEstribosViga_SeleccionarREbar()
        {
            List<EstriboMuroDTO> resul_EstriboVigaDTO = new List<EstriboMuroDTO>();
            //2) seleccionando barras y generando coordenadas
            _seleccionPtosEstriboViga = new SeleccionPtosEstriboViga(_uiapp, _view3D_BUSCAR,  _configuracionInicialEstriboDTO);
            //_seleccionPtosEstriboViga = new SeleccionPtosEstriboVigaGeom(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
            if (!_seleccionPtosEstriboViga.M1_Ejecutar_SeleccionarRebar()) return resul_EstriboVigaDTO;
            //2.1) obtener resultados 

            resul_EstriboVigaDTO = _seleccionPtosEstriboViga.M2_ObtenerEstriboVigaDTO(_seleccionPtosEstriboViga.ObtenerPtoTagViga);
            return resul_EstriboVigaDTO;
        }

        /// <summary>
        /// selecciona viga y otiene el punto inicial y final de estribo con la geometria: 
        /// Nota: resta el recubrimientos en todos las dimensiones tranversales del estribo
        /// </summary>
        ///                           p2  
        ///   |                     |
        ///   p1
        /// <returns></returns>
        public List<EstriboMuroDTO> M1_2_ObteniendoEstribosViga_seleccionviga()
        {
            List<EstriboMuroDTO> resul_EstriboVigaDTO = new List<EstriboMuroDTO>();
            //2) seleccionando barras y generando coordenadas
            _SeleccionPtosEstriboViga_sinSeleccionBarras = new SeleccionPtosEstriboViga_sinSeleccionBarras(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
            //_seleccionPtosEstriboViga = new SeleccionPtosEstriboVigaGeom(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
            if (!_SeleccionPtosEstriboViga_sinSeleccionBarras.M1_Ejecutar_SeleccionViga()) return resul_EstriboVigaDTO;
            //2.1) obtener resultados 

            resul_EstriboVigaDTO = _SeleccionPtosEstriboViga_sinSeleccionBarras.M2_ObtenerEstriboVigaDTO(_SeleccionPtosEstriboViga_sinSeleccionBarras.ObtenerPtoTagViga);
            return resul_EstriboVigaDTO;
        }

        /// <summary>
        /// selecciona viga y otiene el punto inicial y final de estribo con la geometria: 
        /// Nota: resta el recubrimientos en todos las dimensiones tranversales del estribo
        /// </summary>
        ///                           p2  
        ///   |                     |
        ///   p1
        /// <returns></returns>
        public List<EstriboMuroDTO> M1_2_ObteniendoEstribosViga_Asignarviga()
        {
            List<EstriboMuroDTO> resul_EstriboVigaDTO = new List<EstriboMuroDTO>();
            //2) seleccionando barras y generando coordenadas
            _SeleccionPtosEstriboViga_sinSeleccionBarras = new SeleccionPtosEstriboViga_sinSeleccionBarras(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
            
            if (!_SeleccionPtosEstriboViga_sinSeleccionBarras.M1_Ejecutar_SeleccionViga(_configuracionInicialEstriboDTO.ElementoSeleccionado)) return resul_EstriboVigaDTO;
            //2.1) obtener resultados 

            resul_EstriboVigaDTO = _SeleccionPtosEstriboViga_sinSeleccionBarras.M2_ObtenerEstriboVigaDTO(_SeleccionPtosEstriboViga_sinSeleccionBarras.ObtenerPtoTagViga);
            return resul_EstriboVigaDTO;
        }


        //b) caso automatico
        public bool M2_GeneralDatosIniciales_Auto()
        {
            if (tipoEstriboGenera == TipoEstriboGenera.NONE)
            {
                Util.ErrorMsg("Error al definir estribo Generico");
                return false;
            }

            _view3D_VISUALIZAR = TiposFamilia3D.Get3DVisualizar(_doc);
            if (_view3D_VISUALIZAR == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }
            _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            //1)datos de form
            Util1_ObtenerTipodeEstriboGEneral();

            if (tipoEstriboGenera == TipoEstriboGenera.Eviga)
            {
             //   resul_EstriboMuroDTO = ObteniendoEstribosViga();
                _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Horizontal,
                };
                return true;
            }
            else if (tipoEstriboGenera == TipoEstriboGenera.EMuro || tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
            {
              //  resul_EstriboMuroDTO = ObteniendoEstribosConfinamientoPilar();
                _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Vertical,
                };
                return true;
            }
            return false;
        }

        // util
        private void Util1_ObtenerTipodeEstriboGEneral()
        {
            switch (_configuracionInicialEstriboDTO.tipoConfiguracionEstribo)
            {
                case TipoConfiguracionEstribo.Estribo:
                case TipoConfiguracionEstribo.Estribo_Lateral:
                case TipoConfiguracionEstribo.Estribo_Traba:
                case TipoConfiguracionEstribo.Estribo_Lateral_Traba:
                case TipoConfiguracionEstribo.Traba:
                    tipoEstriboGenera = TipoEstriboGenera.EConfinamiento;
                    break;
                case TipoConfiguracionEstribo.EstriboMuro:
                case TipoConfiguracionEstribo.EstriboMuro_Lateral:
                case TipoConfiguracionEstribo.EstriboMuro_Traba:
                case TipoConfiguracionEstribo.EstriboMuro_Lateral_Traba:
                case TipoConfiguracionEstribo.EstriboMuroTraba:
                    tipoEstriboGenera = TipoEstriboGenera.EMuro;
                    break;
                case TipoConfiguracionEstribo.EstriboViga:
                case TipoConfiguracionEstribo.EstriboViga_Lateral:
                case TipoConfiguracionEstribo.EstriboViga_Traba:
                case TipoConfiguracionEstribo.EstriboViga_Lateral_Traba:
                case TipoConfiguracionEstribo.VigaTraba:
                    tipoEstriboGenera = TipoEstriboGenera.Eviga;
                    break;
                default:
                    tipoEstriboGenera = TipoEstriboGenera.NONE;
                    break;
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
