using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.UTILES;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public class ObtenerZPara4ptosRuledFace : AObtenerZPara4ptosPath, IObtenerZPara4ptosPath
    {
    
        private readonly RuledFace _ruledFacece;

        public ObtenerZPara4ptosRuledFace(UIApplication uiapp, List<XYZ> listaPtos, RuledFace face)
            :base(uiapp, listaPtos, null,Util.CmToFoot(150))
        {
     
            this._ruledFacece = face;
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
            
                //ptoAnalizado = M1_2_MoverPto1CmDiagonalInterRoom(ptoAnalizado, i);
              //  CreadorCirculo.CrearCirculo_DetailLine(Util.CmToFoot(15), ptoAnalizado + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT, uiapp.ActiveUIDocument);
                BoundingBoxUV b = _ruledFacece.GetBoundingBox();
                UV p = b.Min;
                UV q = b.Max;
                UV midparam = p + 0.5 * (q - p);
                XYZ midpoint = _ruledFacece.Evaluate(midparam);
                XYZ normal = _ruledFacece.ComputeNormal(midparam);

              //  XYZ ptoInter=ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(normal, midpoint, ptoAnalizado + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT);
              //  IntersectionResult resul = this._ruledFacece.Project(ptoAnalizado + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT);

               var _buscarPtoProyeccionEnLosaInclinada = new BuscarPtoProyeccionEnLosaInclinada(_uiapp, normal, midpoint);

                // if (resul != null)
                if (_buscarPtoProyeccionEnLosaInclinada.BuscarProyeccionEnPlane(ptoAnalizado))
                {
                    _listaPtos[i] = _buscarPtoProyeccionEnLosaInclinada.PtoProyectadoPlanoEnZ;//  new XYZ(_listaPtos[i].X, _listaPtos[i].Y, ptoInter.Z);

                  
                    return false;
                }
            }
            return true;
        }
    }
}
