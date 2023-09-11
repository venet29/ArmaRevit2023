using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.RebarLosa.Geom
{
    public class GenerarGeometriaAhorroMultiples
    {
        private readonly UIApplication _uiapp;
        private readonly BarraRoom _barraRoom;
        private View _view;
        private double espaciamiento;

        public List<XYZ> ListaPtosPerimetroBarras_ { get; set; }

        private double largo_recorridoFoot;
        private XYZ _p1;
        private XYZ _p2;
        private XYZ _p3;
        private XYZ _p4;
        private XYZ _dir_p1_p2;
        private XYZ _dir_p2_p3;
        private double largo_L3;

        public List<XYZ> ListaPtosPerimetroBarras1 { get; set; }
        public List<XYZ> ListaPtosPerimetroBarras2 { get; set; }


        private GenerarGeometriaSimple _GenerarGeometriaSimple1;
        private GenerarGeometriaSimple _GenerarGeometriaSimple2;
        private GenerarGeometriaAhorroDTO _GenerarGeometriaAhorroDTO;
        private double largo_recorridoFoot2;
        private double largo_recorridoFoot1;

        public GenerarGeometriaAhorroMultiples(UIApplication _uiapp, BarraRoom barraRoom)
        {
            this._uiapp = _uiapp;
            this._barraRoom = barraRoom;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            ListaPtosPerimetroBarras1 = new List<XYZ>();
            ListaPtosPerimetroBarras2 = new List<XYZ>();
        }

        public bool Ejecutar()
        {
            try
            {
                try
                {
                    _GenerarGeometriaAhorroDTO = _barraRoom.ObtenerGenerarGeometriaAhorroDTO();
                    if (_GenerarGeometriaAhorroDTO == null) return false;

                    espaciamiento = _GenerarGeometriaAhorroDTO._refereciaRoomDatos.Espaciamiento;
                    ListaPtosPerimetroBarras_ = _GenerarGeometriaAhorroDTO.ListaPtosPerimetroBarras;
                    largo_recorridoFoot = ListaPtosPerimetroBarras_[0].DistanceTo(ListaPtosPerimetroBarras_[1]);

                    GenerarPuntosCoordenadas();

                    _GenerarGeometriaSimple1 = new GenerarGeometriaSimple(_uiapp, _barraRoom);
                    if (!_GenerarGeometriaSimple1.Ejecutar(2,UbicacionPtoMouse.superior)) return false;


                    _GenerarGeometriaSimple2 = new GenerarGeometriaSimple(_uiapp, _barraRoom);
                    if (!_GenerarGeometriaSimple2.Ejecutar(2, UbicacionPtoMouse.inferior)) return false;

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al obtener datos en 'GenerarGeometriaAhorro'  ex:{ex.Message}");
                    return false;
                }


            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        private void GenerarPuntosCoordenadas()
        {
            _p1 = ListaPtosPerimetroBarras_[0];
            _p2 = ListaPtosPerimetroBarras_[1];
            _p3 = ListaPtosPerimetroBarras_[2];
            _p4 = ListaPtosPerimetroBarras_[3];
            _dir_p1_p2 = (_p1 - _p2).Normalize();
            _dir_p2_p3 = (_p2 - _p3).Normalize();
            largo_L3 = Util.CmToFoot(100);
        }

        public RebarInferiorDTO ObtenerGEometria1()
        {
            XYZ _p2_1 = _p2 + _dir_p2_p3 * 0 + _dir_p1_p2 * espaciamiento / 2;
            XYZ _p3_1 = _p3 + _dir_p2_p3 * largo_L3 + _dir_p1_p2 * espaciamiento / 2;
            XYZ _p4_1 = _p4 + _dir_p2_p3 * largo_L3;

            ListaPtosPerimetroBarras1.Add(_p1);
            ListaPtosPerimetroBarras1.Add(_p2_1);
            ListaPtosPerimetroBarras1.Add(_p3_1);
            ListaPtosPerimetroBarras1.Add(_p4_1);

            

            largo_recorridoFoot1 = ListaPtosPerimetroBarras1[0].DistanceTo(ListaPtosPerimetroBarras1[1]);

            List<XYZ> ListaPtosPerimetroBarras_paraREcorrido = new List<XYZ>();

            double deltaLargo = Util.ParteDecimal((largo_recorridoFoot1 / espaciamiento))*espaciamiento;
            XYZ _p1_1v2 = _p1  - _dir_p1_p2 * deltaLargo;
    
            XYZ _p4_1v2 = _p4 - _dir_p1_p2 * deltaLargo;
            ListaPtosPerimetroBarras1.Add(_p1_1v2);
            ListaPtosPerimetroBarras1.Add(_p2_1);
            ListaPtosPerimetroBarras1.Add(_p3_1);
            ListaPtosPerimetroBarras1.Add(_p4_1v2);


            ObtenerGEometriaDTO _ObtenerGEometriaDTO = new ObtenerGEometriaDTO()
            {
                ListaPtosPerimetroBarras = ListaPtosPerimetroBarras1,
                barraMenos = 0,
                usarPoligonoOriginal = true,
                ListaPtosPerimetroBarrasParaDimensiones = ListaPtosPerimetroBarras_
            };


            return _GenerarGeometriaSimple1.ObtenerGEometria(_ObtenerGEometriaDTO);

        }

        internal RebarInferiorDTO ObtenerGEometria2()
        {

            double restoEntero = largo_recorridoFoot - largo_recorridoFoot1;
            XYZ _p1_2 = _p1 - _dir_p2_p3 * largo_L3 - _dir_p1_p2 * espaciamiento / 2;
            XYZ _p2_2 = _p2 - _dir_p2_p3 * largo_L3 + _dir_p1_p2 * (espaciamiento + restoEntero / 2);
            XYZ _p3_2 = _p3 - _dir_p2_p3 * 0 + _dir_p1_p2 * (espaciamiento + restoEntero / 2);
            XYZ _p4_2 = _p4 - _dir_p1_p2 * espaciamiento / 2;


            ListaPtosPerimetroBarras2.Add(_p1_2);
            ListaPtosPerimetroBarras2.Add(_p2_2);
            ListaPtosPerimetroBarras2.Add(_p3_2);
            ListaPtosPerimetroBarras2.Add(_p4_2);

            largo_recorridoFoot2 = ListaPtosPerimetroBarras2[0].DistanceTo(ListaPtosPerimetroBarras2[1]);
            
            ObtenerGEometriaDTO _ObtenerGEometriaDTO = new ObtenerGEometriaDTO()
            {
                ListaPtosPerimetroBarras = ListaPtosPerimetroBarras2,
                barraMenos = 0,
                usarPoligonoOriginal = true,
                ListaPtosPerimetroBarrasParaDimensiones = ListaPtosPerimetroBarras_
            };

            return _GenerarGeometriaSimple2.ObtenerGEometria(_ObtenerGEometriaDTO);
        }
    }
}
