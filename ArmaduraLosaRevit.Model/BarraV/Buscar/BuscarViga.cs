using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarViga
    {
        private UIApplication uIApplication;
        private Document _doc;
        private View _view;
        private double _largoDeBUsquedaFoot;
        private XYZ _rightDirection;

        public XYZ PuntoSobreFAceHost { get; private set; }

        public XYZ CentroCaraNormalSaliendoVista { get; set; }
        public XYZ Largo { get; private set; }

        public Element _vigaElementHost { get; private set; }
        public double _espesorMuro { get; private set; }
        public XYZ _OrigenMuro { get; private set; }
        public XYZ _direccionMuro { get; private set; }

        public BuscarViga(UIApplication uIApplication, double _largoDeBUsquedaFoot)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._rightDirection = this._doc.ActiveView.RightDirection;
        }

        public bool AsignarViga(FamilyInstance _vigaElementHost, DatosMuroSeleccionadoDTO _muroSeleccionadoInicialDTO)
        {
            try
            {
                if (_vigaElementHost == null) return false;

                if (_vigaElementHost.StructuralType == StructuralType.Beam)
                {
                    this._vigaElementHost = _vigaElementHost;
                    _espesorMuro = _vigaElementHost.ObtenerEspesorConPtos_foot(_muroSeleccionadoInicialDTO.ptoSeleccionMouseCentroCaraMuro6, _view.ViewDirection); ;
                    _OrigenMuro = _vigaElementHost.ObtenerOrigin();
                    _direccionMuro = _vigaElementHost.ObtenerDireccionEnElSentidoView(_rightDirection);
                    PuntoSobreFAceHost = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
                   
                    (bool resulta, PlanarFace _planar) = _vigaElementHost.ObtenerCaraVerticalVIsible(_view);
                    CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                }
                else if (_vigaElementHost.StructuralType == StructuralType.Column)
                {
                    this._vigaElementHost = _vigaElementHost;
                    _espesorMuro = _vigaElementHost.ObtenerEspesorConPtos_foot(_muroSeleccionadoInicialDTO.ptoSeleccionMouseCentroCaraMuro6, _view.ViewDirection); ;
                    //_OrigenMuro = _vigaElementHost.ObtenerOrigin();
                    _direccionMuro = _rightDirection;// _vigaElementHost.ObtenerDireccionEnElSentidoView(_rightDirection);
                    PuntoSobreFAceHost = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

                    (bool resulta, PlanarFace _planar) = _vigaElementHost.ObtenerCaraVerticalVIsible(_view);
                    CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                }
                else
                {
                    Util.ErrorMsg($"FamilyInstance tipo {_vigaElementHost.StructuralType} no esta implementada. ");
                    return false;
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'M3_ObtenerPtoViga' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public Element OBtenerRefrenciaViga(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, bool IsdibujarRayoDebusqueda = false)
        {

            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);


            if (IsdibujarRayoDebusqueda)
            {
#if (DEBUG)
                CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                _crearLIneaAux.CrearLinea(PtoCentralBordeRoom, PtoCentralBordeRoom + VectorPerpenBordeRoom * Util.CmToFoot(50));
#endif
            }

            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference WallRef = ref2.GetReference();
                    PuntoSobreFAceHost = WallRef.GlobalPoint;
                    _vigaElementHost = _doc.GetElement(WallRef);

                    if (_vigaElementHost is FamilyInstance)
                    {

                        //---muro buscado hacia el centro de muro
                        _espesorMuro = ((FamilyInstance)_vigaElementHost).ObtenerEspesorConPtos_foot(PtoCentralBordeRoom, VectorPerpenBordeRoom);
                        _OrigenMuro = ((FamilyInstance)_vigaElementHost).ObtenerOrigin();
                        _direccionMuro = ((FamilyInstance)_vigaElementHost).ObtenerDireccionEnElSentidoView(_rightDirection);

                        (bool resulta, PlanarFace _planar) = _vigaElementHost.ObtenerCaraVerticalVIsible(_view);
                        CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);

                        return _vigaElementHost;
                    }
                    else
                        return null;
                }
            }

            return null;
        }


        public BuscarMurosDTO GetBuscarMurosDTO(XYZ _ptobuscarWall_inferior, XYZ _ptobuscarWall_superior)
        {
            return new BuscarMurosDTO()
            {
                _idElement = _vigaElementHost.Id.IntegerValue,
                _direccionMuro = _direccionMuro,
                _espesorMuro = _espesorMuro,
                _ptobuscarWall_inferior = _ptobuscarWall_inferior,
                _ptobuscarWall_superior = _ptobuscarWall_superior
            };
        }
    }
}
