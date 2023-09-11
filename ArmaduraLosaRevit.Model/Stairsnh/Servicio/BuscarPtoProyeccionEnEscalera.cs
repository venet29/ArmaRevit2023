using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Stairsnh.Servicio
{
    public class BuscarPtoProyeccionEnEscalera
    {
        private readonly UIApplication uiapp;
        private Document _doc;
        private readonly PlanarFace faceInferior;
        public XYZ PtoProyectadoCaraInferior { get; set; }

        public XYZ PtoProyectadoCaraSuperior { get; set; }

        public double DistanciaHorizontalCaraSuperior { get; private set; }
        public double DistanciaHorizontalCaraInferior { get; private set; }

        public BuscarPtoProyeccionEnEscalera(UIApplication uiapp, PlanarFace face)
        {

            this.uiapp = uiapp;
            this._doc= uiapp.ActiveUIDocument.Document;
            this.faceInferior = face;
            if (this.faceInferior == null)
            {

            }
        }

        public bool BuscarProyeccionEnCaraInferior(XYZ ptProyectado, double DeltaZ)
        {
            DistanciaHorizontalCaraInferior = 0;
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(faceInferior.FaceNormal.Redondear8(), faceInferior.Origin.AsignarZ(faceInferior.Origin.Z + DeltaZ));
                PtoProyectadoCaraInferior = plano.ProjectOnto(ptProyectado);

                double distanciaD = PtoProyectadoCaraInferior.DistanceTo(ptProyectado);
                double anguloz = (PtoProyectadoCaraInferior - ptProyectado).GetAngleEnZ_respectoPlanoXY();
                DistanciaHorizontalCaraInferior = distanciaD / Math.Cos(anguloz);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptProyectado"></param>
        /// <param name="DeltaZ"> lo que se desplaza la cara inferior hacia arriba</param>
        /// <returns></returns>
        public bool BuscarProyeccionEnCaraInSuperior(XYZ ptProyectado, double DeltaZ)
        {
            DistanciaHorizontalCaraSuperior = 0;
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(faceInferior.FaceNormal.Redondear8(), faceInferior.Origin.AsignarZ(faceInferior.Origin.Z + DeltaZ));
                PtoProyectadoCaraSuperior = plano.ProjectOnto(ptProyectado);

                double distanciaD = PtoProyectadoCaraSuperior.DistanceTo(ptProyectado);
                double anguloz = (PtoProyectadoCaraSuperior - ptProyectado).GetAngleEnZ_respectoPlanoXY();
                DistanciaHorizontalCaraSuperior = distanciaD/Math.Cos(anguloz);
               

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
