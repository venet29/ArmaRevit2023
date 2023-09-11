using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado
{

    public class CambioEstadoProyecto
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public CambioEstadoProyecto(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            ListasViewShhet = new List<ViewSheetNH>();
            ListaEstructura = new List<ViewSheetNH>();
            ListaSchudele = new List<ViewSheetNH>();
        }

        public List<ViewSheetNH> ListasViewShhet { get; private set; }
        public List<ViewSheetNH> ListaEstructura { get; private set; }
        public List<ViewSheetNH> ListaSchudele { get; private set; }


        public bool Ejecutar(string codigoCambio)
        {
            try
            {

                ConfigurarPerPRoject();

                //1.a) obtener todos los sheet
                var aux_ListaSheet = SeleccionarView.Getsheet(_uiapp.ActiveUIDocument).Where(c => !c.IsTemplate).ToList();

                foreach (ViewSheet item in aux_ListaSheet)
                {
                    ViewSheetNH _newviewNH = new ViewSheetNH(_uiapp, item);
                    if (_newviewNH.ObtenerDatos())
                    {
                        ListasViewShhet.Add(_newviewNH);
                    }
                }



                //1.b)obteneroi todsa las sheeduel
                var ListaSchudele_aux = TiposViewSchedule.ObtenerTodos(_doc).Where(c => !c.IsTemplate).ToList();
                foreach (var item in ListaSchudele_aux)
                {
                    ViewSheetNH _newviewNH = new ViewSheetNH(_uiapp, item);
                    if (_newviewNH.ObtenerDatos())
                    {
                        ListaSchudele.Add(_newviewNH);
                    }
                }

                //1.c)obteneroi todsa las vistas
                var ListaEstructura_auix = SeleccionarView.ObtenerView_Todos(_uiapp.ActiveUIDocument).Where(c => !c.IsTemplate).ToList();

                foreach (var item in ListaEstructura_auix)
                {
                    ViewSheetNH _newviewNH = new ViewSheetNH(_uiapp, item);
                    if (_newviewNH.ObtenerDatos())
                    {
                        ListaEstructura.Add(_newviewNH);
                    }
                }



                //desactivarTemplate
                using (Transaction t = new Transaction(_view.Document))
                {

                    t.Start($"DesactivarTemplate");
                    for (int i = 0; i < ListaEstructura.Count; i++)
                    {
                        if (ListaEstructura[i].IsTieneTemplateNh)
                            ListaEstructura[i]._view.DesactivarViewTemplate_SinTrans();
                    }
                    t.Commit();
                }


                OBtenerRevisones _obtenerRevisones = new OBtenerRevisones(_uiapp, codigoCambio);
                if (!_obtenerRevisones.M1_AddAdditionalRevisionsToSheet_Todos(ListasViewShhet)) return false;


                //para sehher
                var EstadoCAmbio = FactoryEstados.ListaEstadosProyecto.Where(c => c.CODIGOESTADODEAVANCE == codigoCambio).FirstOrDefault();
                var listaSheet = ListasViewShhet.Where(c => c.CODIGOESPECIALIDAD == "EST").ToList();

                // var ListaEstruc= ListaEstructura.Where(c => c.CODIGOESPECIALIDAD == "EST").ToList();

                listaSheet.AddRange(ListaEstructura);
                listaSheet.AddRange(ListaSchudele);
                using (Transaction tr = new Transaction(_doc, "CAmbiar parametros"))
                {
                    tr.Start();
                    for (int i = 0; i < listaSheet.Count; i++) //
                    {
                        var sheetNh = listaSheet[i];
                        sheetNh.CambiarEstado(EstadoCAmbio.CurrentRevision);
                    }
                    tr.Commit();
                }


                //activar template
                //desactivarTemplate
                using (Transaction t = new Transaction(_view.Document))
                {

                    t.Start($"Activartemplate");
                    for (int i = 0; i < ListaEstructura.Count; i++)
                    {
                        if (ListaEstructura[i].IsTieneTemplateNh)
                            ListaEstructura[i]._view.ActivarViewTemplate_SinTrans(ListaEstructura[i].IdTemplateAsignadov2);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CambioEstadoProyecto'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void ConfigurarPerPRoject()
        {

            RevisionSettings revisionSettings = Autodesk.Revit.DB.RevisionSettings.GetRevisionSettings(_doc);

            if (revisionSettings.RevisionNumbering != RevisionNumbering.PerProject)
            {
                using (Transaction trans = new Transaction(_doc, "Change Revision Numbering"))
                {
                    trans.Start();
                    revisionSettings.RevisionNumbering = RevisionNumbering.PerProject;
                    trans.Commit();
                }
            }
        }

        public bool Ejecutar2(string codigoCambio)
        {
            try
            {
                var aux_ListaSheet = SeleccionarView.Getsheet(_uiapp.ActiveUIDocument);
                List<Task> tasks = new List<Task>();

                //UtilStopWatch _UtilStopWatch = new UtilStopWatch();
                //_UtilStopWatch.IniciarMedicion(" inicio con task");

                for (int i = 0; i < aux_ListaSheet.Count; i++)
                {
                    var item = aux_ListaSheet[i];
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ViewSheetNH _newviewNH = new ViewSheetNH(_uiapp, item);
                        if (_newviewNH.ObtenerDatos())
                            ListasViewShhet.Add(_newviewNH);
                    }
                     ));
                }
                Task.WaitAll(tasks.ToArray());


                OBtenerRevisones _obtenerRevisones = new OBtenerRevisones(_uiapp, codigoCambio);
                _obtenerRevisones.M1_AddAdditionalRevisionsToSheet_Todos(ListasViewShhet);

                var EstadoCAmbio = FactoryEstados.ListaEstadosProyecto.Where(c => c.CODIGOESTADODEAVANCE == codigoCambio).FirstOrDefault();
                var listaSheet = ListasViewShhet.Where(c => c.CODIGOESPECIALIDAD == "EST").ToList();

                List<Task> taskstRANS = new List<Task>();
                UtilStopWatch _UtilStopWatch = new UtilStopWatch();

                using (Transaction tr = new Transaction(_doc, "CAmbiar parametros"))
                {
                    tr.Start();
                    for (int i = 0; i < listaSheet.Count; i++) //
                    {
                        var item = aux_ListaSheet[i];
                        var sheetNh = listaSheet[i];
                        sheetNh.CambiarEstado(EstadoCAmbio.CurrentRevision);
                    }

                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CambioEstadoProyecto2'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }

}
