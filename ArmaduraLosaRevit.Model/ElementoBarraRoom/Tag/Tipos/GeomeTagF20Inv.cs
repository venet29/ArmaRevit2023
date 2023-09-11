using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF20Inv : IGeometriaTag
    {
        private  Document _doc;
        private  XYZ _ptoMOuse;
        private View _view;
        private  List<XYZ> _listaPtosPerimetroBarras;
        private  SolicitudBarraDTO _solicitudBarraDTO;

        public List<TagBarra> listaTag { get; set; }

        GeomeTagF4 _geomeTagF4_Inferior;
        GeomeTagF4 _geomeTagF4_Superior;
        private double anguloBarra_solocasoF20;

        public GeomeTagF20Inv(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
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
                Util.ErrorMsg($"Error ejecutar TagF20Inv  ex:${ex.Message}");
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

                anguloBarra_solocasoF20 = (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Horizontal ? anguloRoomRad : anguloRoomRad + Util.GradosToRadianes(90));


                XYZ ptoMOuse_Inferior = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra_solocasoF20 + Util.GradosToRadianes(90), Util.CmToFoot(20) + deltaFActoInf, 0);
                _geomeTagF4_Superior = new GeomeTagF4(_doc, ptoMOuse_Inferior, _listaPtosPerimetroBarras, _solicitudBarraDTO);
                if (!_geomeTagF4_Superior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;


                XYZ ptoMOuse_supero = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra_solocasoF20 - Util.GradosToRadianes(90), Util.CmToFoot(20)+ deltaFActoSUP, 0);
                _geomeTagF4_Inferior = new GeomeTagF4(_doc, ptoMOuse_supero, _listaPtosPerimetroBarras, _solicitudBarraDTO);
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
            _geomeTagF4_Inferior.M2_CAlcularPtosDeTAg();
            _geomeTagF4_Superior.M2_CAlcularPtosDeTAg();
        }

        public void M3_DefinirRebarShape()
        {

            GeomeTagF1Alterna _GeomeTagF1Alterna = new GeomeTagF1Alterna(_doc, _solicitudBarraDTO);
            _GeomeTagF1Alterna.Ejecutar(new GeomeTagArgs() { angulorad = anguloBarra_solocasoF20 });
            _geomeTagF4_Inferior._ubicacionEnlosa = _geomeTagF4_Inferior._ubicacionEnlosa;// M3_1_InvertirUbicacionEnLOsa_F3Superior(_geomeTagF3_Superior._ubicacionEnlosa);
            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(_GeomeTagF1Alterna.AsignarPArametros);


            GeomeTagF1 geomeTagF1 = new GeomeTagF1();

            _geomeTagF4_Superior._ubicacionEnlosa = _geomeTagF4_Superior._ubicacionEnlosa;// M3_2_InvertirUbicacionEnLOsa_F11Inferior(_geomeTagF4_Inferior._ubicacionEnlosa);
            geomeTagF1._ubicacionEnlosa = _geomeTagF4_Superior._ubicacionEnlosa;
            _geomeTagF4_Superior.M5_DefinirRebarShapeAhorro(geomeTagF1.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
            listaTag.AddRange(_geomeTagF4_Superior.listaTag);
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
                    return UbicacionLosa.Izquierda;
                case UbicacionLosa.Izquierda:
                    return UbicacionLosa.Derecha;
                case UbicacionLosa.Superior:
                    return UbicacionLosa.Inferior;
                case UbicacionLosa.Inferior:
                    return UbicacionLosa.Superior;
                default:
                    return UbicacionLosa.NONE;
            }
        }

        public bool M4_IsFAmiliaValida() => true;




    }
}
