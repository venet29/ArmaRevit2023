
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

using ArmaduraLosaRevit.Model.BarraV.Intersecciones;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using System.Diagnostics;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class IntervaloBarrasHorqDTO: IntervaloBarrasDTO
    {
     
       public IntervaloBarrasHorqDTO(DatosMuroSeleccionadoDTO muroSeleccionadoDTO, ConfiguracionIniciaWPFlBarraVerticalDTO confWPFiEnfierradoDTO) : base(muroSeleccionadoDTO, confWPFiEnfierradoDTO)
        {
        }






        //para malla H manual
        public void M2_AsiganrCoordenadasH_reccorridoParaleloViewHorq(double ziniFoot, double zfinFoot, SelecionarPtoSup selecionarPtoSup)
        {
        
            //   double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2 + (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2f;

            this.tipobarraV = _confWPFiEnfierradoDTO.tipobarraH;// _tipoLineaMallaH;

            this.ptoini = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot)+
                                      _DatosElementSeleccionadoDTO.DireccionEnFierrado * (espaciamientoBordeFoot );

            this.ptofinal = selecionarPtoSup._PtoFinalIntervaloBarra_ProyectadoCaraMuroHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot)+
                                      _DatosElementSeleccionadoDTO.DireccionEnFierrado * (espaciamientoBordeFoot );
            //  this._nuevaLineaCantidadbarra = ((int)((zfinFoot - ziniFoot) / espaciamientoRecorridoBarrasFoot));

           // if (IsUltimoTramoCOnMouse) _nuevaLineaCantidadbarra += 1;
            this.DireccionRecorridoBarra = new XYZ(0, 0, 1);


            this.ptoPosicionTAg = (this.ptoini + this.ptofinal) / 2;


            //redondear
            RedonderA6Dig();

            _parametrosInternoRebarDTO._IsMalla = true;
            _parametrosInternoRebarDTO._cuantiaMalla = _confWPFiEnfierradoDTO.CuantiaMalla;
            _parametrosInternoRebarDTO._IdMalla = "Ma" + _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot).REdondearString_foot(3).Replace(" ", "").Replace(",", "&");
        }

   
    }
}
