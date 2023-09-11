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
    public class CreardorCrearRoomConPelotaLosaIndividual : CreardorCrearRoomConPelotaLosa
    {
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Document _doc;

        private View _view;
        private LosaGeometrias _losaGeometrias;
        
#pragma warning disable CS0169 // The field 'CreardorCrearRoomConPelotaLosaIndividual._floors' is never used
        private List<Element> _floors;
#pragma warning restore CS0169 // The field 'CreardorCrearRoomConPelotaLosaIndividual._floors' is never used
        private UtilitarioFallasAdvertencias _utilfallas;
        private ISeleccionAnotationPelotaLosa _seleccionAnotationPelotaLosa;
        private readonly ISeleccionarLosaConMouse seleccionarLosaConMouse;

        public CreardorCrearRoomConPelotaLosaIndividual(ExternalCommandData commandData, View activeView,
                                                       ISeleccionAnotationPelotaLosa seleccionAnotationPelotaLosa,
                                                       ISeleccionarLosaConMouse seleccionarLosaConMouse)
            : base(commandData, activeView, seleccionAnotationPelotaLosa)
        {
            _commandData = commandData;
            _uiapp = commandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _doc = _uidoc.Document;
         //   _utilfallas = new UtilitarioFallasAdvertencias();
            _seleccionAnotationPelotaLosa = seleccionAnotationPelotaLosa;
            this.seleccionarLosaConMouse = seleccionarLosaConMouse;
            _view = activeView;


            _losaGeometrias = new LosaGeometrias(_uiapp, seleccionarLosaConMouse);

        }
#pragma warning disable CS0108 // 'CreardorCrearRoomConPelotaLosaIndividual.Execute()' hides inherited member 'CreardorCrearRoomConPelotaLosa.Execute()'. Use the new keyword if hiding was intended.
        public Result Execute()
#pragma warning restore CS0108 // 'CreardorCrearRoomConPelotaLosaIndividual.Execute()' hides inherited member 'CreardorCrearRoomConPelotaLosa.Execute()'. Use the new keyword if hiding was intended.
        {
            M1_ObtenerLevel();

            M2_Seleccionar_SoloUno_AnnotationConPelotasEstructurales();
            if (_listaAnnotation.Count == 0) return Result.Failed;

            M2_seleccionarLosa();
            if (_losaGeometrias.floorseleccionado == null) return Result.Failed;

            if (!M3_ObtenerBordeLosa()) return Result.Failed;

           // if (!M4_BorrarTodasSepataDoresRoom()) return Result.Failed;

            if (!M5_CrearRoomSeparatorPorVigaEnNivel()) return Result.Failed;
           
            if (!M5_CrearRoomSeparatorPorBordeLibre()) return Result.Failed;

            if (!M7_CrearRoom()) return Result.Failed;

            if (!M8_BorrarPelotaEStructural()) return Result.Failed;


            return Result.Succeeded;
        }


        public Result Execute_seleccionandoLosa()
        {




            SeleccionarLosaConMouse _SeleccionarLosaConMouse = new SeleccionarLosaConMouse(_uiapp);
            _losaGeometrias.floorseleccionado =_SeleccionarLosaConMouse.M1_SelecconarFloor();



            if (_losaGeometrias.floorseleccionado == null) return Result.Failed;

            _listaAnnotation = new List<AnotationGeneralPelotaLosa>();

            AnotationGeneralPelotaLosa item_ = new AnotationGeneralPelotaLosa()
            {
                PelotaLosa = null,
                Numero = "105",
                Espesor = "15",
                PointUbicacion = new XYZ(168.586406771323, 36.0588668971806, 24.5),
                Angulo = 0,
                IsEspesorVariable = false
            };
            _listaAnnotation.Add(item_);

            if (!M3_ObtenerBordeLosa()) return Result.Failed;


            _levelLOsa = (Level)_doc.GetElement(_losaGeometrias.floorseleccionado.LevelId);
            // if (!M4_BorrarTodasSepataDoresRoom()) return Result.Failed;

            // if (!M5_CrearRoomSeparatorPorVigaEnNivel()) return Result.Failed;

            if (!M5_CrearRoomSeparatorPorBordeLibre()) return Result.Failed;

            if (!M7_CrearRoom()) return Result.Failed;

            if (!M8_BorrarPelotaEStructural()) return Result.Failed;


            return Result.Succeeded;
        }


        private void M2_Seleccionar_SoloUno_AnnotationConPelotasEstructurales()
        {
            // crea lista 'ListaAnnotation'  con TODOSLOS objeros 'AnnotationGeneralPelotaLosa' con los datos del pelota de losa, en el view analisado            
            _listaAnnotation = _seleccionAnotationPelotaLosa.Get_SoloUno_AnotationPelotaLosaFromViewWithMouse(_uidoc);
        }

        private void M2_seleccionarLosa()
        {
            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
            _losaGeometrias.floorseleccionado = SeleccionarLosaConPto.EjecturaSeleccionarLosaConPtoInclinada(_listaAnnotation[0].PointUbicacion, _levelLOsa);

            if (_losaGeometrias.floorseleccionado != null)
                this._levelLOsa= _losaGeometrias.ObtenerNivelLosa();

        }
        private bool M3_ObtenerBordeLosa()
        {
          return  _losaGeometrias.M2_ObtenerBordeLosaSuperior();
        }

        protected bool M5_CrearRoomSeparatorPorBordeLibre()
        {
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            if (_losaGeometrias.listaBorde.Count > 0)
            {
                try
                {
                        _losaGeometrias.DibujarLineasSeparacionRoom(_levelLOsa);

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    return false;
                }
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return true;

        }
    }



}
