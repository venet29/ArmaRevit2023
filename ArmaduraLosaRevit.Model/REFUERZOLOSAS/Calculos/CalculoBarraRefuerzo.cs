
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Interseccion;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    //pts para trabajr con ls trasformadas
    //entonces siempre seran horizontales
    public class CalculoBarraRefuerzo
    {
        //name  sera  b1Sup,b2Sup,b3Sup,b4Sup....
        //name  sera  b1Inf,b2Inf,b3Inf,b4Inf,...
        public string Name { get; set; }

        private EmpotramientoPatasLosaDTO _empotramientoPatasDTO;
        private readonly double desplazamientoBArraAbajo = Util.CmToFoot(2);
        //coordenadas trasladadas
        // los ptos siempre tendran este orden en el eje x
        //  pa     pb     pc     pd
        public XYZ pa { get; set; }
        public XYZ pb { get; set; }
        public XYZ pc { get; set; }
        public XYZ pd { get; set; }


        private readonly UIApplication _uipp;
        private Transform _Invertrans1;
        private Transform _InverTrans2_rotacion;


        private double _deltaDesplazamiento { get; set; }
        //ptos vueltos las coordenadas genrales
        public XYZ _direcciMueveBarra { get; set; }
        public XYZ pa_Orig { get; set; }
        public XYZ pb_Orig { get; set; }
        public XYZ pc_Orig { get; set; }
        public XYZ pd_Orig { get; set; }

        public XYZ ptoini_interseccion { get; set; }
        public XYZ ptofinal_interseccion { get; set; }
        public TipoBarraRefuerzo tipoBarraRefuerzo { get; set; }

        public int diamEnMM { get; set; }
        public double AnguloBarraRad { get; set; }
        public double AnguloBarra { get; set; }
        public bool IsOK { get; set; }
        public bool IsUsar2pto { get; set; }// se dibuja con los ptos del mouse, evita que se agrgue largodedesarollo
        public XYZ _NormalCaraSuperiorLosa { get; set; }


        // ref losa y bordelosa
        public CalculoBarraRefuerzo(UIApplication _uipp, string name, CalculoBarraRefuerzoDTo _datosRefuerzoTipoViga, XYZ _pb, XYZ _pc, Transform Invertrans1, Transform InverTrans2_rotacion, XYZ direcciMueveBarra, bool _IsUsar2pto = false)
        {
            this._uipp = _uipp;
            this.Name = name;
            this._empotramientoPatasDTO = _datosRefuerzoTipoViga._empotramientoPatasDTO;
            this.diamEnMM = _datosRefuerzoTipoViga.diamtroBarraRefuerzo_MM;
            this.pb = _pb;
            this.pc = _pc;
            this._Invertrans1 = Invertrans1;
            this._InverTrans2_rotacion = InverTrans2_rotacion;
            this._deltaDesplazamiento = 0;
            this._direcciMueveBarra = direcciMueveBarra;
            generaPtosrExtremos_cabezamuro();
            VolverCoordenadasAnguloOrigianl();
            this.IsUsar2pto = _IsUsar2pto;
        }

        //solo cabeza muro
        public CalculoBarraRefuerzo(UIApplication _uipp, string name, int diamEnMM, XYZ _pb, XYZ _pc,
            Transform Invertrans1, Transform InverTrans2_rotacion, double deltaDesplazamiento, XYZ direcciMueveBarra, bool _IsUsar2pto = false)
        {
            this._uipp = _uipp;
            Name = name;
            this.diamEnMM = diamEnMM;
            _empotramientoPatasDTO = new EmpotramientoPatasLosaDTO();
            pb = _pb;
            pc = _pc;
            _Invertrans1 = Invertrans1;
            _InverTrans2_rotacion = InverTrans2_rotacion;
            _deltaDesplazamiento = deltaDesplazamiento;
            this._direcciMueveBarra = direcciMueveBarra;
            this.IsUsar2pto = _IsUsar2pto;
        }

        public CalculoBarraRefuerzo()
        {
        }

        public void CalculosIniciales()
        {
            try
            {
                //  generaPtosrExtremos_cabezamuro();
                VolverCoordenadasAnguloOrigianl();
                AnguloBarra = Util.AnguloEntre2PtosGrados_enPlanoXY(pa_Orig, pb_Orig);
                AnguloBarraRad = Util.GradosToRadianes(AnguloBarra);
            }
            catch (Exception)
            {
                IsOK = false;
            }
            IsOK = true;
        }
        //obtiene los ptos inicial y final que definen la posicion de la brra en coordenadas trasladadas
        public void generaPtosrExtremos_cabezamuro()
        {
            double largoDesarrollo_dentroMuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_dentroMuro(diamEnMM) * _empotramientoPatasDTO.factorLargoIni - _deltaDesplazamiento;
            double largoDesarrollo_fueramuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_fueramuro(diamEnMM) * _empotramientoPatasDTO.factorLargoFin - _deltaDesplazamiento;

            if (IsUsar2pto)
            {
                largoDesarrollo_dentroMuro = 0;
                largoDesarrollo_fueramuro = 0;
            }

            pa = new XYZ(pb.X - largoDesarrollo_dentroMuro, pb.Y, pb.Z);
            pd = new XYZ(pc.X + largoDesarrollo_fueramuro, pb.Y, pb.Z);

            if (Util.IsSimilarValor(pa.DistanceTo(pd), Util.CmToFoot(200), 0.01))
            {
                double diference = Util.CmToFoot(200) - pa.DistanceTo(pd);
                pa = pa + (pa - pd).Normalize() * diference;
            }

        }
        //para caso refuelotipoviga y refuerzo borde
        public void generaPtosrExtremos()
        {
            double largoDesarrollo_dentroMuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamEnMM) * _empotramientoPatasDTO.factorLargoIni - _deltaDesplazamiento;
            double largoDesarrollo_fueramuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamEnMM) * _empotramientoPatasDTO.factorLargoFin - _deltaDesplazamiento;

            pa = new XYZ(pb.X - largoDesarrollo_dentroMuro, pb.Y, pb.Z);
            pd = new XYZ(pc.X + largoDesarrollo_fueramuro, pb.Y, pb.Z);
        }
        //vulave a transformar coordenas a los valores originales
        private void VolverCoordenadasAnguloOrigianl()
        {
            pa_Orig = _Invertrans1.OfPoint(_InverTrans2_rotacion.OfPoint(pa)) - new XYZ(0, 0, desplazamientoBArraAbajo);
            pb_Orig = _Invertrans1.OfPoint(_InverTrans2_rotacion.OfPoint(pb)) - new XYZ(0, 0, desplazamientoBArraAbajo);
            pc_Orig = _Invertrans1.OfPoint(_InverTrans2_rotacion.OfPoint(pc)) - new XYZ(0, 0, desplazamientoBArraAbajo);
            pd_Orig = _Invertrans1.OfPoint(_InverTrans2_rotacion.OfPoint(pd)) - new XYZ(0, 0, desplazamientoBArraAbajo);
        }


        public void BuscarPatasAmbosLadosHorizontal(EmpotramientoPatasLosaDTO _empotramientoPatasDTO, View3D _view3D_paraBuscar)
        {
            IntersecionLosa TiposDeBarraPorInterseccion =
                new IntersecionLosa(_uipp, _view3D_paraBuscar, _empotramientoPatasDTO, pa_Orig, pd_Orig);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            ptoini_interseccion = TiposDeBarraPorInterseccion._ptoini;
            ptofinal_interseccion = TiposDeBarraPorInterseccion._ptofinal;
            tipoBarraRefuerzo = TiposDeBarraPorInterseccion.ResultTipoBarraRef;
        }

        public void IngresarPatasLadosHorizontal(EmpotramientoPatasLosaDTO _empotramientoPatasDTO, View3D _view3D_paraBuscar, TipoBarraRefuerzo tipoBarraRefuerzo)
        {
            IntersecionLosa TiposDeBarraPorInterseccion =
                new IntersecionLosa(_uipp, _view3D_paraBuscar, _empotramientoPatasDTO, pa_Orig, pd_Orig);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            ptoini_interseccion = TiposDeBarraPorInterseccion._ptoini;
            ptofinal_interseccion = TiposDeBarraPorInterseccion._ptofinal;
            this.tipoBarraRefuerzo = tipoBarraRefuerzo;
        }


    }
}
