using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore.Ayuda;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.Prueba
{

    // clase para asignar una datos aun muro seleccinandolo
    public class ManejadotExtStore_Wall
    {
        private readonly UIApplication _uiapp;
        private readonly UIDocument _uidoc;
        private readonly Document _doc;

        public ManejadotExtStore_Wall(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public void AgregarExtStore()
        {
            try
            {
                Element elementoMuro = null;
                //  ISelectionFilter filtroMuro = new FiltroMuro();
                ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                //ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(IndependentTag));
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar Cara de Muro:");
                XYZ _ptoSeleccionMouseCentroCaraMuro = ref_pickobject_element.GlobalPoint;
                elementoMuro = _doc.GetElement(ref_pickobject_element);





                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                //{
                    DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerExtStorePrueba();

                    CreadorExtStore _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);
                    //SET
                    _CreadorExtStore.SET_DataInElement_XYZConTrans(elementoMuro, _ptoSeleccionMouseCentroCaraMuro);
                    //GET
                    _CreadorExtStore.GET_DataInElement_XYZ_SinTrans(elementoMuro, _CreadorExtStoreDTO.SchemaName);

                //}
                //else
                //{
                //    DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerExtStorePrueba();

                //    CreadorExtStore_2020Abajo _CreadorExtStore = new CreadorExtStore_2020Abajo(_uiapp, _CreadorExtStoreDTO);
                //    //SET
                //    _CreadorExtStore.SET_DataInElement_XYZConTrans(elementoMuro, _ptoSeleccionMouseCentroCaraMuro);
                //    //GET
                //    _CreadorExtStore.GET_DataInElement_XYZ_SinTrans(elementoMuro, _CreadorExtStoreDTO.SchemaName);

                //}


                // Rescatar();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }


        //************
        public void AgregarExtStore_multiples()
        {
            try
            {
                Element elementoMuro = null;
                //  ISelectionFilter filtroMuro = new FiltroMuro();
                //ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall));
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar Cara de Muro:");
                XYZ _ptoSeleccionMouseCentroCaraMuro = ref_pickobject_element.GlobalPoint;
                elementoMuro = _doc.GetElement(ref_pickobject_element);
                double area = 20.0;

                DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerExtStorePruebaCompplejo();
                CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                _CreadorExtStore.M1_SET_DataInElement_XYZConTrans(elementoMuro, _ptoSeleccionMouseCentroCaraMuro, EstadoPasada.Validado, area, "caso prueba");


                //d)
                var entityinsertar = _CreadorExtStore.M3_OBtenerResultado_Entity(elementoMuro, "insertar");
                if (entityinsertar != null)
                {
                    //var coment = entityinsertar.Get<XYZ>("SubFieldTest11", UnitTypeId.Feet);
                    XYZ PtoInsercion = XYZ.Zero;// AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(_uiapp, entityinsertar, "SubFieldTest11");
                    if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                        PtoInsercion = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(_uiapp, entityinsertar, "SubFieldTest11");
                    else
                        PtoInsercion = AyudaCasosUnidades_2020Bajo.Obtener_DUT_DECIMAL_FEET_(_uiapp, entityinsertar, "SubFieldTest11");
                }

                //b)
                var entityEstado = _CreadorExtStore.M3_OBtenerResultado_Entity(elementoMuro, "estado");
                if (entityEstado != null)
                {
                    var EstadoPasada1 = entityEstado.Get<string>("SubFieldTest21");
                    var EstadoPasada = _CreadorExtStore.M3_OBtenerResultado_String(elementoMuro, "estado", "SubFieldTest21");//  entityEstado.Get<string>("SubFieldTest21");
                }


                //c)
                var entityArea = _CreadorExtStore.M3_OBtenerResultado_Entity(elementoMuro, "area");
                if (entityArea != null)
                {
                    //var Area1 = entityEstado.Get<double>("SubFieldTest31", UnitTypeId.Feet);
                    double Area = 0;/// AyudaCasosUnidades_2021Arriba.Obtener_DUT_Numero_FEET(_uiapp, entityArea, "SubFieldTest31");
                    if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                        Area = AyudaCasosUnidades_2021Arriba.Obtener_DUT_Numero_FEET(_uiapp, entityArea, "SubFieldTest31");
                    else
                        Area = AyudaCasosUnidades_2020Bajo.Obtener_DUT_NUMERO_FEET_(_uiapp, entityArea, "SubFieldTest31");
                }

                //d)
                var entitycomentario = _CreadorExtStore.M3_OBtenerResultado_Entity(elementoMuro, "comentario");
                if (entitycomentario != null)
                {
                    var coment1 = entitycomentario.Get<string>("SubFieldTest41");
                    var coment = _CreadorExtStore.M3_OBtenerResultado_String(elementoMuro, "comentario", "SubFieldTest41");

                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

        //***

        public void UpdateExtStore_multiples()
        {
            try
            {
                ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall));
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar Cara de Muro:");
                XYZ _ptoSeleccionMouseCentroCaraMuro = ref_pickobject_element.GlobalPoint;
                Element elementoMuro = _doc.GetElement(ref_pickobject_element);

                DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerExtStorePrueba();
                CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                //GET
                _CreadorExtStore.M2_Update_DataInElement_XYZConTrans(elementoMuro, "acepatado", "caso prueba");



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }
    }
}
