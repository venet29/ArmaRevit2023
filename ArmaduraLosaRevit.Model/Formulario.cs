using System;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.IO;

using ArmaduraLosaRevit.Model.Elemento_Losa;
using Newtonsoft.Json;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.UTILES;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.ViewRang;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ahorro;
using ArmaduraLosaRevit.Model.Viewnh;

namespace ArmaduraLosaRevit.Model
{

    public class Formulario
    {
        #region 0) PROPIEDADES n:11
        //  ExternalCommandData commandData;
        UIApplication _uiapp;
        UIDocument uidoc;
        Document _doc;
        private View viewActual;
        Stopwatch timeMeasure = new Stopwatch();
        // public List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa_selec { get; set; }

        public List<ReferenciaRoom> ListaNH_RefereciaRoom { get; set; }
        //public List<PtosCrearSuples> ListasSuples_Vertical { get; set; }  // barra en sentido del pelota de losa
        //public List<PtosCrearSuples> ListasSuples_Horizontal { get; set; }              // barra perpendicul aa pelota de losa

        //public float _largominimo { get; set; }
        //public float _largoAhorro_barra { get; set; }
        //public float _largoAhorro_recorrido { get; set; }

        public bool IsImprimirayuda = false;



        /// <summary>
        /// si IsBuscarTipoBarra = true , el tipo de  barra se obtiene automatimente segun la presencia de continuidad de losa        
        /// </summary>
        private bool IsBuscarTipoBarra { get; set; }
        // public List<Polyline> ListasPoligonoCircuncribeBarraInferior { get; set; }
        // public List<Polyline> ListasPoligonoCircuncribeSuples { get; set; }

        public List<List<XYZ>> ListasPuntosCircuncribeBarraInferior_H { get; set; }
        public List<List<XYZ>> ListasPuntosCircuncribeBarraInferior_V { get; set; }

        public List<List<XYZ>> ListasPoligonoCircuncribeSuples_H { get; set; }
        public List<List<XYZ>> ListasPoligonoCircuncribeSuples_V { get; set; }
        public List<Family> ListaFamilyRebatShape { get; set; }
        #endregion


        #region 1) CONSTRUCTOR  n:2

        // CONSTRUCTOR 
        public Formulario(ExternalCommandData commandData)
        {

            // this.commandData = commandData;
            this._uiapp = commandData.Application;
            this.uidoc = _uiapp.ActiveUIDocument;
            this._doc = uidoc.Document;
            this.viewActual = _doc.ActiveView;


            ListaNH_RefereciaRoom = new List<ReferenciaRoom>();

            ListasPuntosCircuncribeBarraInferior_H = new List<List<XYZ>>();
            ListasPuntosCircuncribeBarraInferior_V = new List<List<XYZ>>();

            ListasPoligonoCircuncribeSuples_H = new List<List<XYZ>>();
            ListasPoligonoCircuncribeSuples_V = new List<List<XYZ>>();



            //rutina si no es necesaria pq obtiene todos los objetos ''NH_RefereciaRoom'
            //yield los almacena en --> lista 'ListaNH_RefereciaRoom'
            // GetListaNH_RefereciaRoom();
            //comunes.Create_ALayer("PERIMETROBARRAS", 2); //AMARILLO

        }

        public void FormatoPtoComa()
        {


            System.Globalization.CultureInfo r = new System.Globalization.CultureInfo("es-ES");
            r.NumberFormat.NumberDecimalSeparator = ".";   // SIMNOLO DE SEPARACION
            r.NumberFormat.CurrencyGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = r;
        }

        public void GetListaNH_RefereciaRoom()
        {
            if (ListaNH_RefereciaRoom == null) return;
            ListaNH_RefereciaRoom.Clear();

            if (ListasPuntosCircuncribeBarraInferior_H == null) return;
            ListasPuntosCircuncribeBarraInferior_H.Clear();

            if (ListasPuntosCircuncribeBarraInferior_V == null) return;
            ListasPuntosCircuncribeBarraInferior_V.Clear();

            if (ListasPoligonoCircuncribeSuples_H == null) return;
            ListasPoligonoCircuncribeSuples_H.Clear();

            if (ListasPoligonoCircuncribeSuples_V == null) return;
            ListasPoligonoCircuncribeSuples_V.Clear();

            //a)crea listas con losas y sUs datos (seleccionados)

            ReferenciaRoomListas administrador_ReferenciaRoom = new ReferenciaRoomListas(_uiapp);
            ListaNH_RefereciaRoom = administrador_ReferenciaRoom.GetLista_RefereciaRoom("GetSelectionAllNivelActual");
        }

