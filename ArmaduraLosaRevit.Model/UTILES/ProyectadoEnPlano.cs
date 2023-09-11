using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class ProyectadoEnPlano
    {
        public static XYZ ObtenerPtoProyectadoEnPlano_conRedondeo8(XYZ normalPlano, XYZ ptoReferencia, XYZ ptoBuscar)
            => ObtenerPtoProyectadoEnPlano(normalPlano, ptoReferencia, ptoBuscar, 8);


        public static XYZ ObtenerPtoProyectadoEnPlano(XYZ normalPlano, XYZ ptoReferencia, XYZ ptoBuscar, int redondeo = -1)
        {
            XYZ ptoProyectado = XYZ.Zero;
            try
            {
                Plane plano = default;
                if (redondeo == -1)
                    plano= Plane.CreateByNormalAndOrigin(normalPlano, ptoReferencia);
                else
                    plano= Plane.CreateByNormalAndOrigin(normalPlano.Redondear(redondeo), ptoReferencia);

                ptoProyectado = plano.ProjectOnto(ptoBuscar);
            }
            catch (Exception)
            {
                return XYZ.Zero;
            }
            return ptoProyectado;

        }
    }
}
