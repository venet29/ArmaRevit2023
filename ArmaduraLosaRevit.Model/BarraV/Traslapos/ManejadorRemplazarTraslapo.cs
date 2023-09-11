using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos
{
    public class ManejadorRemplazarTraslapo
    {
        private readonly UIApplication uiapp;
        private Document _doc;
        private View _view;


        public List<TraslapoDTO> ListaTraslapo { get; set; }
        public TraslapoDTO traslapoENcontrado { get; private set; }

        public ManejadorRemplazarTraslapo(UIApplication _uiapp)
        {
            uiapp = _uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
            ListaTraslapo = new List<TraslapoDTO>();
        }

        public bool ObtenerListaTraslapoDTO()
        {
            try
            {
                var listDim = SeleccionarDimensiones.ObtenerListaInViewPorNombre(_doc, _view, "Traslapo");

                foreach (var item in listDim)
                {
                    TraslapoDTO NewTraslapoDTO = TraslapoDTO.Create(item);
                    if (NewTraslapoDTO.IsOK)
                        ListaTraslapo.Add(NewTraslapoDTO);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Error en ObtenerLista");

                return false;
            }
            return true;
        }

        public bool CrearNuevoTraslapo(int valorIdBarraBorrada, Rebar barraCreada)
        {
            try
            {
                var _dimensionType = SeleccionarDimensiones.ObtenerDimensionTYpo_Traslapo(_doc);
                if (_dimensionType == null) return false;

                 traslapoENcontrado = ListaTraslapo.Where(c => c.Id_RebarFinalRef== valorIdBarraBorrada || c.Id_RebarInicialRef == valorIdBarraBorrada)
                    .FirstOrDefault();

                if (traslapoENcontrado != null)
                {
                    if (traslapoENcontrado.Id_RebarInicialRef == valorIdBarraBorrada)
                    {
                        traslapoENcontrado.RebarInicial = barraCreada;
                        traslapoENcontrado.M1_ObtenerREfIncial(_view);
                    }
                    else if (traslapoENcontrado.Id_RebarFinalRef == valorIdBarraBorrada)
                    {
                        traslapoENcontrado.RebarFinal = barraCreada;
                    }
                    else
                        return false;
                }
                else
                    return false;
                traslapoENcontrado.M4_CrearDimension_ConTrans(_doc, _dimensionType);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista.Ex:{ex.Message}");

                return false;
            }
            return true;
        }
    }
}