        #endregion


        #region 2) Metodos n:4   -->   Importantes n:2  -- complementarios n:2
        public void ExportardatosParaIngenieria()
        {
            FormatoPtoComa();
   
            if (_doc.ActiveView.ViewType != ViewType.FloorPlan)
            {
                Util.ErrorMsg("Comando debe ser ejecutado en un view FloorPlan");
                return;
            }

            string viewNombre = _doc.ActiveView.Name;
            //para solo imprimr circulos en los putos de diseño
            IsImprimirayuda = false;
            #region OBtener DOtosS DE LOSAS

            Util.CambiarStadoSectionBo3d(_uiapp.ActiveUIDocument, ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR, false);

            //string cardName = cardNumber switch{ 13 => "King",  12 => "Queen",  11 => "Jack",  _ => "Pip card"};

            ReferenciaRoomHandler administrador_ReferenciaRoom = new ReferenciaRoomHandler(_uiapp) { IsImprimirAyuda = IsImprimirayuda };

            // calcula todos los datos de la room, datos room, largos minimos, suples y tipo de losa
            administrador_ReferenciaRoom.ReferenciaRoomListas.GetLista_RefereciaRoom("SelectConMouse");

            if (administrador_ReferenciaRoom.ReferenciaRoomListas.Isok == false)
                return;

            //obtiene lista obj 'BoundarySegmentRoomsGeom'
            administrador_ReferenciaRoom.ReferenciaRoomListas.M1_GetLista_BoundarySegment();

            if (administrador_ReferenciaRoom.ReferenciaRoomListas.Lista_RefereciaRoom.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Exportacion de datos no finalizadas. Cero Room correctamente analisados", "Mensaje");
                return;
            }

            if (IsImprimirayuda) return;

            //crea las barras inferiores

            administrador_ReferenciaRoom.CrearBarrasInferiores();

            //c)Obtener Suples  - ( obtienen todos los suples de todas las pelotas seleccionados)
            administrador_ReferenciaRoom.CrearSuples();

            #endregion

            //exprotar a json
            CreadoJson.ExportarAJson(administrador_ReferenciaRoom.ReferenciaRoomListas.Lista_RefereciaRoom, viewNombre);
            //#region Exportar datos

        }


