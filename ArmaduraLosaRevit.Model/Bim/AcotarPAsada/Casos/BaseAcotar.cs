using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public class BaseAcotar
    {
        protected double DesplazarDimDelBorde;
        protected double DesplazarDimDelBorde_Inferio_Dere;

        protected double LargoComprobarLargoMinimo_foot = ConstBim.CONST_LargoComprobarLargoMinimo_foot;
        protected double DesplazamientoDimExtremo_foot = ConstBim.CONST_DesplazamientoDimExtremo_foot;
        protected double DesplazamientoDimCentral_foot = ConstBim.CONST_DesplazamientoDimCentral_foot;

        protected UIApplication _uiapp;
        protected EnvoltoriPasada _envoltoriPasada;
        protected DimensionType _dimensionType;
        protected Document _doc;
        protected View _view;
        protected ParBordeParalelo dim1Planar;
        protected EnvoltoriosPlanos _envoltoriosPlanos;

        protected Dimension dimension1;
        protected Dimension dimension1_grid = default;
        protected XYZ P1;
        protected XYZ P2;
        protected Line _line_dimensionCentral;

        protected List<EnvoltoriosPlanos> ListaPLanosPAsadas_aux { get; set; }
        public BaseAcotar(UIApplication uiapp, EnvoltoriPasada envoltoriPasada, DimensionType _dimensionType)
        {
            this._uiapp = uiapp;
            this._envoltoriPasada = envoltoriPasada;
            this._dimensionType = _dimensionType;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.DesplazarDimDelBorde = Util.CmToFoot(20);
            DesplazarDimDelBorde_Inferio_Dere = Util.CmToFoot(45);
        }



        public bool DibujarDimensionCentral(bool result)
        {
            try
            {
                if (!result) return false;

                P1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.Pt1);
                P2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.Pt2);

                (XYZ p1_, XYZ p2_) = Util.Ordena2PtosV2( P1, P2);
                P1 = p1_.ObtenerCopia();
                P2 = p2_.ObtenerCopia();

                // grillas central
                if (result)
                {
                

                    ReferenceArray ra = new ReferenceArray();

                    _line_dimensionCentral = Line.CreateBound(P1.Redondear(6), P2.Redondear(6));
                    var ref1 = dim1Planar.face_referencia_IzqInf.Reference;
                    var ref2 = dim1Planar.face_referencia_DereSup.Reference;

                    ra.Append(ref1);
                    ra.Append(ref2);
                    dimension1 = _doc.Create.NewDimension(_doc.ActiveView, _line_dimensionCentral, ra, _dimensionType);

                    if (dimension1 != null)
                        ElementTransformUtils.MoveElement(_doc, dimension1.Id, dim1Planar.face_analizada.FaceNormal * DesplazarDimDelBorde);
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
