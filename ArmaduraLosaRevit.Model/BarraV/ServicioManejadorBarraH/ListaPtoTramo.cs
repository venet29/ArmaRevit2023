using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.ServicioManejadorBarraH
{
    public class ListaPtoTramo
    {
        private readonly UIApplication _uiapp;
        private readonly ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO;
        private UIDocument _uidoc;
        private XYZ _PtoInicioIntervaloBarra;
        public List<XYZ> _listaptoTramo { get; private set; }

        public ListaPtoTramo(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO)
        {
            this._uiapp = _uiapp;
            this._configuracionInicialBarraHorizontalDTO = _configuracionInicialBarraHorizontalDTO;
            this._uidoc = _uiapp.ActiveUIDocument;
        }

    

        public bool M2_ListaPtoTramo()
        {
            _listaptoTramo = new List<XYZ>();
            bool continuar = true;
            string tipoPto = "inicial ";
            int cont = 0;
            while (continuar)
            {
                try
                {

                    if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConElemento)
                    {
                        ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                        Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.PointOnElement, $"Seleccionar punto {tipoPto}barra");
                        _PtoInicioIntervaloBarra = ref_pickobject_element.GlobalPoint;

                    }
                    else if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConMouse)
                    {
                        continuar = false;
                        ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest | ObjectSnapTypes.Midpoints;
                        //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                        _PtoInicioIntervaloBarra = _uidoc.Selection.PickPoint(snapTypes, $"Seleccionar punto {tipoPto}  barra").Redondear8();
                    }

                    cont += 1;
                    tipoPto = $"siguiente {cont} ";

                    _listaptoTramo.Add(_PtoInicioIntervaloBarra);
                    continuar = true;
                }
                catch (Exception)
                {
                    continuar = false;
                }
            }
            return (_listaptoTramo.Count > 1 ? true : false);
        }
    }

   
}
