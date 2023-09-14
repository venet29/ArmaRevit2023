using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Viewnh;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Pasadas
{
    public class MAnejadorCrearRegionConPAsada
    {
        private readonly UIApplication _uiapp;
        private readonly string nombreFiilRegion;
        private readonly string nombrePasada;
        private int contadorPasadas;
        private Document _doc;
        private View _view;

        private List<PlanarFace> ListaPLAnarFace;
        private FilledRegionType _FilledRegion;

        public MAnejadorCrearRegionConPAsada(UIApplication uiapp,  string nombreFiilRegion,string nombrePasada)
        {
            this._uiapp = uiapp;
            this.nombreFiilRegion = nombreFiilRegion;
            this.nombrePasada = nombrePasada;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.ActiveView;
           // this._uidoc = uiapp.ActiveUIDocument;
            ListaPLAnarFace = new List<PlanarFace>();
            contadorPasadas = 0;
        }

        public bool Ejecutar(View _viewDato=null)
        {
            try
            {
                if (_viewDato != null) _view = _viewDato;

                ActivarDatail();
                //if (!M1_SeleccionarPAsadas()) return false;
                if (!M1_ObtenerTodasPAsadas()) return false;

                M2_CrearFillRegion();
            }
            catch (Exception)
            {
                
                return false;
            }
            Util.InfoMsg($"Proceso terminado");
            return true;
        }


        public bool EjecutarTodasLasVista()
        {
            try
            {
                RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
                if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return false;

                 _FilledRegion = Tipos_FiilRegion.ObtenerFilledRegionTypePorNombre(_doc, nombreFiilRegion);

                if (_FilledRegion == null)
                {
                    Util.ErrorMsg($"No se encontro Familia  FillRegion :{nombreFiilRegion}.");
                    return false;
                }

                CreadorView.IsMje = true;
                SeleccionarView _SeleccionarView = new SeleccionarView();
                var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);

                string input = Microsoft.VisualBasic.Interaction.InputBox($"Seguro desea cargar 'Pasadas' a {ListaViewSection.Count} view de elevaciones del proyecto?. Proceso puede tardar unos minutos.\n Confirmar escribiendo : Elevacion", "Borrar", "", 300, 300);
                if (input.Trim().ToLower() != "elevacion") return false;

             

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CargarConfiguracion-NH");
                    for (int i = 0; i < ListaViewSection.Count; i++)
                    {

                        _view = ListaViewSection[i];
                        _uiapp.ActiveUIDocument.ActiveView = _view;

                        ActivarDatail();

                        if (!M1_ObtenerTodasPAsadas()) continue;

                        if (!M2_CrearFillRegion())
                        {
                            var result = Util.InfoMsg_YesNo($"Desea continuar?. Falta {ListaViewSection.Count - 1 - i} view por cargar configuraciones.");
                            if (result == System.Windows.Forms.DialogResult.No)
                            {
                                i = ListaViewSection.Count;
                                break;
                            }
                        }
                    }
                    t.Assimilate();
                }

            }
            catch (Exception )

            {
                return false;
            }
            Util.InfoMsg($"Proceso terminado. Se encontraron {contadorPasadas} pasadas");
            return true;
        }


        public bool M1_ObtenerTodasPAsadas()
        {
            try
            {
                ListaPLAnarFace.Clear();
                FilteredElementCollector fillRegion = new FilteredElementCollector(_doc, _view.Id).OfClass(typeof(FamilyInstance));
                var lista = fillRegion.Where(c => c.Name == nombrePasada).ToList();
                foreach (Element ref_elem in lista)
                {

                   (bool reult, PlanarFace caraVisible) = ref_elem.ObtenerCaraVerticalVIsible(_view);
                    if (caraVisible == null) continue;
                    if (caraVisible.Area < Util.CmToFoot(1)) continue;
                    ListaPLAnarFace.Add(caraVisible);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return (ListaPLAnarFace.Count>0);
        }

        private bool M2_CrearFillRegion()
        {

            if (ListaPLAnarFace.Count == 0) return false;

            contadorPasadas += ListaPLAnarFace.Count;
            if (_FilledRegion==null)
                _FilledRegion = Tipos_FiilRegion.ObtenerFilledRegionTypePorNombre(_doc, nombreFiilRegion);


            if (_FilledRegion == null)
            {
                Util.ErrorMsg($"No se encontro Familia  FillRegion :{nombreFiilRegion}.");
                return false;
            }

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar TextoNoteNiveles-NH");

                    M2_CrearFillRegion_sinTrans();

                    t.Commit();
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


        private bool M2_CrearFillRegion_sinTrans()
        {

            if (ListaPLAnarFace.Count == 0) return false;
            try
            {
                    foreach (var item in ListaPLAnarFace)
                    {
                        var listPtos = item.ObtenerListaCurvas();
                        List<CurveLoop> profileloops = new List<CurveLoop>();
                        CurveLoop profileloop = new CurveLoop();

                        foreach (Curve LINE in listPtos)
                        {
                            profileloop.Append(LINE);
                        }
                        profileloops.Add(profileloop);
                        FilledRegion filledRegion = FilledRegion.Create(_doc, _FilledRegion.Id, _view.Id, profileloops);
                    }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool ActivarDatail()
        {

            try
            {
                VisibilidadView _detall = VisibilidadView.Creador_Visibilidad_SinInterfase(_view, BuiltInCategory.OST_DetailComponents, "Detail Items");
                if (_detall == null) return false;
                if (_detall.EstadoActualHide())
                    _detall.CambiarVisibilityBuiltInCategory();

            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }
    }
}
