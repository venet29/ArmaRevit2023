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
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.LosaArmadura.Cmd;
using ArmaduraLosaRevit.Model.Armadura;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.LosaArmadura
{

    public class CreardorCrearRoomConPelotaLosa
    {
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Document _doc;

        private View _view;
        public List<AnotationGeneralPelotaLosa> _listaAnnotation;

        protected Level _levelLOsa;
        private List<Element> _floors;
        private UtilitarioFallasAdvertencias _utilfallas;
        private ISeleccionAnotationPelotaLosa _seleccionAnotationPelotaLosa;

        public CreardorCrearRoomConPelotaLosa(ExternalCommandData commandData, View activeView, ISeleccionAnotationPelotaLosa seleccionAnotationPelotaLosa)
        {
            _commandData = commandData;
            _uiapp = commandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _doc = _uidoc.Document;
            _utilfallas = new UtilitarioFallasAdvertencias();
            _seleccionAnotationPelotaLosa = seleccionAnotationPelotaLosa;
            _view = activeView;
        }
        public Result Execute()
        {
            M1_ObtenerLevel();

            M2_SeleccionarListaAnnotationConPelotasEstructurales();

            using (TransactionGroup t = new TransactionGroup(_doc))
            {
                t.Start("CrearRoomConPelotaLosa-NH");


                if (_listaAnnotation.Count == 0) return Result.Failed;

                if (!M3_VerificarSiExisteUnaLosa()) return Result.Failed;

                if (!M4_BorrarTodasSepataDoresRoom()) return Result.Failed;

                if (!M5_CrearRoomSeparatorPorVigaEnNivel()) return Result.Failed;

                if (!M6_CrearRoomSeparatorPorOpening()) return Result.Failed;

                if (!M7_CrearRoom())
                {
                    t.RollBack();
                    return Result.Cancelled;
                }
                if (!M8_BorrarPelotaEStructural()) return Result.Failed;

                t.Assimilate();
            }

            return Result.Succeeded;
        }


        // obtine el view y level analizado
        protected void M1_ObtenerLevel()
        {

            ///comentar
            // IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(_doc, _view);
            //  viewRangleClase.EditarParametroViewRange(ConstantesGenerales.VIEWRANGE_TOP, ConstantesGenerales.VIEWRANGE_CORTE, ConstantesGenerales.VIEWRANGE_BOTTON, ConstantesGenerales.VIEWRANGE_DEPTH);
            // busca el nivel del pisos analizado
            ElementId levelId = Util.FindLevelId(_doc, _view.GenLevel.Name);
            //obtiene el nivel del la losa
            _levelLOsa = _doc.GetElement(levelId) as Level;
        }
        protected void M2_SeleccionarListaAnnotationConPelotasEstructurales()
        {
            // crea lista 'ListaAnnotation'  con TODOSLOS objeros 'AnnotationGeneralPelotaLosa' con los datos del pelota de losa, en el view analisado            
            _listaAnnotation = _seleccionAnotationPelotaLosa.GetAnotationPelotaLosaFromViewWithMouse(_uidoc);
        }


        //vaerifica si existe una losa
        protected bool M3_VerificarSiExisteUnaLosa()
        {

            //crear lista con las losas en el view en el que se trabaja
            _floors = SeleccionElement.GetElementoFromLevel(_doc, typeof(Floor), _levelLOsa);

            if (_floors.Count < 1)
            {
                TaskDialog.Show("Error", "No se encontraron losas en View analizada ");
                //      commandData.Application.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);
                return false;
            }
            return true;
        }

        //crea las linea room separate para las vigas de el nivel determinado
        protected bool M5_CrearRoomSeparatorPorVigaEnNivel()
        {
            //_uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            // crea objetos con las vigas los contornos de las vigas del view
            NH_ListaVIgas listaVigas = new NH_ListaVIgas(_commandData);
            /// 1)obtiene lista de vigas en el nivel de trabajo 
            /// 2)obtiene su  geometria ProfileBeam
            listaVigas.GetVigaPoligonos(_view);
            if (listaVigas.ListaProfileBeam.Count > 0)
            {
                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("Classe_prueba_vigas-NH");
                        //Genera LAs lineas de separacion de rooms
                        listaVigas.DibujarLineasSeparacicionRoom();
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // message = ex.Message;Util
                    Util.ErrorMsg($"Error al crear RoomSeparator  ex:{ex.Message} ");
                    return false;
                }
            }

            return true;
        }

        protected bool M6_CrearRoomSeparatorPorOpening()
        {
            #region 4)Crear separadores por Openning
            Cmd_DibujarLineasSeparacicionRoomOpening dibujarLineasSeparacicionRoomOpening = new Cmd_DibujarLineasSeparacicionRoomOpening();
            var resul = dibujarLineasSeparacicionRoomOpening.Execute_NH_ListaOpening(_commandData);

            #endregion
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return (resul == Result.Succeeded ? true : false);
        }
        //crea los room
        protected bool M7_CrearRoom()
        {
            if (_listaAnnotation == null) return false;
            #region 5) generar room con pelota de losa
            try
            {
                foreach (var anoot in _listaAnnotation)
                {
                    string nombre = anoot.Numero;
                    try
                    {
                        double espesor = 0;
                        if (double.TryParse(anoot.Espesor, out espesor))
                        {
                            RoomsObj newRoom = new RoomsObj(_doc, null, anoot.Angulo, anoot.Numero, espesor, "8a20", "8a20");
                            if (newRoom.CreateRoom(_levelLOsa, anoot.PointUbicacion) != null)
                            {
                                newRoom.CargarDatosRoom(anoot.IsEspesorVariable);
                                anoot.Isborrar = true;// roomData.CreateRoom(doc, levelLOsa, anoot);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        anoot.Isborrar = false;
                        Util.ErrorMsg($"Error al crear M7_CrearRoom_a  ex:{ex.Message} \n en losa:{nombre} ");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear M7_CrearRoom_b  ex:{ex.Message} ");
                return false;
            }
            return true;
        }
        protected bool M8_BorrarPelotaEStructural()
        {
            if (_listaAnnotation == null) return false;


            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("External Tool-NH");
                    //delete all the unpinned dimensions
                    foreach (var annot in _listaAnnotation)
                    {
                        if (annot.Isborrar == true)
                        {
                            _doc.Delete(annot.PelotaLosa.Id);
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear RoomSeparator  ex:{ex.Message} ");
                return false;

            }
            return true;

        }

        /// <summary>
        /// borras las  lineas Room separation ---> 
        /// </summary>
        public bool M4_BorrarTodasSepataDoresRoom()
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Borrar separateRooom-NH");
                    //Genera LAs lineas de separacion de rooms
                    ICollection<ElementId> linesId = new FilteredElementCollector(_doc).OfClass(typeof(CurveElement)).Where(q => q.Category.Id == new ElementId(BuiltInCategory.OST_RoomSeparationLines) &&
                                                                                                      TipoPhases_.IsFasesExistenete(q)
                                                                                                      && (q as ModelLine).LevelId == (_view.GenLevel as Element).Id).Select(rr => rr.Id).ToList();
                    if (linesId != null)
                        _doc.Delete(linesId);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear Borrar SepataDoresRoom  ex:{ex.Message} ");

                return false;
            }
            return true;

        }

    }



}
