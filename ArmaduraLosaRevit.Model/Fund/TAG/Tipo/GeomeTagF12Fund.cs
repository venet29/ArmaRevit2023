using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF12Fund : GeomeTagBaseFund, IGeometriaTag
    {
        private readonly PathReinfSeleccionDTO _pathReinfSeleccionDTO;

        public GeomeTagF12Fund(Document doc, XYZ ptoMOuse, PathReinfSeleccionDTO _PathReinfSeleccionDTO, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, _PathReinfSeleccionDTO.ListaPtosPerimetroBarras, _solicitudBarraDTO)
        {
            _pathReinfSeleccionDTO = _PathReinfSeleccionDTO;
        }

        public GeomeTagF12Fund() { }

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
                Util.ErrorMsg($"Error ejecutar TagF12Fund  ex:${ex.Message}");
                return false;
            }
            return true;
        }

        public void M3_DefinirRebarShape()
        {
            listaTag.RemoveAll(c => c.nombre == "Ffund");
            //directriz funcion pero no queda bonito, el signo negativo es necesario
            // XYZ newPto = -(pathReinfSeleccionDTO.ptoConMouse - pathReinfSeleccionDTO.PtoOriegenDireztriz).AsignarZ(0);
            XYZ newPto = ObtenerPtoTagConDirectriz(_pathReinfSeleccionDTO.PtoDireccionDireztriz, _pathReinfSeleccionDTO.PtoCodoDireztriz);
            TagP0_F = M1_1_ObtenerTAgBarra(newPto, "Ffund", nombreDefamiliaBase + "_Ffund_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            TagP0_F.IsDIrectriz = true;

            TagP0_F.PtoCodo_LeaderElbow = _pathReinfSeleccionDTO.PtoCodoDireztriz;
            listaTag.Add(TagP0_F);

            TagP0_F.IsLibre = _pathReinfSeleccionDTO.IsLadoLibre;
            TagP0_F.Ptocodo_LeaderEnd = _pathReinfSeleccionDTO.PtoLadoLibre;

            // AgregarTAgLitsta("L", 60, 0, newPto,false,null);

            AsignarPArametros(this);
        }

        private XYZ ObtenerPtoTagConDirectriz(XYZ ptoDireccionDireztriz, XYZ PtoCodoDireztriz)
        {
            XYZ NuevoPtoTAg = XYZ.Zero;
            try
            {
                XYZ direccionBarra_ = (_listaPtosPerimetroBarras[2].GetXY0() - _listaPtosPerimetroBarras[1].GetXY0()).Normalize();
                XYZ direccionRecoorrido_ = (_listaPtosPerimetroBarras[0].GetXY0() - _listaPtosPerimetroBarras[1].GetXY0()).Normalize();

                Line line_direccionBarras = Line.CreateBound(_listaPtosPerimetroBarras[1].GetXY0(), _listaPtosPerimetroBarras[2].GetXY0());
                XYZ ptoDireccionDireztriz_proj = line_direccionBarras.ProjectExtendidaXY0(ptoDireccionDireztriz.GetXY0());
                XYZ ptoOriegenDireztriz_proj = line_direccionBarras.ProjectExtendidaXY0(PtoCodoDireztriz.GetXY0());

                XYZ DIreccionDiretriz_2 = (ptoDireccionDireztriz_proj - ptoOriegenDireztriz_proj).Normalize();

                double factorEscala = 1.0D;
                if (_view.Scale == 75)
                    factorEscala = 75.0D / 50.0D;
                else if (_view.Scale == 100)
                    factorEscala = 100.0 / 50.0f;

                XYZ desplazamieto = direccionRecoorrido_ * Util.CmToFoot(-0.910719328613576 * factorEscala);
                if (Util.IsSimilarValor(_anguloBArraGrado, 0, 0.001))
                {
                    if (Util.GetProductoEscalar(direccionBarra_, DIreccionDiretriz_2) > 0)
                        NuevoPtoTAg = PtoCodoDireztriz + direccionBarra_ * FactoresLargoLeader.FactorDesplazaminetoTag_foot * factorEscala + desplazamieto;
                    else
                        NuevoPtoTAg = PtoCodoDireztriz - direccionBarra_ * FactoresLargoLeader.FactorDesplazaminetoTag_foot * factorEscala + desplazamieto;
                }
                else
                    NuevoPtoTAg = _pathReinfSeleccionDTO.PtoTag;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'ObtenerPtoTagConDirectriz'  ex: {ex.Message}");
            }
            return NuevoPtoTAg;
        }

        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBaseFund _geomeTagBase)
        {
            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_B.IsOk = false;
            _geomeTagBase.TagP0_L.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;
        }
    }
}
