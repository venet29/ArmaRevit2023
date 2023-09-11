using System;
using System.Linq;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioCableTray : EnvoltorioBase
    {


        public CableTray _CableTray { get; set; }



        public EnvoltorioCableTray(CableTray c, Transform transform) : base(c, transform)
        {
            this._CableTray = c;
            _elemento = c;
            this._tipo = c.Category;
           // this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = c.Name;
            conect = (c).ConnectorManager;
        }


        public  bool ObtenerLArgoAncho()
        {
            try
            {
                LargoAncho_DibujarPasada_foot = _CableTray.Width;
                LargoAlto_DibujarPasada_foot = _CableTray.Height;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}

