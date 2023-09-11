using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.AyudasView
{
    public class ObtenerNivelPorOrigenBarra
    {
        private  UIApplication _uiapp;
        private List<NivelLosa> listaLevel;

        public List<NivelLosa> ListaNivelLosa { get; set; }

        public ObtenerNivelPorOrigenBarra(UIApplication _uiapp )
        {
            this._uiapp = _uiapp;
        }


        public bool CalcularNivelesLosa()
        {

            try
            {
                ListaNivelLosa = new List<NivelLosa>();
                SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
              listaLevel = _seleccionarNivel.M3_ObtenerListaNivelOrdenadoPorElevacionDeProyecto().Select(c=> new NivelLosa() { Nivel_= c ,CotaReal=c.ProjectElevation}).ToList();

                //sin datos
                if (listaLevel.Count == 0) return false;

                //1 dato
                if (listaLevel.Count == 1)
                {
                    listaLevel[0].CotaInferior = -1000000000;
                    listaLevel[0].COtaSuperior = +1000000000;
                    return true;
                }

                //mas de 2 dato
                for (int i = 0; i < listaLevel.Count; i++)
                {
                    if (i == 0)
                    {
                        listaLevel[i].CotaInferior = -1000000000;
                        listaLevel[i].COtaSuperior = (listaLevel[i+1].CotaReal + listaLevel[i].CotaReal) /2;
                    }
                    else if (i == listaLevel.Count-1)
                    {
                        listaLevel[i].CotaInferior = listaLevel[i].COtaSuperior ;
                        listaLevel[i].COtaSuperior = +10000000000;
                    }
                    else 
                    {
                        listaLevel[i].CotaInferior =  listaLevel[i - 1].COtaSuperior;
                        listaLevel[i].COtaSuperior = (listaLevel[i + 1].CotaReal + listaLevel[i].CotaReal) / 2;
                    }
                }

            }
            catch (Exception)
            {
                return false;              
            }
            return true;
        }
        public Level ObtenerNivel(double posicion)
        {
            var resul = listaLevel.Where(c => c.CotaInferior < posicion && posicion < c.COtaSuperior).FirstOrDefault();
            return (resul!=null?resul.Nivel_:null);
        }
    }
}
