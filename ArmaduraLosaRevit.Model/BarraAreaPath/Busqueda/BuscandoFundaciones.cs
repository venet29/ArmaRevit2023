using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Busqueda
{
    public class BuscandoFundaciones
    {
        private readonly UIApplication _uiapp;
        private readonly View3D _view3D_buscar;
        private readonly int _diametroV_MM;

        public BuscandoFundaciones(UIApplication uiapp, View3D _view3D_buscar, int diametroMM)
        {
            this._uiapp = uiapp;
            this._view3D_buscar = _view3D_buscar;
            this._diametroV_MM = diametroMM;
        }

        public XYZ p1_porFundaciones { get; private set; }

        public bool OBbtenerFundaciones(XYZ p1)
        {
            try
            {
                BuscarFundacionLosa BuscarMuros = new BuscarFundacionLosa(_uiapp, UtilBarras.largo_L9_DesarrolloFoot_diamMM(_diametroV_MM));
                if (BuscarMuros.OBtenerRefrenciaFundacionSegunVector(_view3D_buscar, p1, new XYZ(0, 0, -1)))
                {
                    p1_porFundaciones = p1.AsignarZ(BuscarMuros.ObtenerZmenorDeElemetosEncontrados() + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Write("ex:" + ex.Message);
            }
            return false;
        }
    }
}
