using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Geometria
{
    public class EstribosRectagularesOrtogonales_H
    {
        private RebarElevDTO rebarElevDTO;

        public EstribosRectagularesOrtogonales_H(RebarElevDTO rebarElevDTO)
        {
            this.rebarElevDTO = rebarElevDTO;
        }

        public XYZ UbicacionDeF { get;  set; }
        public XYZ UbicacionDeL { get; private set; }
        public XYZ UbicacionSup { get; set; }
        public string UbicacionSup_ValorLArgo { get; private set; }
        public XYZ UbicacionIZq { get; set; }
        public string UbicacionIZq_ValorLArgo { get; private set; }

        public bool calcularUbiaciontexto()
        {

            try
            {
                var sololist =rebarElevDTO.ListaCurvaBarrasFinal_conCurva.Where(c => c.TipoCurva == TipoCUrva.linea &&
                                                                                c.FijacionInicial==FijacionRebar.fijo &&
                                                                                c.FijacionFinal == FijacionRebar.fijo).ToList();
                double Delta = 0;
                var cur1 = rebarElevDTO.ListaCurvaBarrasFinal_conCurva.Where(c => c.TipoCurva == TipoCUrva.arco).FirstOrDefault();

                if (cur1 != null)
                {
                    var radio = ((Arc)cur1._curve).Radius;
                    Delta = radio * 2 + rebarElevDTO.diametroFoot;
                }

                List<PtosCurvaAuxDTO> ListaptosTrans = 
                    AyudaObtenerPtosTransformada.ObtenerPtosSINTransformados(sololist, rebarElevDTO._View);

                XYZ unitariZ = new XYZ(0, 0, -1);
                //1) inf
                var Lisinferior =ListaptosTrans.Where(c => c.PtoInicialTransformada.Z < 0 && c.PtoFinalTransformada.Z < 0).OrderByDescending(c=>c.largoCurve).ToList();

                if (Lisinferior.Count== 0)
                    return false;

                UbicacionDeF = Lisinferior[0].PtoMedio + Util.CmToFoot(7) * unitariZ;// rebarElevDTO._View.RightDirection*Util.CmToFoot(10); // cambiar '_View.ViewDirection' si se selecciona a la izquierda o derecha
                UbicacionDeL = Lisinferior[0].PtoMedio + Util.CmToFoot(12) * unitariZ;// rebarElevDTO._View.RightDirection * Util.CmToFoot(15);


                //2)sup
                var superior = ListaptosTrans.Where(c => c.PtoInicialTransformada.Z > 0 && c.PtoFinalTransformada.Z > 0).OrderByDescending(c => c.largoCurve).ToList();

                if (superior.Count == 0)
                    return false;
                UbicacionSup = superior[0].PtoMedio -unitariZ* Util.CmToFoot(5);
                UbicacionSup_ValorLArgo = Math.Round(Util.FootToCm(superior[0].largoCurve+ Delta), 0).ToString(); //largo no real

                //3 izq
                var izq = ListaptosTrans.Where(c => c.PtoInicialTransformada.X < 0 && c.PtoFinalTransformada.X< 0).OrderByDescending(c => c.largoCurve).ToList();

                if (izq.Count == 0)
                    return false;
                UbicacionIZq = izq[0].PtoMedio + rebarElevDTO._View.RightDirection * Util.CmToFoot(5);
                UbicacionIZq_ValorLArgo = Math.Round(Util.FootToCm(izq[0].largoCurve+ Delta), 0).ToString(); //largo no real

            }
            catch (Exception ex)
            {


                Util.ErrorMsg($"Error al obtener ubicacion texto  ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
