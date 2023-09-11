using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.AgregarEspMuro
{
    public class ManejadorAgregarEspesor
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorAgregarEspesor(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public void EjecutarAllView()
        {

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            SeleccionarView _SeleccionarView = new SeleccionarView();
            List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);
            try
            {
                foreach (ViewSection _view in list)
                {
                    _uiapp.ActiveUIDocument.ActiveView = _view;

                    using (Transaction trans2 = new Transaction(_doc))
                    {
                        trans2.Start("ActualizarBarraTipo-NH");


                        CreadorAgregarEspesor _creadorAgregarEspesor = new CreadorAgregarEspesor(_uiapp, _view);
                        _creadorAgregarEspesor.AgregarEsp();

                        trans2.Commit();
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");
        }
      

        public void Ejecutar1View()
        {

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);


            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");
                    CreadorAgregarEspesor _creadorAgregarEspesor = new CreadorAgregarEspesor(_uiapp, (ViewSection)_doc.ActiveView);
                    _creadorAgregarEspesor.AgregarEsp();
                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

        }
    }
}
