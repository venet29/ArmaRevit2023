using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EditarPath.Calculos
{
    public class EditarPathReinSuperior : EditarPathReinInferior
    {

        public EditarPathReinSuperior(UIApplication  uiapp, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto  ) : base(uiapp,  _seleccionarPathReinfomentConPto  )
        {

        }



        public void M1_moverSuperior(double _desplazamientoFoot)
        {


            ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, _seleccionarPathReinfomentConPto.PathReinforcement, _seleccionarPathReinfomentConPto._tipobarra);
            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("AlargarSuperiorGroup-NH");
                    M1_moverAbajo(_desplazamientoFoot);
                    M2_MoverSUperior(-_deltaVectorDesplazamiento);
                    if (newITagF != null) newITagF.Ejecutar();
                    //EjecutarLetraPararametroCambia();
                    transGroup.Assimilate();
                }
            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($"Error al desplazar path superior ex:{ex.Message}");
            }
        }

        private void M2_MoverSUperior(XYZ _deltaVectorDesplazamiento)
        {
            ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, _seleccionarPathReinfomentConPto.PathReinforcement, _seleccionarPathReinfomentConPto._tipobarra);
            try
            {
                using (Transaction tr = new Transaction(_doc, "AlargarSUperior"))
                {
                    tr.Start();
                    //desplazar path
                    _pathReinforcement.Location.Move(_deltaVectorDesplazamiento);
                    moverPathsimbol();
                    if(newITagF!=null) newITagF.Ejecutar();
                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg($"Error al desplazar path superior ex:{ex.Message }");
            }
        }
    }
}
