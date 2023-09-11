using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearTipoDimension
    {
        // ExternalCommandData commandData;
        UIDocument uidoc;
        Document _doc;
        private UIApplication _uipp;
        private string nombreFamilyaRef;
        private string nameTipoTexto;


        public CrearTipoDimension(UIApplication uipp, string nameTipoTexto)
        {
            uidoc = uipp.ActiveUIDocument;
            _doc = uipp.ActiveUIDocument.Document;

            //obtienen el tipo texto
            this.nameTipoTexto = nameTipoTexto;//"DimensionRebar"
            this._uipp = uipp;
            this.nombreFamilyaRef = "Arrow Filled - 2.5mm Arial";
        }



        /// <summary>
        /// obtiene el tipo de texto segun el nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        /// 


        //obs1)
        public bool CrearTipoDimensionConTrasn()
        {
            try
            {
                nombreFamilyaRef=  "MRA Witness Tick Mark Arial";
               DimensionType _dimensionTypedefault  = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, nombreFamilyaRef);

                if (_dimensionTypedefault == null)
                {
                    Util.ErrorMsg($"Error al crear Dimension. No se encontro familia '{nombreFamilyaRef}' de referiencia");
                    return false;
                }

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");


                    Element newElem = _dimensionTypedefault.Duplicate(nameTipoTexto);

                    DimensionType newDimensionType = newElem as DimensionType;

                    if (null != newDimensionType)
                    {

                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_TICK_MARK).Set(new ElementId(-1));

                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public bool CrearTipoDimensionTraslpoConTrasn()
        {
            try
            {
                nombreFamilyaRef = "MRA Witness Tick Mark Arial";
                DimensionType _dimensionTypedefault = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, nombreFamilyaRef);

                if (_dimensionTypedefault == null)
                {
                    Util.ErrorMsg($"Error al crear Dimension. No se encontro familia '{nombreFamilyaRef}' de referiencia");
                    return false;
                }
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear AgregarTraslapo-NH");

                    Element newElem = _dimensionTypedefault.Duplicate(nameTipoTexto);
                    DimensionType newDimensionType = newElem as DimensionType;

                    if (null != newDimensionType)
                    {
                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_TICK_MARK).Set(new ElementId(-1));
                        newDimensionType.get_Parameter(BuiltInParameter.DIM_LEADER_ARROWHEAD).Set(new ElementId(-1));
                        newDimensionType.get_Parameter(BuiltInParameter.LINE_PEN).Set(6);
                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_EXTENSION).Set(0);
                        newDimensionType.get_Parameter(BuiltInParameter.WITNS_LINE_GAP_TO_ELT).Set(0);
                        newDimensionType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(0);
                        newDimensionType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(255);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool CrearTipoDimensionConTrasn_concirculo(string nuvoNombre)
        {
            try
            {
                DimensionType _dimensionTypedefault = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, nombreFamilyaRef);

                if (_dimensionTypedefault == null)
                {
                    Util.ErrorMsg($"Error al crear Dimension. No se encontro familia '{nombreFamilyaRef}' de referiencia");
                    return false;
                }

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");


                    Element newElem = _dimensionTypedefault.Duplicate(nameTipoTexto);

                    DimensionType newDimensionType = newElem as DimensionType;

                    if (null != newDimensionType)
                    {

                        newDimensionType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(0);

                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }


    }
}
