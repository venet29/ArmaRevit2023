using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.EditarPath;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag
{
    public class ManejarEditTagUpdate_MoverConfiguracionInicial
    {

        protected Document _doc;
        private CoordenadaPath _coordenadaPath;
        private string _tipobarra;
        private UbicacionLosa _direccion;
        private double Angle_pelotaLosa1Grado;
        protected readonly UIApplication _uiapp;
        private readonly PathReinforcement _pathReinforcement;
        private XYZ _ptoPathSymbol;
        protected IGeometriaTag _listaTAgBArra;
        protected List<IndependentTag> _listaIndependentTag;
#pragma warning disable CS0169 // The field 'ManejarEditTagUpdate_MoverConfiguracionInicial.VerificarSiTagTieneMismaEscala_QueEscalaCOnfiguracion' is never used
        private bool VerificarSiTagTieneMismaEscala_QueEscalaCOnfiguracion;
#pragma warning restore CS0169 // The field 'ManejarEditTagUpdate_MoverConfiguracionInicial.VerificarSiTagTieneMismaEscala_QueEscalaCOnfiguracion' is never used

        public bool IsOK { get; set; }


        public ManejarEditTagUpdate_MoverConfiguracionInicial(UIApplication uiapp, PathReinforcement pathReinforcement, XYZ ptoPathSymbol, List<IndependentTag> listaIndependentTag)
        {

          
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._pathReinforcement = pathReinforcement;
            this._ptoPathSymbol = ptoPathSymbol;
            this._listaIndependentTag = listaIndependentTag;
            this.IsOK = true;
        }


        public bool Ejecutar_SinTrans()
        {
            try
            {

                if (!M1_ObtenerTipos()) return false;
               // M2_SeleccionarRoom();
                if (!M3_ObtenerBordeDepath()) return false;
                if (!M4_ObtenerTag()) return false;
                M5_Mover();

            }
            catch (Exception ex)
            {

                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }
            return IsOK;
        }
        public bool Ejecutar_Contrans()
        {
            try
            {

                if (!M1_ObtenerTipos()) return false;
                // M2_SeleccionarRoom();
                if (!M3_ObtenerBordeDepath()) return false;
                if (!M4_ObtenerTag()) return false;
          
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ResetTAg");
                    M5_Mover();
                    t.Commit();
                }
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

        public bool M4_ObtenerTag()
        {
            try
            {

                SolicitudBarraDTO _solicitudBarraDTO = new SolicitudBarraDTO(new UIDocument(_doc), _tipobarra, _direccion, TipoConfiguracionBarra.refuerzoInferior);

                _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, _ptoPathSymbol, _solicitudBarraDTO, _coordenadaPath.GetListaXYZ());
                _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(Angle_pelotaLosa1Grado) });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }

            return IsOK;
        }

        private void M2_SeleccionarRoom()
        {
            UIDocument _uidoc = new UIDocument(_doc);
            Room RoomSelecionado = RoomSeleccionar.GetRoomConPtoNivelActual(_uidoc, _ptoPathSymbol);

            ReferenciaRoomDatos _referenciaRoomDatos = new ReferenciaRoomDatos(_doc,RoomSelecionado);
            _referenciaRoomDatos.GetParametrosUnRoom();
            Angle_pelotaLosa1Grado = _referenciaRoomDatos.anguloBarraLosaGrado_1;

        }

        private void M5_Mover()
        {
            try
            {
                //filtrar
                var _listaIndependentTagAnoy = _listaIndependentTag.Select(c => new TagPathReinProcesado() { tagPath = c, Isprocesado = false }).ToList();


                for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                {
                    var item = _listaIndependentTagAnoy[i];

                    if (item.Isprocesado == true) continue;

                    var resul = _listaTAgBArra.listaTag.Where(c => c.nombreFamilia == item.tagPath.Name && c.IsOk == true).ToList();

                    if (resul.Count == 1)
                    {
                        if (!ContieneTagCOrrecto(_listaIndependentTag[i].Name))
                            ElementTransformUtils.MoveElement(_doc, _listaIndependentTag[i].Id, XYZ.Zero);
                        else
                            ElementTransformUtils.MoveElement(_doc, _listaIndependentTag[i].Id, resul[0].posicion - _listaIndependentTag[i].TagHeadPosition);
                        item.Isprocesado = true;
                    }
                    else if (resul.Count == 2)
                    {
                        AnalisisCasoConAhorro(_listaIndependentTagAnoy, _listaIndependentTag[i], item, resul);

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
            }
        }

        private void AnalisisCasoConAhorro(List<TagPathReinProcesado> _listaIndependentTagAnoy,IndependentTag independentTag, TagPathReinProcesado item, List<TagBarra> resul)
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
        private bool ContieneTagCOrrecto(string name)
        {
            return true;
#pragma warning disable CS0162 // Unreachable code detected
            if (name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_F")) return false;
#pragma warning restore CS0162 // Unreachable code detected
            if (name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_L")) return false;
            if (name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_C")) return false;

            return true;
        }


    }

}
