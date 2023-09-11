using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar
{
      public    class SeleccionarLosaOFunda
    {
        private  UIApplication _uiapp;
        private readonly View3D _elem3D_Parabusacr;
        private Document _doc;
        private View _view;
        private  TipoRefuerzoLOSA _tipoRefuerzoLOSA;

        public Floor LosaSelecionado { get; set; }
        public XYZ PuntoSobreFAceIntersectada { get; set; }
        public SeleccionarLosaOFunda(UIApplication _uiapp,View3D elem3d_parabusacr_, TipoRefuerzoLOSA _tipoRefuerzoLOSA_)
        {
            this._uiapp = _uiapp;
            this._elem3D_Parabusacr = elem3d_parabusacr_;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._tipoRefuerzoLOSA = _tipoRefuerzoLOSA_;
        }

        public bool SeleccionarElementoFLoor_Fundaction(XYZ PtoBUscarLosa)
        {
            Level lv = _doc.ActiveView.GenLevel;
            try
            {

         
                if (_tipoRefuerzoLOSA == TipoRefuerzoLOSA.fundacion)
                {
                    var result = _view.Obtener_Z_SoloPLantas();
                    if (result.Isok && PtoBUscarLosa.Z< result.valorz)
                        PtoBUscarLosa = PtoBUscarLosa.AsignarZ(result.valorz);

                    BuscarFundacionLosa _buscarElementosBajo = new BuscarFundacionLosa(_uiapp, Util.CmToFoot(400));
                    if (!_buscarElementosBajo.OBtenerRefrenciaFundacionSegunVector(_elem3D_Parabusacr, PtoBUscarLosa, new XYZ(0, 0, -1)))
                    {
                        Util.ErrorMsg($"NO se encontro Fundacion en las coordenadas de las lineas seleccionadas.\n\n Punto busqueda:{PtoBUscarLosa.REdondearString_cm(0) }.\n\n Seleccione Fundacion manualmente:");

                        SeleccionarFundConMouse seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);
                        if (!seleccionarFundConMouse.M1_Selecconafund(new FiltroFundaciones())) 
                            return false;

                        _buscarElementosBajo.FundLosaElementHost = seleccionarFundConMouse._elementSelecciondo;
                        _buscarElementosBajo._PtoSObreCaraSuperiorFund = seleccionarFundConMouse.PtoMOuse_sobreFundacion;

                    }
                    LosaSelecionado = (Floor)_buscarElementosBajo.FundLosaElementHost;
                    if (LosaSelecionado == null)
                    {
                        Util.ErrorMsg($"No se encontro losa en el nivel analizado   Nivel:{lv.Name}");
                        return false;
                    }
                    PuntoSobreFAceIntersectada = _buscarElementosBajo._PtoSObreCaraSuperiorFund;
                }
                else
                {
                    //SeleccionarLosaConPto seleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
                    ////OTRA ALTERNATIVA SELECIONAR LOSA CON PUNTO                  
                    //LosaSelecionado = seleccionarLosaConPto.EjecturaSeleccionarLosaConPto(PtoBUscarLosa, lv);

                    BuscarFundacionLosa _buscarElementosBajo = new BuscarFundacionLosa(_uiapp, Util.CmToFoot(100));
                    if (!_buscarElementosBajo.OBtenerRefrenciaLosaSegunVector(_elem3D_Parabusacr, PtoBUscarLosa+new XYZ(0,0,1), new XYZ(0, 0, -1)))
                    {
                        Util.ErrorMsg($"NO se encontro Losa en las coordenadas de las lineas seleccionadas.\n\n Punto busqueda:{PtoBUscarLosa.REdondearString_cm(0) }.\n\n Seleccione losa manualmente:");

                        SeleccionarFundConMouse seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);
                        if (!seleccionarFundConMouse.M1_Selecconafund(new FiltroFloor()))
                            return false;

                        _buscarElementosBajo.FundLosaElementHost = seleccionarFundConMouse._elementSelecciondo;
                        _buscarElementosBajo._PtoSObreCaraSuperiorFund = seleccionarFundConMouse.PtoMOuse_sobreFundacion;

                    }

                    LosaSelecionado = (Floor)_buscarElementosBajo.FundLosaElementHost;

                    if (LosaSelecionado == null)
                    {
                        Util.ErrorMsg($"No se encontro losa en el nivel analizado   Nivel:{lv.Name}");
                        return false;
                    }

                    PuntoSobreFAceIntersectada = LosaSelecionado.ObtenerPtosInterseccionFaceSuperior(PtoBUscarLosa);
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"No se encontro losa en el nivel analizado   ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
