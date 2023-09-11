using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{
    public class EnvoltorioMuro
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly Transform transform;

        //protected Element _elemento;

        public Element element { get; set; }
        public Wall WallSelect { get; private set; }
        public Curve LocationCurve_ { get; private set; }
        public double CotaSUperior { get; private set; }
        public double CotaInferior { get; private set; }
        public XYZ Direccion { get; private set; }
        public double Espesor { get; private set; }
        public double Largo { get; private set; }
        public FamilyInstance VigaSelect { get; private set; }

        public EnvoltorioMuro(UIApplication uiapp, Element c, Transform transform_)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.element = c;
            this.transform = transform_;

        }

        public bool ObtenerDatos()
        {
            try
            {
                if (element.Id.ToString() == "3469799")
                { }
                if (element is Wall)
                {
                    WallSelect = (Wall)element;
                    LocationCurve_ = ((LocationCurve)(WallSelect.Location)).Curve;

                    

                    var carSUp = WallSelect.ObtenerCaraSuperior(false);
                    CotaSUperior = transform.Origin.Z + carSUp.Origin.Z;

                    var carInf = WallSelect.ObtenerCaraInferior(false);
                    CotaInferior = transform.Origin.Z + carInf.Origin.Z;
                    // Espesor = WallSelect.ObtenerEspesorMuroFoot(_doc);// tipoFamiliaMuro.Width;
                    Largo = WallSelect.ObtenerLargo();
                }
                else if (element is FamilyInstance)
                {
                    VigaSelect = (FamilyInstance)element;
                    if (VigaSelect.StructuralType == StructuralType.Beam)
                    {
                        LocationCurve_ = ((LocationCurve)(VigaSelect.Location)).Curve;
                        Largo = VigaSelect.ObtenerLargo(); //ObtenerAnchoConPtos(_ptoSeleccionMouseCentroCaraMuro6, _view.ViewDirection);

                        var carSUp = VigaSelect.ObtenerCaraSuperior(false);
                        CotaSUperior = transform.Origin.Z + carSUp.Origin.Z;

                        var carInf = VigaSelect.ObtenerCaraInferior(false);
                        CotaInferior = transform.Origin.Z + carInf.Origin.Z;
                    }
                    else
                    {
                        Util.ErrorMsg($"FamilyInstance tipo {VigaSelect.StructuralType} no esta implementada. ");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener datos de muro:{element.Id}.\n\n ex:{ex.Message} ");
                return false;
            }
            return true;
        }

        internal bool ObtenerCarasVerticalesConReferenias()
        {
            try
            {
                if (element.IsValidObject)
                {
                    List<PlanarFace> listaPLanos_paraReferencia = new List<PlanarFace>();
                    var listaTotal = element.ListaFace_Conferencias(true)[0];

                    listaPLanos_paraReferencia = listaTotal.Where(c => Math.Abs(c.FaceNormal.Z) < 0.1).ToList();

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener caras verticales de elemento id:{element.Id}. ex:{ex.Message} ");
                return false;
            }
            return true;
        }
    }
}

