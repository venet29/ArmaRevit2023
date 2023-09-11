using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using System.Security.Authentication.ExtendedProtection;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public abstract class AObtenerZPara4ptosPath
    {
        protected View3D _elem3d;
        protected readonly UIApplication _uiapp;
        protected readonly Document _doc;
        protected List<XYZ> _listaPtos;
        protected List<XYZ> _listaPtosIniciales;
        protected Face _face;
        public Floor losaActual { get; set; }
        protected double _largoDeBUsquedaFoot;
        private double elevacion;
        private XYZ _midpoint;
        private XYZ _normal;
        private XYZ _minxyz;
        private XYZ _maXxyz;
        protected XYZ _direccionBUSquedaLosa;
        public AObtenerZPara4ptosPath(UIApplication uiapp, List<XYZ> listaPtos, Floor _losaActual,double rangoBusqedaCm)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._listaPtos = listaPtos;
            this.losaActual = _losaActual;
            this._listaPtosIniciales = listaPtos.Select(c => new XYZ(c.X, c.Y, c.Z)).ToList();
            this._largoDeBUsquedaFoot = Util.CmToFoot(rangoBusqedaCm);
        }
        public AObtenerZPara4ptosPath(Document doc, List<XYZ> listaPtos,Face face)
        {

            this._doc = doc;
            this._listaPtos = listaPtos;
            this._listaPtosIniciales = listaPtos.Select(c => new XYZ(c.X, c.Y, c.Z)).ToList();
            this._largoDeBUsquedaFoot = Util.CmToFoot(155);
            this._face = face;
            this._largoDeBUsquedaFoot = Util.CmToFoot(155);
            this.elevacion = listaPtos.Average(c=> c.Z);
            this._direccionBUSquedaLosa = new XYZ(0, 0, 1);
            buscarUbicacion();
        }

          public void buscarUbicacion()
          {

            BoundingBoxUV b = _face.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midparam = p + 0.5 * (q - p);
            _midpoint = _face.Evaluate(midparam);
            _normal = _face.ComputeNormal(midparam);
            _minxyz = _face.Evaluate(b.Min);
            _maXxyz = _face.Evaluate(b.Min);

            //si losa esta bajo nivel de los asociada ,cambiar direccion de busqueda
            if (elevacion > _midpoint.Z)
            { _direccionBUSquedaLosa *= -1; }
        
        }
        #region Nose utilza hasta el momento

        protected double BuscarLosaHaciaArriba(int i)
        {
            XYZ ptoMovidoi1cmExtLosa = ObtenerPtoAnalizado1cmExteriroRoom(i);
            return OBtenerRefrenciaLosaHorizontal(_elem3d, ptoMovidoi1cmExtLosa, new XYZ(0, 0, 1));

        }
        protected double BuscarLosaHaciaBajo(int i)
        {
            XYZ ptoMovidoi1cmExtLosa = ObtenerPtoAnalizado1cmExteriroRoom(i);
            return OBtenerRefrenciaLosaHorizontal(_elem3d, ptoMovidoi1cmExtLosa, new XYZ(0, 0, -1));

        }
        protected double OBtenerRefrenciaLosaHorizontal(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference ceilingRef = ref2.GetReference();
                    Floor floorIntersectada = _doc.GetElement(ceilingRef) as Floor;
                    if (!floorIntersectada.IsEstructural()) return 0;
                    if (floorIntersectada != null)
                    {                   
                        PlanarFace face_ = floorIntersectada.ObtenerPLanarFAce_superior();

                        if (!Util.IsVertical(face_.FaceNormal))
                        {
                            Debug.Write($"Cara de losa ID:{floorIntersectada.Id}  no es completamente horizontal");
                        }
                        return face_.Origin.Z;
                    }
                }
            }
            return 0;
        }


        #endregion
        protected Floor OBtenerRefrenciaLosa(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, bool dibujarMOdelLIne=false)
        {
            if (dibujarMOdelLIne)
            {
#if (DEBUG)
                CrearLIneaAux CrearLIneaAux = new CrearLIneaAux(_doc );
                CrearLIneaAux.CrearLinea(PtoCentralBordeRoom, PtoCentralBordeRoom+ VectorPerpenBordeRoom*5);
#endif
            }
            //CreadorCirculo.CrearCirculo_DetailLine(Util.CmToFoot(15), PtoCentralBordeRoom + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT, _uiapp.ActiveUIDocument);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
            ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference ceilingRef = ref2.GetReference();
                    Floor floorIntersectada = _doc.GetElement(ceilingRef) as Floor;

                    if (!floorIntersectada.IsEstructural()) return null;

                 //   if (losaActual != null && losaActual?.Id.IntegerValue== floorIntersectada.Id.IntegerValue)
                    

                    return floorIntersectada;
                }
            }

            return null;
        }


        //si no cuentra losa mueve pto 1 cm conrespecto a a u par
        // p1 --> 1 con haci p3
        // p2 --> 1 con haci p4      p1    p4
        // p3 --> 1 con haci p1      p2    p3
        // p4 --> 1 con haci p2
        protected XYZ M1_2_MoverPto1CmDiagonalInterRoom(XYZ ptoAnalizado, int i)
        {
            XYZ nuevoPtoDesplazado1cm = ptoAnalizado;
            switch (i)
            {
                case 0:
                    nuevoPtoDesplazado1cm = ptoAnalizado + (_listaPtos[2] - _listaPtos[0]).GetXY0().Normalize() * Util.CmToFoot(2* (i+1));
                    break;
                case 1:
                    nuevoPtoDesplazado1cm = ptoAnalizado + (_listaPtos[3] - _listaPtos[1]).GetXY0().Normalize() * Util.CmToFoot(2* (i + 1));
                    break;
                case 2:
                    nuevoPtoDesplazado1cm = ptoAnalizado + (_listaPtos[0] - _listaPtos[2]).GetXY0().Normalize() * Util.CmToFoot(2* (i + 1));
                    break;
                case 3:
                    nuevoPtoDesplazado1cm = ptoAnalizado + (_listaPtos[1] - _listaPtos[3]).GetXY0().Normalize() * Util.CmToFoot(2* (i + 1));
                    break;

                default:
                    break;
            }
            return nuevoPtoDesplazado1cm;
        }

        //PtoReferencia puede ser : 1,2,3,4
        //1       4
        //2       3
        protected XYZ ObtenerPtoAnalizado1cmExteriroRoom(int PtoReferencia)
        {   //                                   si es izq o inferior  pto medio entre 1y0                                             
            XYZ ptoAnalizado1cmExteriroRoom = (PtoReferencia==1?(_listaPtosIniciales[1]+ _listaPtosIniciales[0])/2 :
                                                                (_listaPtosIniciales[2] + _listaPtosIniciales[3]) / 2);
            switch (PtoReferencia)
            {
                case 0:
                case 1:
                    ptoAnalizado1cmExteriroRoom = ptoAnalizado1cmExteriroRoom + (_listaPtosIniciales[1] - _listaPtosIniciales[2]).Normalize() * Util.CmToFoot(1 + PtoReferencia);
                    break;
                case 2:
                case 3:
                    ptoAnalizado1cmExteriroRoom = ptoAnalizado1cmExteriroRoom + (_listaPtosIniciales[2] - _listaPtosIniciales[1]).Normalize() * Util.CmToFoot(1 + PtoReferencia);
                    break;

                default:
                    break;
            }
            return ptoAnalizado1cmExteriroRoom;
        }

    }
}
