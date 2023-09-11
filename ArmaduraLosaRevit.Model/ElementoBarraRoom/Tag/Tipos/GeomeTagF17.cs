using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF17 : IGeometriaTag
    {
        private readonly Document _doc;
        private readonly XYZ _ptoMOuse;
        private View _view;
        private readonly List<XYZ> _listaPtosPerimetroBarras;
        private readonly SolicitudBarraDTO _solicitudBarraDTO;

        public List<TagBarra> listaTag { get; set; }

        GeomeTagF3 _geomeTagF3_Superior;
        GeomeTagF11A _geomeTagF4_Inferior;
        private double anguloBarra_solocasoF17;

        public GeomeTagF17(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
        {
            this._doc = doc;
            this._ptoMOuse = ptoMOuse;
            this._view = doc.ActiveView;
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            listaTag = new List<TagBarra>();

        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF17   ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {
                Ayuda_deltaFacto_GeomeTag.calcular(_view);
                double deltaFActoSUP = Ayuda_deltaFacto_GeomeTag.deltaFActoSUP;
                double deltaFActoInf = Ayuda_deltaFacto_GeomeTag.deltaFActoInf;

                anguloBarra_solocasoF17 = (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Horizontal ? anguloRoomRad : anguloRoomRad + Util.GradosToRadianes(90));

                XYZ ptoMOuse_supero = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra_solocasoF17 + Util.GradosToRadianes(90), Util.CmToFoot(20)+ deltaFActoSUP, 0);
                _geomeTagF3_Superior = new GeomeTagF3(_doc, ptoMOuse_supero, _listaPtosPerimetroBarras, _solicitudBarraDTO);
                if (!_geomeTagF3_Superior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;

                XYZ ptoMOuse_Inferior = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra_solocasoF17 - Util.GradosToRadianes(90), Util.CmToFoot(20)+ deltaFActoInf, 0);
                _geomeTagF4_Inferior = new GeomeTagF11A(_doc, ptoMOuse_Inferior, _listaPtosPerimetroBarras, _solicitudBarraDTO);
                if (!_geomeTagF4_Inferior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtnerPtosInicialYFinalDeBarra' en { this.GetType().Name}.   ex:{ex.Message} ");
                return false;
            }
            return true; 
        }

        public void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false)
        {
            _geomeTagF3_Superior.M2_CAlcularPtosDeTAg();
            _geomeTagF4_Inferior.M2_CAlcularPtosDeTAg();
        }

        public void M3_DefinirRebarShape()
        {
            // SolicitudBarraDTO _newsolicitudBarraDTO = new SolicitudBarraDTO() { nombreDefamiliaBase = _solicitudBarraDTO.nombreDefamiliaBase };
            GeomeTagF3Alterna _GeomeTagF3Alterna = new GeomeTagF3Alterna(_doc, _solicitudBarraDTO) { };
            _GeomeTagF3Alterna.Ejecutar(new GeomeTagArgs() { angulorad = anguloBarra_solocasoF17 });
            _geomeTagF3_Superior._ubicacionEnlosa = M3_1_InvertirUbicacionEnLOsa_F3Superior(_geomeTagF3_Superior._ubicacionEnlosa);
            _geomeTagF3_Superior.M5_DefinirRebarShapeAhorro(_GeomeTagF3Alterna.AsignarPArametros);


            GeomeTagF11A geomeTagF11 = new GeomeTagF11A();

            _geomeTagF4_Inferior._ubicacionEnlosa = M3_2_InvertirUbicacionEnLOsa_F11Inferior(_geomeTagF4_Inferior._ubicacionEnlosa);
            geomeTagF11._ubicacionEnlosa = _geomeTagF4_Inferior._ubicacionEnlosa;
            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(geomeTagF11.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF3_Superior.listaTag);
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
        }

        private UbicacionLosa M3_1_InvertirUbicacionEnLOsa_F3Superior(UbicacionLosa _ubicacionEnlosa)
        {
            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Derecha:
                case UbicacionLosa.Izquierda:
                    return UbicacionLosa.Izquierda;
                case UbicacionLosa.Superior:
                case UbicacionLosa.Inferior:
                    return UbicacionLosa.Inferior;
                default:
                    return UbicacionLosa.NONE;
            }
        }
        private UbicacionLosa M3_2_InvertirUbicacionEnLOsa_F11Inferior(UbicacionLosa _ubicacionEnlosa)
        {

            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Derecha:
                    return UbicacionLosa.Derecha;
                case UbicacionLosa.Izquierda:
                    return UbicacionLosa.Izquierda;
                case UbicacionLosa.Superior:
                    return UbicacionLosa.Superior;
                case UbicacionLosa.Inferior:
                    return UbicacionLosa.Inferior;
                default:
                    return UbicacionLosa.NONE;
            }
        }

        public bool M4_IsFAmiliaValida() => true;




    }
}
