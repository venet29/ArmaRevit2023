using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.BarraV.TipoTagH.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{

    //solo para barras horizontals o inclinadas  /// no consideradas para laterales ni mallas horizonales de muro
    public class GenerarIntervalosSINNivel_RefuerzoVIga : GenerarIntervalosSINNivel, IGenerarIntervalosH
    {



        public GenerarIntervalosSINNivel_RefuerzoVIga(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO,
                                         SelecionarPtoHorizontal selecionarPtoHorizontal, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiEnfierradoDTO, selecionarPtoHorizontal, muroSeleccionadoDTO)

        {
          //  this._view = _doc.ActiveView;
        }





        public override List<IbarraBase> M2_GenerarListaBarraHorizontal()
        {
            ListaIbarraHorizontal = new List<IbarraBase>();
            try
            {
                foreach (IntervaloBarrasDTO itemIntervaloBarrasDTO in ListaIntervaloBarrasDTO)
                {
                    IGeometriaTag _newGeometriaTag = FactoryGeomTagRebarH.CrearGeometriaTagH_RefuerzoVIga(_uiapp,
                                                                                            itemIntervaloBarrasDTO.tipobarraV,
                                                                                            itemIntervaloBarrasDTO.ptoini,
                                                                                            itemIntervaloBarrasDTO.ptofinal,
                                                                                            itemIntervaloBarrasDTO.ptoPosicionTAg + AyudaGeomtria.ObtenerDesfasePOrescalaArriba(itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z, _view)+
                                                                                                                                    AyudaGeomtria.SoloRefuerZOBOrdeEscala100(_view, _confiEnfierradoDTO.TipoBarraRefuerzoViga),
                                                                                            -itemIntervaloBarrasDTO.DireccionPataEnFierrado * (itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z == 1 ? 0.8 : 0.2));



                    _newGeometriaTag.Ejecutar(new GeomeTagArgs()
                        {
                            angulorad = Util.GradosToRadianes(90),
                            DesplaminetoDirectriz_soloRefuerzoVIga = (itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z>0?new XYZ(0, 0, -1): new XYZ(0, 0, 1))
                        });

                    itemIntervaloBarrasDTO._parametrosInternoRebarDTO._NUmeroLinea_paraTagRefuerzo = _confiEnfierradoDTO.LineaBarraAnalizada;

                    IbarraBase newIbarraVertical = FactoryBarraHorizontal.GeneraraIbarraHorizontal(_uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    newIbarraVertical.IsSoloTag = _vigaSeleccionadoDTO.soloTag1;
                    // newIbarraVertical.M0_CalcularCurva();
                    ListaIbarraHorizontal.Add(newIbarraVertical);
                }
            }
            catch (Exception)
            {
                ListaIbarraHorizontal.Clear();
                return ListaIbarraHorizontal;
            }
            return ListaIbarraHorizontal;
        }


    }
}
