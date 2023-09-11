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
    public class EnvoltorioPipes : EnvoltorioBase
    {
        //protected Element _elemento;

        public Pipe Piper_ { get; set; }

 

        public EnvoltorioPipes(Pipe c, Transform transform) : base(c, transform)
        {
            this.Piper_ = c;
            this._elemento = c;
            this._tipo = c.Category;
          //  this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = Piper_.Name;
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();
            conect = (c).ConnectorManager;
        }


        //public EnvoltorioPipes()
        //{
        //    ListaPasadas = new List<EnvoltorioShaft>();
        //    profile = new CurveArray();
        //}

        public bool ObtenerLArgoAncho()
        {
            try
            {
                double largo = 0;
                // para buscar elemento para recubrir
                ElementCategoryFilter fwall = new ElementCategoryFilter(BuiltInCategory.OST_PipeInsulations);
                var listaDependencias = Piper_.GetDependentElements(fwall);

                if (listaDependencias.Count > 0) //´pipen con insolation
                {
                    for (int i = 0; i < listaDependencias.Count; i++)
                    {
                        var pipeInsolet = _doc.GetElement(listaDependencias[i]) as PipeInsulation;
                        if (pipeInsolet != null)
                        {
                            largo = pipeInsolet.Diameter + Util.CmToFoot(0);
                            break;
                        }
                    }
                }
                else
                    largo = Piper_.Diameter + Util.CmToFoot(0);


                LargoAncho_DibujarPasada_foot = largo;
                LargoAlto_DibujarPasada_foot = largo;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}

