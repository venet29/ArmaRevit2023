using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar.Trabas.WPF
{
    class Manejador_CambiarTag
    {

        public static bool Ejecutar(UIApplication _uiapp)
        {
            try
            {


                Ui_Elevaciones _FormCub = new Ui_Elevaciones(_uiapp);
                _FormCub.ShowDialog();
             //   //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
                UpdateGeneral.M5_DesCargarGenerar(_uiapp);
                if (_FormCub.casoTipo == "btn_crear1vista")
                {
                    Autodesk.Revit.DB.View _view = _uiapp.ActiveUIDocument.ActiveView;
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.CargarFamiliar();
                    _ManejadorActualizarBarraTipo.Ejecutar_enVistaActual_FOrmatoTraba(_view);
                }
                else if (_FormCub.casoTipo == "btn_todasLASvistas")
                {
                    Document _doc = _uiapp.ActiveUIDocument.Document;
                    SeleccionarView _SeleccionarView = new SeleccionarView();
                    var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);

                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.CargarFamiliar();
                    LimpiandoListas.Limpiar();
       
                    try
                    {
                        for (int i = 0; i < ListaViewSection.Count; i++)
                        {
                            _ManejadorActualizarBarraTipo.Ejecutar_enVistaActual_FOrmatoTraba(ListaViewSection[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.DebugDescripcion(ex);
                    }
                   
                }
                Util.InfoMsg($"Proceso Terminado. ");
                UpdateGeneral.M4_CargarGenerar(_uiapp);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
    }
}
