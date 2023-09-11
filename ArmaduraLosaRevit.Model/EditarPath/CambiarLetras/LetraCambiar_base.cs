using ArmaduraLosaRevit.Model.EditarPath.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CambiarLetras
{
    public class LetraCambiar_base
    {
        private readonly Document doc;
        protected PathReinforcement pathReinforcement;
        protected string _TipoBarra;

        public List<LetraCambiarDTO> ListaLetraCambiarDTO { get; set; }
        public bool Isok { get; set; }

    public LetraCambiar_base(Document doc,PathReinforcement pathReinforcement, string _TipoBarra)
        {
            this.doc = doc;
            this.pathReinforcement = pathReinforcement;
            this._TipoBarra = _TipoBarra;
            this.ListaLetraCambiarDTO = new List<LetraCambiarDTO>();
        }

        public LetraCambiar_base()
        {
        }


        //letra: C_ o B_
        protected void CambiarParametros()
        {
            try
            {    
                foreach (LetraCambiarDTO item in ListaLetraCambiarDTO)
                {
                    if (ParameterUtil.FindParaByName(pathReinforcement, item.letraCambiar.ToString()) != null)
                        ParameterUtil.SetParaInt(pathReinforcement, item.letraCambiar.ToString(), item.valor);
                }
     
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar ParametroLetra   ex:{ex.Message}");

            }
        }
    }
}
