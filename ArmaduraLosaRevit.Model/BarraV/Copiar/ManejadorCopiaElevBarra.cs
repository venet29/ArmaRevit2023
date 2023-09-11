using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.BarraV.Copiar.Servicion;
using ArmaduraLosaRevit.Model.BarraV.Copiar.wpf;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar
{
    public class ManejadorCopiaElevBarra
    {
        private UIApplication _uiapp;
        //private  CopiarRebarWpf _copiarRebarWpf;
        private  List<double> listaLevelZ;
        private readonly string _nombreNIvelReferencia;
        private Document _doc;
        private View _view;
        private CopiaBarrasElev _CopiaBarrasElev;
        Stopwatch timeMeasure = new Stopwatch();
        public static int barrasCopiadas;
        public ManejadorCopiaElevBarra(UIApplication uiapp, List<double> lsitaNIveles, string NombreNIvelReferencia = "PISO 1°")
        {
            this._uiapp = uiapp;
            //this._copiarRebarWpf = _CopiarRebarWpf;
            listaLevelZ = lsitaNIveles;
            this._nombreNIvelReferencia = NombreNIvelReferencia;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            barrasCopiadas = 0;
        }

        public bool Ejecutar()
        {
            timeMeasure.Start();
         
            _CopiaBarrasElev = null;
            try
            {
                List<ElementId> ListaIdemVigas = new List<ElementId>();

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CopiarBarrasIdem-NH");

                    for (int i = 0; i < listaLevelZ.Count; i++)
                    {
                        double valorZ = listaLevelZ[i];

                        //copiar datos
                        bool resultCopiar = M1_CopiarBarrasIdem(valorZ);
                        if (resultCopiar == false)
                        {
                            t.RollBack();
                            return false;
                        }
                        //******************************************
                        //cambiar color
                        bool resultCOlor = M2_CambiarColoresBarrasCopiadas();
                        if (resultCOlor == false)
                        {
                            t.RollBack();
                            return false;
                        }
                    }

                    // creada 14-07-2023-- borra en furtuto
                    var tipo = TiposTextNote.ObtenerTextNote(FActoryTipoTextNote.TextoVigaIdem, _doc); ;
                    if (tipo == null)
                    {
                        var ListaTipoNote = FActoryTipoTextNote.ObtenerLista();
                        CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.TextoVigaIdem, TipoCOloresTexto.Amarillo);
                        _CrearTexNote.M2_CrearListaTipoText_ConTrans(ListaTipoNote.Where(c=>c._Nombre== FActoryTipoTextNote.TextoVigaIdem).ToList());
                    }

                    var listaBArrasCOpiadas=_CopiaBarrasElev._listaWrapperFormatoRebar_final;
                    Servicio_CopiarTextoIdem _Servicio_CopiarTextoIdem = new Servicio_CopiarTextoIdem(_uiapp, listaBArrasCOpiadas, _nombreNIvelReferencia, FActoryTipoTextNote.TextoVigaIdem);
                    _Servicio_CopiarTextoIdem.ObtenerPtos();
                    _Servicio_CopiarTextoIdem.CrearTexto(listaLevelZ);

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }

            TimeSpan ts = timeMeasure.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Util.InfoMsg($"Proceso terminado. Tiempio : {elapsedTime}\nBarras Copiada  : {barrasCopiadas}");

            return true;
        }



        private bool M1_CopiarBarrasIdem(double valorZ)
        {

            try
            {
                if (_view.ViewType != ViewType.Section)
                {
                    Util.ErrorMsg("Vista debe ser ViewSection");
                    return false;
                }


                SeleccionarBarrasREbar_InfoComppleta _seleccionarBarrasRebar_InfoCompleta = new SeleccionarBarrasREbar_InfoComppleta(_uiapp, valorZ);
                if (!_seleccionarBarrasRebar_InfoCompleta.GenerarLista()) return false;

                _CopiaBarrasElev = new CopiaBarrasElev(_uiapp, _seleccionarBarrasRebar_InfoCompleta);
                _CopiaBarrasElev.CopiarSinTrasn();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error al copiar barras Idem \n  ex: {ex.Message}");
                return false;
            }
            return true;
        }

        private bool M2_CambiarColoresBarrasCopiadas()
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("CopiarCAmbiarColor-NH");
                    var listaFinal = _CopiaBarrasElev._listaWrapperFormatoRebar_final.GroupBy(c => c.colorBarras);

                    foreach (var grupoPorColor in listaFinal)
                    {
                        Color colorRebar = grupoPorColor.Key;
                        var Lista_BArrasPorPierYOrientacion = grupoPorColor.ToList();
                        if (Lista_BArrasPorPierYOrientacion.Count == 0) continue;

                        var ListaRebarId = Lista_BArrasPorPierYOrientacion.Select(c => c.Barra.Id).ToList();
                        CambiarColorBarras_Service cambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp);

                        bool mitadIntensidad = false;

                        if (!(colorRebar == null) && colorRebar.IsValid)
                        {
                            mitadIntensidad = (!colorRebar.IsValid
                                                    ? false
                                                    : !(colorRebar?.Blue == 255 && colorRebar?.Green == 0 && colorRebar?.Red == 255));

                        }

                        cambiarColorBarras_Service.M1_3_CAmbiarColorPorColor_sintrans(ListaRebarId, colorRebar, mitadIntensidad);
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error al cambiar color en barras copiadas\n  ex: {ex.Message}");
                string msj = ex.Message;
                return false;
            }
            return true;
        }

    }
}
