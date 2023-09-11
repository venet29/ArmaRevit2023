using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Intervalos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public interface ISeleccionarLosaConMouse
    {
        Floor M1_SelecconarFloor();
        double M2_ObtenerEspesorLosaFoot();
    }

    public class SeleccionarLosaConMouse : ISeleccionarLosaConMouse
    {
        protected Document _doc;
        protected UIDocument _uidoc;
        protected View _view;
        protected  UIApplication _uiapp;
        protected bool isTest;
        protected LosaGeometrias _losaGeometrias;
        protected bool IsSeleccionarConDosPtos;

        public Curve _curvaBordeLosa { get; set; }

        // private Floor _selecFloorMouse;
        public Floor LosaSelecionado { get; set; }
        public XYZ _ptoSeleccionEnLosa { get; set; }
        public XYZ _ptoSeleccionEnLosa2 { get; set; }
        public bool _todoBien { get; set; }
        public XYZ DireccionHaciaLosa { get; private set; }
        public Element ElementoSeleccionado { get; private set; }

        public SeleccionarLosaConMouse(UIApplication uiapp, bool isTest = false)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uidoc = uiapp.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._uiapp = uiapp;
            this.isTest = isTest;
            this.IsSeleccionarConDosPtos = false;
            _todoBien = true;
        }



        public Floor M1_SelecconarFloor()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
      
                Reference r;
                try
                {
                    r = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar losa");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    r = null;
                }
                // sirefere3ncia es null salir
                if (r == null) return null;
                ElementoSeleccionado = _doc.GetElement(r.ElementId);

                if (!(ElementoSeleccionado is Floor)) return null;
                //obtiene una referencia floor con la referencia r
                LosaSelecionado = ElementoSeleccionado as Floor;

                _ptoSeleccionEnLosa = r.GlobalPoint;
                //obtiene el nivel del la los

            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al seleccionar de losa");
                return null;
            }
            return LosaSelecionado;
        }
        public Element M1_Selecconar_segunFiltro(ISelectionFilter f)
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                //ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
         
                Reference r;
                try
                {
                    r = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar losa");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    r = null;
                }
                // sirefere3ncia es null salir
                if (r == null) return null;
                ElementoSeleccionado = _doc.GetElement(r.ElementId);

        //        if (!(ElementoSeleccionado is Floor)) return null;
                //obtiene una referencia floor con la referencia r
                
                _ptoSeleccionEnLosa = r.GlobalPoint;
                //obtiene el nivel del la los

            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al seleccionar de losa");
                return null;
            }
            return ElementoSeleccionado;
        }
        public double M2_ObtenerEspesorLosaFoot() => Util.CmToFoot(LosaSelecionado.ObtenerEspesorLosaCm(_doc));

        public bool M3_ObtenerBordeLosa(XYZ _ptoSeleccionBordeLosa)
        {
            try
            {
                _curvaBordeLosa = null;
                _losaGeometrias = new LosaGeometrias(_uidoc.Application, LosaSelecionado);
                _curvaBordeLosa = _losaGeometrias.M2_ObtenerBordeSuperiorLosaSeleccionado(_ptoSeleccionBordeLosa);



                if (_curvaBordeLosa != null)
                {
                    XYZ _ptoSobreBorde = _curvaBordeLosa.Project(_ptoSeleccionEnLosa).XYZPoint;
                    DireccionHaciaLosa = (_ptoSeleccionEnLosa - _ptoSobreBorde).GetXY0().Normalize();

                    return true;
                }
                else
                {
                    Util.ErrorMsg("a)Error al obtener el borde de losa");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"b)Error al obtener el borde de losa ex:{ex.Message}");
                return false;
            }

        }



    


    }
}
