using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GEOM;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public class BuscarFaceSuperiorConPto : GeometriaBase
    {
        private readonly RebarInferiorDTO _RebarInferiorDTO;

        // private readonly UIApplication _uiapp;
        private readonly Element _losa;
        private double _diametroBarraFoot;
        private readonly XYZ direccionBarra;
        private List<PlanarFace> _listaCaraSup;
        private BuscarPtoProyeccionEnLosaInclinada _buscarPtoProyeccionEnLosaInclinada;

        public XYZ PtoEnFaceSuperior { get; private set; }
        public PlanarFace CaraSup { get; set; }
        public XYZ DireccionBarraSuperior { get; set; }
        public double _direccionParalelo { get; set; }


        public double EspesorEnPtoFaceSuperiorConRecub_foot { get;  set; }
        public double EspesorEnPtoFaceSuperiorSinRecub_foot { get; set; }
        public XYZ EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam { get; private set; }

        public BuscarFaceSuperiorConPto(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO,  XYZ direccionBarra) : base(uiapp)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            //this._uiapp = uiapp;
            this._losa = _RebarInferiorDTO.floor;
            this._diametroBarraFoot = _RebarInferiorDTO.diametroFoot;
            this.direccionBarra = direccionBarra;
        }


        public bool ObtenerDirecionDeLosa(XYZ pto, PosicionDeBusqueda posicionDeBusqueda)
        {
            M1_AsignarGeometriaObjecto(_losa);

            if (!M2_ObtenerCaraSuperior(pto)) return false;

            M2_1_EspesorDeLosaEnpto();

            M3_ObtenerDireccion(posicionDeBusqueda);

            return true;
        }

        private void M2_1_EspesorDeLosaEnpto()
        {
            EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam = CaraSup.FaceNormal * (EspesorEnPtoFaceSuperiorConRecub_foot
                                                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm)
                                                                        - Util.CmToFoot( ConstNH.RECUBRIMIENTO_LOSA_INF_cm)- _diametroBarraFoot);
            EspesorEnPtoFaceSuperiorSinRecub_foot = EspesorEnPtoFaceSuperiorConRecub_foot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm)
                                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);
        }

        private bool M2_ObtenerCaraSuperior(XYZ pto)
        {

            _listaCaraSup = listaPlanarFace.Where(c => c.FaceNormal.Z > 0).ToList();

            if (_listaCaraSup.Count == 0)
            {
                Util.ErrorMsg("No se encontro cara superior en losa. Error:154811609");
                return false;
            }

             EspesorEnPtoFaceSuperiorConRecub_foot = 10000;
            foreach (PlanarFace pface in _listaCaraSup)
            {
                _buscarPtoProyeccionEnLosaInclinada =new BuscarPtoProyeccionEnLosaInclinada(_uiapp, pface.FaceNormal,pface.Origin);

                if (pface.Project(pto) == null) continue;

                if (!_buscarPtoProyeccionEnLosaInclinada.BuscarProyeccionEnPlane(pto)) continue;

                double espesorAuxiliarEnPto = _buscarPtoProyeccionEnLosaInclinada.PtoProyectadoPlanoEnZ.DistanceTo(pto);

                if (espesorAuxiliarEnPto < EspesorEnPtoFaceSuperiorConRecub_foot)
                {
                    EspesorEnPtoFaceSuperiorConRecub_foot = espesorAuxiliarEnPto;
                    PtoEnFaceSuperior = _buscarPtoProyeccionEnLosaInclinada.PtoProyectadoPlanoEnZ;
                    CaraSup = pface;
                }
            }
            //CaraSup = _listaCaraSup.MinBy(c => c.Project(pto).Distance);
            return (CaraSup==null?false:true);
        }
        private void M3_ObtenerDireccion(PosicionDeBusqueda posicionDeBusqueda)
        {
            _direccionParalelo = Util.GetProductoEscalar(direccionBarra, CaraSup.XVector);

            if (Math.Abs(Math.Abs(_direccionParalelo) -1)< 0.001)
            {
                M3_1_ObtenerSignoDeLaIDireccion(posicionDeBusqueda, CaraSup.XVector);
            }
            else
            {
                _direccionParalelo = Util.GetProductoEscalar(direccionBarra, CaraSup.YVector);
                M3_1_ObtenerSignoDeLaIDireccion(posicionDeBusqueda, CaraSup.YVector); 
            }
        }

        private void M3_1_ObtenerSignoDeLaIDireccion(PosicionDeBusqueda posicionDeBusqueda, XYZ vectorBarraSuperior)
        {
            if (posicionDeBusqueda == PosicionDeBusqueda.Inicio && _direccionParalelo > 0)
                DireccionBarraSuperior = vectorBarraSuperior;
            else if (posicionDeBusqueda == PosicionDeBusqueda.Inicio && _direccionParalelo < 0)
                DireccionBarraSuperior = -vectorBarraSuperior;
            else if (posicionDeBusqueda == PosicionDeBusqueda.Fin && _direccionParalelo > 0)
                DireccionBarraSuperior = -vectorBarraSuperior;
            else if (posicionDeBusqueda == PosicionDeBusqueda.Fin && _direccionParalelo < 0)
                DireccionBarraSuperior = vectorBarraSuperior;
        }
    }
}
