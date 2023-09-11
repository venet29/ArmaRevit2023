using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath
{


    // p1 -- p4
    // p2 -- p3
    public class CoordenadaPath
    {
        public TipoCaraObjeto ubicacionCara { get; set; }
        public XYZ p1 { get; set; }
        public XYZ p2 { get; set; }
        public XYZ p3 { get; set; }
        public XYZ p4 { get; set; }
        public XYZ centro { get; set; }
        public bool IsPtoOK { get; set; }
        public double LargosPath { get; private set; }
        public double LargosRecorrio { get; private set; }

        public CoordenadaPath(XYZ p1, XYZ p2, XYZ p3, XYZ p4, TipoCaraObjeto  ubicacionCara = TipoCaraObjeto.Inferior)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.IsPtoOK = false;
            this.centro = XYZ.Zero;
            this.ubicacionCara = ubicacionCara;
        }

        public CoordenadaPath()
        {
        }

        public void CalcularCentroPath()
        {
            this.centro = (p1 + p2 + p3 + p4) / 4;
            this.IsPtoOK = true;
        }

        public List<XYZ> GetListaXYZ()
        {
            List<XYZ> list = new List<XYZ>();
            list.Add(p1);
            list.Add(p2);
            list.Add(p3);
            list.Add(p4);
            IsPtoOK = true;
            return list;
        }

        public XYZ Obtenerdirecion1_to_4() => (p4 - p1).Normalize();

        public void ObtenerLargos()
        {
            LargosPath = p2.DistanceTo(p3);
            LargosRecorrio = p2.DistanceTo(p1);
        }
    }


}
