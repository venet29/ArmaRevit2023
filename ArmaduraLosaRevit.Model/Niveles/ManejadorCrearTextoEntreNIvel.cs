using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Viewnh;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Niveles
{
    class ManejadorCrearTextoEntreNIvel
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        ///  private View _view;
        private List<View> _Listaview;
        private CrearTexNote _CrearTexNote;

        public ManejadorCrearTextoEntreNIvel(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            //   this._view = _doc.ActiveView;
            _Listaview = new List<View>();
        }



        public bool Ejecutar(List<View> _ListaviewInicial)
        {
            bool resutl = false;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                if (_ListaviewInicial.Count == 0)
                {
                    if (_doc.ActiveView.ViewType != ViewType.Section)
                    {
                        Util.ErrorMsg("Comando sebe ser ejecutada en una vista de Armadura de elevaciones");
                        return false;
                    }

                    _Listaview.Add(_doc.ActiveView);
                }
                else
                    _Listaview.AddRange(_ListaviewInicial);

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("Crear CrearTextoEntreNIvel-NH");

                    if (!M1_Ejecutar_ActivarVista(false)) return false;

                    if (!M2_ObtenerTipoDeTExto()) return false;

                    M3_Ejecutar_AgregarTexto();

                    M1_Ejecutar_ActivarVista(true);

                    t.Assimilate();
                }
                resutl = true;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                resutl = false;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return resutl;
        }

        public bool EjecutarMultiple()
        {
            try
            {
                CreadorView.IsMje = true;
                SeleccionarView _SeleccionarView = new SeleccionarView();
                var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc).Select(c => (View)c).ToList();

                string input = Microsoft.VisualBasic.Interaction.InputBox($"Seguro desea cargar 'Pasadas' a {ListaViewSection.Count} view de elevaciones del proyecto?. Proceso puede tardar unos minutos.\n Confirmar escribiendo : AgregarTextoLevel", "Borrar", "", 300, 300);
                if (input.Trim().ToLower() != "agregartextolevel") return false;

                Ejecutar(ListaViewSection);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        private bool M2_ObtenerTipoDeTExto()
        {
            try
            {
                // creada 16-11-2022-- borra en furtuto
                var tipo = TiposTextNote.ObtenerTextNote("TextoEntreLevel", _doc); ;
                if (tipo == null)
                {
                    var ListaTipoNote = FActoryTipoTextNote.ObtenerLista();
                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.TextoEntreLevel, TipoCOloresTexto.Amarillo);
                    _CrearTexNote.M2_CrearListaTipoText_ConTrans(ListaTipoNote);
                }

                if (_CrearTexNote == null)
                    _CrearTexNote = new CrearTexNote(_uiapp, "TextoEntreLevel", TipoCOloresTexto.Amarillo);//"2.5mm Arial"
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener tipo texto 'TextoEntreLevel'.\n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_Ejecutar_ActivarVista(bool estado)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TextoVigaIdem-NH");
                    for (int i = 0; i < _Listaview.Count; i++)
                    {
                        var vista = _Listaview[i];
                        if (!vista.IsValidObject) continue;
                        if (vista.IsTemplate) continue;

                        if (vista.CropBoxActive != estado)
                        {

                            vista.CropBoxActive = estado;

                            if (estado == true)
                                vista.CropBoxVisible = !estado;
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en'Ejecutar_ActivarVista'.\n ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M3_Ejecutar_AgregarTexto()
        {
            try
            {
                for (int j = 0; j < _Listaview.Count; j++)
                {
                    var vistaActual = _Listaview[j];
                    _uiapp.ActiveUIDocument.ActiveView = vistaActual;

                    SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                    List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion(vistaActual).OrderBy(c => c.ElevacionRedondeada).ToList(); ;

                    int escala = vistaActual.Scale;

                    if (_CrearTexNote == null) return false;

                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("Crear TextoVigaIdem-NH");

                        for (int i = 0; i < listaLevel.Count - 1; i++)
                        {
                            var _levelENvoltorio = listaLevel[i];

                            if (!_levelENvoltorio.ISok()) continue;
                            double AlturaEntreNiel = listaLevel[i + 1].ElevacionProjectadaRedondeada - listaLevel[i].ElevacionProjectadaRedondeada;
                            // curva
                            var lista1 = _levelENvoltorio._Level.GetCurvesInView(DatumExtentType.ViewSpecific, vistaActual).ToList();
                            XYZ ptoSeleccion = null;

                            if (lista1.Count == 0) continue;

                            var _curve = lista1.First();
                            if (_curve.Length > Util.CmToFoot(300)) continue;
                            var pot1 = _curve.GetEndPoint(0);
                            var pot2 = _curve.GetEndPoint(1);

                            XYZ deltaScala = vistaActual.RightDirection * Util.CmToFoot(0);
                            double deltaZ = Util.CmToFoot(20);

                            if (escala == 75)
                            {
                                deltaScala = -vistaActual.RightDirection * Util.CmToFoot(25);
                                deltaZ = Util.CmToFoot(20);
                            }
                            else if (escala == 100)
                            {
                                deltaScala = -vistaActual.RightDirection * Util.CmToFoot(180);
                                deltaZ = Util.CmToFoot(90);
                            }

                            ptoSeleccion = (pot1 + pot2) / 2 + deltaScala;

                            string NombreNIvel = ObtenerNombreEje(_levelENvoltorio.NombreLevel);

                            var text = _CrearTexNote.M1_CrearCSintrans(ptoSeleccion.AsignarZ(_levelENvoltorio.ElevacionProjectadaRedondeada + AlturaEntreNiel / 2 + deltaZ), NombreNIvel, 0);
                        }
                        t.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }



        #region Borrar

        public bool BorrarTExtoViewActualw()
        {
            try
            {
                var _viewActual = _doc.ActiveView;

                if (_viewActual.ViewType != ViewType.Section)
                {
                    Util.ErrorMsg("Comando sebe ser ejecutada en una vista de Armadura de elevaciones");
                    return false;
                }
                var ListaViewSection = new List<View>() { _viewActual };
                BorrarTExto(ListaViewSection);


            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        public bool BorrarTExtoMultiplesView()
        {
            try
            {
                CreadorView.IsMje = true;
                SeleccionarView _SeleccionarView = new SeleccionarView();
                var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc).Select(c => (View)c).ToList();

                string input = Microsoft.VisualBasic.Interaction.InputBox($"Seguro desea cargar 'Pasadas' a {ListaViewSection.Count} view de elevaciones del proyecto?. Proceso puede tardar unos minutos.\n Confirmar escribiendo : BorrarTextoLevel", "Borrar", "", 300, 300);
                if (input.Trim().ToLower() != "borrartextolevel") return false;

                BorrarTExto(ListaViewSection);


            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public bool BorrarTExto(List<View> _Listview)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar TextoNoteNiveles-NH");

                    for (int i = 0; i < _Listview.Count; i++)
                    {
                        View _view = _Listview[i];
                        SeleccionarTextNote _SeleccionarTextNoteConNombre = new SeleccionarTextNote(_uiapp);
                        if (!_SeleccionarTextNoteConNombre.SeleccionarTextNoteConNombre("TextoEntreLevel", _view)) return false;

                        _doc.Delete(_SeleccionarTextNoteConNombre.ListaTextoPorViewYNombre.Select(c => c.Id).ToList());
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        } 
        #endregion
        private string ObtenerNombreEje(string nombreLevel)
        {
            var result = nombreLevel.Split(' ');

            if (result.Length == 2)
                return result[1].Trim();
            else
                return nombreLevel;
        }
    }
}
