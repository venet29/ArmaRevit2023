using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.Seleccionar
{


    public class SeleccionarFundConMouse
    {
        private Document _doc;
        private UIDocument _uidoc;
        private bool isTest;
        // private Floor _selecFloorMouse;
        public XYZ PtoMOuse_sobreFundacion { get; set; }
        public Floor _fundacionSelecciondo { get; set; }

        public Element _elementSelecciondo { get; set; }

        public PlanarFace _caraInferior { get; set; }
        public PlanarFace _caraSuperior { get; set; }
        public bool _todoBien { get; set; }

        public SeleccionarFundConMouse(UIApplication _UIapp, bool isTest = false)
        {
            this._doc = _UIapp.ActiveUIDocument.Document;
            this._uidoc = _UIapp.ActiveUIDocument;
            this.isTest = isTest;
            _todoBien = true;
        }

        public bool M1_Selecconafund(ISelectionFilter f)
        {
            try
            {
                _fundacionSelecciondo = null;
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)

                //selecciona un objeto floor
                //ISelectionFilter f = new FiltroFloorOrFund();
                Reference r;
                try
                {
                    r = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar Fundacion");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;

                }
                // sirefere3ncia es null salir

                //obtiene una referencia floor con la referencia r
                PtoMOuse_sobreFundacion = r.GlobalPoint;
                _elementSelecciondo = _doc.GetElement(r.ElementId);
                _fundacionSelecciondo = _elementSelecciondo as Floor;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsOk()
        {
            return (PtoMOuse_sobreFundacion == null || _elementSelecciondo == null ? false : true);
        }


        public FundGeoDTO M1_SeleccionarCaraInferiorFund()
        {
            //a)

            if (_elementSelecciondo == null)
            {
                Util.ErrorMsg("Error al seleccionar fundaciones");
                return new FundGeoDTO() { IsOK = false }; ;
            }

            //b)
            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
            _caraInferior = SeleccionarLosaConPto.SeleccionarCAraInferiorFoorOFundation(PtoMOuse_sobreFundacion, _elementSelecciondo);
            //Util.InfoMsg($" Planarface de mayor area  orige:{_caraInferior.GraphicsStyleId.IntegerValue}    Area:{_caraInferior.Area} \n oriegen:{_caraInferior.Origin}   direccion:{ _caraInferior.FaceNormal }");
            if (_caraInferior == null)
            {
                Util.ErrorMsg("Error al seleccionar cara inferior de fundaciones");
                return new FundGeoDTO() { IsOK = false };
            }

            //c)
            FundGeoDTO _fundGeoDTO = new FundGeoDTO()
            {
                FaceAnalizada = _caraInferior,
                CaraAnalizada="inferior",
                fundacion = _elementSelecciondo,
                ptoSeleccionFund = PtoMOuse_sobreFundacion,
                Espesor_foot= _elementSelecciondo.ObtenerEspesorConPtos_foot(PtoMOuse_sobreFundacion,XYZ.BasisZ),
                IsOK = true
            };

            return _fundGeoDTO;
        }

        public FundGeoDTO M2_SeleccionarCaraSuperiorFund()
        {
            //a)

            if (_elementSelecciondo == null)
            {
                Util.ErrorMsg("Error al seleccionar fundaciones");
                return new FundGeoDTO() { IsOK = false }; ;
            }

            //b)
            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
            _caraSuperior = SeleccionarLosaConPto.SeleccionarCAraSuperiorFoorOFundation(PtoMOuse_sobreFundacion, _elementSelecciondo);
            //Util.InfoMsg($" Planarface de mayor area  orige:{_caraInferior.GraphicsStyleId.IntegerValue}    Area:{_caraInferior.Area} \n oriegen:{_caraInferior.Origin}   direccion:{ _caraInferior.FaceNormal }");
            if (_caraSuperior == null)
            {
                Util.ErrorMsg("Error al seleccionar cara superior de fundaciones");
                return new FundGeoDTO() { IsOK = false };
            }

            //c)
            FundGeoDTO _fundGeoDTO = new FundGeoDTO()
            {
                FaceAnalizada = _caraSuperior,
                CaraAnalizada = "superior",
                fundacion = _elementSelecciondo,
                ptoSeleccionFund = PtoMOuse_sobreFundacion,
                Espesor_foot = _elementSelecciondo.ObtenerEspesorConPtos_foot(PtoMOuse_sobreFundacion, XYZ.BasisZ),
                IsOK = true
            };

            return _fundGeoDTO;
        }

  
        public bool M3_ObtenerCarasSUperiorInferior()
        {
            try
            {
                SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
                _caraInferior = SeleccionarLosaConPto.SeleccionarCAraInferiorFoorOFundation(PtoMOuse_sobreFundacion, _elementSelecciondo);
                
                _caraSuperior = SeleccionarLosaConPto.SeleccionarCAraSuperiorFoorOFundation(PtoMOuse_sobreFundacion, _elementSelecciondo);


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
