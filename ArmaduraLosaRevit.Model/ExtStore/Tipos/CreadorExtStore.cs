using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore.Ayuda;
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

namespace ArmaduraLosaRevit.Model.ExtStore.Tipos
{

    //https://thebuildingcoder.typepad.com/blog/2013/05/effortless-extensible-storage.html
    //https://forums.autodesk.com/t5/revit-api-forum/problems-trying-to-save-dictionary-lt-string-double-gt-to/td-p/10126690
    public class CreadorExtStore //para versiones  2021 hacia rriba
    {
        private readonly UIApplication _uiapp;
        private readonly DatosExtStoreDTO _CreadorExtStoreDTO;
        private string _version;
        private Guid _guid;
        private Schema schema;

        public CreadorExtStore(UIApplication uiapp, DatosExtStoreDTO _CreadorExtStoreDTO)
        {
            this._uiapp = uiapp;
            this._CreadorExtStoreDTO = _CreadorExtStoreDTO;
            this._version = uiapp.Application.VersionNumber;
            this._guid = _CreadorExtStoreDTO.NuevoGuid;
        }

        public XYZ retrievedData { get; private set; }



        public bool SET_DataInElement_XYZConTrans(Element elemento, XYZ PTO)
        {
            try
            {
                using (Transaction t = new Transaction(elemento.Document))
                {
                    t.Start("tCreateAndStore");

                    SET_DataInElement_XYZ_SInTrans(elemento, PTO);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }




        public bool SET_DataInElement_XYZ_SInTrans(Element elemento, XYZ PTO)
        {
            try
            {
                //busca esquema
                schema = Schema.Lookup(_guid);
                AyudaCasosUnidades_2021Arriba._uiapp = _uiapp;
                //si no esta lo crea
                if (schema == null)
                {
                    if (!CrearSchema())
                    {
                        Util.ErrorMsg("Error al crear Schema para 'Extensible Storage'");
                        return false;
                    }
                }

                //obtener campo
                Field fieldSpliceLocation = schema.GetField(_CreadorExtStoreDTO.SchemaName); // get the field from the schema

                Entity entity = new Entity(schema); // create an entity (object) for this schema (class)  set the value for this entity entity.Set<XYZ>(fieldSpliceLocation, PTO, UnitTypeId.Feet);

                
              //  entity = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(_uiapp, entity, fieldSpliceLocation, PTO);
                if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                    entity = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(_uiapp, entity, fieldSpliceLocation, PTO);
                else
                    entity = AyudaCasosUnidades_2020Bajo.Asignar_DUT_DECIMAL_FEET(_uiapp, entity, fieldSpliceLocation, PTO);

                elemento.SetEntity(entity);

                //Entity retrievedEntity = elemento.GetEntity(schema);
                //XYZ retrievedData = retrievedEntity.Get<XYZ>(schema.GetField(_CreadorExtStoreDTO.SchemaName), UnitTypeId.Feet);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool CrearSchema()
        {
            try
            {
                SchemaBuilder schemaBuilder = new SchemaBuilder(_guid);
                schemaBuilder.SetReadAccessLevel(_CreadorExtStoreDTO.ReadAccessLevel); // allow anyone to read the object
                schemaBuilder.SetWriteAccessLevel(_CreadorExtStoreDTO.WriteAccessLeve); // restrict writing to this vendor only
                schemaBuilder.SetVendorId(_CreadorExtStoreDTO.VendorId); // required because of restricted write-access
                schemaBuilder.SetSchemaName(_CreadorExtStoreDTO.SchemaName);

                // create a field to store an XYZ
                FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField(_CreadorExtStoreDTO.SchemaName, typeof(XYZ));
                //fieldBuilder.SetSpec(SpecTypeId.Length);
            
                //fieldBuilder = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(_uiapp, fieldBuilder);
                if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                    fieldBuilder = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(_uiapp, fieldBuilder);
                else
                    fieldBuilder = AyudaCasosUnidades_2020Bajo.Obtener_UT_Length(_uiapp, fieldBuilder);

                fieldBuilder.SetDocumentation(_CreadorExtStoreDTO.Documentation);


                schema = schemaBuilder.Finish(); // register the Schema object
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        internal bool GET_DataInElement_XYZ_SinTrans(Element elementoMuro, string _SchemaName)
        {
            try
            {
                Schema schema = Schema.Lookup(_guid);
                if (schema == null) return false;
                Entity retrievedEntity = elementoMuro.GetEntity(schema);
                if (retrievedEntity == null) return false;
                retrievedData = XYZ.Zero;
                // retrievedData = retrievedEntity.Get<XYZ>(schema.GetField(_SchemaName), UnitTypeId.Feet);

               // retrievedData = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(retrievedEntity, _SchemaName);
                if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                    retrievedData = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(_uiapp, retrievedEntity, _SchemaName);
                else
                    retrievedData = AyudaCasosUnidades_2020Bajo.Obtener_DUT_DECIMAL_FEET_(_uiapp, retrievedEntity, _SchemaName);

            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


    }
}
