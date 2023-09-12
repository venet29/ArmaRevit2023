using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.RebarLosa.Geom;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UpdateGenerar;

namespace ArmaduraLosaRevit.Model.RebarLosa
{



    public class ManejadorRebarLosaInclinada: ManejadorRebarBase
    {
     //   private  UIApplication _uiapp;

     //   List<IRebarLosa> _ListIRebarLosa;

        public ManejadorRebarLosaInclinada(UIApplication uiapp):base(uiapp) 
        {
          //  this._uiapp = uiapp;
          //  this._ListIRebarLosa = new List<IRebarLosa>();
        }
        public bool BarraInferiores(TipoBarra tipobarra, UbicacionLosa ubicacion)
        {     
            try
            {
                //obtiene geometria de los ptos
                //1)coordendas
                BarraRoom barraRoom = new BarraRoom(_uiapp, tipobarra.ToString(), ubicacion);

                //para losa inclinada
                if (tipobarra == TipoBarra.f4_incli || tipobarra == TipoBarra.f3_incli || tipobarra == TipoBarra.f1_incli)
                    barraRoom.AsignarTipoFaceBusqueda(Util.Pointsdownwards);

                barraRoom.statusbarra = barraRoom.Load_fx(null, null, false);
                if (barraRoom.statusbarra != Result.Succeeded) return false;

                ComprobarListaPtoPoligono comprobarListaPtoPoligono = new ComprobarListaPtoPoligono(barraRoom);
                if (!comprobarListaPtoPoligono.ComprobarPoligono()) return false;

                //2)
                GenerarGeometriaSimple _GenerarGeometriaSimple = new GenerarGeometriaSimple(_uiapp, barraRoom);
                if (!_GenerarGeometriaSimple.Ejecutar(1, UbicacionPtoMouse.centro)) return false;

                ObtenerGEometriaDTO _ObtenerGEometriaDTO = new ObtenerGEometriaDTO()
                {
                    ListaPtosPerimetroBarras = _GenerarGeometriaSimple.ListaPtosPerimetroBarras,
                    barraMenos = 0
                };
                RebarInferiorDTO rebarInferiorDTO1 = _GenerarGeometriaSimple.ObtenerGEometria(_ObtenerGEometriaDTO);
                if (rebarInferiorDTO1.IsOK == false) return false;

                GenerarBarra_Contras(rebarInferiorDTO1);

                if (_ListIRebarLosa.Count == 0) return false;

                GenerarBarras_ConTras_SegundaPArte();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
            }
            return true;
        }

      

        public bool BarraInferiores_MUltiples(TipoBarra tipobarra, UbicacionLosa ubicacion, UbicacionPtoMouse _ubicacionPtoMouse, bool Iscaso_intervalo)
        {                    
            try
            {
                //obtiene geometria de los ptos
                //1)coordendas
                BarraRoom barraRoom = new BarraRoom(_uiapp, tipobarra.ToString(), ubicacion);

                //para losa inclinada
                if (tipobarra == TipoBarra.f4_incli || tipobarra == TipoBarra.f3_incli || tipobarra == TipoBarra.f1_incli)
                    barraRoom.AsignarTipoFaceBusqueda(Util.Pointsdownwards);

                barraRoom.statusbarra = barraRoom.Load_fx(null, null, false);
                if (barraRoom.statusbarra != Result.Succeeded) return false;

                ComprobarListaPtoPoligono comprobarListaPtoPoligono = new ComprobarListaPtoPoligono(barraRoom);
                if (!comprobarListaPtoPoligono.ComprobarPoligono()) return false;

                ParametrosListaIntervalosDTo _ParametrosListaIntervalosDTo = new ParametrosListaIntervalosDTo()
                {
                    uiapp = _uiapp,
                    barraRoom = barraRoom,
                    _ubicacionPtoMouse = _ubicacionPtoMouse,
                    _tipoPosicionMouse = TipoPosicionMouse.segunMouse,
                    _Iscaso_intervalo = Iscaso_intervalo,
                    _diametroMM = barraRoom.diametroEnMM,
                    _espaciamiento = barraRoom._refereciaRoomDatos.Espaciamiento,
                    PtoSeleccionMouse1 = barraRoom._refereciaRoomDatos.PtoSeleccionMouse1,
                     ListaPtosPerimetroBarras = barraRoom.ListaPtosPerimetroBarras,
                     TipoBarra = barraRoom.TipoBarraStr,
                    ubicacionEnlosa = barraRoom.ubicacionEnlosa
                };

                //NOTA FECHA:27-09-2021  CDV INDICO QUE LAS BARRAS MIENTRAS ES LA BARRA ESTE DENTRO DE LA LOSA TODO BIEN
                //SOLO PARA PENDIENTES MAS ALTAS ( 45°) SE DEBE HACER  TRASLAPO EN EL CAMBIO DE PENDIENTE

                GenerarListaIntervalos _GenerarListaIntervalos = new GenerarListaIntervalos(_ParametrosListaIntervalosDTo);
                _GenerarListaIntervalos.M1_ObtenerIntervalos();

                //foreach
                foreach (GenerarListaIntervalosDTo item in _GenerarListaIntervalos.ListaIntervalosDTO)
                {
                    barraRoom.ListaPtosPerimetroBarras = item.ListaIntervalos;
                    barraRoom._seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = item.ptoMouse;
                    barraRoom.TipoBarraStr = item._tipoBarra;
                    barraRoom.ubicacionEnlosa = item._ubicacionEnlosa;

                    // barraRoom.
                    //2)
                    GenerarGeometriaSimple _GenerarGeometriaSimple = new GenerarGeometriaSimple(_uiapp, barraRoom);
                    if (!_GenerarGeometriaSimple.Ejecutar(1, _ubicacionPtoMouse)) return false;

                    ObtenerGEometriaDTO _ObtenerGEometriaDTO = new ObtenerGEometriaDTO()
                    {
                        ListaPtosPerimetroBarras = _GenerarGeometriaSimple.ListaPtosPerimetroBarras,
                        ListaPtosPerimetroBarrasParaDimensiones = _GenerarListaIntervalos.ListaPtosPerimetroBarrasParaDimension,
                        barraMenos = 0,
                        usarPoligonoOriginal=true
                        
                    };
                    RebarInferiorDTO rebarInferiorDTO1 = _GenerarGeometriaSimple.ObtenerGEometria(_ObtenerGEometriaDTO);
                    if (rebarInferiorDTO1.IsOK == false) return false;
                  //  rebarInferiorDTO1.espaciamientoFoot = rebarInferiorDTO1.espaciamientoFoot * 2;
                    GenerarBarra_Contras(rebarInferiorDTO1);
                }
                if (_ListIRebarLosa.Count == 0) return false;

                GenerarBarras_ConTras_SegundaPArte();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                //  Util.ErrorMsg($"Error al crear Rebar: { ex.Message}");
            }
  
            return true;
        }

