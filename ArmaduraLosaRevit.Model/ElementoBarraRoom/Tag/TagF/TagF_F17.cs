using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
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
    public class TagF_F17 : ATagF_Base, ITagF
    {
        public string RebarSaphe1;
        public string RebarSaphe2;
        private List<ElementId> ListElemId;
        private DatosPathRinforment _ObtenerDatosPathRinforment;

        public TagF_F17(Document doc, PathReinforcement m_createdPathReinforcement) : base(doc, m_createdPathReinforcement)
        {
            this._doc = doc;
            _createdPathReinforcement = m_createdPathReinforcement;
            IsBarraPrimaria = true;
            IsBarraSecuandaria = true;
        }
        public bool Ejecutar()
        {

            try
            {
                if (_createdPathReinforcement == null) return false;
                if (_createdPathReinforcement.IsValidObject == false) return false;

                _ObtenerDatosPathRinforment = new DatosPathRinforment(_createdPathReinforcement);
                if (!_ObtenerDatosPathRinforment.ObtenerCantiadadCasoF17())
                {
                    IsBarraPrimaria = false;
                    IsBarraSecuandaria = false;
                }

                ObtenerParametros();
                CopiarParametrosSinTrasn();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'TagF_simple'  ex: {ex.Message}");
            }
            return true;

        }

        private void ObtenerParametros()
        {
            try
            {
                RebarSaphe1 = _ObtenerDatosPathRinforment.RebarSaphe1;
                RebarSaphe2 = _ObtenerDatosPathRinforment.RebarSaphe2;

                numeroBarrasPrimaria = _ObtenerDatosPathRinforment.numeroBarrasPrimaria;
                numeroBarrasSecundario = _ObtenerDatosPathRinforment.numeroBarrasSecundario;
            


            }
            catch (Exception)
            {
                MsjeError();
                return;
            }
}


private bool Validar()
{
    try
    {
        var Ilist_RebarInSystem = _createdPathReinforcement.GetRebarInSystemIds();

        if (Ilist_RebarInSystem == null)
        {
            MsjeError();
            return false;
        }
        ListElemId = Ilist_RebarInSystem.ToList();
        if (ListElemId.Count != 2)
        {
            MsjeError();
            return false;
        }
    }
    catch (Exception)
    {
        return false;
    }
    return true;
}

private void MsjeError()
{
    IsBarraPrimaria = false;
    IsBarraSecuandaria = false;
    Util.ErrorMsg("Erro al calcular numero de barras Caso F17");
}
    }
}
