using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearTrasformadaSobreEjeZ
    {
        Transform trans1 = null;
        Transform trans2_rotacion = null;

        Transform Invertrans1 = null;
        Transform InverTrans2_rotacion = null;

        public XYZ _origenSeccion { get; set; }//punto mas al derecha, abajo y mas cerca de pantalla a la  vista


        private double _anguloGrados;
        public bool Isvalid { get; set; }



        public CrearTrasformadaSobreEjeZ(XYZ posicion,double anguloGiro_Grados)
        {

            this._origenSeccion = posicion;
             this._anguloGrados = anguloGiro_Grados;
  
            Isvalid = ObtenerTransformados();
        }

  
        private bool ObtenerTransformados()
        {
            try
            {
                trans1 = Transform.CreateTranslation(-_origenSeccion.GetXY0());
                trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(_anguloGrados), XYZ.Zero);
                Invertrans1 = trans1.Inverse;
                InverTrans2_rotacion = trans2_rotacion.Inverse;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public XYZ EjecutarTransformInvertida(XYZ pto)
        {
            XYZ ValorTrasformado= Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto));

            return ValorTrasformado;
        }

        public XYZ EjecutarTransform(XYZ pto)
        {
          //  XYZ ValorTrasformado = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pto));
            XYZ ValorTrasformado = trans2_rotacion.OfPoint(trans1.OfPoint(pto));
            return ValorTrasformado;
        }


    }
}
