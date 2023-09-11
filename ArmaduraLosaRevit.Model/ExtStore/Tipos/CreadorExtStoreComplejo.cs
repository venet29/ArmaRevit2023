using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore.Ayuda;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.Tipos
{

    //https://spiderinnet.typepad.com/blog/2012/02/revit-api-2012-extensible-storage-manage-sub-schemaentity.html
    //https://thebuildingcoder.typepad.com/blog/2013/05/effortless-extensible-storage.html
    //https://forums.autodesk.com/t5/revit-api-forum/problems-trying-to-save-dictionary-lt-string-double-gt-to/td-p/10126690
    public class CreadorExtStoreComplejo // sirve  para todas las versiones  de revit   --> las clases en ArmaduraLosaRevit.Model.ExtStore.Ayuda ayuda con las diversas verisiones
    {
        private UIApplication _uiapp;
        private Document _doc;
        private DatosExtStoreDTO _datosExtStoreDTO;
        private string _version;
        private Guid _guid;
        private Schema schema;
        private Entity ent1;
        private Entity ent2;
        private Entity ent3;
        private Entity ent4;

        public XYZ retrievedData { get; private set; }

        public CreadorExtStoreComplejo(UIApplication uiapp, DatosExtStoreDTO _CreadorExtStoreDTO_)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._datosExtStoreDTO = _CreadorExtStoreDTO_;
            this._version = uiapp.Application.VersionNumber;
            this._guid = _datosExtStoreDTO.NuevoGuid;
        }


        //******** crear
        public bool M1_SET_DataInElement_XYZConTrans(Element elemento, XYZ PTO, EstadoPasada estado ,double area ,string comentario)
        {
            try
            {
                using (Transaction t = new Transaction(elemento.Document))
                {
                    t.Start("tCreateAndStore");
                    M1_SET_DataInElement_XYZ_SInTrans(elemento, PTO, estado,area,  comentario);
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


        public bool M1_SET_DataInElement_XYZ_SInTrans(Element elemento, XYZ PTO, EstadoPasada estado, double area,string comentario)
        {
            try
            {
                schema = Schema.Lookup(_guid);
                //si no esta lo crea
                if (schema == null)
                {
                    if (!M1_1_CrearSchema())
                    {
                        Util.ErrorMsg("Error al crear Schema para 'Extensible Storage'");
                        return false;
                    }
                }
                else if (!M1_2_ObtenerSubEntidades())
                {
                    Util.ErrorMsg("Error al obtener subentidades 'Extensible Storage'");
                    return false;
                }
                ent1 = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(ent1, "SubFieldTest11", PTO);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    ent1 = AyudaCasosUnidades_2021Arriba.Asignar_DUT_DECIMAL_FEET(ent1, "SubFieldTest11", PTO);
                //else
                //    ent1 = AyudaCasosUnidades_2020Bajo.Asignar_DUT_DECIMAL_FEET(ent1, "SubFieldTest11", PTO);

                //ent1.Set<XYZ>("SubFieldTest11", PTO, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
                ent2.Set<string>("SubFieldTest21", estado.ToString());

                ent3 = AyudaCasosUnidades_2021Arriba.Asignar_DUT_Numero_FEET(ent3, "SubFieldTest31", area);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    ent3 = AyudaCasosUnidades_2021Arriba.Asignar_DUT_Numero_FEET(ent3, "SubFieldTest31", area);
                //else
                //    ent3 = AyudaCasosUnidades_2020Bajo.Asignar_DUT_NUMERO_FEET(ent3, "SubFieldTest31", area);


                ent4.Set<string>("SubFieldTest41", comentario.ToString());

                Entity ent = new Entity(schema);
                ent.Set<Entity>("insertar", ent1);
                ent.Set<Entity>("estado", ent2);
                ent.Set<Entity>("area", ent3);
                ent.Set<Entity>("comentario", ent4);

                _doc.GetElement(elemento.Id).SetEntity(ent);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M1_1_CrearSchema()
        {
            try
            {
                string numeroGuid = _guid.ToString();
                string _guidFile1 = "1" + numeroGuid.Substring(1);
                string _guidFile2 = "2" + numeroGuid.Substring(1);
                string _guidFile3 = "3" + numeroGuid.Substring(1);
                string _guidFile4 = "4" + numeroGuid.Substring(1);

                //a) entidad uno
                SchemaBuilder sb1 = new SchemaBuilder(new Guid(_guidFile1));
                sb1.SetSchemaName("SubSchemaTest1");
                FieldBuilder fb11 = sb1.AddSimpleField("SubFieldTest11", typeof(XYZ));

                fb11 = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(fb11);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    fb11 = AyudaCasosUnidades_2021Arriba.Obtener_UT_Length(fb11);
                //else
                //    fb11 = AyudaCasosUnidades_2020Bajo.Obtener_UT_Length(fb11);
                Schema schemaSubCampo1 = sb1.Finish();
                ent1 = new Entity(schemaSubCampo1);


                //b) entidad dos
                SchemaBuilder sb2 = new SchemaBuilder(new Guid(_guidFile2));
                sb2.SetSchemaName("SubSchemaTest2");
                FieldBuilder fb21 = sb2.AddSimpleField("SubFieldTest21", typeof(string));
                Schema schemaSubCampo2 = sb2.Finish();
                ent2 = new Entity(schemaSubCampo2);

                //c) entidad tre
                SchemaBuilder sb3 = new SchemaBuilder(new Guid(_guidFile3));
                sb3.SetSchemaName("SubSchemaTest3");
                FieldBuilder fb31 = sb3.AddSimpleField("SubFieldTest31", typeof(double));

                fb31 = AyudaCasosUnidades_2021Arriba.Obtener_UT_Numero(fb31);
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    fb31 = AyudaCasosUnidades_2021Arriba.Obtener_UT_Numero(fb31);
                //else
                //    fb31 = AyudaCasosUnidades_2020Bajo.Obtener_UT_Numero(fb31);
                Schema schemaSubCampo3 = sb3.Finish();
                ent3 = new Entity(schemaSubCampo3);


                //d) entidad dos
                SchemaBuilder sb4 = new SchemaBuilder(new Guid(_guidFile4));
                sb4.SetSchemaName("SubSchemaTest4");
                FieldBuilder fb41 = sb4.AddSimpleField("SubFieldTest41", typeof(string));
                Schema schemaSubCampo4 = sb4.Finish();
                ent4 = new Entity(schemaSubCampo4);


                //****creando esquema contenedor
                SchemaBuilder SchemaBAse = new SchemaBuilder(_guid);
                SchemaBAse.SetSchemaName("SchemaTest1");

                FieldBuilder fb1 = SchemaBAse.AddSimpleField("insertar", typeof(Entity));
                fb1.SetSubSchemaGUID(new Guid(_guidFile1));
                FieldBuilder fb2 = SchemaBAse.AddSimpleField("estado", typeof(Entity));
                fb2.SetSubSchemaGUID(new Guid(_guidFile2));

                FieldBuilder fb3 = SchemaBAse.AddSimpleField("area", typeof(Entity));
                fb3.SetSubSchemaGUID(new Guid(_guidFile3));

                FieldBuilder fb4 = SchemaBAse.AddSimpleField("comentario", typeof(Entity));
                fb4.SetSubSchemaGUID(new Guid(_guidFile4));

                schema = SchemaBAse.Finish();
                //*****************
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M1_2_ObtenerSubEntidades()
        {
            try
            {
                string numeroGuid = _guid.ToString();
                string _guidFile1 = "1" + numeroGuid.Substring(1);
                string _guidFile2 = "2" + numeroGuid.Substring(1);
                string _guidFile3 = "3" + numeroGuid.Substring(1);
                string _guidFile4 = "4" + numeroGuid.Substring(1);

                Schema schema1 = Schema.Lookup(new Guid(_guidFile1));
                ent1 = new Entity(schema1);

                Schema schema2 = Schema.Lookup(new Guid(_guidFile2));
                ent2 = new Entity(schema2);

                Schema schema3 = Schema.Lookup(new Guid(_guidFile3));
                ent3 = new Entity(schema3);

                Schema schema4 = Schema.Lookup(new Guid(_guidFile4));
                ent4 = new Entity(schema4);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        //********update
        public bool M2_Update_DataInElement_XYZConTrans(Element elemento, string estado, string comentario)
        {
            try
            {
                using (Transaction t = new Transaction(elemento.Document))
                {
                    t.Start("tCreateAndStore");
                    M2_Update_DataInElement_XYZ_SinTrans(elemento, estado, comentario);
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

        public bool M2_Update_DataInElement_XYZ_SinTrans(Element element, string _estado,string _comentario)
        {
            try
            {
                string numeroGuid = _guid.ToString();
                Schema schemaCentral = Schema.Lookup(_guid);
                Entity entCentral = element.GetEntity(schemaCentral);

                //cambiar estado
                Entity entEstado = entCentral.Get<Entity>("estado");       
                if (entEstado != null)
                {
                   // var result =entEstado.Get<string>("SubFieldTest21");
                    entEstado.Set<string>("SubFieldTest21", _estado);
                    entCentral.Set<Entity>("estado", entEstado);
                    element.SetEntity(entCentral);
                }

                //cambiar comentario
                Entity comentarios = entCentral.Get<Entity>("comentario");
                if (comentarios != null)
                {
                    // var result =entEstado.Get<string>("SubFieldTest21");
                    comentarios.Set<string>("SubFieldTest41", _comentario);
                    entCentral.Set<Entity>("comentario", comentarios);
                    element.SetEntity(entCentral);
                }
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public string M3_OBtenerResultado_String(Element element, string nombrePArametro,string nombrecampo)
        {
            try
            {
                string numeroGuid = _guid.ToString();
                Schema schemaCentral = Schema.Lookup(_guid);
                if (schemaCentral == null || element==null) return "";
                Entity entCentral = element.GetEntity(schemaCentral);
                Entity entEstado = entCentral.Get<Entity>(nombrePArametro);

                if (entEstado != null)
                {
                    return entEstado.Get<string>(nombrecampo);
                }
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);
                return "";
            }
            return "";
        }

        public Entity M3_OBtenerResultado_Entity(Element element, string nombrePArametro)
        {
            try
            {
                string numeroGuid = _guid.ToString();
                Schema schemaCentral = Schema.Lookup(_guid);
                if (schemaCentral == null) return null;
                Entity entCentral = element.GetEntity(schemaCentral);
                Entity entEstado = entCentral.Get<Entity>(nombrePArametro);

                if (entEstado != null) return entEstado;
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);
                return null;
            }
            return null;
        }
    }
}
