using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{

    /// <summary>
    /// obtiene el vector normal al plano visible de elemneto contenedor derebar 
    /// </summary>
    public class AyudaObtenerNormarPlanoVisisible
    {
        public static PlanarFace caraVisibleVErtical { get; private set; }
        public static XYZ FaceNormal { get; private set; }
        public static bool Isok { get; private set; }

        public static bool Obtener(Rebar _barra1, View _view)
        {
            Isok = false;
            if (_barra1 == null) return false;
            try
            {
                bool result = false;
                var _doc = _barra1.Document;
                var _ElementContenderoBArra = _doc.GetElement(_barra1.GetHostId());
                ( result, caraVisibleVErtical) = _ElementContenderoBArra.ObtenerCaraVerticalVIsible(_view);
                Isok = result;
                if (!result)
                {
                    Util.ErrorMsg("No se puedo al obtener cara visible vertical de elemento.");
                    return false;
                }
                FaceNormal = caraVisibleVErtical.FaceNormal;
                return true;// Obtener(_ElementContenderoBArra, _view);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener cara visible vertical de elemento.\n ex:{ex.Message}");
                return false;
            }
        }

        public static bool Obtener(Element _ElementContenderoBArra, View _view)
        {
            Isok = false;
            if (_ElementContenderoBArra == null) return false;
            try
            {
                bool result = false;
                ( result, caraVisibleVErtical)  = _ElementContenderoBArra.ObtenerCaraVerticalVIsible(_view);
                if (!result)
                {
                    Util.ErrorMsg("No se puedo al obtener cara visible vertical de elemento.");
                    return false;
                }
                FaceNormal = caraVisibleVErtical.FaceNormal;
                Isok = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener cara visible vertical de elemento.\n ex:{ex.Message}");
                return false;
            }

            return true;
        }
    }
}
