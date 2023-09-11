using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{
    public class TiposRebarShapeFund : ITiposRebarShape
    {
#pragma warning disable CS0169 // The field 'TiposRebarShapeFund._LargoMin_1' is never used
        private double _LargoMin_1;
#pragma warning restore CS0169 // The field 'TiposRebarShapeFund._LargoMin_1' is never used
        private Document _doc;

        private UbicacionLosa _ubicacionEnlosa;
        private string _TipoBarra;
        private double LargoPathreiforment;
        private DatosNuevaBarraDTO _datosNuevaBarra;
        private string nombreFamiliaRebarShape;
#pragma warning disable CS0169 // The field 'TiposRebarShapeFund.nombreFamiliaRebarShapeAlternativo' is never used
        private string nombreFamiliaRebarShapeAlternativo;
#pragma warning restore CS0169 // The field 'TiposRebarShapeFund.nombreFamiliaRebarShapeAlternativo' is never used

        public DimensionesBarras dimBarras { get; private set; }
        public DimensionesBarras dimBarrasAlternativa { get; private set; }
        public DimensionesBarras dimBarras_internos { get; private set; }
        public bool IsBarrAlternative { get; private set; }
        public RebarShape tipoRebarShapeAlternativa { get; private set; }
        public RebarShape tipoRebarShapePrincipal { get; private set; }
        public double espesorEscalera { get; private set; }

        public TiposRebarShapeFund(SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, double EspesorLosa_1)
        {
            this._datosNuevaBarra = datosNuevaBarraDTO;

            this._doc = solicitudDTO.UIdoc.Document;
            this._ubicacionEnlosa = solicitudDTO.UbicacionEnlosa;
            this._TipoBarra = solicitudDTO.TipoBarra;

            this.LargoPathreiforment = datosNuevaBarraDTO.LargoPathreiforment;

            nombreFamiliaRebarShape = "";


        }



        /// <summary>
        /// define la forma del rebarshape para crear el pathreinformet
        /// modificar los valores de los parametros internos de los rebar shape para poder dibujar las barra a medida
        /// modifica :  A,B,C,D,E
        /// </summary>
        /// <param name="ubicacionEnlosa"></param>
        public DatosNuevaBarraDTO DefinirRebarShape()
        {


            switch (_TipoBarra)
            {


                case "f3_refuezoSuple":
                    // nombreFamiliaRebarShape = "NH_F11";
                    dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0,g:0, caso: "RebarShapeNh", LetrasCambiosEspesor: "_");
                    IsBarrAlternative = false;
                    break;
                case "f12":
                    // nombreFamiliaRebarShape = "NH_F11";
                    dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(_datosNuevaBarra.LargoPAtaIzqHook_cm), b: LargoPathreiforment, c: Util.CmToFoot(_datosNuevaBarra.LargoPAtaDereHook_cm), d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                    IsBarrAlternative = false;
                    break;

                case "f11":
                    //nombreFamiliaRebarShape = "NH_F11_v2";

                    dimBarras_internos = new DimensionesBarras(Util.CmToFoot(_datosNuevaBarra.LargoPAtaIzqHook_cm), b: LargoPathreiforment, c: Util.CmToFoot(_datosNuevaBarra.LargoPAtaDereHook_cm), d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                    IsBarrAlternative = false;
                    break;

                default:
                    break;
            }

            nombreFamiliaRebarShape = "M_00";
            //busca la familia de rebarshape de barra principal
            tipoRebarShapePrincipal = ObtenerRebarSHapeBase(nombreFamiliaRebarShape);
            //busca la familia de rebarshape de barra secundaria
            // if (nombreFamiliaRebarShapeAlternativo != "") tipoRebarShapeAlternativa = ObtenerRebarSHape(nombreFamiliaRebarShapeAlternativo, LargoPathreiforment, dimBarrasAlternativa);



            return ObtenerdatosNuevaBarra();

        }



        private RebarShape ObtenerRebarSHapeBase(string nombreFamiliaRebarShape)
        {
            RebarShape rebarshape = TiposFormasRebarShape.getRebarShape(nombreFamiliaRebarShape, _doc);

            return rebarshape;
        }
        /// <summary>
        /// devuelve nuevo objeto con datos calculados en esta clase necesarios para crear las barras
        /// </summary>
        /// <returns></returns>
        private DatosNuevaBarraDTO ObtenerdatosNuevaBarra()
        {
            if (tipoRebarShapePrincipal == null)
            { }
            _datosNuevaBarra.tipoRebarShapePrincipal = tipoRebarShapePrincipal;

            _datosNuevaBarra.nombreFamiliaRebarShape = nombreFamiliaRebarShape;

            _datosNuevaBarra.dimBarras_parameterSharedLetras = dimBarras_internos;

            _datosNuevaBarra.dimBarras = dimBarras;


            return _datosNuevaBarra;
        }
    }
}
