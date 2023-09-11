using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarColumna
    {
        private UIApplication uIApplication;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private XYZ _rightDirection;

        public XYZ PuntoSobreFAceHost { get; private set; }
        public XYZ Largo { get; private set; }

        public Element _vigaElementHost { get; private set; }
        public double _espesorMuro { get; private set; }
        public XYZ _OrigenMuro { get; private set; }
        public XYZ _direccionMuro { get; private set; }

        public BuscarColumna(UIApplication uIApplication, double _largoDeBUsquedaFoot)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
             this._rightDirection = this._doc.ActiveView.RightDirection;
        }

        public void AsignarColumna(DatosMuroSeleccionadoDTO _muroSeleccionadoInicialDTO)
        {
            _vigaElementHost = _muroSeleccionadoInicialDTO.elementoContenedor;
            _espesorMuro = _muroSeleccionadoInicialDTO._EspesorMuroFoot;
            // no se aplica
            _OrigenMuro = XYZ.Zero;
            _direccionMuro = _muroSeleccionadoInicialDTO.DireccionMuro;
            PuntoSobreFAceHost = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
        }

        //no implementado
        public Element OBtenerRefrenciaColumna(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, bool IsdibujarRayoDebusqueda = false)
        {
            Util.ErrorMsg("Funcion no implementada correctamenet. Informar JIH");
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
       //     ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            ReferenceWithContext ref2 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                                 .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                                 .OrderBy(c => c.Proximity).FirstOrDefault();

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
                        _espesorMuro = ((FamilyInstance)_vigaElementHost).ObtenerEspesorVigaFoot();
                        _OrigenMuro = ((FamilyInstance)_vigaElementHost).ObtenerOrigin();
                        _direccionMuro = ((FamilyInstance)_vigaElementHost).ObtenerDireccionEnElSentidoView(_rightDirection);
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
