using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ViewportnNH;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using System.Collections.ObjectModel;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using System;
using ArmaduraLosaRevit.Model.ViewportnNH.Servicios;

namespace ArmaduraLosaRevit.Model.ViewportnNH.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_ViewPortNH
    {

        public static void M1_Ejecutar(UI_ViewPOrtNH _ui, UIApplication _uiapp)
        {
            UtilStopWatch _utilStopWatch = new UtilStopWatch();
            Document _doc = _uiapp.ActiveUIDocument.Document;
            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = _ui.BotonOprimido;

            if (tipoPosiicon == "CrearEstructutar" || tipoPosiicon == "CrearArmaduraLosa" || tipoPosiicon == "CrearArmaduraEleva")
            {
                List<ViewDTO> ListaViewDTO = default;
                if (tipoPosiicon == "CrearEstructutar")
                    ListaViewDTO = _ui.ListaEstructura.ToList();
                else if (tipoPosiicon == "CrearArmaduraLosa")
                    ListaViewDTO = _ui.ListaLosa.ToList();
                else if (tipoPosiicon == "CrearArmaduraEleva")
                    ListaViewDTO = _ui.ListaElev.ToList();
                else
                    return;

                var viewPOrtTipo = TiposViewportType.ObtenerTiposView("TITULO VENTANA (SIN NIVEL)", _doc);
                var temaplateSheet = TiposViewSheet.ObtenerFamiliaPAraSheet(_doc, "FORMATO GENERAL DELPORTE (A0)");

                var listaIsChecket = ListaViewDTO.Where(c => c.IsSelected).ToList();
                var ListaSheet = listaIsChecket.GroupBy(c => c.NumeroSheet).Select(c => c.Key).ToList();
                var cantidadSheet = ListaSheet.Count();
                var result = Util.InfoMsg_YesNo($"confirma que desea crear {cantidadSheet} sheet?");
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    _utilStopWatch.IniciarMedicion();
                    try
                    {
                        using (Transaction t = new Transaction(_doc, "Create a new ViewSheet"))
                        {
                            t.Start();
                            if (tipoPosiicon == "CrearArmaduraEleva")
                            {


                                for (int i = 0; i < ListaSheet.Count; i++)
                                {
                                    var NombreViewSheet = ListaSheet[i];

                                    //obtiene la geometris
                                    var listaMismoSheet = listaIsChecket.Where(c => c.NumeroSheet == NombreViewSheet).ToList();
                                    ServicioGenerarGoemtria _ServicioGenerarGoemtria = new ServicioGenerarGoemtria(_uiapp, NombreViewSheet, listaMismoSheet);
                                    if (!_ServicioGenerarGoemtria.Calcular(10)) continue;

                                    ManejadorCreadorSheetV2 _ManejadorViewportnNH = new ManejadorCreadorSheetV2(_uiapp, _ServicioGenerarGoemtria.ListaViewGeom, viewPOrtTipo, temaplateSheet);
                                   // _ManejadorViewportnNH.BorrarPortDeOtroSheet_conIgualNombreView_SinTrans();
                                    _ManejadorViewportnNH.CreateSheetView_SinTrans();
                                    _utilStopWatch.StopYContinuar($"cont a) fin caso {i}: ----------> ", false);
                                }
                            }
                            else
                            {

                                for (int i = 0; i < listaIsChecket.Count; i++)
                                {
                                    var viewEstructura = listaIsChecket[i];
                                    var listaMismoSheet = new List<ViewDTO>() { listaIsChecket[i] };
                                    ServicioGenerarGoemtria _ServicioGenerarGoemtria = new ServicioGenerarGoemtria(_uiapp, listaIsChecket[i].NumeroSheet, listaMismoSheet);
                                    if (!_ServicioGenerarGoemtria.Calcular()) continue;


                                    ManejadorViewportnNH _ManejadorViewportnNH = new ManejadorViewportnNH(_uiapp, viewEstructura, viewPOrtTipo, temaplateSheet);
                                    _ManejadorViewportnNH.BorrarPortDeOtroSheet_conIgualNombreView_SinTrans();
                                    _ManejadorViewportnNH.CreateSheetView_SinTrans();
                                    _utilStopWatch.StopYContinuar($"cont a) fin caso {i}: ----------> ", false);
                                }
                            }
                            t.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error al crear sheet : ex{ex.Message}");
                    }

                    if (listaIsChecket.Count > 0) Util.InfoMsg("Proceso Terminado.");
                    _utilStopWatch.Terminar($"cont: ----------> sheet ", false);
                }
            }
            else if (tipoPosiicon == "BorrarEstructura" || tipoPosiicon == "BorrarArmaduraLosa" || tipoPosiicon == "BorrarArmaduraEleva")
            {
                List<ViewDTO> ListaViewDTO = default;
                if (tipoPosiicon == "BorrarEstructura")
                    ListaViewDTO = _ui.ListaEstructura.ToList();
                else if (tipoPosiicon == "BorrarArmaduraLosa")
                    ListaViewDTO = _ui.ListaLosa.ToList();
                else if (tipoPosiicon == "BorrarArmaduraEleva")
                    ListaViewDTO = _ui.ListaElev.ToList();
                else
                    return;

                var listaIsChecket = ListaViewDTO.Where(c => c.IsSelected).ToList();

                try
                {
                    using (Transaction t = new Transaction(_doc, "Create a new ViewSheet"))
                    {
                        t.Start();
                        for (int i = 0; i < listaIsChecket.Count; i++)
                        {
                            var viewEstructura = listaIsChecket[i];
                            ManejadorViewportnNH _ManejadorViewportnNH = new ManejadorViewportnNH(_uiapp, viewEstructura);
                            _ManejadorViewportnNH.OBtenerSheet_Y_BorrarViewPOrt_SiTiene();
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al crear sheet : ex{ex.Message}");
                }

                if (listaIsChecket.Count > 0)
                {
                    _ui.LoadData();
                    Util.InfoMsg("Proceso Terminado.");
                }
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
        }
    }
}