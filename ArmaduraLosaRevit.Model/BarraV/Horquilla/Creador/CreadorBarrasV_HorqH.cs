using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Creador
{
    public class CreadorBarrasV_HorqH: CreadorBarrasV
    {

       


        List<TraslapoDTO> ListaTraslapo = new List<TraslapoDTO>();
        public CreadorBarrasV_HorqH(UIApplication uiapp,
            SelecionarPtoSup selecionarPtoSup,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO) : base(uiapp, selecionarPtoSup, confiEnfierradoDTO, muroSeleccionadoDTO)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;

            this._confiWPFEnfierradoDTO = confiEnfierradoDTO;
  
            _listaRebar = new List<Rebar>();
            ManejadorTraslapo = new ManejadorTraslapo(uiapp);
            ListaTraslapo = new List<TraslapoDTO>();
            confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                IsDIrectriz = true,
                LeaderElbow = new XYZ(0, 0, 1.5),
                tagOrientation = TagOrientation.Vertical

            };
        }

        public override bool Ejecutar(int ii)
        {
            try
            {
                M1_AsignarCAntidadEspaciemientoNuevaLineaBarra(ii);

                IGenerarIntervalosV igenerarIntervalos = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosV(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                M2_GenerarIntervalos(igenerarIntervalos);

                M3_1_ObtenerConfiguracionTAgBarraDTo();
                M3_DibujarBarras_sinTrasn(confBarraTag);


                M4_DibujarBarrasCOnfiguracion();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex, "Error al crear rebar verticaL CreadorBarrasV");
                return false;
            }

            return true;
        }
        
        public override bool Ejecutar_sinTrans(int ii)
        {
            try
            {
                M1_AsignarCAntidadEspaciemientoNuevaLineaBarra(ii);

                IGenerarIntervalosV igenerarIntervalos = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosV(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                M2_GenerarIntervalos(igenerarIntervalos);

                M3_1_ObtenerConfiguracionTAgBarraDTo();

                M3_DibujarBarras_sinTrasn(confBarraTag);

                M4_DibujarBarrasCOnfiguracion_sintras();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex, "Error al crear rebar verticaL CreadorBarrasV");
                return false;
            }

            return true;
        }

        /*
        protected void M2_GenerarIntervalos(IGenerarIntervalosV igenerarIntervalos)
        {

            igenerarIntervalos.M1_ObtenerIntervaloBarrasDTO();
            igenerarIntervalos.M2_GenerarListaBarraVertical();

            if (igenerarIntervalos.ListaIntervaloBarrasDTO.Count > 0 && igenerarIntervalos.ListaIntervaloBarrasDTO[0].IsBuscarCororonacion)
                ZSUperior_soloMAllaVertical_auto = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptofinal.Z;
            else
                ZSUperior_soloMAllaVertical_auto = -100000;

            _listaDebarra = igenerarIntervalos.ListaIbarraVertical;
        }

        protected void M1_AsignarCAntidadEspaciemientoNuevaLineaBarra(int i)
        {
            _confiWPFEnfierradoDTO.NuevaLineaCantidadbarra = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
            if (i == 0 && _muroSeleccionadoDTO.TipoElementoSeleccionado != ElementoSeleccionado.Barra)
            {
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza ||
                    _confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_BarraVHorquilla || _confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_Horquilla)
                    _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT;
                else
                {
                    if(_confiWPFEnfierradoDTO.DatosMallasDTO.tipoMallaV==TipoMAllaMuro.SM || _confiWPFEnfierradoDTO.DatosMallasDTO.tipoMallaH == TipoMAllaMuro.SM)
                        _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = Util.CmToFoot(_confiWPFEnfierradoDTO.IntervalosEspaciamiento[i]);
                    else
                        _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT_MALLA;
                }
            }
            else
                _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot += Util.CmToFoot(_confiWPFEnfierradoDTO.IntervalosEspaciamiento[i]);

            _confiWPFEnfierradoDTO.NumeroBarraLinea = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
        }

        public virtual void M3_1_ObtenerConfiguracionTAgBarraDTo()
        {
            double desfase = 1.5;
            double desfaseCodo = ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT;

            bool AUxIsDIrectriz = false;
            TagOrientation tagOrientationAUx = TagOrientation.Horizontal;

            if (TipoBarraVertical.Cabeza_BarraVHorquilla == _confiWPFEnfierradoDTO.TipoBarraRebar_)
            {
                AUxIsDIrectriz = true;
                tagOrientationAUx = TagOrientation.Vertical;
                desfaseCodo = -desfaseCodo * 2;// para bajar el codo


                if (_view.Scale == 75)
                    desfaseCodo = -ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT * 3;
                if (_view.Scale == 100)
                    desfaseCodo = -ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT * 3.5;

            }
            else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza)
            {
                AUxIsDIrectriz = true;
                tagOrientationAUx = TagOrientation.Vertical;
            }



            confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, desfase),
                IsDIrectriz = AUxIsDIrectriz,
                LeaderElbow = new XYZ(0, 0, desfaseCodo),
                tagOrientation = tagOrientationAUx,
                BarraTipo = TipoRebar.ELEV_BA_V
            };
        }
    
        protected  void M3_DibujarBarras(ConfiguracionTAgBarraDTo confBarraTag_)
        {
            //2) dibujar
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar barrasV-NH");

                    M3_DibujarBarras_sinTrasn(confBarraTag_);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
        }
           
        protected void M3_DibujarBarras_sinTrasn(ConfiguracionTAgBarraDTo confBarraTag_)
        {
            //2) dibujar
            try
            {

                _traslapoDTO = null;

                for (int i = 0; i < _listaDebarra.Count; i++)
                {
                    // agregra
                    ManejadorTraslapo.AgregarbarrasInicialTraslpoa();
                    //AgregarbarrasInicialTraslpoa();

                    IbarraBase item = _listaDebarra[i];

                    if (!item.M1_DibujarBarra()) continue;
                    if (_confiWPFEnfierradoDTO.IsDibujarTag) item.M2_DibujarTags(confBarraTag_);

                    IbarraBaseResultDTO _resutItem = item.GetResult();

                    Rebar _rebar = _resutItem._rebar;
                    if (_rebar != null)
                    {
                        _listaRebar.Add(_rebar);
                        ManejadorTraslapo.AgregarbarrasFinalTraslpoa(i, _resutItem._rebar);
                        //AgregarbarrasFinalTraslpoa(i, _rebar);
                        item.M1_1_DibujarBarraCOnfiguracion();
                    }
                    else
                        ManejadorTraslapo.Reset();
                }


            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
        }

      
           public void M4_DibujarBarrasCOnfiguracion()
           {
               //dibujar configuraciones
               try
               {
                   using (Transaction t = new Transaction(_doc))
                   {
                       t.Start("dibujar barrasV-NH");

                       M4_DibujarBarrasCOnfiguracion_sintras();

                       t.Commit();
                   }
               }
               catch (Exception ex)
               {
                   string msj = ex.Message;
               }
           }

           public void M4_DibujarBarrasCOnfiguracion_sintras()
           {
               //dibujar configuraciones
               try
               {
                   foreach (IbarraBase item in _listaDebarra)
                   {
                       item.M1_1_DibujarBarraCOnfiguracion();
                   }
               }
               catch (Exception ex)
               {
                   string msj = ex.Message;
               }
           }
           */
    }
}
