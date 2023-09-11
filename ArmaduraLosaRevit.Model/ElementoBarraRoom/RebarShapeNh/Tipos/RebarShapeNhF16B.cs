using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.NewFolder1;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos
{
    internal class RebarShapeNhF16B : RebarShapeNhBase,IRebarShapeNh
    {
        private bool IsRebarShapeSinInvertir;

        public RebarShapeNhF16B(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _rEbarShapeNHFXXDTO) : base(uiapp,  solicitudDTO, datosNuevaBarraDTO, _rEbarShapeNHFXXDTO)
        {
            //true : igual que caso normal f16A
            IsRebarShapeSinInvertir = true; ///si se quire cambiar o invertir la barra primaria y secuandaria ocupar en falso
        }
        public bool M0_Ejecutar()
        {
            try
            {
                if (!M1_PreCAlculos()) return false;
                if (!M2_EjecutarGeneral()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcular 'M0_Ejecutar'  \n ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }
        public bool M1_PreCAlculos()
        {
            try
            {
                // actualizar largos de ahorro si corresponde
                AyudaLargoPathDTO _AyudaLargoPathDTO = new AyudaLargoPathDTO() { _EspesorLosa_1 = DatosNuevaBarraDTO_.EspesorLosaCm_1 };


                if (IsRebarShapeSinInvertir)
                {
                    if (PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq = PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot;
                    if (PathSymbol_REbarshape_FxxDTO.DesDereSup_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere = PathSymbol_REbarshape_FxxDTO.DesDereSup_foot;
                }
                else
                {
                    if (PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq = PathSymbol_REbarshape_FxxDTO.DesIzqSup_foot;
                    if (PathSymbol_REbarshape_FxxDTO.DesDereSup_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere = PathSymbol_REbarshape_FxxDTO.DesDereInf_foot;
                }
       

                _AyudaLargoPath = new AyudaLargoPath(DatosNuevaBarraDTO_, _AyudaLargoPathDTO);
                _AyudaLargoPath.CalcularLargosYDesplazamientos();



                nombreFamiliaRebarShape = "M_00";
                nombreFamiliaRebarShapeAlternativo = "M_00";
                TiposRebarShape_largoAhorroRedondeo5.ObtenerLargoAhorro5(_AyudaLargoPath.LargoPathreiforment, _AyudaLargoPath.LargoAhorroIzq, _AyudaLargoPath.LargoAhorroDere);

                dimBarras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                dimBarras_parameterSharedLetras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, c2: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                IsBarrAlternative = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcular 'RebarShapeNhFXX' EjecutarGeneral  \n ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }
    }
}