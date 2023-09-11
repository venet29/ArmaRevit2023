using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF16 : IGeometriaTag
    {
        private  Document _doc;
        private View _view;
        private  XYZ _ptoMOuse;
        private  List<XYZ> _listaPtosPerimetroBarras;
        private  SolicitudBarraDTO _solicitudBarraDTO;

        public List<TagBarra> listaTag { get; set; }

        GeomeTagF3 _geomeTagF3_Superior;
        GeomeTagF4 _geomeTagF4_Inferior;
        private double anguloBarra;

        public GeomeTagF16(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
        {
            this._doc = doc;
            this._view = doc.ActiveView;
            this._ptoMOuse = ptoMOuse;
            this._listaPtosPerimetroBarras = listaPtosPerimetroBarras;
            this._solicitudBarraDTO = _solicitudBarraDTO;
            listaTag = new List<TagBarra>();

        }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF16  ex:${ex.Message}");
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


                anguloBarra = (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Horizontal ? anguloRoomRad : anguloRoomRad + Util.GradosToRadianes(90));
                XYZ ptoMOuse_supero = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra + Util.GradosToRadianes(90), Util.CmToFoot(20)+ deltaFActoSUP, 0);
                _geomeTagF3_Superior = new GeomeTagF3(_doc, ptoMOuse_supero, _listaPtosPerimetroBarras, _solicitudBarraDTO);
               if(!_geomeTagF3_Superior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;

                XYZ ptoMOuse_Inferior = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra - Util.GradosToRadianes(90), Util.CmToFoot(20)+ deltaFActoInf, 0);
                _geomeTagF4_Inferior = new GeomeTagF4(_doc, ptoMOuse_Inferior, _listaPtosPerimetroBarras, _solicitudBarraDTO);
               if(!_geomeTagF4_Inferior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtnerPtosInicialYFinalDeBarra' en { this.GetType().Name}.   ex:{ex.Message} ");
                return false;
            }
            return false;
        }

        public void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false)
        {
            _geomeTagF3_Superior.M2_CAlcularPtosDeTAg();
            _geomeTagF4_Inferior.M2_CAlcularPtosDeTAg();
        }

        public void M3_DefinirRebarShape()
        {


            GeomeTagF3Alterna _GeomeTagF3Alterna = new GeomeTagF3Alterna(_doc, _solicitudBarraDTO) { };
            _GeomeTagF3Alterna.Ejecutar(new GeomeTagArgs() { angulorad = anguloBarra });
            // _geomeTagF3_Superior._ubicacionEnlosa = M3_1_InvertirUbicacionEnLOsa_F3Superior(_geomeTagF3_Superior._ubicacionEnlosa);
            _geomeTagF3_Superior.M5_DefinirRebarShapeAhorro(_GeomeTagF3Alterna.AsignarPArametros);



            GeomeTagF3 geomeTagF3 = new GeomeTagF3();
            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF3_Superior.listaTag);
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
        }

        public bool M4_IsFAmiliaValida() => true;
    }
}
