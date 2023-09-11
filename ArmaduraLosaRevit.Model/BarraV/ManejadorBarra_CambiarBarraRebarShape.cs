using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra.EditarRebarShape;

namespace ArmaduraLosaRevit.Model.BarraV
{
    public class ManejadorBarra_CambiarBarraRebarShape
    {
        private UIApplication _uiapp;
        private Document _doc;
        private readonly EditarBarraDTO _editarBarraDTO;


        private List<IbarraBase> _listaDebarra;
        private List<CreadorBarrasV> _listaCreadorBarrasV;

        private EditarBarraRebarShape EditarBarraV;

        public ManejadorBarra_CambiarBarraRebarShape(UIApplication uiapp, EditarBarraDTO editarBarraDTO)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._editarBarraDTO = editarBarraDTO;
 
            _listaDebarra = new List<IbarraBase>();

            _listaCreadorBarrasV = new List<CreadorBarrasV>();
        }

        public void CambiarBarraRebarShape()
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                //
                if (!M1_CalculosIniciales()) return;
                //1
                SeleccionarTagRebar seleccionarTagRebar = new SeleccionarTagRebar(_uiapp);

                if (!seleccionarTagRebar.GetSelecionarRebarTag())
                {
                    if (UtilBarras.IsConNotificaciones)
                        Util.ErrorMsg("Error Al Selecciona barra de referencia");
                    else
                        Debug.WriteLine("Error Al Selecciona barra de referencia");
                    return;
                }


                RebarShape rebarshape = TiposFormasRebarShape.getRebarShape("M_NHI", _doc);
      
                EditarBarraV = new EditarBarraRebarShape(_uiapp, rebarshape, seleccionarTagRebar);
                EditarBarraV.ObtenerDatosDebarra();
                EditarBarraV.CambiarRebarShape(rebarshape);


            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }

  
        private bool M1_CalculosIniciales()
        {
            try
            {
        
                View3D _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (_view3D_paraVisualizar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D VISUALIZAR");
                    return false;
                }
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D BUSCAR");
                    return false;
                }
                _editarBarraDTO.view3D_paraBuscar = _view3D;
                _editarBarraDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _editarBarraDTO.viewActual = _doc.ActiveView; ;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }

 



        private void M7_CAmbiarColor(ElementId elemid)
        {
        
            if (elemid == null) return;

            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeElementColorCONtrans(elemid, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));
            //visibilidadElement
        }

    
    }
}
