using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioConduit : EnvoltorioBase
    {

        public Conduit _Ducto { get; set; }

        public EnvoltorioConduit(Conduit c, Transform transform) : base(c, transform)
        {
            this._Ducto = c;
            _elemento = c;
            this._tipo = c.Category;
         //   this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = c.Name;

            conect = (c).ConnectorManager;
        }

        public  bool ObtenerLArgoAncho()
        {
            try
            {
                var resultDiametro=ParameterUtil.FindParaByName(_Ducto.Parameters, "Diameter");

                if (resultDiametro != null)
                {
                  double  Largo_foot = _Ducto.Diameter + Util.CmToFoot(0);
                    LargoAncho_DibujarPasada_foot = Largo_foot;
                    LargoAlto_DibujarPasada_foot = Largo_foot;
                }
                else
                {
                 //   LargoAncho_foot = _Ducto.Width;// Math.Max(Ancho, Alto) + Util.CmToFoot(0);
                  //  LargoAlto_foot = _Ducto.Height;

                    LargoAncho_DibujarPasada_foot = _Ducto.Width;
                    LargoAlto_DibujarPasada_foot = _Ducto.Height;
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

