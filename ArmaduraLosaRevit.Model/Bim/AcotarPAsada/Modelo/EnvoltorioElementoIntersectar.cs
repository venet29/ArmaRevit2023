using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.model;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{
    public class EnvoltorioElementoIntersectar
    {
        public XYZ ptoInterseccion { get; private set; }

        public string Nombre { get; private set; }
        public EnvoltorioGrid _EnvoltorioGrid { get; set; }
        public Line LineRuta { get;  set; }
        public double distanciaInterseccion_foot { get;  set; }
        public bool IsOk { get; set; }
        public EnvoltorioElementoIntersectar(EnvoltorioGrid c)
        {
            this.Nombre = c.Nombre;
            this._EnvoltorioGrid = c;
            IsOk = false;
        }

        public EnvoltorioElementoIntersectar(EnvoltorioGrid c, XYZ ptoInterseccion, double largo_foot) : this(c)
        {
            this.ptoInterseccion = ptoInterseccion;
            this.distanciaInterseccion_foot = largo_foot -Util.CmToFoot(1); // menos 1 cm para evitar lso casoso en que se intersectan bordes que esten contiguos
            this.IsOk=true;
        }

        internal bool Obtener()
        {
            try
            {
                if (_EnvoltorioGrid != null)
                {
                    LineRuta =(Line) _EnvoltorioGrid.Curva;
                }
                else
                {
                    Util.ErrorMsg("error: caso no programado");
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
