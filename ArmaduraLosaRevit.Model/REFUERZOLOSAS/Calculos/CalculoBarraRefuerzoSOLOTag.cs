
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Interseccion;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    //pts para trabajr con ls trasformadas
    //entonces siempre seran horizontales
    public class CalculoBarraRefuerzoSOLOTag
    {


        private readonly UIApplication _uiapp;
        public string Largoparciales { get; set; }
        public double LargoTotal { get; set; }

        List<BarrasRefSoloTagDTO> listaLargos;
        private Rebar _rebar;

        public CalculoBarraRefuerzoSOLOTag(UIApplication _uipp, SeleccionarRebarElemento seleccionarRebarElemento)
        {
            this._uiapp = _uipp;
            this._rebar = seleccionarRebarElemento._RebarSeleccion;
            listaLargos = new List<BarrasRefSoloTagDTO>();
        }


        public void ObtenerTipo()
        {
            RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
            if (rebarShapeDrivenAccessor == null) return;


            if (!AyudaCurveRebar.GetPrimeraRebarCurves(_rebar))
            {
                Util.ErrorMsg("Error al obtener largos parciales de barra");
                return;
            }
            List<Curve> listaCurva = AyudaCurveRebar.ListacurvesSoloLineas[0];

            //List<Curve> listaCurva = (List<Curve>)rebarShapeDrivenAccessor.ComputeDrivingCurves();
            if (listaCurva == null) return;

            Largoparciales = "";
            LargoTotal = 0;

            foreach (var item in listaCurva)
            {

            }

            if (listaCurva.Count > 1)
            {
                foreach (Curve item in listaCurva)
                {

                    BarrasRefSoloTagDTO barrasRefSoloTagDTO = new BarrasRefSoloTagDTO()
                    {
                        largo = item.Length,
                        P1 = item.GetPoint2(0),
                        P2 = item.GetPoint2(1),
                        posicion = GetPosicion()

                    };

                    listaLargos.Add(barrasRefSoloTagDTO);

                    if (Largoparciales == "")
                    { Largoparciales = Largoparciales + "" + Math.Round(Util.FootToCm(item.Length), 0); }
                    else
                    { Largoparciales = Largoparciales + "+" + Math.Round(Util.FootToCm(item.Length), 0); }

                    LargoTotal += Util.FootToCm(item.Length);
                }

            }
            else if (listaCurva.Count == 1)
            {

                BarrasRefSoloTagDTO barrasRefSoloTagDTO = new BarrasRefSoloTagDTO()
                {
                    largo = listaCurva[0].Length,
                    P1 = listaCurva[0].GetPoint2(0),
                    P2 = listaCurva[0].GetPoint2(1),
                    posicion = TipoBarraSoloTag.Centro
                };

                LargoTotal += Util.FootToCm(listaCurva[0].Length);

                listaLargos.Add(barrasRefSoloTagDTO);
            }
        }



      

        public IGeometriaTag GenerarTagRefuerzo()
        {

            if (listaLargos.Count == 0) return new GeomeTagNull();

            BarrasRefSoloTagDTO LArgoCentro = listaLargos.MinBy(c => -c.largo); 
            IGeometriaTag _newGeometriaTag = FactoryGeomTagRefuerzo.CrearGeometriaTagBarraRefuerzo(_uiapp,
                                                                            ObtenerTipoBarra(),
                                                                             LArgoCentro.P1,
                                                                              LArgoCentro.P2,
                                                                             (LArgoCentro.P1+ LArgoCentro.P2) / 2);
            return _newGeometriaTag;
        }

        private TipoBarraRefuerzo ObtenerTipoBarra()
        {
            return (listaLargos.Count == 1 ? TipoBarraRefuerzo.BarraRefSinPatas : TipoBarraRefuerzo.BarraRefPataAmbos);
        }

        private TipoBarraSoloTag GetPosicion()
        {
            return TipoBarraSoloTag.Centro;
        }
    }
}
