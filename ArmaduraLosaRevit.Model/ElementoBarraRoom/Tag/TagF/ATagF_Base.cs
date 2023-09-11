using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF
{
    public class ATagF_Base 
       {
        protected  Document _doc;
        protected  PathReinforcement _createdPathReinforcement;


        public int numeroBarrasSecundario { get ; set; }
        public int numeroBarrasPrimaria { get  ; set; }
        public bool IsBarraPrimaria { get  ; set; }
        public bool IsBarraSecuandaria { get  ; set; }

        public ATagF_Base()
        { }
        public ATagF_Base(Document doc,PathReinforcement m_createdPathReinforcement)
        {
            this._doc = doc;
            _createdPathReinforcement = m_createdPathReinforcement;
            IsBarraPrimaria = true;
            IsBarraSecuandaria = false;
        }    

        public bool CopiarParametrosSinTrasn()
        {
            try
            {
                if (IsBarraPrimaria)
                {
                    if (ParameterUtil.FindParaByName(_createdPathReinforcement, "NumeroPrimario") != null)
                        ParameterUtil.SetParaInt(_createdPathReinforcement, "NumeroPrimario", numeroBarrasPrimaria.ToString());
                }

                if (IsBarraSecuandaria)
                {
                    if (ParameterUtil.FindParaByName(_createdPathReinforcement, "NumeroSecundario") != null)
                        ParameterUtil.SetParaInt(_createdPathReinforcement, "NumeroSecundario", numeroBarrasSecundario.ToString());
                }
            }
            catch (Exception ex)
            {
               Util.ErrorMsg($" Erro al copiar numero de barras  ex:{ex.Message}");
                return false;
            }

            return true;
        }
           
    }
}
