using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RevisarPerformanceAdviserRuleId
{
    public class RevisarNH
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public RevisarNH(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public void EJecutar()
        {
            StringBuilder sbuilder = new StringBuilder();//PERMITE ALMACER LISTAS DE TEXTOS
            sbuilder.AppendLine("Revisar:");


            //Get the name of each registered PerformanceRule and then execute all of them.
            foreach (PerformanceAdviserRuleId id in PerformanceAdviser.GetPerformanceAdviser().GetAllRuleIds())
            {
                string ruleName = PerformanceAdviser.GetPerformanceAdviser().GetRuleName(id);
                IList<int> listID = new List<int>();
                // listID.Add(id.Guid);
                // PerformanceAdviser.GetPerformanceAdviser().ExecuteRules()

                sbuilder.AppendLine(ruleName);
            }
            var list = PerformanceAdviser.GetPerformanceAdviser().ExecuteAllRules(_doc).ToList();

            foreach (var item in list)
            {
                string text = $" {item.GetDescriptionText()}    {item.GetSeverity()}    ";
                sbuilder.AppendLine(text);
            }





            sbuilder.AppendLine("***********************************************************");
            TaskDialog.Show("Datos", sbuilder.ToString());
        }
    }
}
