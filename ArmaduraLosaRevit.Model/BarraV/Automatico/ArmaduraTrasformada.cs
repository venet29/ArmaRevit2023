using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico
{
    public class ArmaduraTrasformada
    {
        Transform trans1 = null;
        Transform trans2_rotacion = null;

        Transform Invertrans1 = null;
        Transform InverTrans2_rotacion = null;
        public string _nombre { get; set; }
        public XYZ _origenSeccionView { get; set; }//punto mas al derecha, abajo y mas cerca de pantalla a la  vista
        public XYZ _RightDirection { get; set; }//direccion paralalea a la pantalla (izq hacia derecha)
        public XYZ _ViewDirectionSaliendo { get; set; }//direccion perpendicular a la pantalla (saliendo de la pantalla)

        private double _anguloGradosVIew;
        private readonly double deltaelevacionBasePoint;

        public bool Isvalid { get; set; }



        public ArmaduraTrasformada(View view, double deltaElevacionBasePoint = 0)
        {
            this._nombre = view.Name;
            this._origenSeccionView = view.Origin;
            this._RightDirection = view.RightDirection;
            this._ViewDirectionSaliendo = view.ViewDirection;
            this._anguloGradosVIew = Util.AnguloEntre2PtosGrados_enPlanoXY(XYZ.Zero, _RightDirection);
            this._anguloGradosVIew = (Util.IsSimilarValor(_anguloGradosVIew, 0) ? 0 : _anguloGradosVIew);
            Isvalid = ObtenerTransformados();
            this.deltaelevacionBasePoint = deltaElevacionBasePoint;
        }



        private bool ObtenerTransformados()
        {
            try
            {
                trans1 = Transform.CreateTranslation(-_origenSeccionView.GetXY0());
                trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(_anguloGradosVIew), XYZ.Zero);
                Invertrans1 = trans1.Inverse;
                InverTrans2_rotacion = trans2_rotacion.Inverse;




            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        //new XYZ(0, 0, deltaelevacionBasePoint)   es por la diferencie entre :  .ProjectElevation - Elevation;
        public XYZ Ejecutar(XYZ pto)
        {
            XYZ ValorTrasformado = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto)) + new XYZ(0, 0, deltaelevacionBasePoint);

            return ValorTrasformado;
        }
    }
}
