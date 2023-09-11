using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.model
{
    public class AdministradorGrupos
    {
        private Element _IndependentTagPathPrincial;
        private Element _IndependentTagSinTexto;
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private int _escala;

        public int diametroInt { get; set; }
        public double largoFoot { get; set; }
        public double AlturaZ_inferior { get; set; }
        public List<BarraIng> ListaBarraIng { get; set; }

        private double desfaseCodo;

        public string NombreFamiliaPrincipal { get; set; }

        private string CantidadBarras;


        public XYZ _direccionEnfierrado { get; set; }
        public Orientacion _orientacion { get; private set; }
        public XYZ _ptoInsertar { get; set; }
        public double distanciaMasLejosDeBorde { get; set; }

        private ConfiguracionTAgBarraDTo confBarraTag_primeraBarra;
        private ConfiguracionTAgBarraDTo confBarraTag_BarrasSintext;

        public AdministradorGrupos(UIApplication uiapp, int diametroInt, double largoFoot, double AlturaZ_inferior, List<BarraIng> list, ConfiguracionTAgBarraDTo _ConfiguracionTAgBarraDTo)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escala = 50;// _doc.ActiveView.Scale;
            this.diametroInt = diametroInt;
            this.largoFoot = largoFoot;
            this.AlturaZ_inferior = AlturaZ_inferior;
            this.ListaBarraIng = list;
            this.desfaseCodo = ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT;
            this.confBarraTag_primeraBarra = _ConfiguracionTAgBarraDTo;
        }


        public bool M1_ObtenerDatosPreDibujo(GenerarNuevaDirectizDTO _generarNuevaDirectizDTO)
        {
            try
            {
                _ptoInsertar = _generarNuevaDirectizDTO.ptoInserciontag;

                _direccionEnfierrado = _generarNuevaDirectizDTO.DireccionMoverTag;

                _orientacion = _generarNuevaDirectizDTO.OrientacionSeleccion;

                M1_1_ObtenerCantidadBarraGrupo();

                if (!M1_2_ObtenerFamiliasTAg(_generarNuevaDirectizDTO.OrientacionSeleccion)) return false;

                // M3_ObtenerPuntoInsercionTagVertical();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M1_ObtenerDatosPreDibujoAuto(GenerarNuevaDirectizDTO _generarNuevaDirectizDTO)
        {
            try
            {
                _ptoInsertar = _generarNuevaDirectizDTO.ptoInserciontag;

                _direccionEnfierrado = _generarNuevaDirectizDTO.DireccionMoverTag;

                _orientacion = _generarNuevaDirectizDTO.OrientacionSeleccion;
                M1_1_ObtenerCantidadBarraGrupo();

                if (!M1_2_ObtenerFamiliasTAg(_generarNuevaDirectizDTO.OrientacionSeleccion)) return false;

                //M1_3_ObtenerPuntoInsercionTag();
                //pto inicial 
                if (ListaBarraIng[0].IsNoProloganLosaArriba)
                {
                    Debug.WriteLine($" pier: {ListaBarraIng[0].Pier}   ,  p1: {ListaBarraIng[0].P1}    ,    orienta:  {ListaBarraIng[0].orientacion}");
                }

                distanciaMasLejosDeBorde = ListaBarraIng.Max(x => x.distanciaRespectoBorde);
                //_ptoInsertar = _ptoInsertar.AsignarZ(ListaBarraIng[0].ObtenerValorZTAg() - Util.CmToFoot(50)-1.5);
                // - (ListaBarraIng[0].IsNoProloganLosaArriba ? 0 : UtilBarras.largo_traslapoFoot_diamMM(ListaBarraIng[0].diametroInt)) - 1.5);

                // _ptoInsertar = _ptoInsertar.AsignarZ(ListaBarraIng[0].P1.Z - Util.CmToFoot(50) -UtilBarras.largo_traslapoFoot_diamMM(ListaBarraIng[0].diametroInt) - 1.5);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M1_1_ObtenerCantidadBarraGrupo()
        {
            CantidadBarras = "";
            foreach (var item in ListaBarraIng)
            {
                if (CantidadBarras == "")
                    CantidadBarras = item.cantidadBarra.ToString();
                else
                    CantidadBarras = CantidadBarras + "+" + item.cantidadBarra.ToString();
            }
        }

        private bool M1_2_ObtenerFamiliasTAg(Orientacion direccionMOuse)
        {
            try
            {

                NombreFamiliaPrincipal = "";
                string NombreFamiliaSecunadrio = "MRA Rebar_SIN_" + _escala;
                if (direccionMOuse == Orientacion.izquierda)
                    NombreFamiliaPrincipal = ListaBarraIng[0].ObtenerNOmbreFamiliaTagPOrFormaV();
                else
                    NombreFamiliaPrincipal = ListaBarraIng.Last().ObtenerNOmbreFamiliaTagPOrFormaV();

                //caso sin giraR
                if (_IndependentTagPathPrincial == null)
                    _IndependentTagPathPrincial = TiposRebarTag.M1_GetRebarTag(NombreFamiliaPrincipal, _doc);

                if (_IndependentTagSinTexto == null)
                    _IndependentTagSinTexto = TiposRebarTag.M1_GetRebarTag(NombreFamiliaSecunadrio, _doc);

                if (_IndependentTagPathPrincial == null && _IndependentTagSinTexto == null)
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message}");
                return false;
            }
            return true;
        }

        public bool M2_GenerarNuevaDirectrizVertical()
        {
            try
            {
                M2_4_GenerartagPrimeraBarra_contexto(_ptoInsertar);


                M2_5_GEnerarTagOtroBarrasVertical_sintexto();

                M2_6_BorrarTagAntiguo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M2_4_GenerartagPrimeraBarra_contexto(XYZ ptoInsertar)
        {

            TagBarra tag0 = ObtenerTAgBarra(ptoInsertar, _IndependentTagPathPrincial);
            if (tag0.IsOk)
                tag0.DibujarTagRebarV(ListaBarraIng[0]._rebar, _uiapp, _view, confBarraTag_primeraBarra);

            if (CantidadBarras != "" && ParameterUtil.FindParaByName(ListaBarraIng[0]._rebar, "CantidadBarra") != null)
                ParameterUtil.SetParaInt(ListaBarraIng[0]._rebar, "CantidadBarra", CantidadBarras);  //"(2+2+2+2)"

        }

        private void M2_5_GEnerarTagOtroBarrasVertical_sintexto()
        {
            int cantidadBarras = ListaBarraIng.Count();
            XYZ despla = XYZ.Zero;

            if (_view.Scale == 50)
            {
                double largoBase = 0;// Util.CmToFoot(40);

                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
            }
            else if (_view.Scale == 75)
            {

                double largoBase = 0;// Util.CmToFoot(15);
                despla = new XYZ(0, 0, largoBase);
                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(22));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(22));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(24));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(24));

            }
            else if (_view.Scale == 100)
            {

                double largoBase = 0;// Util.CmToFoot(15);
                despla = new XYZ(0, 0, largoBase);
                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(28));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(30));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(32));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(32));

            }


            XYZ _ptoInsertar_sintexto = _ptoInsertar - despla;

            //subir ecodo
            confBarraTag_BarrasSintext = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                IsDIrectriz = true,
                LeaderElbow = new XYZ(0, 0, desfaseCodo) + despla,
                tagOrientation = TagOrientation.Vertical,
                BarraTipo = TipoRebar.ELEV_BA_V
            };

            for (int i = 1; i < ListaBarraIng.Count; i++)
            {
                if (_IndependentTagSinTexto == null) continue;
                TagBarra tag0 = ObtenerTAgBarra(_ptoInsertar_sintexto, _IndependentTagSinTexto);
                if (!tag0.IsOk) continue;
                tag0.DibujarTagRebarV(ListaBarraIng[i]._rebar, _uiapp, _view, confBarraTag_BarrasSintext);

            }

        }

        private void M2_5_GEnerarTagOtroBarrasHortogonal_sintexto()
        {
            int cantidadBarras = ListaBarraIng.Count();
            XYZ despla = XYZ.Zero;

            if (_view.Scale == 50)
            {
                double largoBase = 0;// Util.CmToFoot(40);

                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(16));
            }
            else if (_view.Scale == 75)
            {

                double largoBase = 0;// Util.CmToFoot(15);
                despla = new XYZ(0, 0, largoBase);
                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(22));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(22));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(24));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(24));

            }
            else if (_view.Scale == 100)
            {

                double largoBase = 0;// Util.CmToFoot(15);
                despla = new XYZ(0, 0, largoBase);
                if (cantidadBarras < 6)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(28));
                else if (cantidadBarras < 11)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(30));
                else if (cantidadBarras < 16)
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(32));
                else
                    despla = new XYZ(0, 0, largoBase + (cantidadBarras - 1) * Util.CmToFoot(32));

            }

            var deltaDesplaza = (ListaBarraIng.Count <= 2 ? ListaBarraIng.Count : (Util.CmToFoot(30 + ListaBarraIng.Count * 15)));
            XYZ _ptoInsertar_sintexto = _ptoInsertar + _view.RightDirection * deltaDesplaza;

            //barras sin texto
            confBarraTag_BarrasSintext = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                IsDIrectriz = true,
                LeaderElbow = -_view.RightDirection * deltaDesplaza,// new XYZ(0, 0, desfaseCodo) + despla,
                tagOrientation = TagOrientation.Horizontal,
                BarraTipo = TipoRebar.ELEV_BA_V
            };

            for (int i = 1; i < ListaBarraIng.Count; i++)
            {
                if (_IndependentTagSinTexto == null) continue;
                TagBarra tag0 = ObtenerTAgBarra(_ptoInsertar_sintexto, _IndependentTagSinTexto);
                if (!tag0.IsOk) continue;
                tag0.DibujarTagRebarV(ListaBarraIng[i]._rebar, _uiapp, _view, confBarraTag_BarrasSintext);

            }

        }

        private void M2_6_BorrarTagAntiguo()
        {
            //    var listaTagValidos = ListaBarraIng.SelectMany(c => _tiposRebarTagsEnView.M1_BuscarEnColecctorPorRebar(c._rebar.Id)).Select(c=> c.Id).ToList() ;
            //Borrar Tag
            var listaTagValidos = ListaBarraIng.Where(r => r._IndependentTag_soloParaBorrarTag != null).SelectMany(c => c.ListaTodosTagRebar.Select(f => f.Id)).ToList();
            if (listaTagValidos.Count > 0)
                _doc.Delete(listaTagValidos);
        }

        public bool M3_GenerarNuevaDirectrizHorizontal()
        {
            try
            {
                var deltaDesplaza = (ListaBarraIng.Count <= 2 ? ListaBarraIng.Count : (Util.CmToFoot(30 + ListaBarraIng.Count * 15)));
                // XYZ aux_elbao = confBarraTag_primeraBarra.LeaderElbow;
                confBarraTag_primeraBarra.LeaderElbow = -(_view.RightDirection * deltaDesplaza);
                M2_4_GenerartagPrimeraBarra_contexto(_ptoInsertar + _view.RightDirection * deltaDesplaza);

                M2_5_GEnerarTagOtroBarrasHortogonal_sintexto();
                M2_6_BorrarTagAntiguo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public void M4_ObtenerPuntoInsercionTagVertical()
        {
            //pto inicial 
            //_ptoInsertar = _ptoInsertar.AsignarZ(ListaBarraIng[0].P1.Z - Util.CmToFoot(50) - UtilBarras.largo_L9_DesarrolloFoot_diamMM(ListaBarraIng[0].diametroInt) - 1.5);

            if (AlmacenerNiveles.ListaNiveles.Exists(c => _ptoInsertar.Z - Util.CmToFoot(2.5) < c && c < _ptoInsertar.Z + Util.CmToFoot(2.5)))
            {
                _ptoInsertar = _ptoInsertar + new XYZ(0, 0, -Util.CmToFoot(20)) + _direccionEnfierrado * Util.CmToFoot(23);
                AlmacenerNiveles.ListaNiveles.Add(_ptoInsertar.Z);
            }
            else
            {
                AlmacenerNiveles.ListaNiveles.Add(_ptoInsertar.Z);
            }

            //primero

        }

        private TagBarra ObtenerTAgBarra(XYZ posicion, Element _IndependentTagPath)
        {
            string nombreLetra = "F";

            if (_IndependentTagPath == null)
            {
                Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreLetra}");
                return null;
            }

            TagBarra newTagBarra = new TagBarra(posicion, nombreLetra, _IndependentTagPath.Name, _IndependentTagPath);
            return newTagBarra;

        }
    }
}
