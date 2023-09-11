using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.model
{
    public class AdministradorIndividual
    {
        private Element _IndependentTagPathPrincial;
        private Element _IndependentTagSinTexto;
        private UIApplication _uiapp;
        private  GenerarNuevaDirectizDTO _generarNuevaDirectizDTO;
        private Document _doc;
        private View _view;
        private int _escala;

        public int diametroInt { get; set; }
        public double largoFoot { get; set; }
        public double AlturaZ_inferior { get; set; }
        public BarraIng _barraIng { get; set; }

        private double desfaseCodo;

        public string NombreFamiliaPrincipal { get; set; }

        private string CantidadBarras;


        public XYZ _direccionEnfierrado { get; set; }
        public Orientacion _orientacion { get; private set; }
        public XYZ _ptoInsertar { get; set; }
        public double distanciaMasLejosDeBorde { get; set; }

        private ConfiguracionTAgBarraDTo confBarraTag;

        public AdministradorIndividual(UIApplication uiapp, BarraIng list, GenerarNuevaDirectizDTO _generarNuevaDirectizDTO_inicial)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escala = 50;// _doc.ActiveView.Scale;
            this.diametroInt = list.diametroInt;
            this.largoFoot = list.largoFoot;
            this.AlturaZ_inferior = list.P2.Z;
            this._barraIng = list;
            this._generarNuevaDirectizDTO = _generarNuevaDirectizDTO_inicial;
            this.desfaseCodo = ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT;
            this.confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                IsDIrectriz = true,
                LeaderElbow = new XYZ(0, 0, desfaseCodo),
                tagOrientation = TagOrientation.Vertical,
                BarraTipo = TipoRebar.ELEV_BA_V
            };
        }


        public bool M1_ObtenerDatosPreDibujo()
        {
            try
            {
                _ptoInsertar = _generarNuevaDirectizDTO.ptoInserciontag;

                _direccionEnfierrado = _generarNuevaDirectizDTO.DireccionMoverTag;

                _orientacion = _generarNuevaDirectizDTO.OrientacionSeleccion;

                M1_1_ObtenerCantidadBarraGrupo();

                if (!M1_2_ObtenerFamiliasTAg(_generarNuevaDirectizDTO.OrientacionSeleccion)) return false;

                M1_3_ObtenerPuntoInsercionTag();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M2_GenerarNuevaDirectriz()
        {
            try
            {
                M2_4_GenerartagPrimeraBarra_contexto(_ptoInsertar);

                M2_6_BorrarTagAntiguo();
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
            CantidadBarras = _barraIng.cantidadBarra.ToString();
        }

        private bool M1_2_ObtenerFamiliasTAg(Orientacion direccionMOuse)
        {
            try
            {

                NombreFamiliaPrincipal = "";
                string NombreFamiliaSecunadrio = "MRA Rebar_SIN_" + _escala;
                if (direccionMOuse == Orientacion.izquierda)
                    NombreFamiliaPrincipal = _barraIng.ObtenerNOmbreFamiliaTagPOrFormaV();
                else
                    NombreFamiliaPrincipal = _barraIng.ObtenerNOmbreFamiliaTagPOrFormaV();

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



        private void M1_3_ObtenerPuntoInsercionTag()
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



        private void M2_4_GenerartagPrimeraBarra_contexto(XYZ ptoInsertar)
        {

            TagBarra tag0 = ObtenerTAgBarra(ptoInsertar, _IndependentTagPathPrincial);
            if (tag0.IsOk)
                tag0.DibujarTagRebarV(_barraIng._rebar, _uiapp, _view, confBarraTag);

            if (CantidadBarras != "" && ParameterUtil.FindParaByName(_barraIng._rebar, "CantidadBarra") != null)
                ParameterUtil.SetParaInt(_barraIng._rebar, "CantidadBarra", CantidadBarras);  //"(2+2+2+2)"

        }

        private void M2_6_BorrarTagAntiguo()
        {
            //Borrar Tag
            _doc.Delete(_barraIng._IndependentTag_soloParaBorrarTag.Id);
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
