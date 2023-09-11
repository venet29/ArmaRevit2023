using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Creation;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{
    public class SolicitudBarraDTO
    {

        public ExternalCommandData commandData { get;  set; }
        public bool IsBuscarTipoBarra { get;  set; }
        public TipoConfiguracionBarra TipoConfiguracionBarra { get;  set; }
        public string nombreDefamiliaBase { get;  set; }
        public Func<XYZ, bool> tipofaceBusqueda { get;  set; }
        public string TipoBarra { get;  set; }
        public UbicacionLosa UbicacionEnlosa { get;  set; }
        public UIDocument UIdoc { get;  set; }

        public TipoOrientacionBarra TipoOrientacion { get; set; }
        public bool IsCasoS4 { get; internal set; }
        public bool IsDirectriz { get; internal set; }
        public Ui_pathSymbolDTO Ui_pathSymbolDTO_ { get; internal set; }
        public TipoDireccion DireccionEditarDesplazamiento { get; internal set; } // se utiliza par indicar que lado del path se esta editando -- solo dos opciones   Izq y dere

        public SolicitudBarraDTO() {
            this.Ui_pathSymbolDTO_ = new Ui_pathSymbolDTO();
        }
        
        public SolicitudBarraDTO(UIApplication uiapp, string tipoBarra,
                                  UbicacionLosa ubicacionEnlosa_,  TipoConfiguracionBarra _tipoDeBarra, bool isBuscarTipoBarra = false) 
        {

            this.UIdoc = uiapp.ActiveUIDocument;
            this.TipoBarra = tipoBarra;
            this.UbicacionEnlosa = ubicacionEnlosa_;
            this.IsBuscarTipoBarra = isBuscarTipoBarra;
            this.TipoConfiguracionBarra = _tipoDeBarra;
            this.nombreDefamiliaBase = ConstNH.BASE_FAMILIA_PATHREINF_TAG;
            this.tipofaceBusqueda = Util.PointsUpwards;
            this.Ui_pathSymbolDTO_ = new Ui_pathSymbolDTO();
            //IsCasoS4 = ("s4_Inclinada" == tipoBarra ? true : false);
            ObtenerOrientacion();
        }
        public SolicitudBarraDTO(UIDocument UIdoc_, string tipoBarra,
                             UbicacionLosa ubicacionEnlosa_, TipoConfiguracionBarra _tipoDeBarra, bool isBuscarTipoBarra = false)
        {
            this.UIdoc = UIdoc_;
            this.TipoBarra = tipoBarra;
            this.UbicacionEnlosa = ubicacionEnlosa_;
            this.IsBuscarTipoBarra = isBuscarTipoBarra;
            this.TipoConfiguracionBarra = _tipoDeBarra;
            this.nombreDefamiliaBase = ConstNH.BASE_FAMILIA_PATHREINF_TAG;
            this.tipofaceBusqueda = Util.PointsUpwards;
            this.Ui_pathSymbolDTO_ = new Ui_pathSymbolDTO();
            ObtenerOrientacion();
        }
        public void ObtenerOrientacion()
        {
            TipoOrientacion=(UbicacionEnlosa == UbicacionLosa.Izquierda || UbicacionEnlosa == UbicacionLosa.Derecha ? TipoOrientacionBarra.Horizontal : TipoOrientacionBarra.Vertical);
        }
    }
}