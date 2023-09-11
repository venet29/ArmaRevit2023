using ArmaduraLosaRevit.Model.Extension;
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
    public class TagF_simetrica : ATagF_Base, ITagF
    {
        public TagF_simetrica(Document doc, PathReinforcement m_createdPathReinforcement) : base(doc, m_createdPathReinforcement)
        {

            IsBarraPrimaria = true;
            IsBarraSecuandaria = true;
        }

        public bool Ejecutar()
        {
            try
            {
                if (_createdPathReinforcement == null) return false;
                if (_createdPathReinforcement.IsValidObject == false) return false;

                ObtenerParametros();

                CopiarParametrosSinTrasn();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'TagF_simetrica'  ex: {ex.Message}");
            }
            return true;

        }

        private void ObtenerParametros()
        {
            //int aux_numerobarra = 0;
            //bool result = int.TryParse(ParameterUtil.FindParaByBuiltInParameter(_createdPathReinforcement, BuiltInParameter.PATH_REIN_NUMBER_OF_BARS, _doc), out aux_numerobarra);
            //if (!result) Util.ErrorMsg("No se pudo obtener el numero de barras del path");
            numeroBarrasPrimaria = _createdPathReinforcement.ObtenerNumeroBarras();

            if (Util.IsPar(numeroBarrasPrimaria))
            {
                int mitad = numeroBarrasPrimaria / 2;
                numeroBarrasPrimaria = mitad;
                numeroBarrasSecundario = mitad;
            }
            else
            {
                int mitad = Util.ParteEnteraInt(numeroBarrasPrimaria / 2);
                numeroBarrasPrimaria = mitad + 1;
                numeroBarrasSecundario = mitad;
            }
        }

    }
}
