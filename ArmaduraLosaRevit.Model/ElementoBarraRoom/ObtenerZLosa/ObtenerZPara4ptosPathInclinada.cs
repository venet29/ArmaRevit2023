using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.UTILES;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public class ObtenerZPara4ptosPathInclinada : IObtenerZPara4ptosPath
    {
        private View3D _elem3d;
        private readonly Document _doc;
        private readonly UIApplication _uiapp;
        private List<XYZ> _listaPtos;
        private List<XYZ> _listaPtosIniciales;
        private PlanarFace _face;
        private BuscarPtoProyeccionEnLosaInclinada _buscarPtoProyeccionEnLosaInclinada;

        public ObtenerZPara4ptosPathInclinada(UIApplication uiapp, List<XYZ> listaPtos, PlanarFace face)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;

            this._listaPtos = listaPtos;
            this._listaPtosIniciales = listaPtos.Select(c => new XYZ(c.X, c.Y, c.Z)).ToList();
            this._face = face;

            _buscarPtoProyeccionEnLosaInclinada = new BuscarPtoProyeccionEnLosaInclinada(uiapp, face.FaceNormal,face.Origin);
        }


        public List<XYZ> M1_Obtener4PtoConZCorrespondiente()
        {
            _elem3d = TiposFamilia3D.Get3DBuscar(_doc);

            if (_elem3d == null)
            {
                Util.ErrorMsg("Error: no se encontro 3D_NoEditar. Vuelva a cargar parametros inicales");
                return _listaPtos;
            }

            bool seguir = false;
            XYZ ptoAnalizado = XYZ.Zero;
            //obtener ptos proyectados en cara superior de la losa
            for (int i = 0; i < _listaPtos.Count; i++)
            {
                seguir = true;
                ptoAnalizado = _listaPtos[i];
                if (_buscarPtoProyeccionEnLosaInclinada.BuscarProyeccionEnPlane(ptoAnalizado))
                {
                    _listaPtos[i] = _buscarPtoProyeccionEnLosaInclinada.PtoProyectadoPlanoEnZ;

                }

                if (seguir == false) continue;
            }
            return _listaPtos;
        }


    }
}
