using ArmaduraLosaRevit.Model.UTILES.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{

    public class CrearModeLineAyuda
    {


        public static ModelCurve modelarlineas(Document doc, XYZ p1, XYZ p2)
        {
            List<ObjetosPuntosDTO> Lista = new List<ObjetosPuntosDTO>() { new ObjetosPuntosDTO() { P1 = p1, P2 = p2 } };
            var ListaResul = modelarlineas(doc, Lista);
            return ListaResul.FirstOrDefault();
        }


        public static List<ModelCurve> modelarlineas(Document doc, List<ObjetosPuntosDTO> Lista)
        {
            List<ModelCurve> Listamc = new List<ModelCurve>();
            XYZ p1 = default;
            XYZ p2 = default;
            try
            {

                using (Transaction tr = new Transaction(doc, "CrearModeline-NH"))
                {
                    tr.Start();

                    for (int i = 0; i < Lista.Count; i++)
                    {
                        p1 = Lista[i].P1;
                        p2 = Lista[i].P2;

                        Curve curvamodel = Line.CreateBound(p1, p2);
                        if (curvamodel.Length < doc.Application.ShortCurveTolerance) { return null; }
                        XYZ vectorlinea = new XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
                        Plane plano = Plane.CreateByNormalAndOrigin(p1.CrossProduct(p2), p1);
                        SketchPlane sk = SketchPlane.Create(doc, plano);
                        var mc = doc.Create.NewModelCurve(curvamodel, sk);
                        Listamc.Add(mc);
                    }

                    tr.Commit();
                }

            }
            catch (Exception)
            { 
            }
            return Listamc;
        }



        public static ModelCurve modelarlinea_sinTrans(Document doc, XYZ p1, XYZ p2)
        {
            ModelCurve mc = null;

            Curve curvamodel = Line.CreateBound(p1, p2);
            if (curvamodel.Length < doc.Application.ShortCurveTolerance) { return null; }
            XYZ vectorlinea = new XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            Plane plano = Plane.CreateByNormalAndOrigin(p1.CrossProduct(p2), p1);
            SketchPlane sk = SketchPlane.Create(doc, plano);
            mc = doc.Create.NewModelCurve(curvamodel, sk);


            return mc;
        }
    }
}
