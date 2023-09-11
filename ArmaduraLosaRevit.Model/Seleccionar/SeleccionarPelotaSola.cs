using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.Diagnostics;
using System.Collections;
using ArmaduraLosaRevit.Model.Seleccionar;
//using planta_aux_C.Elemento_Losa;

namespace ArmaduraLosaRevit.Model.Seleccionar
{

    //busca las Pelota de losa de una view (Generic Annotations - BuiltInCategory.OST_GenericAnnotation)
    //y crea un objecto con:
    // PelotaLosa = item,
    //Numero = numero, 
    //Espesor = espesor 
    //PointUbicacion= pointUbicacion 
    //

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SeleccionarPelotaSola : IExternalCommand
    {
        //static string _level_name = "PLANTA ESTRUCTURA CIELO 5° PISO"; // "02 - Floor"

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            SeleccionAnotationPelotaLosa _seleccionAnotationPelotaLosa = new SeleccionAnotationPelotaLosa();
            // Level 2 example criteria
            //ElementId levelId = Util.FindLevelId(doc, _level_name);

            //View actual
            Autodesk.Revit.DB.View view = doc.ActiveView;

            
            // busca el nivel del pisos analizado
            ElementId levelId = Util.FindLevelId(doc, view.GenLevel.Name);

            // crea lista 'ListaAnnotation'  con los objeros 'AnnotationGeneralPelotaLosa' con los datos del pelota de losa, en el view analisado
            var ListaAnnotation = _seleccionAnotationPelotaLosa.GetAllAnotationPelotaLosaFromLevel(uidoc, levelId, BuiltInCategory.OST_GenericAnnotation);




            // borra pelota de losa
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");
            transaction.Start();
            //delete all the unpinned dimensions
            foreach (var annot in ListaAnnotation)
            {
                if (annot.Isborrar == true)
                {
                    commandData.Application.ActiveUIDocument.Document.Delete(annot.PelotaLosa.Id);
                }
            }

            transaction.Commit();

            return Result.Succeeded;
        }





      
    }

}
