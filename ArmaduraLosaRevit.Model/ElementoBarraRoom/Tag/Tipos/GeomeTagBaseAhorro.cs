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
    public class GeomeTagBaseAhorro : IGeometriaTag
    {
        protected readonly Document _doc;
        private View _view;
        protected readonly XYZ _ptoMOuse;
        protected readonly List<XYZ> _listaPtosPerimetroBarras;
        protected readonly SolicitudBarraDTO _solicitudBarraDTO;

        public List<TagBarra> listaTag { get; set; }

        protected GeomeTagF4 _geomeTagF4_Superior;
        protected GeomeTagF4 _geomeTagF4_Inferior;

        public GeomeTagBaseAhorro(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
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
            if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagBaseAhorro  ex:${ex.Message}");
                return false;
            }
            return true;

        }


        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            try
            {

                string scala = _view.ObtenerNombre_EscalaConfiguracion().ToString();

                double desfase = 20;
                if (scala == "50")
                { desfase = 20; }
                else if (scala == "75")
                { desfase = 13; }
                else if (scala == "100")
                { desfase = 10; }

                double anguloBarra = (_solicitudBarraDTO.TipoOrientacion == TipoOrientacionBarra.Horizontal ? anguloRoomRad : anguloRoomRad + Util.GradosToRadianes(90));
                XYZ ptoMOuse_supero = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra + Util.GradosToRadianes(90), Util.CmToFoot(desfase), 0);
                _geomeTagF4_Superior = new GeomeTagF4(_doc, ptoMOuse_supero, _listaPtosPerimetroBarras, _solicitudBarraDTO);
                if (!_geomeTagF4_Superior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;

                XYZ ptoMOuse_Inferior = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMOuse, anguloBarra - Util.GradosToRadianes(90), Util.CmToFoot(desfase), 0);
                _geomeTagF4_Inferior = new GeomeTagF4(_doc, ptoMOuse_Inferior, _listaPtosPerimetroBarras, _solicitudBarraDTO);
                if(_geomeTagF4_Inferior.M1_ObtnerPtosInicialYFinalDeBarra(anguloRoomRad)) return false;

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
            _geomeTagF4_Superior.M2_CAlcularPtosDeTAg();
            _geomeTagF4_Inferior.M2_CAlcularPtosDeTAg();
        }

        public virtual void M3_DefinirRebarShape()
        {

            GeomeTagF3 geomeTagF3 = new GeomeTagF3();
            _geomeTagF4_Superior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);

            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF4_Superior.listaTag);
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
        }

        public bool M4_IsFAmiliaValida() => true;
    }
}
