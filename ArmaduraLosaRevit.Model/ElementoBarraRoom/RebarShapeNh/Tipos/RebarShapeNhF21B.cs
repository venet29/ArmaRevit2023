using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.NewFolder1;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos
{
    internal class RebarShapeNhF21B : RebarShapeNhBase,IRebarShapeNh
    {
        public RebarShapeNhF21B(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _rEbarShapeNHFXXDTO) : base(uiapp, solicitudDTO,datosNuevaBarraDTO, _rEbarShapeNHFXXDTO)
        {
   
        }
        public bool M0_Ejecutar()
        {
            try
            {
                AyudaLargoPathDTO _AyudaLargoPathDTO = new AyudaLargoPathDTO() { _EspesorLosa_1 = DatosNuevaBarraDTO_.EspesorLosaCm_1 };

                //   LargoAhoraDefinidoUsuario_Dere solo se calula ese pq apra el caso F20 el espacieinteo arriba y bajo siempre es el mismo lugar
                if (PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq = PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot;
                if (PathSymbol_REbarshape_FxxDTO.DesDereSup_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq = PathSymbol_REbarshape_FxxDTO.DesDereSup_foot;

                _AyudaLargoPath = new AyudaLargoPath(DatosNuevaBarraDTO_, _AyudaLargoPathDTO);
                _AyudaLargoPath.CalcularLargosYDesplazamientos();


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
                nombreFamiliaRebarShape = "NH_F1B";
                nombreFamiliaRebarShapeAlternativo = "M_00";

                TiposRebarShape_largoAhorroRedondeo5_Alt.ObtenerLargoAhorro5_Alt(_AyudaLargoPath.LargoPathreiforment, _AyudaLargoPath.LargoAhorroIzq, _AyudaLargoPath.LargoAhorroDere);


                dimBarras = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, c2: 0, d: 0, e: 0, g: 0, largo2: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                //para guardar datos internos
                dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, b: 0, c: 0, c2: 0, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

               // double auxLargo2 = TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq + _AyudaLargoPath.LargoPaTa + _AyudaLargoPath._EspesorLosa_EnFoot;
                //para guardar datos internos            
                dimBarras_parameterSharedLetras = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, c2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");

                IsBarrAlternative = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcular 'RebarShapeNhFXX' EjecutarGeneral  \n ex:{ex.Message} ");
                return false; ;
            }
            return true;
        }

        public bool M1_PreCAlculosv2()
        {
            try
            {
                nombreFamiliaRebarShape = "M_00";
                nombreFamiliaRebarShapeAlternativo = "NH_F1B";

                TiposRebarShape_largoAhorroRedondeo5_Alt.ObtenerLargoAhorro5_Alt(_AyudaLargoPath.LargoPathreiforment, _AyudaLargoPath.LargoAhorroIzq, _AyudaLargoPath.LargoAhorroDere);

                //para guardar datos internos
                dimBarras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                double auxLargo2 = TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq + _AyudaLargoPath.LargoPaTa + _AyudaLargoPath._EspesorLosa_EnFoot;
                dimBarrasAlternativa = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, c2: 0, d: 0, e: 0, g: 0, largo2: auxLargo2, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                //para guardar datos internos            
                dimBarras_parameterSharedLetras = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, c2: 0, d: 0, e: 0, g: 0, largo2: auxLargo2, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");

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