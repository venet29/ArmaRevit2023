using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearDetailLineAyuda
    {


        public static DetailCurve modelarlineas_ConTransaccion(Document _doc, View _view, XYZ p1, XYZ p2, string nombreeStilo = "")
        {
            DetailCurve mc = null;
            using (Transaction tr = new Transaction(_doc, "CrearModeline-NH"))
            {
                tr.Start();

                mc = modelarlinea_sinTrans(_doc, _view, p1, p2, nombreeStilo);

                tr.Commit();
            }
            return mc;
        }
        public static List<DetailCurve> modelarlineas_Sintransacion(Document _doc, View _view, List<XYZ> ListaPtosConsecutivos, string nombreeStilo = "")
        {
            List<DetailCurve> Listmc = new List<DetailCurve>();

            if (ListaPtosConsecutivos.Count < 2)
                return Listmc;

            for (int i = 0; i < ListaPtosConsecutivos.Count-1; i++)
            {
                if (ListaPtosConsecutivos[i + 1].DistanceTo(ListaPtosConsecutivos[i]) < Util.CmToFoot(1)) continue;
                var mc = modelarlinea_sinTrans(_doc, _view, ListaPtosConsecutivos[i], ListaPtosConsecutivos[i+1], nombreeStilo);
                Listmc.Add(mc);
            }

            if (ListaPtosConsecutivos.Count > 2 && ListaPtosConsecutivos.Last().DistanceTo(ListaPtosConsecutivos[0]) > Util.CmToFoot(1))
            {

                var mc = modelarlinea_sinTrans(_doc, _view, ListaPtosConsecutivos[0], ListaPtosConsecutivos.Last(), nombreeStilo);
                Listmc.Add(mc);
            }

            return Listmc;
        }


        public static DetailCurve modelarlinea_sinTrans(Document _doc, View _view, XYZ p1, XYZ p2, string nombreeStilo = "")
        {
            DetailCurve mc = null;
            Curve curvamodel = Line.CreateBound(p1, p2);
            if (curvamodel.Length < _doc.Application.ShortCurveTolerance) { return null; }
            mc = _doc.Create.NewDetailCurve(_view, curvamodel);

            if (nombreeStilo != null)
            {
                var verde_primaria_line_styles = TiposLineaPattern.ObtenerTipoLinea(nombreeStilo, _doc);
                if (verde_primaria_line_styles != null)
                    mc.LineStyle = verde_primaria_line_styles;
            }
            return mc;
        }
    }
}
