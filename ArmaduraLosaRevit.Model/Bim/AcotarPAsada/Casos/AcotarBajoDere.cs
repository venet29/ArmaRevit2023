using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Casos
{
    public class AcotarBajoDere : BaseAcotar, IAcotarCasos
    {

        public AcotarBajoDere(UIApplication uiapp, EnvoltoriPasada envoltoriPasada, DimensionType _dimensionType) : base(uiapp, envoltoriPasada, _dimensionType)
        {
            DesplazarDimDelBorde = DesplazarDimDelBorde_Inferio_Dere;
        }

        public bool Ejecutar()
        {
            try
            {
                dim1Planar = _envoltoriPasada.parBordeParalelo1;
                //importante
                //direccion
                bool result = dim1Planar.M3_ObtenerHaciaInferior();

                if (!DibujarDimensionCentral(result)) return false;


                ListaPLanosPAsadas_aux = _envoltoriPasada.ListaPLanosPAsadas.OrderBy(c => c.Normal.Y).ToList();
                _envoltoriosPlanos = ListaPLanosPAsadas_aux.FirstOrDefault();

                //mover  dimension de dimension central en caso de ser pequeña
                if (dimension1 != null)
                {
                    if (_line_dimensionCentral.Length < LargoComprobarLargoMinimo_foot)
                        dimension1.TextPosition = dimension1.TextPosition + _envoltoriosPlanos.Direccion_ * DesplazamientoDimExtremo_foot + _envoltoriPasada.DireccionFAceNormalInferior * DesplazamientoDimCentral_foot;
                }


                if (result)
                {
                    Line line_grid = default;
                    ReferenceArray ra_grid = new ReferenceArray();
                    //************************************
                    // dimensione ala grilla
                    XYZ ptoInter = _envoltoriosPlanos.refrenciaSeleccionado_Arriba_Dere.ptoInterseccion;

                    //if (Util.GetProductoEscalar((P2.GetXY0() - P1.GetXY0()).Normalize(), (ptoInter.GetXY0() - P1.GetXY0()).Normalize()) > 0.8)
                    //    line_grid = Line.CreateBound(ptoInter.Redondear(6), P2.Redondear(6));
                    //else
                    //    line_grid = Line.CreateBound(P1.Redondear(4), ptoInter.Redondear(4));

                    line_grid = Line.CreateBound(ptoInter.Redondear(6), P2.Redondear(6));

                    _envoltoriosPlanos.refrenciaSeleccionado_Arriba_Dere._EnvoltorioGrid.ObtenerReferencIA();
                    var ref1_grid = _envoltoriosPlanos.refrenciaSeleccionado_Arriba_Dere._EnvoltorioGrid.Referencia_;
                    var ref2_grid = dim1Planar.face_referencia_DereSup.Reference;

                    ra_grid.Append(ref1_grid);
                    ra_grid.Append(ref2_grid);
                    dimension1_grid = _doc.Create.NewDimension(_doc.ActiveView, line_grid, ra_grid, _dimensionType);

          

                    XYZ moverDirec2 = dimension1_grid.Origin;

                    if (dimension1_grid != null)
                        ElementTransformUtils.MoveElement(_doc, dimension1_grid.Id, dim1Planar.face_analizada.FaceNormal.Normalize() * DesplazarDimDelBorde);

                    moverDirec2 = moverDirec2 - dimension1_grid.Origin;

                    if (dimension1_grid != null)
                    {
                        ElementTransformUtils.MoveElement(_doc, dimension1_grid.Id, dim1Planar.face_analizada.FaceNormal.Normalize() * DesplazarDimDelBorde + moverDirec2);

                        if (line_grid.Length < LargoComprobarLargoMinimo_foot)
                            dimension1_grid.TextPosition = dimension1_grid.TextPosition + _envoltoriosPlanos.Direccion_ * DesplazamientoDimExtremo_foot;
                    }
                }

            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al obtener dimeniones  Izq-Dere");
                return false;
            }

            return true;
        }
    }
}
