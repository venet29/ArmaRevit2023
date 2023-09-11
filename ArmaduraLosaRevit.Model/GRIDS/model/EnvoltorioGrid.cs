using ArmaduraLosaRevit.Model.GEOM;
using ArmaduraLosaRevit.Model.GRIDS.AgregarEje.Servicios;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.GRIDS.model
{
    public class EnvoltorioGrid
    {
        public string Nombre { get; set; }
        public bool IsOK { get; private set; }
        public Grid Grid_ { get; set; }
        public Curve Curva { get; set; }
        public XYZ p1 { get; set; }
        public XYZ p2 { get; set; }

        public XYZ MaximumOint { get; set; }
        public XYZ MinimumOint { get; set; }
        public Reference Referencia_ { get; private set; }
        public List<GridIntersectado> ListaGridIntersectado { get;  set; }

        public EnvoltorioGrid(Grid item)
        {
            IsOK = true;
            this.Grid_ = item;
            this.Nombre = item.Name;
            this.Curva = item.Curve;
            ListaGridIntersectado = new List<GridIntersectado>();
            if (Curva == null) IsOK = false;

            p1 = Curva.GetEndPoint(0);
            p2 = Curva.GetEndPoint(1);

            var getOutline = item.GetExtents();
            if (getOutline == null) return;
            MaximumOint = getOutline.MaximumPoint;
            MinimumOint = getOutline.MinimumPoint;

            Referencia_ = Curva.Reference;
        }
        public bool ObtenerReferencIA()
        {
            try
            {

                GemetrieLine _GemetrieLIne = new GemetrieLine(null, Grid_);
                if (_GemetrieLIne.ObtenerLine())
                {
                    Referencia_ = _GemetrieLIne.ListaResult[0];
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return true;
        }




    }
}
