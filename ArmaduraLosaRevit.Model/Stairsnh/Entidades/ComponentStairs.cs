using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using System;

using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace ArmaduraLosaRevit.Model.Stairsnh.Entidades
{
    public class ComponentStairs
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private StringBuilder _sbuilder;

        public ComponentStairs(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;

             _sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto
        }

        public Stairs M1_GetAllStairsInfo()
        {
            _sbuilder.Clear();
            _sbuilder.AppendLine($"GetAllStairsInfo----------------------------------------------------------------");
            Stairs stairs = null;

            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            ICollection<ElementId> stairsIds = collector.WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Stairs).ToElementIds();
            int contador = 0;
            foreach (ElementId stairId in stairsIds)
            {
                if (Stairs.IsByComponent(_doc, stairId) == true)
                {
                    stairs = _doc.GetElement(stairId) as Stairs;

                    // Format the information
                    String info = "\nNumber of stories:  " + stairs.NumberOfStories;
                    info += "\nHeight of stairs:  " + stairs.Height;
                    info += "\nNumber of treads:  " + stairs.ActualTreadsNumber;
                    info += "\nTread depth:  " + stairs.ActualTreadDepth;
                    _sbuilder.AppendLine($"{contador}----------------------------------------------------------------");
                    _sbuilder.AppendLine(info);
                    contador += 1;
                    // Show the information to the user.
                    // TaskDialog.Show("Revit", info);
                }
            }

            Util.InfoMsg(_sbuilder.ToString());
            return stairs;
        }
        public void M2_GetStairInfo(Stairs stairs)
        {
            _sbuilder.Clear();
            _sbuilder.AppendLine($"M2_GetStairInfo----------------------------------------------------------------");
            int contador = 0;
            if (Stairs.IsByComponent(_doc, stairs.Id) == true)
                {
                    

                    // Format the information
                    String info = "\nNumber of stories:  " + stairs.NumberOfStories;
                    info += "\nHeight of stairs:  " + stairs.Height;
                    info += "\nNumber of treads:  " + stairs.ActualTreadsNumber;
                    info += "\nTread depth:  " + stairs.ActualTreadDepth;

                _sbuilder.AppendLine(info);
                contador += 1;
                // Show the information to the user.
                //TaskDialog.Show("Revit", info);
                }

            Util.InfoMsg(_sbuilder.ToString());
        }

        public void M3_GetRunType(Stairs stairs)
        {
            ICollection<ElementId> runIds = stairs.GetStairsRuns();
            _sbuilder.Clear();
            _sbuilder.AppendLine($"M3_GetRunType----------------------------------------------------------------");
            int contador = 0;
            foreach (ElementId RunId in runIds)
            {
                StairsRun firstRun = stairs.Document.GetElement(RunId) as StairsRun;
                if (null != firstRun)
                {
                    StairsRunType runType = stairs.Document.GetElement(firstRun.GetTypeId()) as StairsRunType;
                    // Format landing type info for display
                    string info = "Stairs Run Type:  " + runType.Name;
                    info += "\nRiser Thickness:  " + runType.RiserThickness;
                    info += "\nTread Thickness:  " + runType.TreadThickness;
                    _sbuilder.AppendLine($"{contador}----------------------------------------------------------------");
                    _sbuilder.AppendLine(info);
                    contador += 1;
                    //TaskDialog.Show("Revit", info);
                }
            }

            Util.InfoMsg(_sbuilder.ToString());

        }
        public void M4_GetStairsType(Stairs stairs)
        {
            _sbuilder.Clear();
            _sbuilder.AppendLine($"M4_GetStairsType----------------------------------------------------------------");
            StairsType stairsType = stairs.Document.GetElement(stairs.GetTypeId()) as StairsType;

            // Format stairs type info for display
            string info = "Stairs Type:  " + stairsType.Name;
            info += "\nLeft Lateral Offset:  " + stairsType.LeftLateralOffset;
            info += "\nRight Lateral Offset:  " + stairsType.RightLateralOffset;
            info += "\nMax Riser Height:  " + stairsType.MaxRiserHeight;
            info += "\nRight Lateral Offset:  " + stairsType.RightLateralOffset;
            info += "\nLeft Lateral Offset:  " + stairsType.LeftLateralOffset;
            info += "\nMin Run Width:  " + stairsType.MinRunWidth;
    
            _sbuilder.AppendLine(info);
           
           // TaskDialog.Show("Revit", info);

            Util.InfoMsg(_sbuilder.ToString());
        }

        public void M5_AddStartandEndRisers(Stairs stairs)
        {

            ICollection<ElementId> runIds = stairs.GetStairsRuns();

            foreach (ElementId runId in runIds)
            {
                StairsRun run = stairs.Document.GetElement(runId) as StairsRun;
                if (null != run)
                {
                    run.BeginsWithRiser = true;
                    run.EndsWithRiser = true;
                }
            }

          
        }


    }
}
