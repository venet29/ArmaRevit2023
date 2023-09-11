using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public class CalcularLargoTraslapoPAthDTO
    {
        public double largopathFoot;

        public XYZ puntoSeleccionMouse { get; set; }
        public TipoPAthDefinir pathDefinir { get; set; }
        public bool IsDefinirLargo { get; set; }
        public double LargoTraslapo { get; set; }
        public CalcularLargoTraslapoPAthDTO()
        {
            this.IsDefinirLargo = false;
        }


    }
    // clase para redefinir pto seleccion mouse para obteneer largo predefido de path inicial o final
    public class CalcularLargoPAth
    {
        private CoordenadaPath _coordCalculos;
        private readonly CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO;
        private XYZ _puntoSeleccionMouse;
        public XYZ _NuevoPuntoSeleccionMouse { get; internal set; }
        private TipoPAthDefinir _pathDefinir;
        private double _largopath;
        private Line linePA_Pb;
        private XYZ _Pa;
        private XYZ _Pb;

        public CalcularLargoPAth(CoordenadaPath coordenadaPath, CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO)
        {
            this._coordCalculos = coordenadaPath;
            this._CalcularLargoPAthDTO = _CalcularLargoPAthDTO;
            _puntoSeleccionMouse = _CalcularLargoPAthDTO.puntoSeleccionMouse;
            _NuevoPuntoSeleccionMouse = _puntoSeleccionMouse;
            _pathDefinir = _CalcularLargoPAthDTO.pathDefinir;
            _largopath = _CalcularLargoPAthDTO.largopathFoot;
        }



        internal bool Calcular()
        {
            if (!_CalcularLargoPAthDTO.IsDefinirLargo) return false;
            try
            {


                //crar linea en sentido de barra
                if (Util.IsIntersection(_coordCalculos.p1, _coordCalculos.p3,
                                        _coordCalculos.p2, _coordCalculos.p4))
                {
                    //  1 --  pa --4
                    //  2 --   pb--3
                    XYZ _direc = (_coordCalculos.p4 - _coordCalculos.p1).Normalize();
                    if (_pathDefinir == TipoPAthDefinir.PathInicial)
                    {
                        _Pa = _coordCalculos.p1 + _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                        _Pb = _coordCalculos.p2 + _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                    }
                    else if (_pathDefinir == TipoPAthDefinir.PathFinal)
                    {
                        _Pa = _coordCalculos.p4 - _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                        _Pb = _coordCalculos.p3 - _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                    }
                    else if (_pathDefinir == TipoPAthDefinir.Mitad)
                    {
                        double largoPAth = _coordCalculos.p1.DistanceTo(_coordCalculos.p4);
                        _Pa = _coordCalculos.p4 - _direc * (largoPAth / 2);
                        _Pb = _coordCalculos.p3 - _direc * (largoPAth / 2);
                    }
                }
                else
                {
                    //  1 --pa-  3
                    //  2 -- pb--4
                    XYZ _direc = (_coordCalculos.p3 - _coordCalculos.p1).Normalize();

                    if (_pathDefinir == TipoPAthDefinir.PathInicial)
                    {
                        _Pa = _coordCalculos.p1 + _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                        _Pb = _coordCalculos.p2 + _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                    }
                    else if (_pathDefinir == TipoPAthDefinir.PathFinal)
                    {
                        _Pa = _coordCalculos.p3 - _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                        _Pb = _coordCalculos.p4 - _direc * (_largopath - _CalcularLargoPAthDTO.LargoTraslapo / 2);
                    }
                    else if (_pathDefinir == TipoPAthDefinir.Mitad)
                    {
                        double largoPAth = _coordCalculos.p1.DistanceTo(_coordCalculos.p4);
                        _Pa = _coordCalculos.p4 - _direc * (largoPAth / 2);
                        _Pb = _coordCalculos.p3 - _direc * (largoPAth / 2);
                    }
                }

                linePA_Pb = Line.CreateBound(_Pa, _Pb);
                IntersectionResult ptoInterseccionLineSentidoBarraInical = linePA_Pb.Project(_puntoSeleccionMouse);

                if (ptoInterseccionLineSentidoBarraInical == null)
                    Util.ErrorMsg("Error al seleccionar ptoInterIni(p4)");
                else
                {
                    _NuevoPuntoSeleccionMouse = ptoInterseccionLineSentidoBarraInical.XYZPoint.AsignarZ(_puntoSeleccionMouse.Z);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M1_1_ObtenerLineasParalelasBarra' ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
