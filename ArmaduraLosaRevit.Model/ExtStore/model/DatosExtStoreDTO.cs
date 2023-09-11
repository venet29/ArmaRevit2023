using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.model
{
    // pára revit 2021 hacia arriba
    public class DatosExtStoreDTO
    {
        public AccessLevel ReadAccessLevel { get; set; }
        public AccessLevel WriteAccessLeve { get; set; }
        public string VendorId { get; set; }
        public string SchemaName { get; set; }
        public int SpecTypeId_ { get; set; }
      //  public ForgeTypeId UnitTypeId_ { get; set; } // versiones nueva 2021 hacia rriba 
       // public UnitType UnitTypeIdAld_ { get; set; } //2020 hacia abjo
        public string Documentation { get; set; }
        public Guid NuevoGuid { get; internal set; }
        public Guid NuevoSetSubSchemaGUID { get; internal set; }
    }
}
