using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Model
{
    public class DatosHost
    {
        private  UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private RebarDesglose rebarDesglose;
        private XYZ aux_ptoMedio;
        private XYZ aux_direccion;

        public PlanarFace CaraCentral { get;  set; }
        public XYZ CentroHost { get;  set; }
        public double LargoMAximoHost_foot { get;  set; }
        public XYZ Direccion_ParalelaView { get; internal set; }

        public DatosHost(UIApplication _uiapp, RebarDesglose rebarDesglose)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.rebarDesglose = rebarDesglose;
        }

        public bool ObtenerPtoMedioYDireccion()
        {
            try
            {
                WraperRebarLargo curvaPrinciplar = rebarDesglose.ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
                aux_ptoMedio = curvaPrinciplar.ptoMedio;
                aux_direccion = rebarDesglose.CurvaMasLargo.direccion;
                   //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final
      
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }
        public bool ObtenerPtoMedio_conestribo()
        {
            try
            {
                WraperRebarLargo curvaPrinciplar = rebarDesglose.ListaCurvaBarras.Find(c => c.IsBarraPrincipal);
               // aux_ptoMedio = curvaPrinciplar._Trasform.EjecutarTransformInvertida(curvaPrinciplar.ptoMedio);

                aux_direccion = rebarDesglose._normal;
                //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final

            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }

        public bool ObtenerCentroPilarOmUro()
        {
            try
            {
                //WraperRebarLargo curvaPrinciplar =rebarDesglose.ListaCurvaBarras.Find(c=>c.IsBarraPrincipal);
                Element host = _doc.GetElement(rebarDesglose._rebar.GetHostId());

                //XYZ _ptoMedio=rebarDesglose.trasform.EjecutarTransformInvertida(curvaPrinciplar.ptoMedio);
                //en pilares carainferior: caraabajo  //  en vigas cara vertical inicial izquioerda o derecha final
                CaraCentral = host.ObtenerCaraSegun_IgualDireccion_MasLejano( aux_direccion);
               XYZ _aux_p1 = CaraCentral.ObtenerCenterDeCara();


                var Cara2 = host.ObtenerCaraSegun_IgualDireccion_MasLejano(-aux_direccion);
                XYZ _aux_pt2= ((Face)Cara2).ProjectNH(_aux_p1);

          

                if (CaraCentral == null)
                {
                    UtilDesglose.ErrorMsg($"No se pudo obtenerDatos de cara de muro ");
                    return false;
                }
                CentroHost = CaraCentral.ObtenerCenterDeCara();
                LargoMAximoHost_foot = CaraCentral.MaximoladoLArgo();

                var listacurva = CaraCentral.ObtenerListaCurvas();

                 var aux_Direccion_ParalelaView =  CaraCentral.ObtenerListaCurvas().Where(c => !UtilDesglose.IsParallel(((Line)c).Direction, _view.ViewDirection)).FirstOrDefault();
                Direccion_ParalelaView = ((Line)aux_Direccion_ParalelaView).Direction;
            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener datos de host de barras  ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }

     
    }
}
