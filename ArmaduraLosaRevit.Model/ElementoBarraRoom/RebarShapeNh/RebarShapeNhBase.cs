using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos
{
    internal class RebarShapeNhBase
    {
        #region 0) propiedades
        protected UIApplication _uiapp;
        private readonly SolicitudBarraDTO solicitudDTO;
        protected Document _doc;
        protected View _view;
        protected PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FxxDTO;
        protected AyudaLargoPath _AyudaLargoPath;

        RebarShape rebarshape;
        //puede ser  v3, v4 segun se necesite miesras se define una version definitiva
        private string versionPAth = "v2"; // soloe se usa para , para poder cambiar los rebarshape para evvitar borrar lo actuales y generar nueva version de rebarshape

        public RebarShape tipoRebarShapePrincipal { get; set; }
        public RebarShape tipoRebarShapeAlternativa { get; set; }

        protected string nombreFamiliaRebarShape;
        protected string nombreFamiliaRebarShapeAlternativo;

        protected UbicacionLosa _ubicacionEnlosa;
        
        public DimensionesBarras dimBarras { get; set; }
        public DimensionesBarras dimBarrasAlternativa { get; set; }
        public DimensionesBarras dimBarras_parameterSharedLetras { get; set; }
        public DatosNuevaBarraDTO DatosNuevaBarraDTO_ { get; set; }
        //double LargoPathreiforment;

        public bool IsBarrAlternative { get; set; }

        public bool IsokPrimario { get; set; }
        public bool IsokSecundario { get; set; }
        #endregion
        

        public RebarShapeNhBase(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _REbarShapeNHFXXDTO)
        {
            this._uiapp = uiapp;
            this.solicitudDTO = solicitudDTO;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
            DatosNuevaBarraDTO_ = datosNuevaBarraDTO;
            PathSymbol_REbarshape_FxxDTO = _REbarShapeNHFXXDTO;

            nombreFamiliaRebarShape = "";
            nombreFamiliaRebarShapeAlternativo = "";
            _ubicacionEnlosa = solicitudDTO.UbicacionEnlosa;
            //configuracion default se puede reconfigurar en casos espeaciale 
            AyudaLargoPathDTO _AyudaLargoPathDTO = new AyudaLargoPathDTO() { _EspesorLosa_1 = datosNuevaBarraDTO.EspesorLosaCm_1 };
            _AyudaLargoPath = new AyudaLargoPath(datosNuevaBarraDTO, _AyudaLargoPathDTO);
        }


        public bool M2_EjecutarGeneral()
        {
            try
            {
                IsokPrimario = false;
                //primario
                if (ObtenerRebarSHape(nombreFamiliaRebarShape,  dimBarras))
                {
                    tipoRebarShapePrincipal = rebarshape;
                    IsokPrimario = true;
                }

                IsokSecundario = false;
                //secundario
                if (nombreFamiliaRebarShapeAlternativo != "")
                {
                    if (ObtenerRebarSHape(nombreFamiliaRebarShapeAlternativo,  dimBarrasAlternativa))
                    {
                        tipoRebarShapeAlternativa =rebarshape;
                        IsokSecundario = true;
                    }
                }


                if (dimBarras != null)
                {
                    dimBarras._LargoAhorro_Izq_ = _AyudaLargoPath.LargoAhorroIzq;
                    dimBarras._LargoAhorro_Dere_ = _AyudaLargoPath.LargoAhorroDere;

                }

                if (dimBarrasAlternativa != null)
                {
                    dimBarrasAlternativa._LargoAhorro_Izq_ = _AyudaLargoPath.LargoAhorroIzq;
                    dimBarrasAlternativa._LargoAhorro_Dere_ = _AyudaLargoPath.LargoAhorroDere;
                }

                ObtenerdatosNuevaBarra();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcular 'RebarShapeNhFXX' EjecutarGeneral  \n ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }



        private bool ObtenerRebarSHape(string nombreFamiliaRebarShape,  DimensionesBarras dimBarras_)
        {
            try
            {
                rebarshape = null;

                if (nombreFamiliaRebarShape == "M_00") // si NO nesista modificar rebarshape
                {
                    rebarshape = TiposFormasRebarShape.getRebarShape(nombreFamiliaRebarShape, _doc);
                }
                else // Si nesista modificar rebarshape
                {
                    Family fam = null;

                    //Los buscar segum el nombre
                    string nombreFamiliaRebarShape_modif = nombreFamiliaRebarShape + "_A" + dimBarras_.a.valorCM + "_B" + dimBarras_.b.valorCM + "_C" + dimBarras_.c.valorCM + "_D" + dimBarras_.d.valorCM + "_E" + dimBarras_.e.valorCM + versionPAth;

                    rebarshape = TiposFormasRebarShape.getRebarShape(nombreFamiliaRebarShape_modif, _doc);

                    if (rebarshape != null) return true; ;

                    // si no enceuntra la barra especifica, busca la rebarshape de plantilla
                    fam = TiposRebarShapeFamilia.M1_GetRebarShapeFamilia(nombreFamiliaRebarShape, _doc);
                    // si lo encuentra el modifca los parametros
                    if (fam != null && fam.IsValidObject)
                    {

                        //double factor = (nombreFamiliaRebarShape == "NH4_bajo") ? 8.0f : 4.0f;
                        // int factor2 = (nombreFamiliaRebarShape == "NH4_bajo") ? 8 : 4;
                        //double factorRebarShape2= factor2 / largobaa;
                        //crea una copia de la la familianhs
                        double factorRebarShape = 1;// factor / largobaa;
                        string newNombreFamiliaRebarShape = ChangeFamilyRebar.SetDimensionRebarShape(fam, _doc, dimBarras_, nombreFamiliaRebarShape, factorRebarShape, true, versionPAth);
                        // rebarshape = ChangeFamilyRebar.SetDimensionRebarShape1(fam, doc, dimBarras_, nombreFamiliaRebarShape, factor / largobaa, true);
                        rebarshape = TiposFormasRebarShape.getRebarShape(newNombreFamiliaRebarShape, _doc);

                    }

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerRebarSHape' y la familia {nombreFamiliaRebarShape}  \n  ex{ex.Message}");
                return false;
            }
            return true;
        }


        private DatosNuevaBarraDTO ObtenerdatosNuevaBarra()
        {
            if (tipoRebarShapePrincipal == null)
            { }
            DatosNuevaBarraDTO_.tipoRebarShapePrincipal = tipoRebarShapePrincipal;
            DatosNuevaBarraDTO_.tipoRebarShapeAlternativa = tipoRebarShapeAlternativa;

            DatosNuevaBarraDTO_.nombreFamiliaRebarShape = nombreFamiliaRebarShape;
            DatosNuevaBarraDTO_.nombreFamiliaRebarShapeAlternativo = nombreFamiliaRebarShapeAlternativo;

            DatosNuevaBarraDTO_.dimBarras_parameterSharedLetras = dimBarras_parameterSharedLetras;
            DatosNuevaBarraDTO_.dimBarrasAlternativa = dimBarrasAlternativa;
            DatosNuevaBarraDTO_.dimBarras = dimBarras;

            DatosNuevaBarraDTO_.LargoPaTa_foot = _AyudaLargoPath.LargoPaTa;
            DatosNuevaBarraDTO_.LargoAhorroDere = _AyudaLargoPath.LargoAhorroDere;
            DatosNuevaBarraDTO_.LargoAhorroIzq = _AyudaLargoPath.LargoAhorroIzq;
            DatosNuevaBarraDTO_.LargoPataInf = _AyudaLargoPath.largoPataInf;

            DatosNuevaBarraDTO_.IsBarrAlternative = IsBarrAlternative;

            return DatosNuevaBarraDTO_;
        }

    }
}