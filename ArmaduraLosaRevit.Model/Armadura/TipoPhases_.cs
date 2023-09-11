using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TipoPhases_
    {
        public static ElementId idPhase { get; private set; }
        public static Phase PhasePhase_ { get; private set; }

        public static  bool ObtenerFasesExistenete(Document _doc)
        {
            try
            {

                if (GetPhaseId("Existing", _doc))
                    return true;

                if (GetPhaseId("Existente", _doc))
                    return true;


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Phase  \n ex: {ex.Message} ");
            
            }
            return false;

        }


        //

        public static bool IsFasesExistenete(Element q)
        {
            try
            {
                if (q.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString() == "Existing" || 
                    q.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString() == "Existente")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Phase  \n ex: {ex.Message} ");

            }
            return false;

        }


        public static bool GetPhaseId(string phaseName, Document doc)
        {
            try
            {

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(Phase));
                var phases = from Phase phase in collector where phase.Name.Equals(phaseName) select phase;
                 PhasePhase_ = phases.FirstOrDefault();
                if (PhasePhase_ == null)
                    return false;

                idPhase = PhasePhase_.Id;

                return true;

            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error al buscar 'Phasing' : {phaseName}");
                return false;
            }
        }
    }
}
