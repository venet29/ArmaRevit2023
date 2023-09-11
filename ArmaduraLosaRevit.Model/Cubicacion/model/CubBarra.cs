using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.AyudasView;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Cubicacion.model
{



    //crear familia base y dos objetos rebar y path
    public class CubBarra
    {
        private ElementoPathRebarInSystem elementoPathRebarInSystem;
#pragma warning disable CS0649 // Field 'CubBarra.elementoPathRein' is never assigned to, and will always have its default value null
        private ElementoPathRein elementoPathRein;
#pragma warning restore CS0649 // Field 'CubBarra.elementoPathRein' is never assigned to, and will always have its default value null
        private readonly List<View> listaView;
        private readonly ObtenerNivelPorOrigenBarra obtenerNivelPorOrigenBarra;
        private readonly List<LevelDTO> _lista_Level_Habilitados;
        private ElementoPathRebar _elementoRebar;
        public Caso_cub Caso_ { get; set; }

        public string Tipo_cub { get; set; }
        public string nivel { get; set; }
        public int N_Barras { get; set; }

        private Document _doc;



        public int Diammm { get; set; }
        public double LargoCm { get; set; }

        public double pesos_ { get; set; }


        public CasoRabar CasoRabar_ { get; set; }
        public string BarraId { get; set; }
        public double Id { get; set; }

        public CubBarra trabasSoloMAllaH { get; set; }
        public List<CubBarra> ListaBArrasReparticion { get; set; }
        public bool IstrabasSoloMAllaH { get; set; }
        public visibi IsOculto { get; set; }
        public string Eje { get; set; } //nombre elev , en losa:101

        public Orientacion_Cub Orientacion_ { get; set; } //nombre elev , en losa:101
        public Elemento_cub Elemento_ { get; set; } //nombre elev , en losa:101    
        public TipoRebar TipoBarra_ { get; set; }
        public TipoBarraGeneral TipoBarraGeneral { get; set; }
        public bool IsOK { get; private set; }
        public double OrdeElevacion { get; private set; }

        public CubBarra(ElementoPathRebarInSystem elementoPathRein, List<View> listaView, ObtenerNivelPorOrigenBarra _obtenerNivelPorOrigenBarra, List<LevelDTO> Lista_Level_habilitados)
        {
            this.elementoPathRebarInSystem = elementoPathRein;
            this.listaView = listaView;
            obtenerNivelPorOrigenBarra = _obtenerNivelPorOrigenBarra;
            _lista_Level_Habilitados = Lista_Level_habilitados;
            LargoCm = 0;
            N_Barras = 0;
            IstrabasSoloMAllaH = false;
            IsOculto = visibi.no_analizado;
            ListaBArrasReparticion = new List<CubBarra>();
        }

        public CubBarra(ElementoPathRebar item, List<View> listaView, ObtenerNivelPorOrigenBarra _obtenerNivelPorOrigenBarra, List<LevelDTO> Lista_Level_habilitados)
        {
            this._elementoRebar = item;
            this.listaView = listaView;
            obtenerNivelPorOrigenBarra = _obtenerNivelPorOrigenBarra;
            _lista_Level_Habilitados = Lista_Level_habilitados;
            LargoCm = 0;
            N_Barras = 0;
            IstrabasSoloMAllaH = false;
            IsOculto = visibi.no_analizado;
            ListaBArrasReparticion = new List<CubBarra>();
        }



        public bool Calcular_path()
        {
            try
            {
                IsOK = false;
                CasoRabar_ = CasoRabar.Path_;
                DatosPathRinforment _LargoPathRinforment = new DatosPathRinforment(elementoPathRein._pathReinforcement);
                _LargoPathRinforment.M1_ObtenerDatosGenerales();
                _LargoPathRinforment.M2_ObtenerCantiadad_REbarshape_cantidad_();

                _doc = elementoPathRein._pathReinforcement.Document;



                Diammm = elementoPathRein.DiametroBarra;
                //N_Barras = _elementoRebar._rebar.Quantity;

                Eje = AyudaObtenerParametros.ObtenerNombreView(elementoPathRein._pathReinforcement);
                Id = elementoPathRein._pathReinforcement.Id.IntegerValue;
                BarraId = AyudaObtenerParametros.ObtenerIdBarraCopiar(elementoPathRein._pathReinforcement);


                VerificarSiEstaOculto(elementoPathRein._pathReinforcement);

                if (_LargoPathRinforment.Isok)
                {
                    LargoCm = Math.Round(Util.FootToCm(_LargoPathRinforment.LargoPrimaria_Foot + _LargoPathRinforment.LargoAlternative_foot), 2);
                    N_Barras = _LargoPathRinforment.numeroBarrasPrimaria + _LargoPathRinforment.numeroBarrasSecundario;
                }

                //formula para revit =(Bar Diameter / 1.273 cm) ^ 2 * Bar Length / 100 cm * Quantity
                pesos_ = (Diammm / 12.73) * (Diammm / 12.73) * N_Barras * (LargoCm / 100.0f);

                ElementId hostId = elementoPathRein._pathReinforcement.GetHostId();
                if (hostId == null)
                {
                    IsOK = false;
                    return false;
                }
                Element hostElement = _doc.GetElement(hostId);

                //level
                Level nivel_ = (Level)_doc.GetElement(hostElement.LevelId);
                string nivelOriginal = nivel_.Name;
                nivel = nivel_.Name;
                if (NombreDefinidoUsuario.ObtenerNombreDefinidoUsuario(nivelOriginal, _lista_Level_Habilitados))
                    nivel = NombreDefinidoUsuario.nivelModificado;

                if (NombreDefinidoUsuario.ObtenerOrdenPorElevacion(nivelOriginal, _lista_Level_Habilitados))
                    OrdeElevacion = NombreDefinidoUsuario.OrdeElevacion;

                ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(elementoPathRein._pathReinforcement);
                _newObtenerTipoBarra.Ejecutar();

                TipoBarra_ = _newObtenerTipoBarra.TipoBarra_;
                TipoBarraGeneral = _newObtenerTipoBarra.TipoBarraGeneral;


                if (TipoBarra_.ToString().Contains("LOSA_SUP") || TipoBarra_ == TipoRebar.REFUERZO_SUPLE_CAB_MU)
                    ObtenerPataBarraLosaYBarraReparticion();

                ObtenerCaso();

                EntidadBarras _EntidadBarras = Tipos_Barras.M3_Buscar_EntidadBarras_porTipoRebar(TipoBarra_);
                if (_EntidadBarras.TipoParaCub == null)
                { }

                Tipo_cub = _EntidadBarras.TipoParaCub;
                Orientacion_ = _EntidadBarras.Orientacion_Cub_;
                Elemento_ = _EntidadBarras.Elemento_Cub;
                IsOK = true;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);

            }

            return IsOK;
        }
        public bool Calcular_RebarInSystem()
        {
            IsOK = false;
            try
            {
                _doc = elementoPathRebarInSystem._rebarInSystem.Document;
                Diammm = elementoPathRebarInSystem.DiametroBarra;
                Id = elementoPathRebarInSystem._rebarInSystem.Id.IntegerValue;

                CasoRabar_ = CasoRabar.Path_;

                N_Barras = elementoPathRebarInSystem._rebarInSystem.Quantity;
                LargoCm = Math.Round(Util.FootToCm(elementoPathRebarInSystem._rebarInSystem.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble()), 2);

                pesos_ = (Diammm / 12.73) * (Diammm / 12.73) * N_Barras * (LargoCm / 100.0f);

                Eje = AyudaObtenerParametros.ObtenerNombreView(elementoPathRebarInSystem._rebarInSystem);
                BarraId = AyudaObtenerParametros.ObtenerIdBarraCopiar(elementoPathRebarInSystem._rebarInSystem);

                VerificarSiEstaOculto(elementoPathRebarInSystem._rebarInSystem);
                //

                if (!ObtenerNivel(elementoPathRebarInSystem._rebarInSystem.GetHostId())) return false;


                ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(elementoPathRebarInSystem._rebarInSystem);
                _newObtenerTipoBarra.Ejecutar();

                TipoBarra_ = _newObtenerTipoBarra.TipoBarra_;
                TipoBarraGeneral = _newObtenerTipoBarra.TipoBarraGeneral;

                //if (TipoBarra_ == TipoRebar.ELEV_MA_H)
                //    ObtenerTrabasMAlla();
                if (TipoBarra_.ToString().Contains("LOSA_SUP") || TipoBarra_ == TipoRebar.REFUERZO_SUPLE_CAB_MU)
                    ObtenerPataBarraLosaYBarraReparticion();
                else if (TipoBarra_ == TipoRebar.FUND_BA_SUP)
                    ObtenerPataBarraFundacion();

                // caso
                ObtenerCaso();

                EntidadBarras _EntidadBarras = Tipos_Barras.M3_Buscar_EntidadBarras_porTipoRebar(TipoBarra_);
                if (_EntidadBarras.TipoParaCub == null)
                { }
                Tipo_cub = _EntidadBarras.TipoParaCub;
                Orientacion_ = _EntidadBarras.Orientacion_Cub_;
                Elemento_ = _EntidadBarras.Elemento_Cub;
                IsOK = true;
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);

            }


            return IsOK;
        }


        public bool Calcular_Rebar()
        {
            try
            {
                IsOK = false;
                _doc = _elementoRebar._rebar.Document;
                Diammm = _elementoRebar.DiametroBarra;
                Id = _elementoRebar._rebar.Id.IntegerValue;

                CasoRabar_ = CasoRabar.Rebar_;

                N_Barras = _elementoRebar._rebar.Quantity;
                LargoCm = Math.Round(Util.FootToCm(_elementoRebar._rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble()), 2);

                pesos_ = (Diammm / 12.73) * (Diammm / 12.73) * N_Barras * (LargoCm / 100.0f);

                Eje = AyudaObtenerParametros.ObtenerNombreView(_elementoRebar._rebar);
                BarraId = AyudaObtenerParametros.ObtenerIdBarraCopiar(_elementoRebar._rebar);

                VerificarSiEstaOculto(_elementoRebar._rebar);
                //

                if (!ObtenerNivel(_elementoRebar._rebar.GetHostId())) return false;


                ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(_elementoRebar._rebar);
                _newObtenerTipoBarra.Ejecutar();

                TipoBarra_ = _newObtenerTipoBarra.TipoBarra_;
                TipoBarraGeneral = _newObtenerTipoBarra.TipoBarraGeneral;

                if (TipoBarra_ == TipoRebar.ELEV_MA_H)
                    ObtenerTrabasMAlla();

                // caso
                ObtenerCaso();

                EntidadBarras _EntidadBarras = Tipos_Barras.M3_Buscar_EntidadBarras_porTipoRebar(TipoBarra_);

                if (_EntidadBarras.TipoParaCub == null)
                { }

                Tipo_cub = _EntidadBarras.TipoParaCub;
                Orientacion_ = _EntidadBarras.Orientacion_Cub_;
                Elemento_ = _EntidadBarras.Elemento_Cub;
                IsOK = true;
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);

            }

            return IsOK;
        }

        private void VerificarSiEstaOculto(Element elemeBarra)
        {
            View _nivel = listaView.Find(c => c.Name == Eje);
            if (_nivel != null)
            {
                bool result = elemeBarra.IsHidden(_nivel);
                IsOculto = (result ? visibi.oculto : visibi.visible);
            }
        }

        private void ObtenerTrabasMAlla()
        {
            double largoRefcm = 31;
            IstrabasSoloMAllaH = true;
            double espaciamiento_mt = ((Rebar)_elementoRebar._rebar).ObtenerEspaciento_cm() / 100;
            double Area = (LargoCm / 100) * espaciamiento_mt * (this.N_Barras - 1);
            int NumeroTraba = ((int)Math.Round((ConstNH.CONST_CANTIDAD_TRABA_MALLA_XM2 * Area) / 2)); // divido por 2 para que cada cara de malla agrege la mitad de la traba

            trabasSoloMAllaH = new CubBarra(_elementoRebar, null, null,null)
            {
                OrdeElevacion=this.OrdeElevacion,
                Id = this.Id,
                CasoRabar_ = CasoRabar.Rebar_,
                BarraId = "0",
                TipoBarra_ = TipoRebar.ELEV_MA_T,
                Caso_ = Caso_cub.Elev,
                Tipo_cub = "MA-T",
                nivel = this.nivel,
                N_Barras = NumeroTraba,
                Diammm = 8,
                LargoCm = largoRefcm,
                Eje = this.Eje,
                Orientacion_ = Orientacion_Cub.H,
                Elemento_ = Elemento_cub.MURO,
                IsOculto = visibi.no_dibujado,
                pesos_ = (8 / 12.73) * (8 / 12.73) * NumeroTraba * (largoRefcm / 100.0f)
            };

            ListaBArrasReparticion.Add(trabasSoloMAllaH);
        }


        private void ObtenerPataBarraLosaYBarraReparticion()
        {
            //a
            double largoRefcm = 80;
            double espaciamiento_mt = elementoPathRebarInSystem._rebarInSystem.ObtenerEspaciento_cm() / 100;
            double Area = (LargoCm / 100) * espaciamiento_mt * (this.N_Barras - 1);
            int NumeroPataBarra = (int)Math.Round((ConstNH.CONST_CANTIDAD_PatABarralOSA_XM2 * Area));


            CubBarra PataBarralosa = new CubBarra(_elementoRebar, null, null,null)
            {
                OrdeElevacion = this.OrdeElevacion,
                Id = this.Id,
                CasoRabar_ = CasoRabar.Path_,
                BarraId = "0",
                TipoBarra_ = TipoRebar.LOSA_SUP_BPT,
                Caso_ = Caso_cub.Losa,
                Tipo_cub = "BPT",
                nivel = this.nivel,
                N_Barras = NumeroPataBarra,
                Diammm = 8,
                LargoCm = largoRefcm,
                Eje = this.Eje,
                Orientacion_ = Orientacion_Cub.sup,
                TipoBarraGeneral = TipoBarraGeneral.Losa,
                Elemento_ = Elemento_cub.LOSA,
                IsOculto = visibi.no_dibujado,
                pesos_ = (8 / 12.73) * (8 / 12.73) * NumeroPataBarra * (largoRefcm / 100.0f)
            };
            ListaBArrasReparticion.Add(PataBarralosa);

            //b
            double largoRefcm_BarraReparticion = (espaciamiento_mt * 100) * (this.N_Barras - 1);
            double largoBarraPrincipal = LargoCm; //se debe corregri para acortar la pata caso s4 S1
            int NumeroBarraReparticion = Util.ParteEnteraInt(largoBarraPrincipal / ConstNH.CONST_ESPACIAMIENTO_BARRREPARTICION_CM);

            CubBarra BarraReparticion = new CubBarra(_elementoRebar, null, null,null)
            {
                OrdeElevacion = this.OrdeElevacion,
                Id = this.Id,
                CasoRabar_ = CasoRabar.Path_,
                BarraId = "0",
                TipoBarra_ = TipoRebar.LOSA_SUP_BR,
                Caso_ = Caso_cub.Losa,
                Tipo_cub = "BR",
                nivel = this.nivel,
                N_Barras = ((int)NumeroBarraReparticion),
                Diammm = 8,
                LargoCm = largoRefcm_BarraReparticion,
                Eje = this.Eje,
                Orientacion_ = Orientacion_Cub.sup,
                TipoBarraGeneral = TipoBarraGeneral.Losa,
                Elemento_ = Elemento_cub.LOSA,
                IsOculto = visibi.no_dibujado,
                pesos_ = (8 / 12.73) * (8 / 12.73) * NumeroBarraReparticion * (largoRefcm_BarraReparticion / 100.0f)
            };
            ListaBArrasReparticion.Add(BarraReparticion);
        }

        private void ObtenerPataBarraFundacion()
        {
            //a
            double largoRefcm = 300;
            double espaciamiento_mt = elementoPathRebarInSystem._rebarInSystem.ObtenerEspaciento_cm() / 100;
            double Area = (LargoCm / 100) * espaciamiento_mt * (this.N_Barras - 1);
            int NumeroPataBarra = ((int)Math.Round((ConstNH.CONST_CANTIDAD_PatABarraFundaciOn_XM2 * Area)));



            CubBarra PataBarrafund = new CubBarra(_elementoRebar, null, null, null)
            {
                OrdeElevacion = this.OrdeElevacion,
                Id = this.Id,
                CasoRabar_ = CasoRabar.Path_,
                BarraId = "0",
                TipoBarra_ = TipoRebar.FUND_SUP_BPT,
                Caso_ = Caso_cub.Fund,
                Tipo_cub = "BPT",
                nivel = this.nivel,
                N_Barras = NumeroPataBarra,
                Diammm = 8,
                LargoCm = largoRefcm,
                Eje = this.Eje,
                Orientacion_ = Orientacion_Cub.sup,
                Elemento_ = Elemento_cub.FUND,
                IsOculto = visibi.no_dibujado,
                pesos_ = (8 / 12.73) * (8 / 12.73) * NumeroPataBarra * (largoRefcm / 100.0f)
            };
            ListaBArrasReparticion.Add(PataBarrafund);


        }
        public string ObtenerDato()
        {
            return
                $"{OrdeElevacion},"+
               // $"{BarraId}," +
                $"{TipoBarra_}," +
                $"{Caso_}," +
                $"{Tipo_cub}," +
                $"{nivel}," +
                $"{N_Barras}," +
                $"{Diammm}," +
                $"{LargoCm}," +
                $"{Eje}," +
                $"{Orientacion_}," +
                $"{Elemento_}," +
                $"{IsOculto}," +
                $"{Id}," +
                $"{CasoRabar_}," +
                $"{TipoBarraGeneral}," +
                $"{pesos_}";

        }



        public object[] ObtenerDato_array()
        {
            return new object[] {

                OrdeElevacion,
              //  BarraId,
                TipoBarra_.ToString(),
                Caso_.ToString(),
                Tipo_cub.ToString(),
                nivel.ToString().Replace("°",""),
                N_Barras,
                Diammm,
                LargoCm,
                Eje.ToString(),
                Orientacion_.ToString(),
                Elemento_.ToString(),
                IsOculto.ToString(),
                Id,
                CasoRabar_.ToString(),
                TipoBarraGeneral.ToString(),
                pesos_

        };

        }
        private void ObtenerCaso()
        {
            if (TipoBarraGeneral == TipoBarraGeneral.Losa
                || TipoBarraGeneral == TipoBarraGeneral.LosaInclinida
                || TipoBarraGeneral == TipoBarraGeneral.LosaEscalera)
            {
                Caso_ = Caso_cub.Losa;
            }
            else if (TipoBarraGeneral == TipoBarraGeneral.Elevacion)
            {
                Caso_ = Caso_cub.Elev;
            }
            else if (TipoBarraGeneral == TipoBarraGeneral.Fundaciones)
            {
                Caso_ = Caso_cub.Fund;
            }
            else if (TipoBarraGeneral == TipoBarraGeneral.Escalera)
            {
                Caso_ = Caso_cub.ESC;
            }
            else if (TipoBarraGeneral == TipoBarraGeneral.NONE)
            {
                Caso_ = Caso_cub.None;
            }
        }

        //implementado solo para rebar, falta para path
        private bool ObtenerNivel(ElementId hostId)
        {
            try
            {
                Level nivel_ = null;
                if (BarraId == "0")
                {
                    if (hostId == null)
                    {
                        IsOK = false;
                        return false;
                    }
                    Element hostElement = _doc.GetElement(hostId);

                    //level--  

                    if (hostElement is FamilyInstance)
                    {
                        nivel_ = ((FamilyInstance)hostElement).ObtenerLevel();
                        nivel = (nivel_ == null ? "null" : nivel_.Name);
                    }
                    else
                    {
                        nivel_ = (Level)_doc.GetElement(hostElement.LevelId);
                        nivel = (nivel_ == null ? "null" : nivel_.Name);
                    }

                }
                else
                {
                    nivel_ = obtenerNivelPorOrigenBarra.ObtenerNivel(_elementoRebar._rebar.ObtenerInicioCurvaMasLarga().Z);

                    nivel = (nivel_ == null ? "null" : nivel_.Name);
                }

                if (nivel == null || nivel == "null")
                {
                    nivel_ = obtenerNivelPorOrigenBarra.ObtenerNivel(_elementoRebar._rebar.ObtenerInicioCurvaMasLarga().Z);

                    nivel = (nivel_ == null ? "null" : nivel_.Name);
                }

                if (nivel == null)
                {
                    IsOK = false;
                    return false;
                }
                else
                {
                    string nameInicial = nivel;
                    if (NombreDefinidoUsuario.ObtenerNombreDefinidoUsuario(nameInicial, _lista_Level_Habilitados))
                        nivel = NombreDefinidoUsuario.nivelModificado;

                    if (NombreDefinidoUsuario.ObtenerOrdenPorElevacion(nameInicial, _lista_Level_Habilitados))
                        OrdeElevacion = NombreDefinidoUsuario.OrdeElevacion;

                }

            }
            catch (Exception)
            {
                IsOK = false;
                return false;
            }
            return true;
        }



        internal CubBarra CopiarDesde(string name,string nivel="",double ordenElevacion=-100000000)
        {
            CubBarra PataBarrafund = new CubBarra(_elementoRebar, null, null, null)
            {
                OrdeElevacion= (ordenElevacion != -100000000 ? ordenElevacion : this.OrdeElevacion),
                Id = this.Id,
                CasoRabar_ = this.CasoRabar_,
                BarraId = this.BarraId,
                TipoBarra_ = this.TipoBarra_,
                Caso_ = this.Caso_,
                Tipo_cub = Tipo_cub,
                nivel = (nivel!=""? nivel: this.nivel),
                N_Barras = this.N_Barras,
                Diammm = this.Diammm,
                LargoCm = this.LargoCm,
                Eje = name,
                Orientacion_ = this.Orientacion_,
                Elemento_ = this.Elemento_,
                IsOculto = this.IsOculto,
                pesos_ = this.pesos_
            };

            return PataBarrafund;
        }
    }
}
