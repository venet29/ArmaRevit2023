using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Creador
{
    public class CreadorBarrasV_Horq:CreadorBarrasV
    {
       // private readonly RecalcularPtosYEspaciamieto_Horquilla _RecalcularPtosYEspaciamieto_Horqu;

        public CreadorBarrasV_Horq(UIApplication uiapp,
            SelecionarPtoSup selecionarPtoSup,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            :base( uiapp,             selecionarPtoSup,             confiEnfierradoDTO,             muroSeleccionadoDTO)
        
        {
         //   this._RecalcularPtosYEspaciamieto_Horqu = _RecalcularPtosYEspaciamieto_Horqu;
        }

        public override bool Ejecutar(int ii)
        {
            try
            {
                M1_AsignarCAntidadEspaciemientoNuevaLineaBarra(ii);

                //IGenerarIntervalosV igenerarIntervalos=   new GenerarIntervalos1Nivel_Cabeza_Horq(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                IGenerarIntervalosV igenerarIntervalos = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosV_mallas(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                M2_GenerarIntervalos(igenerarIntervalos);

                M3_1_ObtenerConfiguracionTAgBarraDTo();

                M3_DibujarBarras_horq(confBarraTag);

                M4_DibujarBarrasCOnfiguracion();              
            }
            catch (Exception ex)
            {
                Util.ErrorMsg( "Error al crear rebar verticaL CreadorBarrasV");
                return false;
            }

            return true;
        }

        public override void M3_1_ObtenerConfiguracionTAgBarraDTo()
        {
            confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                LeaderElbow = new XYZ(0, 0, -1.5),
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                tagOrientation = TagOrientation.Horizontal,
                RecalcularPtosYEspaciamieto_Horqu = _selecionarPtoSup._RecalcularPtosYEspaciamieto_HORQUILLA
            };
        }
        private void M3_DibujarBarras_horq(ConfiguracionTAgBarraDTo _confBarraTag)
        { 
            //2) dibujar
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar barrasV-NH");

                    foreach (IbarraBase item in _listaDebarra)
                    {
                        if (!item.M1_DibujarBarra()) continue;
                        if (_confiWPFEnfierradoDTO.IsDibujarTag) item.M2_DibujarTags(_confBarraTag);

                        IbarraBaseResultDTO _resutItem = item.GetResult();

                        Rebar _rebar = _resutItem._rebar;
                        if (_rebar == null) continue;

                        _listaRebar.Add(_rebar);

                        item.M1_1_DibujarBarraCOnfiguracion();

                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
        }

       

    }
}
