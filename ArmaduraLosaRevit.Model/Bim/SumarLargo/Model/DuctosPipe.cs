using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.SumarLargo.Model
{
   public class DuctosPipe
    {
        private  Pipe _pipe;
        public double Diamtro_mm { get; private set; }
        public double Largo_mt { get; private set; }
        public string NombreTipo { get; private set; }
        public DuctosPipe(Pipe _pipe)
        {
            this._pipe = _pipe;
        }

   

        internal bool ObtenerDatos()
        {
            try
            {
                Diamtro_mm =Math.Round( Util.FootToMm(_pipe.Diameter),0);
                Largo_mt = Math.Round(Util.FootToCm( ParameterUtil.FindParaByName(_pipe.Parameters, "Length").AsDouble())/100,2);
                NombreTipo = _pipe.Name;

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
