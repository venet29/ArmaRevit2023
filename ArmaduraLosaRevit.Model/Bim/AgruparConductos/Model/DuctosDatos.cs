using Autodesk.Revit.DB.Electrical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AgruparConductos.Model
{
   public class DuctosDatos
    {
        private  Conduit _conduit;
        public double Diamtro_mm { get; private set; }
        public string NombreTipo { get; private set; }
        public DuctosDatos(Conduit _conduit)
        {
            this._conduit = _conduit;
        }

   

        internal bool ObtenerDatos()
        {
            try
            {
                Diamtro_mm = Util.FootToMm(_conduit.Diameter);
                NombreTipo = _conduit.Name;

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}
