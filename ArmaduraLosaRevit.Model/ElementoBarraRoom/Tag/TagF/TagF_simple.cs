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
    public class TagF_simple : ATagF_Base, ITagF
    {
        public TagF_simple(Document doc, PathReinforcement m_createdPathReinforcement) : base(doc, m_createdPathReinforcement)
        {

            IsBarraPrimaria = true;
            IsBarraSecuandaria = false;
        }

        public bool Ejecutar()
        {
            try
            {
                if (_createdPathReinforcement == null) return false;
                if (_createdPathReinforcement.IsValidObject == false) return false;

                numeroBarrasPrimaria = _createdPathReinforcement.ObtenerNumeroBarras();
                CopiarParametrosSinTrasn();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'TagF_simple'  ex: {ex.Message}");
            }
            return true;
        }

        //private void ObtenerParametros()
        //{
        //    int aux_numerobarra = 0;
        //    bool result = int.TryParse(ParameterUtil.FindParaByBuiltInParameter(_createdPathReinforcement, BuiltInParameter.PATH_REIN_NUMBER_OF_BARS, _doc), out aux_numerobarra);
        //    if (!result) Util.ErrorMsg("No se pudo obtener el numero de barras del path");
        //    numeroBarrasPrimaria = aux_numerobarra;
        //}

    }
}
