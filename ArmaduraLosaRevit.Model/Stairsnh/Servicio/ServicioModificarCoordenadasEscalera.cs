using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Stairsnh.Servicio
{
    public class ServicioModificarCoordenadasEscalera
    {
        private Transform trans1 = null;
        private Transform Invertrans1 = null;
        private Transform trans2_rotacion = null;
        private Transform InverTrans2_rotacion = null;
        private double _anguloGrado;

        private readonly double _desplazamientoFoot;
        public PlanarFace _planarFaceMaxArea { get; set; }
        public XYZ pto1 { get; set; }
        public XYZ pto2 { get; set; }
        public XYZ pto3 { get; set; }
        public XYZ pto4 { get; set; }

        public XYZ pto1_transf { get; set; }
        public XYZ pto2_transf { get; set; }
        public XYZ pto3_transf { get; set; }
        public XYZ pto4_transf { get; set; }

        public List<XYZ> lista4ptos { get; set; }
        public List<XYZ> lista4ptosTrasversal { get; set; }

        public ServicioModificarCoordenadasEscalera(XYZ pto1, XYZ pto2, XYZ pto3, XYZ pto4)
        {
            this.pto1 = pto1;
            this.pto2 = pto2;
            this.pto3 = pto3;
            this.pto4 = pto4;
            lista4ptos = new List<XYZ>();
            lista4ptosTrasversal = new List<XYZ>();
        }

        public ServicioModificarCoordenadasEscalera(PlanarFace planarFaceMaxArea, double desplzamientofoot)
        {
            this._planarFaceMaxArea = planarFaceMaxArea;
            this._desplazamientoFoot = desplzamientofoot;
            lista4ptos = new List<XYZ>();
            lista4ptosTrasversal = new List<XYZ>();
        }

        //obtiinene p1,p2,p3,p4 desde coordenadas uv del plano
        public void M1_Obtener4ptosPrincipales()
        {
            if (_planarFaceMaxArea == null) return;

            double xmin = _planarFaceMaxArea.GetBoundingBox().Min.U;
            double ymin = _planarFaceMaxArea.GetBoundingBox().Min.V;
            double xmax = _planarFaceMaxArea.GetBoundingBox().Max.U;
            double ymax = _planarFaceMaxArea.GetBoundingBox().Max.V;

            XYZ minPto = _planarFaceMaxArea.Evaluate(_planarFaceMaxArea.GetBoundingBox().Min);
            XYZ maxPto = _planarFaceMaxArea.Evaluate(_planarFaceMaxArea.GetBoundingBox().Max);

            this.pto1 = _planarFaceMaxArea.Evaluate(new UV(xmin, ymax));
            this.pto2 = _planarFaceMaxArea.Evaluate(new UV(xmin, ymin));
            this.pto3 = _planarFaceMaxArea.Evaluate(new UV(xmax, ymin));
            this.pto4 = _planarFaceMaxArea.Evaluate(new UV(xmax, ymax));

           
        }



        public XYZ M2_ObtenerPtoReferenciaLines(XYZ vectorDireccion, XYZ Origen)
        {
            _anguloGrado = Util.GetAnguloVectoresEnGrados_enPlanoXY(vectorDireccion);
            XYZ result = null;

            trans1 = Transform.CreateTranslation(-Origen);
            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(_anguloGrado), XYZ.Zero);

            Invertrans1 = trans1.Inverse;
            InverTrans2_rotacion = trans2_rotacion.Inverse;
            //trans1.Origin = listaPtos[3];

            pto1_transf = trans2_rotacion.OfPoint(trans1.OfPoint(pto1));
            pto2_transf = trans2_rotacion.OfPoint(trans1.OfPoint(pto2));
            pto3_transf = trans2_rotacion.OfPoint(trans1.OfPoint(pto3));
            pto4_transf = trans2_rotacion.OfPoint(trans1.OfPoint(pto4));


            M2_1_OrdenarPtosTransform(pto1_transf, pto2_transf,pto3_transf,pto4_transf);


            pto1 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto1_transf));
            pto2 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto2_transf));
            pto3 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto3_transf));
            pto4 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto4_transf));

            lista4ptos.Add(pto1.AsignarZ(Math.Max(pto1.Z,pto2.Z))+ new XYZ(0,0, _desplazamientoFoot)); lista4ptos.Add(pto2.AsignarZ(Math.Max(pto1.Z, pto2.Z)) + new XYZ(0, 0, _desplazamientoFoot));
            lista4ptos.Add(pto3.AsignarZ(Math.Min(pto3.Z, pto4.Z)) + new XYZ(0, 0, _desplazamientoFoot)); lista4ptos.Add(pto4.AsignarZ(Math.Min(pto3.Z, pto4.Z)) + new XYZ(0, 0, _desplazamientoFoot));


            lista4ptosTrasversal.Add(lista4ptos[1]); lista4ptosTrasversal.Add(lista4ptos[2]);
            lista4ptosTrasversal.Add(lista4ptos[3]); lista4ptosTrasversal.Add(lista4ptos[0]);
            return result;
        }

        /// <summary>
        /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
        /// p1 -- p4
        ///  |     |
        /// p2 -- p3
        /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha
        /// </summary>
        /// <param name="p1"> pto de linea1</param>
        /// <param name="p2"> pto de linea1 </param>
        /// <param name="p3"> pto de linea2 </param>
        /// <param name="p4"> pto de linea2 </param>
        public  bool M2_1_OrdenarPtosTransform( XYZ p1,  XYZ p2,  XYZ p3,  XYZ p4)
        {
           
            // si los pt1 y pt2 esta en lalinea de la derecha
            if (p4.X < p1.X)
            {
                XYZ aux_p4 = p4;
                XYZ aux_p3 = p3;
                p4 = p1;
                p3 = p2;
                p2 = aux_p3;
                p1 = aux_p4;
            }



            //priemra linea
            if (p2.Y > p1.Y)
            {
                XYZ aux_p1 = p1;
                p1 = p2;
                p2 = aux_p1;
               
            }
            else if (p2.Y > p1.Y)
            {
                Util.ErrorMsg( " P1 y p2, son Igueles ");
                return true;
            }



            //segunda linea
            if (p3.Y > p4.Y)
            {
                XYZ aux_p4 = p4;
                p4 = p3;
                p3 = aux_p4;
    
            }
            else if (p3.Y > p4.Y)
            {
                Util.ErrorMsg(" P1 y p2, son Igueles ");
                return true;
            }

            pto1_transf = p1;
            pto2_transf = p2;
            pto3_transf = p3;
            pto4_transf = p4;

            return true;
        }

    }
}
