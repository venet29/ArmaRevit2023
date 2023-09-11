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


    //clase obsote se remplazo con la opcion generrca borrar mas adelante---> 30-01-2023
    //https://thebuildingcoder.typepad.com/blog/2013/05/effortless-extensible-storage.html
    //https://forums.autodesk.com/t5/revit-api-forum/problems-trying-to-save-dictionary-lt-string-double-gt-to/td-p/10126690
    public class CreadorExtStore_2020Abajo // para versioens 2020 hacia abajo
    {
        private readonly UIApplication _uiapp;
        private readonly DatosExtStoreDTO _datosExtStoreDTO;
        private string _version;
        private Guid _guid;
        private Schema _schema;

        public XYZ retrievedData { get; private set; }

        public CreadorExtStore_2020Abajo(UIApplication uiapp, DatosExtStoreDTO _CreadorExtStoreDTO)
        {
            this._uiapp = uiapp;
            this._datosExtStoreDTO = _CreadorExtStoreDTO;
            this._version = uiapp.Application.VersionNumber;
            this._guid = _CreadorExtStoreDTO.NuevoGuid;
        }




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
                _schema = Schema.Lookup(_guid);

                //si no esta lo crea
                if (_schema == null)
                {
                    if (!CrearSchema())
                    {
                        Util.ErrorMsg("Error al crear Schema para 'Extensible Storage'");
                        return false;
                    }
                }

                Field fieldSpliceLocation = _schema.GetField(_datosExtStoreDTO.SchemaName); // get the field from the schema

                Entity entity = new Entity(_schema); // create an entity (object) for this schema (class) set the value for this entity entity.Set<XYZ>(fieldSpliceLocation, PTO, UnitTypeId.Feet);

                entity = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(entity, fieldSpliceLocation, PTO);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    entity = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(entity, fieldSpliceLocation, PTO);
                //else
                //    entity = AyudaCasosUnidades_2020Bajo.Asignar_DUT_DECIMAL_FEET(entity, fieldSpliceLocation, PTO);

        
                elemento.SetEntity(entity);
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool CrearSchema()
        {
            try
            {
                SchemaBuilder schemaBuilder = new SchemaBuilder(_guid);
                schemaBuilder.SetReadAccessLevel(_datosExtStoreDTO.ReadAccessLevel); // allow anyone to read the object
                schemaBuilder.SetWriteAccessLevel(_datosExtStoreDTO.WriteAccessLeve); // restrict writing to this vendor only
                schemaBuilder.SetVendorId(_datosExtStoreDTO.VendorId); // required because of restricted write-access
                schemaBuilder.SetSchemaName(_datosExtStoreDTO.SchemaName);

                // create a field to store an XYZ
                FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField(_datosExtStoreDTO.SchemaName, typeof(XYZ));
                // fieldBuilder.SetUnitType(_datosExtStoreDTOOld_2020Abajo.UnitTypeIdAld_);

                fieldBuilder = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(fieldBuilder);
                
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    fieldBuilder = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(fieldBuilder);
                //else
                //    fieldBuilder = AyudaCasosUnidades_2020Bajo.Obtener_UT_Length(fieldBuilder);

                fieldBuilder.SetDocumentation(_datosExtStoreDTO.Documentation);

                _schema = schemaBuilder.Finish(); // register the Schema objec
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool GET_DataInElement_XYZ_SinTrans(Element elementoMuro, string _SchemaName)
        {
            try
            {
                Schema schema = Schema.Lookup(_guid);
                if (schema == null) return false;
                Entity retrievedEntity = elementoMuro.GetEntity(schema);
                if (retrievedEntity == null) return false;
                retrievedData = XYZ.Zero;
                // retrievedData = retrievedEntity.Get<XYZ>(_SchemaName, DisplayUnitType.DUT_DECIMAL_FEET);

                retrievedData = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(retrievedEntity, _SchemaName);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    retrievedData = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(retrievedEntity,_SchemaName );
                //else
                //    retrievedData = AyudaCasosUnidades_2020Bajo.Obtener_DUT_DECIMAL_FEET_(retrievedEntity,_SchemaName);
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