        public bool BarraInferiores_ConAhorrro(TipoBarra tipobarra, UbicacionLosa ubicacion)
        {
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
        
            try
            {
                //obtiene geometria de los ptos
                //1)coordendas
                BarraRoom barraRoom = new BarraRoom(_uiapp, tipobarra.ToString(), ubicacion);

                //para losa inclinada
                if (tipobarra == TipoBarra.f4_incli || tipobarra == TipoBarra.f3_incli || tipobarra == TipoBarra.f1_incli)
                    barraRoom.AsignarTipoFaceBusqueda(Util.Pointsdownwards);

                barraRoom.statusbarra = barraRoom.Load_fx(null, null, false);
                if (barraRoom.statusbarra != Result.Succeeded) return false;

                ComprobarListaPtoPoligono comprobarListaPtoPoligono = new ComprobarListaPtoPoligono(barraRoom);
                if (!comprobarListaPtoPoligono.ComprobarPoligono()) return false;

                GenerarGeometriaAhorro _GenerarGeometriaAhorro = new GenerarGeometriaAhorro(_uiapp, barraRoom);
                if (!_GenerarGeometriaAhorro.Ejecutar()) return false;

                //2)
                RebarInferiorDTO rebarInferiorDTO1 = _GenerarGeometriaAhorro.ObtenerGEometria1();
                if (rebarInferiorDTO1.IsOK == false) return false;

                RebarInferiorDTO rebarInferiorDTO2 = _GenerarGeometriaAhorro.ObtenerGEometria2();
                if (rebarInferiorDTO2.IsOK == false) return false;

                using (TransactionGroup t = new TransactionGroup(_uiapp.ActiveUIDocument.Document))
                {
                    t.Start("CrearBarraInclinada-NH");

                    GenerarBarra_Sintrasn(rebarInferiorDTO1);
                    GenerarBarra_Sintrasn(rebarInferiorDTO2);
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                //  Util.ErrorMsg($"Error al crear Rebar: { ex.Message}");
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);

            return true;
        }


    
        //private bool GenerarBarra_Contrasm(RebarInferiorDTO rebarInferiorDTO1)
        //{
        //    try
        //    {

        //        //3)tag
        //        IGeometriaTag _newIGeometriaTag = FactoryGeomTag.CrearGeometriaTag_casoRebar(_uiapp, rebarInferiorDTO1);
        //        if (_newIGeometriaTag == null) return false;

        //        //4)barra
        //        IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
        //        if (!rebarLosa.M1A_IsTodoOK()) return false;

        //        _ListIRebarLosa.Add(rebarLosa);

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.DebugDescripcion(ex);
        //        return false;
        //    }
        //    return true;
        //}

        //private bool GenerarBarra_Sintrasn(RebarInferiorDTO rebarInferiorDTO1)
        //{
        //    //3)tag
        //    IGeometriaTag _newIGeometriaTag = FactoryGeomTag.CrearGeometriaTag_casoRebar(_uiapp, rebarInferiorDTO1);
        //    if (_newIGeometriaTag == null) return false;

        //    //4)barra
        //    IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
        //    if (!rebarLosa.M1A_IsTodoOK()) return false;

        //    rebarLosa.M2A_GenerarBarra();

        //    return true;
        //}
    }
}
