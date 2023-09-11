
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public class BuscarPtoProyeccionEnLosaInclinada
    {
        private readonly UIApplication uiapp;
        private Document _doc;
        private Plane _plano;
        public XYZ PtoProyectadoPlanoEnZ { get; set; }


        public double DistanciaHaciaPLanoEnZ { get; private set; }

        public BuscarPtoProyeccionEnLosaInclinada(UIApplication uiapp,XYZ  FaceNormal, XYZ Origin)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
   
            _plano = Plane.CreateByNormalAndOrigin(FaceNormal.Redondear8(), Origin);
        }


        public bool BuscarProyeccionEnPlane(XYZ ptProyectado)
        {
            DistanciaHaciaPLanoEnZ = 0;
            try
            {
                //opcion 1
                XYZ PtoProyectadoPerpendicularPlano = _plano.ProjectOnto(ptProyectado);
                double DistanciaD = PtoProyectadoPerpendicularPlano.DistanceTo(ptProyectado);

                //opcion2
                UV resultuv = new UV();
                double distanceResult = 0;
                _plano.Project(ptProyectado, out resultuv, out distanceResult);

                if (DistanciaD < 0.001)
                { PtoProyectadoPlanoEnZ = ptProyectado; }
                else
                {
                    double anguloz = Math.PI / 2 - Math.Abs((PtoProyectadoPerpendicularPlano - ptProyectado).GetAngleEnZ_respectoPlanoXY());
                    DistanciaHaciaPLanoEnZ = DistanciaD / Math.Cos(anguloz) * (ptProyectado.Z > PtoProyectadoPerpendicularPlano.Z ? -1 : 1);

                    PtoProyectadoPlanoEnZ = ptProyectado + new XYZ(0, 0, DistanciaHaciaPLanoEnZ);
                  //  CrearModeLineAyuda.modelarlineas(_doc, ptProyectado, PtoProyectadoPerpendicularPlano);
             
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al buscar punto proyectado en losa ex:{ex.Message}. Error:14201254");
                return false;
            }
            return true;
        }
    }
}
