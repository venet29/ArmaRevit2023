using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{
    public   class BarraRoomF1_Sup
    {
        private  List<XYZ> _listaPtosPerimetroBarras;
        private readonly double _largoMinLosa;
        private readonly double _diamtromm;
        private int _espesorLosa;

        public List<XYZ> ListaPtosPerimetroBarrasIzqINf { get; set; }
        public List<XYZ> ListaPtosPerimetroBarrasDereSUp { get; set; }


        public List<Curve> CurvesPathreiforment_IzqInf { get; set; }
        public List<Curve> CurvesPathreiforment_DereSup { get; set; }

        public XYZ ptoConMouseEnlosaF1_SUPIzqINf { get; set; }
        public XYZ ptoConMouseEnlosaF1_SUPDereSup { get;  set; }

        private double _pataInferiro_foot;
        private double _espesorMuro;
        private double _anchoPathborde;
        private double _angulobarra;
        private double _anguloContraBarra;

        public BarraRoomF1_Sup(List<XYZ> listaPtosPerimetroBarras, double largoMinLosa, double diamtromm)
        {
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._largoMinLosa = largoMinLosa;
            this._diamtromm = diamtromm;
            this._espesorLosa = 0;
            ListaPtosPerimetroBarrasIzqINf = new List<XYZ>();
            ListaPtosPerimetroBarrasDereSUp = new List<XYZ>();
            CurvesPathreiforment_IzqInf = new List<Curve>();
            CurvesPathreiforment_DereSup = new List<Curve>();
        }



        //MUEVE LAS PATH SYMBOL DEL CASO F1_SUP_   :Obs2
        public  void CalcularPAth_F1_SUP( )
        {
            //lado izq - bajo
            CalculosPrevios();

            OBtenerPathIzqInf_casoF1SUP();

            OBtenerPathDereSup_casoF1SUP();

        }

        private void CalculosPrevios()
        {
            _pataInferiro_foot = Util.CmToFoot(ConstNH.CONST_PATA_SX); //Util.MmToFoot(20 * _diamtromm + 100);
            _espesorMuro = Util.CmToFoot(15);
            _anchoPathborde = Math.Max( _espesorMuro + (_largoMinLosa * ConstNH.PORCENTAJE_LARGO_PATA), ConstNH.LARGO_MIN_PATH_S4_FOOT);
            _angulobarra = Util.angulo_entre_pt_Rad_XY0(_listaPtosPerimetroBarras[1], _listaPtosPerimetroBarras[2]);
            _anguloContraBarra = Util.angulo_entre_pt_Rad_XY0(_listaPtosPerimetroBarras[2], _listaPtosPerimetroBarras[1]);
        }

        private void OBtenerPathIzqInf_casoF1SUP()
        {
            double deltaRedondeo = 0;
            if (RedonderLargoBarras.RedondearFoot5_AltMascercano(_anchoPathborde + _pataInferiro_foot + _espesorLosa))
                deltaRedondeo = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;



            XYZ p4_pathIzqInf = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_listaPtosPerimetroBarras[0], _angulobarra, _anchoPathborde);
            XYZ p3_pathIzqInf = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_listaPtosPerimetroBarras[1], _angulobarra, _anchoPathborde);


            ListaPtosPerimetroBarrasIzqINf.Add(_listaPtosPerimetroBarras[0]);
            ListaPtosPerimetroBarrasIzqINf.Add(_listaPtosPerimetroBarras[1]);
            ListaPtosPerimetroBarrasIzqINf.Add(p3_pathIzqInf);
            ListaPtosPerimetroBarrasIzqINf.Add(p4_pathIzqInf);
            
            //CurvesPathreiforment_IzqInf.Add(Line.CreateBound(p4_pathIzqInf, p3_pathIzqInf));

            //ptoConMouseEnlosaF1_SUPIzqINf = new XYZ( ListaPtosPerimetroBarrasIzqINf.Average(c=>c.X ),
            //     ListaPtosPerimetroBarrasIzqINf.Average(c => c.Y),
            //      ListaPtosPerimetroBarrasIzqINf.Average(c => c.Z));
        }

        private void OBtenerPathDereSup_casoF1SUP()
        {
            XYZ p1_pathDereSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_listaPtosPerimetroBarras[3], _anguloContraBarra, _anchoPathborde);
            XYZ p2_pathDereSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_listaPtosPerimetroBarras[2], _anguloContraBarra, _anchoPathborde);
     
            ListaPtosPerimetroBarrasDereSUp.Add(p1_pathDereSup);
            ListaPtosPerimetroBarrasDereSUp.Add(p2_pathDereSup);
            ListaPtosPerimetroBarrasDereSUp.Add(_listaPtosPerimetroBarras[2]);
            ListaPtosPerimetroBarrasDereSUp.Add(_listaPtosPerimetroBarras[3]);
           
            //CurvesPathreiforment_DereSup.Add(Line.CreateBound(_listaPtosPerimetroBarras[3], _listaPtosPerimetroBarras[2]));
         
            //ptoConMouseEnlosaF1_SUPDereSup = new XYZ(ListaPtosPerimetroBarrasIzqINf.Average(c => c.X),
            //     ListaPtosPerimetroBarrasIzqINf.Average(c => c.Y),
            //      ListaPtosPerimetroBarrasIzqINf.Average(c => c.Z));
        }



    }
}

