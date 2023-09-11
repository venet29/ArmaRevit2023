using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public class ObtenerZPara4ptosPathHorizontal : AObtenerZPara4ptosPath, IObtenerZPara4ptosPath
    {


        public ObtenerZPara4ptosPathHorizontal(Document doc, List<XYZ> listaPtos, Face face):base(doc,listaPtos,face)
        {


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

                seguir = M1_1_Iterar(ptoAnalizado, i);

                if (seguir == false) continue;

            }
            return _listaPtos;
        }

        private bool M1_1_Iterar(XYZ ptoAnalizado, int i)
        {


            for (int k = 0; k < 10; k++)
            {
                ptoAnalizado = M1_2_MoverPto1CmDiagonalInterRoom(ptoAnalizado, i);
                XYZ resul = _face.ProjectNH(ptoAnalizado + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT);

                if (resul.IsDistintoLargoCero())
                {
                    _listaPtos[i] = new XYZ(_listaPtos[i].X, _listaPtos[i].Y, resul.Z);
                    return false;
                }
            }
            return true;
        }


    }
}
