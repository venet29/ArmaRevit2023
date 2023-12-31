﻿using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.Factory
{
    class FactoryExtStore
    {
        #region Registro SOLO pruebas

        //1
        public static DatosExtStoreDTO ObtnerExtStorePrueba()
        {

            var result = new DatosExtStoreDTO()
            {
                Documentation = "hola",
                ReadAccessLevel = AccessLevel.Public,
                WriteAccessLeve = AccessLevel.Public,
                SchemaName = "prueba",
                VendorId = "nh2147",
                SpecTypeId_ = 1,
                NuevoGuid = new Guid("03f6295b-03b4-40a6-8058-51f85a608d0b")// "71cfa9df-85bd-41cc-b710-00a41cfa8bca")
                                                                            //  UnitTypeId_ = UnitTypeId.Feet

            };


            return result;
        }

        public static DatosExtStoreDTO ObtnerExtStorePruebaCompplejo()
        {

            var result = new DatosExtStoreDTO()
            {
                Documentation = "hola_complejo",
                ReadAccessLevel = AccessLevel.Public,
                WriteAccessLeve = AccessLevel.Public,
                SchemaName = "prueba_complejo",
                VendorId = "nh2147_compl",
                SpecTypeId_ = 1,
                // nota 'NuevoGuid' no puede coemnzar por 1,2,3 o 4 pq son los utlizado por lo subindices
                NuevoGuid = new Guid("71cfa9df-85bd-41cc-b710-00a41cfa8bca")// "71cfa9df-85bd-41cc-b710-00a41cfa8bca")    
                                                                            //  UnitTypeId_ = UnitTypeId.Feet

            };

            return result;
        }

        #endregion

        //2
        public static DatosExtStoreDTO ObtnerPosicionTagLosa()
        {
            var result = new DatosExtStoreDTO()
            {
                Documentation = "Posicion",
                ReadAccessLevel = AccessLevel.Public,
                WriteAccessLeve = AccessLevel.Public,
                SchemaName = "PosicionTAg14042022",
                VendorId = "nh14042022",

                SpecTypeId_ = 1,
                NuevoGuid = new Guid("ee6c68cf-5e19-4440-a422-682fe007460d")
                // = UnitTypeId.Feet

            };


            return result;
        }
        public static DatosExtStoreDTO ObtnerEstadoMOverTAgdePAth()
        {
            var result = new DatosExtStoreDTO()
            {
                Documentation = "TipoMove",
                ReadAccessLevel = AccessLevel.Public,
                WriteAccessLeve = AccessLevel.Public,
                SchemaName = "TipoMove1442022",
                VendorId = "nh14042022",

                SpecTypeId_ = 1,
                NuevoGuid = new Guid("8503e7ae-a9b8-4b67-9642-16925d88dad9")
                // UnitTypeId_ = UnitTypeId.Feet

            };


            return result;
        }


        public static DatosExtStoreDTO ObtnerCreacionOpening()
        {
            var result = new DatosExtStoreDTO()
            {
                Documentation = "Datos Creacion de Opening con comentarios",
                ReadAccessLevel = AccessLevel.Public,
                WriteAccessLeve = AccessLevel.Public,
                SchemaName = "CreacionOpening07022023",
                VendorId = "nh07022023",
                SpecTypeId_ = 1,
                NuevoGuid = new Guid("04bb2492-1346-47b2-968c-91b019d07da7"),
            };


            return result;
        }
    }
}
