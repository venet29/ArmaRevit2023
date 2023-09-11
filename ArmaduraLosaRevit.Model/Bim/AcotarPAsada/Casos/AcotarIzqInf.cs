using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
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
    public class AcotarIzqInf : BaseAcotar, IAcotarCasos
    {

        public AcotarIzqInf(UIApplication uiapp, EnvoltoriPasada envoltoriPasada, DimensionType _dimensionType) : base(uiapp, envoltoriPasada, _dimensionType)
        {

        }



        public bool Ejecutar()
        {
            try
            {
                dim1Planar = _envoltoriPasada.parBordeParalelo1;

                //importante
                //direccion
                bool result = dim1Planar.M1_ObtenerHaciaIzq();

                if (!DibujarDimensionCentral(result)) return false;

                ListaPLanosPAsadas_aux = _envoltoriPasada.ListaPLanosPAsadas.OrderBy(c => c.Normal.X).ToList();
                _envoltoriosPlanos = ListaPLanosPAsadas_aux.FirstOrDefault();

                if (result)
                {
                    Line line_grid = default;
                    ReferenceArray ra_grid = new ReferenceArray();                    
                    //************************************
                    // dimensione ala grilla
                    XYZ ptoInter = _envoltoriosPlanos.refrenciaSeleccionado_Izq_Inf.ptoInterseccion;

                    line_grid = Line.CreateBound(P1.Redondear(4), ptoInter.Redondear(4));

                    _envoltoriosPlanos.refrenciaSeleccionado_Izq_Inf._EnvoltorioGrid.ObtenerReferencIA();
                    var ref1_grid = _envoltoriosPlanos.refrenciaSeleccionado_Izq_Inf._EnvoltorioGrid.Referencia_;
                    var ref2_grid = dim1Planar.face_referencia_IzqInf.Reference;

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
                            dimension1_grid.TextPosition = dimension1_grid.TextPosition + -_envoltoriosPlanos.Direccion_ * DesplazamientoDimExtremo_foot;
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
