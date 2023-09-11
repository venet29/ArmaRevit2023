using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ASSERT.Extension
{
    public class ptoRevisar
    {
        public ptoRevisar(int v1, int v2, int v3, int v4)
        {
            this.X = v1;
            this.Y = v2;
            this.Z = v3;
            this.Result = v4;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Result { get; set; }

    }

    [TransactionAttribute(TransactionMode.Manual)]

    public class cmd_ExtensionPuntoXYZ : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            List<ptoRevisar> listpto_ProbarAnguloXY = new List<ptoRevisar>();
            listpto_ProbarAnguloXY.Add(new ptoRevisar(0, 0, 0, 0));//0
            listpto_ProbarAnguloXY.Add(new ptoRevisar(1, 1, 0, 45)); //45°
            listpto_ProbarAnguloXY.Add(new ptoRevisar(0, 1, 0, 90));//90°
            listpto_ProbarAnguloXY.Add(new ptoRevisar(-1, 1, 0, 135)); //135°
            listpto_ProbarAnguloXY.Add(new ptoRevisar(-1, 0, 0, 180)); //180°
            listpto_ProbarAnguloXY.Add(new ptoRevisar(1, -1, 0, -45));//-0
            listpto_ProbarAnguloXY.Add(new ptoRevisar(0, -1, 0, -90)); //-45°
            listpto_ProbarAnguloXY.Add(new ptoRevisar(-1, -1, 0, -135));//-90°

            foreach (var item in listpto_ProbarAnguloXY) ProbarAnguloXY(item.X, item.Y, item.Result);


            int[] listaZ = { 1, 3, 9,  -1, -3, -9 };
            double[] reslt = { 35.26438968, 64.76059818, 81.06985816, -35.26438968, -64.76059818, -81.06985816 };

            for (int i = 0; i < listaZ.Length; i++)   ProbarAnguloPlanoXYConEjeZ(1, 1, listaZ[i], reslt[i]);

            for (int i = 0; i < listaZ.Length; i++) ProbarAnguloPlanoXYConEjeZ(-1, 1, listaZ[i], reslt[i]);
            for (int i = 0; i < listaZ.Length; i++) ProbarAnguloPlanoXYConEjeZ(-1, -1, listaZ[i], reslt[i]);
            for (int i = 0; i < listaZ.Length; i++) ProbarAnguloPlanoXYConEjeZ(1, -1, listaZ[i], reslt[i]);



            //caso al asar
            XYZ p1 = new XYZ(25 ,-9, 65);
            Debug.Assert(Util.IsSimilarValor(p1.GetAngleEnZ_respectoPlanoXY(true), 67.76626777, 0.000000000001));

            XYZ p2 = new XYZ(-123.3265, 5.659, -365.235);
            Debug.Assert(Util.IsSimilarValor(p2.GetAngleEnZ_respectoPlanoXY(true), -71.32378416, 0.000000000001));
            return Result.Succeeded;
        }


        private static void ProbarAnguloXY(double x, double y, double result)
        {
            XYZ valor_0 = new XYZ(x, y, 50);
            XYZ valor2_0 = new XYZ(x, y, -50);
            Debug.Assert(Util.IsSimilarValor(valor_0.GetAngleXY0(true), result));
            Debug.Assert(Util.IsSimilarValor(valor2_0.GetAngleXY0(true), result));
        }

        private static void ProbarAnguloPlanoXYConEjeZ(double x, double y, double z, double result)
        {
            XYZ valor_0 = new XYZ(x, y, z);
            XYZ valor2_0 = new XYZ(-x, -y, z);
            Debug.Assert(Util.IsSimilarValor(valor_0.GetAngleEnZ_respectoPlanoXY(true), result, 0.000000000001));
            Debug.Assert(Util.IsSimilarValor(valor2_0.GetAngleEnZ_respectoPlanoXY(true), result,0.000000000001));
        }
    }
}

