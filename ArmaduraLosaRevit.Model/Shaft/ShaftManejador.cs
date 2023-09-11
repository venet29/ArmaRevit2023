using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Shaft.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft
{


    public class ShaftManejador
    {
        private UIApplication _UIapp;
        private Document _doc;

        public ShaftManejador(ExternalCommandData commandData)
        {
             _UIapp = commandData.Application;
            _doc = _UIapp.ActiveUIDocument.Document;
        }

        public Result execute()
        {
            //TaskDialog.Show("ERRRO", "SDFSDF");

            bool mientras = true;
            while (mientras)
            {
                mientras = false;
                //1)seleccionana
                SeleccionarOpeningConMouse seleccionarOpeningConMouse = new SeleccionarOpeningConMouse(_UIapp);
                seleccionarOpeningConMouse.M1_SelecconaOpening();
                if (!seleccionarOpeningConMouse.IsOk()) return Result.Succeeded;   //ShaftIndividualNULL

                //2)Obtienen datos del shaft correcto
                ShaftConjunto shaftGeom = new ShaftConjunto(_doc, seleccionarOpeningConMouse, _doc.ActiveView.GenLevel);
                shaftGeom.Ejecutar();

                //3)obtiene objeto que representa shaft --geometria
                ShaftIndividual ShaftIndividuas = shaftGeom.shaftUnicoSeleccoinado;
                if (ShaftIndividuas.IsOk == false)
                {
                    mientras = true;
                    continue;
                }
                if (!ShaftIndividuas.M2_IsMAs2Ptos())
                {
                    mientras = true;
                    continue;
                }

                //4)dibiuja shaft y cruz
                ShaftIndividuas.M3_CrearSeparacionRoom(_UIapp);
                ShaftIndividuas.M4_CrearCruz();
                mientras = true;
            }
            return Result.Succeeded;
        }


    }
}
