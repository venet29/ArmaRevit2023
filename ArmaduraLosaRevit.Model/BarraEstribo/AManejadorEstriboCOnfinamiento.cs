
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion;
using ArmaduraLosaRevit.Model.BarraEstribo.TAG;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.BarraEstribo.Barras;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;

namespace ArmaduraLosaRevit.Model.BarraEstribo
{
    public abstract class AManejadorEstriboCOnfinamiento
    {
        protected UIApplication _uiapp;
        protected DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;
        protected Document _doc;
        protected View _view;
        protected List<ElementId> _listaRebarIdCambiarColor;

        protected TipoEstriboConfig _configuracionEnfierrado;
        protected Element rebarEstribo;
        protected TipoRebar _BarraTipo = TipoRebar.NONE;
        protected View3D _view3D_buscar;
        protected View3D _view3D_paraVisualizar;

        public List<ElementId> ListaLateralesRebarIdCreados { get; private set; }
        public List<ElementId> ListaTrabasRebarIdCreadas { get; private set; }

        //  ConfiguracionTAgEstriboDTo _configuracionTAgEstriboDTo = null;
        // List<EstriboMuroDTO> resul_EstriboMuroDTO = null;
        // private View3D _view3D_BUSCAR;

        public AManejadorEstriboCOnfinamiento(UIApplication uiapp, DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
        {
            this._uiapp = uiapp;
            this._configuracionInicialEstriboDTO = configuracionInicialEstriboDTO;
            this._doc = uiapp.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
            _listaRebarIdCambiarColor = new List<ElementId>();
            ListaLateralesRebarIdCreados = new List<ElementId>();
            ListaTrabasRebarIdCreadas = new List<ElementId>();
            //  this._configuracionTAgEstriboDTo = null;
            // this.resul_EstriboMuroDTO = null;
        }

        #region Estribo
     

        protected BarraRefuerzoEstriboMuroSinTras m1_1_GEnerarBArrasSinTras(GenerarDatosIniciales_Service generarDatosIniciales_Service, EstriboMuroDTO item)
        {

            IGeometriaTag IGeometriaTag = null;
            if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.Eviga)
                IGeometriaTag = new GeomeTagEstriboViga(_doc, item.Posi1TAg, item.NombreFamilia, _configuracionEnfierrado);
            else if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                IGeometriaTag = new GeomeTagEstriboConfinamiento(_doc, item, _configuracionEnfierrado);
            else if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EMuro)
            {
                if (_configuracionInicialEstriboDTO.IsTrabaFalsa==true)
                {
                    if (_configuracionEnfierrado == TipoEstriboConfig.EL)
                        _configuracionEnfierrado = TipoEstriboConfig.EL_TF;
                    else if (_configuracionEnfierrado == TipoEstriboConfig.E)
                        _configuracionEnfierrado = TipoEstriboConfig.E_TF;
                }
                IGeometriaTag = new GeomeTagEstriboMuro(_doc, item, _configuracionEnfierrado);
            }
            

            IGeometriaTag.Ejecutar(new GeomeTagArgs());

            //estribo
            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboMuro = new BarraRefuerzoEstriboMuroSinTras(_uiapp, _doc.ActiveView, generarDatosIniciales_Service, item, IGeometriaTag);
            //  barraRefuerzoEstriboMuro.tipo
            barraRefuerzoEstriboMuro.BarraTipo = ObtenerBarraTipo_estribo(generarDatosIniciales_Service.tipoEstriboGenera);// BarraTipo.ELEV_ES;
            barraRefuerzoEstriboMuro.M1_GenerarBarra();
            if(barraRefuerzoEstriboMuro.EstriboRebarCreado!=null)
                _listaRebarIdCambiarColor.Add(barraRefuerzoEstriboMuro.EstriboRebarCreado.Id);
            rebarEstribo = barraRefuerzoEstriboMuro.EstriboRebarCreado;
            //tag
            // barraRefuerzoEstriboMuro.DibujarTagsEstribo(generarDatosIniciales_Service._configuracionTAgEstriboDTo);


            return barraRefuerzoEstriboMuro;
        }

