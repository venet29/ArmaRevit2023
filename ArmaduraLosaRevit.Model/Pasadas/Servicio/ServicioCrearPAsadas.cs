using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.Servicio
{
    public class PAsadasCuandradaDTO
    {

        public double ancho_foot { get; set; }
        public double largo_foot { get; set; }
        public double Espesor_foot { get; set; }

        public PlanarFace face { get; set; }

        public XYZ puntoInsercion { get; set; }
        public XYZ NormalFace { get; internal set; }
        public ColorTipoPasada NombrePasada { get; internal set; }
        public string NombreEje { get; internal set; }
        public TipoElementoBArraV NombreElementoIntersectado { get; internal set; }
    }

    public class ServicioCrearPAsadas
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private ServicioActivarFamiliaPasada _servicioActivarFamiliaPasada;
        private FamilySymbol FamilySymbolPAsada;

        public FamilyInstance familyInstance { get; set; }

        public ServicioCrearPAsadas(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;

            this._doc = _uiapp.ActiveUIDocument.Document;
            _servicioActivarFamiliaPasada = new ServicioActivarFamiliaPasada(_uiapp);
        }

        public bool Crear_sintrasn(PAsadasCuandradaDTO _PAsadasCuandradaDTO)
        {
            try
            {
                familyInstance = null;


                if (_PAsadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_VERDE)
                    FamilySymbolPAsada = _servicioActivarFamiliaPasada.ObtenerFamilia(ColorTipoPasada.PASADA_VERDE);
                else if (_PAsadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_ROJA)
                    FamilySymbolPAsada = _servicioActivarFamiliaPasada.ObtenerFamilia(ColorTipoPasada.PASADA_ROJA);
                else if (_PAsadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_NARANJO)
                    FamilySymbolPAsada = _servicioActivarFamiliaPasada.ObtenerFamilia(ColorTipoPasada.PASADA_NARANJO);
                else if (_PAsadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_AZUL)
                    FamilySymbolPAsada = _servicioActivarFamiliaPasada.ObtenerFamilia(ColorTipoPasada.PASADA_AZUL);
                else if (_PAsadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_GRIS)
                    FamilySymbolPAsada = _servicioActivarFamiliaPasada.ObtenerFamilia(ColorTipoPasada.PASADA_GRIS);

                //if (!M1_ObtenerFamilia_ModelGeneric(_PAsadasCuandradaDTO.NombrePasada)) return false;

                //  XYZ puntoInsercion = face.ObtenerCenterDeCara();
                XYZ direccion = -XYZ.BasisZ.CrossProduct(_PAsadasCuandradaDTO.NormalFace);//. new XYZ(0, 1, 0);

                if (Util.IsSimilarValor(direccion.GetLength(), 0, 0.001))
                    direccion = XYZ.BasisX;

                M2_crearPAsadad_modeGeneric_SinTrans(_PAsadasCuandradaDTO.puntoInsercion, direccion, _PAsadasCuandradaDTO.face, _PAsadasCuandradaDTO);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Crear_sintrasn'  EX:{ex.Message}");
            }
            return true;
        }
        public bool M1_ObtenerFamilia_ModelGeneric(string nombrePasada)
        {
            try
            {
                if (FamilySymbolPAsada != null)
                {
                    if (!FamilySymbolPAsada.IsActive)
                        M2_ActivarFamiliaPasada();

                    return true;
                }

                FamilySymbolPAsada = TiposGenericFamilySymbol.M1_GenericFamilySymbol_nh(nombrePasada, BuiltInCategory.OST_GenericModel, _doc);

                if (FamilySymbolPAsada == null)
                {
                    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                    _ManejadorCargarFAmilias.cargarFamilias_Pasada();

                    FamilySymbolPAsada = TiposGenericFamilySymbol.M1_GenericFamilySymbol_nh(nombrePasada, BuiltInCategory.OST_GenericModel, _doc);
                }

                if (!FamilySymbolPAsada.IsActive)
                    M2_ActivarFamiliaPasada();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_ObtenerFamilia_ModelGeneric' EX:{ex.Message}");

            }
            return true;
        }


        public bool M2_ActivarFamiliaPasada()
        {
            try
            {
                if (FamilySymbolPAsada == null) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ActivarPAsada-NH");


                    if (!FamilySymbolPAsada.IsActive)
                        FamilySymbolPAsada.Activate();

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_ActivarFamiliaPasada'  EX:{ex.Message}");
            }
            return true;
        }


        public bool M2_crearPAsadad_modeGeneric_ConTrans(XYZ ptoInserccion, XYZ direccion, Face face, PAsadasCuandradaDTO _PAsadasCuandradaDTO)
        {
            try
            {
                if (FamilySymbolPAsada == null) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CrearPelotaLosaEstructural_aux-NH");

                    M2_crearPAsadad_modeGeneric_SinTrans(ptoInserccion, direccion, face, _PAsadasCuandradaDTO);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_crearPAsadad_modelGeneric_ConTrans'    EX:{ex.Message}");
            }
            return true;
        }
        public bool M2_crearPAsadad_modeGeneric_SinTrans(XYZ ptoInserccion, XYZ direccion, Face face, PAsadasCuandradaDTO _PAsadasCuandradaDTO)
        {
            try
            {
                if (FamilySymbolPAsada == null) return false;
                //agrega la annotation generico de pelota de losa 

                familyInstance = _doc.Create.NewFamilyInstance(face, ptoInserccion, direccion, FamilySymbolPAsada);

                if (familyInstance == null) return false;

                var nm = ParameterUtil.SetParaInt(familyInstance, "Ancho", _PAsadasCuandradaDTO.ancho_foot);
                var es = ParameterUtil.SetParaInt(familyInstance, "Largo", _PAsadasCuandradaDTO.largo_foot);

                var esp = ParameterUtil.SetParaInt(familyInstance, "Espesor de muro", _PAsadasCuandradaDTO.Espesor_foot);

                ParameterUtil.SetParaStringNH(familyInstance, CONST_PARAMETER.CONT_INFO_GENERICMODEL, 
                    $"{familyInstance.Id.IntegerValue},{_PAsadasCuandradaDTO.NombreEje},{_PAsadasCuandradaDTO.NombreElementoIntersectado}");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_crearPAsadad_modeGeneric_SinTrans'    EX:{ex.Message}");
            }
            return true;
        }
    }
}
