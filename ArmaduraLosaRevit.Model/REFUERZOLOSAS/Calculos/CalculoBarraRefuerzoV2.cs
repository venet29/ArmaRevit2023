
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
    public class CalculoBarraRefuerzoV2
    {
        //name  sera  b1Sup,b2Sup,b3Sup,b4Sup....
        //name  sera  b1Inf,b2Inf,b3Inf,b4Inf,...
        public string Name { get; set; }
        private readonly double desplazamientoBArraAbajo = Util.CmToFoot(2);
        //coordenadas trasladadas
        // los ptos siempre tendran este orden en el eje x
        //  pa     pb     pc     pd
        public XYZ pa { get; set; }
        public XYZ pb { get; set; }
        public XYZ pc { get; set; }
        public XYZ pd { get; set; }


        private readonly UIApplication _uipp;

        private double _deltaDesplazamiento { get; set; }
        //ptos vueltos las coordenadas genrales
        public XYZ _direcciMueveBarra { get; set; }
        public XYZ _direcciBarra { get; set; }

        public XYZ _NormalCaraSuperiorLosa { get; set; }

        public XYZ ptoini_interseccion { get; set; }
        public XYZ ptofinal_interseccion { get; set; }
        public TipoBarraRefuerzo tipoBarraRefuerzo { get; set; }

        public int diamEnMM { get; set; }
        public double AnguloBarraRad { get; set; }
        public double AnguloBarra { get; set; }
        public bool IsOK { get; private set; }
        public bool IsUsar2pto { get;  set; }// se dibuja con los ptos del mouse, evita que se agrgue largodedesarollo


        // ref losa y bordelosa
        public CalculoBarraRefuerzoV2(UIApplication _uipp, string name, int diamEnMM, XYZ _pb, XYZ _pc, Transform Invertrans1, Transform InverTrans2_rotacion, XYZ direcciMueveBarra, bool _IsUsar2pto = false)
        {
            this._uipp = _uipp;
            this.Name = name;
            this.diamEnMM = diamEnMM;
            this.pb = _pb;
            this.pc = _pc;
            this._deltaDesplazamiento = 0;
            this._direcciBarra = (_pc - _pb).Normalize(); ;
            this._direcciMueveBarra = direcciMueveBarra;
            generaPtosrExtremos_cabezamuro();
           // VolverCoordenadasAnguloOrigianl();
            this.IsUsar2pto = _IsUsar2pto;
        }

        //solo cabeza muro
        public CalculoBarraRefuerzoV2(UIApplication _uipp, string name, int diamEnMM, XYZ _pb, XYZ _pc, double deltaDesplazamiento, XYZ direcciMueveBarra, XYZ NormalCaraSuperiorLosa, bool _IsUsar2pto = false)
        {
            this._uipp = _uipp;
            Name = name;
            this.diamEnMM = diamEnMM;
            pb = _pb;
            pc = _pc;
            this._direcciBarra = (_pc - _pb).Normalize(); 
            _deltaDesplazamiento = deltaDesplazamiento;
            this._direcciMueveBarra = direcciMueveBarra;
            this.IsUsar2pto = _IsUsar2pto;
            this._NormalCaraSuperiorLosa = NormalCaraSuperiorLosa;
    }
        public void CalculosIniciales()
        {
            try
            {
             
                VolverCoordenadasAnguloOrigianl();
                AnguloBarra = Util.AnguloEntre2PtosGrados_enPlanoXY(pa, pb);
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
            double largoDesarrollo_dentroMuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_dentroMuro(diamEnMM) - _deltaDesplazamiento;
            double largoDesarrollo_fueramuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_fueramuro(diamEnMM) - _deltaDesplazamiento;

            if (IsUsar2pto)
            {
                largoDesarrollo_dentroMuro = 0;
                largoDesarrollo_fueramuro = 0;
            }

            pa = pb - largoDesarrollo_dentroMuro* _direcciBarra;
            pd = pc + largoDesarrollo_fueramuro * _direcciBarra;
        }
        //para caso refuelotipoviga y refuerzo borde
        public void generaPtosrExtremos()
        {
            double largoDesarrollo_dentroMuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamEnMM) - _deltaDesplazamiento;
            double largoDesarrollo_fueramuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM(diamEnMM) - _deltaDesplazamiento;

            pa = new XYZ(pb.X - largoDesarrollo_dentroMuro, pb.Y, pb.Z);
            pd = new XYZ(pc.X + largoDesarrollo_fueramuro, pb.Y, pb.Z);
        }
        //vulave a transformar coordenas a los valores originales
        private void VolverCoordenadasAnguloOrigianl()
        {
            pa = pa - new XYZ(0, 0, desplazamientoBArraAbajo);
            pb = pb - new XYZ(0, 0, desplazamientoBArraAbajo);
            pc = pc - new XYZ(0, 0, desplazamientoBArraAbajo);
            pd = pd - new XYZ(0, 0, desplazamientoBArraAbajo);
        }


        public void BuscarPatasAmbosLadosHorizontal(EmpotramientoPatasLosaDTO _empotramientoPatasDTO, View3D _view3D_paraBuscar)
        {
            IntersecionLosa TiposDeBarraPorInterseccion =
                new IntersecionLosa(_uipp, _view3D_paraBuscar, _empotramientoPatasDTO, pa, pd);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            ptoini_interseccion = TiposDeBarraPorInterseccion._ptoini;
            ptofinal_interseccion = TiposDeBarraPorInterseccion._ptofinal;
            tipoBarraRefuerzo = TiposDeBarraPorInterseccion.ResultTipoBarraRef;
        }

        public void IngresarPatasLadosHorizontal(EmpotramientoPatasLosaDTO _empotramientoPatasDTO, View3D _view3D_paraBuscar, TipoBarraRefuerzo tipoBarraRefuerzo)
        {
            IntersecionLosa TiposDeBarraPorInterseccion =
                new IntersecionLosa(_uipp, _view3D_paraBuscar, _empotramientoPatasDTO, pa, pd);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            ptoini_interseccion = TiposDeBarraPorInterseccion._ptoini;
            ptofinal_interseccion = TiposDeBarraPorInterseccion._ptofinal;
            this.tipoBarraRefuerzo = tipoBarraRefuerzo;
        }

        public CalculoBarraRefuerzo ObtenerbarraRefuerzoDTOCentralOriginal()
        {
            return new CalculoBarraRefuerzo() {
                AnguloBarra =AnguloBarra,
                AnguloBarraRad=AnguloBarraRad,
                diamEnMM=diamEnMM,
                IsOK=IsOK,
                IsUsar2pto=IsUsar2pto,
                Name=Name,
                pa=pa,
                pb = pb,
                pc = pc,
                pd = pd,
                pa_Orig = pa,
                pb_Orig = pb,
                pc_Orig = pc,
                pd_Orig = pd,
                _direcciMueveBarra=_direcciMueveBarra,
                tipoBarraRefuerzo= tipoBarraRefuerzo,
                ptoini_interseccion= ptoini_interseccion,
                ptofinal_interseccion= ptofinal_interseccion,
                _NormalCaraSuperiorLosa = _NormalCaraSuperiorLosa
            };
        }
    }
}