        private TipoRebar ObtenerBarraTipo_estribo(TipoEstriboGenera tipoEstriboGenera)
        {
            if (tipoEstriboGenera == TipoEstriboGenera.Eviga)
                return TipoRebar.ELEV_ES_V;
            else if (tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                return TipoRebar.ELEV_CO;
            else if (tipoEstriboGenera == TipoEstriboGenera.EMuro)
                return TipoRebar.ELEV_ES;
            return TipoRebar.NONE;
        }
        #endregion



        #region metodos traba
        //metodo cuando se implente trabas

        protected BarraRefuerzoEstriboMuroSinTras M1_3_GenerarTrabasGeneralSinTrans(GenerarDatosIniciales_Service generarDatosIniciales_Service, EstriboMuroDTO item)
        {
            IGeometriaTag IGeometriaTagTraba = null;
            if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.Eviga)
                IGeometriaTagTraba = new GeomeTagTrabaViga(_doc, item, _configuracionEnfierrado);
            else if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EMuro || generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                IGeometriaTagTraba = new GeomeTagTrabaMuro(_doc, item, _configuracionEnfierrado);

            IGeometriaTagTraba.Ejecutar(new GeomeTagArgs());

            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboMuroTrabas = new BarraRefuerzoEstriboMuroSinTras(_uiapp, _doc.ActiveView, generarDatosIniciales_Service, item, IGeometriaTagTraba);
            barraRefuerzoEstriboMuroTrabas.BarraTipo = ObtenerBarraTipo_Traba(generarDatosIniciales_Service.tipoEstriboGenera);//
            ListaTrabasRebarIdCreadas = barraRefuerzoEstriboMuroTrabas.GenerarTrabas();

            if (ListaTrabasRebarIdCreadas.Count == 0) return null;

            barraRefuerzoEstriboMuroTrabas.DibujarTagsEstribo(generarDatosIniciales_Service._configuracionTAgEstriboDTo, _doc.GetElement(ListaTrabasRebarIdCreadas[0]));

            if (ListaTrabasRebarIdCreadas.Count > 0) _listaRebarIdCambiarColor.AddRange(ListaTrabasRebarIdCreadas);

            return barraRefuerzoEstriboMuroTrabas;
        }

        private TipoRebar ObtenerBarraTipo_Traba(TipoEstriboGenera tipoEstriboGenera)
        {
            if (tipoEstriboGenera == TipoEstriboGenera.Eviga)
                return TipoRebar.ELEV_ES_VT;
            else if (tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                return TipoRebar.ELEV_CO_T;
            else if (tipoEstriboGenera == TipoEstriboGenera.EMuro)
                return TipoRebar.ELEV_ES_T;
            return TipoRebar.NONE;
        }
        #endregion

        #region laterales

        protected BarraRefuerzoEstriboMuroSinTras M1_2_GenerarLAteralesGeneralSinTrans(GenerarDatosIniciales_Service generarDatosIniciales_Service, EstriboMuroDTO item)
        {
            IGeometriaTag IGeometriaTagLAt = null;
            if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.Eviga)
                IGeometriaTagLAt = new GeomeTagLateralesViga(_doc, item.Posi1TAg, item.NombreFamilia + "L");
            else if (generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EMuro || generarDatosIniciales_Service.tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                IGeometriaTagLAt = new GeomeTagLateralesMuro(_doc, item, _configuracionEnfierrado);


            IGeometriaTagLAt.Ejecutar(new GeomeTagArgs());
            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboMuroLat = new BarraRefuerzoEstriboMuroSinTras(_uiapp, _doc.ActiveView, generarDatosIniciales_Service, item, IGeometriaTagLAt);
            barraRefuerzoEstriboMuroLat.BarraTipo = ObtenerBarraTipo_Lateral(generarDatosIniciales_Service.tipoEstriboGenera);//
             ListaLateralesRebarIdCreados = barraRefuerzoEstriboMuroLat.GenerarLaterales();

            if (ListaLateralesRebarIdCreados.Count > 0)
                barraRefuerzoEstriboMuroLat.DibujarTagsEstribo(generarDatosIniciales_Service._configuracionTAgEstriboDTo, _doc.GetElement(ListaLateralesRebarIdCreados[0]));

            if (ListaLateralesRebarIdCreados.Count > 0) _listaRebarIdCambiarColor.AddRange(ListaLateralesRebarIdCreados);

            return barraRefuerzoEstriboMuroLat;
        }

        private TipoRebar ObtenerBarraTipo_Lateral(TipoEstriboGenera tipoEstriboGenera)
        {
            if (tipoEstriboGenera == TipoEstriboGenera.Eviga)
                return TipoRebar.ELEV_ES_VL;
            else if (tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                return TipoRebar.NONE;
            else if (tipoEstriboGenera == TipoEstriboGenera.EMuro)
                return TipoRebar.ELEV_ES_L;
            return TipoRebar.NONE;
        }
        #endregion

    
        protected bool CalculosIniciales()
        {
            _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);
            _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            if (_view3D_buscar == null) return true;
            if (_view3D_paraVisualizar == null) return true;

            return true;

        }


    }
}
