using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagLateralesViga : GeomeTagEstriboBase, IGeometriaTag
    {
    //    private readonly EstriboMuroDTO _item;

        public GeomeTagLateralesViga(Document doc, XYZ posiciontag, string nombreDefamiliaBase) : base(doc, posiciontag, nombreDefamiliaBase)
        {

        }




        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagLateralesViga  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagLateralesViga> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {
            _geomeTagBase.TagP0_Estribo.IsOk = false;
            _geomeTagBase.TagP0_Traba.IsOk = false;
            _geomeTagBase.TagP0_Espesor.IsOk = false;
        }


    }
}
