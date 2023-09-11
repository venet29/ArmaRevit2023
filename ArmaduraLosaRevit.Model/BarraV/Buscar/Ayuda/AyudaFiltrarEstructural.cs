using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda
{
    class AyudaFiltrarEstructural
    {

        public static bool SiEsWalloLosaEstructural(ReferenceWithContext cc, Document _doc)
        {
            try
            {
                if (cc == null) return false;

                Reference WallRef = cc.GetReference();

                var WalloVigaElementHost = _doc.GetElement(WallRef);
                if (WalloVigaElementHost is Wall || WalloVigaElementHost is Floor)
                {
                    if (!WalloVigaElementHost.IsEstructural())
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
