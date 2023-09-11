using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
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
    internal class RebarShapeNhF19 : RebarShapeNhBase,IRebarShapeNh
    {

        public RebarShapeNhF19(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _rEbarShapeNHFXXDTO) 
            : base(uiapp, solicitudDTO, datosNuevaBarraDTO, _rEbarShapeNHFXXDTO)
        {

        }
        public bool M0_Ejecutar()
        {
            try
            {
                AyudaLargoPathDTO _AyudaLargoPathDTO = new AyudaLargoPathDTO() { _EspesorLosa_1 = DatosNuevaBarraDTO_.EspesorLosaCm_1 };

                //   LargoAhoraDefinidoUsuario_Dere solo se calula ese pq apra el caso F20 el espacieinteo arriba y bajo siempre es el mismo lugar
                if (PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_izq = PathSymbol_REbarshape_FxxDTO.DesIzqInf_foot;
                if (PathSymbol_REbarshape_FxxDTO.DesDereSup_foot > 0) _AyudaLargoPathDTO.LargoAhoraDefinidoUsuario_Dere = PathSymbol_REbarshape_FxxDTO.DesDereSup_foot;

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
                //*************************************

                nombreFamiliaRebarShapeAlternativo = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1A";
                nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1B" : "NH_F1B";

                TiposRebarShape_largoAhorroRedondeo5_Alt.ObtenerLargoAhorro5_Alt(_AyudaLargoPath.LargoPathreiforment, _AyudaLargoPath.LargoAhorroIzq, _AyudaLargoPath.LargoAhorroDere);
                //para guardar datos internos
                dimBarras = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                dimBarrasAlternativa = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                double aux_largo2 = _AyudaLargoPath.LargoPaTa + _AyudaLargoPath._EspesorLosa_EnFoot + TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere;
                //para guardar datos internos
                string aux_Letracambiar = TipoLetraCambiar.ObtenerLetra("f19");
                dimBarras_parameterSharedLetras = new DimensionesBarras(a: _AyudaLargoPath.LargoPaTa, b: _AyudaLargoPath._EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltDere, c2: TiposRebarShape_largoAhorroRedondeo5_Alt.largo5_AltIzq, d: 0, e: 0, g: 0, largo2: aux_largo2, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

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