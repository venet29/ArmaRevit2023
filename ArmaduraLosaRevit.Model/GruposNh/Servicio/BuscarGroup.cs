using ArmaduraLosaRevit.Model.GruposNh.Model;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Armadura
{


    public class BuscarGroup
    {
        public static List<MOdelGroupNH> LIStaMOdelGroupNH { get; set; }
        public static List<MOdelGroupNH> LIStaDetailGroupsNH { get; set; }
        public static List<MOdelGroupNH> ObtenerGroupModel(Document _doc)
        {
            if (LIStaMOdelGroupNH == null)
                LIStaMOdelGroupNH = new List<MOdelGroupNH>();




            FilteredElementCollector groupCollector = new FilteredElementCollector(_doc).OfClass(typeof(GroupType));
            foreach (GroupType groupType in groupCollector)
            {
                if (groupType.FamilyName != "Model Group") continue;

                //Retrieve a set of all the groups that have this type.
                foreach (Group group in groupType.Groups)
                {
                    if (group == null) continue;
                    var newGrupo = new MOdelGroupNH(group);
                    newGrupo.Filtrar();
                    LIStaMOdelGroupNH.Add(newGrupo); ;

                    /*
                    // Get GroupType group name
                    message = "\nThe group type name is : " + groupType.Name;
                    //Returns all the members of the group.
                    message += "\nThe types and ids of the group members are : ";
                    IList<ElementId> groupMembers = group.GetMemberIds();
                    foreach (ElementId memberId in groupMembers)
                    {
                        Element element = group.Document.GetElement(memberId);
                        // Get GroupType group element id
                        message += "\n\t" + element.GetType().Name + " : " + memberId.IntegerValue;
                    }
                    */
                }
            }

            return LIStaMOdelGroupNH;
        }

        public static List<MOdelGroupNH> ObtenerArrayDetailGroup(Document _doc)
        {
            if (LIStaDetailGroupsNH == null)
                LIStaDetailGroupsNH = new List<MOdelGroupNH>();




            FilteredElementCollector groupCollector = new FilteredElementCollector(_doc).OfClass(typeof(GroupType));
            foreach (GroupType groupType in groupCollector)
            {
                if (groupType.FamilyName != "Detail Group") continue;


                string message = "GroupType";
                //Retrieve a set of all the groups that have this type.
                foreach (Group group in groupType.Groups)
                {
                    if (group == null) continue;
                    var newGrupo = new MOdelGroupNH(group);
                    newGrupo.Filtrar();
                    LIStaMOdelGroupNH.Add(newGrupo); ;

                    /*
                    // Get GroupType group name
                    message = "\nThe group type name is : " + groupType.Name;
                    //Returns all the members of the group.
                    message += "\nThe types and ids of the group members are : ";
                    IList<ElementId> groupMembers = group.GetMemberIds();
                    foreach (ElementId memberId in groupMembers)
                    {
                        Element element = group.Document.GetElement(memberId);
                        // Get GroupType group element id
                        message += "\n\t" + element.GetType().Name + " : " + memberId.IntegerValue;
                    }
                    */
                }
            }

            return LIStaMOdelGroupNH;
        }
    }
}
