using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioPipesFittings : EnvoltorioBase
    {
        //protected Element _elemento;

        public Pipe Piper_ { get; set; }
     

        public EnvoltorioPipesFittings(Pipe c, Transform transform) :base(c, transform)
        {
            this.Piper_ = c;
            this._elemento = c;
            this._tipo = c.Category;
           // this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = Piper_.Name;
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();
            conect = (c).ConnectorManager;
        }


        public override bool ObtenerDatos(View _view)
        {
            try
            {
                IsOK = true;
                //var curva = Ducto.GetCurve();
                var curva = _elemento.GetCurve() as Arc;
                if (curva == null) return IsOK = false;
                NombreId = _elemento.Id.ToString();
                if (NombreId == "3548395")
                {
#pragma warning disable CS0219 // The variable 'ddds' is assigned but its value is never used
                    var ddds = 1;
#pragma warning restore CS0219 // The variable 'ddds' is assigned but its value is never used
                }
                Pto1 = _transform.OfPoint(curva.GetEndPoint(0));//+ _OrigenRevitLinkInstance;
                Pto2 = _transform.OfPoint(curva.GetEndPoint(1));// + _OrigenRevitLinkInstance;
                LargoPipe = curva.Length;
                LargoPipeCm = Math.Round(Util.FootToCm(curva.Length), 1);
                Dire = (Pto2 - Pto1).Normalize();
                InterseccionPipe_ = InterseccionPipe.NoAnalizado;
                BuscandoFamilia_ = BuscandoFamilia.NoSeEncontro;

                var bx = _elemento.get_BoundingBox(_view);

                PMin = _transform.OfPoint(bx.Min);// + _OrigenRevitLinkInstance;
                PMax = _transform.OfPoint(bx.Max);// + _OrigenRevitLinkInstance;

                EstadoShaft = "SinShaft";
                ColorEstadoShaft = "White";

                // ObtenerLeveYgrilla(); 
                ObtnerDireccion();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                IsOK = false;
            }
            return IsOK;
        }

        public  bool ObtenerLArgoAncho()
        {
            try
            {
                double largo = Piper_.Diameter + Util.CmToFoot(0);

                LargoAncho_DibujarPasada_foot = largo;
                LargoAlto_DibujarPasada_foot = largo;


            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //public bool ObtenerDatos(View _view)
        //{
        //    try
        //    {
        //        IsOK = true;
        //        //var curva = Ducto.GetCurve();
        //        var curva = _elemento.GetCurve();

        //        if (curva == null) return IsOK = false;
        //        NombreId = _elemento.Id.ToString();
        //        Pto1 = curva.GetEndPoint(0) + _OrigenRevitLinkInstance;
        //        Pto2 = curva.GetEndPoint(1) + _OrigenRevitLinkInstance;
        //        LargoPipe = curva.Length;
        //        LargoPipeCm = Math.Round(Util.FootToCm(curva.Length), 1);
        //        Dire = ((Line)curva).Direction;
        //        InterseccionPipe_ = InterseccionPipe.NoAnalizado;
        //        BuscandoFamilia_ = BuscandoFamilia.NoSeEncontro;

        //        var bx = _elemento.get_BoundingBox(_view);

        //        PMin = bx.Min + _OrigenRevitLinkInstance;
        //        PMax = bx.Max + _OrigenRevitLinkInstance;

        //        EstadoShaft = "SinShaft";
        //        ColorEstadoShaft = "White";
        //        conect = ((Pipe)_elemento).ConnectorManager;
        //    }
        //    catch (Exception ex)
        //    {

        //        IsOK = false;
        //    }
        //    return IsOK;
        //}




    }
}

