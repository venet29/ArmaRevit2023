using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GruposNh.Model
{
    public class MOdelGroupNH
    {
        private MOdelGroupNH_JSON c;

        public string Nombre { get; set; }
        public IList<ElementId> ListaGruopId { get; set; }
        public int IdGroup { get; private set; }
        public Group GroupNH { get; }
        public GroupType GroupTipo { get; }

        public MOdelGroupNH(Group group)//string name, IList<ElementId> elementIds, int groupid)
        {
            this.Nombre = group.Name;
            this.ListaGruopId = group.GetMemberIds();
            this.IdGroup = group.Id.IntegerValue;
            GroupNH = group;
            GroupTipo = group.GroupType;
        }

        public MOdelGroupNH(Document _doc,MOdelGroupNH_JSON c)
        {
            this.c = c;
            this.Nombre = c.Nombre;
            this.ListaGruopId = c.ListaGruopId.Select(r=> new ElementId(r)).ToList();
            this.IdGroup = c.IdGroup;
            //GroupNH = (Group)_doc.GetElement(new ElementId(c.GroupNH));
            GroupTipo=(GroupType)_doc.GetElement(new ElementId(c.GroupTipo));
        }

        public bool Filtrar()
        {
            try
            {

                ListaGruopId.Clear();
                // Get GroupType group name
                string message = "\nThe group type name is : " + GroupTipo.Name;
                //Returns all the members of the group.
                message += "\nThe types and ids of the group members are : ";
                IList<ElementId> groupMembers = GroupNH.GetMemberIds();
                IList<Element> ListaGroupELeme = GroupNH.GetMemberIds().Select(r=> GroupNH.Document.GetElement(r)).ToList();
                foreach (ElementId memberId in groupMembers)
                {
                    Element element = GroupNH.Document.GetElement(memberId);
                    if (!((element is Room) || (element.GetType().Name == "Line")))
                    {
                        ListaGruopId.Add(memberId);
                    }
                    else
                    { 
                    
                    }
                    // Get GroupType group element id
                    message += "\n\t" + element.GetType().Name + " : " + memberId.IntegerValue;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Filtrar'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public MOdelGroupNH_JSON ObtenerJson()
        {
            try
            {
                MOdelGroupNH_JSON _newJson = new MOdelGroupNH_JSON()
                {
                    Nombre = Nombre,
                    ListaGruopId = ListaGruopId.Select(c => c.IntegerValue).ToList(),
                    IdGroup = IdGroup,
                    GroupNH = GroupNH.Id.IntegerValue,
                    GroupTipo = GroupTipo.Id.IntegerValue

                };
                return _newJson;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'MOdelGroupNH-ObtenerJson'. ex:{ex.Message}");
                return null;
            }

        }


    }
}
