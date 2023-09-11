using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.RebarLosa
{
    public class ManejadorRebarBase
    {
        protected  UIApplication _uiapp;
        protected Document _doc;
        protected View _view;
        protected List<IRebarLosa> _ListIRebarLosa;

        public ManejadorRebarBase(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._ListIRebarLosa = new List<IRebarLosa>();
        }

        protected bool GenerarBarra_Contras(RebarInferiorDTO rebarInferiorDTO1)
        {
            try
            {
                //3)tag
                IGeometriaTag _newIGeometriaTag = FactoryGeomTag.CrearGeometriaTag_casoRebar(_uiapp, rebarInferiorDTO1);
                if (_newIGeometriaTag == null) return false;
                
                //4)barra
                IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                _ListIRebarLosa.Add(rebarLosa);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        protected void GenerarBarras_ConTras_SegundaPArte()
        {
            using (TransactionGroup t = new TransactionGroup(_doc))
            {
                t.Start("CrearBarraRebar-NH");
                foreach (var rebarLosa in _ListIRebarLosa)
                {
                    rebarLosa.M2A_GenerarBarra();
                }
                t.Assimilate();
            }
        }

        protected bool GenerarBarra_Sintrasn(RebarInferiorDTO rebarInferiorDTO1)
        {
            //3)tag
            IGeometriaTag _newIGeometriaTag = FactoryGeomTag.CrearGeometriaTag_casoRebar(_uiapp, rebarInferiorDTO1);
            if (_newIGeometriaTag == null) return false;

            //4)barra
            IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
            if (!rebarLosa.M1A_IsTodoOK()) return false;

            rebarLosa.M2A_GenerarBarra();

            return true;
        }
    }
}
