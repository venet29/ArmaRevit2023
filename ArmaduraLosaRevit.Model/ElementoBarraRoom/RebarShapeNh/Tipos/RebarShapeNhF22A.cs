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
    internal class RebarShapeNhF22A : RebarShapeNhBase, IRebarShapeNh
    {

        public RebarShapeNhF22A(UIApplication uiapp, SolicitudBarraDTO solicitudDTO,DatosNuevaBarraDTO datosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO pathSymbol16ADTO):base(uiapp, solicitudDTO, datosNuevaBarraDTO, pathSymbol16ADTO)
        {
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
         
                if (PathSymbol_REbarshape_FxxDTO.DesDereInf_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere = PathSymbol_REbarshape_FxxDTO.DesDereInf_foot;
                if (PathSymbol_REbarshape_FxxDTO.DesDereSup_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere = PathSymbol_REbarshape_FxxDTO.DesDereSup_foot ;

                _AyudaLargoPath = new AyudaLargoPath(DatosNuevaBarraDTO_, _AyudaLargoPathDTO);
                _AyudaLargoPath.CalcularLargosYDesplazamientos();

        

                nombreFamiliaRebarShape = "M_00";
                nombreFamiliaRebarShapeAlternativo = "M_00";
                TiposRebarShape_largoAhorroRedondeo5_Alt.ObtenerLargoAhorroDer5_Alt(_AyudaLargoPath.LargoPathreiforment, _AyudaLargoPath.LargoAhorroDere);

                dimBarras = new DimensionesBarras(a: _AyudaLargoPath.LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                dimBarras_parameterSharedLetras = new DimensionesBarras(a: _AyudaLargoPath.LargoPathreiforment, b: 0, c: 0, c2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
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