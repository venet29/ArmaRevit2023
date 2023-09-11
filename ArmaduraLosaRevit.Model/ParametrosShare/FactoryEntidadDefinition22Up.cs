using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametrosShare
{
    public class FactoryEntidadDefinition22Up
    {
        public static List<EntidadDefinition> _lista;

        internal static List<EntidadDefinition> CrearListaConParametrosFundaciones(UIApplication uiapp)
        {
            Util.ErrorMsg("Parametros de funsacioneas no implementadas");
            _lista = new List<EntidadDefinition>();
            return _lista;
        }

        internal static List<EntidadDefinition> CrearListaConParametrosEscalera(UIApplication uiapp)
        {
            Util.ErrorMsg("Parametros de funsacioneas no implementadas");
            _lista = new List<EntidadDefinition>();
            return _lista;
        }

        internal static EntidadDefinition AsignarNuevoParametroALista(string nombrepara)
        {
            return new EntidadDefinition(nombrepara);
        }

        
        internal static List<EntidadDefinition> CrearListaConParametrosView(UIApplication uiapp)
        {
            //  Util.ErrorMsg("Parametros de funsacioneas no implementadas");
            _lista = new List<EntidadDefinition>();
             BuiltInCategory[] arrayViewRevision = new BuiltInCategory[] { BuiltInCategory.OST_Sheets, BuiltInCategory.OST_Views };
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Views, "SpecTypeId.String.Text", "ViewNombre", "Generales", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "e01b4f22-4f34-44d6-bf8d-a69aac2aea04");
            AsignarNuevoParametroALista(uiapp, arrayViewRevision, "SpecTypeId.String.Text", FactoryNombre.EstadoViewIsTerminado, "Generales", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "9e39697a-b616-4afb-b765-22271269c50e");          
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Views, "SpecTypeId.String.Text", "EscalaConfiguracion", "Generales", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "9c322d62-e491-4197-967a-4b1eef196e1b");


            return _lista;
        }

        internal static List<EntidadDefinition> CrearListaLargoRevision(UIApplication uiapp)
        {
            _lista = new List<EntidadDefinition>();
         //   BuiltInCategory[] arrayDobleRebar = new BuiltInCategory[] { BuiltInCategory.OST_PathRein, BuiltInCategory.OST_Rebar };
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.Number", "LargoRevision", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "41f1524e-45f3-4379-866f-02d108809a3b");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.Number", "PesoBarra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "ef326ddb-996a-4e18-82d6-96c8d4365d9f");
            return _lista;
        }

        internal static List<EntidadDefinition> CrearListaConParametrosLosa(UIApplication uiapp, bool repetido = false)
        {
            /*
             path 

            IDTipo 				f1,f3,f4	
            IDTipoDireccion  	izq, derecho,inferior,zuperior
            TipoDireccionBarra   s o i


            rebar

            BarraTipo			fi,f2,f3
            BarraOrientacion			izq, derecho,inferior,zuperior
            TipoDireccionBarra   s o i
             */
            _lista = new List<EntidadDefinition>();
            repetido = true;

            // pathreinforment g1          
            if (repetido) AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.Number", "Espesor", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b3ab9217-e4bb-4d98-8fe5-55e52fef73d8");
            //AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, ParameterType.Length, "Espesor_Losa", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "ElementoBorrar", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "b22d1526-28d3-445e-8357-1f4d3cd59c7e");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.Angle", "Angulo", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Angulo en grados", "9a773f67-6c0a-465e-a8eb-a77da464e02f");
            if (repetido) AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "Direccion Horizontal", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Indica si direccion vertical es principal", "a1aa0f6e-40ef-4726-a943-a5c84d0a33d9");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.Length", "LargoMax", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Largo maximo de losa", "03d5a49e-e1ed-438d-ac96-1fcd65ca8639");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms,"SpecTypeId.Length", "LargoMin", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Largo minimo de losa", "0c5833a1-7ddb-4a5b-aaea-b5f6263dafae");
            if (repetido) AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "Numero Losa", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Numero descriptivo de losa", "39b6b8c1-f642-4952-9744-f510ae1c5d73");
            if (repetido) AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "Direccion Vertical", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Indica si direccion vertical es princiapl", "cae874db-c186-48c8-8677-18e4c015f466");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "Cuantia Vertical", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Indica la cuantia vertica de diseño", "3613b6f4-715a-4313-ba71-56d7b28e6b72");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rooms, "SpecTypeId.String.Text", "Cuantia Horizontal", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Indica la cuantia horizontal de diseño", "e39a52fe-11ae-4c6b-b871-b2a50f3c5ac6");

            // pathreinforment g2
            if (repetido)
            {
                BuiltInCategory[] arrayDobleRebar = new BuiltInCategory[] { BuiltInCategory.OST_PathRein, BuiltInCategory.OST_Rebar };

                //AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "BBOpt", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "580ddc62-7546-4da1-8a80-bcc52da1b4f6");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "A_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "26fa0a89-bd02-4240-bbab-d415408554a9");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "B_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "9ffa0f81-c67c-4487-8dd2-88ad10ec6ef0");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "C_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "769c49c1-3908-4ce9-9e21-c20a70eab507");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "C2_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "bc2f635b-4ce1-44b4-9761-454c50e99df3");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "D_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b86c2822-7114-4f51-bb15-381463e87977");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "E_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "c4b9121c-24f1-4c2a-be34-82a82750ecbb");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "F_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "1e44c3c7-d83d-45dd-96c3-e8cb2743b14d");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "G_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "63cf5205-fc74-4d49-bddd-f6810b02007f");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "H_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "07e58240-e8c2-4912-bdb5-4ed070f141e6");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Number", "PesoBarra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "ef326ddb-996a-4e18-82d6-96c8d4365d9f");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "Prefijo_F", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "d85a45da-96f7-4576-a243-58cfe065d816");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Boolean.YesNo", "EsPrincipal", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Barra es principal?", "fb57db3d-c223-4009-8a12-f336210239f6");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "IDTipoDireccion", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "3ad0cc4a-e913-47af-9057-ee616edaba5e");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "TipoDireccionBarra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "5336e963-ed8d-4cc2-a8f8-08e79f1f63b0");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "CantidadBarra", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "0368c956-9e15-41f5-a86b-375c0c55b11e");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "LargoParciales", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "53530261-a393-46a6-9ae0-01639eb6a3f8");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "LargoTotal", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d0bcbf64-eaca-4d45-92c9-a3a5bda8e75e");



                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Number", "IDNumero", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "69f76586-85c2-49da-8cb2-6c4d22431e55");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "IDTipo", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "2847f3ed-47ea-4984-827e-68d811265066");
            }



            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, "SpecTypeId.String.Text", "LargoParciales2", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "50d1116e-8b0b-4050-8d71-33cca319af9e");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, "SpecTypeId.String.Text", "LargoTotal2", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "b4b7b5d4-45a0-4364-9697-bebd847af7e9");

            // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "IDTipoDireccion", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "3f6efa10-e57b-4732-9314-d877ebad98b4");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "BarraOrientacion", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "de33c5d3-f153-4204-a2ea-3bfaa4759193");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, "SpecTypeId.ReinforcementLength", "L2barra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "bbbea68c-7e83-4fed-8adb-10e8ebc23dc7");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, "SpecTypeId.String.Text", "NumeroPrimario", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "4018c065-4c1d-411c-b752-10271a62423c");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_PathRein, "SpecTypeId.String.Text", "NumeroSecundario", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "9147f03d-cb01-451c-9ff9-a0724a7e5314");


            return _lista;
        }

        internal static List<EntidadDefinition> CrearListaConParametrosDesglose(UIApplication uiapp, bool repetido = false)
        {

            _lista = new List<EntidadDefinition>();

            //solo desglose
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "IdMLB", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "a133d20d-32bf-4780-88e9-9826c3d4a5d2");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "MLB", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Tipo de grupo de barra", "0f022cc2-8b49-470a-8687-2d8e4feabd3f");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "OpcionCuantia", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d711e591-ffd0-48d9-985b-da3039cbfd6a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.ReinforcementLength", "LargoSumaParciales", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "014e7044-cdb9-4bba-80e0-5251d13a2dda");
            return _lista;

        }

        // no se utilizo  para  no se aplica a todas los tipos shaft
        internal static List<EntidadDefinition> CrearListaConParametrosPasadas(UIApplication uiapp, bool repetido = false)
        {

            _lista = new List<EntidadDefinition>();

            //solo desglose
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_ShaftOpening, "SpecTypeId.String.Text", "PASADA_Estado", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "34e90951-45d6-42ab-b2ea-f135cc7f7566");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_GenericModel, "SpecTypeId.String.Text", CONST_PARAMETER.CONT_INFO_GENERICMODEL, "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "496260ab-c170-4507-afad-d9360e8bc507");
           // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_SWallRectOpening, "SpecTypeId.String.Text", "PASADA_EstadoWall", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "0e8b2651-aad5-4ddd-9682-8cc2926a66fe");
           // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_StructuralFraming, "SpecTypeId.String.Text", "PASADA_EstadoViga", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b0bfbf7b-dae2-4d46-940f-a7fc63ab7307");
            return _lista;

        }

        internal static List<EntidadDefinition> CrearListaConParametrosEemento(UIApplication uiapp, bool repetido = false)
        {

            _lista = new List<EntidadDefinition>();
            BuiltInCategory[] arrayDobleRebar = new BuiltInCategory[] { BuiltInCategory.OST_Walls, BuiltInCategory.OST_StructuralFraming };
            //solo desglose
            AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", CONST_PARAMETER.CONT_INFORVT, "Generales", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "5d54d044-cec5-4385-8062-3da7b7d75829");
            //AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_FloorOpening, "SpecTypeId.String.Text", "PASADA_EstadoFlorr", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "406d2b32-9aeb-47da-9d10-a18f43d3990a");
            // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_SWallRectOpening, "SpecTypeId.String.Text", "PASADA_EstadoWall", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "0e8b2651-aad5-4ddd-9682-8cc2926a66fe");
            // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_StructuralFraming, "SpecTypeId.String.Text", "PASADA_EstadoViga", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b0bfbf7b-dae2-4d46-940f-a7fc63ab7307");
            return _lista;

        }

        internal static List<EntidadDefinition> CrearListaConParametrosBIM(UIApplication uiapp, bool repetido = false)
        {

            _lista = new List<EntidadDefinition>();

            //solo desglose
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Conduit, "SpecTypeId.String.Text", "GrupoConduit", "BIM", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "13e84d68-e5e6-4c97-aec1-c97b7bf794ba");
            //AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_GenericModel, "SpecTypeId.String.Text", "PASADA_EstadoFlorr", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "406d2b32-9aeb-47da-9d10-a18f43d3990a");
            // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_SWallRectOpening, "SpecTypeId.String.Text", "PASADA_EstadoWall", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "0e8b2651-aad5-4ddd-9682-8cc2926a66fe");
            // AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_StructuralFraming, "SpecTypeId.String.Text", "PASADA_EstadoViga", "Pasadas", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b0bfbf7b-dae2-4d46-940f-a7fc63ab7307");
            return _lista;

        }



        internal static List<EntidadDefinition> CrearListaConParametrosElevaciones(UIApplication uiapp, bool repetido = false)
        {
            _lista = new List<EntidadDefinition>();
            repetido = true;
            // pathreinforment g2
            // pathreinforment g2
            if (repetido)
            {
                BuiltInCategory[] arrayDobleRebarVita = new BuiltInCategory[] { BuiltInCategory.OST_PathRein, BuiltInCategory.OST_AreaRein, BuiltInCategory.OST_Rebar, };
                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.String.Text", "NombreVista", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "8a9a3d39-9adc-4738-947d-53db9169c41e");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.String.Text", "NombreVista2", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "cdb9442a-96e5-4a81-b362-d19e314bbd99");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.String.Text", "BarraTipo", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b753643d-b4db-4022-b9bf-355f9172dd4e");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.Int.Integer", "IdBarraCopiar", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "6ddf5b15-a1f1-491f-a22e-8f351cd2b065");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.Number", "IdBarra", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "4181eee7-652e-46ed-a84e-669218be698b");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebarVita, "SpecTypeId.String.Text", "Correlativo", "Rebar General", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "f0a2cda2-29dd-4529-affb-6641ffcc57d0");


                BuiltInCategory[] arrayDobleRebar = new BuiltInCategory[] { BuiltInCategory.OST_PathRein, BuiltInCategory.OST_Rebar };
                // AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "BBOpt", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "580ddc62-7546-4da1-8a80-bcc52da1b4f6");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "LargoOpt", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b9079984-dba1-4eae-a8ef-e6202101b65b");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "A_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "26fa0a89-bd02-4240-bbab-d415408554a9");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "B_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "9ffa0f81-c67c-4487-8dd2-88ad10ec6ef0");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "C_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "769c49c1-3908-4ce9-9e21-c20a70eab507");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "D_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "b86c2822-7114-4f51-bb15-381463e87977");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "E_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "c4b9121c-24f1-4c2a-be34-82a82750ecbb");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "F_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "1e44c3c7-d83d-45dd-96c3-e8cb2743b14d");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "G_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "63cf5205-fc74-4d49-bddd-f6810b02007f");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.ReinforcementLength", "H_", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "07e58240-e8c2-4912-bdb5-4ed070f141e6");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Number", "PesoBarra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "ef326ddb-996a-4e18-82d6-96c8d4365d9f");


                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "Prefijo_F", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d85a45da-96f7-4576-a243-58cfe065d816");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Boolean.YesNo", "EsPrincipal", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "Barra es principal?", "fb57db3d-c223-4009-8a12-f336210239f6");

                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "TipoDireccionBarra", "Rebar Shape Parameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "a2d761c4-cae8-4e98-ab1f-d3c4043cff74");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "IDTipoDireccion", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "3f6efa10-e57b-4732-9314-d877ebad98b4");


                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "CantidadBarra", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "0368c956-9e15-41f5-a86b-375c0c55b11e");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "LargoParciales", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "53530261-a393-46a6-9ae0-01639eb6a3f8");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "LargoTotal", "ArmaduraLosa", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d0bcbf64-eaca-4d45-92c9-a3a5bda8e75e");


                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.Number", "IDNumero", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "69f76586-85c2-49da-8cb2-6c4d22431e55");
                AsignarNuevoParametroALista(uiapp, arrayDobleRebar, "SpecTypeId.String.Text", "IDTipo", "PathReinformentSymbolParameters", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "2847f3ed-47ea-4984-827e-68d811265066");
            }

            //AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "DiametroRevision", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "c145309f-c2e2-45a4-85a7-81cf5ca084d3");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.Number", "LargoRevision", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: true, "", "41f1524e-45f3-4379-866f-02d108809a3b");


            // pathreinforment g6
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "CuantiaRefuerzo", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "d960cf8b-35eb-4a95-b565-1d1cb1a563a2");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "TipoDiametro", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "86dc5e3a-d3d2-431b-82a2-6358425e78b3");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "NumeroLinea", "Rebar", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "9f52e3a6-80a2-4390-bddd-83e59c5b30cc");

            // pathreinforment g8
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "CantidadEstriboCONF", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "630baf2e-6d65-4a4c-b331-fcb9e7d63a7a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "CantidadEstriboLAT", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "f34f3a0f-de70-4c05-b3c6-2a216d7f758a");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "CantidadEstriboTRABA", "Estribo", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "c6a11417-9b23-4742-ac77-ac9bf1d4246d");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_AreaRein, "SpecTypeId.String.Text", "CuantiaMuro", "MallaMuro", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "aca8ff50-39cc-4d12-bf90-dc0dadfa2266");

            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "MallaRebarMuro", "MallaMuro", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "fe54e8c5-aacd-4cac-ab65-c09032b31529");
            AsignarNuevoParametroALista(uiapp, BuiltInCategory.OST_Rebar, "SpecTypeId.String.Text", "IdMallaGrupo", "MallaMuro", IsModificable: true, EsOcultoCuandoNOvalor: false, EsVisible: false, "", "94f7b65e-5354-4aa9-9158-e18baea942cd");
            return _lista;
        }

        public static void AsignarNuevoParametroALista(UIApplication uiapp, BuiltInCategory builtInCategory, string parameterType, string nombreParametro,
                                                    string nombreGrupo, bool IsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description, string _guid = "")
        {
            try
            {
                EntidadDefinition _entidadDefinition1 = new EntidadDefinition(uiapp, builtInCategory, parameterType, nombreParametro, nombreGrupo, IsModificable, EsOcultoCuandoNOvalor, EsVisible, Description, _guid);
                if (_entidadDefinition1 == null)
                {
                    Util.ErrorMsg($"No se pudo crear parametro compartido {nombreParametro}");
                    return;
                }
                _lista.Add(_entidadDefinition1);
            }
            catch (Exception)
            {

                Util.ErrorMsg($"Error al crear parametro compartido {nombreParametro}");
            }
        }
        public static void AsignarNuevoParametroALista(UIApplication uiapp, BuiltInCategory[] builtInCategory, string parameterType, string nombreParametro, string nombreGrupo,
                                            bool IsModificable, bool EsOcultoCuandoNOvalor, bool EsVisible, string Description, string _guid = "")
        {
            try
            {
                EntidadDefinition _entidadDefinition1 = new EntidadDefinition(uiapp, builtInCategory, parameterType, nombreParametro,
                                                                              nombreGrupo, IsModificable, EsOcultoCuandoNOvalor, EsVisible, Description, _guid);
                if (_entidadDefinition1 == null)
                {
                    Util.ErrorMsg($"No se pudo crear parametro compartido {nombreParametro}");
                    return;
                }
                _lista.Add(_entidadDefinition1);
            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error al crear parametro compartido {nombreParametro}");
            }
        }
    }
}
