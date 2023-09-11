using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Geometria
{
    public class GeometriaS4
    {
        private UbicacionLosa direccion;
        private XYZ p1;
        private XYZ p2;
        private XYZ p3;
        private XYZ p4;
        private UbicacionLosa ubicacionEnlosa;
        private Transform Invertrans1;
        private Transform InverTrans2_rotacion;

        public List<XYZ> Lista { get; set; }
        public GeometriaS4(List<XYZ> lista, UbicacionLosa direccion)
        {
            Lista = lista;
            this.direccion = direccion;
        }

        public GeometriaS4(XYZ p1, XYZ p2, XYZ p3, XYZ p4, UbicacionLosa ubicacionEnlosa)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            Lista = new List<XYZ>();
            this.ubicacionEnlosa = ubicacionEnlosa;
        }

        public GeometriaS4(Transform invertrans1, Transform inverTrans2_rotacion, XYZ p1, XYZ p2, XYZ p3, XYZ p4, UbicacionLosa ubicacionEnlosa)
        {
            this.Invertrans1 = invertrans1;
            this.InverTrans2_rotacion = inverTrans2_rotacion;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            Lista = new List<XYZ>();
            this.ubicacionEnlosa = ubicacionEnlosa;
        }

        public bool Generar4Ptos()
        {

            try
            {
                switch (ubicacionEnlosa)
                {
                    case UbicacionLosa.Superior:
                    case UbicacionLosa.Derecha:
                        AjustarDereSup();
                        break;
                    case UbicacionLosa.Izquierda:
                    case UbicacionLosa.Inferior:
                        AjustarIzqInf();
                        break;
                    case UbicacionLosa.NONE:
                        break;
                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;

            }

            Lista.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p1)));
            Lista.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p2)));
            Lista.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p3)));
            Lista.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p4)));

            return true;
        }

        private void AjustarIzqInf()
        {
            p3 = new XYZ(p3.X, p2.Y, p3.Z);
            p4 = new XYZ(p4.X, p1.Y, p4.Z);
        }

        private void AjustarDereSup()
        {
            p2= new XYZ(p2.X, p3.Y, p2.Z);
            p1 = new XYZ(p1.X, p4.Y, p1.Z);
        }
    }
}
