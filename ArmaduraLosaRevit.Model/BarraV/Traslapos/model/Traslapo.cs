using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos.model
{
    public class TraslapoDTO
    {
       

        public Rebar RebarInicial { get; set; }
        public Rebar RebarFinal { get; set; }


        public XYZ ptoIni { get; set; }
        public XYZ ptoFin { get; set; }
        public XYZ ptoInicial { get; private set; }

        #region metodos para obtener datos de dimension

        public Reference RebarInicialRef { get; set; }
        public int Id_RebarInicialRef { get; set; }
        public XYZ ptoFinal { get; private set; }
        public Reference RebarFinalRef { get; set; }
        public int Id_RebarFinalRef { get; set; }
        public Line curva { get; private set; }

        #endregion
        public bool IsOK { get; set; } = false;
        public Dimension dimension { get; private set; }

        public TraslapoDTO()
        {
           
        }
        public static TraslapoDTO Create(Dimension item)
        {


            var newTraslapoDTO = new TraslapoDTO();
            try
            {
                ReferenceArray refs = item.References;
                if (refs.IsEmpty) return newTraslapoDTO;
                newTraslapoDTO.RebarInicialRef = refs.get_Item(1);
                newTraslapoDTO.Id_RebarInicialRef = newTraslapoDTO.RebarInicialRef.ElementId.IntegerValue;
                newTraslapoDTO.RebarFinalRef = refs.get_Item(0);
                newTraslapoDTO.Id_RebarFinalRef= newTraslapoDTO.RebarFinalRef.ElementId.IntegerValue;
                newTraslapoDTO.curva = (Line)item.Curve;
                if (newTraslapoDTO.curva.Reference == null)

                {
                    var pt2 = item.Origin + ((Line)item.Curve).Direction * (double)item.Value;
                    newTraslapoDTO.curva = Line.CreateBound(item.Origin, pt2);
                }
                
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener ''TraslapoDTO' de Dimension id:{item.Id}.\n ex:{ex.Message}");
                return new TraslapoDTO() { IsOK = false };
            }

            newTraslapoDTO.IsOK = true;
            return newTraslapoDTO;
        }


        public bool M1_ObtenerREfIncial(View _view)
        {
            try
            {
                IsOK = false;
                Line rebarSeg1 = null;
                bool bOk = RebarInicial.getRebarSegmentMasLArgo(out rebarSeg1);
                if (!bOk)
                    return false;

                ptoInicial = RebarInicial.ObtenerInicioCurvaMasLarga();
                RebarInicialRef = RebarInicial.getReferenceForPointOfBar(_view, rebarSeg1, 1);
                if (RebarInicialRef == null) return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }

            return IsOK = true;
        }

        public bool M2_ObtenerREfFinal(View _view)
        {
            try
            {
                IsOK = false;
                Line rebarSeg2 = null;
                bool bOk2 = RebarFinal.getRebarSegmentMasLArgo(out rebarSeg2);
                if (!bOk2)
                    return false;
                ptoFinal = RebarInicial.ObtenerFinCurvaMasLarga();
                RebarFinalRef = RebarFinal.getReferenceForStartOfBar(_view, rebarSeg2);
                if (RebarFinalRef == null) return false; ;
            }
            catch (Exception)
            {
                return false;
            }
            IsOK = true;
            return true;

        }

        public bool M3_ObtenerPtos(View _view)
        {
            try
            {
                IsOK = false;
                XYZ pt2 = RebarInicial.ObtenerFinCurvaMasLarga();
                XYZ pt1 = RebarFinal.ObtenerInicioCurvaMasLarga();

              

                XYZ direccion = (pt1 - pt2).Normalize();

                pt1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, pt1);
                pt2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, pt2);

                ptoIni = pt1;
                ptoFin = pt2;

                double distancia = pt1.GetXY0().DistanceTo(pt2.GetXY0());
                double diamInic = RebarInicial.ObtenerDiametroFoot();
                double diamFin = RebarFinal.ObtenerDiametroFoot();
                if (distancia > Math.Max(diamFin, diamInic))
                    return false;

            }
            catch (Exception)
            {
                return false;
            }
            IsOK = true;
            return true;

        }

        public bool M4_CrearDimension_ConTrans(Document _doc, DimensionType _dimensionType)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar DimensionTraslapo");

                    Line line = default;
                    ReferenceArray ra = new ReferenceArray();
                    if (curva != null)
                        line = (Line)curva;
                    else
                        line = Line.CreateBound(ptoIni, ptoFin);
                    ra.Append(RebarInicialRef);
                    ra.Append(RebarFinalRef);
                    dimension = _doc.Create.NewDimension(_doc.ActiveView, line, ra, _dimensionType);

                    t.Commit();
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'crear dimension' de Dimension .\n ex:{ex.Message}");
                return IsOK = false;
            }
            return IsOK = true;
        }

        public bool M4_CrearDimension_SinTrans(Document _doc, DimensionType _dimensionType)
        {

            try
            {
                Line line = default;
                ReferenceArray ra = new ReferenceArray();
                if (curva != null)
                    line = (Line)curva;
                else
                    line = Line.CreateBound(ptoIni, ptoFin);
                ra.Append(RebarInicialRef);
                ra.Append(RebarFinalRef);
                dimension = _doc.Create.NewDimension(_doc.ActiveView, line, ra, _dimensionType);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'crear dimension' de Dimension .\n ex:{ex.Message}");
                return IsOK = false;
            }
            return IsOK = true;
        }

    }
}
