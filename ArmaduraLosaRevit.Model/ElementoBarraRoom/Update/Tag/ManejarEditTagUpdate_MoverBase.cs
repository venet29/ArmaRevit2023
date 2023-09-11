using ArmaduraLosaRevit.Model.EditarPath;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Calculo;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ExtStore;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag
{
    public class ManejarEditTagUpdate_MoverBase
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected CoordenadaPath _coordenadaPath;
        protected string _tipobarra;
        protected UbicacionLosa _direccion;
        protected double Angle_pelotaLosa1Grado;
        protected readonly PathReinforcement _pathReinforcement;
        protected XYZ _ptoPathSymbol;
        protected DatosExtStoreDTO _CreadorExtStoreDTO;

        protected List<IndependentTag> _listaIndependentTag_Enview;
        protected CreadorExtStore _CreadorExtStore;

        protected List<string> ListaCasosNoconsiderar;
        public bool IsOK { get; set; }


        public ManejarEditTagUpdate_MoverBase(UIApplication uiapp, PathReinforcement pathReinforcement, XYZ ptoPathSymbol, List<IndependentTag> listaIndependentTag)
        {

            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._pathReinforcement = pathReinforcement;
            this._ptoPathSymbol = ptoPathSymbol;
            this._listaIndependentTag_Enview = listaIndependentTag;
            this.IsOK = true;
            this.ListaCasosNoconsiderar = new List<string>();
        }

        //desactivado
        public bool Ejecutar_Inicial()
        {
            try
            {

                if (!M1_ObtenerTipos()) return false;
                // M2_SeleccionarRoom();
                if (!M3_ObtenerBordeDepath()) return false;
                //if (!M4_ObtenerTag()) return false;
                M5_Mover();

            }
            catch (Exception ex)
            {

                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }
            return IsOK;
        }


        public bool M1_ObtenerTipos()
        {
            try
            {
                string _direccion_aux = "";
                Parameter aux_tipobarra = ParameterUtil.FindParaByName(_pathReinforcement, "IDTipo");
                if (aux_tipobarra != null)
                    _tipobarra = aux_tipobarra.AsString();

                Parameter aux_direccion = ParameterUtil.FindParaByName(_pathReinforcement, "IDTipoDireccion");
                if (aux_direccion != null)
                { _direccion_aux = aux_direccion.AsString(); }

                _direccion = EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, _direccion_aux);

                var cantidadBarras=_pathReinforcement.ObtenerNumeroBarras(_doc);

            }
            catch (Exception ex)
            {

                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }

            return IsOK;
        }

        public bool M3_ObtenerBordeDepath()
        {
            try
            {
                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_pathReinforcement, _doc);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();
                pathReinformeCalculos.ObtenerAngulo_P2_P3();
                Angle_pelotaLosa1Grado = pathReinformeCalculos.AngleP2_p3;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }
            return IsOK = _coordenadaPath.IsPtoOK;
        }


        private void M5_Mover()
        {
            try
            {
                //filtrar
                var _listaIndependentTagAnoy = _listaIndependentTag_Enview.Select(c => new TagPathReinProcesado() { tagPath = c, Isprocesado = false }).ToList();

                /*
                if (UtilVersionesRevit.IsMAyorOigual(_uiapp, VersionREvitNh.v2021))
                {
                    _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
                    _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);


                    for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                    {
                        var item = _listaIndependentTagAnoy[i];
                        if (item.Isprocesado == true) continue;

                        //get
                        if (_CreadorExtStore.GET_DataInElement_XYZ_SinTrans(item.tagPath, _CreadorExtStoreDTO.SchemaName))
                        {
                            if (_CreadorExtStore.retrievedData.IsAlmostEqualTo(XYZ.Zero)) continue;

                            XYZ posicionAnteriorGuardada = _CreadorExtStore.retrievedData;

                            XYZ DesltaDesplaiento = ObtenerDesplazaminetoEnSentidoBarras.Ejecutar(posicionAnteriorGuardada, item.tagPath, _coordenadaPath);
                            ElementTransformUtils.MoveElement(_doc, item.tagPath.Id, DesltaDesplaiento.AsignarZ(0));
                        }
                        //set
                        _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(item.tagPath, item.tagPath.TagHeadPosition);

                    }
                }
                else
                {
                    var _CreadorExtStoreDTOoOld = FactoryExtStore.ObtnerPosicionTagLosa();
                    var _CreadorExtStoreold = new CreadorExtStore_2020Abajo(_uiapp, _CreadorExtStoreDTOoOld);


                    for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                    {
                        var item = _listaIndependentTagAnoy[i];
                        if (item.Isprocesado == true) continue;

                        //get
                        if (_CreadorExtStoreold.GET_DataInElement_XYZ_SinTrans(item.tagPath, _CreadorExtStoreDTOoOld.SchemaName))
                        {
                            if (_CreadorExtStoreold.retrievedData.IsAlmostEqualTo(XYZ.Zero)) continue;

                            XYZ posicionAnteriorGuardada = _CreadorExtStoreold.retrievedData;

                            XYZ DesltaDesplaiento = ObtenerDesplazaminetoEnSentidoBarras.Ejecutar(posicionAnteriorGuardada, item.tagPath, _coordenadaPath);
                            ElementTransformUtils.MoveElement(_doc, item.tagPath.Id, DesltaDesplaiento.AsignarZ(0));
                        }
                        //set
                        _CreadorExtStoreold.SET_DataInElement_XYZ_SInTrans(item.tagPath, item.tagPath.TagHeadPosition);

                    }
                }
                */

                _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
                _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);


                for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                {
                    var item = _listaIndependentTagAnoy[i];
                    if (item.Isprocesado == true) continue;

                    //get
                    if (_CreadorExtStore.GET_DataInElement_XYZ_SinTrans(item.tagPath, _CreadorExtStoreDTO.SchemaName))
                    {
                        if (_CreadorExtStore.retrievedData.IsAlmostEqualTo(XYZ.Zero)) continue;

                        XYZ posicionAnteriorGuardada = _CreadorExtStore.retrievedData;

                        XYZ DesltaDesplaiento = ObtenerDesplazaminetoEnSentidoBarras.Ejecutar(posicionAnteriorGuardada, item.tagPath, _coordenadaPath);
                        ElementTransformUtils.MoveElement(_doc, item.tagPath.Id, DesltaDesplaiento.AsignarZ(0));
                    }
                    //set
                    _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(item.tagPath, item.tagPath.TagHeadPosition);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
            }
        }

        private void AnalisisCasoConAhorro(List<TagPathReinProcesado> _listaIndependentTagAnoy, IndependentTag independentTag, TagPathReinProcesado item, List<TagBarra> resul)
        {
            if (!ContieneTagCOrrecto(independentTag.Name))
            {
                ElementTransformUtils.MoveElement(_doc, independentTag.Id, XYZ.Zero);
                item.Isprocesado = true;
            }
            else
            {
                var ListaTag = _listaIndependentTagAnoy.Where(c => c.tagPath.Name == item.tagPath.Name).ToList();

                if (Util.IntersectionXYZ(ListaTag[0].tagPath.TagHeadPosition.GetXY0(), resul[0].posicion.GetXY0(),
                                         ListaTag[1].tagPath.TagHeadPosition.GetXY0(), resul[1].posicion.GetXY0()).IsAlmostEqualTo(XYZ.Zero))
                {
                    ElementTransformUtils.MoveElement(_doc, ListaTag[0].tagPath.Id, resul[0].posicion - ListaTag[0].tagPath.TagHeadPosition);
                    ElementTransformUtils.MoveElement(_doc, ListaTag[1].tagPath.Id, resul[1].posicion - ListaTag[1].tagPath.TagHeadPosition);
                }//no se cruzan
                else
                {
                    ElementTransformUtils.MoveElement(_doc, ListaTag[0].tagPath.Id, resul[0].posicion - ListaTag[1].tagPath.TagHeadPosition);
                    ElementTransformUtils.MoveElement(_doc, ListaTag[1].tagPath.Id, resul[1].posicion - ListaTag[0].tagPath.TagHeadPosition);
                }//secruzan
                ListaTag[0].Isprocesado = true;
                ListaTag[1].Isprocesado = true;
            }
        }


        // si es true vuelve a la configuracion
        protected bool ContieneTagCOrrecto(string name)
        {
            try
            {


                var resul = ListaCasosNoconsiderar.Where(c => name.Contains(c)).ToList();
                if (resul.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ContieneTagCOrrecto'  con tag '{name}' \nex:{ex.Message}");
                return false;
            }
        }


    }

}
