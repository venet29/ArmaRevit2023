using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Suples
{
    public class ManejadorUnirSuples
    {
        private UIApplication uiapp;
        private Document _doc;
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfoment2ConPto;

        public iCalculoDatosParaReinforment calculoDatosParaReinforment { get; private set; }

        public ManejadorUnirSuples(UIApplication uiapp, SeleccionarPathReinfomentConPto seleccionarPathReinfoment1ConPto, SeleccionarPathReinfomentConPto seleccionarPathReinfoment2ConPto)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._seleccionarPathReinfomentConPto = seleccionarPathReinfoment1ConPto;
            this._seleccionarPathReinfoment2ConPto = seleccionarPathReinfoment2ConPto;
        }

        internal void EjecutarUnir()
        {
            throw new NotImplementedException();
        }

        public void M1_GenerarCalculosGenerales()
        {

            try
            {
                XYZ _puntoSeleccionMouse = null;
                //1
                ICalculoTiposTraslapos calculoTiposTraslapos = CalculoTiposTraslapos.CreatorCalculoTiposTraslapos(_seleccionarPathReinfomentConPto.PathReinforcement, _doc);
                if (!calculoTiposTraslapos.IsOk) return;
                //2
                CalculoDatoslosa calculoDatoslosa = new CalculoDatoslosa(_seleccionarPathReinfomentConPto, _doc);
                ContenedorDatosLosaDTO DatosLosaYpathInicialesDTO = calculoDatoslosa.ObtenerContenedorDatosLosa();
                if (!DatosLosaYpathInicialesDTO.IsOk) return;
                //3
                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_seleccionarPathReinfomentConPto, _doc);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                if (!pathReinformeCalculos.IsPtoOK) return;

                //final
                calculoDatosParaReinforment =
                    FactoryITraslapo.CreateNewPathReinformentV2(pathReinformeCalculos.Obtener4pointPathReinf(), _puntoSeleccionMouse,
                                                                DatosLosaYpathInicialesDTO, calculoTiposTraslapos);

                if (!calculoDatosParaReinforment.M1_Obtener2PathReinformeTraslapoDatos()) return;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M1_GenerarCalculosGenerales' ex:{ex.Message}");
                return ;
            }
            return ;
        }
    }
}
