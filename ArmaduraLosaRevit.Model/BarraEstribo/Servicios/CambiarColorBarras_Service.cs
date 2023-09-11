using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
   public class CambiarColorBarras_Service
    {
        private readonly UIApplication _uiapp;
        private readonly DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;
        private TipoConfiguracionEstribo _tipoConfiguracionBarra;

        public CambiarColorBarras_Service(UIApplication uiapp, DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO)
        {
            this._uiapp = uiapp;
            this._configuracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            this._tipoConfiguracionBarra = _configuracionInicialEstriboDTO.tipoConfiguracionEstribo;
        }
        public CambiarColorBarras_Service(UIApplication uiapp, TipoConfiguracionEstribo _tipoConfiguracionBarra)
        {
            this._uiapp = uiapp;      
            this._tipoConfiguracionBarra = _tipoConfiguracionBarra;
        }
        public CambiarColorBarras_Service(UIApplication uiapp)
        {
            this._uiapp = uiapp;

        }
        public void M1_2_CAmbiarColor(List<ElementId> _listaRebarIdCambiarColor,bool _Halftone = true)
        {
            if (_listaRebarIdCambiarColor == null) return;
            if (_listaRebarIdCambiarColor.Count == 0) return;

            Color newcolor = FactoryColores.ObtenerColorEstribo(_tipoConfiguracionBarra);

     
            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarIdCambiarColor, newcolor, _Halftone);
            //visibilidadElement
        }

        public void M1_2_CAmbiarColor_sintrans(List<ElementId> _listaRebarIdCambiarColor, bool _Halftone = true)
        {
            if (_listaRebarIdCambiarColor == null) return;
            if (_listaRebarIdCambiarColor.Count == 0) return;

         //   Color newcolor = new Color((byte)254, (byte)254, (byte)254);
            Color newcolor  = FactoryColores.ObtenerColorEstribo(_tipoConfiguracionBarra);

            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListElementsColorSinTrans(_listaRebarIdCambiarColor, newcolor, _Halftone);
            //visibilidadElement
        }



        public void M1_3_CAmbiarColorPorColor_sintrans(List<ElementId> _listaRebarIdCambiarColor, Color newcolor, bool _Halftone = false)
        {
            if (_listaRebarIdCambiarColor == null) return;
            if (_listaRebarIdCambiarColor.Count == 0) return;

            if (newcolor == null) return;
            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListElementsColorSinTrans(_listaRebarIdCambiarColor, newcolor, _Halftone);
            //visibilidadElement
        }
    }
}
