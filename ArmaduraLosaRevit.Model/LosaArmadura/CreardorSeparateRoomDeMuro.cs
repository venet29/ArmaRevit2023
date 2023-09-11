using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

//using planta_aux_C.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.ViewRang;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.Enumeraciones;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.LosaArmadura
{


    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CreardorSeparateRoomDeMuro
    {
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Document _doc;

        private View _view;
        private MuroGeometrias _MuroGeometrias;

        private UtilitarioFallasAdvertencias _utilfallas;
        private HelperSeleccinarMuro _seleccinarMuroRefuerzo;
        private Level _levelLOsa;
        private readonly ISeleccionarLosaConMouse seleccionarLosaConMouse;

        public CreardorSeparateRoomDeMuro(ExternalCommandData commandData, View activeView,
                                                       HelperSeleccinarMuro seleccinarMuroRefuerzo,
                                                       ISeleccionarLosaConMouse seleccionarLosaConMouse)

        {
            _commandData = commandData;
            _uiapp = commandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _doc = _uidoc.Document;
           // _utilfallas = new UtilitarioFallasAdvertencias();

            _seleccinarMuroRefuerzo = seleccinarMuroRefuerzo;
            this.seleccionarLosaConMouse = seleccionarLosaConMouse;
            _view = activeView;


         

        }
        public Result Execute()
        {
            bool seguir = true;

            while (seguir)
            {
                _MuroGeometrias = new MuroGeometrias(_uiapp, _seleccinarMuroRefuerzo);
                M1_ObtenerLevel();

                if (!M2_Seleccionar_SoloUno_Muro())
                {
                    seguir = false;
                    return Result.Succeeded;
                    }
                

                if (!M3_ObtenerBordeLosa()) return Result.Failed;

                if (!M5_CrearRoomSeparatorPorBordeLibre()) return Result.Failed;
            }
            return Result.Succeeded;
        }
        protected void M1_ObtenerLevel()
        {

            ///comentar
           // IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(_doc, _view);
         //viewRangleClase.EditarParametroViewRange(ConstantesGenerales.VIEWRANGE_TOP, ConstantesGenerales.VIEWRANGE_CORTE, ConstantesGenerales.VIEWRANGE_BOTTON, ConstantesGenerales.VIEWRANGE_DEPTH);
            // busca el nivel del pisos analizado
            ElementId levelId = Util.FindLevelId(_doc, _view.GenLevel.Name);
            //obtiene el nivel del la losa
            _levelLOsa = _doc.GetElement(levelId) as Level;
        }
        private bool M2_Seleccionar_SoloUno_Muro()
        {
           return _MuroGeometrias.M1_ObtenerCaraSuperiorMuro();
            //   _seleccinarMuroRefuerzo.EjecutarSeleccion();

        }


        private bool M3_ObtenerBordeLosa()
        {
            return _MuroGeometrias.M2_ObtenerBordeLosa(_levelLOsa.ProjectElevation);
        }

        protected bool M5_CrearRoomSeparatorPorBordeLibre()
        {
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            if (_MuroGeometrias.listaBorde.Count > 0)
            {
                try
                {
                    _MuroGeometrias.M3_DibujarLineasSeparacionRoom(_levelLOsa);
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    // message = ex.Message;
                    return false;

                }
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return true;

        }
    }



}
