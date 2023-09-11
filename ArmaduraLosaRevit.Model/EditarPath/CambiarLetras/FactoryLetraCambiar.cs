using ArmaduraLosaRevit.Model.EditarPath.CambiarLetras;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CambiarLetras
{
    public class FactoryLetraCambiar
    {

        public static ILetraCambiar  ObtenerParametroLetra(Document doc,PathReinforcement pathReinforcement,string _TipoBarra)
        {
#pragma warning disable CS0219 // The variable 'letraCambiar' is assigned but its value is never used
            ParametroLetra letraCambiar=ParametroLetra.A_;
#pragma warning restore CS0219 // The variable 'letraCambiar' is assigned but its value is never used
            ILetraCambiar newILetraCambiar = new LetraCambiarNULL();

            switch (_TipoBarra)
            {

                case "f1_a":
                case "f1_b":
            
                    newILetraCambiar=   new LetraCambiarSimple(doc,pathReinforcement,  _TipoBarra, ParametroLetra.C_);
                    break;

                case "f1_esc135_sinpata":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //  dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");
                    break;
                case "f1_esc45_conpata":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");
                    break;

                case "f1_SUP":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(15), b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");

                    break;
                case "f9a":
                case "f1":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");
                    break;
                case "s2":
                case "f3_ab":
                case "f3_ba":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: 0, b: Util.CmToFoot(30), c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;
                case "f3_a0":
                case "f3_b0":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: 0, b: Util.CmToFoot(30), c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;
                case "f3_esc135":// PATA LISA ESCALERA
                case "f3_esc45"://PATA LISA ESCALERE
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.A_);
                    //dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment + distanciaBordeLosaHastaEscalera, b: Util.CmToFoot(60), c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;

                case "f3b_esc135":// PATA C HACIA ARRIBA ESCALERA
                case "f3b_esc45"://PATA C  HACIA ARRIBA ESCALERE
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.D_);
                    //dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(espesorEscalera), c: Util.CmToFoot(60), d: LargoPathreiforment + distanciaBordeLosaHastaEscalera, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;
                case "f3_0a":
                case "f3_0b":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: 0, b: 0, c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;
                case "f3":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.A_);
                    //dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;
                case "f9":
                case "f4":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Dere_Sup, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_,D_");
                    break;

                case "s3":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(ConstantesGenerales.CONST_PATA_SX), b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_,D_");
                    break;
                case "f7":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_,D_");
                    break;
                case "s1":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.C_);
                    //dimBarras_internos = new DimensionesBarras(a: Util.CmToFoot(ConstantesGenerales.CONST_PATA_SX), b: _EspesorLosa_EnFoot,
                    //c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: Util.CmToFoot(ConstantesGenerales.CONST_PATA_SX), caso: "RebarShapeNh", LetrasCambiosEspesor: "B_,D_");
                    break;

                case "f10":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.B_);
                    //dimBarras_internos = new DimensionesBarras(_EspesorLosa_EnFoot, b: LargoPathreiforment, c: _EspesorLosa_EnFoot, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_,C_");
                    break;

                case "f11":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.B_);
                    //dimBarras_internos = new DimensionesBarras(_EspesorLosa_EnFoot, b: LargoPathreiforment, c: _EspesorLosa_EnFoot, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_,C_");
                    break;


                case "f10a":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.B_);
                    //dimBarras_internos = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                    break;
                case "f11a":
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.B_);
                    //dimBarras_internos = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                    break;
                case "f16b":
                case "f16a":
                case "f16":
                    
                    newILetraCambiar = new LetraCambiarf16(doc, pathReinforcement, _TipoBarra);
                    //dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    break;

                case "f16_Izq":
                case "f16_Dere":
                    
                 
                    break;
                //dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                case "f17b":
                    newILetraCambiar = new LetraCambiarf17B(doc, pathReinforcement, _TipoBarra);
                    break;
                case "f17a":
                case "f17A_Tras":
                case "f17B_Tras":
                case "f17":
                    newILetraCambiar = new LetraCambiarf17A(doc, pathReinforcement, _TipoBarra);
                    break;
          
                    //dimBarras_internos = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                case "f18":
                    newILetraCambiar = new LetraCambiarf18(doc, pathReinforcement, _TipoBarra);
                    break;
                    //dimBarras_internos = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "A_");
                case "f19":
                    newILetraCambiar = new LetraCambiarf19(doc, pathReinforcement, _TipoBarra);
                    break;
                case "f20A_Dere_Tras":
                case "f20B_Izq_Tras":
                    break;
#pragma warning disable CS0162 // Unreachable code detected
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.A_);
#pragma warning restore CS0162 // Unreachable code detected
                    //dimBarrasAlternativa = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    // dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                case "f20A_Izq_Tras":
                case "f20B_Dere_Tras":
                case "f20bInv":
                case "f20aInv":                
                case "f20b":
                case "f20a":
                case "f20":
                    newILetraCambiar = new LetraCambiarf19(doc, pathReinforcement, _TipoBarra);
                    break;
                    //dimBarrasAlternativa = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //dimBarras_internos = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");                    

                case "f19_Izq":
                case "f19_Dere":
                case "f21A_Izq_Tras":
                case "f21B_Dere_Tras":
                case "f21a":
                case "f21b":
                case "f21":
                    newILetraCambiar = new LetraCambiarf21(doc, pathReinforcement, _TipoBarra);
                    break;
                    //dimBarrasAlternativa = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");                    
                    //dimBarras_internos = new DimensionesBarras(a: _LargoMin_1 * ConstantesGenerales.CONST_PORCENTAJE_LARGOPATA + _EspesorMuro_Izq_abajo, b: _EspesorLosa_EnFoot, c: LargoPathreiforment * 0.85, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");
                case "f21A_Dere_Tras":
                case "f21B_Izq_Tras":
                    break;
#pragma warning disable CS0162 // Unreachable code detected
                    newILetraCambiar = new LetraCambiarSimple(doc, pathReinforcement, _TipoBarra, ParametroLetra.A_);
#pragma warning restore CS0162 // Unreachable code detected
                //dimBarras_internos = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");                    

                case "f22bInv":
                case "f22aInv":
                case "f22b":
                case "f22a":
                case "f22":
                    newILetraCambiar = new LetraCambiarf22(doc, pathReinforcement, _TipoBarra);
                    break;
                default:
                    break;
            }




            return newILetraCambiar;
        }

    
    }
}
