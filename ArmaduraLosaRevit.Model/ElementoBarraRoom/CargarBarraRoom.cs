using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Text;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{
    public class CargarBarraRoomDTO
    {
        public string tipobarra { get; set; }
        public UbicacionLosa ubicacion { get; set; }
        public int DiamnetroMM { get; set; }
        public double espaciamientoCm { get; set; } 
        public bool Dibujar_principal { get; set; } 
        public bool DibujarSecundarios { get; set; } 
        public PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FxxDTO_ { get; set; }
        public Ui_pathSymbolDTO Ui_pathSymbolDTO_ { get; internal set; }

        public CargarBarraRoomDTO(string tipobarra, UbicacionLosa ubicacion,
                                        int DiamnetroMM = 8, double espaciamientoCm = 20,
                                    bool Dibujar_principal = true, bool DibujarSecundarios = true,
                                    PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FxxDTO_ = null)
        {
            this.tipobarra = tipobarra;
            this.ubicacion = ubicacion;
            this.DiamnetroMM = DiamnetroMM;
            this.espaciamientoCm = espaciamientoCm;
            this.Dibujar_principal = Dibujar_principal;
            this.DibujarSecundarios = DibujarSecundarios;

            if (PathSymbol_REbarshape_FxxDTO_ == null)
            {
                this.PathSymbol_REbarshape_FxxDTO_ = new PathSymbol_REbarshape_FxxDTO()
                {
                    IsOK = false, // para no considere esta configuracion, si es fase toma los largo estandar de esta clase : 'FactoryPathSymbol_REbarshape_FxxDTO.cs'
                    CopiarFamiliasDiferentesPatas = false  // para no crear las clases  defaul de las direferentes tipos de clases
                };


            }
            else
                this.PathSymbol_REbarshape_FxxDTO_ = PathSymbol_REbarshape_FxxDTO_;

        }

    }

    //**************************************  DTO

    public class CargarBarraRoom
    {
        public static BarraRoom _newBarralosa { get; private set; }


        //1)
        public static Autodesk.Revit.UI.Result Cargar(UIApplication uiapp, string tipobarra, UbicacionLosa ubicacion,
                                     int DiamnetroMM = 8, double espaciamientoCm = 20,
                                 bool Dibujar_principal = true, bool DibujarSecundarios = true)
        {

            CargarBarraRoomDTO _CargarBarraRoomDTO = new CargarBarraRoomDTO(tipobarra, ubicacion, DiamnetroMM, espaciamientoCm, Dibujar_principal, DibujarSecundarios);
            return Cargar(uiapp, _CargarBarraRoomDTO);

        }

        //2)
        public static Autodesk.Revit.UI.Result Cargar(UIApplication _uiapp, CargarBarraRoomDTO _CargarBarraRoomDTO)
        {

            string tipobarra = _CargarBarraRoomDTO.tipobarra;
            UbicacionLosa ubicacion = _CargarBarraRoomDTO.ubicacion;
            int DiamnetroMM = _CargarBarraRoomDTO.DiamnetroMM;

            double espaciamientoCm = _CargarBarraRoomDTO.espaciamientoCm;
            bool Dibujar_principal = _CargarBarraRoomDTO.Dibujar_principal;
            bool DibujarSecundarios = _CargarBarraRoomDTO.DibujarSecundarios;



            //TaskDialog.Show("ERRRO","SDFSDF");
            ConstNH.sbLog = new StringBuilder();

            View prueb = _uiapp.ActiveUIDocument.ActiveView;

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return Autodesk.Revit.UI.Result.Failed;

            //NOTA SE REINICIA PARA EVITAR EL PORBLEMA DEL UNDO
            //TiposPathReinformentSymbolElement.ObtenerPathReinfDefaul_forzado(uiapp.ActiveUIDocument.Document);

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            var  resutlnh = Autodesk.Revit.UI.Result.Succeeded;
            try
            {


                UtilitarioFallasDialogosCarga.Cargar(_uiapp);
                VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM = true;
                DatosDiseño.IS_PATHREIN_AJUSTADO = VariablesSistemas.IsAjusteBarra_Recorrido;
                DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = VariablesSistemas.IsAjusteBarra_Largo;

                if (tipobarra == "s4") Dibujar_principal = false;

                DatosDiseñoDto _DatosDiseñoDto = new DatosDiseñoDto();

                _DatosDiseñoDto.PathSymbol_REbarshape_FXXDTO_ = _CargarBarraRoomDTO.PathSymbol_REbarshape_FxxDTO_;
                _DatosDiseñoDto.Ui_pathSymbolDTO_ = _CargarBarraRoomDTO.Ui_pathSymbolDTO_;

                if (tipobarra == "s1" || tipobarra == "s2" || tipobarra == "s3" || tipobarra == "s4")
                {
                    _DatosDiseñoDto.IsConsiderarDatosIniciales = true;
                    _DatosDiseñoDto.DiamnetroMM = DiamnetroMM;
                    _DatosDiseñoDto.espaciamientoCm = espaciamientoCm;
                }

                //** para denombrar familia pathsymbol 13-04-2022
                ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
                if (_ManejadorReNombrar.IsFamiliasAntiguas())
                    _ManejadorReNombrar.Renombarra();

                //**
                bool aux_continua = true;
                while (aux_continua)
                {

                    _newBarralosa = new BarraRoom(_uiapp, tipobarra, ubicacion, _DatosDiseñoDto, false);   //f1_SUP

                    if (_newBarralosa.statusbarra == Autodesk.Revit.UI.Result.Succeeded)
                    {
                        if (Dibujar_principal)
                            _newBarralosa.CrearBarra(_newBarralosa.CurvesPathreiforment,
                                                        _newBarralosa.LargoPathreiforment,
                                                        _newBarralosa.nombreSimboloPathReinforcement,
                                                        _newBarralosa.diametroEnMM,
                                                        _newBarralosa.Espaciamiento,
                                                        XYZ.Zero);

                        if (_newBarralosa.statusbarra != Autodesk.Revit.UI.Result.Succeeded)
                        {
                            aux_continua = false;
                            _newBarralosa.message = $"A) Error al crear barras";

                            Util.ErrorMsg(_newBarralosa.message);
                            return Autodesk.Revit.UI.Result.Succeeded;
                        }

                        if (tipobarra == "s4") tipobarra = "f1_SUP";

                        if (DibujarSecundarios == true)
                        {
                            //crea barra a izq-inf y dere-sup
                            if (tipobarra == "f1_SUP" ||
                                    ((tipobarra == "fautof1_sup" || tipobarra == "fautoahorrof1_sup" || tipobarra == "fauto") &&
                                     (_newBarralosa.TipoBarra_izq_Inf != "" || _newBarralosa.TipoBarra_dere_sup != "")) &&
                                     VariablesSistemas.IsDibujarS4 == true
                                     )
                            {
                                _newBarralosa.BuscarDireccion_F1SUP();

                                _newBarralosa.CrearBarraExtremos();
                            }
                        }

                        if (VariablesSistemas.IsReSeleccionarPuntoRango)
                        {
                            SelecionarPathSeleccionadoParteSuperior _SelecionarPathCreadoParteSuperior = new SelecionarPathSeleccionadoParteSuperior(_uiapp, _newBarralosa._PathReinSpanSymbol);
                            _SelecionarPathCreadoParteSuperior.Ejecutar();
                        }
                    }
                    else
                    {
                        aux_continua = false;
                        // _newBarralosa.message = "Error al crear barras_b";
                        //   if (_newBarralosa.statusbarra==Result.Failed)
                        //    TaskDialog.Show("Error", _newBarralosa.message);
                    }
                }

                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                
            }
            catch (Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                // If there are something wrong, give error information and return failed
                LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
                Util.ErrorMsg($"B)Error al crear barra:" + ex.Message);
                resutlnh= Autodesk.Revit.UI.Result.Failed;
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);
            return resutlnh;

        }


    }
}
