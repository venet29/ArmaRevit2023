using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.UI.Selection;
using System.Runtime.InteropServices.WindowsRuntime;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;

namespace ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor
{
    public class CambiarEspesorLosa_CambiaPAthYRoom
    {
        private Document _doc;
        private double _espesorLosaFoot;
        private Floor _selecFloor;

#pragma warning disable CS0169 // The field 'CambiarEspesorLosa_CambiaPAthYRoom._listaDePathReinfNivelActual' is never used
        private List<Element> _listaDePathReinfNivelActual;
#pragma warning restore CS0169 // The field 'CambiarEspesorLosa_CambiaPAthYRoom._listaDePathReinfNivelActual' is never used
        private List<PathReinforcement> _listaDePathReinDelosa;
#pragma warning disable CS0414 // The field 'CambiarEspesorLosa_CambiaPAthYRoom._todoBien' is assigned but its value is never used
        private bool _todoBien;
#pragma warning restore CS0414 // The field 'CambiarEspesorLosa_CambiaPAthYRoom._todoBien' is assigned but its value is never used
        private readonly UIDocument _uidoc;
        private readonly View _view;
        private readonly ISeleccionarLosaConMouse iSeleccionarLosaConMose;
        private readonly IManejadorPathCambioEspesor manejadorPathCambioEspesor;
        private readonly IManejadorRoomCambioEspesor iManejadorRoomCambioEspesor;


        public int CAntidadRoomCAMbiados { get; set; }
        public CambiarEspesorLosa_CambiaPAthYRoom(ExternalCommandData commandData,
                                                    ISeleccionarLosaConMouse iSeleccionarLosaConMose,
                                                    IManejadorPathCambioEspesor iManejadorPathCambioEspesor,
                                                    IManejadorRoomCambioEspesor iManejadorRoomCambioEspesor)
        {
            this._doc = commandData.Application.ActiveUIDocument.Document;
            this._uidoc = commandData.Application.ActiveUIDocument;
            this._view = this._doc.ActiveView;

            _todoBien = true;
            this.iSeleccionarLosaConMose = iSeleccionarLosaConMose;
            this.manejadorPathCambioEspesor = iManejadorPathCambioEspesor;
            this.iManejadorRoomCambioEspesor = iManejadorRoomCambioEspesor;
        }

        public void Ejecutar()
        {
            try
            {
                //selesccionar ejecutar
                _selecFloor =  M1_SelecconarFloor();

                _espesorLosaFoot= M2_ObtenerEspesorLosaFoot();

                M3_SelecconarTodoslosPAthDeLosaSeleccionado();

                M4_ObtenerLosEspesoresCAmbiar();

                M5_CAmbiarPArametroEspesorPath();

                M5B_CambiarShape();

                M6_ObtenerRoomDeLosa();

                M7_CAmbiarPArametroEspesorDeRoom();

                TaskDialog.Show("Cambio Espesor", "Se cambiaron correctamente los espesores de " + manejadorPathCambioEspesor.cantidadPathCambiado + " PathReinforment");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error al cambiar los espesores de pathReinformet");
            }
        }


        public Floor M1_SelecconarFloor() => iSeleccionarLosaConMose.M1_SelecconarFloor();
        public double M2_ObtenerEspesorLosaFoot() => iSeleccionarLosaConMose.M2_ObtenerEspesorLosaFoot();
        
        public void M3_SelecconarTodoslosPAthDeLosaSeleccionado()
        {
            //  M3_1_buscarListaPathReinFamiliasEnBrowser();
            //  _listaDePathReinDelosa = _listaDePathReinfNivelActual.Cast<PathReinforcement>().Where(cc => cc.GetHostId().IntegerValue == _selecFloor.Id.IntegerValue).ToList();
            _listaDePathReinDelosa = manejadorPathCambioEspesor.M3_SelecconarTodoslosPAthDeLosaSeleccionado(_selecFloor);
        }

        private void M4_ObtenerLosEspesoresCAmbiar()
        {
            manejadorPathCambioEspesor.M4_ObtenerLosEspesoresCAmbiar(_listaDePathReinDelosa);
        }
        public void M5_CAmbiarPArametroEspesorPath()
        {
            manejadorPathCambioEspesor.M5_CAmbiarPArametroEspesorPath(_espesorLosaFoot
                                                                - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm)
                                                                - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm));
        }

        private void M5B_CambiarShape()
        {
            manejadorPathCambioEspesor.M5B_CAmbiarREbarShapeCuadrado();
        }

        private void M6_ObtenerRoomDeLosa()
        {
            iManejadorRoomCambioEspesor.M6_ObtenerRoomDeLosa(_selecFloor);
        }
        private void M7_CAmbiarPArametroEspesorDeRoom()
        {
            CAntidadRoomCAMbiados = iManejadorRoomCambioEspesor.M7_CAmbiarPArametroEspesorDeRoom(Util.FootToCm(_espesorLosaFoot));
        }
    }


}