        /// <summary>
        ///  Importar barras desde un archivo json para autodibujar
        /// </summary>
        public void ImportarBarrasParaDibujo(TipoConfiguracionBarra tipoBarraParaDibujar, ConfiguracionAhorro configuracionAhorro, bool IsSoloCopiarDatos = false, bool IsBuscarTipoBarra = true)
        {
            FormatoPtoComa();
            //tipoBarraParaDibujar = tipoDeBarra.suple;
            string message = "";

            if (_doc.ActiveView.ViewType != ViewType.FloorPlan)
            {
                Util.ErrorMsg("Comando debe ser ejecutado en un view FloorPlan");
                return;
            }

            List<XYZ> listaPtos = new List<XYZ>();
            List<List<XYZ>> listaPtos_Vertical = new List<List<XYZ>>();
            List<List<XYZ>> listaPtos_Horizontal = new List<List<XYZ>>();
            ListaFamilyRebatShape = new List<Family>();


            //    ConfiguracionAhorro configuracionAhorro = new ConfiguracionAhorro(_largoAhorro_recorrido, _largoAhorro_barra, _largominimo);
            this.IsBuscarTipoBarra = IsBuscarTipoBarra;

            //desactivar section box
            Util.CambiarStadoSectionBo3d(_uiapp.ActiveUIDocument, ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR, false);

            // IMPORT:recarga los datos de loSas poligono de losas


            this.GetListaNH_RefereciaRoom();


            bool IsCargarConArchivo = true;

            ReferenciaRoomHandler NH_PoligonoDeRoom_analisis = new ReferenciaRoomHandler(_uiapp);
            NH_PoligonoDeRoom_analisis.uidoc = uidoc;

            bool resultado = false;

            if (IsCargarConArchivo) //cargar archivo desde json
            {
                resultado = NH_PoligonoDeRoom_analisis.LeerArchivoJson();
            }
            else// seleccionar y generar barras directamente
            {
                // FALTA IMPLEMENTAR  auto generar archivo json y leer
            }

            if (!resultado) return;
            if (IsSoloCopiarDatos)
            {
                Util.InfoMsg("Solo se realizo la copia de datos en room. Copia realizada correctamente.");
                return;
            }

            timeMeasure.Start();
            // 1) DIBUJAR BARRA INFERIOR
            #region Dibujando barras

            ///<summary>
            ///dibuja las barras VErticales yhorizontales
            ///message : mensaje del error o ok
            ///listaPtos_Vertical  : lista que contiene los puntos de los contornos de las barras dibujadas
            ///para no dibujar barra donde ya existe
            ///</summary>
            ///Barrasverticales


            bool isDibujarH = false;
            bool isDibujarV = false;
            bool isSuple = false;


            if (tipoBarraParaDibujar == TipoConfiguracionBarra.suple)
            {
                isSuple = true;
                isDibujarH = false;
                isDibujarV = false;
            }
            else if (tipoBarraParaDibujar == TipoConfiguracionBarra.refuerzoInferior)
            {
                isSuple = false;
                isDibujarH = true;
                isDibujarV = true;
            }


#if false //DEBUG
            isDibujarH = true;
            isDibujarV = true;
            isSuple = false;
#endif

            //importante
            DatosDiseño.DISENO_TIPO_F1 = configuracionAhorro.DISENO_TIPO_F1;
            DatosDiseño.DISENO_VALIDAR_ESPESOR = configuracionAhorro.DISENO_VALIDAR_ESPESOR;


            //NOTA SE REINICIA PARA EVITAR EL PORBLEMA DEL UNDO
            //TiposPathReinformentSymbolElement.ObtenerPathReinfDefaul_forzado(_doc);

            //Manejador_UpDateMoverPathSymbol _manejadorUpdateRebar = new Manejador_UpDateMoverPathSymbol(_uiapp);
            
            Manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol(_uiapp);
            //Manejador_UpDateMoverTagPathSymbol _ManejadorTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(_uiapp);
            Manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol(_uiapp);
          
            //Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(_uiapp);
            //_Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();


            try
            {

                //barra vertical
                if (isDibujarV)
                {
                    if (NH_PoligonoDeRoom_analisis.ReferenciaRoomListas.ListaPtos_Vertical_Barra.Count == 0)
                    {
                        Util.ErrorMsg($"No se exportaron barras Verticales");

                    }
                    else
                    {
                        if (DibujarBarrasDeListaPtos_Auto(ref message,
                            ref listaPtos_Vertical,
                            NH_PoligonoDeRoom_analisis.ReferenciaRoomListas.ListaPtos_Vertical_Barra,
                            configuracionAhorro, IsBuscarTipoBarra: true) == Result.Failed) return;
                    };
                }

                //Barras horizontales
                if (isDibujarH)
                {
                    if (NH_PoligonoDeRoom_analisis.ReferenciaRoomListas.ListaPtos_Horizontal_Barra.Count == 0)
                    {
                        Util.ErrorMsg($"No se exportaron barras Horizontales");
                    }
                    else
                    {
                        if (DibujarBarrasDeListaPtos_Auto(ref message,
                            ref listaPtos_Horizontal,
                            NH_PoligonoDeRoom_analisis.ReferenciaRoomListas.ListaPtos_Horizontal_Barra,
                            configuracionAhorro, IsBuscarTipoBarra: true) == Result.Failed) return;
                    }
                }

                //suples
                if (isSuple)
                {
                    if (NH_PoligonoDeRoom_analisis.ListasSuples_Todos.Count == 0)
                    {
                        Util.ErrorMsg($"No se exportaron barras Suples");
                    }
                    else
                        DibujarSupleDeListaPtos_auto(ref message, ref listaPtos_Horizontal, NH_PoligonoDeRoom_analisis.ListasSuples_Todos, configuracionAhorro);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
            }
            Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(_uiapp);
            Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(_uiapp);
          //  _Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();
            #endregion

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = timeMeasure.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            System.Windows.Forms.MessageBox.Show($"Finaliza la exportacion y dibujo de barras  \n Tiempo Desarrollo: {elapsedTime} ", "Confirmation");
            //throw new NotImplementedException();
        }





        private Result DibujarBarrasDeListaPtos_Auto(ref string message,
                                                ref List<List<XYZ>> listaPtos_Contenedor,
                                                List<NH_RefereciaCrearBarra> ListaNH_RefereciaCrearBarra,
                                                ConfiguracionAhorro configuracionAhorro,
                                                bool IsBuscarTipoBarra)
        {

#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
            Result result = Result.Failed;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            int conta = 1;
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            List<BarraRoom> ListaPathReinforcementCreados = new List<BarraRoom>();
            List<BarraRoom> ListaPathReinforcementCreados_izq = new List<BarraRoom>();
            List<BarraRoom> ListaPathReinforcementCreados_dere = new List<BarraRoom>();




            if (conta > 1) return Result.Succeeded;
            bool saliraux = false;
            try
            {
                foreach (NH_RefereciaCrearBarra pto in ListaNH_RefereciaCrearBarra)
                {
                    conta += 1;
                    if (saliraux) return Result.Succeeded;
                    try
                    {
                        //string result1 = "";
                        //Stopwatch timeMeasureTotal = Stopwatch.StartNew();
                        pto.tipoBarra = "fautof1_sup";

                        if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(pto.PosicionPto_Barra, listaPtos_Contenedor) == true) continue;


                        BarraRoom newBarralosa = new BarraRoom(_uiapp, pto, configuracionAhorro, IsBuscarTipoBarra);

                        if (VariablesSistemas.IsDibujarS4)
                            newBarralosa.CrearBarraExtremos_Auto();
                        ListaPathReinforcementCreados.Add(newBarralosa);

                        if (newBarralosa.newBarralosa_izq != null && VariablesSistemas.IsDibujarS4)
                            ListaPathReinforcementCreados_izq.Add(newBarralosa.newBarralosa_izq);

                        if (newBarralosa.newBarralosa_dere != null && VariablesSistemas.IsDibujarS4)
                            ListaPathReinforcementCreados_dere.Add(newBarralosa.newBarralosa_dere);

                        listaPtos_Contenedor.Add(newBarralosa.ListaPtosPerimetroBarras);

                    }
                    catch (Exception ex)
                    {
                        // If there are something wrong, give error information and return failed
                        message = ex.Message;
                        result = Autodesk.Revit.UI.Result.Failed;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en la barra {conta}    ex{ex.Message}");
                return Result.Failed; ;
            }

            //*2)--------------------------------------------------------------------------------------------------

            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("CreatePathReinforcement inferior_Sup-NH");
                    if (!DibujarBarras(ListaPathReinforcementCreados)) return Result.Failed;
                    if (!DibujarBarras(ListaPathReinforcementCreados_izq)) return Result.Failed;
                    if (!DibujarBarras(ListaPathReinforcementCreados_dere)) return Result.Failed;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en la barra {conta}    ex{ex.Message}");
                return Result.Failed; ;
            }


            //*3)--------------------------------------------------------------------------------------------------
            BarraRoom_ocultar_view _barraRoom_ocultar_view = new BarraRoom_ocultar_view(_uiapp);
            _barraRoom_ocultar_view.Ejecutar(ListaPathReinforcementCreados);

            LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return Result.Succeeded;

        }

        private static bool DibujarBarras(List<BarraRoom> ListaPathReinforcementCreados_)
        {
            int conta = 0;
            foreach (BarraRoom newBarralosa in ListaPathReinforcementCreados_)
            {
                conta += 1;
                //if (saliraux) return Result.Succeeded;
                try
                {
                    if (newBarralosa.statusbarra == Result.Succeeded)
                    {
                        if (newBarralosa.TipoBarraStr == "f1_SUP")
                            newBarralosa.BUscarViewSUPERIOR();
                        newBarralosa.CrearBarraAuto(newBarralosa.CurvesPathreiforment,
                                                newBarralosa.LargoPathreiforment,
                                                newBarralosa.nombreSimboloPathReinforcement,
                                                newBarralosa.diametroEnMM,
                                                newBarralosa.Espaciamiento,
                                                XYZ.Zero);
                        if (newBarralosa.statusbarra != Result.Succeeded)
                        {
                            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Error al crear barra. Desea salir?", "Salir", System.Windows.Forms.MessageBoxButtons.YesNo);

                            if (result == System.Windows.Forms.DialogResult.Yes) return false;

                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Datos de barra presentan error" + newBarralosa.message);
                        // result = Result.Succeeded;
                    }
                    //  result = Result.Succeeded;
                }
                catch (Exception ex)
                {
                    // If there are something wrong, give error information and return failed
                    Debug.WriteLine($" error en la barra {conta}    ex{ex.Message}");
                    // result = Autodesk.Revit.UI.Result.Failed;
                }

            }


            return true;
        }

        private Result DibujarSupleDeListaPtos_aux(ref string message, ref List<List<XYZ>> listaPtos_Contenedor, List<NH_RefereciaCrearSuple> ListaNH_RefereciaCrearSuples, ConfiguracionAhorro configuracionAhorro)
        {
            Result result = Result.Failed;

            int conta = 1;
            bool saliraux = false;
            foreach (NH_RefereciaCrearSuple pto in ListaNH_RefereciaCrearSuples)
            {
                pto.tipoBarra = configuracionAhorro.remplazosx;
                conta += 1;
                if (saliraux) return Result.Succeeded;
                try
                {


                    if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(pto.PosicionPtoSupleInicial, listaPtos_Contenedor) == true) continue;
                    //dibuja circulo 
                    //Util.CrearCirculo(0.4, pto.PosicionPto_Barra, uidoc);

                    BarraRoom newBarralosa = new BarraRoom(_uiapp, pto);
                    if (newBarralosa.statusbarra == Result.Succeeded)
                    {


                        newBarralosa.CrearBarra(newBarralosa.CurvesPathreiforment,
                                                newBarralosa.LargoPathreiforment,
                                                newBarralosa.nombreSimboloPathReinforcement,
                                                newBarralosa.diametroEnMM,
                                       newBarralosa.Espaciamiento,
                                       XYZ.Zero);
                        if (newBarralosa.statusbarra != Result.Succeeded)
                        {
                            TaskDialog.Show("Error", newBarralosa.message);
                            result = Result.Succeeded;
                        }
                        listaPtos_Contenedor.Add(newBarralosa.ListaPtosPerimetroBarras);
                    }
                    else
                    {
                        TaskDialog.Show("Error", newBarralosa.message);
                        result = Result.Succeeded;
                    }

                    result = Result.Succeeded;

                }
                catch (Exception ex)
                {
                    // If there are something wrong, give error information and return failed
                    message = ex.Message;
                    result = Autodesk.Revit.UI.Result.Failed;
                }


            }

            return result;

        }



        private Result DibujarSupleDeListaPtos_auto(ref string message, ref List<List<XYZ>> listaPtos_Contenedor, List<NH_RefereciaCrearSuple> ListaNH_RefereciaCrearSuples, ConfiguracionAhorro configuracionAhorro)
        {
            Result result = Result.Failed;


            List<BarraRoom> ListaPathReinforcementCreados = new List<BarraRoom>();

            int conta = 1;
            bool saliraux = false;

            //1)----------------------------------------------------------------
            try
            {

                foreach (NH_RefereciaCrearSuple pto in ListaNH_RefereciaCrearSuples)
                {
                    pto.tipoBarra = configuracionAhorro.remplazosx;
                    conta += 1;
                    if (saliraux) return Result.Succeeded;
                    try
                    {


                        if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(pto.PosicionPtoSupleInicial, listaPtos_Contenedor) == true) continue;
                        //dibuja circulo 
                        //Util.CrearCirculo(0.4, pto.PosicionPto_Barra, uidoc);

                        BarraRoom newBarralosa = new BarraRoom(_uiapp, pto);
                        ListaPathReinforcementCreados.Add(newBarralosa);
                        listaPtos_Contenedor.Add(newBarralosa.ListaPtosPerimetroBarras);
                        result = Result.Succeeded;

                    }
                    catch (Exception ex)
                    {
                        // If there are something wrong, give error information and return failed
                        message = ex.Message;
                        result = Autodesk.Revit.UI.Result.Failed;
                    }


                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en la barra {conta}    ex{ex.Message}");
                return Result.Failed; ;
            }



            //*2)--------------------------------------------------------------------------------------------------

            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("CreatePathReinforcement Superior-NH");
                    foreach (BarraRoom newBarralosa in ListaPathReinforcementCreados)
                    {

                        conta += 1;
                        if (saliraux) return Result.Succeeded;
                        try
                        {

                            if (newBarralosa.statusbarra == Result.Succeeded)
                            {

                                newBarralosa.CrearBarraAuto(newBarralosa.CurvesPathreiforment,
                                                        newBarralosa.LargoPathreiforment,
                                                        newBarralosa.nombreSimboloPathReinforcement,
                                                        newBarralosa.diametroEnMM,
                                                        newBarralosa.Espaciamiento,
                                                        XYZ.Zero);
                                if (newBarralosa.statusbarra != Result.Succeeded)
                                {
                                    TaskDialog.Show("Error", newBarralosa.message);
                                    return Result.Succeeded;
                                }

                            }
                            else
                            {
                                TaskDialog.Show("Error", newBarralosa.message);
                                result = Result.Succeeded;
                            }
                            result = Result.Succeeded;
                        }
                        catch (Exception ex)
                        {
                            // If there are something wrong, give error information and return failed
                            message = ex.Message;
                            result = Autodesk.Revit.UI.Result.Failed;
                        }
                    }

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en la barra {conta}    ex{ex.Message}");
                return Result.Failed; ;
            }


            //*3)--------------------------------------------------------------------------------------------------
            BarraRoom_ocultar_view _barraRoom_ocultar_view = new BarraRoom_ocultar_view(_uiapp);
            _barraRoom_ocultar_view.Ejecutar(ListaPathReinforcementCreados);

            return result;

        }



        #endregion
    }
}
